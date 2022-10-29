using hu_app.Shared;
using MediatR;

namespace hu_app.Components
{
    public interface IHuRequest : IRequest<HuResponse> { }
}
