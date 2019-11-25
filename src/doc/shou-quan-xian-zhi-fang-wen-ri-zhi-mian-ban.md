---
description: >-
  在生产环境自然不是所有人都可以访问日志面板,我们需要配置授权过滤器或登陆用户具有某个角色或权限才可以访问。本文源代码在
  https://github.com/liangshiw/LogDashboard/tree/master/samples/UseAuthorization
---

# 授权限制访问日志面板

## 创建一个NetCore项目

打开VisualStudio创建带有身份验证的Asp.Net Core项目，身份验证选择个人用户账户。如下图所示

![](https://github.com/liangshiw/LogDashboard.Site/tree/1f492ab0a4699225dd35f9bd6a90f03e927d4c4b/src/doc/gitbook/assets/image%20%282%29.png)

这时我们已经拥有一个带有注册登陆的Web应用程序

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

添加类SamplesAuthorizationFilter ，让它继承自**ILogDashboardAuthorizationFilter**接口，它将是我们的自定义授权过滤器，在这里简单的判断了IP是否是本地

> **ILogDashboardAuthorizationFilter接口是LogDashboard的授权过滤器接口，在进入面板之前会执行所有其被注册的派生类**

```text
public class SamplesAuthorizationFilter : ILogDashboardAuthorizationFilter
{
    public bool Authorization(LogDashboardContext context)
    {
        var url = context.HttpContext.Request.GetDisplayUrl();
        return url.Contains("localhost") || url.Contains("127.0.0.1");
    }
}
```

打开Startup.cs我们要做两件事

1. 在ConfigureServices方法中配置服务并使用option的**AddAuthorizationFilter**方法添加我们的自定义过滤器，除了**AddAuthorizationFilter**方法还使用了**AddAuthorizeAttribute**方法添加了**AuthorizeAttribute，**它是Asp.Net core Mvc的自带Attribute用来进行授权，这时日志面板将只有登陆用户才可以看到

> **AddAuthorizeAttribute还有角色与权限参数可以限制具有指定角色或权限的用户才可以访问日志面板**
>
> **AddAuthorizeAttribute方法接收IAuthorizeData的派生类**

```text
public void ConfigureServices(IServiceCollection services)
{
     services.AddLogDashboard(opt =>
    {
        opt.AddAuthorizeAttribute(new AuthorizeAttribute());
        opt.AddAuthorizationFilter(new SamplesAuthorizationFilter());
    });
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

这时我们启动项目，导航到/logdashboard，因为我们没有登陆用户，它会自动跳转到登陆页面要求用户登陆

![](https://github.com/liangshiw/LogDashboard.Site/tree/1f492ab0a4699225dd35f9bd6a90f03e927d4c4b/src/doc/gitbook/assets/image%20%281%29.png)

## 发布时需要注意！

打开.csproj项目文件添加以下行 , 原因请参见 [https://github.com/aspnet/Mvc/issues/6021](https://github.com/aspnet/Mvc/issues/6021)

```text
<PropertyGroup>
   <MvcRazorExcludeRefAssembliesFromPublish>false</MvcRazorExcludeRefAssembliesFromPublish>
 </PropertyGroup>
```

