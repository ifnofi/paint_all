
using UnityEngine;

namespace LFramework
{
    public static class UnityEngineVectorExtension 
    {
        /// <summary>
        /// 获取从自身到目标组件的方向向量。
        /// </summary>
        public static Vector3 DirectionTo(this Component self, Component to) =>
            to.transform.position - self.transform.position;

        /// <summary>
        /// 获取从自身到目标游戏对象的方向向量。
        /// </summary>
        public static Vector3 DirectionTo(this GameObject self, GameObject to) =>
            to.transform.position - self.transform.position;

        /// <summary>
        /// 获取从自身到目标游戏对象的方向向量。
        /// </summary>
        public static Vector3 DirectionTo(this Component self, GameObject to) =>
            to.transform.position - self.transform.position;

        /// <summary>
        /// 获取从自身到目标组件的方向向量。
        /// </summary>
        public static Vector3 DirectionTo(this GameObject self, Component to) =>
            to.transform.position - self.transform.position;

        /// <summary>
        /// 获取从来源组件到自身的方向向量。
        /// </summary>
        public static Vector3 DirectionFrom(this Component self, Component from) =>
            self.transform.position - from.transform.position;

        /// <summary>
        /// 获取从来源游戏对象到自身的方向向量。
        /// </summary>
        public static Vector3 DirectionFrom(this GameObject self, GameObject from) =>
            self.transform.position - from.transform.position;

        /// <summary>
        /// 获取从来源组件到自身的方向向量。
        /// </summary>
        public static Vector3 DirectionFrom(this GameObject self, Component from) =>
            self.transform.position - from.transform.position;

        /// <summary>
        /// 获取从来源游戏对象到自身的方向向量。
        /// </summary>
        public static Vector3 DirectionFrom(this Component self, GameObject from) =>
            self.transform.position - from.transform.position;

        /// <summary>
        /// 获取从自身到目标组件的归一化方向向量。
        /// </summary>
        public static Vector3 NormalizedDirectionTo(this Component self, Component to) =>
            self.DirectionTo(to).normalized;

        /// <summary>
        /// 获取从自身到目标游戏对象的归一化方向向量。
        /// </summary>
        public static Vector3 NormalizedDirectionTo(this GameObject self, GameObject to) =>
            self.DirectionTo(to).normalized;

        /// <summary>
        /// 获取从自身到目标游戏对象的归一化方向向量。
        /// </summary>
        public static Vector3 NormalizedDirectionTo(this Component self, GameObject to) =>
            self.DirectionTo(to).normalized;

        /// <summary>
        /// 获取从自身到目标组件的归一化方向向量。
        /// </summary>
        public static Vector3 NormalizedDirectionTo(this GameObject self, Component to) =>
            self.DirectionTo(to).normalized;

        /// <summary>
        /// 获取从来源组件到自身的归一化方向向量。
        /// </summary>
        public static Vector3 NormalizedDirectionFrom(this Component self, Component from) =>
            self.DirectionFrom(from).normalized;

        /// <summary>
        /// 获取从来源游戏对象到自身的归一化方向向量。
        /// </summary>
        public static Vector3 NormalizedDirectionFrom(this GameObject self, GameObject from) =>
            self.DirectionFrom(from).normalized;

        /// <summary>
        /// 获取从来源组件到自身的归一化方向向量。
        /// </summary>
        public static Vector3 NormalizedDirectionFrom(this GameObject self, Component from) =>
            self.DirectionFrom(from).normalized;

        /// <summary>
        /// 获取从来源游戏对象到自身的归一化方向向量。
        /// </summary>
        public static Vector3 NormalizedDirectionFrom(this Component self, GameObject from) =>
            self.DirectionFrom(from).normalized;

        /// <summary>
        /// 获取从自身到目标组件的二维方向向量（忽略z轴）。
        /// </summary>
        public static Vector2 Direction2DTo(this Component self, Component to) =>
            to.transform.position - self.transform.position;

        /// <summary>
        /// 获取从自身到目标游戏对象的二维方向向量（忽略z轴）。
        /// </summary>
        public static Vector2 Direction2DTo(this GameObject self, GameObject to) =>
            to.transform.position - self.transform.position;

        /// <summary>
        /// 获取从自身到目标游戏对象的二维方向向量（忽略z轴）。
        /// </summary>
        public static Vector2 Direction2DTo(this Component self, GameObject to) =>
            to.transform.position - self.transform.position;

