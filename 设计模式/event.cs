#!/usr/bin/env dotnet-script
using System;

// ===== 观察者模式：主程序入口 =====
Console.WriteLine("--- 创建网络管理器 ---");
NetworkManager net = new NetworkManager();

Console.WriteLine("--- 创建订阅者（自动注册） ---");
UI ui = new UI(net);
Logger logger = new Logger(net);

Console.WriteLine("--- 触发连接事件 ---");
bool connectResult = net.Connect();
Console.WriteLine($"连接操作结果：{connectResult}");

Console.WriteLine("\n--- 触发断开事件 ---");
bool? disconnectResult = net.Disconnect();
Console.WriteLine($"断开操作结果：{(disconnectResult.HasValue ? disconnectResult.Value.ToString() : "null")}");


/// <summary>
/// 网络管理器 - 使用 Event 实现观察者模式
/// </summary>
public class NetworkManager
{
    // 事件：通知所有订阅者状态变化，并收集 UI 的返回值
    public event Func<bool, bool>? StatusChangedForUi;

    // 事件：通知 Logger 模块（无返回值）
    public event Action<bool>? StatusChangedForLogger;

    /// <summary>
    /// 建立网络连接
    /// </summary>
    public bool Connect()
    {
        // 触发 UI 事件，收集返回值（如果有多个订阅者，只取最后一个）
        bool result = false;
        if (StatusChangedForUi != null)
        {
            foreach (Func<bool, bool> handler in StatusChangedForUi.GetInvocationList())
            {
                result = handler(true); //等价于handler.Invoke(true)
            }
        }

        // 触发 Logger 事件
        StatusChangedForLogger?.Invoke(true);

        return result;
    }

    /// <summary>
    /// 断开网络连接
    /// </summary>
    public bool? Disconnect()
    {
        bool? result = null;
        if (StatusChangedForUi != null)
        {
            foreach (Func<bool, bool> handler in StatusChangedForUi.GetInvocationList())
            {
                result = handler(false);
            }
        }

        StatusChangedForLogger?.Invoke(false);
        return result;
    }
}

/// <summary>
/// 用户界面模块
/// </summary>
public class UI
{
    public UI(NetworkManager network)
    {
        // 使用 += 订阅事件，无需手动注册方法
        network.StatusChangedForUi += UpdateIcon;
    }

    public bool UpdateIcon(bool isConnected)
    {
        string icon = isConnected ? "🟢" : "🔴";
        Console.WriteLine($"更新图标：{icon}");
        return true;
    }
}

/// <summary>
/// 日志模块
/// </summary>
public class Logger
{
    public Logger(NetworkManager network)
    {
        network.StatusChangedForLogger += LogStatus;
    }

    public void LogStatus(bool isConnected)
    {
        string status = isConnected ? "已连接" : "已断开";
        Console.WriteLine($"[Logger] 连接状态：{status}");
    }
}