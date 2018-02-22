using Jurassic.AppCenter.SmartClient.Infrastructure.Interface;
using Microsoft.Practices.CompositeUI.Commands;
using Microsoft.Practices.CompositeUI.EventBroker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Jurassic.So.Infrastructure;
using Jurassic.So.GeoTopic.SubmissionTool.Services;
using System.ComponentModel;
using System.IO;
using Jurassic.AppCenter;
using System.Collections.Concurrent;
using Jurassic.PKS.WebAPI.Submission;

namespace Jurassic.So.GeoTopic.SubmissionTool.Views
{
    public class ChildViewPresenter : Presenter<IChildView>
    {
        #region 加载和关闭
        /// <summary>
        /// This method is a placeholder that will be called by the view when it has been loaded.
        /// </summary>
        public override void OnViewReady()
        {
            base.OnViewReady();
            this.ModuleController = this.WorkItem.GetController().As<ModuleController>();
            this.SubmissionService = this.ModuleController.SubmissionService;
            var config = this.SubmissionService.Config;
            this.View.MetadataTagsFile = config.MetadataTagsUrl;
            this.View.MetadataTagsFile = config.MetadataTagsFile;
            this.View.PTDir = config.PTDir;
            this.View.ExcelFile = config.ExcelFile;
            BindMetadataTags();
            this.View.SubmissionUrl = config.SubmissionUrl;
            this.View.SubmitApplication = config.SubmissionOption.Application;
            this.View.SubmitAuthentic = config.SubmissionOption.Authentic;
            this.View.SubmitAutoComplement = config.SubmissionOption.AutoComplement;
            this.View.SubmitContact = config.SubmissionOption.Contact;
            this.View.SubmitTask = config.SubmissionOption.Task;
            this.View.SubmitUploadedBy = config.SubmissionOption.UploadedBy;
            this.View.SubmitApplication = config.SubmissionOption.Application;
        }
        /// <summary>
        /// Close the view
        /// </summary>
        public override void OnCloseView()
        {
            base.CloseView();
        }
        #endregion

        #region 数据成员
        /// <summary>工作项控制器</summary>
        private ModuleController ModuleController { get; set; }
        /// <summary>提交服务</summary>
        private SubmissionService SubmissionService { get; set; }
        #endregion