        /// <summary>
        /// 获取从自身到目标组件的二维方向向量（忽略z轴）。
        /// </summary>
        public static Vector2 Direction2DTo(this GameObject self, Component to) =>
            to.transform.position - self.transform.position;

        /// <summary>
        /// 获取从来源组件到自身的二维方向向量（忽略z轴）。
        /// </summary>
        public static Vector2 Direction2DFrom(this Component self, Component from) =>
            self.transform.position - from.transform.position;

        /// <summary>
        /// 获取从来源游戏对象到自身的二维方向向量（忽略z轴）。
        /// </summary>
        public static Vector2 Direction2DFrom(this GameObject self, GameObject from) =>
            self.transform.position - from.transform.position;

        /// <summary>
        /// 获取从来源组件到自身的二维方向向量（忽略z轴）。
        /// </summary>
        public static Vector2 Direction2DFrom(this GameObject self, Component from) =>
            self.transform.position - from.transform.position;

        /// <summary>
        /// 获取从来源游戏对象到自身的二维方向向量（忽略z轴）。
        /// </summary>
        public static Vector2 Direction2DFrom(this Component self, GameObject from) =>
            self.transform.position - from.transform.position;

        /// <summary>
        /// 获取从自身到目标组件的归一化二维方向向量。
        /// </summary>
        public static Vector2 NormalizedDirection2DTo(this Component self, Component to) =>
            self.Direction2DTo(to).normalized;

        /// <summary>
        /// 获取从自身到目标游戏对象的归一化二维方向向量。
        /// </summary>
        public static Vector2 NormalizedDirection2DTo(this GameObject self, GameObject to) =>
            self.Direction2DTo(to).normalized;

        /// <summary>
        /// 获取从自身到目标游戏对象的归一化二维方向向量。
        /// </summary>
        public static Vector2 NormalizedDirection2DTo(this Component self, GameObject to) =>
            self.Direction2DTo(to).normalized;

        /// <summary>
        /// 获取从自身到目标组件的归一化二维方向向量。
        /// </summary>
        public static Vector2 NormalizedDirection2DTo(this GameObject self, Component to) =>
            self.Direction2DTo(to).normalized;

        /// <summary>
        /// 获取从来源组件到自身的归一化二维方向向量。
        /// </summary>
        public static Vector2 NormalizedDirection2DFrom(this Component self, Component from) =>
            self.Direction2DFrom(from).normalized;

        /// <summary>
        /// 获取从来源游戏对象到自身的归一化二维方向向量。
        /// </summary>
        public static Vector2 NormalizedDirection2DFrom(this GameObject self, GameObject from) =>
            self.Direction2DFrom(from).normalized;

        /// <summary>
        /// 获取从来源组件到自身的归一化二维方向向量。
        /// </summary>
        public static Vector2 NormalizedDirection2DFrom(this GameObject self, Component from) =>
            self.Direction2DFrom(from).normalized;

        /// <summary>
        /// 获取从来源游戏对象到自身的归一化二维方向向量。
        /// </summary>
        public static Vector2 NormalizedDirection2DFrom(this Component self, GameObject from) =>
            self.Direction2DFrom(from).normalized;

        /// <summary>
        /// 将Vector3转换为Vector2（丢弃z轴）。
        /// </summary>
        public static Vector2 ToVector2(this Vector3 self) => new Vector2(self.x, self.y);

        /// <summary>
        /// 将Vector2转换为Vector3，可指定z轴。
        /// </summary>
        public static Vector3 ToVector3(this Vector2 self, float z = 0)
        {
            return new Vector3(self.x, self.y, z);
        }
        
        /// <summary>
        /// 设置Vector3的x分量并返回新向量。
        /// </summary>
        public static Vector3 X(this Vector3 self,float x)
        {
            self.x = x;
            return self;
        }
        
        /// <summary>
        /// 设置Vector3的y分量并返回新向量。
        /// </summary>
        public static Vector3 Y(this Vector3 self,float y)
        {
            self.y = y;
            return self;
        }
        
        /// <summary>
        /// 设置Vector3的z分量并返回新向量。
        /// </summary>
        public static Vector3 Z(this Vector3 self,float z)
        {
            self.z = z;
            return self;
        }
        
