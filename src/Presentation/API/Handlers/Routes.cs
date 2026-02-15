using Application.Abstractions.Handlers;
using Application.Abstractions.Services.Externals;
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
using Application.CQRS.Commands.Currencies.Tags.Add;
using Application.CQRS.Commands.Currencies.Tags.Delete;
using Application.CQRS.Commands.Currencies.Tags.Update;
using Application.CQRS.Commands.Currencies.Update;
using Application.CQRS.Commands.Currencies.UpdateValue;
using Application.CQRS.Commands.UpdateProfile;
using Application.CQRS.Commands.UserAssetHistories.SaveUserAssetHistory;
using Application.CQRS.Commands.UserCurrencyFollows.Add;
using Application.CQRS.Commands.UserCurrencyFollows.AddRange;
using Application.CQRS.Commands.UserCurrencyFollows.ChangeStatus;
using Application.CQRS.Commands.UserCurrencyFollows.Delete;
using Application.CQRS.Commands.Users.Login;
using Application.CQRS.Commands.Users.Register;
using Application.CQRS.Commons.Services;
using Application.CQRS.Queries.Assets.GetForUpdate;
using Application.CQRS.Queries.Assets.GetUserAssetHistory;
using Application.CQRS.Queries.Assets.GetUserAssetInfo;
using Application.CQRS.Queries.Assets.GetUsersAssets;
using Application.CQRS.Queries.Assets.UserAssetItems;
using Application.CQRS.Queries.Assets.UserAssetsForOperationQuery;
using Application.CQRS.Queries.Assets.UserSummary;
using Application.CQRS.Queries.Categories.Info;
using Application.CQRS.Queries.Categories.List;
using Application.CQRS.Queries.Currencies.BroadcastInfo;
using Application.CQRS.Queries.Currencies.Calculator;
using Application.CQRS.Queries.Currencies.EUList;
using Application.CQRS.Queries.Currencies.Info;
using Application.CQRS.Queries.Currencies.List;
using Application.CQRS.Queries.Currencies.Tags.Tool;
using Application.CQRS.Queries.Currencies.WithHistoryInfo;
using Application.CQRS.Queries.PriceInfo;
using Application.CQRS.Queries.Tools;
using Application.CQRS.Queries.Tools.GetCurrencyToolList;
using Application.CQRS.Queries.Tools.ToolWithoutFavs;
using Application.CQRS.Queries.UserAssetHistories.ItemList;
using Application.CQRS.Queries.UserAssetHistories.List;
using Application.CQRS.Queries.UserCurrencyFollows.Info;
using Application.CQRS.Queries.UserCurrencyFollows.List;
using Application.CQRS.Queries.UserCurrencyFollows.UserCurrencyFavList;
using Application.Models.Constants.Roles;
using Application.Models.Constants.Settings;
using Application.Models.DTOs.AIs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace API.Handlers
{
    public static class Routes
    {
        public static void RouteMap(this IEndpointRouteBuilder app)
        {
            var api = app.MapGroup("api");

            var auth = api.MapGroup("auth");

            auth.MapPost("login",
                async ([FromServices] IAuthHandler handler, [FromBody] LoginCommand command, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.Login(command, dispatcher, cancellationToken))
                .WithName("Login")
                .WithTags("Auth")
                .RequireRateLimiting(SettingConstant.AnonymousRateLimiting);

            auth.MapPost("register",
                async ([FromServices] IAuthHandler handler, [FromBody] RegisterCommand command, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.Register(command, dispatcher, cancellationToken))
                .WithName("Register")
                .WithTags("Auth")
                .RequireRateLimiting(SettingConstant.AnonymousRateLimiting);

            var users = api.MapGroup("users");

            users.MapGet("profile",
                async ([FromServices] IUserHandler handler, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.GetProfile(dispatcher, cancellationToken))
                .WithName("Profile")
                .WithTags("Users")
                .RequireAuthorization()
                .RequireRateLimiting(SettingConstant.RichRateLimiting);

            users.MapPut("update-profile",
                async ([FromServices] IUserHandler handler, [FromBody] UpdateProfileCommand command, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.UpdateProfile(command, dispatcher, cancellationToken))
                .WithName("Update Profile")
                .WithTags("Users")
                .RequireAuthorization()
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            var asset = api.MapGroup("assets");

            asset.MapPost("",
                async ([FromServices] IAssetHandler handler, [FromBody] AddAssetCommand command, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.Add(command, dispatcher, cancellationToken))
                .WithName("Add Asset")
                .WithTags("Assets")
                .RequireAuthorization()
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            asset.MapPut("{assetId}",
                async ([FromServices] IAssetHandler handler, [FromRoute(Name = "assetId")] int assetId, [FromBody] UpdateAssetCommand command, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    command.AssetId = assetId;
                    return await handler.Update(command, dispatcher, cancellationToken);
                })
                .WithName("Update Asset")
                .WithTags("Assets")
                .RequireAuthorization()
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            asset.MapDelete("{assetId}",
                async ([FromServices] IAssetHandler handler, [FromRoute(Name = "assetId")] int assetId, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    return await handler.Delete(new DeleteAssetCommand(assetId), dispatcher, cancellationToken);
                })
                .WithName("Delete Asset")
                .WithTags("Assets")
                .RequireAuthorization()
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            asset.MapGet("",
                async ([FromServices] IAssetHandler handler, [AsParameters] UserAssetItemsQuery query, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.UserAssetItems(query, dispatcher, cancellationToken))
                .WithName("User's Assets")
                .WithTags("Assets")
                .RequireAuthorization()
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            asset.MapGet("summary",
                async ([FromServices] IAssetHandler handler, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.UserSummary(new UserSummaryAssetQuery(), dispatcher, cancellationToken))
                .WithName("User's Asset Summary")
                .WithTags("Assets")
                .RequireAuthorization()
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            asset.MapGet("user-asset-history",
                async ([FromServices] IAssetHandler handler, [AsParameters] GetUserAssetHistoryQuery query, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.GetUsersAssetHistory(query, dispatcher, cancellationToken))
                .WithName("User's Asset History")
                .WithTags("Assets")
                .RequireAuthorization()
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            asset.MapGet("user-asset-group",
                async ([FromServices] IAssetHandler handler, [AsParameters] GetUsersAssetWithGroupQuery query, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.GetUserAssetWithGroup(query, dispatcher, cancellationToken))
                .WithName("User's Asset List With Group")
                .WithTags("Assets")
                .RequireAuthorization()
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            asset.MapGet("for-update/{assetId}",
                async ([FromServices] IAssetHandler handler, [FromRoute(Name = "assetId")] int assetId, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    GetForUpdateAssetQuery query = new GetForUpdateAssetQuery(assetId);
                    return await handler.GetForUpdate(query, dispatcher, cancellationToken);
                })
                .WithName("For Update Asset")
                .WithTags("Assets")
                .RequireAuthorization()
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            asset.MapGet("users-asset-info/{assetId}",
                async ([FromServices] IAssetHandler handler, [FromRoute(Name = "assetId")] int assetId, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    GetUsersAssetInfoQuery query = new GetUsersAssetInfoQuery(assetId);
                    return await handler.GetUsersAssetInfo(query, dispatcher, cancellationToken);
                })
                .WithName("User's Asset Info")
                .WithTags("Assets")
                .RequireAuthorization()
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            asset.MapGet("users-assets-for-operation",
                async ([FromServices] IAssetHandler handler, [AsParameters] UserAssetsForOperationQuery query, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.UserAssetsForOperation(query, dispatcher, cancellationToken))
                .WithName("User's Assets For Operation")
                .WithTags("Assets")
                .RequireAuthorization()
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            var currency = api.MapGroup("currencies");

            currency.MapGet("calculator",
                async ([FromServices] ICurrencyHandler handler, [AsParameters] CalculatorQuery query, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.Calculator(query, dispatcher, cancellationToken))
                .WithName("Calculator")
                .WithTags("Currencies")
                .RequireRateLimiting(SettingConstant.AnonymousRateLimiting);

            currency.MapGet("",
                async ([FromServices] ICurrencyHandler handler, [AsParameters] EUCurrencyListQuery query, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                    await handler.EUList(query, dispatcher, cancellationToken))
                    .WithName("EU Currency List")
                    .WithTags("Currencies")
                    .RequireRateLimiting(SettingConstant.AnonymousRateLimiting);

            currency.MapGet("{currencyId}",
                async ([FromServices] ICurrencyHandler handler, [FromRoute(Name = "currencyId")] int currencyId, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    return await handler.EUInfo(new EUCurrencyInfoQuery(currencyId), dispatcher, cancellationToken);
                })
                .WithName("End-User Currency info")
                .WithTags("Currencies")
                .RequireRateLimiting(SettingConstant.AnonymousRateLimiting);

            currency.MapGet("history-info/{currencyId}",
                async ([FromServices] ICurrencyHandler handler, [FromRoute(Name = "currencyId")] int currencyId, [AsParameters] CurrencyWithHistoryInfoQuery query, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    query.CurrencyId = currencyId;
                    return await handler.HistoryInfo(query, dispatcher, cancellationToken);
                })
                .WithName("Currency History Info")
                .WithTags("Currencies")
                .RequireAuthorization()
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            var currencyPanel = currency.MapGroup("panel");

            currencyPanel.MapGet("",
                async ([FromServices] ICurrencyHandler handler, [AsParameters] CurrencyListQuery query, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.List(query, dispatcher, cancellationToken))
                .WithName("Currency List")
                .WithTags("Currencies")
                .RequireAuthorization(AppRoles.Admin)
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            currencyPanel.MapPost("",
                async ([FromServices] ICurrencyHandler handler, [FromBody] AddCurrencyCommand command, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.Add(command, dispatcher, cancellationToken))
                .WithName("Add Currency")
                .WithTags("Currencies")
                .RequireAuthorization(AppRoles.Admin)
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            currencyPanel.MapPatch("status/{currencyId}",
                async ([FromServices] ICurrencyHandler handler, [FromRoute(Name = "currencyId")] int currencyId, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    return await handler.ChangeStatus(new ChangeCurrencyStatusCommand(currencyId), dispatcher, cancellationToken);
                })
                .WithName("Change Currency Status")
                .WithTags("Currencies")
                .RequireAuthorization(AppRoles.Admin)
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            currencyPanel.MapPut("{currencyId}",
                async ([FromServices] ICurrencyHandler handler, [FromRoute(Name = "currencyId")] int currencyId, [FromBody] UpdateCurrencyCommand command, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    command.CurrencyId = currencyId;
                    return await handler.Update(command, dispatcher, cancellationToken);
                })
                .WithName("Update Currency")
                .WithTags("Currencies")
                .RequireAuthorization(AppRoles.Admin)
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            currencyPanel.MapDelete("{currencyId}",
                async ([FromServices] ICurrencyHandler handler, [FromRoute(Name = "currencyId")] int currencyId, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    return await handler.Delete(new DeleteCurrencyCommand(currencyId), dispatcher, cancellationToken);
                })
                .WithName("Delete Currency")
                .WithTags("Currencies")
                .RequireAuthorization(AppRoles.Admin)
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            currencyPanel.MapGet("{currencyId}",
                async ([FromServices] ICurrencyHandler handler, [FromRoute(Name = "currencyId")] int currencyId, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    return await handler.Info(new CurrencyInfoQuery(currencyId), dispatcher, cancellationToken);
                })
                .WithName("Currency info")
                .WithTags("Currencies")
                .RequireAuthorization(AppRoles.Admin)
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            currencyPanel.MapGet("price-info/{currencyId}",
                async ([FromServices] ICurrencyHandler handler, [FromRoute(Name = "currencyId")] int currencyId, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    return await handler.PriceInfo(new CurrencyPriceInfoQuery(currencyId), dispatcher, cancellationToken);
                })
                .WithName("Currency Price Info")
                .WithTags("Currencies")
                .RequireAuthorization(AppRoles.Admin)
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            currencyPanel.MapPatch("change-price/{currencyId}",
                async ([FromServices] ICurrencyHandler handler, [FromRoute(Name = "currencyId")] int currencyId, [FromBody] UpdateCurrencyValueCommand command, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    command.CurrencyId = currencyId;
                    return await handler.UpdateValue(command, dispatcher, cancellationToken);
                })
                .WithName("Change Currency Value")
                .WithTags("Currencies")
                .RequireAuthorization(AppRoles.Admin)
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            var currencyTagPanel = currency.MapGroup("tags/panel");

            currencyTagPanel.MapPost("",
                async ([FromServices] ICurrencyHandler handler, [FromBody] AddCurrencyTagCommand command, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.AddTag(command, dispatcher, cancellationToken))
                .WithName("Add Currency Tag")
                .WithTags("Currency Tags")
                .RequireAuthorization(AppRoles.Admin)
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            currencyTagPanel.MapGet("{currencyTagId}",
                async ([FromServices] ICurrencyHandler handler, [FromRoute(Name = "currencyTagId")] int currencyTagId, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.TagInfo(new(currencyTagId), dispatcher, cancellationToken))
                .WithName("Currency Tag Info")
                .WithTags("Currency Tags")
                .RequireAuthorization()
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            currencyTagPanel.MapPut("{currencyTagId}",
                async ([FromServices] ICurrencyHandler handler, [FromRoute(Name = "currencyTagId")] int currencyTagId, [FromBody] UpdateCurrencyTagCommand command, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    command.CurrencyTagId = currencyTagId;
                    return await handler.UpdateTag(command, dispatcher, cancellationToken);
                })
                .WithName("Update Currency Tag")
                .WithTags("Currency Tags")
                .RequireAuthorization(AppRoles.Admin)
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            currencyTagPanel.MapDelete("{currencyTagId}",
                async ([FromServices] ICurrencyHandler handler, [FromRoute(Name = "currencyTagId")] int currencyTagId, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    return await handler.DeleteTag(new DeleteCurrencyTagCommand(currencyTagId), dispatcher, cancellationToken);
                })
                .WithName("Delete Currency Tag")
                .WithTags("Currency Tags")
                .RequireAuthorization(AppRoles.Admin)
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            var category = api.MapGroup("categories");

            var categoryPanel = category.MapGroup("panel");

            categoryPanel.MapPost("",
                async ([FromServices] ICategoryHandler handler, [FromBody] AddCategoryCommand command, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.AddAsync(command, dispatcher, cancellationToken))
                .WithName("Add Category")
                .WithTags("Categories")
                .RequireAuthorization(AppRoles.Admin)
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            categoryPanel.MapPut("{categoryId}",
                async ([FromServices] ICategoryHandler handler, [FromRoute(Name = "categoryId")] int categoryId, [FromBody] UpdateCategoryCommand command, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    command.CategoryId = categoryId;
                    return await handler.UpdateAsync(command, dispatcher, cancellationToken);
                })
                .WithName("Update Category")
                .WithTags("Categories")
                .RequireAuthorization(AppRoles.Admin)
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            categoryPanel.MapPatch("status/{categoryId}",
                async ([FromServices] ICategoryHandler handler, [FromRoute(Name = "categoryId")] int categoryId, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    return await handler.ChangeStatusAsync(new ChangeCategoryStatusCommand(categoryId), dispatcher, cancellationToken);
                })
                .WithName("Change Category Status")
                .WithTags("Categories")
                .RequireAuthorization(AppRoles.Admin)
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            categoryPanel.MapDelete("{categoryId}",
                async ([FromServices] ICategoryHandler handler, [FromRoute(Name = "categoryId")] int categoryId, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    return await handler.DeleteAsync(new DeleteCategoryCommand(categoryId), dispatcher, cancellationToken);
                })
                .WithName("Delete Category")
                .WithTags("Categories")
                .RequireAuthorization(AppRoles.Admin)
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            categoryPanel.MapGet("",
                async ([FromServices] ICategoryHandler handler, [AsParameters] CategoryListQuery query, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.ListAsync(query, dispatcher, cancellationToken))
                .WithName("List Category")
                .WithTags("Categories")
                .RequireAuthorization(AppRoles.Admin)
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            categoryPanel.MapGet("{categoryId}",
                async ([FromServices] ICategoryHandler handler, [FromRoute(Name = "categoryId")] int categoryId, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    return await handler.Get(new CategoryInfoQuery(categoryId), dispatcher, cancellationToken);
                })
                .WithName("Get Category")
                .WithTags("Categories")
                .RequireAuthorization(AppRoles.Admin)
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            var tool = api.MapGroup("tools");

            tool.MapGet("category-list",
                    async ([FromServices] IToolHandler handler, [AsParameters] GetCategoryToolListQuery query, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.CategoryToolList(query, dispatcher, cancellationToken))
                    .WithName("Category Tool List")
                    .WithTags("Tools")
                    .RequireRateLimiting(SettingConstant.RichRateLimiting);
            // .CacheOutput(SettingConstant.Tool1mOutputCache);

            tool.MapGet("currency-list",
                    async ([FromServices] IToolHandler handler, [AsParameters] GetCurrencyToolListQuery query, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.CurrencyToolList(query, dispatcher, cancellationToken))
                    .WithName("Currency Tool List")
                    .WithTags("Tools")
                    .RequireRateLimiting(SettingConstant.RichRateLimiting);
            // .CacheOutput(SettingConstant.Tool1mOutputCache);

            tool.MapGet("currency-list-without-favs",
                async ([FromServices] IToolHandler handler, [AsParameters] CurrencyToolWithoutFavsQuery query, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                    await handler.CurrencyToolWithoutFavs(query, dispatcher, cancellationToken))
                    .WithName("Currency Tool Without Favs")
                    .WithTags("Tools")
                    .RequireAuthorization()
                    .RequireRateLimiting(SettingConstant.RichRateLimiting);

            tool.MapGet("currency-tag-tool",
                async ([FromServices] IToolHandler handler, [AsParameters] CurrencyTagToolQuery query, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                    await handler.CurrencyTagToolList(query, dispatcher, cancellationToken))
                    .WithName("Currency Tag Tool")
                    .WithTags("Tools")
                    .RequireRateLimiting(SettingConstant.RichRateLimiting);

            var userAssetHistory = api.MapGroup("user-asset-histories");

            userAssetHistory.MapGet("",
                async ([FromServices] IUserAssetHistoryHandler handler, [AsParameters] UserAssetHistoryListQuery query, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.List(query, dispatcher, cancellationToken))
                .WithName("User Asset History List")
                .WithTags("User Asset Histories")
                .RequireAuthorization()
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            userAssetHistory.MapGet("item/{userAssetHistoryId}",
                async ([FromServices] IUserAssetHistoryHandler handler, [FromRoute(Name = "userAssetHistoryId")] int userAssetHistoryId, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.ItemList(new UserAssetItemHistoryListQuery(userAssetHistoryId), dispatcher, cancellationToken))
                .WithName("User Asset Item History List")
                .WithTags("User Asset Histories")
                .RequireAuthorization()
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            userAssetHistory.MapPost("save",
                async ([FromServices] IUserAssetHistoryHandler handler, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.Save(new SaveUserAssetHistoryCommand(), dispatcher, cancellationToken))
                .WithName("Save User Asset History")
                .WithTags("User Asset Histories")
                .RequireAuthorization()
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            var userCurrencyFollow = api.MapGroup("user-currency-follows");

            userCurrencyFollow.MapPost("",
                async ([FromServices] IUserCurrencyFollowHandler handler, [FromBody] AddUserCurrencyFollowCommand command, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.AddAsync(command, dispatcher, cancellationToken))
                .WithName("Add User Currency Follow")
                .WithTags("User Currency Follows")
                .RequireAuthorization()
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            userCurrencyFollow.MapPost("add-multiple",
                async ([FromServices] IUserCurrencyFollowHandler handler, [FromBody] AddRangeUserCurrencyFollowCommand command, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.AddRangeAsync(command, dispatcher, cancellationToken))
                .WithName("Add Multiple User Currency Follow")
                .WithTags("User Currency Follows")
                .RequireAuthorization()
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            userCurrencyFollow.MapPut("{userCurrencyFollowId}",
                async ([FromServices] IUserCurrencyFollowHandler handler, [FromRoute(Name = "userCurrencyFollowId")] int userCurrencyFollowId, [FromBody] ChangeUserCurrencyFollowStatusCommand command, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    command.UserCurrencyFollowId = userCurrencyFollowId;
                    return await handler.ChangeStatusAsync(command, dispatcher, cancellationToken);
                })
                .WithName("Change User Currency Follow")
                .WithTags("User Currency Follows")
                .RequireAuthorization()
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            userCurrencyFollow.MapDelete("{currencyId}",
                async ([FromServices] IUserCurrencyFollowHandler handler, [FromRoute(Name = "currencyId")] int currencyId, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.DeleteAsync(new DeleteUserCurrencyFollowCommand(currencyId), dispatcher, cancellationToken))
                .WithName("Delete User Currency Follow")
                .WithTags("User Currency Follows")
                .RequireAuthorization()
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            userCurrencyFollow.MapGet("fav-list",
                async ([FromServices] IUserCurrencyFollowHandler handler, [AsParameters] UserCurrencyFavListQuery query, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.FavListAsync(query, dispatcher, cancellationToken))
                .WithName("Broadcast List User Currency Follows")
                .WithTags("User Currency Follows")
                .RequireAuthorization()
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            userCurrencyFollow.MapGet("",
                async ([FromServices] IUserCurrencyFollowHandler handler, [AsParameters] UserCurrencyListQuery query, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.ListAsync(query, dispatcher, cancellationToken))
                .WithName("User Currency Follow List")
                .WithTags("User Currency Follows")
                .RequireAuthorization()
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            userCurrencyFollow.MapGet("{userCurrencyFollowId}",
                async ([FromServices] IUserCurrencyFollowHandler handler, [FromRoute(Name = "userCurrencyFollowId")] int userCurrencyFollowId, [FromServices] Dispatcher dispatcher, CancellationToken cancellationToken) => await handler.InfoAsync(new UserCurrencyFollowInfoQuery(userCurrencyFollowId), dispatcher, cancellationToken))
                .WithName("User Currency Follow Info")
                .WithTags("User Currency Follows")
                .RequireAuthorization()
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);

            var aiHandlers = api.MapGroup("ai");

            aiHandlers.MapPost("parse",
                async ([FromServices] IAIHandler handler,[FromBody] AIRequest message, [FromServices] IAIOrchestrator orchestrator, CancellationToken cancellationToken) => await handler.Parse(message.Message, message.UserId, orchestrator, cancellationToken))
                .WithName("AI Parser")
                .WithTags("AIs")
                .RequireRateLimiting(SettingConstant.PerUserRateLimiting);
        }
    }
}