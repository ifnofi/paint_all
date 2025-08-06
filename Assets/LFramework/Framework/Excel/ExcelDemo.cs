using System.IO;
using OfficeOpenXml;
using UnityEngine;

namespace LFramework.Excel
{
    public class ExcelDemo : MonoBehaviour
    {
        private void Start()
        {
            using (var excel = new ExcelPackage(new FileInfo(Application.streamingAssetsPath + "/Test.xlsx")))
            {
                var worksheet = excel.Workbook.Worksheets[0];
                worksheet.Cells[1, 1].Value = "Hello World";
                worksheet.Cells[1, 2].Value = "Hello World";
                worksheet.Cells[2, 1].Value = "Hello World";
                worksheet.Cells[2, 2].Value = "Hello World";
                var str = worksheet.GetMergeValue(1, 1);
                excel.SaveAs(new FileInfo(Application.streamingAssetsPath + "/Test2.xlsx"));
            }
        }
    }
}