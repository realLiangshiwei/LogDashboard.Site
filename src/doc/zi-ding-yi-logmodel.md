---
description: >-
  LogModel提供了五个基本日志的属性
  LongDate、Logger、Level、Message、Exception。除以之外还可以自定义属性。本文源码在https://github.com/liangshiw/LogDashboard/tree/master/samples/CustomLogModel
---

# 自定义LogModel

## 创建一个NotCore项目

确保机器上安装上DotnetCore SDK ，同快速入门一样，我们需要一个DotnetCore Empty项目 。打开PowerShell运行以下命令

```text
dotnet new empty
```

## 安装Nlog日志组件

使用VSCode或VisualStudio打开项目，这时我们还需要做一些其他的准备工作。日志组件选用Nlog

```text
Install-Package NLog.Web.AspNetCore
```

打开Program.cs在CreateWebHostBuilder方法中添加Nlog中间件，复制以下代码覆盖CreateWebHostBuilder方法

```text
public static IWebHost CreateWebHostBuilder(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
       .UseStartup<Startup>()
       .ConfigureLogging(logging =>
       {
           logging.ClearProviders();
           logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
       })
       .UseNLog()
       .Build();
```

添加一个Nlog.config到项目中，并右键文件设置为复制到输出目录（始终复制\)，以下是Nlog.config的全部内容

配置文件需要分隔符才可以被NLogDashboard解析，默认是\|\|与\|\|end，当然这些可以自定义，请参见 TODO

layout最后添加了 ${var:application}与${var:requestMethod}变量， ${machinename}预定义变量

> 关于更多的Nlog预定义变量请参阅 [Nlog文档](https://nlog-project.org/config/?tab=layout-renderers)

```text
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      autoReload="true"
      throwExceptions="false"
      internalLogLevel="Off" internalLogFile="c:\temp\nlog-internal.log">
  <variable name="myvar" value="myvalue"/>

  <targets>
    <target xsi:type="file" name="File" fileName="${basedir}/logs/${shortdate}.log"
            layout="${longdate}||${level}||${logger}||${message}||${exception:format=ToString:innerFormat=ToString:maxInnerExceptionLevel=10:separator=\r\n}||${var:application}||${var:requestMethod}||${machinename}||end" />
  </targets>

  <rules>
    <logger name="*" minlevel="Debug" writeTo="file" />
  </rules>
</nlog>
```

## 安装LogDashboard

准备工作已经结束，这时安装LogDashboard

```text
Install-Package LogDashboard
```

在项目中添加ApplicationLogModel类，并继承自LogModel

```text
public class ApplicationLogModel : LogModel
{
    public string Application { get; set; }

    public string RequestMethod { get; set; }

    public string MachineName { get; set; }
}
```

类中定义了Application、RequestMethod与MachineName属性，接下来打开Startup.cs我们要做两件事

1. 在ConfigureServices方法中配置服务与变量，使用option的CuomstLogModel方法将ApplicationLogModel做为反省参数传递

> 因为MachineName是Nlog预定义的变量，所以不需要手动赋值

```text
public void ConfigureServices(IServiceCollection services)
{
    // 自定义变量
    LogManager.Configuration.Variables["application"] = "CustomLogModel";
    LogManager.Configuration.Variables["requestMethod"] = "Get";
    services.AddLogDashboard(opt => { opt.CustomLogModel<ApplicationLogModel>(); });
}
```

关于更多的配置请参阅 [LogDashboard配置](logdashboard-pei-zhi.md)

1. 在Configure方法中配置中间件

```text
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseLogDashboard();

    app.Run(async (context) =>
    {
        await context.Response.WriteAsync("Hello World!");
    });
}
```

大功告成，这时运行项目，在浏览器中导航到/logdashboard，并点击日志列表中的详情查看自定义属性

![](https://github.com/liangshiw/LogDashboard.Site/tree/1f492ab0a4699225dd35f9bd6a90f03e927d4c4b/src/doc/gitbook/assets/custominfo.png)

## 发布时需要注意！

打开.csproj项目文件添加以下行 , 原因请参见 [https://github.com/aspnet/Mvc/issues/6021](https://github.com/aspnet/Mvc/issues/6021)

```text
<PropertyGroup>
   <MvcRazorExcludeRefAssembliesFromPublish>false</MvcRazorExcludeRefAssembliesFromPublish>
 </PropertyGroup>
```

