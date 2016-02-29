Example Project
This is an example project to Monitor Server status. A weekend project one might call it.

This is written in C# 6 using Visual Studio 2015 targeting .NET 4.6.1. C# 6 is required because I find the "Expression Body" pretty clean.

Preparation to Run:
    For simplicity; it'll be assumed that a copy of Visual Studio 2015 is available.
    A Browser is available and/or Fiddler (or Charles for the mac-olytes)

Running:
    Open the ServerTrack.sln in Visual Studio. There will be 2 projects; Primary and Unit Tests.
    The primary project is "ServerTrack". This contains the end points for recording a server load and for displaying the average information for a server.
    From what ever your prefered "Start" method is out of visual studio; start the project. It is set as the StartUp project.

Started:
    This should start the project; and likely (configuration dependant, but does by default) open a browser.

ServerLoads:
    To provide a serverload; the URL must have the following path
        api/v1/record/{serverName}/cpu/{cpuLoad}/ram/{ramLoad}/
    where {serverName} is a string and {cpuLoad} and {ramLoad} are doubles
    Make GET requests to the url and the data will be stored and averaged for the server specified.
    A full url for this will look like - http://localhost:2708/api/v1/record/testServerGuy/cpu/76.123/ram/53.123/
    All entries with the same servername will be computed together.
    It is very important that the record load url end in a slash. Any doubles that have a decimal point will not work. This wasn't detected until late in the flow; and not enough time to correct it.
    Lame, I know - but a weekend project isn't going to hit all the edge cases perfectly.


Display Data:
    To display data; the url most have the following path
        api/v1/display/{serverName}
    A full url for this will look like - http://localhost:2708/api/v1/display/testServerGuy
    The result will be a json string looking something like
        {"ServerName":"testServer","MinuteResolutionLoads":{"CpuLoad":[76.12,0.0,0.0,0.0,0.0,76.0],"RamLoad":[53.12,0.0,0.0,0.0,0.0,53.0]},"HourResolutionLoads":{"CpuLoad":[25.35],"RamLoad":[17.69]}}

