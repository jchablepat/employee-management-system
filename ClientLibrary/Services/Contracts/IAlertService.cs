namespace ClientLibrary.Services.Contracts
{
    public interface IAlertService
    {
        Task ShowMessage(string title, string message, string icon = "info");
        Task ShowSuccess(string message);
        Task ShowError(string message);
        Task<bool> ShowConfirmation(string title, string message, string confirmButtonText = "Sí", string cancelButtonText = "Cancelar");
        public Task ShowSuccessToast(string message);
        public Task ShowErrorToast(string message);
        public Task ShowWarningToast(string message);
        public Task ShowInfoToast(string message);
        public Task ShowToast(string message, string icon = "success", int duration = 2000, string position = "top-end");
    }
}
