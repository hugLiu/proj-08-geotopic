using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Jurassic.So.GeoTopic.Database.Models;
using Jurassic.So.GeoTopic.Database.Service;
using Jurassic.So.GeoTopic.DataService.Enum;
using Jurassic.So.GeoTopic.DataService.Models;
using Jurassic.So.GeoTopic.DataService.Tool;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace Jurassic.So.GeoTopic.DataService
{
    //读取Excel文件信息
    public class ImportParamsService: IImportParamsService
    {

        public IGT_IndexDefinitionEFRepository gt_IndexDefinition { get; set; }

        public IGT_RenderUrlEFRepository gt_RenderUrl { get; set; }

        public IGT_RenderTypeEFRepository gt_renderType { get; set; }

        public IGT_TopicCardEFRepository gt_TopicCard { get; set; }

        public IGT_TopicIndexEFRepository gt_TopicIndex { get; set; }

        //保存样式导出单元格样式
        private ICellStyle _cellStyle=null;
        private   List<GT_RenderUrl> _renderUrlList=new List<GT_RenderUrl>();

        public ImportParamsService(
            IGT_IndexDefinitionEFRepository GT_IndexDefinition, 
            IGT_RenderUrlEFRepository GT_RenderUrl, 
            IGT_RenderTypeEFRepository GT_renderType, 
            IGT_TopicCardEFRepository GT_TopicCard,
            IGT_TopicIndexEFRepository GT_TopicIndex)
        {
            gt_IndexDefinition = GT_IndexDefinition;
            gt_RenderUrl = GT_RenderUrl;
            gt_renderType = GT_renderType;
            gt_TopicCard = GT_TopicCard;
            gt_TopicIndex = GT_TopicIndex;
        }



        //保存样式导出单元格样式
        private void setCellStyle(IWorkbook wk)
        {
            ICellStyle style = wk.CreateCellStyle();
            IFont font = wk.CreateFont();
            font.FontName = "微软雅黑";
            style.SetFont(font);

            style.VerticalAlignment=VerticalAlignment.Center;
            style.Alignment = HorizontalAlignment.Left;

            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            _cellStyle = style;
        }

        //设置具体单元格样式
        private void SetCell(ICell cell,string cellValue)
        {
            cell.SetCellValue(cellValue);
            cell.CellStyle = _cellStyle;
        }



        //获取当前工作文件
        public IWorkbook GetWorkBook(FileStream fileStream,string filePath)
        {
            IWorkbook wk = null;
            if(filePath.IndexOf(".xlsx")>0)         //获取07及以上版本数据
                wk=new XSSFWorkbook(fileStream);
            else
            if (filePath.IndexOf(".xls")>0)          //获取03版本
                wk=new HSSFWorkbook(fileStream);
            return wk;
        }

        public List<ExcelEntityModel> ReadDataFromImportExcel(string filePath)
        {
            List<ExcelEntityModel> excelEntityModels=new List<ExcelEntityModel>();
            using (FileStream fs = File.OpenRead(filePath))
             {
                IWorkbook wk = GetWorkBook(fs, filePath);
                //读取当前表第一张表
                //ISheet sheet = wk.GetSheetAt(0);
                 //try
                 //{
                    ISheet sheet = wk.GetSheet(EnumSheetName.knowledgeTemp);
                    List<RowEntityModel> RowEntityModels = new List<RowEntityModel>();     //存放rowList数据
                    for (int j = 2; j <= sheet.LastRowNum; j++)  //LastRowNum 是当前表的总行数
                    {
                        IRow row = sheet.GetRow(j);  //读取当前行数据   读取表格数据
                        if (row != null)
                        {
                            RowEntityModel rowEntity = new RowEntityModel();
                            RowInfoMappingToRowEntity1(rowEntity, row);
                            //Ecxelrow数据转化成RowEntityModels集合
                            RowEntityModels.Add(rowEntity);
                        }
                    }
                    excelEntityModels = RowEntitysMappingToExcelEntity(RowEntityModels);
                //}
                //catch (Exception ex)
                // {

                // }
            }

            //保存参数IndexDefinition数据，RenderUrl数据   暂时不写入数据库
            //UpdateIndexDefinition(filePath);
            //UpdateRenderUrl(filePath);
            return excelEntityModels;
        }

        //导出模板
        public void ExportExcelTemplate(string filePath)
        {
            CreateModelExcel(filePath);
            WriteDataToIndexDefinition(filePath);
            WriteDataToRenderUrl(filePath);

            //将获取的展示页面Url写到第一个模板页，   改进
            RenderUrl(filePath);
            WriteDataToMataDataParam(filePath);   //获得元数据集合
        }


        //导出知识链数据到Excel模板
        public void ExportDataToExcel(string filePath,List<KTopicModel> kTopicModelList)
        {
            //创建了一个模板
            ExportExcelTemplate(filePath);
            WriteDataToModel(filePath, kTopicModelList);
        }


        //创建知识模型参数
        public void CreateModelExcel(string filePath)
        {
            #region   NPOI方式创建知识模板
            //IWorkbook wk = null;
            //using (FileStream file = new FileStream(filePath, FileMode.OpenOrCreate))
            //{
            //    if (filePath.IndexOf(".xlsx") > 0)         //获取07及以上版本数据
            //        wk = new XSSFWorkbook();
            //    else
            //    if (filePath.IndexOf(".xls") > 0)          //获取03版本
            //        wk = new HSSFWorkbook();

            //    wk.CreateSheet(EnumSheetName.knowledgeTemp);
            //    wk.CreateSheet(EnumSheetName.indexDefinition);
            //    wk.CreateSheet(EnumSheetName.renderUrl);
            //    wk.CreateSheet(EnumSheetName.metadataParam);

            //    #region    设置知识模板表头
            //    //设置字体
            //    IFont font = wk.CreateFont();
            //    font.FontName = "微软雅黑";
            //    font.FontHeightInPoints = 11;
            //    font.Boldweight = (short)FontBoldWeight.Bold;
            //    ICellStyle style = wk.CreateCellStyle();
            //    style.SetFont(font);
            //    //设置居中
            //    style.VerticalAlignment = VerticalAlignment.Center;
            //    style.Alignment = HorizontalAlignment.Center;

            //    //设置背景图案
            //    style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Gold.Index;

            //    style.FillPattern = FillPattern.SolidForeground;
            //    style.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Pink.Index;

            //    //设置知识模板表头标题边框
            //    //style.BorderBottom = BorderStyle.Thin;
            //    //style.BorderLeft = BorderStyle.Thin;
            //    //style.BorderRight = BorderStyle.Thin;
            //    //style.BorderTop = BorderStyle.Thin;
            //    #endregion

            //    #region    设置知识模板表头
            //    ISheet sheetModel = wk.GetSheet(EnumSheetName.knowledgeTemp);
            //    IRow topRow = sheetModel.CreateRow(0);
            //    IRow secondRow = sheetModel.CreateRow(1);

            //    topRow.Height = 15 * 20;
            //    secondRow.Height = 15 * 20;
            //    sheetModel.DefaultRowHeight = 20 * 20;
            //    sheetModel.SetColumnWidth(0, 25 * 256);
            //    sheetModel.SetColumnWidth(1, 25 * 256);
            //    sheetModel.SetColumnWidth(2, 5 * 256);
            //    sheetModel.SetColumnWidth(3, 20 * 256);
            //    sheetModel.SetColumnWidth(4, 25 * 256);
            //    sheetModel.SetColumnWidth(5, 25 * 256);
            //    sheetModel.SetColumnWidth(6, 25 * 256);

            //    ICell topicCell = topRow.CreateCell(0);
            //    topicCell.SetCellValue("主题");
            //    sheetModel.AddMergedRegion(new CellRangeAddress(0, 1, 0, 0));
            //    topicCell.CellStyle = style;

            //    ICell indexDefCell = topRow.CreateCell(1);
            //    indexDefCell.SetCellValue("主题属性");
            //    sheetModel.AddMergedRegion(new CellRangeAddress(0, 1, 1, 1));
            //    indexDefCell.CellStyle = style;

            //    ICell tCardCell = topRow.CreateCell(2);
            //    tCardCell.SetCellValue("基本知识卡定义");
            //    tCardCell.CellStyle = style;
            //    sheetModel.AddMergedRegion(new CellRangeAddress(0, 0, 2, 6));

            //    ICell flagCell = secondRow.CreateCell(2);
            //    flagCell.SetCellValue("ID");
            //    flagCell.CellStyle = style;
            //    ICell ptListCell = secondRow.CreateCell(3);
            //    ptListCell.SetCellValue("成果标题");
            //    ptListCell.CellStyle = style;
            //    ICell paramCell = secondRow.CreateCell(4);
            //    paramCell.SetCellValue("参数");
            //    paramCell.CellStyle = style;
            //    ICell renderUrlCell = secondRow.CreateCell(5);
            //    renderUrlCell.SetCellValue("渲染页面");
            //    renderUrlCell.CellStyle = style;
            //    ICell layoutCell = secondRow.CreateCell(6);
            //    layoutCell.SetCellValue("知识卡布局");
            //    layoutCell.CellStyle = style;
            //    #endregion

            //    #region    设置主题属性表头
            //    ISheet sheetIndexDef = wk.GetSheet(EnumSheetName.indexDefinition);
            //    IRow topRowIndex = sheetIndexDef.CreateRow(0);

            //    topRowIndex.Height = 15 * 20;
            //    sheetIndexDef.DefaultRowHeight = 20 * 20;
            //    sheetIndexDef.SetColumnWidth(0, 20 * 256);
            //    sheetIndexDef.SetColumnWidth(1, 20 * 256);

            //    ICell indexCodeCell = topRowIndex.CreateCell(0);
            //    indexCodeCell.SetCellValue("主题属性（Code）");
            //    indexCodeCell.CellStyle = style;
            //    ICell indexDescriptionCell = topRowIndex.CreateCell(1);
            //    indexDescriptionCell.SetCellValue("描述");
            //    indexDescriptionCell.CellStyle = style;
            //    #endregion

            //    #region    设置展示页面表头
            //    ISheet sheetRenderUrl = wk.GetSheet(EnumSheetName.renderUrl);
            //    IRow topRowRenderUrl = sheetRenderUrl.CreateRow(0);

            //    topRowRenderUrl.Height = 15 * 20;
            //    sheetRenderUrl.DefaultRowHeight = 20 * 20;
            //    sheetRenderUrl.SetColumnWidth(0, 20 * 256);
            //    sheetRenderUrl.SetColumnWidth(1, 20 * 256);
            //    sheetRenderUrl.SetColumnWidth(2, 20 * 256);

            //    ICell typeCell = topRowRenderUrl.CreateCell(0);
            //    typeCell.SetCellValue("类型");
            //    typeCell.CellStyle = style;
            //    ICell urlDescriptionCell = topRowRenderUrl.CreateCell(1);
            //    urlDescriptionCell.SetCellValue("页面");
            //    urlDescriptionCell.CellStyle = style;
            //    ICell presentUrlCell = topRowRenderUrl.CreateCell(2);
            //    presentUrlCell.SetCellValue("展示URL");
            //    presentUrlCell.CellStyle = style;
            //    #endregion

            //    #region    设置参数表头
            //    ISheet sheetParam = wk.GetSheet(EnumSheetName.metadataParam);
            //    IRow topRowMetadata = sheetParam.CreateRow(0);

            //    topRowMetadata.Height = 15 * 20;
            //    sheetParam.DefaultRowHeight = 20 * 20;
            //    sheetParam.SetColumnWidth(0, 20 * 256);
            //    sheetParam.SetColumnWidth(1, 20 * 256);

            //    ICell MetadataCell = topRowMetadata.CreateCell(0);
            //    MetadataCell.SetCellValue("参数(元数据标签)");
            //    MetadataCell.CellStyle = style;
            //    ICell descriptionCell = topRowMetadata.CreateCell(1);
            //    descriptionCell.SetCellValue("描述");
            //    descriptionCell.CellStyle = style;

            //    #endregion
            //    wk.Write(file);
            //}
            #endregion

            string tempFilePath = HttpContext.Current.Request.PhysicalApplicationPath + EnumSheetName.modelFolder;
            string modelFilePath = tempFilePath + "\\" + EnumSheetName.newFileName;
            if (!Directory.Exists(modelFilePath))
            {
                Directory.CreateDirectory(tempFilePath);
            }
            File.Copy(modelFilePath, filePath, true);
        }


        public void RowInfoMappingToRowEntity1(RowEntityModel rowEntity,IRow CurrentRow)
        {
            rowEntity.TopicPathTitle = CurrentRow.GetCell(0).StringCellValue;
            rowEntity.IndexDefinitionInfo = CurrentRow.GetCell(1)==null ? string.Empty:CurrentRow.GetCell(1).StringCellValue;
            rowEntity.flagId = CurrentRow.GetCell(2)==null ? string.Empty:CurrentRow.GetCell(2).StringCellValue;
            rowEntity.PtTitle = CurrentRow.GetCell(3) == null ? string.Empty:CurrentRow.GetCell(3).StringCellValue;
            rowEntity.Params = CurrentRow.GetCell(4) == null? string.Empty : CurrentRow.GetCell(4).StringCellValue;
            rowEntity.Url = CurrentRow.GetCell(5)==null? string.Empty: CurrentRow.GetCell(5).StringCellValue;
            rowEntity.Layout = CurrentRow.GetCell(6)==null? string.Empty:CurrentRow.GetCell(6).StringCellValue;
            rowEntity.IsPtList =string.Empty;
            rowEntity.IsRemark = string.Empty;
        }

        public List<ExcelEntityModel> RowEntitysMappingToExcelEntity(List<RowEntityModel> RowEntityModels)
        {
            List < ExcelEntityModel > excelEntityModels=new List<ExcelEntityModel>();
            foreach (var rowmodel in RowEntityModels)
            {
                if (!string.IsNullOrEmpty(rowmodel.TopicPathTitle))
                {
                    ExcelEntityModel excelEntity = new ExcelEntityModel()
                    {
                        TopicPathTitle = rowmodel.TopicPathTitle,
                        IndexDefinitionInfo = rowmodel.IndexDefinitionInfo,
                        Layout = rowmodel.Layout,
                        IsPtList = BoolConvert(rowmodel.IsPtList),
                        IsRemark = BoolConvert(rowmodel.IsRemark)
                    };
                    PtCellInfo ptCellInfo = new PtCellInfo()
                    {
                        falgId = rowmodel.flagId,
                        PtTitle = rowmodel.PtTitle,
                        Params = rowmodel.Params,
                        Url = rowmodel.Url
                    };
                    excelEntity.PtCellInfos.Add(ptCellInfo);
                    excelEntityModels.Add(excelEntity);
                }
                else
                {
                    PtCellInfo ptCellInfo = new PtCellInfo()
                    {
                        falgId = rowmodel.flagId,
                        PtTitle = rowmodel.PtTitle,
                        Params = rowmodel.Params,
                        Url = rowmodel.Url
                    };
                    excelEntityModels[excelEntityModels.Count - 1].PtCellInfos.Add(ptCellInfo);
                }
            }

            return excelEntityModels;
        }
        public bool BoolConvert(string boolStr)
        {
            if (boolStr.Trim() == "是")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //读取主题属性Sheet页数据
        public List<Dic_tIndexModel> ReadDataIndexDefinition(string filePath)
        {
            List<Dic_tIndexModel> indexDefList = new List<Dic_tIndexModel>();
            using (FileStream fs = File.OpenRead(filePath))
            {
                //var wk = new XSSFWorkbook(fs);
                IWorkbook wk = GetWorkBook(fs, filePath);


                ISheet IndexDefSheet = wk.GetSheetAt(1);
                //ISheet IndexDefSheet = wk.GetSheet(EnumSheetName.indexDefinition);

                for (int i = 1; i < IndexDefSheet.LastRowNum; i++)
                {
                    IRow currentRow = IndexDefSheet.GetRow(i);

                    if (!string.IsNullOrEmpty(currentRow.GetCell(0).StringCellValue))
                    {
                        var indexDef = new Dic_tIndexModel()
                        {
                            Code = currentRow.GetCell(0).StringCellValue,
                            Title = currentRow.GetCell(1).StringCellValue
                        };

                        indexDefList.Add(indexDef);

                    }
                }
            }
            return indexDefList;
        }

        //读取展示页面Sheet页数据
        public List<Dic_tUrlModel> ReadDataRenderUrl(string filePath)
        {
            List<Dic_tUrlModel> tUrlList = new List<Dic_tUrlModel>();
            using (FileStream fs = File.OpenRead(filePath))
            {
                IWorkbook wk = GetWorkBook(fs, filePath);

                ISheet IndexDefSheet = wk.GetSheetAt(2);
                //ISheet IndexDefSheet = wk.GetSheet(EnumSheetName.renderUrl);
                //遍历sheet行数据
                for (int i = 1; i < IndexDefSheet.LastRowNum; i++)
                {
                    IRow currentRow = IndexDefSheet.GetRow(i);

                    if (!string.IsNullOrEmpty(currentRow.GetCell(0).StringCellValue) )
                    {
                        var indexDef = new Dic_tUrlModel()
                        {
                            RenderTypeTitle = currentRow.GetCell(0).StringCellValue,
                            Description = currentRow.GetCell(1).StringCellValue,
                            Url = currentRow.GetCell(2).StringCellValue
                        };
                        tUrlList.Add(indexDef);

                    }

                }
            }
            return tUrlList;
        }

        //写入数据到知识模板页
        public void WriteDataToModel(string filePath,List<KTopicModel> kTopicModelList)
        {
            IWorkbook wk = null;
            using (FileStream fs = File.OpenRead(filePath))
            {
                wk = GetWorkBook(fs, filePath);

                //设置单元格样式
                setCellStyle(wk);

                ISheet sheet = wk.GetSheet(EnumSheetName.knowledgeTemp);
                int rowIndex = 2;
                foreach (var kTopicMode in kTopicModelList)
                {
                    //获取indexDefinition  的Code Value值   合并单元格
                    var indexDefinitionStr=GetIndexDefinition(kTopicMode.Id);
                    //获取知识卡
                    var curentTCards=gt_TopicCard.FindList(t => t.TopicId == kTopicMode.Id, t => t.OrderId, 1);
                    //当存在知识主题存在知识卡   即导出该节点数据
                    IRow newRow = null;
                    if (curentTCards != null && curentTCards.Count() > 0)
                    {
                        //int firstRowIndex = rowIndex;
                        //保存知识卡信息
                        foreach (var tcard in curentTCards)
                        {
                            int firstRowIndex = rowIndex;

                            //获取到保存参数节点
                            var paramDefinition = tcard.Definition;
                            if ( !string.IsNullOrEmpty(paramDefinition))
                            {
                                var cardDefModel =
                                    JsonUtil.JsonToObject(paramDefinition, typeof (CardDefModel)) as CardDefModel;
                                if (cardDefModel != null && cardDefModel.cells.Count>0)
                                {
                                    foreach (var cellInfo in cardDefModel.cells)
                                    {
                                        try
                                        {
                                            newRow = sheet.CreateRow(rowIndex);
                                        }
                                        catch (Exception)
                                        {
                                            newRow = sheet.GetRow(rowIndex);
                                        }

                                        SetCell(newRow.CreateCell(2), cellInfo.id);
                                        SetCell(newRow.CreateCell(3), cellInfo.title);
                                        SetCell(newRow.CreateCell(4), GetParams(cellInfo.param)); //参数需要解析
                                        SetCell(newRow.CreateCell(5), cellInfo.url);

                                        rowIndex++;
                                    }

                                    SetCell(sheet.GetRow(firstRowIndex).CreateCell(6), JsonUtil.ObjectToJson(cardDefModel.layout));

                                    //--------------------
                                    //kTopicMode.fullPath  和斌单元个赋值
                                    SetCell(sheet.GetRow(firstRowIndex).CreateCell(0), kTopicMode.fullPath);
                                    SetCell(sheet.GetRow(firstRowIndex).CreateCell(1), indexDefinitionStr);

                                    if (rowIndex - 1 > firstRowIndex)
                                    {
                                        //合并单元格加边框 (第1列 第2列 第7列)
                                        int[] mergedCols = { 0, 1, 6 };
                                        foreach (var mergedCol in mergedCols)
                                        {
                                            sheet.AddMergedRegion(new CellRangeAddress(firstRowIndex, rowIndex - 1, mergedCol, mergedCol));
                                            for (int i = firstRowIndex; i <= rowIndex - 1; i++)
                                            {
                                                ICell singleCell = sheet.GetRow(i).GetCell(mergedCol);
                                                if (singleCell == null)
                                                    singleCell = sheet.GetRow(i).CreateCell(mergedCol);
                                                singleCell.CellStyle = _cellStyle;
                                            }
                                        }
                                    }
                                    //-------------------------
                                }
                            }
                            //当存在知识卡，但没有定义只是卡的内容信息,也应该生成一行记录
                            else
                            {
                                newRow = sheet.CreateRow(rowIndex);
                                SetCell(newRow.CreateCell(0), kTopicMode.fullPath);
                                SetCell(newRow.CreateCell(1), indexDefinitionStr);
                                rowIndex++;
                            }
                        }
                    }
                    else   //当相应的主题不存在知识卡
                    {
                        newRow=sheet.CreateRow(rowIndex);

                        SetCell(newRow.CreateCell(0), kTopicMode.fullPath);
                        SetCell(newRow.CreateCell(1), indexDefinitionStr);

                        rowIndex++;
                    }
                }
            }
            WriteToFile(wk, filePath);
        }

        //获取indexDefinition的Code=Value|code1=value1形式数据
        public string GetIndexDefinition( int topicId)
        {
            var indexDefinitionStr = string.Empty;
            //获取主题索引信息   合成字符串   indexDefinition....Code Value
            var topicIndexList = gt_TopicIndex.FindList(t => t.TopicId == topicId, t => t.Id, 1);
            //保存indexdefinition  code--value
            if (topicIndexList!=null&&topicIndexList.Count() > 0)
            {
                StringBuilder sb=new StringBuilder();
                foreach (var topicIndex in topicIndexList)
                {
                    var code = gt_IndexDefinition.Find(t => t.Id == topicIndex.IndexDefinitionId).Code;
                    var value = topicIndex.Value;
                    sb.Append(code + "=" + value+"|");
                }
                indexDefinitionStr = sb.Remove(sb.Length - 1, 1).ToString();
            }
            return indexDefinitionStr;
        }


        //解析param字段 --->  ep.pt="成果1" and ep.bo="@ep.bo"
        public string GetParams(string JsonStr)
        {

            string paramStr = string.Empty;

            if (string.IsNullOrEmpty(JsonStr))
                return paramStr;

            string filterType = string.Empty;
            if (JsonStr.IndexOf("and") > 0)
            {
                filterType = "$and";
            }
            else if (JsonStr.IndexOf("or") > 0)
            {
                filterType = "$or";
            }
            else if (JsonStr.IndexOf("in") > 0)
            {
                filterType = "$in";
            }
            try
            {
                //将Josn字符串转化成动态对象
                var DynamicObject = JsonConvert.DeserializeObject<dynamic>(JsonStr);

                var paramCount = DynamicObject.filter[filterType].Count;
                var sb = new StringBuilder();
                //此处模拟在不建实体类的情况下，反转将json返回回dynamic对象
                for (int i = 0; i < paramCount; i++)
                {
                    var key = ((Newtonsoft.Json.Linq.JProperty)((Newtonsoft.Json.Linq.JContainer)(DynamicObject.filter[filterType][i])).First).Name;
                    var value = (((Newtonsoft.Json.Linq.JProperty)((Newtonsoft.Json.Linq.JContainer)(DynamicObject.filter[filterType][i])).First)).Value;
                    sb.AppendFormat("{0}={1} {2} ", key, value, filterType.Substring(1));
                }
                string str = sb.ToString();
                paramStr = str.Remove(str.LastIndexOf(filterType.Substring(1)));
            }
            catch (Exception)
            {
                //如果不符合参数模式   则导出数据的参数数据为空
                paramStr = JsonStr;
            }
            return paramStr;
        }




        //写入数据到主题属性sheet
        public void WriteDataToIndexDefinition(string filePath)
        {
            List<GT_IndexDefinition> indexDefList = gt_IndexDefinition.GetAll();
            IWorkbook wk = null;
            using (FileStream fs = File.OpenRead(filePath))
            {
                wk = GetWorkBook(fs, filePath);

                //设置单元格样式
                setCellStyle(wk);

                ISheet sheet = wk.GetSheet(EnumSheetName.indexDefinition);
                int rowCount = 1;
                foreach (var indexDef in indexDefList)
                {
                    IRow newRow = null;
                    try
                    {
                        newRow = sheet.CreateRow(rowCount);
                    }
                    catch (Exception)
                    {
                        newRow = sheet.GetRow(rowCount);
                    }

                    SetCell(newRow.CreateCell(0), indexDef.Code);
                    SetCell(newRow.CreateCell(1), indexDef.Title);

                    rowCount++;
                }
            }

            WriteToFile(wk, filePath);
        }


        //写入数据到展示页面sheet
        public void WriteDataToRenderUrl(string filePath)
        {
            List<GT_RenderUrl> renderUrlList = gt_RenderUrl.GetAll();

            _renderUrlList = renderUrlList;
            IWorkbook wk = null;
            using (FileStream fs = File.OpenRead(filePath))
            {
                wk = GetWorkBook(fs, filePath);
                ISheet sheet = wk.GetSheet(EnumSheetName.renderUrl);

                setCellStyle(wk);

                int rowCount = 1;
                foreach (var renderUrl in renderUrlList)
                {
                    var typeTitle = gt_renderType.Find(t => t.Id == renderUrl.RenderTypeId).Title;
                    IRow newRow = null;
                    try
                    {
                        newRow = sheet.CreateRow(rowCount);
                    }
                    catch (Exception)
                    {
                        newRow = sheet.GetRow(rowCount);
                    }

                    SetCell(newRow.CreateCell(0), typeTitle);
                    SetCell(newRow.CreateCell(1), renderUrl.Description);
                    SetCell(newRow.CreateCell(2), renderUrl.Url);

                    rowCount++;
                }
            }
            WriteToFile(wk, filePath);
        }

        //写入数据到参数sheet  --调用获取元数据集合API
        public void WriteDataToMataDataParam(string filePath)
        {
            //调用语义服务  得到所有的元数据集合  mataDataresult
            IWrapedDataSearchService searchService =new WrapedDataSearchService();
            var mataDataresult=searchService.GetMetadataDefinition();

            IWorkbook wk = null;
            using (FileStream fs = File.OpenRead(filePath))
            {
                wk = GetWorkBook(fs, filePath);

                setCellStyle(wk);

                ISheet sheet = wk.GetSheet(EnumSheetName.metadataParam);
                int rowCount = 1;
                foreach (var mataData in mataDataresult)
                {
                    IRow newRow = null;
                    try
                    {
                        newRow = sheet.CreateRow(rowCount);
                    }
                    catch (Exception)
                    {
                        newRow = sheet.GetRow(rowCount);
                    }

                    SetCell(newRow.CreateCell(0), mataData.Name);
                    SetCell(newRow.CreateCell(1), mataData.Description);

                    rowCount++;
                }
            }
            WriteToFile(wk, filePath);
        }



        /// <summary>
        /// 渲染知识模板的的展示URL添加下拉框数据
        /// </summary>
        private void RenderUrl(string filePath)
        {
            IWorkbook wk = null;
            using (FileStream fs = File.OpenRead(filePath))
            {
                wk = GetWorkBook(fs, filePath);

                var RangeName = "UrlRange";
                ISheet sheetModel = wk.GetSheet(EnumSheetName.knowledgeTemp);
                IName range = wk.CreateName();
                range.RefersToFormula = string.Format("{0}!$C$2:$C${1}", EnumSheetName.renderUrl, (_renderUrlList.Count + 1).ToString());
                range.NameName = RangeName;
                CellRangeAddressList regions=new CellRangeAddressList(2,100,5,5);
                IDataValidationHelper dvHelper=new XSSFDataValidationHelper((XSSFSheet)sheetModel);
                IDataValidationConstraint dvConstraint = dvHelper.CreateFormulaListConstraint(RangeName);
                IDataValidation validation = dvHelper.CreateValidation(dvConstraint, regions);
                sheetModel.AddValidationData(validation);
            }
            WriteToFile(wk, filePath);
        }

        //更新数据库IndexDefinition表
        public void UpdateIndexDefinition(string filePath)
        {
            var indexDefList = ReadDataIndexDefinition(filePath);
            foreach (var indexDef in indexDefList)
            {
                var CurrentIndexDef = gt_IndexDefinition.Find(t => t.Code == indexDef.Code);
                //如果没有就加入
                if (CurrentIndexDef != null)
                {
                    //如果存在 则修改IndexDefinitionTitle
                    CurrentIndexDef.Title = indexDef.Title;
                    CurrentIndexDef.UpdatedDate = DateTime.Now;
                    gt_IndexDefinition.Update(CurrentIndexDef);

                }
                else
                {
                    gt_IndexDefinition.Add(new GT_IndexDefinition()
                    {
                        Code = indexDef.Code,
                        Title = indexDef.Title,
                        CreatedBy = "pmis",
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    });
                }
            }
        }

        //更新数据库RenderUrl表
        public void UpdateRenderUrl(string filePath)
        {
            var tUrlModelList = ReadDataRenderUrl(filePath);
            foreach (var tUrlModel in tUrlModelList)
            {
                var renderType = gt_renderType.Find(t => t.Title == tUrlModel.RenderTypeTitle);
                if (renderType == null)
                {
                    return;
                }
                //是否存在当前RenderUrl
                var currentRenderUrl = gt_RenderUrl.Find(t => t.Url == tUrlModel.Url);
                if (currentRenderUrl != null)
                {
                    //原来有这个RenderUrl，即更新RenderUrl信息
                    currentRenderUrl.Description = tUrlModel.Description;
                    currentRenderUrl.UpdatedDate = DateTime.Now;
                    gt_RenderUrl.Update(currentRenderUrl);
                }
                else
                {
                    //原来没有RenderUrl，即添加RenderUrl信息
                    gt_RenderUrl.Add(new GT_RenderUrl()
                    {
                        RenderTypeId = renderType.Id,
                        Url = tUrlModel.Url,
                        Description = tUrlModel.Description,
                        CreatedDate = DateTime.Now,
                        CreatedBy = "pmis",
                        UpdatedDate = DateTime.Now,
                    });
                }
            }
        }


        public IWorkbook NPOIOpenExcel(string FileName)
        {
            IWorkbook MyXSSFWorkBook;
            Stream MyExcelStream = OpenClasspathResource(FileName);
            MyXSSFWorkBook = new XSSFWorkbook(MyExcelStream);
            return MyXSSFWorkBook;
        }
        private Stream OpenClasspathResource(String fileName)
        {
            FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            return file;
        }
        private void WriteToFile(IWorkbook workbook, String fileName)
        {
            FileStream file = File.Create(fileName);
            workbook.Write(file);
            file.Close();
        }



        /// <summary>
        /// 下载模板文件
        /// </summary>
        /// <param name="fileName">文件路径</param>
        public void DonwnLoadFile(string relativPath)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath(relativPath); //路径
            if (System.IO.File.Exists(path))  //如果文件存在
            {
                FileInfo fileInfo = new FileInfo(path);
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();
                //加上HttpUtility.UrlEncode()方法，防止文件下载时，文件名乱码，（保存到磁盘上的文件名称应为“中文名.gif”）
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(relativPath.Substring(relativPath.LastIndexOf('/')+1)));
                //Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                HttpContext.Current.Response.AddHeader("Content-Transfer-Encoding", "binary");
                //Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                //Response.TransmitFile(path);  
                HttpContext.Current.Response.WriteFile(fileInfo.FullName);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

    }
}
