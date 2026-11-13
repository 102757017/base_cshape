using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


Student[] students = new Student[]
{
    new Student(001,"LiMing",20,"Male"),
    new Student(002,"WangWu",21,"Female"),
    new Student(003,"ZhangSan",22,"Male"),
    new Student(004,"ZhaoLiu",23,"Female"),
    new Student(005,"SunQi",24,"Male"),
};

bool result0=students.Any(s => s.Age > 20);
Console.WriteLine($"是否存在年龄大于20的对象：{result0}");

var result1 = students
    .Where(s => s.Age > 20 && s.StudentName != null)        // 第1步：筛选
    .OrderBy(s => s.Age)                                    // 第2步：排序
    .ThenBy(s => s.StudentName)      
    .Select(s => new{s.StudentName,s.Age})                  // 第3步：投影,创建一个没有名字的临时类型对象
    .Take(10)                                               // 第4步：取前10
    .ToList();                                              // 第5步：物化,支持List/Array/Dictionary
Console.WriteLine(string.Join("\n\r", result1));


//分组统计
var result2 = students
    .GroupBy(s => s.Sex)                                                       // 按性别分组
    .Select(s => new { s.Key, Count = s.Count(), Avg = s.Average(s=>s.Age)})   // 统计每组的数量
    .ToList();
Console.WriteLine(string.Join("\n\r", result2));

//
var result3 = students
    .GroupBy(s => new { s.Sex, s.Age })     // 只有两个字段都相同的元素才会分到同一组。
    .Select(g => new{g.Key,Count = g.Count(),Avg = g.Average(s => s.Age)})
    .ToList();
Console.WriteLine(string.Join("\n\r", result3));


Room[] rooms = new Room[]
{
    new Room(001,"RoomA",30,"Building1",students),
    new Room(002,"RoomB",20,"Building2",students),
    new Room(003,"RoomC",25,"Building3",students),
};
var result4= rooms
    .Where(r => r.Capacity > 20 )                 // 第1步：筛选
    .ToList();                                   // 第8步：物化,支持List/Array/Dictionary
Console.WriteLine(string.Join("\n\r", result4));


public class Student 
{
    public int StudentID { get; set; }
    public string StudentName { get; set; }
    public int Age { get; set; }
    public string Sex { get; set; }
	
    public Student(int id, string name, int age, string sex)
    {
        StudentID = id;
        StudentName = name;
        Age = age;
        Sex = sex;
    }
	
}

public class Room
{
    public int RoomID { get; set; }
    public string RoomName { get; set; }
    public int Capacity { get; set; }
    public string Location { get; set; }

    public Student[] Students { get; set; }

    public Room(int id, string name, int capacity, string location, Student[] students)
    {
        RoomID = id;
        RoomName = name;
        Capacity = capacity;
        Location = location;
        Students = students;
    }
}