#!/usr/bin/env dotnet-script

using System;

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
Console.WriteLine(a.Length);
Console.WriteLine("\n");

// ===== 5. 切片操作（Substring） =====
Console.WriteLine("从第二位开始提取a[1..3]：");
Console.WriteLine(a[1..3]);  // 从索引1开始，取2个字符
Console.WriteLine("\n");

Console.WriteLine("从第二位开始提取a[1:]，提取后方全部的字符");
Console.WriteLine(a[1..]);  // 从索引1到末尾
Console.WriteLine("\n");

// ===== 6. 负数索引（从末尾提取） =====
Console.WriteLine("提取最后一位：");
Console.WriteLine(a[^1]);  // C# 8.0 索引运算符
Console.WriteLine("\n");

Console.WriteLine("截取倒数第3位与倒数第2位之间的字符");
Console.WriteLine(a[^3..^1]);  // C# 8.0 范围运算符
// 传统方式：a.Substring(a.Length - 3, 2)
Console.WriteLine("\n");

Console.WriteLine("截取倒数第3位到结尾的字符");
Console.WriteLine(a[^3..]);  // C# 8.0 范围运算符
// 传统方式：a.Substring(a.Length - 3)
Console.WriteLine("\n");

// ===== 7. 替换 =====
Console.WriteLine("将字符串中的b替换为空");
string b = a.Replace("b", "");
Console.WriteLine(b);
Console.WriteLine("\n");