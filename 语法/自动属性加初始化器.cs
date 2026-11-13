using System;
using System.Collections.Generic;


PlcChannel p1=new PlcChannel();
Console.WriteLine(p1);


public class PlcChannel
{
    public string Ip { get; set; } = "192.168.1.10";
    public int Port { get; set; } = 502;
    public byte SlaveId { get; set; } = 1;
    public int TimeoutMs { get; set; } = 800;
	
	
	//重写 ToString()以便Console.WriteLine可以显示出属性
    public override string ToString()
    {
        return $"PlcChannel {{ Ip = {Ip}, Port = {Port}, SlaveId = {SlaveId}, TimeoutMs = {TimeoutMs} }}";
    }
}

//record 是 C# 9.0（.NET 5 时代） 引入的语言特性，.NET Framework 4.8默认无法使用。
//在 .NET Framework 4.8 项目里解决 init 的 BCL（基础类库）缺失，需要加上下面的语句
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}