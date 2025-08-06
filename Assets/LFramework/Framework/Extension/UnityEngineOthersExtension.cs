using System.Collections.Generic;
using UnityEngine;

namespace LFramework
{
    /// <summary>
    /// Unity 常用类型扩展方法集合
    /// </summary>
    public static class UnityEngineOthersExtension
    {
        /// <summary>
        /// 从列表中随机获取一个元素
        /// </summary>
        public static T GetRandomItem<T>(this List<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        /// <summary>
        /// 随机获取并移除列表中的一个元素
        /// </summary>
        public static T GetAndRemoveRandomItem<T>(this List<T> list)
        {
            var randomIndex = UnityEngine.Random.Range(0, list.Count);
            var randomItem = list[randomIndex];
            list.RemoveAt(randomIndex);
            return randomItem;
        }

        /// <summary>
        /// 设置 SpriteRenderer 的透明度
        /// </summary>
        public static SpriteRenderer Alpha(this SpriteRenderer self, float alpha)
        {
            var color = self.color;
            color.a = alpha;
            self.color = color;
            return self;
        }

        /// <summary>
        /// 对两个值进行插值（Lerp），self 作为 t
        /// </summary>
        public static float Lerp(this float self, float a, float b)
        {
            return Mathf.Lerp(a, b, self);
        }

        /// <summary>
        /// 取 float 的绝对值
        /// </summary>
        public static float Abs(this float self)
        {
            return Mathf.Abs(self);
        }

        /// <summary>
        /// 取 int 的绝对值
        /// </summary>
        public static float Abs(this int self)
        {
            return Mathf.Abs(self);
        }

        /// <summary>
        /// 取 float 的指数
        /// </summary>
        public static float Exp(this float self)
        {
            return Mathf.Exp(self);
        }

        /// <summary>
        /// 取 int 的指数
        /// </summary>
        public static float Exp(this int self)
        {
            return Mathf.Exp(self);
        }

        /// <summary>
        /// 获取 float 的符号（正负号）
        /// </summary>
        public static float Sign(this float self)
        {
            return Mathf.Sign(self);
        }

        /// <summary>
        /// 获取 int 的符号（正负号）
        /// </summary>
        public static float Sign(this int self)
        {
            return Mathf.Sign(self);
        }

        /// <summary>
        /// 计算 float 的余弦值（弧度制）
        /// </summary>
        public static float Cos(this float self)
        {
            return Mathf.Cos(self);
        }

        /// <summary>
        /// 计算 int 的余弦值（弧度制）
        /// </summary>
        public static float Cos(this int self)
        {
            return Mathf.Cos(self);
        }

        /// <summary>
        /// 计算 float 的正弦值（弧度制）
        /// </summary>
        public static float Sin(this float self)
        {
            return Mathf.Sin(self);
        }

        /// <summary>
        /// 计算 int 的正弦值（弧度制）
        /// </summary>
        public static float Sin(this int self)
        {
            return Mathf.Sin(self);
        }

        /// <summary>
        /// 角度转余弦（角度制）
        /// </summary>
        public static float CosAngle(this float self)
        {
            return Mathf.Cos(self * Mathf.Deg2Rad);
        }

        /// <summary>
        /// 角度转余弦（角度制）
        /// </summary>
        public static float CosAngle(this int self)
        {
            return Mathf.Cos(self * Mathf.Deg2Rad);
        }

        /// <summary>
        /// 角度转正弦（角度制）
        /// </summary>
        public static float SinAngle(this float self)
        {
            return Mathf.Sin(self * Mathf.Deg2Rad);
        }

        /// <summary>
        /// 角度转正弦（角度制）
        /// </summary>
        public static float SinAngle(this int self)
        {
            return Mathf.Sin(self * Mathf.Deg2Rad);
        }

        /// <summary>
        /// 角度转弧度
        /// </summary>
        public static float Deg2Rad(this float self)
        {
            return self * Mathf.Deg2Rad;
        }

        /// <summary>
        /// 角度转弧度
        /// </summary>
        public static float Deg2Rad(this int self)
        {
            return self * Mathf.Deg2Rad;
        }

        /// <summary>
        /// 弧度转角度
        /// </summary>
        public static float Rad2Deg(this float self)
        {
            return self * Mathf.Rad2Deg;
        }

        /// <summary>
        /// 弧度转角度
        /// </summary>
        public static float Rad2Deg(this int self)
        {
            return self * Mathf.Rad2Deg;
        }

        /// <summary>
        /// 角度转二维方向向量
        /// </summary>
        public static Vector2 AngleToDirection2D(this int self)
        {
            return new Vector2(self.CosAngle(), self.SinAngle());
        }

        /// <summary>
        /// 角度转二维方向向量
        /// </summary>
        public static Vector2 AngleToDirection2D(this float self)
        {
            return new Vector2(self.CosAngle(), self.SinAngle());
        }

        /// <summary>
        /// 二维向量转角度
        /// </summary>
        public static float ToAngle(this Vector2 self)
        {
            return Mathf.Atan2(self.y, self.x).Rad2Deg();
        }
    }

    public static class RandomUtility
    {
        /// <summary>
        /// 随机选择 传递的 参数中任意一个
        /// var result = RandomUtility.Choose(1,1,1,2,2,2,2,3,3);
        /// </summary>
        /// <param name="args"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Choose<T>(params T[] args)
        {
            return args[UnityEngine.Random.Range(0, args.Length)];
        }
    }
}