using ASSISTENTE.Infrastructure.Audio.Contracts;
using ASSISTENTE.Infrastructure.LLM.OpenAi.Errors;
using CSharpFunctionalExtensions;
using OpenAI;
using OpenAI.Audio;
using OpenAI.Models;

namespace ASSISTENTE.Infrastructure.Audio;

internal sealed class OpenAiClient(OpenAIClient client) : IAudioClient
{
    public async Task<Result<Transcription>> GenerateTranscription(AudioFile audioFile)
    {
        var transcriptionRequest = new AudioTranscriptionRequest(
            audio: audioFile.Stream,
            audioName: audioFile.Name,
            model: Model.Whisper1,
            responseFormat: AudioResponseFormat.Text,
            language: "pl"
        );

        var transcriptionText = await client.AudioEndpoint.CreateTranscriptionTextAsync(transcriptionRequest);
        
        if (transcriptionText is null)
            return Result.Failure<Transcription>(ClientErrors.EmptyAnswer.Build());
        
        return Transcription.Create(transcriptionText);
    }
}