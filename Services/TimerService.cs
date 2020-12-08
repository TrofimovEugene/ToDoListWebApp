using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoListWebApp.Context;

namespace ToDoListWebApp.Services
{
    public class TimerService : ITimerService
    {
        private readonly IEmailService _emailService;
        private readonly ToDoListContext _context;
 
        public TimerService(IEmailService emailService,
            ToDoListContext context)
        {
            _emailService = emailService;
            _context = context;
        }

        private Timer? _timer = null;

        public async void CheckReminders(object? state)
        {
            Trace.WriteLine("Check reminders");
            Trace.WriteLine($"{DateTime.Now}");
            var reminders = await _context.Reminders.ToListAsync();
            if (reminders == null) 
                return;
            foreach (var reminder in reminders.Where(reminder => (DateTime.Now.Subtract(reminder.ReminderDate)) <= TimeSpan.FromSeconds(60)!))
            {
                var user = await _context.Users.FindAsync(reminder.IdUser);
                Trace.WriteLine("Send mail!!!");
                await _emailService.SendEmailAsync(user.Email, "Напоминание", reminder.Header);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(CheckReminders, null, 0, 30000);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}