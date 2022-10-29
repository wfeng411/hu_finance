using hu_app.Models;
using hu_app.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace hu_app.Components
{
    public class HuRequestHandler<TRequest, TEntity> : HuRequestHandler<TRequest>
        where TRequest : IHuRequest
        where TEntity : BaseLookup
    {
        private readonly HuRepository<TEntity> _repo;

        public HuRequestHandler(HuRepository<TEntity> repo)
        {
            _repo = repo;
        }

        public override async Task Load(TRequest request)
        {
            Data = await _repo.GetQueryable()
                .OrderBy(x => x.Order).ThenBy(x => x.Name)
                .ToListAsync();
        }
    }
}
