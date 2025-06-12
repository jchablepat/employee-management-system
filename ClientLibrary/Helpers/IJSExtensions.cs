
using Microsoft.JSInterop;

namespace ClientLibrary.Helpers
{
    /// <summary>
    /// Extension methods for IJSRuntime to show SweetAlert messages.
    /// </summary>
    public static class IJSExtensions
    {
        /// <summary>
        /// Shows a SweetAlert message with the specified title, message, and icon.
        /// </summary>
        /// <param name="js"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static async Task ShowMessage(this IJSRuntime js, string title, string message, string icon = "info") {
            await js.InvokeVoidAsync("Swal.fire", new {
                title,
                text = message,
                icon
            });
        }

        public static Task ShowSuccess(this IJSRuntime js, string message) => js.ShowMessage("Éxito", message, "success");

        public static Task ShowError(this IJSRuntime js, string message) => js.ShowMessage("Error", message, "error");

        public static async Task<bool> ShowConfirmation(this IJSRuntime js, string title, string message, string confirmButtonText = "Sí", string cancelButtonText = "Cancelar")
        {
            var result = await js.InvokeAsync<JSConfirmResult>("Swal.fire", new {
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
    }
}
