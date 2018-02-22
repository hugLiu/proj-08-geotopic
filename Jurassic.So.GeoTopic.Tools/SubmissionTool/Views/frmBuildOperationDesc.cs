using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.GeoTopic.SubmissionTool.Views
{
    public partial class frmBuildOperationDesc : Form
    {
        public frmBuildOperationDesc()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            var resName = "Jurassic.So.GeoTopic.SubmissionTool.Services.BuildOperationDesc.txt";
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resName);
            this.txtDesc.Text = Encoding.UTF8.GetString(stream.ToByteArray());
            this.txtDesc.SelectionStart = 0;
            this.txtDesc.SelectionLength = 0;
        }
    }
}
