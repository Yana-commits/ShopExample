using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Interfaces
{
    public interface IInternalHttpClientService
    {
        Task<TResponse> SendAsync<TResponse, TRequest>(string url, HttpMethod method, TRequest? request);
    }
}
