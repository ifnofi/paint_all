using System;
using System.Collections.Generic;
using System.IO;

public static class IniTool
{
    private static readonly Dictionary<string, Ini> IniDic = new Dictionary<string, Ini>();


    public static string GetValue(string title, string key, string path, string def = "")
    {
        if (IniDic.TryGetValue(path, out var value))
        {
            return value.GetValue(title, key, def);
        }
        else
        {
            var ininew = new Ini(path);

            IniDic.Add(path, ininew);

            return ininew.GetValue(title, key, def);
        }
    }


    internal static Dictionary<string, string> GetValueGroup(string title, string path)
    {
        var group = new Dictionary<string, string>();

        if (IniDic.ContainsKey(path))
        {
            var keys = IniDic[path].GetKeys(title);

            foreach (var k in keys)
            {
                group.Add(k, IniDic[path].GetValue(title, k));
            }

            return group;
        }
        else
        {
            var iniNew = new Ini(path);

            IniDic.Add(path, iniNew);

            var keys = IniDic[path].GetKeys(title);

            foreach (var k in keys)
            {
                group.Add(k, IniDic[path].GetValue(title, k));
            }

            return group;
        }
    }


    public static void SetValue(string title, string key, string value, string path)
    {
        if (IniDic.TryGetValue(path, out var value1))
        {
            value1.SetValue(title, key, value);
        }
        else
        {
            var iniNew = new Ini(path);

            IniDic.Add(path, iniNew);

            IniDic[path].SetValue(title, key, value);
        }

        IniDic[path].Save();
    }


    internal static void SetValue(string title, Dictionary<string, string> keyGroup, string path)
    {
        if (IniDic.TryGetValue(path, out var value))
        {
            foreach (var item in keyGroup)
            {
                value.SetValue(title, item.Key, item.Value);
            }
        }
        else
        {
            var iniNew = new Ini(path);

            IniDic.Add(path, iniNew);

            foreach (var item in keyGroup)
            {
                IniDic[path].SetValue(title, item.Key, item.Value);
            }
        }

        IniDic[path].Save();
    }

    internal static void Remove(string title, string path)
    {
        if (IniDic.TryGetValue(path, out var value))
        {
            value.Remove(title);
        }
        else
        {
            var iniNew = new Ini(path);

            IniDic.Add(path, iniNew);

            IniDic[path].Remove(title);
        }

        IniDic[path].Save();
    }


    internal static List<string> GetTitles(string path)
    {
        if (IniDic.TryGetValue(path, out var value))
        {
            return value.GetTitles();
        }
        else
        {
            var iniNew = new Ini(path);
            IniDic.Add(path, iniNew);
            return IniDic[path].GetTitles();
        }
    }

    internal static void Remove(string path, List<string> titles)
    {
        if (IniDic.TryGetValue(path, out var value))
        {
            value.Remove(titles);
        }
        else
        {
            var iniNew = new Ini(path);
            IniDic.Add(path, iniNew);
            IniDic[path].Remove(titles);
        }

        IniDic[path].Save();
    }
}


public class Ini
{
    /// <summary>
    /// 数据信息
    /// </summary>
    private Dictionary<string, Dictionary<string, string>> ini = new Dictionary<string, Dictionary<string, string>>();

