using Application.Abstractions.Handlers;
using Application.CQRS.Commands.Assets.AddAsset;
using Application.CQRS.Commands.Assets.Delete;
using Application.CQRS.Commands.Assets.Update;
using Application.CQRS.Commands.Categories.Add;
using Application.CQRS.Commands.Categories.ChangeStatus;
using Application.CQRS.Commands.Categories.Delete;
using Application.CQRS.Commands.Categories.Update;
using Application.CQRS.Commands.Currencies.Add;
using Application.CQRS.Commands.Currencies.ChangeStatus;
using Application.CQRS.Commands.Currencies.Delete;
using Application.CQRS.Commands.Currencies.Update;
using Application.CQRS.Commands.Currencies.UpdateValue;
using Application.CQRS.Commands.Users.Login;
using Application.CQRS.Commands.Users.Register;
using Application.CQRS.Commons.Services;
using Application.CQRS.Queries.Assets.GetForUpdate;
using Application.CQRS.Queries.Assets.GetUserAssetHistory;
using Application.CQRS.Queries.Assets.GetUserAssetInfo;
using Application.CQRS.Queries.Assets.GetUsersAssets;
using Application.CQRS.Queries.Assets.UserAssetItems;
using Application.CQRS.Queries.Assets.UserSummary;
using Application.CQRS.Queries.Categories.Info;
using Application.CQRS.Queries.Categories.List;
using Application.CQRS.Queries.Currencies.Info;
using Application.CQRS.Queries.Currencies.List;
using Application.CQRS.Queries.Currencies.WithHistoryInfo;
using Application.CQRS.Queries.PriceInfo;
using Application.CQRS.Queries.Tools;
using Application.CQRS.Queries.Tools.GetCurrencyToolList;
using Application.Models.Constants.Roles;
using Microsoft.AspNetCore.Mvc;

