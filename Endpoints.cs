using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
class Endpoints
{
    public static void setup() 
    {
        Application.app.MapPost(API_ENDPOINTS.SIGN_UP_ENDPOINT, (SignUpBody body) => 
        {
            return Results.Ok(SignUp.endpoint(body));
        });

        Application.app.MapPost(API_ENDPOINTS.SIGN_IN_ENDPOINT, (SignInBody body) => 
        {
            return Results.Ok(SignIn.endpoint(body));
        });
    }
}