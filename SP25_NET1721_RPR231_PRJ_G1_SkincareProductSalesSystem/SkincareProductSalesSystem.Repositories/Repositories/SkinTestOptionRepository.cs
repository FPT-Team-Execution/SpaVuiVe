using Microsoft.EntityFrameworkCore;
using SkincareProductSalesSystem.Repositories.Base;
using SkincareProductSalesSystem.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Repositories.Repositories
{
    public class SkinTestOptionRepository : GenericRepository<SkinTestOption>
    {
        public new async Task<SkinTestOption?> GetByIdAsync(string id)
        {
            return await _context.Set<SkinTestOption>().FindAsync(id);
        }
        public async Task<SkinTestOption?> GetByIdAsync(string questionId, string optionId)
        {
            return await _context.Set<SkinTestOption>().FirstOrDefaultAsync(x => x.QuestionId.Equals(questionId) && x.OptionId.Equals(optionId));
        }
       
        public async Task<List<SkinTestOption>> GetByQuestionIdAsync(string questionId)
        {
            return await _context.Set<SkinTestOption>().Where(x => x.QuestionId.Equals(questionId)).ToListAsync();
        }
    }
}
