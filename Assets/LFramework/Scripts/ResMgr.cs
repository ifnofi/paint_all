using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LFramework;
using LFramework.Excel;
using OfficeOpenXml;

public class ResMgr : MonoSingleton<ResMgr>
{
    public List<ResData> youqiDatas = new List<ResData>();
    public List<ResData> liangshiDatas = new List<ResData>();
    public List<ResData> meitanDatas = new List<ResData>();
    public List<ResData> tiekuangDatas = new List<ResData>();
    public List<ResData> yousejinshuDatas = new List<ResData>();
    public List<ResData> yuanyangDatas = new List<ResData>();

    public Dictionary<string, List<ResData>> all = new Dictionary<string, List<ResData>>();

    public IEnumerator Load()
    {
        all = new Dictionary<string, List<ResData>>();
        var path = PathTool.Path.Combine("Data.xlsx");
        using (var package = new ExcelPackage(new FileInfo(path)))
        {
            yield return LoadSheet(package, "油气", youqiDatas);
            yield return LoadSheet(package, "粮食", liangshiDatas);
            yield return LoadSheet(package, "煤炭", meitanDatas);
            yield return LoadSheet(package, "铁矿石", tiekuangDatas);
            yield return LoadSheet(package, "有色金属", yousejinshuDatas);
            yield return LoadSheet(package, "远洋渔业", yuanyangDatas);
        }
    }


    private IEnumerator LoadSheet(ExcelPackage package, string sheetName, List<ResData> datas)
    {
        datas = new List<ResData>();
        var sheet = package.Workbook.Worksheets[sheetName];
        var strs = sheet.GetMergeValue(1, 1).Split('\n');
        var parentTitle = "";
        var parentTitleEN = "";
        var videos = PathTool.Path.Combine(sheetName).GetVideoPathsByDirPath(true);

        if (strs.Length >= 2)
        {
            parentTitle = strs[0];
            parentTitleEN = strs[1].ToUpper();
        }
        else
        {
            parentTitle = sheet.GetMergeValue(1, 1);
            parentTitleEN = "";
        }

        for (int i = 2; i <= sheet.Dimension.End.Row; i++)
        {
            yield return null;
            var data = new ResData();
            // var lines = sheet.GetMergeValue(i, 2).Split('\n');
            // if (lines.Length >= 2)
            // {
            //     data.title = lines[0];
            //     data.titleEN = lines[1].ToUpper();
            // }
            // else
            {
                data.title = sheet.GetMergeValue(i, 2);
                data.titleEN = "";
            }

            var imagePath = PathTool.Path.Combine(sheetName).Combine(data.title.Replace("\n", ""));
            var images = imagePath.GetImagePathsByDirPath(true);

            if (!imagePath.Combine("图片放这里,循环轮播.txt").IsExistFile())
            {
                imagePath.Combine("图片放这里,循环轮播.txt").CreateFileByPath();
            }

            foreach (var image in images)
            {
                var t2d = image.CreateT2dData();
                data.t2dDatas.Add(t2d);
                TextureMgr.Instance.AppendT2d(t2d);
            }

            data.packages.Add((sheet.GetMergeValue(i, 3), sheet.GetMergeValue(i, 4)));
            data.packages.Add((sheet.GetMergeValue(i, 5), sheet.GetMergeValue(i, 6)));
            data.packages.Add((sheet.GetMergeValue(i, 7), sheet.GetMergeValue(i, 8)));

            data.videos = videos;


            data.parentTitle = parentTitle;
            data.parentTitleEN = parentTitleEN;
            datas.Add(data);
        }

        all.Add(sheetName, datas);
        yield return null;
    }


    [Serializable]
    public class ResData
    {
        /// <summary>
        /// 二级标题
        /// </summary>
        public string parentTitle;

        public string parentTitleEN;

        /// <summary>
        /// 三级标题  二级按钮的名字
        /// </summary>
        public string title;

        public string titleEN;
        public List<(string, string)> packages = new List<(string, string)>();
        public List<string> videos = new List<string>();
        public List<T2dData> t2dDatas = new List<T2dData>();
    }
}