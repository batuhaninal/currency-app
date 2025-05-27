using System.Linq.Expressions;
using Application.Models.RequestParameters.Currencies;
using Application.Utilities.Helpers;
using Domain;

namespace Application.Abstractions.Repositories.Currencies.Extensions
{
    public static class CurrencyFilterExtensions
    {
        public static IQueryable<Currency> FilterAllConditions(this IQueryable<Currency> source, CurrencyBaseRequestParameter parameter)
        {
            source = source.Filter(parameter);

            source = source.Search(parameter.Condition);

            return source.OrderQuery(parameter.OrderBy);
        }

        public static IQueryable<Currency> Filter(this IQueryable<Currency> source, CurrencyBaseRequestParameter parameter)
        {
            var predicate = PredicateBuilderHelper.True<Currency>();

            if (parameter.CategoryId is not null)
                predicate = predicate.And(x => parameter.CategoryId.Contains(x.CategoryId));

            if (parameter.IsActive is not null)
                predicate = predicate.And(x => x.IsActive == parameter.IsActive);

            return source.Where(predicate);
        }

        public static IQueryable<Currency> Search(this IQueryable<Currency> source, string? condition)
        {
            if (string.IsNullOrWhiteSpace(condition))
                return source;

            string normalizedCondition = condition.TrimStart().TrimEnd().ToLower();

            return source.Where(s =>
                s.Title.ToLower().Contains(normalizedCondition)
            );
        }

        public static IQueryable<Currency> OrderQuery(this IQueryable<Currency> source, string? orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                return source;

            string normalizedConditiom = orderBy.TrimStart().TrimEnd().ToLower();
            
            string[] orderByQuery = orderBy.Split('_');

            Expression<Func<Currency, object>> keySelector = orderByQuery[0] switch
            {
                "title" => currency => currency.Title,
                "created" => currency => currency.CreatedDate,
                "isActive" => currency => currency.IsActive,
                _ => currency => currency.Id
            };

            if(normalizedConditiom.Contains("_desc"))
                source = source.OrderByDescending(keySelector);
            else
                source = source.OrderBy(keySelector);

            return source;
        }

        public static IQueryable<Currency> OrderSplitQuery(this IQueryable<Currency> source, string? orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                return source;

            string normalizedConditiom = orderBy.TrimStart().TrimEnd().ToLower();

            string[] orderByQuery = orderBy.Split('_');

            Expression<Func<Currency, object>> keySelector = orderByQuery[0] switch
            {
                "title" => currency => currency.Title,
                "created" => currency => currency.CreatedDate,
                "isActive" => currency => currency.IsActive,
                _ => currency => currency.Id
            };

            if (normalizedConditiom.Contains("_desc"))
                source = source.OrderByDescending(keySelector).ThenByDescending(x=> x.Id);
            else
                source = source.OrderBy(keySelector).ThenBy(x=> x.Id);

            return source;
        }
    }
}