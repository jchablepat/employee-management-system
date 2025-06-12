using ClientLibrary.Services.Contracts;
using Microsoft.JSInterop;

namespace ClientLibrary.Services.Implementations
{
    public class AlertService(IJSRuntime JS) : IAlertService
    {
        public async Task ShowMessage(string title, string message, string icon = "info")
        {
            await JS.InvokeVoidAsync("Swal.fire", new {
                title,
                text = message,
                icon
            });
        }

        public Task ShowSuccess(string message) => ShowMessage("Éxito", message, "success");

        public Task ShowError(string message) => ShowMessage("Error", message, "error");

        public async Task<bool> ShowConfirmation(string title, string message, string confirmButtonText = "Yes", string cancelButtonText = "Cancel")
        {
            var result = await JS.InvokeAsync<JSConfirmResult>("Swal.fire", new
            {
                title,
                text = message,
                icon = "question",
                showCancelButton = true,
                confirmButtonText,
                cancelButtonText
            });

            return result.IsConfirmed;
        }

        private class JSConfirmResult
        {
            public bool IsConfirmed { get; set; }
        }

        public async Task ShowToast(string message, string icon = "success", int duration = 3000, string position = "top-end")
        {
            await JS.InvokeVoidAsync("Swal.fire", new
            {
                toast = true,
                position,
                icon,
                title = message,
                showConfirmButton = false,
                timer = duration,
                timerProgressBar = true
            });
        }

        public Task ShowSuccessToast(string message) => ShowToast(message, "success");

        public Task ShowErrorToast(string message) => ShowToast(message, "error");

        public Task ShowInfoToast(string message) => ShowToast(message, "info");

        public Task ShowWarningToast(string message) => ShowToast(message, "warning");
    }
}
