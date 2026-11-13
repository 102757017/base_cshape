using System;
using System.Collections.Generic;


PersonDto p1=new PersonDto("Sunny",43);
Console.WriteLine(p1);



//快速定义一个类及其属性，一行代码会由编译器展开成一个完整的 record class，包含属性、构造函数、相等性、ToString、Deconstruct 等一大堆成员。
public record PersonDto(string Name, int Age);


//record 是 C# 9.0（.NET 5 时代） 引入的语言特性，.NET Framework 4.8默认无法使用。
//在 .NET Framework 4.8 项目里解决 init 的 BCL（基础类库）缺失，需要加上下面的语句
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}