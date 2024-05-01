using ASSISTENTE.Contract.Requests.Internal.Common.RequestBases;

namespace ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetQuestions;

public sealed class GetQuestionsRequest : GetRequestBase
{
    public static GetQuestionsRequest Create()
    {
        return new GetQuestionsRequest();
    }
}