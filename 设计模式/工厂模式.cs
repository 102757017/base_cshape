using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FactoryVsSwitchDemo
{
    // ============================================================
    // 版本 A：switch 写法
    // ============================================================
    public class ToolA1 { public void Run() => Console.WriteLine("  [A] Tool1 执行"); }
    public class ToolA2 { public void Run() => Console.WriteLine("  [A] Tool2 执行"); }

    public class JobSwitch
    {
        public void Execute(string toolName)
        {
            switch (toolName)
            {
                case "Tool1":
                    new ToolA1().Run();
                    break;
                case "Tool2":
                    new ToolA2().Run();
                    break;
                // 新增 Tool3 时，必须在这里加一个 case
                default:
                    throw new ArgumentException($"未知工具: {toolName}");
            }
        }
    }

    // ============================================================
    // 版本 B：工厂写法
    // ============================================================

    // 统一接口
    public interface ITool { void Run(); }

    // 标记特性
    [AttributeUsage(AttributeTargets.Class)]  //枚举值，表示ToolAttribute只允许贴在类（class）上
    public class ToolAttribute : Attribute    //特性类名以 Attribute 结尾时，使用时可以省略 Attribute 后缀
    {
        public string Name { get; }
        public ToolAttribute(string name) => Name = name;
    }

    // 工具类：自己声明名字
    [Tool("Tool1")]
    public class ToolB1 : ITool { public void Run() => Console.WriteLine("  [B] Tool1 执行"); }

    [Tool("Tool2")]
    public class ToolB2 : ITool { public void Run() => Console.WriteLine("  [B] Tool2 执行"); }

    // 新增 Tool3：只加这个类，其他文件一行都不用改
    [Tool("Tool3")]
    public class ToolB3 : ITool { public void Run() => Console.WriteLine("  [B] Tool3 执行"); }

    // 工厂：一次性扫描注册
    public static class ToolFactory
    {
        private static readonly Dictionary<string, Type> _registry = new();

        static ToolFactory()
        {
            foreach (var t in typeof(ToolFactory).Assembly.GetTypes())
            {
                if (t.IsAbstract || !typeof(ITool).IsAssignableFrom(t)) continue;
                var attr = t.GetCustomAttribute<ToolAttribute>();
                if (attr != null) _registry[attr.Name] = t;
            }
        }

		public static ITool Create(string name)
		{
			if (!_registry.TryGetValue(name, out var type))
				throw new ArgumentException($"未知工具: {name}");

			return (ITool)(Activator.CreateInstance(type)
				?? throw new InvalidOperationException($"无法创建: {name}"));
		}
    }

    // 调用方：只依赖接口
    public class JobFactory
    {
        public void Execute(string toolName) => ToolFactory.Create(toolName).Run();
    }

    // ============================================================
    // 入口
    // ============================================================
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== 版本 A：switch ===");
            var jobA = new JobSwitch();
            jobA.Execute("Tool1");
            jobA.Execute("Tool2");
            try { jobA.Execute("Tool3"); }         // A 版本不认识 Tool3
            catch (Exception e) { Console.WriteLine($"  [A] {e.Message}"); }

            Console.WriteLine();
            Console.WriteLine("=== 版本 B：工厂 ===");
            var jobB = new JobFactory();
            jobB.Execute("Tool1");
            jobB.Execute("Tool2");
            jobB.Execute("Tool3");                 // B 版本自动支持
        }
    }
}