using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDPeople.Filters.ResourceFilter
{
    public class FeatureDisabledResourceFilter : IAsyncResourceFilter
    {
        private readonly ILogger<FeatureDisabledResourceFilter> _logger;
        private bool _isDisabled;
        public FeatureDisabledResourceFilter(ILogger<FeatureDisabledResourceFilter> logger, bool isDisabled=true)
        {
            _logger = logger;
            _isDisabled = isDisabled;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            //TO Do: before logic
            _logger.LogInformation("{FilterName}.{MethodName}-before", nameof(FeatureDisabledResourceFilter), nameof(OnResourceExecutionAsync));
            if (_isDisabled)
            {
                 context.Result = new NotFoundResult(); //404-NotFiniteNumberException Found

            }
            else
            {
                await next();
            }

            //To Do: after logic
            _logger.LogInformation("{FilterName}.{MethodName}-after", nameof(FeatureDisabledResourceFilter), nameof(OnResourceExecutionAsync));



        }
    }
}
