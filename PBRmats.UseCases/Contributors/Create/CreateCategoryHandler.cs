using PBRmats.Core.Entities;
using PBRmats.Repositories.Interfaces;

namespace PBRmats.UseCases.Contributors.Create
{
    public class CreateCategoryHandler
    {
        private readonly IRepository<Category, int> _categoryRepository;

        public CreateCategoryHandler(IRepository<Category, int> categoryRepository) => 
            _categoryRepository = categoryRepository;

        public int Handle(Category category)
        {
            _categoryRepository.Create(category);

            return category.Id;
        }
    }
}
