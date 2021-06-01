# Hoe is logging te gebruiken?

## Table Of Contents
- Library
- Implementatie
- Workshop

## Library
Ik heb als eerste weer gegrepen naar [Tim Corey's video](https://www.youtube.com/watch?v=_iryZxv8Rxw&ab_channel=IAmTimCorey) over logging in .NET. Tim legt uit hoe je omgaat met de built-in ILogger interface en hier je voordeel uit kan halen. Via de officiële [Microsoft documentatie](https://docs.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line) over .NET heb ik een aantal overzichten zoals [Log Levels](https://docs.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line#log-level) gemakkelijk kunnen vinden.  
### Log Levels
De verschillende Log Levels geven aan hoe kritiek de toestand is of geven andere informatie weer.
![image](https://user-images.githubusercontent.com/58031089/120306557-42870e00-c2d2-11eb-8c45-487d23c1616e.png)  

## Implementatie in .NET Core 5
Om de ILogger interface aan te kunnen moeten we via dependency injection een private readonly maken:
```csharp
private readonly ILogger<WeatherForecastController> _logger;

public WeatherForecastController(ILogger<WeatherForecastController> logger)
{
     _logger = logger;
}
```  
De Log Levels kunnen aangeroepen worden d.m.v. methodes binnen logger. De methodes zijn bij de Log Levels eerder in dit bestand te vinden.  
Van te voren moet je er voor zorgen dat je in appsettings.json je controller of ander bestand binnen je project geconfigureerd hebt, anders zullen de logs zich niet laten zien.  
appsettings.json:  
```json
{
  "AllowedHosts": "*",
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "LoggingDemoAPI.Controllers.WeatherForecastController": "Trace"
    }
  }
}
```  
De aangegeven Log Levels zijn de minimale logs die weergegeven worden, bijvoorbeeld; vanaf Warning worden Warning, Error en Critical gedisplayed en bij Trace worden alle logs gedisplayed.  
Om alle Log Levels uit te proberen heb ik LoggingDemoAPI.Controllers.WeatherForecastController veranderd naar Trace.  
Mijn test logs:  
```csharp
_logger.LogInformation("some info");
_logger.LogDebug("Debug");
_logger.LogTrace("Tracing....");
_logger.LogError("APP crashed");
_logger.LogCritical("something was breached");
_logger.LogWarning("alert something is probably not right");
```

De output:  
![image](https://user-images.githubusercontent.com/58031089/120362837-cf9a8900-c30b-11eb-8fe1-16a566e707cb.png)  
Alles wordt dus netjes gedisplayed. Het is ook mogelijk om variabelen te laten zien, of een log ID mee te geven.

Variabelen en Id's:  
```csharp
 var i = 10;
var id = 14;
_logger.LogWarning(id,"whoops server timed out.. {Servertime}ms", i);
```

## Workshop
