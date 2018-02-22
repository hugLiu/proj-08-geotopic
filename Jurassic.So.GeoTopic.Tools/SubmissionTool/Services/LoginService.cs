using Jurassic.AppCenter;
using Jurassic.AppCenter.SmartClient.Infrastructure.Interface.Services;
using Microsoft.Practices.CompositeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Jurassic.So.GeoTopic.SubmissionTool.Services
{
    public class LoginService : ILoginService
    {
        public AppUser Login()
        {
            var loginer = new AppUser();
            loginer.Id = "0";
            loginer.Name = "pmis";
            loginer.Password = "888888";
            loginer.IsDefaultRole = true;
            loginer.IsOnline = true;
            loginer.LastOpTime = DateTime.Now;
            return loginer;
        }
    }
}
