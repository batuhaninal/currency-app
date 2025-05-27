using Application.Abstractions.Repositories.Assets;
using Domain;
using Persistence.Contexts;
using Persistence.Repositories.Commons;

namespace Persistence
{
    public class AssetReadRepository : ReadRepository<Asset>, IAssetReadRepository
    {
        public AssetReadRepository(CurrencyContext context) : base(context)
        {
        }
    }
}