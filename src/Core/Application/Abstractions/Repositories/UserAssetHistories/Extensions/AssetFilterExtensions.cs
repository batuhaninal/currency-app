using System.Linq.Expressions;
using Application.Models.RequestParameters.UserAssetHistories;
using Application.Utilities.Helpers;
using Domain.Entities;

namespace Application.Abstractions.Repositories.UserAssetHistories.Extensions
{
    public static class UserAssetHistoryFilterExtensions
    {
        public static IQueryable<UserAssetHistory> FilterAllConditions(this IQueryable<UserAssetHistory> source, UserAssetHistoryBaseRequestParameter parameter)
        {
            source = source.Filter(parameter);

            // source = source.Search(parameter.Condition);

            return source.OrderQuery(parameter.OrderBy);
        }

        public static IQueryable<UserAssetHistory> Filter(this IQueryable<UserAssetHistory> source, UserAssetHistoryBaseRequestParameter parameter)
        {
            var predicate = PredicateBuilderHelper.True<UserAssetHistory>();

            if (parameter.StartDate is not null)
                predicate = predicate.And(x => x.Date >= parameter.StartDate.Value);

            if (parameter.EndDate is not null)
                predicate = predicate.And(x => x.Date <= parameter.EndDate.Value);

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

        public static IQueryable<UserAssetHistory> OrderQuery(this IQueryable<UserAssetHistory> source, string? orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                return source.OrderByDescending(x=> x.Date);

            string normalizedConditiom = orderBy.TrimStart().TrimEnd().ToLower();
            
            string[] orderByQuery = orderBy.Split('_');

            Expression<Func<UserAssetHistory, object>> keySelector = orderByQuery[0] switch
            {
                "date" => uah => uah.Date,
                "t-p-price" => uah => uah.TotalPurchasePrice,
                "t-s-price" => uah => uah.TotalSalePrice,
                "t-cu-p-price" => uah => uah.TotalCurrentPurchasePrice,
                "t-cu-s-price" => uah => uah.TotalCurrentSalePrice,
                "t-co-p-price" => uah => uah.TotalCostPurchasePrice,
                "t-co-s-price" => uah => uah.TotalCostSalePrice,
                "created" => uah => uah.CreatedDate,
                _ => uah => uah.Id
            };

            if(normalizedConditiom.Contains("_desc"))
                source = source.OrderByDescending(keySelector);
            else
                source = source.OrderBy(keySelector);

            return source;
        }

        public static IQueryable<UserAssetHistory> OrderSplitQuery(this IQueryable<UserAssetHistory> source, string? orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                return source;

            string normalizedConditiom = orderBy.TrimStart().TrimEnd().ToLower();

            string[] orderByQuery = orderBy.Split('_');

            Expression<Func<UserAssetHistory, object>> keySelector = orderByQuery[0] switch
            {
                "date" => uah => uah.Date,
                "t-p-price" => uah => uah.TotalPurchasePrice,
                "t-s-price" => uah => uah.TotalSalePrice,
                "t-cu-p-price" => uah => uah.TotalCurrentPurchasePrice,
                "t-cu-s-price" => uah => uah.TotalCurrentSalePrice,
                "t-co-p-price" => uah => uah.TotalCostPurchasePrice,
                "t-co-s-price" => uah => uah.TotalCostSalePrice,
                "created" => uah => uah.CreatedDate,
                _ => uah => uah.Id
            };

            if (normalizedConditiom.Contains("_desc"))
                source = source.OrderByDescending(keySelector).ThenByDescending(x=> x.Id);
            else
                source = source.OrderBy(keySelector).ThenBy(x=> x.Id);

            return source;
        }
    }
}