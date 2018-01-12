using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Newtonsoft.Json.Linq;

namespace githubmanage.helper

{
  public class Helper
  {
    public static JObject CreateMasterBranch(string apiUri, string githubOrg, string repoName, string githubPat, ILambdaContext context)
    {
      string githubUri = $"{apiUri}/repos/{githubOrg}/{repoName}/contents/README.md";
      string jsonPayload = "{\"message\":\"Creating README.md\", \"content\":\"SGVsbG8gd29ybGQgOmNhdDo = \", \"branch\":\"master\"}";

      context.Logger.LogLine("Building the httpClient...");
      HttpContent httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

      HttpClient httpClient = new HttpClient();
      httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"token {githubPat}");
      httpClient.DefaultRequestHeaders.Add("User-Agent", "githubmanage-lambda");
      var result = httpClient.PutAsync(githubUri, httpContent);
      Task.WaitAll(result);
      context.Logger.LogLine("Parsing the result");

      var output = result.Result.Content.ReadAsStringAsync();
      Task.WaitAll(output);

      context.Logger.LogLine(output.Result);
      JObject resultJson = JObject.Parse(output.Result);
      return resultJson;
    }
    public static JObject ConfigureGithubBranch(string apiUri, string githubOrg, string repoName, string githubPat, ILambdaContext context)
    {
      string githubUri = $"{apiUri}/repos/{githubOrg}/{repoName}/branches/master/protection";
      string jsonPayload = "{\"required_status_checks\": null,\"enforce_admins\": null,\"required_pull_request_reviews\": {\"dismissal_restrictions\": {\"users\": [], \"teams\": []},\"dismiss_stale_reviews\": true,\"require_code_owner_reviews\": true},\"restrictions\": null}";

      context.Logger.LogLine("Building the httpClient...");
      HttpContent httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

      HttpClient httpClient = new HttpClient();
      httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"token {githubPat}");
      httpClient.DefaultRequestHeaders.Add("User-Agent", "githubmanage-lambda");
      var result = httpClient.PutAsync(githubUri, httpContent);
      Task.WaitAll(result);
      context.Logger.LogLine("Parsing the result");

      var output = result.Result.Content.ReadAsStringAsync();
      Task.WaitAll(output);

      context.Logger.LogLine(output.Result);
      JObject resultJson = JObject.Parse(output.Result);
      return resultJson;
    }

    public static JObject ConfigureGithubRepo(string apiUri, string githubOrg, string repoName, string githubPat, ILambdaContext context)
    {
      string githubUri = $"{apiUri}/repos/{githubOrg}/{repoName}";
      string jsonPayload = $@"{{""name"": ""{repoName}"", ""allow_squash_merge"": true, ""allow_merge_commit"": false, ""allow_rebase_merge"": false}}";
      context.Logger.LogLine(jsonPayload);
      context.Logger.LogLine("Building the httpClient...");
      HttpContent httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

      HttpClient httpClient = new HttpClient();
      httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"token {githubPat}");
      httpClient.DefaultRequestHeaders.Add("User-Agent", "githubmanage-lambda");

      var result = httpClient.PatchAsync(new Uri(githubUri),httpContent);
      Task.WaitAll(result);
      context.Logger.LogLine("Parsing the result");

      var output = result.Result.Content.ReadAsStringAsync();
      Task.WaitAll(output);

      context.Logger.LogLine(output.Result);
      JObject resultJson = JObject.Parse(output.Result);
      return resultJson;
    }

  }
}
