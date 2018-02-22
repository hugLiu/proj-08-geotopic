using Jurassic.AppCenter;
using Jurassic.AppCenter.SmartClient.Infrastructure.Interface.Services;
using Microsoft.Practices.CompositeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Jurassic.PKS.Service;
using Jurassic.So.Infrastructure;
using System.IO;
using NPOI.HSSF.UserModel;
using Jurassic.Com.DB;
using Jurassic.PKS.Service.Submission;
using System.Data;
using Jurassic.So.Business;
using Jurassic.PKS.WebAPI.Submission;
using System.Data.OleDb;
using Jurassic.AppCenter.SmartClient.Infrastructure.Interface;

namespace Jurassic.So.GeoTopic.SubmissionTool.Services
{
    /// <summary>提交服务</summary>
    internal sealed class SubmissionService
    {
        #region 构造函数
        /// <summary>构造函数</summary>
        public SubmissionService()
        {

        }
        #endregion

        #region 数据成员
        /// <summary>配置处理器</summary>
        private SubmissionConfigHandler ConfigHandler { get; set; }
        /// <summary>配置</summary>
        public SubmissionConfig Config { get; private set; }
        #endregion

        #region 访问方法
        /// <summary>初始化方法</summary>
        public void Initialize()
        {
            this.ConfigHandler = new SubmissionConfigHandler();
            this.Config = this.ConfigHandler.Load(Application.StartupPath);
            if (this.Config == null)
            {
                this.Config = new SubmissionConfig();
                this.Config.Load();
            }
            else
            {
                var loaded = false;
                if (!this.Config.MetadataTagsUrl.IsNullOrEmpty())
                {
                    loaded = LoadMetadataTagsFromUrl(this.Config.MetadataTagsUrl, true);
                }
                if (!loaded && !this.Config.MetadataTagsFile.IsNullOrEmpty())
                {
                    LoadMetadataTagsFromFile(this.Config.MetadataTagsFile);
                }
            }
        }
        /// <summary>加载元数据标签文件</summary>
        public bool LoadMetadataTagsFromUrl(string url, bool startup)
        {
            try
            {
                var service = new ApiWrappedSearchService(url);
                var tags = service.GetMetadataDefinition();
                LoadMetadataTags(tags);
                this.Config.MetadataTagsUrl = url;
                return true;
            }
            catch (Exception ex)
            {
                if (!startup) throw;
                Program.WriteLog(ex);
                return false;
            }
        }
        /// <summary>加载元数据标签文件</summary>
        public bool LoadMetadataTagsFromFile(string file)
        {
            try
            {
                var content = File.ReadAllText(file, Encoding.Default);
                var tags = content.JsonTo<MetadataDefinitionCollection>();
                LoadMetadataTags(tags);
                this.Config.MetadataTagsFile = file;
                return true;
            }
            catch (Exception ex)
            {
                Program.WriteLog(ex);
                return false;
            }
        }
        /// <summary>加载元数据标签文件</summary>
        private void LoadMetadataTags(MetadataDefinitionCollection tags)
        {
            foreach (var tag in tags)
            {
                if (tag.InnerTag) continue;
                var config = this.Config.MetadataTags.FirstOrDefault(e => e.Name == tag.Name);
                if (config != null)
                {
                    config.Refer = tag;
                    continue;
                }
                config = new MetadataTagConfig();
                config.Name = tag.Name;
                config.Refer = tag;
                config.Enabled = true;
                config.Width = 100;
                if (tag.Type == TagType.DateString) config.Width = 140;
                if (tag.Items.IsNullOrEmpty())
                {
                    config.EnumValues = string.Empty;
                }
                else
                {
                    var enumValues = tag.Items.Select(e => e.Text).ToList();
                    config.EnumValues = string.Join(",", enumValues);
                }
                this.Config.MetadataTags.Add(config);
            }
            for (int i = 0; i < this.Config.MetadataTags.Count;)
            {
                if (this.Config.MetadataTags[i].Refer == null)
                {
                    this.Config.MetadataTags.RemoveAt(i);
                    continue;
                }
                i++;
            }
        }
        /// <summary>保存配置方法</summary>
        public void Save()
        {
            this.ConfigHandler.Save(Application.StartupPath, this.Config);
        }
        /// <summary>生成Excel模板文件</summary>
        public void BuildExcelTemplate(string[] ptFiles)
        {
            using (var stream = new FileStream(this.Config.ExcelFile, FileMode.Create, FileAccess.Write))
            {
                var excelWorkBook = new HSSFWorkbook();
                var excelSheet = excelWorkBook.CreateSheet("成果元数据");
                var rowIndex = 0;
                var cellData = new List<CellProperty>();
                //加入第一行标题
                cellData.Add(new CellProperty() { Value = "序号", Span = 1, Width = 60 });
                cellData.Add(new CellProperty() { Value = "成果路径", Span = 1, Width = 160 });
                cellData.Add(new CellProperty() { Value = "成果文件", Span = 1, Width = 140 });
                var tags = this.Config.MetadataTags.Where(e => e.Enabled).ToArray();
                if (tags.Length > 0)
                {
                    cellData.Add(new CellProperty() { Value = "元数据标签", Span = tags.Length, Widths = tags.Select(e => e.Width).ToArray() });
                }
                var options = this.Config.MetadataOptions;
                if (options.Count > 0)
                {
                    cellData.Add(new CellProperty() { Value = "扩展选项", Span = options.Count, Widths = options.Select(e => e.Width).ToArray() });
                }
                excelSheet.AddRow(rowIndex++, cellData, true);
                cellData.Clear();
                //加入第二行标题
                cellData.Add(new CellProperty() { Value = "", Span = 1 });
                cellData.Add(new CellProperty() { Value = "", Span = 1 });
                cellData.Add(new CellProperty() { Value = "", Span = 1 });
                foreach (var tag in tags)
                {
                    cellData.Add(new CellProperty() { Value = tag.Name, Span = 1 });
                }
                foreach (var option in options)
                {
                    cellData.Add(new CellProperty() { Value = option.Name, Span = 1 });
                }
                excelSheet.AddRow(rowIndex++, cellData, false);
                cellData.Clear();
                //加入第三行标题
                cellData.Add(new CellProperty() { Value = "", Span = 1 });
                cellData.Add(new CellProperty() { Value = "", Span = 1 });
                cellData.Add(new CellProperty() { Value = "", Span = 1 });
                foreach (var tag in tags)
                {
                    cellData.Add(new CellProperty() { Value = tag.Title, Span = 1 });
                }
                foreach (var option in options)
                {
                    cellData.Add(new CellProperty() { Value = "", Span = 1 });
                }
                excelSheet.AddRow(rowIndex++, cellData, false);
                cellData.Clear();
                //生成合并单元格
                excelSheet.BuildMergedRegion(0, 2, 0, 0);
                excelSheet.BuildMergedRegion(0, 2, 1, 1);
                excelSheet.BuildMergedRegion(0, 2, 2, 2);
                var firstCol = 3 + tags.Length;
                for (int i = 0; i < options.Count; i++)
                {
                    excelSheet.BuildMergedRegion(1, 2, firstCol, firstCol);
                    firstCol++;
                }
                excelSheet.SetTitleStyle(0, 2);
                //加入成果文件
                for (int i = 0; i < ptFiles.Length; i++)
                {
                    cellData.Add(new CellProperty() { Value = i + 1, Span = 1 });
                    var ptFile = ptFiles[i];
                    cellData.Add(new CellProperty() { Value = Path.GetDirectoryName(ptFile), Span = 1 });
                    cellData.Add(new CellProperty() { Value = Path.GetFileName(ptFile), Span = 1 });
                    excelSheet.AddRow(rowIndex++, cellData, false);
                    cellData.Clear();
                }
                excelSheet.SetNumberStyle(3, 0);
                excelSheet.CreateFreezePane(3, 3);
                //设置列下拉列表
                var startRow = 3;
                var startColumn = 3;
                for (int i = 0; i < tags.Length; i++)
                {
                    var tag = tags[i];
                    if (!tag.EnumValues.IsNullOrEmpty())
                    {
                        excelSheet.SetColumnDropdownList(tag.GetEnumValues(), startRow, startColumn + i);
                    }
                    if (tag.Refer.Type == TagType.DateString)
                    {
                        excelSheet.SetDateTimeStyle(startRow, startColumn + i);
                    }
                }
                //保存
                excelWorkBook.Write(stream);
            }
        }
        /// <summary>从Excel文件加载成果集合</summary>
        public void LoadProducts(SubmitContext context, string excelFile)
        {
            var excelSheets = GetExcelSheets(excelFile);
            if (excelSheets.IsNullOrEmpty())
            {
                throw new JException("Excel文件中不存在表单Sheet！");
            }
            var excelSheet = excelSheets[0];
            var excelTable = ExcelDB.GetExcelTable(excelFile, excelSheet);
            if (excelTable == null || excelTable.Rows.Count <= 2)
            {
                throw new JException($"Excel表单[{excelSheet}]中不存在数据！");
            }
            //加载元数据标签
            var tags = this.Config.MetadataTags.Where(e => e.Enabled).ToArray();
            var kmdConfig = new KMDConfiguration(tags.Select(e => e.Refer));
            JsonMetadata.InitMetadataDefinitions(kmdConfig);
            //检查Excel文件
            var totalColumn = 3 + tags.Length + this.Config.MetadataOptions.Count;
            if (totalColumn != excelTable.Columns.Count)
            {
                throw new JException($"Excel表单列数 [{excelTable.Columns.Count.ToString()}]与配置中列数[{totalColumn.ToString()}]不一致！");
            }
            //删除前两行标题行
            excelTable.Rows.RemoveAt(0);
            excelTable.Rows.RemoveAt(0);
            //加入排序列
            var rowIndexColumn = new DataColumn("RowIndex", typeof(int));
            rowIndexColumn.DefaultValue = 0;
            excelTable.Columns.Add(rowIndexColumn);
            var rowIDColumn = new DataColumn("RowID", typeof(int));
            rowIDColumn.DefaultValue = 0;
            excelTable.Columns.Add(rowIDColumn);
            var total = excelTable.Rows.Count;
            var orderColumn = excelTable.Columns[0];
            var ptPathColumn = excelTable.Columns[1];
            var ptFileColumn = excelTable.Columns[2];
            for (int i = 0; i < total; i++)
            {
                var row = excelTable.Rows[i];
                row[rowIndexColumn.Ordinal] = i + 2;
                row[rowIDColumn.Ordinal] = int.Parse(row[orderColumn.Ordinal].ToString());
            }
            //获得排序视图
            var excelRows = excelTable.Select("", "RowID, RowIndex");
            total = excelRows.Length;
            var progressProperty = context.Progress;
            var worker = context.Worker;
            progressProperty.Message = "正在生成成果数据...";
            progressProperty.MaxValue = total;
            worker.ReportProgress(-1, progressProperty.Clone());
            progressProperty.Message = "";
            SubmitProduct prevProduct = null;
            for (int i = 0; i < total; i++)
            {
                var row = excelRows[i];
                var id = (int)row[rowIDColumn.Ordinal];
                SubmitProduct product = null;
                if (prevProduct == null || prevProduct.ID != id)
                {
                    product = new SubmitProduct();
                    product.ID = id;
                    product.Files = new List<SubmitProductFile>();
                }
                else
                {
                    product = prevProduct;
                }
                var productFile = new SubmitProductFile();
                productFile.Path = row[ptPathColumn.Ordinal].ToString();
                productFile.File = row[ptFileColumn.Ordinal].ToString();
                product.Files.Add(productFile);
                if (product != prevProduct)
                {
                    var productInfo = BuildProductInfo();
                    var startColumn = 3;
                    productInfo.KMD = BuildProductKmd(row, tags, rowIndexColumn, startColumn);
                    var configOption = this.Config.SubmissionOption;
                    var option = BuildProductOption();
                    startColumn += tags.Length;
                    BuildProductOptionValues(row, option, startColumn);
                    productInfo.Option = option;
                    product.Info = productInfo;
                }
                context.Values.Enqueue(product);
                prevProduct = product;
                worker.ReportProgress(i, progressProperty.Clone());
            }
        }
        /// <summary>生成成果信息</summary>
        private SubmissionInfoRequest BuildProductInfo()
        {
            var productInfo = new SubmissionInfoRequest();
            productInfo.Action = SubmissionAction.Create;
            return productInfo;
        }
        /// <summary>生成成果元数据</summary>
        private JsonMetadata BuildProductKmd(DataRow row, MetadataTagConfig[] tags, DataColumn rowIndexColumn, int startColumn)
        {
            var kmd = new JsonMetadata();
            for (int i = 0; i < tags.Length; i++)
            {
                var tag = tags[i];
                var tagValue = row[startColumn + i];
                if (tagValue == null) continue;
                var tagValue2 = tagValue.ToString();
                if (tagValue2.IsNullOrEmpty()) continue;
                object tagValue3 = tagValue2;
                switch (tag.Refer.Type)
                {
                    case TagType.DateString:
                        var dtTagValue = tagValue2.TryParseStandardString();
                        if (!dtTagValue.HasValue)
                        {
                            throw new JException($"第{row[rowIndexColumn.Ordinal]}行第{startColumn + i}列[{tag.Title}]的时间值{tagValue2}转换失败！");
                        }
                        tagValue3 = dtTagValue.Value;
                        break;
                    case TagType.StringArray:
                        tagValue3 = tagValue2.Split(',');
                        break;
                }
                kmd.SetValue(tag.Name, tagValue3);
            }
            return kmd;
        }
        /// <summary>生成成果选项</summary>
        private SubmissionOption BuildProductOption()
        {
            var configOption = this.Config.SubmissionOption;
            var option = new SubmissionOption();
            option.Application = configOption.Application;
            option.Authentic = configOption.Authentic;
            option.AutoComplement = configOption.AutoComplement;
            option.Contact = configOption.Contact;
            option.Task = configOption.Task;
            option.UploadedBy = configOption.UploadedBy;
            option.Values = new Dictionary<string, object>();
            return option;
        }
        /// <summary>生成成果扩展选项</summary>
        private void BuildProductOptionValues(DataRow row, SubmissionOption option, int startColumn)
        {
            var metadataOptions = this.Config.MetadataOptions;
            var values = option.Values;
            for (int i = 0; i < metadataOptions.Count; i++)
            {
                var metadataOption = metadataOptions[i];
                var tagValue = row[startColumn + i];
                if (tagValue == null) continue;
                var tagValue2 = tagValue.ToString();
                if (tagValue2.IsNullOrEmpty()) continue;
                values.Add(metadataOption.Name, tagValue2);
            }
        }
        /// <summary>上传产品</summary>
        public void UploadProduct(SubmitContext context, SubmitProduct product)
        {
            var progressProperty = context.Progress.Clone();
            var files = string.Join(",", product.Files.Select(e => e.File).ToArray());
            progressProperty.Content = $"成果[{files}]";
            try
            {
                context.View.RefreshProductStatus(product, "正在提交");
                var apiService = context.ApiSubmissionService;
                product.Info.FileIDs = new List<string>();
                foreach (var productFile in product.Files)
                {
                    var file = Path.Combine(productFile.Path, productFile.File);
                    var fileID = apiService.Upload(file);
                    product.Info.FileIDs.Add(fileID);
                }
                apiService.Submit(product.Info);
                progressProperty.Content += "提交成功！";
                context.View.RefreshProductStatus(product, "提交成功");
            }
            catch (Exception ex)
            {
                progressProperty.Content += "提交失败！";
                context.FailureValues.Enqueue(product);
                context.View.RefreshProductStatus(product, "提交失败");
                context.WorkItem.Error(progressProperty.Content + ex.Message);
                Program.WriteLog(ex);
            }
            context.Worker.ReportProgress(context.GetNextProgressValue(), progressProperty);
        }
        /// <summary>获得Excel表单数组</summary>
        private string[] GetExcelSheets(string excelFile)
        {
            var connectionString = $"Provider=Microsoft.Ace.OleDb.12.0;Data Source={excelFile};Extended Properties='Excel 12.0; HDR=YES; IMEX=1'";
            using (var connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                var dataTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                return dataTable.Rows.Cast<DataRow>().Select(e => e["Table_Name"].ToString()).ToArray();
            }
        }
        #endregion
    }
}
