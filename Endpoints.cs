using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
class Endpoints
{
    public static void setup() 
    {
        Application.app.MapPost(API_ENDPOINTS.SIGN_UP_ENDPOINT, (SignUpBody body) => 
        {
            SignUp signUp = new SignUp();
            return Results.Ok(signUp.endpoint(body));
        });
    }
}