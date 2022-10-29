using hu_app.Shared;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace hu_app.Components
{
    public class HuRequestHandler<TRequest> : IRequestHandler<TRequest, HuResponse> where TRequest : IHuRequest
    {
        protected object Data;

        public async Task<HuResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            if (MethodIsOverridden(nameof(Load)))
            {
                await Load(request);
            }
            if (MethodIsOverridden(nameof(Process)))
            {
                await Process(request);
            }
            return HuResult.Ok(Data);
        }

        public virtual async Task Load(TRequest request) { }

        public virtual async Task Process(TRequest request) { }

        public bool MethodIsOverridden(string methodName)
        {
            var t = this.GetType();
            var methodInfo = t.GetMethod(methodName);

            var methodInType = methodInfo.DeclaringType.FullName;
            if (methodInType == t.FullName)
            {
                return true;
            }

            var lookupHandlerType = typeof(HuRequestHandler<,>).FullName;
            methodInType = methodInfo.DeclaringType.GetGenericTypeDefinition().FullName;
            if (methodInType == lookupHandlerType)
            {
                return true;
            }

            return false;
        }
    }
}
