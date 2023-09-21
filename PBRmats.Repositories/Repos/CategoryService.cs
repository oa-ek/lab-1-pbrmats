using PBRmats.Core.Context;
using PBRmats.Core.Entities;
using PBRmats.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBRmats.Repositories.Repos
{
    public class CategoryService : IListService<Category>
    {
        private readonly PBRmatsContext _context;

        public CategoryService(PBRmatsContext context) => 
            _context = context;

        public ICollection<Category> GetList() =>
            _context.Categories.ToList();
    }
}
