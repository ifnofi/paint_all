using LFramework;

namespace Command
{
    public class ExampleStartGameCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            // 发送事件或执行其他逻辑
            TypeEventSystem.Global.Send<ExampleStartGameEvent>();
        }
    }

    public struct ExampleStartGameEvent
    {
    }
}