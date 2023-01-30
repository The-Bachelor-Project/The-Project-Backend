using System.Data.SqlClient;

namespace BackendService;

class AddPortfolio
{
    public static AddPortfolioResponse endpoint(AddPortfolioBody body)
    {
        AddPortfolioResponse addPortfolioResponse = new AddPortfolioResponse();

        using (SqlConnection connection = Database.createConnection())
        {
            String uid = RandomStringGenerator.Generate(32);
            String query = "INSERT INTO Portfolios (uid, name, owner, currency, balance) VALUES (@uid, @name, @owner, @currency, @balance)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@uid", uid);
            command.Parameters.AddWithValue("@name", body.name);
            command.Parameters.AddWithValue("@owner", body.owner);
            command.Parameters.AddWithValue("@currency", body.currency);
            command.Parameters.AddWithValue("@balance", body.balance);
            try
            {
                command.ExecuteNonQuery();
                addPortfolioResponse.response = "success";
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e);
                addPortfolioResponse.response = "error";
            }
        }

        return addPortfolioResponse;
    }
}

class AddPortfolioResponse
{
    public String response { get; set; }
}

class AddPortfolioBody
{
    public String name { get; set; }
    public String owner { get; set; }
    public String currency { get; set; }
    public Double balance { get; set; }
}