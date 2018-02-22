using System.Collections.Generic;
using Jurassic.So.Infrastructure;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace Jurassic.So.GeoTopic.SubmissionTool.Services
{
    /// <summary>Excel工具</summary>
    public static class ExcelUtil
    {
        /// <summary>加入一行</summary>
        public static void AddRow(this ISheet sheet, int rowIndex, List<CellProperty> cellData, bool setWidth)
        {
            var row = sheet.CreateRow(rowIndex);
            var colIndex = 0;
            for (int i = 0; i < cellData.Count; i++)
            {
                var property = cellData[i];
                var cell = row.CreateCell(colIndex++);
                if (property.Value.GetType() == typeof(int))
                {
                    cell.SetCellType(CellType.Numeric);
                    cell.SetCellValue((int)property.Value);
                }
                else
                {
                    cell.SetCellType(CellType.String);
                    cell.SetCellValue(property.Value.ToString());
                }
                if (setWidth)
                {
                    var width = property.Widths == null ? property.Width : property.Widths[0];
                    width = (int)(width * 13.57 * 2.56);
                    sheet.SetColumnWidth(cell.ColumnIndex, width);
                }
                if (property.Span > 1)
                {
                    for (int j = 1; j < property.Span; j++)
                    {
                        var cell2 = row.CreateCell(colIndex++);
                        cell2.SetCellType(CellType.String);
                        if (setWidth)
                        {
                            var width = property.Widths[j];
                            width = (int)(width * 13.57 * 2.56);
                            sheet.SetColumnWidth(cell2.ColumnIndex, width);
                        }
                    }
                    var region = new CellRangeAddress(row.RowNum, row.RowNum, cell.ColumnIndex, cell.ColumnIndex + property.Span - 1);
                    sheet.AddMergedRegion(region);
                }
            }
        }
        /// <summary>生成合并单元格</summary>
        public static void BuildMergedRegion(this ISheet sheet, int firstRow, int lastRow, int firstCol, int lastCol)
        {
            var region = new CellRangeAddress(firstRow, lastRow, firstCol, lastCol);
            sheet.AddMergedRegion(region);
        }
        /// <summary>设置标题风格</summary>
        public static void SetTitleStyle(this ISheet sheet, int firstRow, int lastRow)
        {
            var style = sheet.Workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            var font = style.GetFont(sheet.Workbook);
            var newFont = sheet.Workbook.CreateFont();
            newFont.FontName = font.FontName;
            //newFont.Charset = font.Charset;
            //newFont.Color = font.Color;
            //newFont.FontHeight = font.FontHeight + 2;
            newFont.FontHeightInPoints = (short)(font.FontHeightInPoints + 2);
            //newFont.IsItalic = font.IsItalic;
            //newFont.IsStrikeout = font.IsStrikeout;
            newFont.Boldweight = (short)FontBoldWeight.Bold;
            style.SetFont(newFont);
            for (int i = firstRow; i <= lastRow; i++)
            {
                sheet.GetRow(i).Cells.ForEach(e => e.CellStyle = style);
            }
            //if (setBorder)
            //{
            //    RegionUtil.SetBorderLeft((int)BorderStyle.Thin, region, sheet, sheet.Workbook);
            //    RegionUtil.SetBorderTop((int)BorderStyle.Thin, region, sheet, sheet.Workbook);
            //    RegionUtil.SetBorderRight((int)BorderStyle.Thin, region, sheet, sheet.Workbook);
            //    RegionUtil.SetBorderBottom((int)BorderStyle.Thin, region, sheet, sheet.Workbook);
            //}
        }
        /// <summary>设置数字风格</summary>
        public static void SetNumberStyle(this ISheet sheet, int firstRow, int column)
        {
            var style = sheet.Workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            var lastRow = 65535;
            for (int i = firstRow; i <= lastRow; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) break;
                var cell = row.GetCell(column);
                if (cell == null)
                {
                    cell = row.CreateCell(column);
                }
                cell.SetCellType(CellType.Numeric);
                //cell.SetCellValue((string)null);
                cell.CellStyle = style;
            }
        }
        /// <summary>设置日期风格</summary>
        public static void SetDateTimeStyle(this ISheet sheet, int firstRow, int column)
        {
            var style = sheet.Workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Left;
            var format = sheet.Workbook.CreateDataFormat();
            style.DataFormat = format.GetFormat(DataTimeUtil.StandardFormat);
            var lastRow = 65535;
            for (int i = firstRow; i <= lastRow; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) break;
                var cell = row.GetCell(column);
                if (cell == null)
                {
                    cell = row.CreateCell(column);
                }
                cell.SetCellType(CellType.Numeric);
                cell.SetCellValue((string)null);
                cell.CellStyle = style;
            }
        }
        /// <summary>设置列下拉列表</summary>
        public static void SetColumnDropdownList(this ISheet sheet, string[] values, int startRow, int column)
        {
            var regions = new CellRangeAddressList(startRow, 65535, column, column);
            var constraint = DVConstraint.CreateExplicitListConstraint(values);
            var validation = new HSSFDataValidation(regions, constraint);
            validation.CreateErrorBox("输入不合法", "请输入下拉列表中的值。");
            validation.ShowPromptBox = true;
            sheet.AddValidationData(validation);
        }
    }
}
