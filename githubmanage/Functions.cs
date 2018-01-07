using System;
using System.Collections.Generic;
using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using githubmanage.helper;
using Newtonsoft.Json.Linq;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace githubmanage
{
  public class Functions
  {

    public APIGatewayProxyResponse CreateApiGatewayProxyResponse(string returnBody)
    {
      LambdaLogger.Log(returnBody);
      var response = new APIGatewayProxyResponse
      {
        StatusCode = (int)HttpStatusCode.OK,
        Body = returnBody,
        Headers = new Dictionary<string, string> { { "Content-Type", "text/json" } }
      };
      return response;
    }

    public APIGatewayProxyResponse ConfigureGithubRepo(APIGatewayProxyRequest request, ILambdaContext context)
    {
      context.Logger.LogLine($"Function started executing at {DateTime.UtcNow}");
      context.Logger.LogLine("Function name: " + context.FunctionName);
      context.Logger.LogLine("RemainingTime: " + context.RemainingTime);

      string githubPat = Environment.GetEnvironmentVariable("githubPat");
      string githubBaseApiUri = "https://api.github.com";

      JObject requestBody = JObject.Parse(request.Body);

      string repoName = (requestBody["repository"]["name"]).Value<string>();
      string orgName = (requestBody["repository"]["owner"]["login"]).Value<string>();

      context.Logger.LogLine($"Executing against repo {repoName} and org {orgName}...");
      context.Logger.LogLine("Configuring Repository...");
      var repoConfigResult = Helper.ConfigureGithubRepo(githubBaseApiUri, orgName, repoName, githubPat, context);
      context.Logger.LogLine(repoConfigResult.ToString());

      var response = CreateApiGatewayProxyResponse($"Repository {repoName} successfully reconfigured...");

      return response;

    }

    public APIGatewayProxyResponse ConfigureGithubBranch(APIGatewayProxyRequest request, ILambdaContext context)
    {
      context.Logger.LogLine($"Function started executing at {DateTime.UtcNow}");
      context.Logger.LogLine("Function name: " + context.FunctionName);
      context.Logger.LogLine("RemainingTime: " + context.RemainingTime);

      string githubPat = Environment.GetEnvironmentVariable("githubPat");
      string githubBaseApiUri = "https://api.github.com";

      JObject requestBody = JObject.Parse(request.Body);

      string repoName = (requestBody["repository"]["name"]).Value<string>();
      string orgName = (requestBody["repository"]["owner"]["login"]).Value<string>();

      context.Logger.LogLine($"Executing against repo {repoName} and org {orgName}...");
      context.Logger.LogLine("Configuring master branch...");
      var branchConfigResult = Helper.ConfigureGithubBranch(githubBaseApiUri, orgName, repoName, githubPat, context);

      context.Logger.LogLine(branchConfigResult.ToString());

      var response = CreateApiGatewayProxyResponse($"Repository {repoName} successfully reconfigured...");

      return response;

    }
  }
}
