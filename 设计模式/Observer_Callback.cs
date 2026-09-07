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
// 为了显示 null 值更清晰，做一下处理
Console.WriteLine($"断开操作结果：{(disconnectResult.HasValue ? disconnectResult.Value.ToString() : "null")}");


/// <summary>
/// 网络管理器（类似于 Qt 中的 QObject）
/// 负责管理网络连接状态，并在状态变化时通知所有已注册的订阅者。
/// </summary>
public class NetworkManager
{
    // 为 UI 订阅者预留回调槽位
    // 类型说明：接收一个 bool 参数（连接状态），返回一个 bool（处理结果）
    private Func<bool, bool>? _onStatusChangedUi;

    // 为 Logger 订阅者预留回调槽位
    // 类型说明：接收一个 bool 参数，无返回值（仅记录日志，不需要反馈）
    private Action<bool>? _onStatusChangedLogger;

    /// <summary>
    /// 注册 UI 模块的回调函数。
    /// </summary>
    /// <param name="callback">接收一个 bool 参数（连接状态），返回 bool（处理结果）</param>
    public void RegisterUi(Func<bool, bool> callback)
    {
        _onStatusChangedUi = callback;
    }

    /// <summary>
    /// 注册 Logger 模块的回调函数。
    /// </summary>
    /// <param name="callback">接收一个 bool 参数（连接状态），无返回值</param>
    public void RegisterLogger(Action<bool> callback)
    {
        _onStatusChangedLogger = callback;
    }

    /// <summary>
    /// 建立网络连接，并通知所有订阅者。
    /// </summary>
    /// <returns>bool: UI 模块的处理结果（True 表示 UI 更新成功，False 表示失败）</returns>
    public bool Connect()
    {
        // 调用 UI 回调，传入 True 表示连接成功，并获取其返回值
        if (_onStatusChangedUi != null)
        {
            bool result = _onStatusChangedUi(true);
            return result; // 将 UI 的处理结果返回给调用者
        }

        // 调用 Logger 回调（如果有），传入 True 表示连接成功
        _onStatusChangedLogger?.Invoke(true);

        // 如果没有 UI 回调，返回 False
        return false;
    }

    /// <summary>
    /// 断开网络连接，并通知所有订阅者。
    /// </summary>
    /// <returns>
    /// bool?:
    /// - True 表示 UI 断开处理成功
    /// - False 表示 UI 断开处理失败
    /// - null 表示没有注册 UI 回调，无需处理
    /// </returns>
    public bool? Disconnect()
    {
        bool? uiResult = null;

        // 调用 UI 回调，传入 False 表示断开连接
        if (_onStatusChangedUi != null)
        {
            uiResult = _onStatusChangedUi(false);
        }

        // 调用 Logger 回调（如果有），传入 False 表示断开连接
        _onStatusChangedLogger?.Invoke(false);

        // 返回 UI 的处理结果（可能为 null）
        return uiResult;
    }
}

/// <summary>
/// 用户界面模块（类似 Qt 中的 Widget）
/// 负责更新界面图标，反映网络连接状态。
/// </summary>
public class UI
{
    private readonly NetworkManager _network;

    /// <summary>
    /// 构造函数，自动向 NetworkManager 注册自己的回调。
    /// </summary>
    /// <param name="network">NetworkManager 实例（依赖注入）</param>
    public UI(NetworkManager network)
    {
        _network = network;
        // 注册回调时只传方法名（不加括号），避免立即执行
        _network.RegisterUi(UpdateIcon);
    }

    /// <summary>
    /// 更新界面图标（回调函数）。
    /// </summary>
    /// <param name="isConnected">True 表示已连接，False 表示已断开</param>
    /// <returns>bool: 始终返回 True，表示界面更新成功</returns>
    public bool UpdateIcon(bool isConnected)
    {
        // 根据连接状态选择不同的图标
        string icon = isConnected ? "🟢" : "🔴";
        Console.WriteLine($"更新图标：{icon}");

        // 模拟界面更新成功
        return true;
    }
}

/// <summary>
/// 日志模块（独立的订阅者）
/// 负责记录网络状态变化，用于调试和监控。
/// </summary>
public class Logger
{
    private readonly NetworkManager _network;

    /// <summary>
    /// 构造函数，自动向 NetworkManager 注册自己的回调。
    /// </summary>
    /// <param name="network">NetworkManager 实例</param>
    public Logger(NetworkManager network)
    {
        _network = network;
        _network.RegisterLogger(LogStatus);
    }

    /// <summary>
    /// 记录连接状态（回调函数）。
    /// </summary>
    /// <param name="isConnected">True 表示已连接，False 表示已断开</param>
    public void LogStatus(bool isConnected)
    {
        string status = isConnected ? "已连接" : "已断开";
        Console.WriteLine($"[Logger] 连接状态：{status}");
    }
}