        #region 访问方法
        /// <summary>加载元数据标签服务URL</summary>
        public void LoadMetadataTagsFromUrl(string url)
        {
            try
            {
                this.SubmissionService.LoadMetadataTagsFromUrl(url, false);
                BindMetadataTags();
            }
            catch (Exception ex)
            {
                this.WorkItem.Error(ex.Message);
            }
        }
        /// <summary>加载元数据标签文件</summary>
        public void LoadMetadataTagsFromFile(string file)
        {
            this.SubmissionService.LoadMetadataTagsFromFile(file);
            BindMetadataTags();
        }
        /// <summary>绑定标签</summary>
        private void BindMetadataTags()
        {
            BindMetadataTags(this.View.MetadataTagsOnlyShowEnabled);
        }
        /// <summary>绑定标签</summary>
        public void BindMetadataTags(bool onlyEnabled)
        {
            var config = this.SubmissionService.Config;
            if (onlyEnabled)
            {
                config.MetadataTagsView = config.MetadataTags.Where(e => e.Enabled).ToList();
            }
            else
            {
                config.MetadataTagsView = config.MetadataTags;
            }
            this.View.BindMetadataTags(config);
        }
        /// <summary>上移</summary>
        public void MoveUpMetadataTag(int rowIndex)
        {
            var config = this.SubmissionService.Config;
            var newTag = config.MetadataTagsView[rowIndex];
            var oldTag = config.MetadataTagsView[rowIndex - 1];
            config.MetadataTagsView.RemoveAt(rowIndex);
            config.MetadataTagsView.Insert(rowIndex - 1, newTag);
            if (!this.View.MetadataTagsOnlyShowEnabled) return;
            var newTagIndex = config.MetadataTags.IndexOf(newTag);
            var oldTagIndex = config.MetadataTags.IndexOf(oldTag);
            config.MetadataTags.Insert(oldTagIndex, newTag);
        }
        /// <summary>下移</summary>
        public void MoveDnMetadataTag(int rowIndex)
        {
            var config = this.SubmissionService.Config;
            var newTag = config.MetadataTagsView[rowIndex];
            var oldTag = config.MetadataTagsView[rowIndex + 1];
            config.MetadataTagsView.RemoveAt(rowIndex);
            config.MetadataTagsView.Insert(rowIndex + 1, newTag);
            if (!this.View.MetadataTagsOnlyShowEnabled) return;
            var newTagIndex = config.MetadataTags.IndexOf(newTag);
            var oldTagIndex = config.MetadataTags.IndexOf(oldTag);
            config.MetadataTags.Insert(oldTagIndex + 1, newTag);
        }
        /// <summary>保存成果文件夹</summary>
        public void SavePTDir()
        {
            this.SubmissionService.Config.PTDir = this.View.PTDir;
            this.SubmissionService.Save();
        }
        /// <summary>保存Excel文件</summary>
        public void SaveExcelFile()
        {
            this.SubmissionService.Config.ExcelFile = this.View.ExcelFile;
            this.SubmissionService.Save();
        }
        /// <summary>保存</summary>
        public void Save()
        {
            this.SubmissionService.Save();
        }
        /// <summary>生成Excel模板文件</summary>
        public void BuildExcelTemplate()
        {
            var config = this.SubmissionService.Config;
            var ptDir = this.View.PTDir;
            if (ptDir.Length == 0)
            {
                throw new JException($"请先设置成果文件夹！");
            }
            if (!Directory.Exists(ptDir))
            {
                throw new JException($"成果文件夹[{ptDir}]不存在！");
            }
            var ptFiles = Directory.GetFiles(ptDir, "*.*", SearchOption.AllDirectories);
            if (ptFiles.Length == 0)
            {
                throw new JException($"成果文件夹不存在成果文件！");
            }
            if (config.PTDir != ptDir) config.PTDir = ptDir;
            var excelFile = this.View.ExcelFile;
            if (config.ExcelFile != excelFile) config.ExcelFile = excelFile;
            this.SubmissionService.Save();
            this.SubmissionService.BuildExcelTemplate(ptFiles);
        }
        /// <summary>保存提交配置</summary>
        public void SaveSubmitConfig()
        {
            var config = this.SubmissionService.Config;
            config.ExcelFile = this.View.ExcelFile;
            config.SubmissionUrl = this.View.SubmissionUrl;
            config.SubmissionOption.Application = this.View.SubmitApplication;
            config.SubmissionOption.Authentic = this.View.SubmitAuthentic;
            config.SubmissionOption.AutoComplement = this.View.SubmitAutoComplement;
            config.SubmissionOption.Contact = this.View.SubmitContact;
            config.SubmissionOption.Task = this.View.SubmitTask;
            config.SubmissionOption.UploadedBy = this.View.SubmitUploadedBy;
            this.SubmissionService.Save();
        }
        /// <summary>当前提交上下文</summary>
        private SubmitContext CurrentSubmitContext { get; set; }
        /// <summary>开始批量提交</summary>
        public void BatchSubmit(BackgroundWorker worker, bool retry)
        {
            var config = this.SubmissionService.Config;
            var progressProperty = new ProgressProperty();
            progressProperty.Message = string.Empty;
            progressProperty.Content = string.Empty;
            progressProperty.MaxValue = -1;
            if (retry)
            {
                if (this.CurrentSubmitContext == null)
                {
                    throw new JException("请先批量提交！");
                }
                if (this.CurrentSubmitContext.FailureValues.Count == 0)
                {
                    throw new JException("已经全部批量提交成功！");
                }
                this.CurrentSubmitContext.Move();
            }
            else
            {
                if (config.MetadataTagsFile.IsNullOrEmpty() || config.MetadataTags.Count == 0)
                {
                    throw new JException("未设置元数据标签文件或元数据标签文件未加载！");
                }
                this.CurrentSubmitContext = new SubmitContext();
                this.CurrentSubmitContext.Values = new ConcurrentQueue<SubmitProduct>();
                this.CurrentSubmitContext.FailureValues = new ConcurrentQueue<SubmitProduct>();
            }
            var context = this.CurrentSubmitContext;
            context.View = this.View;
            context.WorkItem = this.WorkItem;
            context.Progress = progressProperty;
            context.Worker = worker;
            if (!retry)
            {
                var excelFile = this.View.ExcelFile;
                progressProperty.Message = "正在加载Excel文件...";
                progressProperty.MaxValue = -1;
                worker.ReportProgress(-1, progressProperty.Clone());
                this.SubmissionService.LoadProducts(context, excelFile);
            }
            var products = context.Values.ToArray();
            this.View.ShowProducts(products);
            var total = context.Values.Count;
            progressProperty.Message = "正在提交成果数据...";
            progressProperty.MaxValue = total;
            worker.ReportProgress(-1, progressProperty.Clone());
            progressProperty.Message = string.Empty;
            progressProperty.Content = string.Empty;
            context.ProgressValue = 0;
            context.ApiSubmissionService = new ApiWrappedSubmissionService(config.SubmissionUrl);
            context.Values
                .AsParallel<SubmitProduct>()
                .ForAll(e => this.SubmissionService.UploadProduct(context, e));
            //while (context.Values.Count > 0)
            //{
            //    SubmitProduct product;
            //    if (!context.Values.TryDequeue(out product)) continue;
            //    this.SubmissionService.UploadProduct(context, product);
            //}
            progressProperty.Message = "提交成果数据完成";
            worker.ReportProgress(-2, progressProperty.Clone());
        }
        #endregion
    }
}
