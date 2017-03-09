using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace JWTTokens.Models
{
    public static class RequestUtils
    {
        public static string BaseUrl(this HttpRequestMessage request)
        {
            return request.RequestUri.ToString().Replace(request.RequestUri.LocalPath, "").Split('?')[0];
        }

        public static string Header(this HttpRequestMessage request, string headerName)
        {
            var header = request.Headers.FirstOrDefault(f => f.Key == headerName);
            return header.IsDefault() ? null : header.Value?.First();
        }

        public static string BaseUrl(this HttpActionContext actionContext )
        {
            return actionContext.Request.BaseUrl();
        }

        public static string Header(this HttpActionContext actionContext, string headerName)
        {
            return actionContext.Request.Header(headerName);
        }


        public static bool IsDefault<T>(this T value) where T : struct
        {
            bool isDefault = value.Equals(default(T));

            return isDefault;
        }
    }
}
