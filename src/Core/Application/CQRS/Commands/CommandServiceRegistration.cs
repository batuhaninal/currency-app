using Application.Abstractions.Commons.Results;
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
using Application.CQRS.Commands.UpdateProfile;
using Application.CQRS.Commands.Users.Login;
using Application.CQRS.Commands.Users.Register;
using Application.CQRS.Commons.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.CQRS.Commands
{
    public static class CommandServiceRegistration
    {
        public static void BindCommands(this IServiceCollection services)
        {
            #region User
            services.AddScoped<RegisterCommandHandler>();
            services.AddScoped<ICommandHandler<RegisterCommand, IBaseResult>, RegisterCommandHandler>();

            services.AddScoped<LoginCommandHandler>();
            services.AddScoped<ICommandHandler<LoginCommand, IBaseResult>, LoginCommandHandler>();

            services.AddScoped<UpdateProfileCommand>();
            services.AddScoped<ICommandHandler<UpdateProfileCommand, IBaseResult>, UpdateProfileCommandHandler>();
            #endregion

            #region Asset
            services.AddScoped<AddAssetCommandHandler>();
            services.AddScoped<ICommandHandler<AddAssetCommand, IBaseResult>, AddAssetCommandHandler>();

            services.AddScoped<UpdateAssetCommand>();
            services.AddScoped<ICommandHandler<UpdateAssetCommand, IBaseResult>, UpdateAssetCommandHandler>();

            services.AddScoped<DeleteAssetCommand>();
            services.AddScoped<ICommandHandler<DeleteAssetCommand, IBaseResult>, DeleteAssetCommandHandler>();
            #endregion

            #region Currency
            services.AddScoped<AddCurrencyCommand>();
            services.AddScoped<ICommandHandler<AddCurrencyCommand, IBaseResult>, AddCurrencyCommandHandler>();

            services.AddScoped<UpdateCurrencyCommand>();
            services.AddScoped<ICommandHandler<UpdateCurrencyCommand, IBaseResult>, UpdateCurrencyCommandHandler>();

            services.AddScoped<DeleteCurrencyCommand>();
            services.AddScoped<ICommandHandler<DeleteCurrencyCommand, IBaseResult>, DeleteCurrencyCommandHandler>();

            services.AddScoped<ChangeCurrencyStatusCommand>();
            services.AddScoped<ICommandHandler<ChangeCurrencyStatusCommand, IBaseResult>, ChangeCurrencyStatusCommandHandler>();

            services.AddScoped<UpdateCurrencyValueCommand>();
            services.AddScoped<ICommandHandler<UpdateCurrencyValueCommand, IBaseResult>, UpdateCurrencyValueCommandHandler>();
            #endregion

            #region Category
            services.AddScoped<AddCategoryCommand>();
            services.AddScoped<ICommandHandler<AddCategoryCommand, IBaseResult>, AddCategoryCommandHandler>();

            services.AddScoped<UpdateCategoryCommand>();
            services.AddScoped<ICommandHandler<UpdateCategoryCommand, IBaseResult>, UpdateCategoryCommandHandler>();

            services.AddScoped<ChangeCategoryStatusCommand>();
            services.AddScoped<ICommandHandler<ChangeCategoryStatusCommand, IBaseResult>, ChangeCategoryStatusCommandHandler>();

            services.AddScoped<DeleteCategoryCommand>();
            services.AddScoped<ICommandHandler<DeleteCategoryCommand, IBaseResult>, DeleteCategoryCommandHandler>();
            #endregion
        }
    }
}