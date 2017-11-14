using MachineLearing.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MachineLearing.Controllers
{
    public class WelcomePageController : Controller
    {
        // GET: WelcomePage
        static string msg = null;
        ObservableCollection<UserQuery> Solution = new ObservableCollection<UserQuery>();

        [HttpGet]
        public ActionResult WelcomePage()
        {

           return View();

        }
        [HttpPost]
        public ActionResult WelcomePage(UserQuery q)
        {
            InvokeRequestResponseService(q).Wait();
            ViewBag.ErrorMsg = msg;
            //if (msg != null)
            //{
            //    Solution = null;
            //    return PartialView("_ResponseGridView", Solution);
            //}
           
            return View();


        }

        private async Task InvokeRequestResponseService(UserQuery q)
        {
            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {

                    Inputs = new Dictionary<string, QueryRequest>() {
                        {
                            "input1",
                            new QueryRequest()
                            {
                                ColumnNames = new string[] {"Area/Third Party", "Integration", "Problem", "Solution"},
                                Values = new string[,] {  { q.ThirdParty.ToString(), q.Integration.ToString(), q.Query,""} }
                            }
                        },
                    },
              
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };

                const string apiKey = "abc123"; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/e0cc4165870b46ed8995f0ad0da9e60f/services/2e0e9035a1ed4c06bca3b7c1b53b74c0/execute?api-version=2.0&details=true");

                // WARNING: The 'await' statement below can result in a deadlock if you are calling this code from the UI thread of an ASP.Net application.
                // One way to address this would be to call ConfigureAwait(false) so that the execution does not attempt to resume on the original context.
                // For instance, replace code such as:
                //      result = await DoSomeTask()
                // with the following:
                //      result = await DoSomeTask().ConfigureAwait(false)

                string result = string.Empty;
                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                     result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Result: {0}", result);
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    MachineLearing.Models.ErrorResponse.RootObject rootObject = JsonConvert.DeserializeObject<MachineLearing.Models.ErrorResponse.RootObject>(responseContent);
                    msg = rootObject.error.details[0].message;
                   
                }
            }
            
        }


[ChildActionOnly]
public ActionResult _ResponseGridView()
{
    Solution.Add(new UserQuery { Id = 1, Response = "Add recording Interface manually in AvigilonJacques config file.", Score = 8.1 });
    Solution.Add(new UserQuery { Id = 2, Response = "Check in the Configuration client for licensing info whether the license is expired or on the wrong location", Score = 2.5 });
    Solution.Add(new UserQuery { Id = 3, Response = "Make the IP of system where Command Centre is installed as static", Score = 3.1 });
    Solution.Add(new UserQuery { Id = 4, Response = "The AvigilonGallagherCCVMS dll is not installed properly. This is done manually via RegAsm.exe /codebase command.", Score = 9.4 });
    Solution.Add(new UserQuery { Id = 5, Response = "This seems to be an issue in CCVMS framwork but we were unable to replicate the issue", Score = 2.6 });
    Solution.Add(new UserQuery { Id = 6, Response = "Restrict the user to configure camera tile size.", Score = 1.12 });
    Solution.Add(new UserQuery { Id = 7, Response = "Check for resolution of following steps", Score = 3.37 });
    return PartialView(Solution);

}
    }
}

            
        
        
    
