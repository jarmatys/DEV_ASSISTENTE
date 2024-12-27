using ASSISTENTE.API.Agent;
using ASSISTENTE.API.Agent.Extensions;
using ASSISTENTE.API.Agent.Models;
using ASSISTENTE.Infrastructure.LLM.Contracts;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using SOFTURE.Settings.Extensions;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>()
    .Build()
    .ValidateSettings<ApiSettings>();

var builder = WebApplication
    .CreateBuilder(args)
    .ConfigureSettings<ApiSettings>(configuration)
    .AddCommon<ApiSettings>()
    .AddModules<ApiSettings>();

var app = builder.Build();

app.MapPost("/api/instructions", async (
        [FromBody] InstructionRequest payload,
        [FromServices] ILlmClient llmClient,
        [FromServices] ILogger<Program> logger) =>
    {
        logger.LogInformation(payload.Instruction);

        var directionPrompt = $"""
                               Na podstawie instrukcji, podaj jakie ruchy zostały wykonane na planszy 4 x 4

                               <INSTRUKCJA>
                               {payload.Instruction}
                               </INSTRUKCJA>

                               <ZASADY>
                               1. Zwróć tylko i wyłącznie kierunki ruchu. 
                               2. Nie zwracaj żadnej dodatkowej informacji
                               </ZASADY>

                               <PRZYKŁAD>
                               INSTRUKCJA: "Sluchaj kolego. Lecimy na maksa w prawo, a pózniej ile wlezie w dól. Co tam widzisz?"
                               ODPOWIEDŹ: RIGHT, RIGHT, RIGHT, DOWN, DOWN, DOWN
                               </PRZYKŁAD>
                               """;

        var directions = await Prompt.Create(directionPrompt)
            .Bind(async prompt => await llmClient.GenerateAnswer(prompt))
            .GetValueOrDefault(x => x.Text);

        logger.LogInformation(directions);

        const string matrix = """
                              | Rząd/Kolumna | 1               | 2             | 3                         | 4               |
                              |--------------|-----------------|---------------|---------------------------|-----------------|
                              | 1            | Tu zaczynasz    | Pusta trawa   | Jedno drzewo              | Dom             |
                              | 2            | Pusta trawa     | Młyn          | Pusta trawa               | Pusta trawa     |
                              | 3            | Pusta trawa     | Pusta trawa   | Skały                     | Dwa drzewa      |
                              | 4            | Góry            | Góry          | Samochód                  | Jaskinia        |
                              """;

        var masterPrompt = $"""
                            Na podstawie otrzymanej instrukcji zgłoś nad jakim polem znajduje się obiekt.

                            <INSTRUKCJA>
                            {directions}
                            </INSTRUKCJA>

                            <MACIERZ>
                            {matrix}
                            </MACIERZ>

                            Tabela opisuje siatkę punktów geograficznych w układzie 4x4. Każda komórka zawiera element lub opis lokalizacji. Oto szczegółowy opis w formie punktów geograficznych:

                            <ZASADY>
                            1. Zwróć tylko i wyłącznie nazwę pola, na którym znajduje się obiekt.
                            2. Zwrócony opis może mieć maksymalnie 2 słowa.
                            </ZASADY>
                            
                            <PRZYKŁAD>
                            Góry
                            </PRZYKŁAD>

                            </PRZYKŁAD>
                            Pusta trawa
                            </PRZYKŁAD>
                            """;

        var answer = await Prompt.Create(masterPrompt)
            .Bind(async prompt => await llmClient.GenerateAnswer(prompt))
            .GetValueOrDefault(x => x.Text);

        logger.LogInformation(answer);

        var response = new InstructionResponse
        {
            Description = answer!
        };

        return Results.Ok(response);
    })
    .WithName(nameof(InstructionRequest));

app.Run();