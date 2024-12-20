using CSharpFunctionalExtensions;

namespace ASSISTENTE.Infrastructure.Audio.Contracts;

public interface IAudioClient
{
    Task<Result<Transcription>> GenerateTranscription(AudioFile audioFile);
}