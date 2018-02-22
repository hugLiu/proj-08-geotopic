using Jurassic.Semantics.IService.ViewModel;
using PlainElastic.Net;
using PlainElastic.Net.Serialization;
using System;
using System.Collections.Generic;
using System.IO;

namespace Jurassic.Semantics.EFProvider
{
    public class ElasticSearchAccess<T> : IDisposable
    {
        public static string EsIndex = string.Empty;
        public static string Index_Type = string.Empty;
        public static string es_Host = string.Empty;
        public static int es_Port;
        private static EsClient esClient;
        static ElasticSearchAccess()
        {
            EsIndex = AppConfigHelper.GetValueByKey("es_index");
            Index_Type = AppConfigHelper.GetValueByKey("es_type");
            es_Host = AppConfigHelper.GetValueByKey("es_host");
            es_Port = Convert.ToInt32(AppConfigHelper.GetValueByKey("es_port"));
            esClient = EsClient.Create(es_Host, es_Port);
        }

        /// <summary>
        /// 可以创建任意索引库下不同类型的模型
        /// </summary>
        /// <param name="type">类型名称</param>
        /// <param name="path">实体模型的文件位置</param>
        /// <returns>创建成功返回为True 否则为false</returns>
        public string BuildModelBase(string type, string path)
        {
            DeleteType(type);

            var mappings = File.ReadAllText(path);
            var command = string.Format("{0}/_mapping/{1}", EsIndex, type);

            string response = esClient.Put(command, mappings);
            return response;
        }

        /// <summary>
        /// 创建索引seting部分
        /// </summary>
        /// <param name="isUpdate">标志是重建还是在原来的基础上更新</param>
        /// <param name="path">seting设置的文件路径</param>
        /// <returns>创建成功返回为True 否则为false</returns>
        public string BuildModelSettings(bool isUpdate, string path)
        {
            string settings = File.ReadAllText(path);
            string response;
            if (!isUpdate)
            {
                esClient.Delete(EsIndex);
                //重建
                response = esClient.Put(EsIndex, settings);
            }
            else
            {
                //更新
                esClient.Post(EsIndex + "/_close");
                response = esClient.Put(EsIndex + "/_settings", settings);
                esClient.Post(EsIndex + "/_open");
            }
            return response;
        }

        /// <summary>
        /// 删除整个索引
        /// </summary>
        /// <returns></returns>
        public string DeleteIndex()
        {
            string command = Commands.Delete(EsIndex);
            var result = esClient.Delete(command);
            return result;
        }

        /// <summary>
        /// 删除整个索引
        /// </summary>
        /// <param name="type">表名称</param>
        /// <returns></returns>
        public string DeleteType(string type)
        {
            //注意ES2.0 之后 通过query api删除 type的功能去除，然后需要安装
            //Delete By Query Plugin  这个插件，然后就是重启即可使用
            string command = Commands.Delete(EsIndex, type);
            var result = esClient.Delete(command);
            return result;
        }

        /// <summary>
        /// 根据id删除某条记录
        /// </summary>
        /// <param name="type">表名称</param>
        /// <param name="id">文档id</param>
        /// <returns></returns>
        public string Delete(string type, string id)
        {
            string command = Commands.Delete(EsIndex, type, id);
            var result = esClient.Delete(command);
            return result;
        }

        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public string Index(T doc)
        {
            return Index(doc, Index_Type);
        }

        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string Index(T doc, string type)
        {
            string command = Commands.Index(EsIndex, type);
            var jsonDocument = (new JsonNetSerializer()).Serialize(doc);
            var result = esClient.Post(command, jsonDocument);
            return result;
        }
        /// <summary>
        /// 修改数据模型模型后，导入ES的数据
        /// </summary>
        /// <param name="docs"></param>
        /// <returns></returns>
        public string Index(List<T> docs)
        {
            return Index(docs, Index_Type);
        }

        /// <summary>
        /// 修改数据模型模型后，导入ES的数据
        /// </summary>
        /// <param name="docs"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string Index(List<T> docs, string type)
        {
            var serializer = new JsonNetSerializer();
            var bulkJson =
                new BulkBuilder(serializer)
                   .BuildCollection(docs,
                   (builder, doc) => builder.Index(doc)
            );
            return Bulk(bulkJson, type); ;
        }

        /// <summary>
        /// bulk操作，根据指定的索引和类型
        /// </summary>
        /// <param name="bulkJson"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string Bulk(string bulkJson, string type)
        {
            if (bulkJson == null) return "索引文档为空";
            string bulkCommand = new BulkCommand(EsIndex, type);
            var result = esClient.Post(bulkCommand, bulkJson);
            return result;
        }

        /// <summary>
        /// 执行bulk操作，按照默认的配置文件
        /// </summary>
        /// <param name="bulkJson"></param>
        /// <returns></returns>
        public string Bulk(string bulkJson)
        {
            return Bulk(bulkJson, Index_Type);
        }
        /// <summary>
        /// 关闭刷新
        /// </summary>
        /// <returns></returns>
        public string CloseRefresh()
        {
            string command = new UpdateSettingsCommand(EsIndex);
            const string settingJson = "{\"settings\":{\"refresh_interval\":\"-1\"}}";
            var result = esClient.Put(command, settingJson);
            return result;
        }
        /// <summary>
        /// 启动刷新
        /// </summary>
        /// <returns></returns>
        public string OpenRefresh()
        {
            string command = new UpdateSettingsCommand(EsIndex);
            const string settingJson = "{\"settings\":{\"refresh_interval\":\"1s\"}}";
            var result = esClient.Put(command, settingJson);
            return result;
        }

        /// <summary>
        /// 运行期优化
        /// </summary>
        /// <returns></returns>
        public string Optimize(string type)
        {
            string command = new OptimizeCommand(EsIndex, type)
                            .MaxNumSegments(10)       //段数优化。要全面优化索引，将其设置为1。默认设置只需检查是否需要执行一个合并，如果需要，则执行它
                            .OnlyExpungeDeletes(true) //该优化操作是否只清空打有删除标签的索引记录
                            .Refresh(false)           //在执行完优化操作之后，再执行刷新操作
                            .Flush()
                            .WaitForMerge(false)     //当该参数设置为true时，表示其他请求操作要等到合并segment操作结束之后，再进行响应。
                            .BuildCommand();

            return esClient.Post(command, null);
        }

        public void Dispose()
        {
            esClient.Dispose();
        }
    }
}
