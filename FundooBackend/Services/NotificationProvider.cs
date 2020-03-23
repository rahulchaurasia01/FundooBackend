using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FundooAppBackend.Services
{
    public class NotificationProvider
    {

        private const string notificationProviderString = "http://localhost:49798/api/User/ReminderNotification";

        private readonly HttpClient _httpClient;

        public NotificationProvider()
        {
            _httpClient = new HttpClient();
        }

        public async Task SendNotification(CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsync(notificationProviderString, null, cancellationToken);


        }


    }
}
