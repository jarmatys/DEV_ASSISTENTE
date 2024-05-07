using ASSISTENTE.Contract.Requests.Internal.Common;

namespace ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetQuestions;

public sealed class GetQuestionsRequest : PaginationRequestBase
{
    public static GetQuestionsRequest Create(int page, int elements)
    {
        return new GetQuestionsRequest
        {
            Page = page,
            Elements = elements
        };
    }
}