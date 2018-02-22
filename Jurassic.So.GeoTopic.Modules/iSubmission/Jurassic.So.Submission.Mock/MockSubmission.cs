using Jurassic.PKS.Service.Submission;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Jurassic.PKS.Service;
using Jurassic.So.Infrastructure;
using Jurassic.So.Submission.Mock.Models;
using Newtonsoft.Json.Linq;

namespace Jurassic.So.Submission.Mock
{
    /// <summary>模拟成果提交服务 </summary>
    public class MockSubmission : ISubmission
    {
        //private readonly MockGBContent _content;
        private readonly Dictionary<string, string> _submissionMap;

        public MockSubmission()
        {
            //_content = new MockGBContent();
            _submissionMap = LoadMap();
        }

        private Dictionary<string, string> LoadMap()
        {
            string binPath = this.GetBinPath();
            string kmdConfigurationFile = Path.Combine(binPath, "SubmissionMap.json");
            string json = File.ReadAllText(kmdConfigurationFile);
            return json.JsonTo<Dictionary<string, string>>();
        }

        private string GetBinPath()
        {
            string dllFile = Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", "");
            string binPath = dllFile.Substring(0, dllFile.LastIndexOf('/'));
            return binPath;
        }

        public string Submit(SubmissionInfo info)
        {
            var naturekey = "";
            switch (info.Action)
            {
                case SubmissionAction.Create:
                    naturekey = CreateSubmission(info);
                    break;
                case SubmissionAction.Delete:
                    naturekey = DeleteSubmission(info);
                    break;
                case SubmissionAction.Replace:
                    naturekey = ReplaceSubmission(info);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return naturekey;
        }

        public async Task<string> SubmitAsync(SubmissionInfo info)
        {
            return await Task.FromResult(Submit(info));
        }

        public string Upload(string fileName, Stream stream)
        {
            var gbFile = new GB_File
            {
                FileName = fileName,
                File = stream.ToBytes(),
                CreatedDate = DateTime.Now,
                CreatedBy = "pmis",
                UpdatedDate = DateTime.Now,
                UpdatedBy = "pmis",
                IsDelete = false
            };
            using (var _content = new MockGBContent())
            {
                _content.GB_File.Add(gbFile);
                _content.SaveChanges();
            }
            stream.Close();
            return gbFile.Id.ToString();
        }
    

        public async Task<string> UploadAsync(string fileName, Stream stream)
        {
            return await Task.FromResult(Upload(fileName, stream));
        }

        private string CreateSubmission(SubmissionInfo info)
        {

            var naturekey = Guid.NewGuid();
            var gbSubmission = new GB_Submission
            {
                NatureKey = naturekey,
                Authentic = info.Option.Authentic,
                AutoComplement = info.Option.AutoComplement,
                UploadedBy = info.Option.UploadedBy,
                UploadedDate = info.Option.UploadedDate,
                Contact = info.Option.Contact,
                Application = info.Option.Application,
                Task = info.Option.Task,
                CreatedDate = DateTime.Now,
                CreatedBy = "pmis",
                UpdatedDate = DateTime.Now,
                UpdatedBy = "pmis",
                IsDelete = false
            };

            var kmd = info.KMD as KMD;
            if (kmd != null)
            {
                foreach (var item in _submissionMap)
                {
                    if (item.Value == "") continue;
                    var obj = kmd.GetProperty(item.Value);
                    if (obj == null) continue;
                    var gbPropert = typeof(GB_Submission).GetProperty(item.Key);
                    if (gbPropert == null) continue;
                    if (obj is JArray)
                    {
                        var strs =
                            obj.As<JArray>().ToList<object>().ConvertAll(t => t.ToJson().JsonTo<string>()).ToArray();
                        var str = string.Join(",", strs);
                        gbPropert.SetValue(gbSubmission, str);
                    }
                    else
                    {
                        gbPropert.SetValue(gbSubmission, obj);
                    }
                }
            }
            using (var _content = new MockGBContent())
            {
                //_content.GB_Submission.Add(gbSubmission);
                //_content.SaveChanges();
                //foreach (var filelD in info.FilelDs)
                //{
                //    var gbFile = _content.GB_File.FirstOrDefault(t => t.Id.ToString() == filelD.Trim());
                //    if (gbFile != null)
                //        gbFile.SubmissiomId = gbSubmission.Id;
                //}

                var gbFiles = _content.GB_File.Where(t => info.FileIDs.Contains(t.Id.ToString())).ToList();
                gbFiles.ForEach(e => gbSubmission.GB_File.Add(e));
                _content.GB_Submission.Add(gbSubmission);
                _content.SaveChanges();
            }
            return naturekey.ToString();

        }

        private string DeleteSubmission(SubmissionInfo info)
        {
            using (var _content = new MockGBContent())
            {
                var gbSubmission = _content.GB_Submission.FirstOrDefault(t => t.NatureKey.ToString() == info.NatureKey.Trim());
                if (gbSubmission == null) return new { naturekey = info.NatureKey }.ToJson();
                var gbFile = _content.GB_File.Where(t => t.SubmissiomId == gbSubmission.Id).ToList();
                _content.GB_File.RemoveRange(gbFile);
                _content.GB_Submission.Remove(gbSubmission);
                _content.SaveChanges();
            }
            return info.NatureKey;
        }

        private string ReplaceSubmission(SubmissionInfo info)
        {

            using (var _content = new MockGBContent())
            {
                var gbSubmission = _content.GB_Submission.FirstOrDefault(t => t.NatureKey.ToString() == info.NatureKey.Trim());
                if (gbSubmission == null)
                {
                    return null;
                }
                gbSubmission.Authentic = info.Option.Authentic;
                gbSubmission.AutoComplement = info.Option.AutoComplement;
                gbSubmission.UploadedBy = info.Option.UploadedBy;
                gbSubmission.UploadedDate = info.Option.UploadedDate;
                gbSubmission.Contact = info.Option.Contact;
                gbSubmission.Application = info.Option.Application;
                gbSubmission.Task = info.Option.Task;
                gbSubmission.UpdatedDate = DateTime.Now;
                gbSubmission.UpdatedBy = "pmis";

                var kmd = info.KMD as KMD;
                if (kmd != null)
                {
                    foreach (var item in _submissionMap)
                    {
                        if (item.Value == "") continue;
                        var obj = kmd.GetProperty(item.Value);
                        if (obj == null) continue;
                        var gbPropert = typeof(GB_Submission).GetProperty(item.Key);
                        if (gbPropert == null) continue;
                        if (obj is JArray)
                        {
                            var strs =
                                obj.As<JArray>().ToList<object>().ConvertAll(t => t.ToJson().JsonTo<string>()).ToArray();
                            var str = string.Join(",", strs);
                            gbPropert.SetValue(gbSubmission, str);
                        }
                        else
                        {
                            gbPropert.SetValue(gbSubmission, obj);
                        }
                    }
                }

                foreach (var filelD in info.FileIDs)
                {
                    var gbFile = _content.GB_File.FirstOrDefault(t => t.Id.ToString() == filelD.Trim());
                    if (gbFile != null)
                        gbFile.SubmissiomId = gbSubmission.Id;
                }
                _content.SaveChanges();
            }
            return info.NatureKey;
        }
    }
}
