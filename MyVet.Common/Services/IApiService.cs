﻿using System.Threading.Tasks;
using MyVet.Common.Models;

namespace MyVet.Common.Services
{
    public interface IApiService
    {
        Task<Response<OwnerResponse>> GetOwnerByEmailAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken,
            string email);

        Task<Response<TokenResponse>> GetTokenAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            TokenRequest request);

        Task<bool> CheckConnection(string url);

        Task<Response<object>> RegisterUserAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            UserRequest userRequest);

        Task<Response<object>> RecoverPasswordAsync(
        string urlBase,
        string servicePrefix,
        string controller,
        EmailRequest emailRequest);

        Task<Response<object>> PutAsync<T>(
        string urlBase,
        string servicePrefix,
        string controller,
        T model,
        string tokenType,
        string accessToken);


    }
}
