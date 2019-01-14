---
description: >-
  logdashboard支持log4net文件源,本示例源码在https://github.com/liangshiw/LogDashboard/tree/master/samples/UseLog4net
---

# 使用log4net

## 创建一个NotCore项目

确保机器上安装上DotnetCore SDK ，同快速入门一样，我们需要一个DotnetCore Empty项目 。打开PowerShell运行以下命令

```
dotnet new empty
```

## 安装log4net组件包

使用visualstudio打开项目，这时安装log4net程序包

```
Install-Package Microsoft.Extensions.Logging.Log4Net.AspNetCore
```

打开Startup.cs在Configure方法中添加log4net中间件，复制以下代码添加到Configure方法中

``` csharp
loggerFactory.AddLog4Net(new Log4NetProviderOptions
{
    PropertyOverrides =
        new List<NodeInfo>
        {
            new NodeInfo { XPath = "/log4net/appender/file[last()]", Attributes = new Dictionary<string, string> { { "value", $"{AppContext.BaseDirectory}LogFiles/" } } }
        }
});
```

添加log4net.config到项目中，并右键文件设置为复制到输出目录（始终复制\)，以下是log4net.config的全部内容

配置文件需要分隔符才可以被NLogDashboard解析，默认是\|\|与\|\|end，当然这些可以自定义，请参见 [LogDashboard配置](logdashboard-pei-zhi.md#wen-jian-yuan-ri-zhi-fen-ge-fu)

```text
<?xml version="1.0" encoding="utf-8" ?>
<log4net>
  <appender name="RollingFile" type="log4net.Appender.RollingFileAppender">
    <file value="LogFiles/"/>
    <preserveLogFileNameExtension value="true" />
    <datePattern value="yyyy-MM-dd'.log'" />
    <staticLogFileName value="false"/>
    <appendToFile value="true" />
    <maximumFileSize value="100KB" />
    <maxSizeRollBackups value="2" />
    <layout type="log4net.Layout.PatternLayout">
      <conversionPattern value="%date || %5level || %logger || %message || %exception ||end %newline" />
    </layout>
  </appender>
  <root>
    <level value="ALL"/>
    <appender-ref ref="RollingFile" />
  </root>
</log4net>
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