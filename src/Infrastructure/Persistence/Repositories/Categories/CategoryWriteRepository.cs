using Application.Abstractions.Repositories.Categories;
using Domain;
using Domain.Entities;
using Persistence.Contexts;
using Persistence.Repositories.Commons;

namespace Persistence.Repositories.Categories
{
    public class CategoryWriteRepository(CurrencyContext context) : WriteRepository<Category>(context), ICategoryWriteRepository
    {
    }
}
