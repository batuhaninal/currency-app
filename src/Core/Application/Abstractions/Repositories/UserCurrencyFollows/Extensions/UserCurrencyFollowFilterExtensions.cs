using System.Linq.Expressions;
using Application.Models.RequestParameters.UserCufrrencyFollows;
using Application.Utilities.Helpers;
using Domain.Entities;

namespace Application.Abstractions.Repositories.UserCurrencyFollows.Extensions
{
    public static class UserCurrencyFollowFilterExtensions
    {
        public static IQueryable<UserCurrencyFollow> FilterAllConditions(this IQueryable<UserCurrencyFollow> source, UserCurrencyFollowBaseRequestParameter parameter)
        {
            source = source.Filter(parameter);

            // source = source.Search(parameter.Condition);

            return source.OrderQuery(parameter.OrderBy);
        }

        public static IQueryable<UserCurrencyFollow> Filter(this IQueryable<UserCurrencyFollow> source, UserCurrencyFollowBaseRequestParameter parameter)
        {
            var predicate = PredicateBuilderHelper.True<UserCurrencyFollow>();

            if (parameter.CurrencyIds is not null && parameter.CurrencyIds.Any())
                predicate = predicate.And(x => parameter.CurrencyIds.Contains(x.CurrencyId));

            // if (parameter.UserIds is not null && parameter.UserIds.Any())
            //     predicate = predicate.And(x => parameter.UserIds.Contains(x.UserId));

            if (parameter.IsActive is not null)
                predicate = predicate.And(x => x.IsActive == parameter.IsActive);

            if(parameter.MinUpdatedDate is not null)
                predicate = predicate.And(x=> x.UpdatedDate >= parameter.MinUpdatedDate.Value);
            
            if(parameter.MaxUpdatedDate is not null)
                predicate = predicate.And(x=> x.UpdatedDate <= parameter.MaxUpdatedDate.Value);

            return source.Where(predicate);
        }

        // public static IQueryable<UserCurrencyFollow> Search(this IQueryable<UserCurrencyFollow> source, string? condition)
        // {
        //     if (string.IsNullOrWhiteSpace(condition))
        //         return source;

        //     string normalizedCondition = condition.TrimStart().TrimEnd().ToLower();

        //     return source.Where(s =>
        //         s..ToLower().Contains(normalizedCondition)
        //     );
        // }

        public static IQueryable<UserCurrencyFollow> OrderQuery(this IQueryable<UserCurrencyFollow> source, string? orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                return source.OrderByDescending(x=> x.UpdatedDate);

            string normalizedConditiom = orderBy.TrimStart().TrimEnd().ToLower();
            
            string[] orderByQuery = orderBy.Split('_');

            Expression<Func<UserCurrencyFollow, object>> keySelector = orderByQuery[0] switch
            {
                // "user_id" => uah => uah.UserId,
                "currency_id" => uah => uah.CurrencyId,
                "created" => uah => uah.CreatedDate,
                "updated" => uah => uah.UpdatedDate,
                _ => uah => uah.Id
            };

            if(normalizedConditiom.Contains("_desc"))
                source = source.OrderByDescending(keySelector);
            else
                source = source.OrderBy(keySelector);

            return source;
        }

        public static IQueryable<UserCurrencyFollow> OrderSplitQuery(this IQueryable<UserCurrencyFollow> source, string? orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                return source
                    .OrderByDescending(x => x.UpdatedDate)
                        .ThenBy(x=> x.Id);

            string normalizedConditiom = orderBy.TrimStart().TrimEnd().ToLower();

            string[] orderByQuery = orderBy.Split('_');

            Expression<Func<UserCurrencyFollow, object>> keySelector = orderByQuery[0] switch
            {
                // "user_id" => uah => uah.UserId,
                "currency_id" => uah => uah.CurrencyId,
                "created" => uah => uah.CreatedDate,
                "updated" => uah => uah.UpdatedDate,
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