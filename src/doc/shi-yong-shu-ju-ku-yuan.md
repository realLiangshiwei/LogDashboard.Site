---
description: >-
  在快速入门中使用的是文件源，在生产环境使用数据源是最好的选择。本文源码在https://github.com/liangshiw/LogDashboard/tree/master/samples/DatabaseSource
---

# 使用数据库源

## 创建一个NotCore项目

确保机器上安装上DotnetCore SDK ，同快速入门一样，我们需要一个DotnetCore Empty项目 。打开PowerShell运行以下命令

```text
dotnet new empty
```

打开appsettings.json添加连接字符串配置

```text
"ConnectionStrings": {
   "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=log;Trusted_Connection=True;MultipleActiveResultSets=true"
 }
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
注释中的代码是创建数据库和表的SQL，请在DBMS中执行

```text
<?xml version="1.0" encoding="utf-8" ?>

<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      autoReload="true">

  <targets>
    <target name="database" xsi:type="Database">
      <connectionString>${var:ConnectionString}</connectionString>
      <!--
    USE [master]
    GO
    /****** Object:  Database [log]    Script Date: 2019/1/3 15:06:59 ******/
    CREATE DATABASE [log]
     CONTAINMENT = NONE
     ON  PRIMARY 
    ( NAME = N'log', FILENAME = N'C:\Users\admin\log.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
     LOG ON 
    ( NAME = N'log_log', FILENAME = N'C:\Users\admin\log_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
    GO
    ALTER DATABASE [log] SET COMPATIBILITY_LEVEL = 130
    GO
    IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
    begin
    EXEC [log].[dbo].[sp_fulltext_database] @action = 'enable'
    end
    GO
    ALTER DATABASE [log] SET ANSI_NULL_DEFAULT OFF 
    GO
    ALTER DATABASE [log] SET ANSI_NULLS OFF 
    GO
    ALTER DATABASE [log] SET ANSI_PADDING OFF 
    GO
    ALTER DATABASE [log] SET ANSI_WARNINGS OFF 
    GO
    ALTER DATABASE [log] SET ARITHABORT OFF 
    GO
    ALTER DATABASE [log] SET AUTO_CLOSE OFF 
    GO
    ALTER DATABASE [log] SET AUTO_SHRINK OFF 
    GO
    ALTER DATABASE [log] SET AUTO_UPDATE_STATISTICS ON 
    GO
    ALTER DATABASE [log] SET CURSOR_CLOSE_ON_COMMIT OFF 
    GO
    ALTER DATABASE [log] SET CURSOR_DEFAULT  GLOBAL 
    GO
    ALTER DATABASE [log] SET CONCAT_NULL_YIELDS_NULL OFF 
    GO
    ALTER DATABASE [log] SET NUMERIC_ROUNDABORT OFF 
    GO
    ALTER DATABASE [log] SET QUOTED_IDENTIFIER OFF 
    GO
    ALTER DATABASE [log] SET RECURSIVE_TRIGGERS OFF 
    GO
    ALTER DATABASE [log] SET  DISABLE_BROKER 
    GO
    ALTER DATABASE [log] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
    GO
    ALTER DATABASE [log] SET DATE_CORRELATION_OPTIMIZATION OFF 
    GO
    ALTER DATABASE [log] SET TRUSTWORTHY OFF 
    GO
    ALTER DATABASE [log] SET ALLOW_SNAPSHOT_ISOLATION OFF 
    GO
    ALTER DATABASE [log] SET PARAMETERIZATION SIMPLE 
    GO
    ALTER DATABASE [log] SET READ_COMMITTED_SNAPSHOT OFF 
    GO
    ALTER DATABASE [log] SET HONOR_BROKER_PRIORITY OFF 
    GO
    ALTER DATABASE [log] SET RECOVERY SIMPLE 
    GO
    ALTER DATABASE [log] SET  MULTI_USER 
    GO
    ALTER DATABASE [log] SET PAGE_VERIFY CHECKSUM  
    GO
    ALTER DATABASE [log] SET DB_CHAINING OFF 
    GO
    ALTER DATABASE [log] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
    GO
    ALTER DATABASE [log] SET TARGET_RECOVERY_TIME = 60 SECONDS 
    GO
    ALTER DATABASE [log] SET DELAYED_DURABILITY = DISABLED 
    GO
    ALTER DATABASE [log] SET QUERY_STORE = OFF
    GO
    USE [log]
    GO
    ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
    GO
    ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
    GO
    ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
    GO
    ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
    GO
    USE [log]
    GO
    /****** Object:  Table [dbo].[Log]    Script Date: 2019/1/3 15:06:59 ******/
    SET ANSI_NULLS ON
    GO
    SET QUOTED_IDENTIFIER ON
    GO
    CREATE TABLE [dbo].[Log](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [MachineName] [nvarchar](50) NOT NULL,
        [LongDate] [datetime2] NOT NULL,
        [Level] [nvarchar](50) NOT NULL,
        [Message] [nvarchar](max) NOT NULL,
        [Logger] [nvarchar](250) NULL,
        [Callsite] [nvarchar](max) NULL,
        [Exception] [nvarchar](max) NULL,
     CONSTRAINT [PK_dbo.Log] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    GO
    USE [master]
    GO
    ALTER DATABASE [log] SET  READ_WRITE 
    GO

  -->

      <commandText>
        insert into dbo.Log (
        MachineName, LongDate, Level, Message,
        Logger, Callsite, Exception
        ) values (
        @MachineName, @LongDate, @Level, @Message,
        @Logger, @Callsite, @Exception
        );
      </commandText>

      <parameter name="@MachineName" layout="${machinename}" />
      <parameter name="@LongDate" layout="${longDate}" />
      <parameter name="@Level" layout="${level}" />
      <parameter name="@Message" layout="${message}" />
      <parameter name="@Logger" layout="${logger}" />
      <parameter name="@Callsite" layout="${callsite}" />
      <parameter name="@Exception" layout="${exception:tostring}" />
    </target>
  </targets>

  <rules>
    <logger name="*" minlevel="Info" writeTo="database" />
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
    // 数据库连接字符串
    var connectionString = Configuration.GetConnectionString("DefaultConnection");
    LogManager.Configuration.Variables["ConnectionString"] = connectionString;
    services.AddLogDashboard(opt =>
    {
        opt.UseDataBase(connectionString);
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

大功告成，这时运行项目，在浏览器中导航到/logdashboard。这时就能看到日志面板了

![](https://github.com/liangshiw/LogDashboard.Site/tree/1f492ab0a4699225dd35f9bd6a90f03e927d4c4b/src/doc/gitbook/assets/databaseboard.png)

## 发布时需要注意！

打开.csproj项目文件添加以下行 , 原因请参见 [https://github.com/aspnet/Mvc/issues/6021](https://github.com/aspnet/Mvc/issues/6021)

```text
<PropertyGroup>
   <MvcRazorExcludeRefAssembliesFromPublish>false</MvcRazorExcludeRefAssembliesFromPublish>
 </PropertyGroup>
```

