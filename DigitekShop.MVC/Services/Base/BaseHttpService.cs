
using System.Net.Http.Headers;
using DigitekShop.Application.Responses;
using DigitekShop.Infrastructure.LocalStorage;

namespace DigitekShop.MVC.Services.Base
{
    public class BaseHttpService
    {
        protected readonly IClient _client;
        protected readonly ILocalStorageService _localStorage;

        public BaseHttpService(IClient client, ILocalStorageService localStorage)
        {
            _client = client;
            _localStorage = localStorage;
        }

        //protected Response<Guid> ConvertApiExceptions<Guid>(BaseResponse exception)
        //{
        //    if (exception. == 400)
        //    {
        //        return new Response<Guid>() { Message = "Validation errors have occured.", ValidationErrors = exception.Response, Success = false };
        //    }
        //    else if (exception.StatusCode == 404)
        //    {
        //        return new Response<Guid>() { Message = "Not Found ...", Success = false };
        //    }
        //    else
        //    {
        //        return new Response<Guid>() { Message = "Somthing went wrong,try again ... ", Success = false };
        //    }
        //}

        protected void AddBearerToken()
        {
            if(_localStorage.Exists("token"))
            {
                _client.HttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _localStorage.GetStorageValue<string>("token"));
            }
        }

    }
}
