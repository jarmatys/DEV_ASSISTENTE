using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Domain.Entities.Questions.Interfaces;
using ASSISTENTE.Language.Identifiers;
using ASSISTENTE.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ASSISTENTE.Persistence.Repositories;

internal sealed class QuestionRepository(IAssistenteDbContext context) 
    : BaseRepository<Question, QuestionId>(context), IQuestionRepository
{
    private readonly IAssistenteDbContext _context = context;

    protected override IQueryable<Question> Get()
    {
        return _context.Questions
            .Include(x => x.Answer)
            .Include(x => x.Files)
            .Include(x => x.Resources)
            .ThenInclude(x => x.Resource);
    }

    protected override IQueryable<Question> List()
    {
        return Get()            
            .Where(x => x.Context != null)
            .OrderByDescending(x => x.Created);
    }
}