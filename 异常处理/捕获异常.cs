using System;

class Program
{
    static void Main()
    {
        try
        {
            int r = 10 / int.Parse("a");  // 会抛出 FormatException
            Console.WriteLine($"result: {r}");
        }
        // 捕获特定异常（对应 Python 的 ValueError）
        catch (FormatException e)  // Python: ValueError
        {
            Console.WriteLine($"FormatException 错误信息是: {e.Message}");
        }
        // 捕获除零异常
        catch (DivideByZeroException e)  // Python: ZeroDivisionError
        {
            Console.WriteLine($"DivideByZeroException 错误信息是: {e.Message}");
        }
        // 捕获所有异常（对应 Python 的 BaseException）
        catch (Exception e)  // Python: BaseException
        {
            Console.WriteLine($"Exception 错误信息是: {e.Message}");
        }
        // finally 块始终执行
        finally
        {
            Console.WriteLine("finally 后面的语句是一定会执行的");
        }

        Console.WriteLine("\n--- 分隔线 ---\n");




        // ============================================================
        // 主动抛出异常
        // ============================================================
        try
        {
            throw new InvalidOperationException("主动抛出异常，后面的代码不继续执行");
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine($"InvalidOperationException 错误信息是: {e.Message}");
        }
    }
}