using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

// 启用 BinaryFormatter 只能在 .NET 5之前的版本中使用，在此后的版本废弃了。

// 顶级语句必须放在任何类定义之前
var original = new Person
{
    Name = "张三",
    Age = 28,
    Email = "zhangsan@example.com",
    TempData = "运行时临时数据"
};
Console.WriteLine("=== 原始对象 ===");
Console.WriteLine(original);

string filePath = "person.bin";
var formatter = new BinaryFormatter(); //二进制序列化的类
using (var stream = new FileStream(filePath, FileMode.Create))  //FileMode是一个枚举,支持创建新文件/打开现有文件/追加到末尾
{
    formatter.Serialize(stream, original);
}
Console.WriteLine($"\n✅ 序列化完成，文件保存到：{filePath}");

using (var stream = new FileStream(filePath, FileMode.Open))
{
    var deserialized = (Person)formatter.Deserialize(stream); //BinaryFormatter反序列化返回的是 object 类型，无法自动推断类型，必须显式转换。System.Text.Json序列化可以指定推断类型
    Console.WriteLine("\n=== 反序列化后的对象 ===");
    Console.WriteLine(deserialized);
}


var p = new CustomPerson { Name = "张三", Age = 28, Secret = "MyPassword123" };
using (var ms = new MemoryStream())
{
    formatter.Serialize(ms, p);
    ms.Position = 0;
    var p2 = (CustomPerson)formatter.Deserialize(ms);
    Console.WriteLine($"\n自定义反序列化后的对象：{p2}");
}



Console.WriteLine("\n按任意键退出...");
Console.ReadKey();















// 类定义放在最后（顶级语句之后）
[Serializable]
public class Person
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Email { get; set; } = string.Empty;

    [NonSerialized]
    public string TempData = "这个字段不会被序列化";

    public override string ToString()
    {
        return $"[Person] Name={Name}, Age={Age}, Email={Email}, TempData={TempData}";
    }
}

//自定义序列化需要继承ISerializable，
[Serializable]
public class CustomPerson : ISerializable
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Secret { get; set; }  // 想加密存储

    // 必须有无参构造函数（反序列化时可能调用，但非必须）
    public CustomPerson() { }

    // 必须实现自定义序列化的GetObjectData 方法：将数据写入 SerializationInfo，函数签名完全固定
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("Name", Name);
        info.AddValue("Age", Age);
        // 自定义处理：加密 Secret
        info.AddValue("Secret", Encrypt(Secret)); 
        Console.WriteLine("\n自定义序列化执行");
    }

    // 必须实现自定义反序列化的构造函数：从 SerializationInfo 读取数据，构造函数必须是 protected 或 private，命名和签名固定
    protected CustomPerson(SerializationInfo info, StreamingContext context)
    {
        Name = info.GetString("Name");
        Age = info.GetInt32("Age");
        // 自定义处理：解密 Secret
        Secret = Decrypt(info.GetString("Secret"));
        Console.WriteLine("\n自定义反序列化执行");
    }

    private string Encrypt(string s) => Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(s));
    private string Decrypt(string s) => System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(s));

    public override string ToString() => $"[Person] Name={Name}, Age={Age}, Secret={Secret}";
}
