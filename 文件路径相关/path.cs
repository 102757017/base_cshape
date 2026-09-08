#!/usr/bin/env dotnet-script

using System;
using System.IO;
using System.Linq;

//Path 类处理路径的格式和字符串表示，不涉及实际文件系统。
//Directory 类对实际的文件系统进行操作（创建、删除、移动、遍历目录）

// 默认目录为当前bat路径，切换工作目录为exe所在目录
Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
var root = Environment.CurrentDirectory;

// 1. 当前程序目录
Console.WriteLine($"程序目录: {root}");

// 2. 路径分隔符
Console.WriteLine($"分隔符: {Path.DirectorySeparatorChar}");

// 3. 上级目录
var parent = Directory.GetParent(root)?.FullName;  //当对象不为 null 时才访问其成员，否则直接返回 null，而不会抛出异常
Console.WriteLine($"上级目录: {parent}");

// 4. 构造路径
var constructed = Path.Combine(parent ?? "", "xxx", "yyy");  //?? 的作用是：如果左边的表达式结果为 null，则返回右边的值；否则返回左边的值。
Console.WriteLine($"构造路径: {constructed}");

// 5. 当前工作目录
Console.WriteLine($"工作目录: {Environment.CurrentDirectory}");

// 7. 删除 temp（如果存在）并重新创建
var temp = Path.Combine(root, "temp");
if (Directory.Exists(temp)) Directory.Delete(temp, true);
Directory.CreateDirectory(temp);

// 8. 创建 log.txt 并复制
var log = Path.Combine(root, "log.txt");
File.Create(log).Close();
File.Copy(log, Path.Combine(root, "log2.txt"), true);
Console.WriteLine($"文件存在: {File.Exists(log)}");
File.Delete(log);

// 9. 创建 test 文件夹并复制
var test = Path.Combine(root, "test");
Directory.CreateDirectory(test);
CopyDirectory(test, Path.Combine(root, "test2"));
Console.WriteLine($"目录存在: {Directory.Exists(test)}");
Directory.Delete(test, true);
Directory.Delete(Path.Combine(root, "test2"), true);

// 10. 遍历文件和文件夹（全部 / 顶层 / txt）
var all = Directory.GetFileSystemEntries(root, "*", SearchOption.AllDirectories);
var top = Directory.GetFileSystemEntries(root, "*", SearchOption.TopDirectoryOnly);
var txts = Directory.GetFiles(root, "*.txt", SearchOption.AllDirectories);

// 输出（只显示前5项）
Console.WriteLine($"全部条目（{all.Length}个）: {string.Join(", ", all.Take(5))}...");
Console.WriteLine($"顶层条目（{top.Length}个）: {string.Join(", ", top.Take(5))}...");
Console.WriteLine($"txt文件（{txts.Length}个）: {string.Join(", ", txts.Take(5))}...");

// 辅助方法：递归复制目录
void CopyDirectory(string source, string dest)
{
    if (!Directory.Exists(source)) return;
    Directory.CreateDirectory(dest);
    foreach (var f in Directory.GetFiles(source))
        File.Copy(f, Path.Combine(dest, Path.GetFileName(f)), true);
    foreach (var d in Directory.GetDirectories(source))
        CopyDirectory(d, Path.Combine(dest, Path.GetFileName(d)));
}