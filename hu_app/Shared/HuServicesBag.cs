using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace hu_app.Shared
{
    public class HuServicesBag
    {
        public IMediator Mediator;
        public IMapper Mapper;
        public IHttpContextAccessor HttpContextAccessor;

        public HuServicesBag(
            IMediator mediator
            , IMapper mapper
            , IHttpContextAccessor httpContextAccessor)
        {
            Mediator = mediator;
            Mapper = mapper;
            HttpContextAccessor = httpContextAccessor;
        }
    }
}
