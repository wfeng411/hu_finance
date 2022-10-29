using hu_app.Models;
using hu_app.Shared;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace hu_app.Components.User
{
    public class GetUsersRequest : IHuRequest { }

    public class GetUsersHandler : HuRequestHandler<GetUsersRequest>
    {
        private readonly HuRepository<HuAppUser> _repo;

        public GetUsersHandler(HuRepository<HuAppUser> repo)
        {
            _repo = repo;
        }

        public override async Task Load(GetUsersRequest request)
        {
            Data = await _repo.GetQueryable().ToListAsync();
        }
    }
}