    /// <summary>
    /// 存储路径
    /// </summary>
    public string file;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="file"></param>
    public Ini(string file)
    {
        this.file = file;

        Load();
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void Load()
    {
        if (!File.Exists(file))
        {
            Console.WriteLine($"ini 文件不存在 path={file}");
            return;
        }


        using (StreamReader reader = new StreamReader(file))
        {
            string section = "";
            Dictionary<string, string> sectionData = new Dictionary<string, string>();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine().Trim();

                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    // 开始一个新的节
                    section = line.Substring(1, line.Length - 2);
                    sectionData = new Dictionary<string, string>();
                    ini[section] = sectionData;
                }
                else if (line.Contains("="))
                {
                    // 解析键值对
                    string[] parts = line.Split('=');
                    string key = parts[0].Trim();
                    string value = parts[1].Trim();
                    sectionData[key] = value;
                }
            }
        }
    }

    /// <summary>
    /// 获得当前 ini 的某条数据
    /// </summary>
    /// <param name="title">title</param>
    /// <param name="key">key</param>
    /// <returns></returns>
    internal string GetValue(string title, string key, string def = "")
    {
        if (ini.ContainsKey(title))
        {
            if (ini[title].ContainsKey(key))
            {
                return ini[title][key];
            }
            else
            {
                Console.WriteLine("GetValue: key 不存在");
                ini[title].Add(key, def);
                Save();
                return def;
            }
        }

        Console.WriteLine("GetValue: title不存在");

        ini.Add(title, new Dictionary<string, string>());
        ini[title].Add(key, def);
        Save();
        return def;
    }


    /// <summary>
    /// 获得当前 title 的所有 key 的名字集合
    /// </summary>
    /// <param name="title">title</param>
    /// <returns></returns>
    internal List<string> GetKeys(string title)
    {
        var keys = new List<string>();
        foreach (var data in ini)
        {
            if (data.Key == title)
            {
                foreach (var key in data.Value)
                {
                    keys.Add(key.Key);
                }

                return keys;
            }
        }

        Console.WriteLine("GetValue: title 不存在");
        return keys;
    }

    /// <summary>
    /// 设置当前 ini 的某条数据
    /// </summary>
    /// <param name="title">title</param>
    /// <param name="key">k</param>
    /// <param name="value">v</param>
    internal void SetValue(string title, string key, string value)
    {
        if (ini.ContainsKey(title))
        {
            if (ini[title].ContainsKey(key))
            {
                ini[title][key] = value;
                return;
            }
            else
            {
                Console.WriteLine("GetValue: key 不存在");
                ini[title].Add(key, value);
                return;
            }
        }

        Console.WriteLine("GetValue: title不存在");

        var dic = new Dictionary<string, string>();
        dic.Add(key, value);
        ini.Add(title, dic);
    }

    /// <summary>
    /// 保存当前 ini 数据内容到本地
    /// </summary>
    public void Save()
    {
        //JsonLitTool.SaveToJson(ini, file);
        lock (ini)
        {
            var profiles = new List<string>();
            foreach (var item in ini)
            {
                string title = "[" + item.Key + "]";
                profiles.Add(title);
                foreach (var kv in item.Value)
                {
                    string line = kv.Key + "=" + kv.Value;
                    profiles.Add(line);
                }

                profiles.Add("");
            }

            File.WriteAllLines(file, profiles.ToArray());
        }
    }

    /// <summary>
    /// 移除某个 title 的所有 kv对
    /// </summary>
    /// <param name="title">某个title</param>
    internal void Remove(string title)
    {
        if (ini.ContainsKey(title))
        {
            ini.Remove(title);
            return;
        }

        Console.WriteLine("Remove: title 不存在");
        return;
    }

    /// <summary>
    /// 移除某个 title 下的 key-value 对
    /// </summary>
    /// <param name="title">某个title</param>
    /// <param name="key">某个key</param>
    internal void Remove(string title, string key)
    {
        if (ini.ContainsKey(title))
        {
            if (ini[title].ContainsKey(key))
            {
                ini[title].Remove(key);
            }
            else
            {
                Console.WriteLine("Remove: key 不存在");
            }

            return;
        }

        Console.WriteLine("Remove: title 不存在");
        return;
    }

    /// <summary>
    /// 移除一些 title 的 kv 对
    /// </summary>
    /// <param name="titles">titles集合</param>
    internal void Remove(List<string> titles)
    {
        foreach (var str in titles)
        {
            Remove(str);
        }
    }

    /// <summary>
    /// 清除
    /// </summary>
    internal void Remove()
    {
        ini.Clear();
    }

    /// <summary>
    /// 获取所有的 title 名字的集合
    /// </summary>
    /// <returns></returns>
    internal List<string> GetTitles()
    {
        var titles = new List<string>();
        foreach (var data in ini)
        {
            titles.Add(data.Key);
        }

        return titles;
    }
}