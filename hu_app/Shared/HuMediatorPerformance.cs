using MediatR;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace hu_app.Shared
{
    public class HuMediatorPerformance<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var sw = new Stopwatch();

            sw.Start();

            var response = await next();

            sw.Stop();

            long? dataSize = null;
            if (response is HuResponse huResponse)
            {
                var dataString = JsonConvert.SerializeObject(huResponse);
                dataSize = dataString.Length;
            }

            HuLogger.LogMediatorPerformance(typeof(TRequest).Name, (int)sw.ElapsedMilliseconds, dataSize);

            return response;
        }
    }
}
