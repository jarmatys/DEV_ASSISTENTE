using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ASSISTENTE.UI.Auth.Common.Extensions;
using ASSISTENTE.UI.Auth.Models;
using ASSISTENTE.UI.Auth.Providers.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Supabase.Gotrue;

namespace ASSISTENTE.UI.Auth.Providers
{
    public class AuthStateProvider(ILocalStorageService localStorage, SubabaseClient supabaseClient)
        : AuthenticationStateProvider
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var authStore = await localStorage.GetItemAsync<AuthStore>("auth");

            if (authStore == null)
            {
                return new AuthenticationState(user);
            }

            var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(authStore.AccessToken);

            if (tokenContent.ValidTo >= DateTime.UtcNow)
            {
                return await AuthState(authStore.AccessToken);
            }

            try
            {
                var session = await supabaseClient.Auth
                    .SignIn(Constants.SignInType.RefreshToken, authStore.RefreshToken);

                await SetSession(session);

                return await AuthState(session?.AccessToken);
            }
            catch
            {
                return new AuthenticationState(user);
            }
        }

        public async Task<AuthResponse> LoginAsync(RedirectDto model)
        {
            try
            {
                await supabaseClient.Auth.SetSession(model.AccessToken, model.RefreshToken);

                await localStorage.SetItemAsync("auth", new AuthStore(model.AccessToken, model.RefreshToken));

                var authState = AuthState(model.AccessToken);

                NotifyAuthenticationStateChanged(authState);

                return AuthResponse.Success();
            }
            catch (Exception ex)
            {
                var authError = JsonConvert.DeserializeObject<AuthError>(ex.Message);

                return AuthResponse.Fail(authError);
            }
        }

        public async Task<AuthResponse> LoginAsync(LoginDto model)
        {
            try
            {
                var session = await supabaseClient.Auth.SignIn(model.Email, model.Password);

                await SetSession(session);

                var authState = AuthState(session?.AccessToken);

                NotifyAuthenticationStateChanged(authState);

                return AuthResponse.Success();
            }
            catch (Exception ex)
            {
                var authError = JsonConvert.DeserializeObject<AuthError>(ex.Message);

                return AuthResponse.Fail(authError);
            }
        }

        private async Task SetSession(Session? result)
        {
            await localStorage.SetItemAsync("auth", new AuthStore(result?.AccessToken!, result?.RefreshToken!));
        }

        public async Task<AuthResponse> RegisterAsync(RegisterDto model)
        {
            try
            {
                var registerOptions = new SignUpOptions
                {
                    RedirectTo = "http://localhost:1008/auth/redirect",
                    Data = new Dictionary<string, object>
                    {
                        { "display_name", model.Username }
                    }
                };

                var user = await supabaseClient.Auth.SignUp(
                    Constants.SignUpType.Email,
                    model.Email,
                    model.Password,
                    registerOptions);

                return AuthResponse.Success();
            }
            catch (Exception ex)
            {
                var authError = JsonConvert.DeserializeObject<AuthError>(ex.Message);

                return AuthResponse.Fail(authError);
            }
        }

        public async Task<AuthResponse> LogoutAsync()
        {
            try
            {
                await localStorage.ClearAsync();

                await supabaseClient.Auth.SignOut();

                NotifyAuthenticationStateChanged(AuthState());

                return AuthResponse.Success();
            }
            catch (Exception ex)
            {
                var authError = JsonConvert.DeserializeObject<AuthError>(ex.Message);

                return AuthResponse.Fail(authError);
            }
        }

        private Task<AuthenticationState> AuthState(string? accessToken = null)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity());

            if (string.IsNullOrEmpty(accessToken))
                return Task.FromResult(new AuthenticationState(user));

            var claims = _jwtSecurityTokenHandler
                .ReadJwtToken(accessToken)
                .Claims
                .ToList();

            user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

            return Task.FromResult(new AuthenticationState(user));
        }
    }
}