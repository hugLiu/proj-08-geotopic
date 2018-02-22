using Jurassic.AppCenter.Config;
using Jurassic.AppCenter.Resources;
using Jurassic.AppCenter.SmartClient.Infrastructure.Interface;
using Jurassic.AppCenter.SmartClient.Infrastructure.Shell;
//using Jurassic.Com.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Jurassic.AppCenter.Logs;
using Jurassic.AppCenter.SmartClient.Infrastructure.Interface.Constants;
using Infragistics.Practices.CompositeUI.WinForms;
using Infragistics.Win.UltraWinTabs;
using SR = Jurassic.So.GeoTopic.SubmissionTool.Properties.Resources;

namespace Jurassic.So.GeoTopic.SubmissionTool
{
    class Program : ShellApplication
    {
        [STAThread]
        static void Main()
        {
            ShellApplication.Main<Program>();
        }
        protected override void AddServices()
        {
            base.AddServices();
        }
        protected override void AfterShellCreated()
        {
            base.AfterShellCreated();
            //this.RootWorkItem.Services.Remove<IAuthenticationService>();
            //var _bgImage = Image.FromFile(Path.Combine(Application.StartupPath, @"Images\mainbg.jpg"));
            //var mdiClient = GetClient();
            //mdiClient.BackgroundImage = _bgImage;

            Shell.Text = "Jurassic Shell";
            var appIcon = SR.personalization;
            if (appIcon != null) Shell.Icon = appIcon;
            //(Shell as ShellForm).ToolbarsManager.Ribbon.DisplayMode = Infragistics.Win.UltraWinToolbars.RibbonDisplayMode.TabsOnly;

            var context = new UIContext(this.Shell);
            context.Load("Location", "X=100,Y=100");
            context.Load("Size", "Width=860,Height=700");
            //context.Load("StartPosition", "CenterScreen");
            //context.Load("WindowState", "Maximized");

            var mdiTabWorkspace = (UltraMdiTabWorkspace)this.RootWorkItem.Workspaces.Get(WorkspaceNames.MdiTabWorkspace);
            mdiTabWorkspace.TabSettings.CloseButtonVisibility = TabCloseButtonVisibility.Never;

            this.Shell.ToolbarsManager.Visible = false;

            this.RootWorkItem.WorkItems.First().Value.GetController().Run();

            Shell.FormClosed += (s, e) =>
            {
                context.Save(true);
            };
        }
        public static void WriteLog(string message)
        {
            LogHelper.Write(new JLogInfo()
            {
                ActionName = "SubmissionTool",
                ModuleName = "SubmissionTool",
                LogType = "Info",
                Message = message
            }, null);
        }
        public static void WriteLog(Exception ex)
        {
            LogHelper.Write(new JLogInfo()
            {
                ActionName = "SubmissionTool",
                ModuleName = "SubmissionTool",
                LogType = "Error",
                Message = ex.Message
            }, ex);
        }
    }
}
