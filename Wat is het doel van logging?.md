# Wat is het doel van logging?

# Table of Contents
- Library
- Logging concreet
- Waarom logging?

## Library  
Als eerste, ben ik gaan kijken wat er al bestaat over logging. Ik heb al een aantal bronnen gevonden die mij logging kunnen leren.
De eerste bron die ik vond was [Tim Corey's video](https://www.youtube.com/watch?v=_iryZxv8Rxw&ab_channel=IAmTimCorey) over Logging.
Tim Corey legt haarfijn uit d.m.v. voorbeelden hoe logging werkt in .NET. 
Verder heb ik ook de officiële [Microsoft documentatie over .NET Logging](https://docs.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line) bekeken, deze was echter meer relevant voor de implementatie.  
Een apart onderzoek dat meer in gaat op de "Why's" van logging, is het artikel op [SolarWinds Loggly](https://www.loggly.com/ultimate-guide/net-logging-basics/). In het artikel wordt beschreven waarom logging handig kan zijn en wat het in feiten is.  

## Logging concreet  
Logging kan fungeren als een manier van errorhandling, maar ook debug analysis en performance reviews worden gemakkelijker gemaakt met logging. Het is mogelijk om naar verschillende soorten bestanden te loggen i.p.v. alleen de command line. Je kan loggen naar een database of bijvoorbeeld Windows Server. In dit onderzoek ga ik alleen focussen op de command line in .NET.  
Logging is in principe een waarde die je wegschrijft naar een plek (zoals de command line), als er een bepaald stukje dit zegt. 

## Waarom logging?
Logging wordt vooral gebruikt om het debuggen gemakkelijker te maken en perfomance te reviewen, zo hoef je niet meer bulletpoints te gebruiken, maar kan je op één punt de situatie bekijken en direct naar plaats delict gaan om het probleem snel op te lossen. Logging scheelt vooral veel tijd en houdt de code gemakkelijker onderhoudbaar. 
