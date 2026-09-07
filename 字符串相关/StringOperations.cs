#!/usr/bin/env dotnet-script

using System;
using System.Text.Json;

// ===== 1. 转义字符 =====
Console.WriteLine("字符串内包含双引号和单引号：");
Console.WriteLine("\'");   // 单引号
Console.WriteLine("\"");   // 双引号
Console.WriteLine("\n");

// ===== 2. 反斜杠 =====
Console.WriteLine("字符串内包含\\");
Console.WriteLine("\n");

// ===== 3. 多行字符串 =====
Console.WriteLine(@"第一行
第二行
第三行");
Console.WriteLine("\n");


// ===== 4. 字符串长度 =====
string a = "abcdef";
Console.WriteLine("字符串abcdef的长度为：");
Console.WriteLine(a.Length+ "\n");

// ===== 5. 切片操作（Substring） =====
Console.WriteLine("从第二位开始提取a[1..3]：");
Console.WriteLine(a[1..3]+ "\n");  // 从索引1开始，取2个字符

Console.WriteLine("从第二位开始提取a[1:]，提取后方全部的字符");
Console.WriteLine(a[1..]+ "\n");  // 从索引1到末尾

// ===== 6. 负数索引（从末尾提取） =====
Console.WriteLine("提取最后一位：");
Console.WriteLine(a[^1]+ "\n");  // C# 8.0 索引运算符

Console.WriteLine("截取倒数第3位与倒数第2位之间的字符");
Console.WriteLine(a[^3..^1]+ "\n");  // C# 8.0 范围运算符


Console.WriteLine("截取倒数第3位到结尾的字符");
Console.WriteLine(a[^3..]+ "\n");  // C# 8.0 范围运算符


// ===== 7. 替换 =====
Console.WriteLine($"将字符串{a}中的b替换为空");
string b = a.Replace("b", "");
Console.WriteLine(b+ "\n");

// ===== 8. 用 / 连接字符串 =====
Console.WriteLine("用/连接字符串");
string c = string.Join("/", new[] { "hello", "world", "are you" });
Console.WriteLine(c + "\n");

//分割
string[] list1 = c.Split('/');
foreach (string v in list1) Console.WriteLine(v + "\n");


// 遍历list与索引，使用LINQ
foreach (var (item, index) in list1.Select((item, index) => (item, index)))
{
    Console.WriteLine($"{index} {item}");
}


//遍历dict
var dict1 = new Dictionary<string, int>
{
    { "Michael", 95 },
    { "Bob", 75 },
    { "Tracy", 85 }
};

int i = 0; //手动维护计数器
foreach (var key in dict1.Keys)
{
    Console.WriteLine($"{i} {key}");
    i++;
}