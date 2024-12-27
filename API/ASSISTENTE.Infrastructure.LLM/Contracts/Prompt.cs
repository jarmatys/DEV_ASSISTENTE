using CSharpFunctionalExtensions;
using SOFTURE.Results;

namespace ASSISTENTE.Infrastructure.LLM.Contracts;

public sealed class Prompt : ValueObject
{
    private Prompt(string value)
    {
        Value = value;
    }
    
    public string Value { get; }
    public string? Model { get; private set; }
    public string? System { get; private set; }
    
    public static Result<Prompt> Create(string prompt)
    {
        if (string.IsNullOrEmpty(prompt))
            return Result.Failure<Prompt>(CommonErrors.EmptyParameter.Build());
        
        return new Prompt(prompt);
    }
    
    public Result<Prompt> ChooseModel(string model)
    {
        if (string.IsNullOrEmpty(model))
            return Result.Failure<Prompt>(CommonErrors.EmptyParameter.Build());
        
        Model = model;
        
        return Result.Success(this);
    }
    
    public Result<Prompt> ConfigureSystem(string system)
    {
        if (string.IsNullOrEmpty(system))
            return Result.Failure<Prompt>(CommonErrors.EmptyParameter.Build());
        
        System = system;
        
        return Result.Success(this);
    }

    protected override IEnumerable<IComparable?> GetEqualityComponents()
    {
        yield return Value;
        yield return Model;
        yield return System;
    }
}