        /// <summary>
        /// 设置Vector2的x分量并返回新向量。
        /// </summary>
        public static Vector2 X(this Vector2 self,float x)
        {
            self.x = x;
            return self;
        }
        
        /// <summary>
        /// 设置Vector2的y分量并返回新向量。
        /// </summary>
        public static Vector2 Y(this Vector2 self,float y)
        {
            self.y = y;
            return self;
        }

        /// <summary>
        /// 计算两个GameObject之间的三维距离。
        /// </summary>
        public static float Distance(this GameObject self, GameObject other)
        {
            return Vector3.Distance(self.Position(), other.Position());
        }
        
        /// <summary>
        /// 计算Component与GameObject之间的三维距离。
        /// </summary>
        public static float Distance(this Component self, GameObject other)
        {
            return Vector3.Distance(self.Position(), other.Position());
        }
        
        /// <summary>
        /// 计算GameObject与Component之间的三维距离。
        /// </summary>
        public static float Distance(this GameObject self, Component other)
        {
            return Vector3.Distance(self.Position(), other.Position());
        }
        
        /// <summary>
        /// 计算两个Component之间的三维距离。
        /// </summary>
        public static float Distance(this Component self, Component other)
        {
            return Vector3.Distance(self.Position(), other.Position());
        }
        
        /// <summary>
        /// 计算两个GameObject之间的二维距离。
        /// </summary>
        public static float Distance2D(this GameObject self, GameObject other)
        {
            return Vector2.Distance(self.Position2D(), other.Position2D());
        }
        
        /// <summary>
        /// 计算Component与GameObject之间的二维距离。
        /// </summary>
        public static float Distance2D(this Component self, GameObject other)
        {
            return Vector2.Distance(self.Position2D(), other.Position2D());
        }
        
        /// <summary>
        /// 计算GameObject与Component之间的二维距离。
        /// </summary>
        public static float Distance2D(this GameObject self, Component other)
        {
            return Vector2.Distance(self.Position2D(), other.Position2D());
        }
        
        /// <summary>
        /// 计算两个Component之间的二维距离。
        /// </summary>
        public static float Distance2D(this Component self, Component other)
        {
            return Vector2.Distance(self.Position2D(), other.Position2D());
        }
        
        /// <summary>
        /// 计算两个GameObject之间的本地三维距离。
        /// </summary>
        public static float LocalDistance(this GameObject self, GameObject other)
        {
            return Vector3.Distance(self.LocalPosition(), other.LocalPosition());
        }
        
        /// <summary>
        /// 计算Component与GameObject之间的本地三维距离。
        /// </summary>
        public static float LocalDistance(this Component self, GameObject other)
        {
            return Vector3.Distance(self.LocalPosition(), other.LocalPosition());
        }
        
        /// <summary>
        /// 计算GameObject与Component之间的本地三维距离。
        /// </summary>
        public static float LocalDistance(this GameObject self, Component other)
        {
            return Vector3.Distance(self.LocalPosition(), other.LocalPosition());
        }
        
        /// <summary>
        /// 计算两个Component之间的本地三维距离。
        /// </summary>
        public static float LocalDistance(this Component self, Component other)
        {
            return Vector3.Distance(self.LocalPosition(), other.LocalPosition());
        }
        
        /// <summary>
        /// 计算两个GameObject之间的本地二维距离。
        /// </summary>
        public static float LocalDistance2D(this GameObject self, GameObject other)
        {
            return Vector2.Distance(self.LocalPosition2D(), other.LocalPosition2D());
        }
        
        /// <summary>
        /// 计算Component与GameObject之间的本地二维距离。
        /// </summary>
        public static float LocalDistance2D(this Component self, GameObject other)
        {
            return Vector2.Distance(self.LocalPosition2D(), other.LocalPosition2D());
        }
        
        /// <summary>
        /// 计算GameObject与Component之间的本地二维距离。
        /// </summary>
        public static float LocalDistance2D(this GameObject self, Component other)
        {
            return Vector2.Distance(self.LocalPosition2D(), other.LocalPosition2D());
        }
        
        /// <summary>
        /// 计算两个Component之间的本地二维距离。
        /// </summary>
        public static float LocalDistance2D(this Component self, Component other)
        {
            return Vector2.Distance(self.LocalPosition2D(), other.LocalPosition2D());
        }
    }
}
