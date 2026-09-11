using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AttributeDemo
{
    // ====================特性是用来记录保存元数据的=======================
    // 第一步：定义一个自定义特性
    // ============================================================
    //
    // 要点：
    //   1. 特性类必须继承自 System.Attribute
    //   2. 类名约定以 Attribute 结尾（使用时可以省略后缀）
    //   3. [AttributeUsage] 用来限定这个特性可以贴在哪些地方

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ToolAttribute : Attribute
    {
        // 只读属性：保存工具的名字
        // { get; } 表示构造时赋值一次，之后不允许修改
        public string Name { get; }

        // 可选属性：给个默认值，构造后仍可通过命名参数赋值
        public string? Description { get; set; }

        // 构造函数：接收 [Tool("xxx")] 里传进来的字符串
        // 当你写 [Tool("Tool1")] 时，编译器就是在调用这个构造函数
        public ToolAttribute(string name)
        {
            Name = name;
        }
    }

    // ============================================================
    // 第二步：再定义一个特性，演示"能贴在不同地方"和"命名参数"
    // ============================================================
    //
    // AttributeTargets.Method 表示这个特性只能贴在方法上
    // AllowMultiple = true 表示同一个方法可以贴多个

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class LogAttribute : Attribute
    {
        public string Message { get; }

        // Level 有默认值，使用时可以用命名参数覆盖
        public int Level { get; set; } = 0;

        public LogAttribute(string message)
        {
            Message = message;
        }
    }

    // ============================================================
    // 第三步：把特性贴到类和方法上
    // ============================================================
    //
    // 注意 [Tool("Echo")] 等价于 [ToolAttribute("Echo")]
    // 因为类名以 Attribute 结尾，C# 允许省略后缀

    [Tool("Echo", Description = "原样返回输入")]
    public class EchoTool
    {
        // 同一个方法贴了两个 Log 特性
        // 因为 LogAttribute 设置了 AllowMultiple = true
        [Log("开始执行", Level = 1)]
        [Log("执行结束", Level = 2)]
        public string Run(string input)
        {
            return input;
        }
    }

    [Tool("Upper", Description = "转成大写")]
    public class UpperTool
    {
        [Log("Upper 执行")]
        public string Run(string input)
        {
            return input.ToUpper();
        }
    }

    // 一个没有贴任何 [Tool] 的类，用于演示"过滤"
    public class NotATool
    {
        public void Whatever() { }
    }

    // ============================================================
    // 第四步：写一个"反射读取特性"的工具类
    // ============================================================
    //
    // 特性贴上去之后，本身不会自动生效。
    // 必须有人通过反射把它读出来，它才有意义。

    public static class AttributeReader
    {
        // 读取一个类上的 ToolAttribute
        public static ToolAttribute? GetToolAttribute(Type type)
        {
            // GetCustomAttribute<T>() 是 .NET 提供的扩展方法
            // 返回贴在该类型上的 ToolAttribute，没有则返回 null
            return type.GetCustomAttribute<ToolAttribute>();
        }

        // 读取一个方法上的所有 LogAttribute
        public static List<LogAttribute> GetLogAttributes(MethodInfo method)
        {
            // GetCustomAttributes<T>() 返回所有该类型的特性
            // 因为 AllowMultiple = true，可能返回多个
            return method.GetCustomAttributes<LogAttribute>().ToList();
        }

        // 扫描整个程序集，找出所有贴了 [Tool] 的类
        public static Dictionary<string, Type> BuildToolRegistry()
        {
            var registry = new Dictionary<string, Type>();

            // 拿到当前程序集里所有类型
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                // 跳过抽象类、接口等
                if (type.IsAbstract || type.IsInterface) continue;

                // 尝试读取 ToolAttribute
                var attr = type.GetCustomAttribute<ToolAttribute>();

                // 没有贴 [Tool] 的类，跳过
                if (attr == null) continue;

                // 建立了 "名字 -> 类型" 的映射
                registry[attr.Name] = type;
            }

            return registry;
        }
    }

    // ============================================================
    // 第五步：入口，把上面所有东西串起来演示
    // ============================================================

    class Program
    {
        static void Main()
        {
            Console.WriteLine("========== 演示 1：读取类上的 Tool 特性 ==========\n");

            // 用 typeof 拿到 EchoTool 的 Type 对象
            Type echoType = typeof(EchoTool);

            // 通过反射读取它身上的 ToolAttribute
            var toolAttr = AttributeReader.GetToolAttribute(echoType);

            if (toolAttr != null)
            {
                // 读取构造函数里存进去的 Name
                Console.WriteLine($"EchoTool 的 Name        = {toolAttr.Name}");
                // 读取通过命名参数设置的 Description
                Console.WriteLine($"EchoTool 的 Description = {toolAttr.Description}");
            }

            Console.WriteLine();


            Console.WriteLine("========== 演示 2：读取方法上的 Log 特性 ==========\n");

            // 拿到 EchoTool 的 Run 方法
            MethodInfo runMethod = echoType.GetMethod("Run")!;

            // 读取这个方法上的所有 LogAttribute
            var logs = AttributeReader.GetLogAttributes(runMethod);

            Console.WriteLine($"EchoTool.Run 上共贴了 {logs.Count} 个 Log 特性：");
            foreach (var log in logs)
            {
                Console.WriteLine($"  - Message = {log.Message}, Level = {log.Level}");
            }

            Console.WriteLine();


            Console.WriteLine("========== 演示 3：扫描程序集，自动构建注册表 ==========\n");

            // 这一步就是"工厂模式"里注册表的来源
            var registry = AttributeReader.BuildToolRegistry();

            Console.WriteLine($"共找到 {registry.Count} 个贴了 [Tool] 的类：");
            foreach (var kv in registry)
            {
                Console.WriteLine($"  {kv.Key} -> {kv.Value.Name}");
            }

            Console.WriteLine();


            Console.WriteLine("========== 演示 4：根据名字用反射创建实例并调用 ==========\n");

            // 用注册表 + 反射，动态创建对象
            // 注意：这里全程没有出现 new EchoTool() / new UpperTool()
            string targetName = "Upper";
            if (registry.TryGetValue(targetName, out Type? targetType))
            {
                // Activator.CreateInstance 等价于 new targetType()
                object instance = Activator.CreateInstance(targetType)!;

                // 反射调用 Run 方法
                MethodInfo? method = targetType.GetMethod("Run");
                object? result = method?.Invoke(instance, new object[] { "hello" });

                Console.WriteLine($"动态调用 {targetName}.Run(\"hello\") => {result}");
            }

            Console.WriteLine();


            Console.WriteLine("========== 演示 5：没有贴 [Tool] 的类不会被注册 ==========\n");

            bool containsNotATool = registry.Values.Contains(typeof(NotATool));
            Console.WriteLine($"注册表里是否包含 NotATool？ {containsNotATool}");
            Console.WriteLine("（答案是 False，因为它没贴 [Tool] 特性）");
        }
    }
}