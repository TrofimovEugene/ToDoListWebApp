using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoListWebApp.Context;

namespace ToDoListWebApp.Services
{
    public class TimerService : ITimerService
    {
        private readonly IEmailService _emailService;
        private readonly ToDoListContext _context;
        private readonly ILogger<TimerService> _logger;
 
        public TimerService(IEmailService emailService,
            ToDoListContext context,
            ILogger<TimerService> logger)
        {
            _emailService = emailService;
            _context = context;
            _logger = logger;
        }

        private Timer? _timer = null;

        public async void CheckReminders(object? state)
        {
            _logger.LogInformation($"Check reminders. {DateTime.Now}");
            var reminders = await _context.Reminders.ToListAsync();
            if (reminders == null) 
                return;
            foreach (var reminder in reminders.Where(reminder => DateTime.Now-reminder.ReminderDate >= TimeSpan.Zero && DateTime.Now-reminder.ReminderDate <= TimeSpan.FromSeconds(60)))
            {
                var user = await _context.Users.FindAsync(reminder.IdUser);
                _logger.LogInformation("Send mail!!!");
                await _emailService.SendEmailAsync(user.Email, reminder.Header, reminder.Text);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start timer.");
            _timer = new Timer(CheckReminders, null, 0, 60000);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stop timer.");
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}