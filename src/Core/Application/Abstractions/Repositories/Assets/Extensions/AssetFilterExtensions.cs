using System.Linq.Expressions;
using Application.Models.RequestParameters.Assets;
using Application.Utilities.Helpers;
using Domain;

namespace Application.Abstractions.Repositories.Assets.Extensions
{
    public static class CategoryFilterExtensions
    {
        public static IQueryable<Asset> FilterAllConditions(this IQueryable<Asset> source, AssetBaseRequestParameter parameter)
        {
            source = source.Filter(parameter);

            // source = source.Search(parameter.Condition);

            return source.OrderQuery(parameter.OrderBy);
        }

        public static IQueryable<Asset> Filter(this IQueryable<Asset> source, AssetBaseRequestParameter parameter)
        {
            var predicate = PredicateBuilderHelper.True<Asset>();

            if (parameter.CurrencyId is not null && parameter.CurrencyId.Any())
                predicate = predicate.And(x => parameter.CurrencyId.Contains(x.CurrencyId));

            if (parameter.IsActive is not null)
                predicate = predicate.And(x => x.IsActive == parameter.IsActive);

            return source.Where(predicate);
        }

        // public static IQueryable<Asset> Search(this IQueryable<Asset> source, string? condition)
        // {
        //     if (string.IsNullOrWhiteSpace(condition))
        //         return source;

        //     string normalizedCondition = condition.TrimStart().TrimEnd().ToLower();

        //     return source.Where(s =>
        //         s..ToLower().Contains(normalizedCondition)
        //     );
        // }

        public static IQueryable<Asset> OrderQuery(this IQueryable<Asset> source, string? orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                return source.OrderBy(x=> x.Id);

            string normalizedConditiom = orderBy.TrimStart().TrimEnd().ToLower();
            
            string[] orderByQuery = orderBy.Split('_');

            Expression<Func<Asset, object>> keySelector = orderByQuery[0] switch
            {
                "count" => asset => asset.Count,
                "p_date" => asset => asset.PurchaseDate,
                "p_price" => asset => asset.PurchasePrice,
                "s_price" => asset => asset.SalePrice,
                "c_p_price" => asset => asset.CurrentPurchasePrice,
                "c_s_price" => asset => asset.CurrentSalePrice,
                "created" => asset => asset.CreatedDate,
                _ => asset => asset.Id
            };

            if(normalizedConditiom.Contains("_desc"))
                source = source.OrderByDescending(keySelector);
            else
                source = source.OrderBy(keySelector);

            return source;
        }

        public static IQueryable<Asset> OrderSplitQuery(this IQueryable<Asset> source, string? orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                return source;

            string normalizedConditiom = orderBy.TrimStart().TrimEnd().ToLower();

            string[] orderByQuery = orderBy.Split('_');

            Expression<Func<Asset, object>> keySelector = orderByQuery[0] switch
            {
                "count" => asset => asset.Count,
                "p_date" => asset => asset.PurchaseDate,
                "p_price" => asset => asset.PurchasePrice,
                "s_price" => asset => asset.SalePrice,
                "c_p_price" => asset => asset.CurrentPurchasePrice,
                "c_s_price" => asset => asset.CurrentSalePrice,
                "created" => asset => asset.CreatedDate,
                _ => asset => asset.Id
            };

            if (normalizedConditiom.Contains("_desc"))
                source = source.OrderByDescending(keySelector).ThenByDescending(x=> x.Id);
            else
                source = source.OrderBy(keySelector).ThenBy(x=> x.Id);

            return source;
        }
    }
}