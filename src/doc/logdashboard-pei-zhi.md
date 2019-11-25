# LogDashboard 配置

## LogDashboardOptions

添加服务时**AddLogDashboard**方法有参数**LogDashboardOptions**它有以下配置

## 添加授权Attribute

```text
void AddAuthorizeAttribute(params IAuthorizeData[] authorizeAttributes)
```

参数是**IAuthorizeData**接口，可以传入它的任意派生类，例如**AuthorizeAttribute**

> **关于授权的更多内容，请参阅Asp.Net Core** [**授权文档**](https://docs.microsoft.com/zh-cn/aspnet/core/security/authorization/introduction?view=aspnetcore-2.2)\*\*\*\*

## 添加身份验证过滤器

```text
void AddAuthorizationFilter(params ILogDashboardAuthorizationFilter[] filters)
```

参数是**ILogDashboardAuthorizationFilter**接口，可以传入它的任意派生类，继承此接口可以自定义身份验证

## 自定义LogModel

```text
CustomLogModel<T>() where T : class, ILogModel
```

传入ILogModel的派生类，自定义日志的模型

## 使用数据库源

```text
void UseDataBase(string connectionString, string tableName = "log")
```

它有两个参数，第一个是数据库的连接字符串，第二个则是表名。表明默认是log，可以传入参数自定义表名

## 文件源日志分隔符

```text
  /// <summary>
  /// file log field Delimiter
  /// </summary>
  public string FileFieldDelimiter { get; set; }


  /// <summary>
  /// file log end Delimiter
  /// </summary>
  public string FileEndDelimiter { get; set; }
```

用于解析日志文件的内容，根据nlog.config进行定义，默认是 \|\| 与 \|\|end，可以更改为任意的分隔符

```text
layout="${longdate}||${level}||${logger}||${message}||${exception:format=ToString:innerFormat=ToString:maxInnerExceptionLevel=10:separator=\r\n}||end"
```

## 自定义日志面板 url

pathMatch默认值是 /logdashboard，可以在Startup.cs添加中间件时进行自定义

```text
 static IApplicationBuilder UseLogDashboard(
            this IApplicationBuilder builder, string pathMatch = "/LogDashboard")
```

