using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SOFTURE.Contract.Common.RequestBases;

namespace ASSISTENTE.API.Agent.Models;

public sealed class InstructionRequest : PostRequestBase
{
    [Required]
    [JsonPropertyName("instruction")]
    public string Instruction { get; set; } = null!;

    public override void Clear()
    {
        Instruction = string.Empty;
    }
};