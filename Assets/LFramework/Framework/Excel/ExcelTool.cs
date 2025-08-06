using OfficeOpenXml;

namespace LFramework.Excel
{
    public static class ExcelTool
    {
        /// <summary>
        /// 获取合并的单元格的数据
        /// </summary>
        /// <param name="wSheet"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static string GetMergeValue(this ExcelWorksheet wSheet, int row, int column)
        {
            string range = wSheet.MergedCells[row, column];
            if (range == null)
                if (wSheet.Cells[row, column].Value != null)
                    return wSheet.Cells[row, column].Value.ToString();
                else
                    return "";
            object value = wSheet.Cells[(new ExcelAddress(range)).Start.Row, (new ExcelAddress(range)).Start.Column].Value;
            if (value != null)
                return value.ToString();
            else
                return "";
        }
    }
}