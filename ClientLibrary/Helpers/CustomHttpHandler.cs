using BaseLibrary.DTOs;
using ClientLibrary.Services.Contracts;
using System.Net;

namespace ClientLibrary.Helpers
{
    /// <summary>
    /// Custom Http Handler to intercept and check if the request has a status code of Unauthorized. If so, then refresh the token and update the localStorage.
    /// </summary>
    /// <param name="getHttpClient"></param>
    /// <param name="localStorageService"></param>
    /// <param name="accountService"></param>
    public class CustomHttpHandler(GetHttpClient getHttpClient, LocalStorageService localStorageService, IUserAccountService accountService) : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            bool loginUrl = request.RequestUri!.AbsoluteUri.Contains("login");
            bool registerUrl = request.RequestUri!.AbsoluteUri.Contains("register");
            bool refreshTokenUrl = request.RequestUri!.AbsoluteUri.Contains("refresh-token");

            // If the request is for login, register or refresh token, then send the request without checking the token (public routes).
            if (loginUrl || registerUrl || refreshTokenUrl) return await base.SendAsync(request, cancellationToken); 

            var result = await base.SendAsync(request, cancellationToken);
            if(result.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Get token from localStorage
                var stringToken = await localStorageService.GetToken();
                if (stringToken is null) return result;

                // Check if the header containers token
                string token = string.Empty;
                try { token = request.Headers.Authorization!.Parameter!; }
                catch { }

                var deserializedToken = Serializations.DeserializeJsonString<UserSession>(stringToken);
                if (deserializedToken is null) return result;

                // If the header does not contain the token, then add it to the header and send the request again.
                if (string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", deserializedToken.Token);
                    return await base.SendAsync(request, cancellationToken);
                }

                // Call for refresh token
                var newJwtToken = await GetRefreshToken(deserializedToken.RefreshToken!);
                if (string.IsNullOrEmpty(newJwtToken)) return result;

                // Update the header with the new token and send the request again.
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", newJwtToken);
                return await base.SendAsync(request, cancellationToken);
            }

            return result;
        }

        /// <summary>
        /// Call the refresh token endpoint and update the localStorage
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        private async Task<string> GetRefreshToken(string refreshToken) 
        {
            var result = await accountService.RefreshTokenAsync(new RefreshToken { Token = refreshToken });
            string serializedtoken = Serializations.SerializeObj(new UserSession { Token = result?.Token, RefreshToken = result?.RefreshToken });

            await localStorageService.SetToken(serializedtoken);

            return result?.Token!;
        }
    }
}
