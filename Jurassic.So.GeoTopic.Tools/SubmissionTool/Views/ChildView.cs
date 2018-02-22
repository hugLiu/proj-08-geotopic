using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinToolbars;
using Jurassic.AppCenter.SmartClient.Infrastructure.Interface;
using Microsoft.Practices.CompositeUI.Commands;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Jurassic.So.Infrastructure;
using System.IO;
using Jurassic.AppCenter.SmartClient.Infrastructure.Interface.Services;
using Jurassic.So.GeoTopic.SubmissionTool.Services;

namespace Jurassic.So.GeoTopic.SubmissionTool.Views
{
    [SmartPart]
    public partial class ChildView : Form, IChildView
    {
        private ChildViewPresenter _presenter;
        public ChildView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the presenter. The dependency injection system will automatically
        /// create a new presenter for you.
        /// </summary>
        [CreateNew]
        public ChildViewPresenter Presenter
        {
            set
            {
                _presenter = value;
                _presenter.View = this;
            }
        }

        public UltraPanel MainPanel
        {
            get { return ChildView_Fill_Panel; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _presenter.OnViewReady();
        }
        /// <summary>元数据标签服务URL</summary>
        /// <summary>元数据标签文件</summary>
        public string MetadataTagsUrl
        {
            get { return this.txtMetadataTagsUrl.Text.Trim(); }
            set { this.txtMetadataTagsUrl.Text = value; }
        }
        /// <summary>加载元数据标签URL</summary>
        private void btnLoadMetadataTagsUrl_Click(object sender, EventArgs e)
        {
            var metadataTagsUrl = this.MetadataTagsUrl;
            if (metadataTagsUrl.Length == 0)
            {
                _presenter.WorkItem.Alert("请先设置元数据标签服务URL！");
                this.txtMetadataTagsUrl.Focus();
                return;
            }
            _presenter.LoadMetadataTagsFromUrl(this.MetadataTagsUrl);
        }
        /// <summary>元数据标签文件</summary>
        public string MetadataTagsFile
        {
            get { return this.txtMetadataTagsFile.Text.Trim(); }
            set
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => this.txtMetadataTagsFile.Text = value));
                }
                else
                {
                    this.txtMetadataTagsFile.Text = value;
                }
            }
        }
        /// <summary>浏览元数据标签文件</summary>
        private void btnBrowseMetadataTagsFile_Click(object sender, EventArgs e)
        {
            using (var browser = new OpenFileDialog())
            {
                var file = this.MetadataTagsFile;
                if (file.Length > 0) browser.InitialDirectory = Path.GetDirectoryName(file);
                //browser.CheckFileExists = true;
                if (browser.ShowDialog() != DialogResult.OK) return;
                this.MetadataTagsFile = browser.FileName;
                this.Worker.DoWork += BeginLoadMetadataTagsFile;
                this.Worker.RunWorkerCompleted += EndLoadMetadataTagsFile;
                //this.submissionConfigBindingSource.SuspendBinding();
                this.Worker.RunWorkerAsync(browser.FileName);
            }
        }
        /// <summary>开始加载元数据标签文件</summary>
        private void BeginLoadMetadataTagsFile(object sender, DoWorkEventArgs e)
        {
            //var splash = _presenter.WorkItem.Services.Get<ISplashService>();
            try
            {
                //splash.Show("正在加载元数据标签文件，请稍候...");
                _presenter.LoadMetadataTagsFromFile(e.Argument.ToString());
            }
            finally
            {
                //splash.Close();
                this.Worker.DoWork -= BeginLoadMetadataTagsFile;
                //this.submissionConfigBindingSource.ResumeBinding();
            }
        }
        /// <summary>加载元数据标签文件完成</summary>
        private void EndLoadMetadataTagsFile(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null) return;
                Program.WriteLog(e.Error);
                _presenter.WorkItem.Error(e.Error.Message);
            }
            finally
            {
                this.Worker.RunWorkerCompleted -= EndLoadMetadataTagsFile;
            }
        }
        /// <summary>绑定元数据标签</summary>
        public void BindMetadataTags(object dataSource)
        {
            this.submissionConfigBindingSource.DataSource = dataSource;
            this.submissionConfigBindingSource.ResetBindings(false);
        }
        /// <summary>是否只显示可用的元数据标签</summary>
        public bool MetadataTagsOnlyShowEnabled
        {
            get { return this.chkMetadataTagsOnlyShowEnabled.Checked; }
            set
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => this.chkMetadataTagsOnlyShowEnabled.Checked = value));
                }
                else
                {
                    this.chkMetadataTagsOnlyShowEnabled.Checked = value;
                }
            }
        }
        /// <summary>是否只显示可用的元数据标签</summary>
        private void chkMetadataTagsOnlyShowEnabled_CheckedChanged(object sender, EventArgs e)
        {
            _presenter.BindMetadataTags(this.MetadataTagsOnlyShowEnabled);
            this.submissionConfigBindingSource.ResetBindings(false);
        }
        /// <summary>保存标签</summary>
        private void btnSaveTags_Click(object sender, EventArgs e)
        {
            this.submissionConfigBindingSource.EndEdit();
            _presenter.Save();
        }
        /// <summary>上下移动</summary>
        private void grdTags_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.grdBuildTags.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                if (e.ColumnIndex == upTagsColumn.Index)
                {
                    if (e.RowIndex == 0) return;
                    var height = this.grdBuildTags.Rows[e.RowIndex - 1].Height;
                    MouseUtil.MoveUp(height);
                    _presenter.MoveUpMetadataTag(e.RowIndex);
                    this.submissionConfigBindingSource.ResetBindings(false);
                    this.grdBuildTags.CurrentCell = this.grdBuildTags.Rows[e.RowIndex - 1].Cells[0];
                }
                else
                {
                    if (e.RowIndex + 1 == this.grdBuildTags.Rows.Count) return;
                    var height = this.grdBuildTags.Rows[e.RowIndex + 1].Height;
                    MouseUtil.MoveDn(height);
                    _presenter.MoveDnMetadataTag(e.RowIndex);
                    this.submissionConfigBindingSource.ResetBindings(false);
                    this.grdBuildTags.CurrentCell = this.grdBuildTags.Rows[e.RowIndex + 1].Cells[0];
                }
            }
        }
        /// <summary>删除选项</summary>
        private void grdOptions_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.grdBuildOptions.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                this.grdBuildOptions.Rows.RemoveAt(e.RowIndex);
            }
        }
        /// <summary>保存选项</summary>
        private void btnSaveOptions_Click(object sender, EventArgs e)
        {
            this.submissionConfigBindingSource.EndEdit();
            _presenter.Save();
        }
        /// <summary>成果文件夹</summary>
        public string PTDir
        {
            get { return this.txtBuildPTDir.Text.Trim(); }
            set
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => this.txtBuildPTDir.Text = value));
                }
                else
                {
                    this.txtBuildPTDir.Text = value;
                }
            }
        }
        /// <summary>选择成果文件夹</summary>
        private void btnBrowsePTDir_Click(object sender, EventArgs e)
        {
            using (var browser = new FolderBrowserDialog())
            {
                var folder = this.PTDir;
                if (folder.Length > 0) browser.SelectedPath = folder;
                if (browser.ShowDialog() != DialogResult.OK) return;
                this.PTDir = browser.SelectedPath;
                _presenter.SavePTDir();
            }
        }
        /// <summary>Excel文件</summary>
        public string ExcelFile
        {
            get { return this.txtBuildExcelFile.Text.Trim(); }
            set
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => this.txtBuildExcelFile.Text = value));
                }
                else
                {
                    this.txtBuildExcelFile.Text = value;
                }
            }
        }
        /// <summary>浏览Excel文件</summary>
        private void btnExcelFile_Click(object sender, EventArgs e)
        {
            using (var browser = new SaveFileDialog())
            {
                var file = this.ExcelFile;
                if (file.Length > 0) browser.InitialDirectory = Path.GetDirectoryName(file);
                //browser.CheckFileExists = true;
                if (browser.ShowDialog() != DialogResult.OK) return;
                this.ExcelFile = browser.FileName;
                _presenter.SaveExcelFile();
            }
        }
        /// <summary>开始生成Excel模板文件</summary>
        private void btnStartBuildExcelTemplate_Click(object sender, EventArgs e)
        {
            this.Worker.DoWork += BeginBuildExcelTemplate;
            this.Worker.RunWorkerCompleted += EndBuildExcelTemplate;
            this.Worker.RunWorkerAsync();
            this.Enabled = false;
        }
        /// <summary>开始生成Excel模板文件</summary>
        private void BeginBuildExcelTemplate(object sender, DoWorkEventArgs e)
        {
            var splash = _presenter.WorkItem.Services.Get<ISplashService>();
            try
            {
                splash.Show("正在生成Excel模板文件，请稍候...");
                _presenter.BuildExcelTemplate();
            }
            finally
            {
                splash.Close();
                this.Worker.DoWork -= BeginBuildExcelTemplate;
            }
        }
        /// <summary>结束生成Excel模板文件</summary>
        private void EndBuildExcelTemplate(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    _presenter.WorkItem.Alert("Excel模板文件生成成功！");
                }
                else
                {
                    Program.WriteLog(e.Error);
                    _presenter.WorkItem.Error(e.Error.Message);
                }
            }
            finally
            {
                this.Worker.RunWorkerCompleted -= EndBuildExcelTemplate;
                this.Enabled = true;
            }
        }
        /// <summary>显示操作说明</summary>
        private void btnBuildOperationDesc_Click(object sender, EventArgs e)
        {
            new frmBuildOperationDesc().ShowDialog();
        }
        /// <summary>提交服务URL</summary>
        public string SubmissionUrl
        {
            get { return this.txtSubmitUrl.Text.Trim(); }
            set { this.txtSubmitUrl.Text = value; }
        }
        /// <summary>是否为审核状态</summary>
        public bool SubmitAuthentic
        {
            get { return this.chkSubmitAuthentic.Checked; }
            set { this.chkSubmitAuthentic.Checked = value; }
        }
        /// <summary>是否需要调用元数据自动补全功能</summary>
        public bool SubmitAutoComplement
        {
            get { return this.chkSubmitAutoComplement.Checked; }
            set { this.chkSubmitAutoComplement.Checked = value; }
        }
        /// <summary>上传人</summary>
        public string SubmitUploadedBy
        {
            get { return this.txtSubmitUploadedBy.Text.Trim(); }
            set { this.txtSubmitUploadedBy.Text = value; }
        }
        /// <summary>
        /// 联系方式。
        /// </summary>
        /// <remarks>
        /// 可以提供多个联系方式，使用“|”分割。如”email:z3@a.com | qq:12345 | wc:3214 | tel: 18601234567”
        /// </remarks>
        public string SubmitContact
        {
            get { return this.txtSubmitContact.Text.Trim(); }
            set { this.txtSubmitContact.Text = value; }
        }
        /// <summary>
        /// 提交源系统名称。（从哪个系统提交的，如油田规划应用）
        /// </summary>
        public string SubmitApplication
        {
            get { return this.txtSubmitApplication.Text.Trim(); }
            set { this.txtSubmitApplication.Text = value; }
        }
        /// <summary>
        /// 提交的任务名称。（做什么任务的时候提交的，如资源建设、评价总结）
        /// </summary>
        public string SubmitTask
        {
            get { return this.txtSubmitTask.Text.Trim(); }
            set { this.txtSubmitTask.Text = value; }
        }
        /// <summary>保存提交配置</summary>
        private void btnSubmitSave_Click(object sender, EventArgs e)
        {
            var submissionUrl = this.SubmissionUrl;
            if (submissionUrl.Length == 0)
            {
                _presenter.WorkItem.Alert("请先设置提交URL！");
                this.txtSubmitUrl.Focus();
                return;
            }
            _presenter.SaveSubmitConfig();
        }
        /// <summary>批量提交失败重试</summary>
        private void btrnRetryBatchSubmit_Click(object sender, EventArgs e)
        {
            StartBatchSubmit(true);
        }
        /// <summary>开始批量提交</summary>
        private void btnBatchSubmit_Click(object sender, EventArgs e)
        {
            var submissionUrl = this.SubmissionUrl;
            if (submissionUrl.Length == 0)
            {
                _presenter.WorkItem.Alert("请先设置提交URL！");
                this.txtSubmitUrl.Focus();
                return;
            }
            var excelFile = this.ExcelFile;
            if (excelFile.Length == 0)
            {
                _presenter.WorkItem.Alert("请先设置要提交的Excel文件！");
                this.tabMain.SelectedTab = this.pagBuildTemplate.Tab;
                this.txtBuildExcelFile.Focus();
                return;
            }
            if (!File.Exists(excelFile))
            {
                _presenter.WorkItem.Alert($"要提交的Excel文件[{excelFile}]不存在！");
                this.tabMain.SelectedTab = this.pagBuildTemplate.Tab;
                this.txtBuildExcelFile.Focus();
                return;
            }
            _presenter.SaveSubmitConfig();
            StartBatchSubmit(false);
        }
        /// <summary>批量提交失败重试</summary>
        private void StartBatchSubmit(bool retry)
        {
            //if (!retry) this.txtSubmitContent.Clear();
            this.Worker.WorkerReportsProgress = true;
            this.Worker.DoWork += BeginBatchSubmit;
            this.Worker.ProgressChanged += BatchSubmit_ProgressChanged;
            this.Worker.RunWorkerCompleted += EndBatchSubmit;
            //var eventHandler = StartBatchSubmitEventHandler;
            //if (eventHandler != null) eventHandler(this, new EventArgs<BackgroundWorker>(this.Worker));
            this.Worker.RunWorkerAsync(retry);
            this.Enabled = false;
        }
        /// <summary>开始批量提交</summary>
        private void BeginBatchSubmit(object sender, DoWorkEventArgs e)
        {
            try
            {
                _presenter.BatchSubmit(this.Worker, (bool)e.Argument);
            }
            finally
            {
                this.Worker.DoWork -= BeginBatchSubmit;
            }
        }
        /// <summary>处理进度事件</summary>
        private void BatchSubmit_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ProgressChangedEventHandler(BatchSubmit_ProgressChanged), sender, e);
                return;
            }
            var progressProperty = e.UserState.As<ProgressProperty>();
            if (e.ProgressPercentage == -1)
            {
                if (progressProperty.MaxValue == -1)
                {
                    this.pbarSubmit.Style = ProgressBarStyle.Marquee;
                    this.pbarSubmit.Maximum = 100;
                    this.pbarSubmit.Value = 0;
                }
                else
                {
                    this.pbarSubmit.Style = ProgressBarStyle.Continuous;
                    this.pbarSubmit.Maximum = progressProperty.MaxValue;
                    this.pbarSubmit.Value = 0;
                }
                //this.txtSubmitContent.Focus();
            }
            else if (e.ProgressPercentage > 0)
            {
                this.pbarSubmit.Value = e.ProgressPercentage;
            }
            if (progressProperty.Message.Length > 0)
            {
                this.lblSubmitState.Text = progressProperty.Message;
            }
            if (progressProperty.Content.Length > 0)
            {
                //this.txtSubmitContent.AppendText(progressProperty.Content);
                //this.txtSubmitContent.AppendText(Environment.NewLine);
                //this.txtSubmitContent.Select(this.txtSubmitContent.TextLength, 0);
                //this.txtSubmitContent.ScrollToCaret();
            }
        }
        /// <summary>结束批量提交</summary>
        private void EndBatchSubmit(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    _presenter.WorkItem.Alert("批量提交完成！");
                }
                else
                {
                    Program.WriteLog(e.Error);
                    _presenter.WorkItem.Error(e.Error.Message);
                }
            }
            finally
            {
                this.Worker.ProgressChanged -= BatchSubmit_ProgressChanged;
                this.Worker.RunWorkerCompleted -= EndBatchSubmit;
                this.Enabled = true;
            }
        }
        /// <summary>显示正在提交的产品</summary>
        public void ShowProducts(SubmitProduct[] products)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<SubmitProduct[]>(ShowProducts), (object)products);
                return;
            }
            try
            {
                this.lvwSubmitProducts.BeginUpdate();
                foreach (var product in products)
                {
                    foreach (var file in product.Files)
                    {
                        var lvwItem = new ListViewItem(product.ID.ToString());
                        lvwItem.SubItems.Add(file.Path);
                        lvwItem.SubItems.Add(file.File);
                        lvwItem.SubItems.Add("等待提交");
                        file.ListViewItem = lvwItem;
                        this.lvwSubmitProducts.Items.Add(lvwItem);
                    }
                }
            }
            finally
            {
                this.lvwSubmitProducts.EndUpdate();
            }
        }
        /// <summary>刷新产品状态</summary>
        public void RefreshProductStatus(SubmitProduct product, string status)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<SubmitProduct, string>(RefreshProductStatus), product, status);
                return;
            }
            foreach (var file in product.Files)
            {
                file.ListViewItem.SubItems[3].Text = status;
            }
            product.Files[0].ListViewItem.EnsureVisible();
        }
    }
}