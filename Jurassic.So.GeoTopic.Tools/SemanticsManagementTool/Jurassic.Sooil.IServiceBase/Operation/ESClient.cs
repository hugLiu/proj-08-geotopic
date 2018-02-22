using System.Threading.Tasks;
using PlainElastic.Net;

namespace Jurassic.Sooil.IServiceBase
{
    public class ESClient
    {
        private string host;
        private int port;
        private readonly ElasticConnection connection;
        public ESClient(string host, int port)
        {
            this.host = host;
            this.port = port;
            connection = new ElasticConnection(host, port);
        }

        public static ESClient Create(string host, int port)
        {
            return new ESClient(host, port);
        }

        public virtual async Task<string> Put(string command, string jsonData = null)
        {
            return await connection.PutAsync(command, jsonData);
        }

        public virtual async Task<string> Post(string command, string jsonData = null)
        {
            return await connection.PostAsync(command, jsonData);
        }
    }
}
