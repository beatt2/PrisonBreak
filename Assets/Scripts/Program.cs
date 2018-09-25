using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Analytics.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using UnityEngine;
namespace GoogleAPItest
{
    internal class Program
    {

        [STAThread]
        static void Main(string[] args)
        {

            //try
            //{
            //    new Program().Run().Wait();
            //}
            //catch (AggregateException ex)
            //{
            //    foreach (var e in ex.InnerExceptions)
            //    {
            //        Console.WriteLine("Error:" + e.Message);
            //    }
            //}


        }
        [STAThread]
        public static void GetInformation()
        {
            try
            {
                new Program().Run().Wait();

            }
            catch(AggregateException ex)
            {
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("Error:" + e.Message);
                }
            }
        }


        private async Task Run()
        {
            Debug.Log("hoi");
            UserCredential credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = "443778536853-v82km602ihr7ijel46vp2mpphb9nv175.apps.googleusercontent.com",
                    ClientSecret = "y2l2nRLKuy1dxq7kFulxPqvL"

                },

                new[] {AnalyticsService.Scope.Analytics},
                "user",
                CancellationToken.None,
                new FileDataStore("GoogleAPItest"));
            Debug.Log(credential);


            var service = new AnalyticsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Portfolio ",

            });
            var shelve = await service.Data.Realtime.Get("ga:170563200", "rt:activeUsers").ExecuteAsync();
            Debug.Log(shelve.TotalResults.Value);


        }

    }
}

