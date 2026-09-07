using System;
using System.IO;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== 查找编译输出位置 ===");
        
        // 打印当前进程的 .dll 文件路径
        var mainModule = System.Diagnostics.Process.GetCurrentProcess().MainModule;
        Console.WriteLine($"当前运行的程序：{mainModule.FileName}");
        
        // 打印当前程序集路径
        var assemblyPath = typeof(Program).Assembly.Location;
        Console.WriteLine($"当前程序集路径：{assemblyPath}");
        
        // 打印临时目录
        Console.WriteLine($"系统临时目录：{Path.GetTempPath()}");
        
        Console.WriteLine("\n按任意键退出...");
        Console.ReadKey();
    }
}