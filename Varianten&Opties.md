# Zijn er andere opties dan logging?

## Table of Contents
- Serilog  
- NLog  

## Serilog  
Serilog is een van de eerste opties die ik heb leren kennen via de eerder benoemde Tim Corey, met Serilog kun je overzichtelijkere logs krijgen wat logging nog handiger maakt. voor het implementeren zul je eerst naar Program.cs moeten:  
```csharp
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()  // Hier wordt de appsettings.json aangeroepen
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration) // Hier wordt de appsettings.json Serilog configuratie geïmplementeerd
                .CreateLogger();

            try
            {
                Log.Information("Starting up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog(); // voeg dit toe
    }
```  
Op deze plek is het mogelijk alles te configureren, maar mij leek het verstandiger om dat in appsettings.json te doen zodat het niet hard-coded is.  
Onderweg moet je 3 NuGetPackages installeren:
- Serilog (2.10.0)
- Serilog.Settings.Configuration (3.1.0)  
- Serilog.AspNetCore (4.1.0)  

De try, catch, finally is optioneel, dit is om de start up van de applicatie te loggen.  

Als laatste moet er nog een app.Use toegevoegd worden aan de startup.cs file:  
```csharp
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LoggingDemoAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging(); // Voeg dit toe

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
```  
Zorg ervoor dat deze using voor Routing staat, anders komen er errors.  

In de appsettings.json file wordt alles geconfigureerd, net als bij de standaard ingebouwde logging.

appsettings.json file:  
```json
  "Serilog": {
    "Using": [],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "Enrich": [ "FromlogContext", "WithMachineName", "WithProcessId", "WithThreadId" ],
    "WriteTo": [
      { "Name": "Console" }
    ]
  }
```  
Alles spreekt voor zich als je het originele loggen al kent, behalve "Enrich". Enrich zorgt ervoor dat er zaken buiten de applicatie gelogt kunnen worden, zoals bijvoorbeeld de Machine naam (voorbeeld: DESKTOP-UC-24K).   
Om Enrich te gebruiken heb je 3 NuGet Packages nodig:  
- Serilog.Enrichers.Envirement (2.1.3)
- Serilog.Enrichers.Process (2.0.1)
- Serilog.Enrichers.Thread (3.1.0)  

Verder ga ik niet op Enrich in want dit is voor mij op dit moment niet heel relevant, wel wilde ik dit graag even benoemen.

Maar wat krijgen we nu als output?

De output:  
![image](https://user-images.githubusercontent.com/58031089/120633308-49488900-c46a-11eb-955e-8d471c40e3c0.png)  
Wat mij het eerste op valt is dat de tijd automatisch wordt weergegeven en er veel meer overzicht is t.o.v. originele logging. Alle variabelen worden paars gekleurd, zo springen ze er iets meer uit.  

## NLog  
Voor NLog heb ik een [tutorial van Shad Sluiter](https://www.youtube.com/watch?v=PnlxRmHg0lU&t=155s) gevolgd. Om NLog te gruiken hebben we een paar NuGet packages nodig:  
- NLog 
- NLog.Config (zorgt voor de .config file)
- NLog.Schemas (zorgt voor intellisense in de .config file)  
  
De NLog.config file:
```html
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      xsi:schemaLocation="http://www.nlog-project.org/schemas/NLog.xsd NLog.xsd"
      autoReload="true"
      throwExceptions="false"
      internalLogLevel="Off" internalLogFile="c:\temp\nlog-internal.log">

  <!-- optional, add some variables
  https://github.com/nlog/NLog/wiki/Configuration-file#variables
  -->
  <variable name="myvar" value="myvalue"/>

  <!--
  See https://github.com/nlog/nlog/wiki/Configuration-file
  for information on customizing logging rules and outputs.
   -->
  <targets>
    <targets>
      <!-- write to file -->
      <target name="f" xsi:type="File" fileName="${basedir}/logs/${shortdate}.log"
              layout="${longdate} - ${message} -   
        ${exception:format=StackTrace}${newline}" />
    </targets>
    <!--
    add your targets here
    See https://github.com/nlog/NLog/wiki/Targets for possible targets.
    See https://github.com/nlog/NLog/wiki/Layout-Renderers for the possible layout renderers.
    -->

    <!--
    Write events to a file with the date in the filename.
    <target xsi:type="File" name="f" fileName="${basedir}/logs/${shortdate}.log"
            layout="${longdate} ${uppercase:${level}} ${message}" />
    -->
  </targets>

  <rules>
    <!-- add your logging rules here -->

    <!--
    Write all events with minimal level of Debug (So Debug, Info, Warn, Error and Fatal, but not Trace)  to "f"
        -->
    <logger name="*" minlevel="Trace" writeTo="f" />

  </rules>
</nlog>
```  
Hier moet je een target kopiëren en plakken en uit commenten:  
```html
      <target name="f" xsi:type="File" fileName="${basedir}/logs/${shortdate}.log"
              layout="${longdate} - ${message} -   
        ${exception:format=StackTrace}${newline}" />
```
Er is heel veel mogelijk in de .config file, maar ik wil alleen de basis laten zien. Als deze target is geconfigureerd wordt er automatisch een huidige datum en tijd toegevoegd aan de log en ook het loglevel en de melding zelf wordt weergegeven.

In de program.cs file moet deze regel worden toegevoegd aan de ConfigureLogging methode:
```csharp
Logging.AddNLog();
```

In de rules van de .config file kunnen loglevels en de locatie waar de logs naar worden weggeschreven geconfigureerd worden (wat bij eerdere loggers in de appsettings.json file werd gedaan.  
Kortom, NLog is anders dan de concurrentie. NLog is naar mijn idee overzichtelijker, omdat het zijn eigen losse .config file heeft en niet in appsettings.json geconfigureerd hoeft te worden.

