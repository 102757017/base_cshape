#:package Newtonsoft.Json@13.0.3

using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// 顶级语句必须放在任何类定义之前

// 一般序列化
var original = new Person
{
    Name = "张三",
    Age = 28,
    Email = "zhangsan@example.com",
    TempData = "运行时临时数据"
};

Console.WriteLine("=== 原始对象 ===");
Console.WriteLine(original);

string filePath = "person.json";

var settings = new JsonSerializerSettings
{
    Formatting = Formatting.Indented
};

string json = JsonConvert.SerializeObject(original, settings);
File.WriteAllText(filePath, json);

Console.WriteLine($"\n✅ 序列化完成，文件保存到：{filePath}");
Console.WriteLine(json);

string loadedJson = File.ReadAllText(filePath);
var deserialized = JsonConvert.DeserializeObject<Person>(loadedJson);

Console.WriteLine("\n=== 反序列化后的对象 ===");
Console.WriteLine(deserialized);

// 自定义序列化：通过 JsonConverter 加密 Secret
var p = new CustomPerson
{
    Name = "张三",
    Age = 28,
    Secret = "MyPassword123"
};

string customJson = JsonConvert.SerializeObject(p, settings);
Console.WriteLine("\n=== 自定义序列化后的 JSON ===");
Console.WriteLine(customJson);

var p2 = JsonConvert.DeserializeObject<CustomPerson>(customJson);
Console.WriteLine($"\n自定义反序列化后的对象：{p2}");

Console.WriteLine("\n按任意键退出...");
Console.ReadKey();

// 类定义放在最后

public class Person
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Email { get; set; } = string.Empty;

    // 对应 BinaryFormatter 的 [NonSerialized]
    [JsonIgnore]
    public string TempData = "这个字段不会被序列化";

    public override string ToString()
    {
        return $"[Person] Name={Name}, Age={Age}, Email={Email}, TempData={TempData}";
    }
}

// 使用 JsonConverter 实现自定义序列化
[JsonConverter(typeof(CustomPersonConverter))]
public class CustomPerson
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Secret { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"[CustomPerson] Name={Name}, Age={Age}, Secret={Secret}";
    }
}

public class CustomPersonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(CustomPerson);
    }

    // 自定义序列化：将 Secret 加密后写入 JSON
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var p = (CustomPerson)value;

        writer.WriteStartObject();

        writer.WritePropertyName("Name");
        writer.WriteValue(p.Name);

        writer.WritePropertyName("Age");
        writer.WriteValue(p.Age);

        writer.WritePropertyName("Secret");
        writer.WriteValue(Encrypt(p.Secret ?? string.Empty));

        writer.WriteEndObject();

        Console.WriteLine("\n自定义序列化执行");
    }

    // 自定义反序列化：读取 JSON 后解密 Secret
    public override object ReadJson(
        JsonReader reader,
        Type objectType,
        object existingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return null;

        var obj = JObject.Load(reader);

        var p = new CustomPerson
        {
            Name = obj["Name"]?.ToString() ?? string.Empty,
            Age = obj["Age"]?.ToObject<int>() ?? 0,
            Secret = Decrypt(obj["Secret"]?.ToString() ?? string.Empty)
        };

        Console.WriteLine("\n自定义反序列化执行");
        return p;
    }

    private static string Encrypt(string s)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
    }

    private static string Decrypt(string s)
    {
        return Encoding.UTF8.GetString(Convert.FromBase64String(s));
    }
}