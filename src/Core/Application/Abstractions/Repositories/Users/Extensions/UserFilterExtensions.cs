using System.Linq.Expressions;
using Application.Models.RequestParameters.Users;
using Application.Utilities.Helpers;
using Domain.Entities;

namespace Application.Abstractions.Repositories.Users.Extensions
{
    public static class UserFilterExtensions
    {
        public static IQueryable<User> FilterAllConditions(this IQueryable<User> source, UserRequestParameter parameter)
        {
            source = source.Filter(parameter);

            source = source.Search(parameter.Condition);

            return source.OrderQuery(parameter.OrderBy);
        }

        public static IQueryable<User> Filter(this IQueryable<User> source, UserRequestParameter parameter)
        {
            var predicate = PredicateBuilderHelper.True<User>();

            if (parameter.IsActive is not null)
                predicate = predicate.And(x => x.IsActive == parameter.IsActive);

            return source.Where(predicate);
        }

        public static IQueryable<User> Search(this IQueryable<User> source, string? condition)
        {
            if (string.IsNullOrWhiteSpace(condition))
                return source;

            string normalizedCondition = condition.TrimStart().TrimEnd().ToLower();

            return source.Where(s =>
                s.Email.ToLower().Contains(normalizedCondition) || 
                (s.FirstName + " " + s.LastName).Contains(normalizedCondition)
            );
        }

        public static IQueryable<User> OrderQuery(this IQueryable<User> source, string? orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                return source;

            
            string normalizedConditiom = orderBy.TrimStart().TrimEnd().ToLower();

            string[] orderByQUery = orderBy.Split('_');

            Expression<Func<User, object>> keySelector = orderByQUery[0] switch
            {
                "fname" => user => user.FirstName,
                "lname" => user => user.LastName,
                "email" => user => user.Email,
                "name" => user => user.FirstName + " " + user.LastName,
                "created" => user => user.CreatedDate,
                _ => user => user.Id
            };

            if(normalizedConditiom.Contains("_desc"))
                source = source.OrderByDescending(keySelector);
            else
                source = source.OrderBy(keySelector);
            
            return source;
        }
    }
}
