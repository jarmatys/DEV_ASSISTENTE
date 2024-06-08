using ASSISTENTE.Domain.Entities.Questions.Enums;

namespace ASSISTENTE.Domain.Entities.Questions;

public sealed partial class Question
{
    private void ConfigureStateMachine()
    {
        StateMachine.Configure(QuestionStates.Created)
            .OnActivate(() => State = QuestionStates.Created);
        
        StateMachine.Configure(QuestionStates.Created)
            .Permit(QuestionActions.ResolveContext, QuestionStates.ContextResolved);
        
        StateMachine.Configure(QuestionStates.ContextResolved)
            .Permit(QuestionActions.GenerateAnswer, QuestionStates.Answered);
    }
}

public enum QuestionActions
{
    Create = 1,
    ResolveContext = 2,
    GenerateAnswer = 3,
}