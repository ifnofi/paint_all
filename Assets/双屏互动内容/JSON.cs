using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Dynamic;
using System.Linq;

#if !ANDROID

namespace LFramework
{
    public class JSON
    {
        public static dynamic Parse(string json)
        {
            return new SuiBianJson(json);
        }

        public static string Stringify(dynamic jsonObj)
        {
            if (jsonObj == null)
                return "";
            if (jsonObj is SuiBianJson)
                return jsonObj.Stringify();
            if (jsonObj is string)
                return (string)jsonObj;
            return JsonConvert.SerializeObject(jsonObj);
        }
    }

    /// <summary>
    /// 【随便json】
    /// 出于C#中访问json成员以及相关判断太过繁琐，写了这个工具类
    /// </summary>
    public class SuiBianJson : DynamicObject
    {
        private JToken _token = null;

        private SuiBianJson(JToken token, bool b)
        {
            _token = token;
        }

        public SuiBianJson(string jsonStr)
        {
            _token = JsonConvert.DeserializeObject(jsonStr) as JToken;
        }

        public int Count => null == _token ? 0 : _token.Count();

        public int Length => Count;

        public bool IsArray => _token is JArray;

        public delegate bool ForEachHandler(object key, dynamic value);

        public void ForEach(ForEachHandler handler)
        {
            if (null == _token || handler == null)
                return;
            if (_token is JArray)
            {
                var count = ((JArray)_token).Count;
                for (var i = 0; i < count; i++)
                {
                    var b = handler?.Invoke(i, GetMemberValue(i));
                    if (b == null || b == false)
                        break;
                }
            }
            else if (_token is JObject)
            {
                var obj = ((JObject)_token);
                foreach (var item in obj)
                {
                    var b = handler?.Invoke(item.Key, GetMemberValue(item.Key));
                    if (b == null || b == false)
                        break;
                }
            }
        }

        public dynamic this[object key]
        {
            get => GetMemberValue(key);
            set => SetMemberValue(key, value);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = GetMemberValue(binder.Name);
            return true;
        }

        private dynamic GetMemberValue(object name)
        {
            if (null == _token || name == null)
            {
                return null;
            }

            object v = null;
            if (_token is JArray)
            {
                if (!int.TryParse(name.ToString(), out int index))
                    v = null;
                else
                    v = index < 0 || index >= ((JArray)_token).Count ? null : _token[index];
            }
            else if (_token is JObject)
            {
                v = _token[name];
            }
            else
            {
                try
                {
                    v = _token[name];
                }
                catch
                {
                }
            }

            if (v is JToken)
            {
                return new SuiBianJson((JToken)v, false);
            }
            else
            {
                return new SuiBianJson(null, false);
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            SetMemberValue(binder.Name, value);
            return true;
        }

        private void SetMemberValue(object name, object value)
        {
            if (_token == null)
                return;
            if (value is JToken)
            {
                _token[name] = value as JToken;
            }
            else
            {
                try
                {
                    _token[name] = new JValue(value);
                }
                catch
                {
                    try
                    {
                        //尝试序列化value，再转成JToken写入，如果失败，则写入null
                        var s = JsonConvert.SerializeObject(value);
                        _token[name] = JsonConvert.DeserializeObject<JToken>(s);
                    }
                    catch
                    {
                        _token[name] = null;
                    }
                }
            }
        }

        public override string ToString()
        {
            if (null == _token)
                return string.Empty;
            return this + "";
        }

        public string Stringify()
        {
            if (null == _token)
                return string.Empty;
            return JsonConvert.SerializeObject(_token);
        }

        // 隐式转换到bool
        public static implicit operator bool(SuiBianJson suiBianValue)
        {
            var token = suiBianValue._token;
            if (token == null)
                return false;
            if (token is JValue)
            {
                var v = ((JValue)token).Value;
                //1、null和NaN检测
                if (null == v)
                    return false;
                if (v is double && (double.IsNaN((double)v)))
                    return false;
                if (v is float && float.IsNaN((float)v))
                    return false;
                //2、bool检测
                if (v is bool)
                    return (bool)v;
                //3、零检测
                if ((v is long || v is int || v is byte || v is ulong || v is uint || v is ushort || v is short) && (ulong)v == 0)
                    return false;
                //4、空串检测
                if (v is string && string.IsNullOrEmpty((string)v))
                    return false;
            }


            return true;
        }

        // 隐式转换到string
        public static implicit operator string(SuiBianJson suiBianValue)
        {
            var token = suiBianValue._token;
            if (token == null)
                return "";

            if (token is JValue)
            {
                var v = ((JValue)token).Value;
                if (null == v)
                {
                    return "";
                }
                else
                {
                    return v.ToString() ?? "";
                }
            }

            return "";
        }

        // 隐式转换到double
        public static implicit operator double(SuiBianJson suiBianValue)
        {
            var token = suiBianValue._token;
            if (token == null)
                return 0;
            try
            {
                if (token is JValue)
                {
                    var v = ((JValue)token).Value;
                    if (v is long || v is int || v is byte || v is ulong || v is uint || v is ushort || v is short)
                        return Convert.ToInt64(v);
                    if (v is float || v is double || v is decimal)
                        return (double)Convert.ToDouble(v);
                    return Convert.ToDouble(v);
                }
            }
            catch
            {
            }

            return 0;
        }

        public static implicit operator float(SuiBianJson suiBianValue)
        {
            return (float)(suiBianValue + 0.0);
        }

        public static implicit operator long(SuiBianJson suiBianValue)
        {
            return (long)(suiBianValue + 0.0);
        }

        public static implicit operator int(SuiBianJson suiBianValue)
        {
            return (int)(suiBianValue + 0.0);
        }

        public static implicit operator byte(SuiBianJson suiBianValue)
        {
            return (byte)(suiBianValue + 0.0);
        }

        public static implicit operator short(SuiBianJson suiBianValue)
        {
            return (short)(suiBianValue + 0.0);
        }

        public static implicit operator ulong(SuiBianJson suiBianValue)
        {
            return (ulong)(suiBianValue + 0.0);
        }

        public static implicit operator uint(SuiBianJson suiBianValue)
        {
            return (uint)(suiBianValue + 0.0);
        }

        public static implicit operator ushort(SuiBianJson suiBianValue)
        {
            return (ushort)(suiBianValue + 0.0);
        }
    }
}

#endif