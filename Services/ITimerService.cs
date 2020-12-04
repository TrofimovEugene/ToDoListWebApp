namespace ToDoListWebApp.Services
{
    public interface ITimerService
    {
        void InvokeAsync();

        void DisableTimer();
    }
}