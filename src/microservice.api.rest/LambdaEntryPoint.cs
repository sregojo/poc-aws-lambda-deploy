using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System.Diagnostics;

namespace microservice.api.rest
{
    /// <summary>
    /// This class extends from APIGatewayProxyFunction which contains the method FunctionHandlerAsync which is the 
    /// actual Lambda function entry point. The Lambda handler field should be set to
    /// 
    /// microservice.api.rest::microservice.api.rest.LambdaEntryPoint::FunctionHandlerAsync
    /// </summary>
    public class LambdaEntryPoint : Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
    {

        static long callcount = 0;
        static long initcount = 0;

        /// <summary>
        /// The builder has configuration, logging and Amazon API Gateway already configured. The startup class
        /// needs to be configured in this method using the UseStartup<>() method.
        /// </summary>
        /// <param name="builder"></param>
        protected override void Init(IWebHostBuilder builder)
        {
            builder.UseStartup<Startup>();
            initcount++;
        }

        public async override Task<APIGatewayProxyResponse> FunctionHandlerAsync(APIGatewayProxyRequest request, ILambdaContext lambdaContext)
        {

            Stopwatch timeCall = Stopwatch.StartNew();

            var result =  await base.FunctionHandlerAsync(request, lambdaContext);
            //var result = new APIGatewayProxyResponse()
            //{
            //    Headers = new Dictionary<string,string>(),
            //    Body = "{test}",
            //    IsBase64Encoded = false,
            //    StatusCode = 200
            //};

            timeCall.Stop();

            result.Headers.Add("FunctionHandlerAsyncTIME", timeCall.ElapsedMilliseconds.ToString());
            result.Headers.Add("FunctionHandlerAsyncCALLCOUNT", (++callcount).ToString());
            result.Headers.Add("InitCALLCOUNT", (initcount).ToString());

            return result;
        }
    }
}
