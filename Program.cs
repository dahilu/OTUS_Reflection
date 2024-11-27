using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

// Класс F для тестирования
public class F
{
    public int i1 { get; set; }
    public int i2 { get; set; }
    public int i3 { get; set; }
    public int i4 { get; set; }
    public int i5 { get; set; }

    public static F Get()
    {
        return new F() { i1 = 1, i2 = 2, i3 = 3, i4 = 4, i5 = 5 };
    }
}

// Класс-сериализатор
public static class CsvSerializer
{
    public static string Serialize<T>(T obj)
    {
        Type type = typeof(T);
        PropertyInfo[] properties = type.GetProperties();
        FieldInfo[] fields = type.GetFields();

        StringBuilder csv = new StringBuilder();

        foreach (var prop in properties)
        {
            csv.Append(prop.GetValue(obj)?.ToString() + ",");
        }

        foreach (var field in fields)
        {
            csv.Append(field.GetValue(obj)?.ToString() + ",");
        }

        // Удаляем последний лишний символ запятой
        if (csv.Length > 0)
            csv.Length--;

        return csv.ToString();
    }
}
public static class CsvDeserializer
{
    public static T Deserialize<T>(string csv) where T : new()
    {
        T obj = new T();
        Type type = typeof(T);
        PropertyInfo[] properties = type.GetProperties();
        FieldInfo[] fields = type.GetFields();

        string[] values = csv.Split(',');

        int index = 0;

        foreach (var prop in properties)
        {
            if (index < values.Length && prop.CanWrite)
            {
                prop.SetValue(obj, Convert.ChangeType(values[index], prop.PropertyType));
                index++;
            }
        }

        foreach (var field in fields)
        {
            if (index < values.Length)
            {
                field.SetValue(obj, Convert.ChangeType(values[index], field.FieldType));
                index++;
            }
        }

        return obj;
    }
}

class Program
{
    static void Main()
    {
        int iterations = 1000;

        F obj = F.Get();




        // Замер времени на сериализацию кастомным сериализатором
        Stopwatch stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            string csv = CsvSerializer.Serialize(obj);
        }
        stopwatch.Stop();
        Console.WriteLine($"Custom CSV Serializer: {stopwatch.ElapsedMilliseconds} ms");




        // Замер времени на вывод текста в консоль
        stopwatch.Restart();
        for (int i = 0; i < iterations; i++)
        {
            Console.WriteLine(CsvSerializer.Serialize(obj));
        }
        stopwatch.Stop();
        Console.WriteLine($"Time to output to console: {stopwatch.ElapsedMilliseconds} ms");





        // Замер времени на сериализацию стандартным механизмом (JSON)
        stopwatch.Restart();
        for (int i = 0; i < iterations; i++)
        {
            string json = JsonConvert.SerializeObject(obj);
        }
        stopwatch.Stop();
        Console.WriteLine($"Standard JSON Serializer: {stopwatch.ElapsedMilliseconds} ms");
        /*
        */







        /*
        string csv = CsvSerializer.Serialize(obj);

        // Замер времени на десериализацию кастомным десериализатором
        Stopwatch stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            F deserializedObj = CsvDeserializer.Deserialize<F>(csv);
        }
        stopwatch.Stop();
        Console.WriteLine($"Custom CSV Deserializer: {stopwatch.ElapsedMilliseconds} ms");






        // Десериализация с использованием Newtonsoft.Json (JSON)
        string json = JsonConvert.SerializeObject(obj);

        // Замер времени на десериализацию стандартным механизмом (JSON)
        stopwatch.Restart();
        for (int i = 0; i < iterations; i++)
        {
            F? deserializedObj = JsonConvert.DeserializeObject<F>(json);
        }
        stopwatch.Stop();
        Console.WriteLine($"Standard JSON Deserializer: {stopwatch.ElapsedMilliseconds} ms");
        */



    }
}

