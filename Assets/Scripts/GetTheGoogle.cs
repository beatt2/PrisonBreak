using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Door;
using Google.Apis.Analytics.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;

public class GetTheGoogle
{
    private static AnalyticsService _service;

    public GetTheGoogle()
    {
        String serviceAcountEmail = "beatt2@portfolio-196511.iam.gserviceaccount.com";
        var certificate = new X509Certificate2(@"key.p12", "notasecret", X509KeyStorageFlags.Exportable);
        ServiceAccountCredential credential = new ServiceAccountCredential(
            new ServiceAccountCredential.Initializer(serviceAcountEmail)
            {
                Scopes = new[] {AnalyticsService.Scope.Analytics}
            }.FromCertificate(certificate));

        _service = new AnalyticsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "Portfolio ",
        });

        SecuritySucks.Instate();

    }

    public void GetGoogle()
    {
        DoItAsync();
    }

    private async void DoItAsync()
    {
        int value = 0;
        await Task.Run(() =>
        {
            
            var temp = _service.Data.Realtime.Get("ga:170563200", "rt:activeUsers").ExecuteAsync();
            try
            {
                value = int.Parse(temp.Result.TotalsForAllResults["rt:activeUsers"]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        });
        DoorManager.Instance.GoogleValueFound(value);
    }
}