using System.Linq.Expressions;
using Application.Models.RequestParameters.Categories;
using Application.Utilities.Helpers;
using Domain.Entities;

namespace Application.Abstractions.Repositories.Categories.Extensions
{
    public static class CategoryFilterExtensions
    {
        public static IQueryable<Category> FilterAllConditions(this IQueryable<Category> source, CategoryBaseRequestParameter parameter)
        {
            source = source.Filter(parameter);

            source = source.Search(parameter.Condition);

            return source.OrderQuery(parameter.OrderBy);
        }

        public static IQueryable<Category> Filter(this IQueryable<Category> source, CategoryBaseRequestParameter parameter)
        {
            var predicate = PredicateBuilderHelper.True<Category>();

            if (parameter.IsActive is not null)
                predicate = predicate.And(x => x.IsActive == parameter.IsActive);

            return source.Where(predicate);
        }

        public static IQueryable<Category> Search(this IQueryable<Category> source, string? condition)
        {
            if (string.IsNullOrWhiteSpace(condition))
                return source;

            string normalizedCondition = condition.TrimStart().TrimEnd().ToLower();

            return source.Where(s =>
                s.Title.ToLower().Contains(normalizedCondition)
            );
        }

        public static IQueryable<Category> OrderQuery(this IQueryable<Category> source, string? orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                return source;

            string normalizedConditiom = orderBy.TrimStart().TrimEnd().ToLower();
            
            string[] orderByQuery = orderBy.Split('_');

            Expression<Func<Category, object>> keySelector = orderByQuery[0] switch
            {
                "title" => category => category.Title,
                "created" => category => category.CreatedDate,
                "isActive" => category => category.IsActive,
                _ => category => category.Id
            };

            if(normalizedConditiom.Contains("_desc"))
                source = source.OrderByDescending(keySelector);
            else
                source = source.OrderBy(keySelector);

            return source;
        }

        public static IQueryable<Category> OrderSplitQuery(this IQueryable<Category> source, string? orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                return source;

            string normalizedConditiom = orderBy.TrimStart().TrimEnd().ToLower();

            string[] orderByQuery = orderBy.Split('_');

            Expression<Func<Category, object>> keySelector = orderByQuery[0] switch
            {
                "title" => category => category.Title,
                "created" => category => category.CreatedDate,
                "isActive" => category => category.IsActive,
                _ => category => category.Id
            };

            if (normalizedConditiom.Contains("_desc"))
                source = source.OrderByDescending(keySelector).ThenByDescending(x=> x.Id);
            else
                source = source.OrderBy(keySelector).ThenBy(x=> x.Id);

            return source;
        }
    }
}
