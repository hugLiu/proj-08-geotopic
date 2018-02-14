using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Jurassic.PKS.Service;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.GeoTopic.DataService.Models
{
    [Serializable]
    [DataContract]
    public class KeyValueItem
    {
        [DataMember(Name = "key")]
        public string Key { get; set; }
        [DataMember(Name = "value")]
        public string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    public class GroupItem
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "items")]
        public List<KeyValueItem> Items { get; set; }
    }

    /// <summary>
    /// Detail组件数据模型
    /// </summary>
    [Serializable]
    [DataContract]
    public class DetailDataModel
    {
        public DetailDataModel() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="kmd">元数据</param>
        /// <param name="metadataDefinitions">元数据定义</param>
        public DetailDataModel(KMD kmd, KMDConfiguration metadataDefinitions)
        {
            this.InitModel(kmd, metadataDefinitions);
        }
        /// <summary>
        /// IIId
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        [DataMember(Name = "thumbnailUrl")]
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// 基础信息集
        /// </summary>
        [DataMember(Name = "basicInfo")]
        public List<KeyValueItem> BasicInfo { get; set; }

        /// <summary>
        /// 分组信息集
        /// </summary>
        [DataMember(Name = "groups")]
        public List<GroupItem> Groups { get; set; }

        public void InitModel(KMD kmd, KMDConfiguration metadataDefinitions)
        {
            Id = kmd.IIId;
            ThumbnailUrl = kmd.GetStringProperty("Thumbnail");
            BasicInfo = new List<KeyValueItem>();
            Groups = new List<GroupItem>();
            var basicInfoKeys = metadataDefinitions.Values
                .Where(t => t.Showindetail)
                .OrderBy(t => t.ItemOrder);
            foreach (var basicInfoKey in basicInfoKeys)
            {
                var value = string.Empty;
                if (basicInfoKey.Type == TagType.DateString)
                {
                    //kmd.GetISODateProperty(basicInfoKey.Name)?.As<string>();
                }
                else
                {
                    value = kmd.GetProperty(basicInfoKey.Name).As<string>();
                }
                this.BasicInfo.Add(new KeyValueItem { Key = basicInfoKey.Title, Value = value });
            }
            var groupOrders = metadataDefinitions.Values
                .Where(t => !t.InnerTag)
                .Select(t => t.GroupOrder)
                .Distinct()
                .OrderBy(t => t);
            foreach (var groupOrder in groupOrders)
            {
                var itemKeys = metadataDefinitions.Values
                    .Where(t => t.GroupOrder == groupOrder && !t.InnerTag)
                    .OrderBy(t => t.ItemOrder);
                var groupItem = new GroupItem
                {
                    Name = itemKeys.First().GroupName,
                    Items = new List<KeyValueItem>()
                };
                foreach (var key in itemKeys)
                {
                    var value = string.Empty;
                    if (key.Type == TagType.DateString)
                    {
                        kmd.GetISODateProperty(key.Name)?.As<string>();
                    }
                    else
                    {
                        value = kmd.GetProperty(key.Name)?.As<string>();
                    }
                    groupItem.Items.Add(new KeyValueItem { Key = key.Title, Value = value });
                }
                if (groupItem.Items.Count > 0)
                {
                    this.Groups.Add(groupItem);
                }
            }
        }
    }

}