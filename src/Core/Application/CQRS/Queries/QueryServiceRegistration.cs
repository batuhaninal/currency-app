using Application.Abstractions.Commons.Results;
using Application.CQRS.Commons.Interfaces;
using Application.CQRS.Queries.Assets.GetForUpdate;
using Application.CQRS.Queries.Assets.GetUserAssetHistory;
using Application.CQRS.Queries.Assets.GetUserAssetInfo;
using Application.CQRS.Queries.Assets.GetUsersAssets;
using Application.CQRS.Queries.Assets.UserAssetItems;
using Application.CQRS.Queries.Assets.UserAssetsForOperationQuery;
using Application.CQRS.Queries.Assets.UserSummary;
using Application.CQRS.Queries.Categories.Info;
using Application.CQRS.Queries.Categories.List;
using Application.CQRS.Queries.Currencies.Calculator;
using Application.CQRS.Queries.Currencies.Info;
using Application.CQRS.Queries.Currencies.List;
using Application.CQRS.Queries.Currencies.WithHistoryInfo;
using Application.CQRS.Queries.PriceInfo;
using Application.CQRS.Queries.Tools;
using Application.CQRS.Queries.Tools.GetCurrencyToolList;
using Application.CQRS.Queries.Users.GetProfile;
using Microsoft.Extensions.DependencyInjection;

namespace Application.CQRS.Queries
{
    public static class QueryServiceRegistration
    {
        public static void BindQueries(this IServiceCollection services)
        {
            services.AddScoped<GetProfileQueryHandler>();
            services.AddScoped<IQueryHandler<GetProfileQuery, IBaseResult>, GetProfileQueryHandler>();

            #region Tool
            services.AddScoped<GetCategoryToolListQuery>();
            services.AddScoped<IQueryHandler<GetCategoryToolListQuery, IBaseResult>, GetCategoryToolListQueryHandler>();

            services.AddScoped<GetCurrencyToolListQuery>();
            services.AddScoped<IQueryHandler<GetCurrencyToolListQuery, IBaseResult>, GetCurrencyToolListQueryHandler>();
            #endregion

            #region Asset
            services.AddScoped<GetUsersAssetWithGroupQuery>();
            services.AddScoped<IQueryHandler<GetUsersAssetWithGroupQuery, IBaseResult>, GetUsersAssetWithGroupQueryHandler>();

            services.AddScoped<GetUsersAssetInfoQuery>();
            services.AddScoped<IQueryHandler<GetUsersAssetInfoQuery, IBaseResult>, GetUsersAssetInfoQueryHandler>();

            services.AddScoped<GetUserAssetHistoryQuery>();
            services.AddScoped<IQueryHandler<GetUserAssetHistoryQuery, IBaseResult>, GetUserAssetHistoryQueryHandler>();

            services.AddScoped<UserAssetItemsQuery>();
            services.AddScoped<IQueryHandler<UserAssetItemsQuery, IBaseResult>, UserAssetItemsQueryHandler>();

            services.AddScoped<GetForUpdateAssetQuery>();
            services.AddScoped<IQueryHandler<GetForUpdateAssetQuery, IBaseResult>, GetForUpdateAssetQueryHandler>();

            services.AddScoped<UserSummaryAssetQuery>();
            services.AddScoped<IQueryHandler<UserSummaryAssetQuery, IBaseResult>, UserSummaryAssetQueryHandler>();

            services.AddScoped<UserAssetsForOperationQuery>();
            services.AddScoped<IQueryHandler<UserAssetsForOperationQuery, IBaseResult>, UserAssetsForOperationQueryHandler>();
            #endregion

            #region Category
            services.AddScoped<CategoryListQuery>();
            services.AddScoped<IQueryHandler<CategoryListQuery, IBaseResult>, CategoryListQueryHandler>();

            services.AddScoped<CategoryInfoQuery>();
            services.AddScoped<IQueryHandler<CategoryInfoQuery, IBaseResult>, CategoryInfoQueryHandler>();
            #endregion

            #region Currency
            services.AddScoped<CurrencyListQuery>();
            services.AddScoped<IQueryHandler<CurrencyListQuery, IBaseResult>, CurrencyListQueryHandler>();

            services.AddScoped<CurrencyInfoQuery>();
            services.AddScoped<IQueryHandler<CurrencyInfoQuery, IBaseResult>, CurrencyInfoQueryHandler>();

            services.AddScoped<CurrencyWithHistoryInfoQuery>();
            services.AddScoped<IQueryHandler<CurrencyWithHistoryInfoQuery, IBaseResult>, CurrencyWithHistoryInfoQueryHandler>();

            services.AddScoped<CurrencyPriceInfoQuery>();
            services.AddScoped<IQueryHandler<CurrencyPriceInfoQuery, IBaseResult>, CurrencyPriceInfoQueryHandler>();

            services.AddScoped<CalculatorQuery>();
            services.AddScoped<IQueryHandler<CalculatorQuery, IBaseResult>, CalculatorQueryHandler>();
            #endregion
        }
    }
}