﻿using MachineLearing.Models;
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
using System.Web.Security;

namespace MachineLearing.Controllers
{
    public class WelcomePageController : Controller
    {
        // GET: WelcomePage
       //  bool viewDummyData = Properties.Settings.Default.ViewDummyData;
         static string msg = null;
         string[] possibleSolution;
         double[] score;
         int[] id;
        
        ObservableCollection<UserQuery> Solution = new ObservableCollection<UserQuery>();

        [HttpGet]
        public ActionResult WelcomePage()
        {
            ExpiresCache();

            if (Session["UserName"]!=null)
                return View();
            else
                return RedirectToAction("UserLogIn", "LogIn");

        }
        [HttpPost]
        public ActionResult WelcomePage(UserQuery q)
        {
           ExpiresCache();
            
            InvokeRequestResponseService(q).Wait();
            

            if(q.ThirdParty.ToString()!=null && q.Integration.ToString() !=null && q.Query !=null)
            {
                FillSolution();
            }
            else
            {
                ViewBag.ErrorMsg = msg;
            }

            //if (msg != null)
               
            //    FillSolution();               
            //     //Solution = null;           
            //else
            //{
            //   // if (viewDummyData == false)
            //   // {
            //   //     FillResponse();
            //   // }
            //   // else
            //  //  {
            //        FillSolution();
            //   // }
           // }
               

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
                     result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    SuccessResponse.RootObject responseRootObject = JsonConvert.DeserializeObject<MachineLearing.Models.SuccessResponse.RootObject>(result);
                    try
                    {
                        int length = Convert.ToInt32(responseRootObject.Results.output1.value.ColumnNames);
                        for (int i = 0; i < length; i++)
                        {
                            possibleSolution[i] = responseRootObject.Results.output1.value.ColumnNames[i];
                            score[i] = i + 2;
                            id[i] = i+1;
                        }


                      //  viewDummyData = false; //set as true to Show Dummy Data
                    }catch(Exception ex)
                    {
                       // viewDummyData = true;
                    }
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    MachineLearing.Models.ErrorResponse.RootObject rootObject = JsonConvert.DeserializeObject<MachineLearing.Models.ErrorResponse.RootObject>(responseContent);
                    msg = rootObject.error.details[0].message;
                   
                }
            }
            
        }

        private void FillResponse()
        {
            try
            {
                Session["Solution"] = null;

                for (int n = 0; n <= (id.Length - 1); n++)
                {
                    Solution.Add(new UserQuery { Id = id[n], PossibleSolution = possibleSolution[n], Score = score[n] });
                }
            }catch(Exception ex)
            {
                FillSolution();
            }
        }

        private void FillSolution()
        {
            Session["Solution"] = null;
            
            Solution.Add(new UserQuery { Id = 1, PossibleSolution = "Add recording Interface manually in config file.", Score = 0.3125 });
            Solution.Add(new UserQuery { Id = 2, PossibleSolution = "Check in the Configuration client for licensing info whether the license is expired or on the wrong location", Score = 0.0625 });
            Solution.Add(new UserQuery { Id = 3, PossibleSolution = "Make the IP of system where Command Centre is installed as static", Score = 0.1 });
            Solution.Add(new UserQuery { Id = 4, PossibleSolution = "The AvigilonGallagherCCVMS dll is not installed properly. This is done manually via RegAsm.exe /codebase command.", Score = 0.04 });
            Solution.Add(new UserQuery { Id = 5, PossibleSolution = "This seems to be an issue in CCVMS framwork but we were unable to replicate the issue", Score = 0.6 });
            Solution.Add(new UserQuery { Id = 6, PossibleSolution = "Restrict the user to configure camera tile size.", Score = 0.9 });
            Solution.Add(new UserQuery { Id = 7, PossibleSolution = "Check for resolution of following steps", Score = 0.0625 });
            Session["Solution"] = Solution;
        }

        [ChildActionOnly]
        public ActionResult _ResponseGridView()
        {
            ObservableCollection<UserQuery> SolutionList = (ObservableCollection<UserQuery>)Session["Solution"];

            return PartialView(SolutionList);
        }

        public void ExpiresCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            Response.Cache.SetNoStore();
            FormsAuthentication.SignOut();
        }

    }
}

            
        
        
    
