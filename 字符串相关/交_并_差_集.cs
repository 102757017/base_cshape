#!/usr/bin/env dotnet-script
using System;
using System.Collections.Generic;
using System.Linq;


// ===== 1. 字符串转换为 Set =====
string a = "这是一个测试，这是另一个测试";
var b = new HashSet<char>(a);
Console.WriteLine($"字符串转换为set: [{string.Join(", ", b)}]");

// ===== 2. List 转换为 Set =====
string[] c = ["这", "是", "一个", "测试"];
var d = new HashSet<string>(c);
Console.WriteLine($"list转换为set: [{string.Join(", ", d)}]");

// ===== 注意：b 是 HashSet<char>，d 是 HashSet<string>，类型不同无法直接运算 =====
// 需要统一类型，这里以 string 为例
var b2 = new HashSet<string>(a.Select(ch => ch.ToString()));
var d2 = new HashSet<string>(c);

// ===== 3. 求交集 =====
var intersect = b2.Intersect(d2).ToHashSet();
Console.WriteLine($"求交集: [{string.Join(", ", intersect)}]");

// ===== 4. 求差集 =====
var except = b2.Except(d2).ToHashSet();
Console.WriteLine($"求差集: [{string.Join(", ", except)}]");

// ===== 5. 求并集 =====
var union = b2.Union(d2).ToHashSet();
Console.WriteLine($"求并集: [{string.Join(", ", union)}]");

// ===== 6. 求对称差集 =====
var symmetricExcept = b2.ToHashSet();
symmetricExcept.SymmetricExceptWith(d2);
Console.WriteLine($"求对称差集: [{string.Join(", ", symmetricExcept)}]");