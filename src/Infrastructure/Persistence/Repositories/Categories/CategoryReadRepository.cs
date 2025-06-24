using Application.Abstractions.Repositories.Categories;
using Domain.Entities;
using Persistence.Contexts;
using Persistence.Repositories.Commons;

namespace Persistence.Repositories.Categories
{
    public class CategoryReadRepository : ReadRepository<Category>, ICategoryReadRepository
    {
        public CategoryReadRepository(CurrencyContext context) : base(context)
        {
        }
    }
}