namespace API.Handlers
{
    public static class Routes
    {
        public static void RouteMap(this IEndpointRouteBuilder app)
        {
            var api = app.MapGroup("api");

            var panel = api.MapGroup("panel");

            var auth = api.MapGroup("auth");

            auth.MapPost("login",
                async ([FromServices] IAuthHandler handler, [FromBody] LoginCommand command, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.Login(command, dispatcher, cancellationToken))
                .WithName("Login")
                .WithTags("Auth");

            auth.MapPost("register",
                async ([FromServices] IAuthHandler handler, [FromBody] RegisterCommand command, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.Register(command, dispatcher, cancellationToken))
                .WithName("Register")
                .WithTags("Auth");

            var users = api.MapGroup("users");

            users.MapGet("profile",
                async ([FromServices] IUserHandler handler, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.GetProfile(dispatcher, cancellationToken))
                .WithName("Profile")
                .WithTags("Users")
                .RequireAuthorization();

            var asset = api.MapGroup("assets");

            asset.MapPost("",
                async ([FromServices] IAssetHandler handler, [FromBody] AddAssetCommand command, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.Add(command, dispatcher, cancellationToken))
                .WithName("Add Asset")
                .WithTags("Assets")
                .RequireAuthorization();

            asset.MapPut("{assetId}",
                async ([FromServices] IAssetHandler handler, [FromRoute(Name = "assetId")] int assetId, [FromBody] UpdateAssetCommand command, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    command.AssetId = assetId;
                    return await handler.Update(command, dispatcher, cancellationToken);
                })
                .WithName("Update Asset")
                .WithTags("Assets")
                .RequireAuthorization();

            asset.MapDelete("{assetId}",
                async ([FromServices] IAssetHandler handler, [FromRoute(Name = "assetId")] int assetId, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    return await handler.Delete(new DeleteAssetCommand(assetId), dispatcher, cancellationToken);
                })
                .WithName("Delete Asset")
                .WithTags("Assets")
                .RequireAuthorization();

            asset.MapGet("",
                async ([FromServices] IAssetHandler handler, [AsParameters] UserAssetItemsQuery query, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.UserAssetItems(query, dispatcher, cancellationToken))
                .WithName("User's Assets")
                .WithTags("Assets")
                .RequireAuthorization();

            asset.MapGet("summary",
                async ([FromServices] IAssetHandler handler, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.UserSummary(new UserSummaryAssetQuery(), dispatcher, cancellationToken))
                .WithName("User's Asset Summary")
                .WithTags("Assets")
                .RequireAuthorization();

            asset.MapGet("user-asset-history",
                async ([FromServices] IAssetHandler handler, [AsParameters] GetUserAssetHistoryQuery query, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.GetUsersAssetHistory(query, dispatcher, cancellationToken))
                .WithName("User's Asset History")
                .WithTags("Assets")
                .RequireAuthorization();

            asset.MapGet("user-asset-group",
                async ([FromServices] IAssetHandler handler, [AsParameters] GetUsersAssetWithGroupQuery query, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.GetUserAssetWithGroup(query, dispatcher, cancellationToken))
                .WithName("User's Asset List With Group")
                .WithTags("Assets")
                .RequireAuthorization();

            asset.MapGet("for-update/{assetId}",
                async ([FromServices] IAssetHandler handler, [FromRoute(Name = "assetId")] int assetId, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    GetForUpdateAssetQuery query = new GetForUpdateAssetQuery(assetId);
                    return await handler.GetForUpdate(query, dispatcher, cancellationToken);
                })
                .WithName("For Update Asset")
                .WithTags("Assets")
                .RequireAuthorization();

            asset.MapGet("users-asset-info/{assetId}",
                async ([FromServices] IAssetHandler handler, [FromRoute(Name = "assetId")] int assetId, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    GetUsersAssetInfoQuery query = new GetUsersAssetInfoQuery(assetId);
                    return await handler.GetUsersAssetInfo(query, dispatcher, cancellationToken);
                })
                .WithName("User's Asset Info")
                .WithTags("Assets")
                .RequireAuthorization();

            var currency = api.MapGroup("currencies");

            var currencyPanel = panel.MapGroup("currencies");

            currencyPanel.MapGet("",
                async ([FromServices] ICurrencyHandler handler, [AsParameters] CurrencyListQuery query, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.List(query, dispatcher, cancellationToken))
                .WithName("Currency List")
                .WithTags("Currencies")
                .RequireAuthorization(AppRoles.Admin);

            currencyPanel.MapPost("",
                async ([FromServices] ICurrencyHandler handler, [FromBody] AddCurrencyCommand command, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.Add(command, dispatcher, cancellationToken))
                .WithName("Add Currency")
                .WithTags("Currencies")
                .RequireAuthorization(AppRoles.Admin);

            currencyPanel.MapPatch("status/{currencyId}",
                async ([FromServices] ICurrencyHandler handler, [FromRoute(Name = "currencyId")] int currencyId, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    return await handler.ChangeStatus(new ChangeCurrencyStatusCommand(currencyId), dispatcher, cancellationToken);
                })
                .WithName("Change Currency Status")
                .WithTags("Currencies")
                .RequireAuthorization(AppRoles.Admin);

            currencyPanel.MapPut("{currencyId}",
                async ([FromServices] ICurrencyHandler handler, [FromRoute(Name = "currencyId")] int currencyId, [FromBody] UpdateCurrencyCommand command, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    command.CurrencyId = currencyId;
                    return await handler.Update(command, dispatcher, cancellationToken);
                })
                .WithName("Update Currency")
                .WithTags("Currencies")
                .RequireAuthorization(AppRoles.Admin);

            currencyPanel.MapDelete("{currencyId}",
                async ([FromServices] ICurrencyHandler handler, [FromRoute(Name = "currencyId")] int currencyId, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    return await handler.Delete(new DeleteCurrencyCommand(currencyId), dispatcher, cancellationToken);
                })
                .WithName("Delete Currency")
                .WithTags("Currencies")
                .RequireAuthorization(AppRoles.Admin);

            currencyPanel.MapGet("{currencyId}",
                async ([FromServices] ICurrencyHandler handler, [FromRoute(Name = "currencyId")] int currencyId, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    return await handler.Info(new CurrencyInfoQuery(currencyId), dispatcher, cancellationToken);
                })
                .WithName("Currency info")
                .WithTags("Currencies")
                .RequireAuthorization(AppRoles.Admin);

            currencyPanel.MapGet("history-info/{currencyId}",
                async ([FromServices] ICurrencyHandler handler, [FromRoute(Name = "currencyId")] int currencyId, [AsParameters] CurrencyWithHistoryInfoQuery query, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    query.CurrencyId = currencyId;
                    return await handler.HistoryInfo(query, dispatcher, cancellationToken);
                })
                .WithName("Currency History Info")
                .WithTags("Currencies")
                .RequireAuthorization(AppRoles.Admin);

            currencyPanel.MapGet("price-info/{currencyId}",
                async ([FromServices] ICurrencyHandler handler, [FromRoute(Name = "currencyId")] int currencyId, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    return await handler.PriceInfo(new CurrencyPriceInfoQuery(currencyId), dispatcher, cancellationToken);
                })
                .WithName("Currency Price Info")
                .WithTags("Currencies")
                .RequireAuthorization(AppRoles.Admin);

            currencyPanel.MapPatch("change-price/{currencyId}",
                async ([FromServices] ICurrencyHandler handler, [FromRoute(Name = "currencyId")] int currencyId, [FromBody] UpdateCurrencyValueCommand command, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    command.CurrencyId = currencyId;
                    return await handler.UpdateValue(command, dispatcher, cancellationToken);
                })
                .WithName("Change Currency Value")
                .WithTags("Currencies")
                .RequireAuthorization(AppRoles.Admin);

            var category = api.MapGroup("categories");

            var categoryPanel = panel.MapGroup("categories");

            categoryPanel.MapPost("",
                async ([FromServices] ICategoryHandler handler, [FromBody] AddCategoryCommand command, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.AddAsync(command, dispatcher, cancellationToken))
                .WithName("Add Category")
                .WithTags("Categories")
                .RequireAuthorization(AppRoles.Admin);

            categoryPanel.MapPut("{categoryId}",
                async ([FromServices] ICategoryHandler handler, [FromRoute(Name = "categoryId")] int categoryId, [FromBody] UpdateCategoryCommand command, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    command.CategoryId = categoryId;
                    return await handler.UpdateAsync(command, dispatcher, cancellationToken);
                }) 
                .WithName("Update Category")
                .WithTags("Categories")
                .RequireAuthorization(AppRoles.Admin);

            categoryPanel.MapPatch("status/{categoryId}",
                async ([FromServices] ICategoryHandler handler, [FromRoute(Name = "categoryId")] int categoryId, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    return await handler.ChangeStatusAsync(new ChangeCategoryStatusCommand(categoryId), dispatcher, cancellationToken);
                }) 
                .WithName("Change Category Status")
                .WithTags("Categories")
                .RequireAuthorization(AppRoles.Admin);

            categoryPanel.MapDelete("{categoryId}",
                async ([FromServices] ICategoryHandler handler, [FromRoute(Name = "categoryId")] int categoryId, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    return await handler.DeleteAsync(new DeleteCategoryCommand(categoryId), dispatcher, cancellationToken);
                }) 
                .WithName("Delete Category")
                .WithTags("Categories")
                .RequireAuthorization(AppRoles.Admin);

            categoryPanel.MapGet("",
                async ([FromServices] ICategoryHandler handler, [AsParameters] CategoryListQuery query, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.ListAsync(query, dispatcher, cancellationToken))
                .WithName("List Category")
                .WithTags("Categories")
                .RequireAuthorization(AppRoles.Admin);

            categoryPanel.MapGet("{categoryId}",
                async ([FromServices] ICategoryHandler handler, [FromRoute(Name = "categoryId")] int categoryId, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    return await handler.Get(new CategoryInfoQuery(categoryId), dispatcher, cancellationToken);
                })
                .WithName("Get Category")
                .WithTags("Categories")
                .RequireAuthorization(AppRoles.Admin);

        var tool = api.MapGroup("tools");

        tool.MapGet("category-list",
                async([FromServices] IToolHandler handler, [AsParameters] GetCategoryToolListQuery query, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.CategoryToolList(query, dispatcher, cancellationToken))
                .WithName("Category Tool List")
                .WithTags("Tools");

        tool.MapGet("currency-list",
                async([FromServices] IToolHandler handler, [AsParameters] GetCurrencyToolListQuery query, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.CurrencyToolList(query, dispatcher, cancellationToken))
                .WithName("Currency Tool List")
                .WithTags("Tools");
    }
}
}