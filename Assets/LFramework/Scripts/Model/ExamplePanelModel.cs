using System.Collections.Generic;
using LFramework;

namespace Model
{
    /// <summary>
    /// 数据层 可以定义 :  一个面板一个Model数据  数据可以有多个  可以使用数据绑定
    /// </summary>
    public class ExamplePanelModel : AbstractModel
    {
        public BindableProperty<int> ExampleInt { get; set; } = new BindableProperty<int>(0);
        public string ExampleData { get; set; } = string.Empty;
        public List<T2dData> T2dDataList { get; set; } = new List<T2dData>();

        protected override void OnInit()
        {
            // 工具层进行赋值
            // ExampleInt.Value = this.GetUtility<>()
            // 在Panel里面监听 这里是样例
            ExampleInt.Register((a) =>
            {
                //注册监听
            });
        }
    }
}