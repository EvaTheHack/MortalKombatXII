using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortalKombatXII.Core.Services
{
    public class BattleHostedService : IHostedService, IDisposable
    {
        private Timer _timer = null!;
        private bool IsRunning;
        private readonly BattleService _battle;

        public BattleHostedService(IServiceScopeFactory scopeFactory)
        {
            _battle = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<BattleService>();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Start, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void Start(object? state)
        {
            if (IsRunning)
            {
                return;
            }
            IsRunning = true;

            _battle.Battle();

            IsRunning = false;
            
        }
    }
}
