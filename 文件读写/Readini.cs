#!/usr/bin/env dotnet-script
#r "nuget: Microsoft.Extensions.Configuration, 8.0.0"
#r "nuget: Microsoft.Extensions.Configuration.Ini, 8.0.0"
#r "nuget: Microsoft.Extensions.Configuration.Binder, 8.0.0"

using System;
using System.IO;
using Microsoft.Extensions.Configuration;

// 设置工作目录为脚本所在目录
Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

string configFile = "seting.ini";

// 读取配置
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddIniFile(configFile, optional: false, reloadOnChange: true)
    .Build();

// 获取配置值（键名会自动转为小写）
string level = configuration["logging:level"];
string host = configuration["mysql:host"];
string port = configuration["mysql:port"];

Console.WriteLine($"获取指定的section下的option: {port} (类型: {port.GetType()})");
Console.WriteLine($"Level: {level}");
Console.WriteLine($"Host: {host}");

// 使用强类型绑定（可选）
// 定义配置类
public class MySqlConfig
{
    public string Host { get; set; }
    public string Port { get; set; }
}

var mysqlConfig = configuration.GetSection("mysql").Get<MySqlConfig>();
if (mysqlConfig != null)
{
    Console.WriteLine($"\n强类型绑定:");
    Console.WriteLine($"Host: {mysqlConfig.Host}");
    Console.WriteLine($"Port: {mysqlConfig.Port}");
}

// 获取所有配置（展示）
Console.WriteLine("\n所有配置:");
foreach (var section in configuration.GetChildren())
{
    Console.WriteLine($"[{section.Key}]");
    foreach (var child in section.GetChildren())
    {
        Console.WriteLine($"  {child.Key} = {child.Value}");
    }
}