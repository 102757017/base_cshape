using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("===== C# 条件运算符与合并运算符 Demo =====\n");

        // ============================================================
        // 1. 三元运算符（?:）- 条件运算符
        // 作用：根据条件返回两个值之一
        // 语法：condition ? true_value : false_value
        // ============================================================
        Console.WriteLine("--- 1. 三元运算符 (?:) ---");
        int age = 20;
        string status = age >= 18 ? "成年人" : "未成年人";
        Console.WriteLine($"年龄 {age}：{status}\n");


        // ============================================================
        // 2. Null 合并运算符（??）
        // 作用：如果左边不为 null，返回左边；否则返回右边
        // 语法：left ?? right
        // ============================================================
        Console.WriteLine("--- 2. Null 合并运算符 (??) ---");
        string? name = null;
        string displayName = name ?? "匿名用户";
        Console.WriteLine($"name = null，显示：{displayName}");
        name = "张三";
        displayName = name ?? "匿名用户";
        Console.WriteLine($"name = '张三'，显示：{displayName}\n");



        // ============================================================
        // 3. Null 合并赋值运算符（??=）
        // 作用：如果左边为 null，将右边赋值给左边
        // 语法：left ??= right
        // ============================================================
        Console.WriteLine("--- 3. Null 合并赋值运算符 (??=) ---");
        List<string>? list = null;
        list ??= new List<string>();  // list 为 null，创建新列表
        list.Add("项目1");
        Console.WriteLine($"列表元素数：{list.Count}");
        list ??= new List<string>();  // list 不为 null，不执行赋值
        Console.WriteLine($"列表元素数仍为：{list.Count}\n");



        // ============================================================
        // 4. Null 条件运算符（?. 和 ?[]）
        // 作用：如果对象为 null，停止访问并返回 null；否则继续访问
        // 语法：object?.member 或 object?[index]
        // ============================================================
        Console.WriteLine("--- 4. Null 条件运算符 (?.) ---");
        Person? person = null;
        string? city = person?.Address?.City;  // person 为 null，直接返回 null
        Console.WriteLine($"person = null，城市：{(city ?? "未知")}");
        
        person = new Person { Address = new Address { City = "武汉" } };
        city = person?.Address?.City;  // 正常访问
        Console.WriteLine($"person 不为 null，城市：{city}");
        
        // ?[] 用于索引访问
        List<int>? numbers = null;
        int? first = numbers?[0];  // numbers 为 null，返回 null
        Console.WriteLine($"numbers = null，第一个元素：{(first.HasValue ? first.ToString() : "null")}");
        
        numbers = new List<int> { 10, 20, 30 };
        first = numbers?[0];  // 正常访问
        Console.WriteLine($"numbers 有元素，第一个：{first}\n");



        // ============================================================
        // 5. 组合使用（?. 和 ?? 配合）
        // 作用：安全访问 + 默认值
        // ============================================================
        Console.WriteLine("--- 5. 组合使用 (?.) + (??) ---");
        Person? unknownPerson = null;
        // 如果 person 为 null，?. 返回 null，?? 提供默认值
        string result = unknownPerson?.Name ?? "默认姓名";
        Console.WriteLine($"未知人员姓名：{result}");
        
        Person knownPerson = new Person { Name = "李四" };
        result = knownPerson?.Name ?? "默认姓名";
        Console.WriteLine($"已知人员姓名：{result}\n");

        // ============================================================
        // 6. 实际应用场景
        // ============================================================
        Console.WriteLine("--- 6. 实际应用场景 ---");
        
        // 场景1：配置读取
        string? configValue = GetConfig("Timeout");
        int timeout = int.TryParse(configValue, out int t) ? t : 30;
        Console.WriteLine($"超时时间：{timeout} 秒");
        
        // 场景2：安全访问集合
        List<Person>? people = null;
        int count = people?.Count ?? 0;  // 如果列表为 null，返回 0
        Console.WriteLine($"人员数量：{count}");
        
        // 场景3：事件触发（常用）
        EventHandler? myEvent = null;
        myEvent?.Invoke(null, EventArgs.Empty);  // 安全触发事件
        Console.WriteLine("事件安全触发完成（无订阅者时不会报错）");
    }
    
    static string? GetConfig(string key)
    {
        // 模拟配置读取
        return null;
    }
}

// 示例类
class Person
{
    public string? Name { get; set; }
    public Address? Address { get; set; }
}

class Address
{
    public string? City { get; set; }
}