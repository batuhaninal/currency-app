using Application.Abstractions.Commons.Results;
using Application.Abstractions.Handlers;
using Application.CQRS.Commons.Services;
using Application.CQRS.Queries.Users.GetProfile;

namespace API.Handlers.v1
{
    public class UserHandler : IUserHandler
    {
        public async Task<IResult> GetProfile(Dispatcher dispatcher, CancellationToken cancellationToken)
        {
            IBaseResult result = await dispatcher.SendQueryAsync<GetProfileQuery, IBaseResult>(new GetProfileQuery(), cancellationToken);
            return Results.Json<IBaseResult>(result, statusCode: result.StatusCode);
        }
    }
}