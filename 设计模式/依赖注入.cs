﻿using System;

// ===== 依赖注入：主程序入口 =====
Console.WriteLine("--- 创建具体服务 ---");
IMessageSender emailSender = new EmailSender();
IMessageSender smsSender = new SmsSender();

Console.WriteLine("--- 通过构造函数注入依赖 ---");
NotificationService emailService = new NotificationService(emailSender);
NotificationService smsService = new NotificationService(smsSender);

Console.WriteLine("--- 使用注入的服务发送通知 ---");
emailService.Notify("user@example.com", "欢迎注册！");
smsService.Notify("13800000000", "您的验证码是 123456");

Console.WriteLine("\n--- 动态切换依赖（构造注入 + 重新创建对象） ---");
NotificationService switchedService = new NotificationService(new SmsSender());
switchedService.Notify("13900000000", "依赖已切换为短信发送");


/// <summary>
/// 消息发送抽象接口
/// </summary>
public interface IMessageSender
{
    void Send(string target, string message);
}

/// <summary>
/// 邮件发送实现
/// </summary>
public class EmailSender : IMessageSender
{
    public void Send(string target, string message)
    {
        Console.WriteLine($"[Email] 发送给 {target}：{message}");
    }
}

/// <summary>
/// 短信发送实现
/// </summary>
public class SmsSender : IMessageSender
{
    public void Send(string target, string message)
    {
        Console.WriteLine($"[SMS] 发送给 {target}：{message}");
    }
}

/// <summary>
/// 通知服务 —— 依赖 IMessageSender，通过构造函数注入
/// </summary>
public class NotificationService
{
    private readonly IMessageSender _messageSender;

    // 构造函数注入：由外部传入具体实现
    public NotificationService(IMessageSender messageSender)
    {
        _messageSender = messageSender;
    }

    public void Notify(string target, string message)
    {
        Console.WriteLine("--- 开始发送通知 ---");
        _messageSender.Send(target, message);
        Console.WriteLine("--- 通知发送完成 ---\n");
    }
}