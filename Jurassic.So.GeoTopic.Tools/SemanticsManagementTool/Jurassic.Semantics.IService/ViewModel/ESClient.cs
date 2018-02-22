using PlainElastic.Net;
using System;
using System.Threading.Tasks;

namespace Jurassic.Semantics.IService.ViewModel
{
    public class EsClient:IDisposable
    {
        private string _host;
        private int _port;
        private readonly ElasticConnection _connection;
        private const string SuccessFlag = "\"acknowledged\":true";
        public EsClient(string host, int port)
        {
            this._host = host;
            this._port = port;
            _connection = new ElasticConnection(host, port);
        }

        public static EsClient Create(string host, int port)
        {
            return new EsClient(host, port);
        }

        public virtual string Put(string command, string jsonData = null)
        {
            bool actual;
            try
            {
                string response =  _connection.Put(command, jsonData);
                actual = response.Contains(SuccessFlag);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return actual ? command + " success！" : command + "error！";
        }

        public virtual string Post(string command, string jsonData = null)
        {
            bool actual;
            string response;
            try
            {
                response =  _connection.Post(command, jsonData);
                if (response.Length > 1000) response = response.Substring(0, 1000);
                actual = response.Contains(SuccessFlag);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return actual ? command + " success！" : response;
        }
        public virtual string Delete(string command, string jsonData = null)
        {
            bool actual;
            try
            {
                string response =  _connection.Delete(command, jsonData);
                actual = response.Contains(SuccessFlag);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return actual ? "Delete success！" : "Delete unfind！";
        }

        public void Dispose()
        {
            this.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
