using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FundooAppBackend.Services
{
    public abstract class HostedService : IHostedService, IDisposable
    {

        private Task _executingTask;
        private CancellationTokenSource _stoppingCts;

        protected abstract Task ExecuteAsync(CancellationToken stoppingToken);

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _stoppingCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            _executingTask = ExecuteAsync(_stoppingCts.Token);

            return _executingTask.IsCompleted ? _executingTask : Task.CompletedTask;

        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executingTask == null)
                return;

            try
            {
                _stoppingCts.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

        public void Dispose()
        {
            _stoppingCts.Cancel();
        }
    }
}
