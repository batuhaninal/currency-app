using Application.Abstractions.Repositories.Assets;
using Domain;
using Persistence.Contexts;
using Persistence.Repositories.Commons;

namespace Persistence
{
    public class AssetWriteRepository : WriteRepository<Asset>, IAssetWriteRepository
    {
        public AssetWriteRepository(CurrencyContext context) : base(context)
        {
        }
    }
}