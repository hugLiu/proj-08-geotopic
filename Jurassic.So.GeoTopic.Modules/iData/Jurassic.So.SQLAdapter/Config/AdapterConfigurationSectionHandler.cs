using System.Xml.Linq;
using Jurassic.So.ETL;
using Jurassic.So.Infrastructure;
using Jurassic.PKS.Service.Adapter;
using System.Collections.Generic;
using System.Linq;
using System;
using Jurassic.PKS.Service;
using Jurassic.So.Business;
using System.Configuration;
using System.Xml;
using Jurassic.So.Infrastructure.Logging;

namespace Jurassic.So.Adapter
{
    /// <summary>适配器配置节处理器</summary>
    public class AdapterConfigurationSectionHandler : IConfigurationSectionHandler
    {
        #region 构造函数
        /// <summary>构造函数</summary>
        public AdapterConfigurationSectionHandler()
        {
            this.Config = new ETLXmlConfiguration();
        }
        #endregion

        #region 数据成员
        /// <summary>默认配置文件</summary>
        public string ConfigFile { get { return "Adapter.Config"; } }
        /// <summary>ETL的XML配置</summary>
        public ETLXmlConfiguration Config { get; set; }
        #endregion

        #region 配置方法
        /// <summary>加载</summary>
        public object Create(object parent, object configContext, XmlNode section)
        {
            throw new NotImplementedException();
        }
        /// <summary>加载</summary>
        public AdapterConfigurationSectionHandler Load(string xmlFile, string nodeName = "")
        {
            var xdoc = XDocument.Load(xmlFile);
            var root = xdoc.Root;
            if (!nodeName.IsNullOrEmpty())
            {
                root = root.Element(nodeName);
            }
            this.Config.LoadRuntimeComponents();
            LoadAdapter(root);
            return this;
        }
        /// <summary>创建适配器</summary>
        public IAdapter Build()
        {
            return this.AdapterInstance.As<IAdapter>();
        }
        /// <summary>保存</summary>
        public void Save(string xmlFile)
        {
            var xdoc = new XDocument();
            xdoc.Declaration = new XDeclaration("1.0", "utf-8", "");
            var root = BuildAdapter();
            xdoc.Add(root);
            xdoc.Save(xmlFile);
        }
        #endregion

        #region 适配器节
        /// <summary>默认节名</summary>
        public string DefalutSectionName { get { return "Adapter"; } }
        /// <summary>适配器实例</summary>
        public IXmlConfigAdapter AdapterInstance { get; set; }
        /// <summary>加载适配器节</summary>
        protected void LoadAdapter(XElement node)
        {
            try
            {
                var adapter = this.AdapterInstance;
                if (adapter == null)
                {
                    adapter = this.Config.LoadComponent<IXmlConfigAdapter>(node);
                    this.AdapterInstance = adapter;
                }
                adapter.AdapterInfo = LoadAdapterInfo(node);
                adapter.MetadataTags = LoadMetadataTags(node, adapter);
                adapter.Connections = this.Config.LoadConnections(node, nameof(adapter.Connections));
                this.Config.Connections = adapter.Connections;
                adapter.Variables = this.Config.LoadVariables(node, nameof(adapter.Variables));
                this.Config.CommonTasks = new Dictionary<string, ETLTask>();
                adapter.Scopes = LoadAdapterScopes(node, nameof(adapter.Scopes));
                adapter.AdapterInfo.Scopes = adapter.Scopes.Select(e => e.ScopeInfo).ToList(new ScopeCollection());
                adapter.Configuration = this;
            }
            finally
            {
                this.Config.Connections = null;
                this.Config.CommonTasks = null;
            }
        }
        /// <summary>生成适配器节</summary>
        protected XElement BuildAdapter()
        {
            var adapter = this.AdapterInstance;
            var root = this.Config.BuildComponent(this.AdapterInstance);
            BuildAdapterInfo(root, adapter.AdapterInfo);
            root.Add(this.Config.BuildConnections(adapter.Connections, nameof(adapter.Connections)));
            root.Add(this.Config.BuildVariables(adapter.Variables, nameof(adapter.Variables)));
            root.Add(BuildAdapterScopes(adapter.Scopes, nameof(adapter.Scopes)));
            root.Add(BuildMetadataTags(adapter.MetadataTags, adapter));
            return root;
        }
        /// <summary>加载适配器信息</summary>
        protected AdapterInfo LoadAdapterInfo(XElement node)
        {
            var info = new AdapterInfo();
            info.Id = this.Config.GetAttributeValue_Name(node);
            info.DataSourceName = this.Config.GetAttributeValue(node, nameof(info.DataSourceName).JsonToCamelCase());
            info.DataSourceType = this.Config.GetAttributeValue(node, nameof(info.DataSourceType).JsonToCamelCase());
            return info;
        }
        /// <summary>生成适配器信息</summary>
        protected void BuildAdapterInfo(XElement node, AdapterInfo info)
        {
            this.Config.SetAttributeValue_Name(node, info.Id);
            this.Config.SetAttributeValue(node, nameof(info.DataSourceName).JsonToCamelCase(), info.DataSourceName);
            this.Config.SetAttributeValue(node, nameof(info.DataSourceType).JsonToCamelCase(), info.DataSourceType);
        }
        #endregion

