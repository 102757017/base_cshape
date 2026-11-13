using System;
using System.Collections.Generic;

Person p1=new Person{Name="Sunny",Age=43};
Console.WriteLine(p1.Method().x);

public class Person
{
	public string Name {get;set;}
	public int Age {get;set;}
	
	//C#返回元组必须先定义，不像python可以直接写return x,y
	public (string x,int y) Method()
	{
		return (this.Name,this.Age);
	}
	
}
