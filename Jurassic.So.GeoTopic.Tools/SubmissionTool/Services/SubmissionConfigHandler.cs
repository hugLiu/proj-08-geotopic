using Jurassic.AppCenter;
using Jurassic.AppCenter.SmartClient.Infrastructure.Interface.Services;
using Microsoft.Practices.CompositeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Jurassic.AppCenter.Logs;
using System.Xml.Serialization;

namespace Jurassic.So.GeoTopic.SubmissionTool.Services
{
    /// <summary>提交配置</summary>
    internal sealed class SubmissionConfigHandler
    {
        /// <summary>配置文件</summary>
        private string ConfigFile
        {
            get { return "Submission.config"; }
        }
        /// <summary>载入方法</summary>
        public SubmissionConfig Load(string path)
        {
            var xmlFile = Path.Combine(path, this.ConfigFile);
            if (!File.Exists(xmlFile)) return null;
            try
            {
                using (var stream = new FileStream(xmlFile, FileMode.Open))
                {
                    var serializer = new XmlSerializer(typeof(SubmissionConfig));
                    var config = (SubmissionConfig)serializer.Deserialize(stream);
                    config.Load();
                    return config;
                }
            }
            catch (Exception ex)
            {
                Program.WriteLog(ex);
            }
            return null;
        }
        /// <summary>保存方法</summary>
        public void Save(string path, SubmissionConfig config)
        {
            var xmlFile = Path.Combine(path, this.ConfigFile);
            using (var stream = new FileStream(xmlFile, FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(SubmissionConfig));
                serializer.Serialize(stream, config);
            }
        }
    }
}