        #region 适配器变量
        /// <summary>变量_适配器名称</summary>
        private string Variable_AdapterName { get { return "AdapterName"; } }
        /// <summary>变量_适配器名称</summary>
        private string Variable_AdapterDataSourceName { get { return "AdapterDataSourceName"; } }
        /// <summary>变量_数据源类型</summary>
        private string Variable_AdapterDataSourceType { get { return "AdapterDataSourceType"; } }
        /// <summary>变量_元数据标签集合</summary>
        private string Variable_MetadataTags { get { return "MetadataTags"; } }
        /// <summary>设置适配器变量</summary>
        public void SetAdapterVariables(IXmlConfigAdapter adapter, Dictionary<string, object> variables)
        {
            variables[Variable_AdapterName] = adapter.AdapterInfo.Id;
            variables[Variable_AdapterDataSourceName] = adapter.AdapterInfo.DataSourceName;
            variables[Variable_AdapterDataSourceType] = adapter.AdapterInfo.DataSourceType;
            variables[Variable_MetadataTags] = adapter.MetadataTags;
        }
        #endregion

        #region 元数据标签
        /// <summary>元数据标签</summary>
        private string XElement_MetadataTags { get { return "MetadataTags"; } }
        /// <summary>元数据标签</summary>
        private string XElement_MetadataTag { get { return "MetadataTag"; } }
        /// <summary>载入元数据标签集合</summary>
        private KMDConfiguration LoadMetadataTags(XElement node, IXmlConfigAdapter adapter)
        {
            KMDConfiguration tags = null;
            try
            {
                var kmd = new KMD();
                var kmdConfig = KMD.DefaultKmdConfiguration;
                if (!kmdConfig.IsNullOrEmpty())
                {
                    tags = new KMDConfiguration();
                    foreach (var pair in kmdConfig)
                    {
                        tags.Add(pair.Key, pair.Value.MapTo<MetadataTag>());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            if (tags == null)
            {
                var elements = this.Config.LoadElements(node, nameof(adapter.MetadataTags), XElement_MetadataTag, LoadMetadataTag);
                tags = new KMDConfiguration(elements);
            }
            var tagIncrementColumnValue = BuildMetadataTag_IncrementColumnValue();
            tags.Add(tagIncrementColumnValue.Name, tagIncrementColumnValue);
            FlatMetadata.MetadataDefinitions = tags;
            JsonMetadata.InitMetadataDefinitions(tags);
            return tags;
        }
        /// <summary>适配器信息</summary>
        private MetadataTag LoadMetadataTag(XElement node)
        {
            var value = new MetadataTag();
            value.Name = this.Config.GetElementValue(node, nameof(value.Name));
            value.Title = this.Config.GetElementValue(node, nameof(value.Title));
            value.Description = this.Config.GetElementValue(node, nameof(value.Description));
            value.Type = this.Config.GetElementValue_Enum(node, nameof(value.Type), TagType.String);
            value.Required = this.Config.GetElementValue(node, nameof(value.Required)).ToBool();
            value.Enabled = this.Config.GetElementValue(node, nameof(value.Enabled)).ToBool();
            var mappingNode = this.Config.GetElement(node, nameof(value.Mapping));
            value.Mapping = new Mapping();
            value.Mapping.Get = this.Config.GetElementValue(mappingNode, nameof(value.Mapping.Get));
            value.Mapping.Set = new List<Set>();
            var mappingSetsNode = this.Config.GetElement(mappingNode, nameof(value.Mapping.Set) + "s");
            foreach (var mappingSetNode in mappingSetsNode.Elements(nameof(value.Mapping.Set)))
            {
                var mappingSet = new Set();
                mappingSet.Key = this.Config.GetElementValue(mappingSetNode, nameof(mappingSet.Key), mappingSet.Key);
                mappingSet.Value = this.Config.GetElementValue(mappingSetNode, nameof(mappingSet.Value), mappingSet.Value);
                value.Mapping.Set.Add(mappingSet);
            }
            return value;
        }
        /// <summary>生成标签_增量字段值</summary>
        private MetadataTag BuildMetadataTag_IncrementColumnValue()
        {
            var value = new MetadataTag();
            value.Name = nameof(IIncMetadataRow.IncrementColumnValue);
            value.Title = "增量字段值";
            value.Description = string.Empty;
            value.Type = TagType.String;
            value.Required = false;
            value.Enabled = false;
            value.Mapping = new Mapping();
            value.Mapping.Get = value.Name;
            value.Mapping.Set = new List<Set>();
            var mappingSet = new Set();
            mappingSet.Key = value.Name;
            mappingSet.Value = "@value";
            value.Mapping.Set.Add(mappingSet);
            return value;
        }
        /// <summary>生成元数据标签集合</summary>
        public XElement BuildMetadataTags(KMDConfiguration values, IXmlConfigAdapter adapter)
        {
            return BuildMetadataTags(values, nameof(adapter.MetadataTags));
        }
        /// <summary>生成元数据标签集合</summary>
        public XElement BuildMetadataTags(KMDConfiguration values, string nodeName = "")
        {
            if (nodeName.IsNullOrEmpty()) nodeName = this.XElement_MetadataTags;
            return this.Config.BuildElements(values.Values, nodeName, XElement_MetadataTag, BuildMetadataTag);
        }
        /// <summary>生成元数据标签</summary>
        private void BuildMetadataTag(XElement node, MetadataDefinition value)
        {
            var value2 = value.As<MetadataTag>();
            this.Config.SetElementValue(node, nameof(value2.Name), value2.Name);
            this.Config.SetElementValue(node, nameof(value2.Title), value2.Title);
            this.Config.SetElementValue(node, nameof(value2.Description), value2.Description);
            this.Config.SetElementValue(node, nameof(value2.Type), value2.Type.ToString());
            this.Config.SetElementValue(node, nameof(value2.Required), value2.Required.ToString());
            this.Config.SetElementValue(node, nameof(value2.Enabled), value2.Enabled.ToString());
            var mappingNode = this.Config.AddElement(node, nameof(value2.Mapping));
            this.Config.SetElementValue(mappingNode, nameof(value2.Mapping.Get), value2.Mapping.Get);
            var mappingSetsNode = this.Config.AddElement(mappingNode, nameof(value2.Mapping.Set) + "s");
            foreach (var mappingSet in value2.Mapping.Set)
            {
                var mappingSetNode = this.Config.AddElement(mappingSetsNode, nameof(value2.Mapping.Set));
                this.Config.SetElementValue(mappingSetNode, nameof(mappingSet.Key), mappingSet.Key);
                this.Config.SetElementValue(mappingSetNode, nameof(mappingSet.Value), mappingSet.Value);
            }
        }
        #endregion

        #region 适配器域节
        /// <summary>加载适配器域</summary>
        public List<IAdapterScope> LoadAdapterScopes(XElement node, string nodeName)
        {
            return this.Config.LoadElements(node, nodeName, null, LoadAdapterScope);
        }
        /// <summary>默认爬取分页大小</summary>
        public int DefaultSpiderSize { get { return 10; } }
        /// <summary>加载适配器域</summary>
        private IAdapterScope LoadAdapterScope(XElement node)
        {
            try
            {
                var scope = this.Config.LoadComponent<IAdapterScope>(node);
                scope.ScopeInfo = LoadAdapterScopeInfo(node);
                scope.SpiderSize = this.Config.GetElementValue_Int32(node, nameof(scope.SpiderSize), this.DefaultSpiderSize);
                this.Config.OnComponentLoaded = this.OnCommonTaskLoaded;
                scope.CommonTasks = LoadCommonTasks(node, scope);
                this.Config.OnComponentLoaded = null;
                scope.Variables = this.Config.LoadVariables(node, nameof(scope.Variables));
                scope.SpiderCountPackage = LoadPackage(node, nameof(scope.SpiderCountPackage));
                scope.SpiderPackage = LoadPackage(node, nameof(scope.SpiderPackage));
                scope.RetrievePackage = LoadPackage(node, nameof(scope.RetrievePackage));
                scope.GetDataPackage = LoadPackage(node, nameof(scope.GetDataPackage));
                scope.Configuration = this;
                return scope;
            }
            finally
            {
                this.Config.CommonTasks.Clear();
            }
        }
        /// <summary>生成适配器域</summary>
        public XElement BuildAdapterScopes(List<IAdapterScope> scopes, string nodeName)
        {
            return this.Config.BuildElements(scopes, nodeName, "Scope", BuildAdapterScope);
        }
        /// <summary>加载适配器域</summary>
        private void BuildAdapterScope(XElement node, IAdapterScope scope)
        {
            this.Config.BuildComponent(node, scope);
            BuildAdapterScopeInfo(node, scope.ScopeInfo);
            this.Config.SetElementValue(node, nameof(scope.SpiderSize), scope.SpiderSize.ToString());
            node.Add(this.Config.BuildVariables(scope.Variables, nameof(scope.Variables)));
            node.Add(BuildCommonTasks(scope));
            node.Add(BuildPackage(scope.SpiderCountPackage, nameof(scope.SpiderCountPackage)));
            node.Add(BuildPackage(scope.SpiderPackage, nameof(scope.SpiderPackage)));
            node.Add(BuildPackage(scope.RetrievePackage, nameof(scope.RetrievePackage)));
            node.Add(BuildPackage(scope.GetDataPackage, nameof(scope.GetDataPackage)));
        }
        /// <summary>加载适配器域信息</summary>
        private Scope LoadAdapterScopeInfo(XElement node)
        {
            var info = new Scope();
            info.Name = this.Config.GetAttributeValue_Name(node);
            info.IncrementType = this.Config.GetAttributeValue_Enum(node, nameof(info.IncrementType).JsonToCamelCase(), IncrementType.None);
            return info;
        }
        /// <summary>生成适配器域信息</summary>
        private void BuildAdapterScopeInfo(XElement node, Scope info)
        {
            this.Config.SetAttributeValue_Name(node, info.Name);
            this.Config.SetAttributeValue(node, nameof(info.IncrementType).JsonToCamelCase(), info.IncrementType.ToString());
        }
        #endregion

        #region 公共任务方法
        /// <summary>加载任务集合_公共任务集合</summary>
        public Dictionary<string, ETLTask> LoadCommonTasks(XElement node, IAdapterScope scope)
        {
            return this.Config.LoadTasks(node, nameof(scope.CommonTasks))
                .ToDictionary(e => e.Name, StringComparer.OrdinalIgnoreCase);
        }
        /// <summary>处理公共任务加载</summary>
        public void OnCommonTaskLoaded(object component)
        {
            var task = component.As<ETLTask>();
            if (task != null) this.Config.OnCommonTaskLoaded(task);
        }
        /// <summary>生成任务集合_公共任务集合</summary>
        public XElement BuildCommonTasks(IAdapterScope scope)
        {
            return this.Config.BuildTasks(scope.CommonTasks.Values, nameof(scope.CommonTasks));
        }
        #endregion

        #region 包方法
        /// <summary>加载包</summary>
        private ETLPackage LoadPackage(XElement node, string nodeName)
        {
            var package = new ETLPackage();
            package.TaskContainer.LoadXml(this.Config, node.Element(nodeName));
            return package;
        }
        /// <summary>生成包</summary>
        private XElement BuildPackage(ETLPackage package, string nodeName)
        {
            var node = this.Config.AddElement(null, nodeName);
            package.TaskContainer.BuildXml(this.Config, node);
            return node;
        }
        #endregion

        #region 适配器域变量
        /// <summary>变量_域名称</summary>
        private string Variable_ScopeName { get { return "ScopeName"; } }
        /// <summary>变量_增量类型</summary>
        private string Variable_IncrementType { get { return "ScopeIncrementType"; } }
        /// <summary>变量_增量值</summary>
        private string Variable_IncrementValue { get { return "ScopeIncrementValue"; } }
        /// <summary>变量_分页值</summary>
        private string Variable_Pager { get { return "ScopePager"; } }
        /// <summary>变量_NatureKey</summary>
        private string Variable_NatureKey { get { return "MT_NatureKey"; } }
        /// <summary>变量_Ticket</summary>
        private string Variable_Ticket { get { return "ED_Ticket"; } }
        /// <summary>设置适配器域变量</summary>
        public void SetAdapterScopeVariables(Dictionary<string, object> variables, IAdapterScope scope)
        {
            variables[Variable_ScopeName] = scope.ScopeInfo.Name;
            variables[Variable_IncrementType] = scope.ScopeInfo.IncrementType;
        }
        /// <summary>设置适配器域变量</summary>
        public void SetAdapterScopeVariable_IncrementValue(Dictionary<string, object> variables, object incrementValue)
        {
            variables[Variable_IncrementValue] = incrementValue;
        }
        /// <summary>设置适配器域变量</summary>
        public void SetAdapterScopeVariable_Pager(Dictionary<string, object> variables, Pager pager)
        {
            variables[Variable_Pager] = pager;
        }
        /// <summary>设置适配器域变量</summary>
        public void SetAdapterScopeVariable_NatureKey(Dictionary<string, object> variables, string natureKey)
        {
            variables[Variable_NatureKey] = natureKey;
        }
        /// <summary>设置适配器域变量</summary>
        public void SetAdapterScopeVariable_Ticket(Dictionary<string, object> variables, string ticket)
        {
            variables[Variable_Ticket] = ticket;
        }
        #endregion
    }
}
