using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ASSISTENTE.UI.Auth.Common.Extensions;
using ASSISTENTE.UI.Auth.Models;
using ASSISTENTE.UI.Auth.Providers.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
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

            if (authStore?.AccessToken == null)
            {
                return new AuthenticationState(user);
            }

            var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(authStore?.AccessToken);

            if (tokenContent.ValidTo < DateTime.Now)
            {
                // TODO: Logic to refresh token
                
                return new AuthenticationState(user);
            }

            var claims = GetClaims(authStore?.AccessToken!);

            user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

            return new AuthenticationState(user);
        }

        public async Task LoginAsync(RedirectDto model)
        {
            // TODO: parsed error to AuthError.cs
            
            await supabaseClient.Auth.SetSession(model.AccessToken, model.RefreshToken);

            await localStorage.SetItemAsync("auth", new AuthStore(model.AccessToken, model.RefreshToken));

            var claims = GetClaims(model.AccessToken);

            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

            var authState = Task.FromResult(new AuthenticationState(user));

            NotifyAuthenticationStateChanged(authState);
        }

        public async Task LoginAsync(LoginDto model)
        {
            // TODO: parsed error to AuthError.cs
            
            var result = await supabaseClient.Auth.SignIn(model.Email, model.Password);

            await localStorage.SetItemAsync("auth", new AuthStore(result?.AccessToken!, result?.RefreshToken!));

            var claims = GetClaims(result?.AccessToken!);

            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

            var authState = Task.FromResult(new AuthenticationState(user));

            NotifyAuthenticationStateChanged(authState);
        }

        public async Task RegisterAsync(RegisterDto model)
        {
            // TODO: parsed error to AuthError.cs

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
        }

        public async Task LogoutAsync()
        {
            await localStorage.ClearAsync();

            await supabaseClient.Auth.SignOut();
            
            var nobody = new ClaimsPrincipal(new ClaimsIdentity());

            var authState = Task.FromResult(new AuthenticationState(nobody));

            NotifyAuthenticationStateChanged(authState);
        }

        private List<Claim> GetClaims(string token)
        {
            var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(token);
            var claims = tokenContent.Claims.ToList();

            return claims;
        }
    }
}