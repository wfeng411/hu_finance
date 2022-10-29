using AutoMapper;
using hu_app.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace hu_app.Pages
{
    public class HuPageModel : PageModel
    {
        protected readonly IMediator _mediator;
        protected readonly IMapper _mapper;

        public HuPageModel(HuServicesBag servicesBag)
        {
            _mediator = servicesBag.Mediator;
            _mapper = servicesBag.Mapper;
        }

        public UserPreference GetUserPreference()
        {
            var preference = HttpContext.Session.GetString("preference");
            if (preference == null)
            {
                return new UserPreference();
            }
            return JsonConvert.DeserializeObject<UserPreference>(preference);
        }

        public void SetUserPreference(UserPreference preference)
        {
            HttpContext.Session.SetString("preference", JsonConvert.SerializeObject(preference ?? new UserPreference()));
        }

        public async Task<T> GetData<T>(IRequest<HuResponse> request) where T : class
        {
            var response = await _mediator.Send(request);
            if (response.Ok)
            {
                return response.Data as T;
            }
            return null;
        }

        public async Task<HuResponse> ProcessData(IRequest<HuResponse> request)
        {
            return await _mediator.Send(request);
        }
    }
}
