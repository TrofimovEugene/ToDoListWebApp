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
        
        public void InvokeAsync()
        {
            if (_timer == null)
                return;
            
            _timer = new Timer(async delegate
            {
                await CheckReminders();
            }, null, 0, 30000);
        }

        public void DisableTimer()
        {
            _timer?.DisposeAsync();
        }

        private async Task CheckReminders()
        {
            Debug.WriteLine("Check reminders");
            var reminders = await _context.Reminders.ToListAsync();
            var dateTimeNow = new DateTime(DateTime.Now.ToBinary());
            dateTimeNow = dateTimeNow.AddSeconds(dateTimeNow.Second * -1).AddMilliseconds(dateTimeNow.Second * -1);
            if (reminders != null)
                foreach (var reminder in reminders.Where(reminder => reminder.ReminderDate == dateTimeNow)!)
                {
                    var user = await _context.Users.FindAsync(reminder.IdUser);
                    Debug.WriteLine("Send mail!!!");
                    await _emailService.SendEmailAsync(user.Email, "Напоминание", reminder.Header);
                }
        }
    }
}