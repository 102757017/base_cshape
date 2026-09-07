#!/usr/bin/env dotnet-script
#r "nuget: ini-parser-netstandard, 2.5.3"

using System;
using System.IO;
using IniParser;
using IniParser.Model;

// 设置工作目录
Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
string configFile = "seting.ini";

// 1. 创建一个解析器实例
var parser = new FileIniDataParser();

// 2. 读取或创建配置
IniData data; //声明一个层级化的数据容器
if (File.Exists(configFile))
{
    // 读取文件
    data = parser.ReadFile(configFile);
}
else
{
    // 创建一个空的配置数据
    data = new IniData();
    // 写入示例
    data["logging"]["level"] = "20";
    data["mysql"]["host"] = "127.0.0.1";
    data["mysql"]["port"] = "80";
    // 保存新文件
    parser.WriteFile(configFile, data);
    Console.WriteLine($"✅ 已创建配置文件: {configFile}");
}

// 3. 读取配置（无需强制类型转换，直接就是 string）
string level = data["logging"]["level"];
string host = data["mysql"]["host"];
string port = data["mysql"]["port"];

Console.WriteLine($"获取指定的section下的option: {port} (类型: {port.GetType()})");
Console.WriteLine($"Level: {level}");
Console.WriteLine($"Host: {host}");

// 4. 修改并保存配置
data["mysql"]["port"] = "3306"; // 修改值
data["mysql"]["new_key"] = "新值"; // 新增键
parser.WriteFile(configFile, data); // 保存回文件
Console.WriteLine("✅ 配置已更新并保存。");