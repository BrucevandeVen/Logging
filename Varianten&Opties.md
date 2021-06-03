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

Maar wat krijgen we nu als output?

De output:  
![image](https://user-images.githubusercontent.com/58031089/120633308-49488900-c46a-11eb-955e-8d471c40e3c0.png)  
Wat mij het eerste op valt is dat de tijd automatisch wordt weergegeven en er veel meer overzicht is t.o.v. originele logging. Alle variabelen worden paars gekleurd, zo springen ze er iets meer uit.  

## NLog
