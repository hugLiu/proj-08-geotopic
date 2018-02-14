using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.GeoTopic.DataService.Models;

namespace Jurassic.So.GeoTopic.DataService
{
    public interface IImportParamsService
    {
        /// <summary>
        /// 读取导入知识主题模板文件数据
        /// </summary>
        /// <param name="filePath">模板文件路径</param>
        /// <returns>模板文件集合</returns>
        List<ExcelEntityModel> ReadDataFromImportExcel(string filePath);

        /// <summary>
        /// 新建知识模板文件
        /// </summary>
        /// <param name="filePath">初始文件模板路径</param>
        void ExportExcelTemplate(string filePath);

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileName">文件路径</param>
        void DonwnLoadFile(string fileName);

        /// <summary>
        /// 导出知识链数据至知识模板
        /// </summary>
        /// <param name="filePath">知识模板路径</param>
        /// <param name="kTopicModelList">知识主题树数据集合</param>
        void ExportDataToExcel(string filePath, List<KTopicModel> kTopicModelList);

        //void WriteDataToModel(string filePath, List<KTopicModel> kTopicModelList);
    }
}
