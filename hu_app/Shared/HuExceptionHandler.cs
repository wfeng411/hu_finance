using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace hu_app.Shared
{
    public static class HuExceptionHandler
    {
        public static void UseHuExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var errors = new List<string>();

                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    Exception exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;

                    if (!(exception is HuMediatorException))
                    {
                        try
                        {
                            HuLogger.LogException(exception);
                        }
                        catch (Exception ex)
                        {
                            errors.AddRange(ex.GetErrors());
                        }
                    }

                    errors.AddRange(exception.GetErrors());

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new HuResponse
                    {
                        Ok = false,
                        Errors = errors,
                        Position = exception.GetPosition()
                    }));
                });
            });
        }
    }
}
