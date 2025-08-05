using BaseLibrary.Entities;
using Blazored.LocalStorage;
using Client;
using Client.ApplicationStates;
using ClientLibrary.Helpers;
using ClientLibrary.Services.Contracts;
using ClientLibrary.Services.Implementations;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Popups;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<CustomHttpHandler>();
builder.Services.AddHttpClient("SystemApiClient", client => {
    client.BaseAddress = new Uri("https://employees-api-ajc5czd6f2h0a5ee.canadacentral-01.azurewebsites.net");
}).AddHttpMessageHandler<CustomHttpHandler>();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7272") }); //builder.HostEnvironment.BaseAddress
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<GetHttpClient>();
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();
builder.Services.AddScoped<IGenericService<GeneralDepartment>, GenericService<GeneralDepartment>>();
builder.Services.AddScoped<IGenericService<Department>, GenericService<Department>>();
builder.Services.AddScoped<IGenericService<Branch>, GenericService<Branch>>();
builder.Services.AddScoped<IGenericService<Country>, GenericService<Country>>();
builder.Services.AddScoped<IGenericService<City>, GenericService<City>>();
builder.Services.AddScoped<IGenericService<Town>, GenericService<Town>>();

builder.Services.AddScoped<IGenericService<Vacation>, GenericService<Vacation>>();
builder.Services.AddScoped<IGenericService<VacationType>, GenericService<VacationType>>();

builder.Services.AddScoped<IGenericService<Overtime>, GenericService<Overtime>>();
builder.Services.AddScoped<IGenericService<OvertimeType>, GenericService<OvertimeType>>();

builder.Services.AddScoped<IGenericService<Sanction>, GenericService<Sanction>>();
builder.Services.AddScoped<IGenericService<SanctionType>, GenericService<SanctionType>>();

builder.Services.AddScoped<IGenericService<Doctor>, GenericService<Doctor>>();
builder.Services.AddScoped<IGenericService<Employee>, GenericService<Employee>>();

builder.Services.AddScoped<AllState>();
builder.Services.AddSyncfusionBlazor();
builder.Services.AddMudServices();
builder.Services.AddScoped<SfDialogService>();
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<IExcelService, ExcelService>();
builder.Services.AddScoped<IPdfService, PdfService>();

await builder.Build().RunAsync();
