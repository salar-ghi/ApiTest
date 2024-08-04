using System.Security.Cryptography.Pkcs;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace MyApiTwo.Middleware;

public class AuthMiddleware
{
    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
        

    }
    private readonly RequestDelegate _next;
    //private readonly string _authorizationUrl;

    public async Task InvokeAsync(HttpContext context)
    {
        //checked if the user is SignedCms in (you can implement your logic here )
        //if (!context.User.Identity.IsAuthenticated)
        //{
        //    var redirectUrl = "https://localhost:5000/api/Auth/Index?returnUrl=" + context.Request.Path;
        //    context.Response.Redirect(redirectUrl);
        //}

        if (context.User.Identity.IsAuthenticated)
        {
            //context.Response.Redirect("");
            //return;
            await _next(context);
        }
        else
        {
            // User is not signed in, redirect to login

            var redirectUrl = "https://localhost:5000/api/Auth/Index?returnUrl=" + context.Request.Path;
            context.Response.Redirect(redirectUrl);

            //context.Response.Redirect("https://localhost:5000/api/Auth/Index");
            context.Response.StatusCode = (int)HttpStatusCode.Redirect;
            //return;
        }
        //await _next(context);
    }
}
