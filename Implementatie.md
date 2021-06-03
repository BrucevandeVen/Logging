# Hoe is logging te gebruiken?

## Table Of Contents
*Introductie*  
  
*Library*  
- Log Levels
  
*Implementatie in .NET Core 5*  
- appsettings.json
- Log parameters (variabelen & Id's)  
- Debugger en Console  
  
*Errorhandling*


## Introductie
Ik heb een [simpele API](https://github.com/BrucevandeVen/Logging/tree/main/LoggingDemoAPI/LoggingDemoAPI) gemaakt waar ik logging op heb toegepast. hier worden alle onderstaande voorbeelden kort laten zien in de praktijk.

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

### Log parameters (variabelen & Id's)  
Alles wordt dus netjes gedisplayed. Het is ook mogelijk om variabelen te laten zien, of een log ID mee te geven.

Variabelen en Id's:  
```csharp
var i = 10;
var id = 14;
_logger.LogWarning(id,"whoops server timed out.. {Servertime}ms", i);
```  
Een vaakgebruikte functie, is het gebruiken van DateTime.Now in de variabelen, om een tijdsperceptie te creëren.

De output:  
![image](https://user-images.githubusercontent.com/58031089/120365297-8c8de500-c30e-11eb-81d1-4b1381129221.png)  

D.m.v. {..} kunnen variabelen na de komma ingevoerd worden, de logId moet voor de message gegeven worden.  
![image](https://user-images.githubusercontent.com/58031089/120365190-6b2cf900-c30e-11eb-8df3-54c9cd9d8e49.png)  

### appsettings.json
Van te voren moet je er voor zorgen dat je in appsettings.json je controller of ander bestand binnen je project geconfigureerd hebt, anders zullen de logs zich niet laten zien. Let op, het is mogelijk dat er een appsettings.developement.json is aangemaakt (automatisch), deze kan je verwijderen of invullen. De appsettings.developement.json file is dominant over de appsettings.json files, dus je zult hier je aanpassingen in moeten verrichten wil je resultaat zien. (voor grotere applicaties is aan te raden dit gescheiden te houden).  

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
Zelf heb je waarschijnlijk nog niet je eigen API controller hier bij staan, deze zul je zelf moeten toevoegen. Normaal staan hier ook Microsoft en Microsoft.Hosting.Lifetime bij, deze zie je altijd bij het opstarten van de applicatie (mits je deze niet verwijderd heb net als hierboven). 
De aangegeven Log Levels zijn de minimale logs die weergegeven worden, bijvoorbeeld; vanaf Warning worden Warning, Error en Critical gedisplayed en bij Trace worden alle logs gedisplayed. Om alle Log Levels uit te proberen heb ik LoggingDemoAPI.Controllers.WeatherForecastController veranderd naar Trace. Het is dus mogelijk om per klasse of service een ander LogLevel minimum in te stellen, dit kan handig zijn bij grotere projecten. 
Trace moet je zo min mogelijk gebruiken (behalve voor eigen gemaakte klassen/services), omdat deze heel veel weergeeft als je Micrsosoft toestemming geeft Trace te loggen. 

### Debugger en Console  
De debugger en console kunnen beide gebruikt worden om log informatie te laten zien. De default zet alles open en laat het op beide plekken zien, als je het anders wilt zul je logger anders moeten configureren in de Program.cs file. We gaan de configureLogging methode gebruiken om de debugger aan of uit te zetten.  
Voorbeeld:  
```csharp
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                    logging.AddConsole(); // Gebruik console voor logging
                    logging.AddDebug(); // Gebruik Debugger voor logging
                })
```  
Als je geen console of debugger wil gebruiken voor logging kun je deze simpelweg verwijderen of uit commenten. Let op dat context.Configuration.GetSection() dezelfde naam krijgt gegeven als in je appsettings.json file (in mijn geval "Logging").  
In appsettings.json is het mogelijk om te specificeren waar je welke logs wil laten zien (debugger of Console), dat doe je door de Console en Debug default (of andere klassen/services) te specificeren:  
```json
{
  "AllowedHosts": "*",
  "Logging": {
    "Console": {
      "LogLevel": {
        "Default": "Critical"
      }
    },
    "Debug": {
      "LogLevel": {
        "Default": "Warning"
      }
    },
    "LogLevel": {
      "Default": "Warning",
      "LoggingDemoAPI.Controllers.WeatherForecastController": "Trace"
    }
  }
}
```  

De output van de Console (voorbeeld):  
![image](https://user-images.githubusercontent.com/58031089/120620661-7b072300-c45d-11eb-801b-aa954ba9f7e2.png)  
Zoals je ziet zijn alleen de Critical logs zichtbaar, zonder dat ik de code heb veranderd.

De output van de Debugger (voorbeeld):
![image](https://user-images.githubusercontent.com/58031089/120620902-b6a1ed00-c45d-11eb-8c51-73002572c636.png)  
Nu zijn er er veel meer logs te zien.  

## Errorhandling en Debuggen 
Logging maakt het heel gemakkelijk om errors op te sporen, voor meer kijk mijn stukje over [Het doel van Logging](https://github.com/BrucevandeVen/Logging/blob/main/Logging_Concreet.md).  
hiervoor wordt vooral Try & Catch gebruikt. Als de "Try" faalt, vangt de "Catch" het op met wat je daar ook geschreven hebt (veelal komt hier logging aan te pas).  

Try & Catch:
```csharp
            try
            {
                throw new Exception("ERROR ERROR ERROR");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{time} There was an exception", DateTime.UtcNow);
            }
```  
Er wordt een exception gecreëerd en deze wordt gelogt.  

De output:  
![image](https://user-images.githubusercontent.com/58031089/120617922-daafff00-c45a-11eb-9af6-3c18903f22b9.png)



