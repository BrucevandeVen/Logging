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

## Implementatie
Om de ILogger interface aan te kunnen moeten we via dependency injection een private readonly maken:
```csharp
private readonly ILogger<WeatherForecastController> _logger;

public WeatherForecastController(ILogger<WeatherForecastController> logger)
{
     _logger = logger;
}
```  
De Log Levels kunnen aangeroepen worden d.m.v. _logger methodes

## Workshop
