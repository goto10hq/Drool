# Drool

[![Software License](https://img.shields.io/badge/license-MIT-brightgreen.svg?style=flat-square)](LICENSE.md)
[![Latest Version on NuGet](https://img.shields.io/nuget/v/Drool.svg?style=flat-square)](https://www.nuget.org/packages/Drool/)
[![NuGet](https://img.shields.io/nuget/dt/Drool.svg?style=flat-square)](https://www.nuget.org/packages/Drool/)
[![Visual Studio Team services](https://img.shields.io/vso/build/frohikey/c3964e53-4bf3-417a-a96e-661031ef862f/38.svg?style=flat-square)]()

## What is it?

Mega easy way to send HTML email with advantages:
- auto converting images in HTML as inline content (cid resources)
- possibility for easy string replacing

## Configuration

```xml
<appSettings>
    <add key="Drool.SmtpServer" value="smtp.sendgrid.net"/>
    <add key="Drool.SmtpPort" value="25"/>
    <add key="Drool.EnableSsl" value="false"/>
    <add key="Drool.SmtpLogin" value="mylogin"/>
    <add key="Drool.SmtpPassword" value="mypassword"/>
    <add key="Drool.Category" value="Drool"/>
</appSettings>
```

## Usage

```csharp
var mailer = new Mailer("EmailTemplate/index.html", new MailGun("Drool", true, new { level = 42 }));

mailer.Send(from, to, "My subject is test", new Dictionary<string, object>
    {
        { "Salutation", "Hello my lovely robot," },
        { "Yes", "http://www.goto10.cz" },
        { "No", "http://www.github.com" }
    });
```

## TODO

- some template engine... Razor?
- instead of System.Net using MailKit?