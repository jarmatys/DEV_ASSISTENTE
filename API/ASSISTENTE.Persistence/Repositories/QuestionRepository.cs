using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Domain.Entities.Questions.Interfaces;
using ASSISTENTE.Language.Identifiers;
using ASSISTENTE.Persistence.MSSQL;
using Microsoft.EntityFrameworkCore;

namespace ASSISTENTE.Persistence.Repositories;

internal sealed class QuestionRepository(IAssistenteDbContext context) 
    : BaseRepository<Question, QuestionId>(context), IQuestionRepository
{
    private readonly IAssistenteDbContext _context = context;

    protected override IQueryable<Question> Get()
    {
        return _context.Questions
            .Include(x => x.Resources);
    }
}