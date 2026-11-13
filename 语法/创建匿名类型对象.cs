using System;
using System.Collections.Generic;


//匿名类型
var x = new { Name = "张三", Age = 20 };
Console.WriteLine(x);


//匿名类型也能自动生成属性名，规则是：取成员访问表达式的最后一段标识符作为属性名
var y = new { x.Name, x.Age };
Console.WriteLine(y);
