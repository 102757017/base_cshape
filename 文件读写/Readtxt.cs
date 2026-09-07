using System;
using System.IO;
using System.Text;


// 设置工作目录为程序所在目录
//Environment.CurrentDirectory 是 System.Environment 类的一个静态属性，表示当前进程的工作目录。
//AppDomain.CurrentDomain.BaseDirectory  .exe所在的文件夹路径，由运行时自动提供
Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;


// ===== 覆盖模式写入 =====
using (var fs = new FileStream("覆盖了吗.txt", FileMode.Create))  //不写大括号时，using、if、for、foreach、while 等语句只作用于紧随其后的 下一条语句
using (var writer = new StreamWriter(fs, Encoding.UTF8))
{
	writer.Write("不会换行");  // 对应 Python 的 f.write('不会换行')
}

// 第二次覆盖写入
using (var fs = new FileStream("覆盖了吗.txt", FileMode.Create))
using (var writer = new StreamWriter(fs, Encoding.UTF8))
{
	writer.Write("不会换行");
}

// 第三次覆盖写入
using (var fs = new FileStream("覆盖了吗.txt", FileMode.Create))
using (var writer = new StreamWriter(fs, Encoding.UTF8))
{
	writer.Write("不会换行");
}

// ===== 追加模式写入（对应 Python 的 'a' 模式） =====
// FileMode.Append 会自动定位到文件末尾
using (var fs = new FileStream("filename.txt", FileMode.Append))
using (var writer = new StreamWriter(fs, Encoding.UTF8))
{
	writer.Write("我会换行\n");  // \n 会换行
}

// 第二次追加
using (var fs = new FileStream("filename.txt", FileMode.Append))
using (var writer = new StreamWriter(fs, Encoding.UTF8))
{
	writer.Write("我不会换行");  // 不会自动换行
}

// ===== 读取模式：一次读取一行（对应 Python 的 readline） =====
Console.WriteLine("一次只读取一行");
using (var fs = new FileStream("filename.txt", FileMode.Open))
using (var reader = new StreamReader(fs, Encoding.UTF8))
{
	string line = reader.ReadLine();  // 读取一行
	Console.WriteLine(line);
}
Console.WriteLine("\n");

// ===== 读取模式：一次读取整个文件（对应 Python 的 read） =====
Console.WriteLine("一次读取整个文件");
using (var fs = new FileStream("filename.txt", FileMode.Open))
using (var reader = new StreamReader(fs, Encoding.UTF8))
{
	string content = reader.ReadToEnd();  // 读取整个文件
	Console.WriteLine(content);
}
Console.WriteLine("\n");
