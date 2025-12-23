using System;
using System.Threading.Tasks;

namespace TodoApp.Desktop.Services;

public class ConfirmationResult
{
    public bool Confirmed { get; set; }
}

public class ConfirmationService
{
    public Func<string, string, Task<bool>>? ShowConfirmationAsync { get; set; }

    public async Task<bool> ShowConfirmation(string title, string message)
    {
        if (ShowConfirmationAsync != null)
        {
            return await ShowConfirmationAsync(title, message);
        }
        return false;
    }
}

