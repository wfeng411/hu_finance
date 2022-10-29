using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hu_app.Shared
{
    public class HuMediatorValidation<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public HuMediatorValidation(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext<TRequest>(request);



            var failures = new List<ValidationFailure>();
            foreach (var validator in _validators)
            {
                var validationResult = await validator.ValidateAsync(context);
                failures.AddRange(validationResult.Errors.Where(x => x != null));
            }

            if (failures.Count != 0)
            {
                var errors = failures.Select(x => x.ErrorMessage).ToList();
                HuLogger.LogMediatorValidationError(typeof(TRequest).Name, errors);
                throw new HuMediatorException(errors);
            }

            return await next();
        }
    }
}
