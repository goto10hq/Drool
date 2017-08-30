# Drool

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