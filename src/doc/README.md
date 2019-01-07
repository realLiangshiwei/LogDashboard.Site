---
description: >-
  本文源码在
  https://github.com/liangshiw/LogDashboard/tree/master/samples/DotNetCoreEmptyUseNlog
---

# 快速入门

## 创建一个NetCore项目

确保机器上安装了DotNetCore SDK，打开PowerShell运行以下命令,我们将创建一个AspNetCore空项目

```
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


配置文件需要分隔符才可以被NLogDashboard解析，默认是\|\|与\|\|end，当然这些可以自定义，请参见 [LogDashboard配置](logdashboard-pei-zhi.md#wen-jian-yuan-ri-zhi-fen-ge-fu)


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
            layout="${longdate}||${level}||${logger}||${message}||${exception:format=ToString:innerFormat=ToString:maxInnerExceptionLevel=10:separator=\r\n}||end" />
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

打开Startup.cs我们要做两件事  
  
1. 在ConfigureServices方法中配置服务

```text
public void ConfigureServices(IServiceCollection services)
{
    services.AddLogDashboard();
}
```


关于更多的配置请参阅 [LogDashboard配置](logdashboard-pei-zhi.md)


2. 在Configure方法中配置中间件

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

大功告成，这时运行项目，在浏览器中导航到/logdashboard。这时就能看到日志面板了  

![](gitbook/assets/dashboard.png)

## 发布时需要注意!

打开.csproj项目文件添加以下行 , 原因请参见 [https://github.com/aspnet/Mvc/issues/6021](https://github.com/aspnet/Mvc/issues/6021)

```text
<PropertyGroup>
   <MvcRazorExcludeRefAssembliesFromPublish>false</MvcRazorExcludeRefAssembliesFromPublish>
 </PropertyGroup>
```