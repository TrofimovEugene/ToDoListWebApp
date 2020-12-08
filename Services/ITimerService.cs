using System;
using Microsoft.Extensions.Hosting;

namespace ToDoListWebApp.Services
{
    public interface ITimerService : IHostedService, IDisposable
    {
    }
}