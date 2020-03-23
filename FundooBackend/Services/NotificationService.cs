using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FundooAppBackend.Services
{
    public class NotificationService : HostedService
    {

        private readonly NotificationProvider _notificationProvider;

        public NotificationService(NotificationProvider notificationProvider)
        {
            _notificationProvider = notificationProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while(!stoppingToken.IsCancellationRequested)
            {
                await _notificationProvider.SendNotification(stoppingToken);
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }

        }
    }
}
