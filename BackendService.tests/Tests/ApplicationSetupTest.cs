using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace BackendService.tests;

[TestClass]
public class ApplicationSetupTest
{
	[TestMethod]
	public void SetupApplicationTest()
	{
		try
		{
			API.v1.Application.Setup(false);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Application could not setup correctly: " + e.Message);
		}
	}

	[TestMethod]
	public void SetupEndpoint_PostUsersTest()
	{
		try
		{
			WebApplication app = WebApplication.CreateBuilder().Build();
			API.v1.PostUsers.Setup(app);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Endpoint could not setup correctly: " + e.Message);
		}
	}

	[TestMethod]
	public void SetupEndpoint_GetStockProfiles()
	{
		try
		{
			WebApplication app = WebApplication.CreateBuilder().Build();
			API.v1.GetStockProfiles.Setup(app);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Endpoint could not setup correctly: " + e.Message);
		}
	}

	[TestMethod]
	public void SetupEndpoint_GetPortfolios()
	{
		try
		{
			WebApplication app = WebApplication.CreateBuilder().Build();
			API.v1.GetPortfolios.Setup(app);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Endpoint could not setup correctly: " + e.Message);
		}
	}

	[TestMethod]
	public void SetupEndpoint_PostPortfolios()
	{
		try
		{
			WebApplication app = WebApplication.CreateBuilder().Build();
			API.v1.PostPortfolios.Setup(app);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Endpoint could not setup correctly: " + e.Message);
		}
	}

	[TestMethod]
	public void SetupEndpoint_GetStockHistories()
	{
		try
		{
			WebApplication app = WebApplication.CreateBuilder().Build();
			API.v1.GetStockHistories.Setup(app);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Endpoint could not setup correctly: " + e.Message);
		}
	}

	[TestMethod]
	public void SetupEndpoint_PutTokens()
	{
		try
		{
			WebApplication app = WebApplication.CreateBuilder().Build();
			API.v1.PutTokens.Setup(app);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Endpoint could not setup correctly: " + e.Message);
		}
	}

	[TestMethod]
	public void SetupEndpoint_PostTokens()
	{
		try
		{
			WebApplication app = WebApplication.CreateBuilder().Build();
			API.v1.PostTokens.Setup(app);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Endpoint could not setup correctly: " + e.Message);
		}
	}

	[TestMethod]
	public void SetupEndpoint_GetSearchResults()
	{
		try
		{
			WebApplication app = WebApplication.CreateBuilder().Build();
			API.v1.GetSearchResults.Setup(app);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Endpoint could not setup correctly: " + e.Message);
		}
	}

	[TestMethod]
	public void SetupEndpoint_GetStockTransactions()
	{
		try
		{
			WebApplication app = WebApplication.CreateBuilder().Build();
			API.v1.GetStockTransactions.Setup(app);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Endpoint could not setup correctly: " + e.Message);
		}
	}

	[TestMethod]
	public void SetupEndpoint_PostStockTransactions()
	{
		try
		{
			WebApplication app = WebApplication.CreateBuilder().Build();
			API.v1.PostStockTransactions.Setup(app);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Endpoint could not setup correctly: " + e.Message);
		}
	}

	[TestMethod]
	public void SetupEndpoint_GetTransactions()
	{
		try
		{
			WebApplication app = WebApplication.CreateBuilder().Build();
			API.v1.GetTransactions.Setup(app);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Endpoint could not setup correctly: " + e.Message);
		}
	}

	[TestMethod]
	public void SetupEndpoint_GetValueHistory()
	{
		try
		{
			WebApplication app = WebApplication.CreateBuilder().Build();
			API.v1.GetValueHistory.Setup(app);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Endpoint could not setup correctly: " + e.Message);
		}
	}

	[TestMethod]
	public void SetupEndpoint_PutUsers()
	{
		try
		{
			WebApplication app = WebApplication.CreateBuilder().Build();
			API.v1.PutUsers.Setup(app);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Endpoint could not setup correctly: " + e.Message);
		}
	}

	[TestMethod]
	public void SetupEndpoint_PutPortfolios()
	{
		try
		{
			WebApplication app = WebApplication.CreateBuilder().Build();
			API.v1.PutPortfolios.Setup(app);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Endpoint could not setup correctly: " + e.Message);
		}
	}

	[TestMethod]
	public void SetupEndpoint_PutStockTransactions()
	{
		try
		{
			WebApplication app = WebApplication.CreateBuilder().Build();
			API.v1.PutStockTransactions.Setup(app);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Endpoint could not setup correctly: " + e.Message);
		}
	}

	[TestMethod]
	public void SetupEndpoint_DeleteStockTransactions()
	{
		try
		{
			WebApplication app = WebApplication.CreateBuilder().Build();
			API.v1.DeleteStockTransactions.Setup(app);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Endpoint could not setup correctly: " + e.Message);
		}
	}

	[TestMethod]
	public void SetupEndpoint_DeletePortfolios()
	{
		try
		{
			WebApplication app = WebApplication.CreateBuilder().Build();
			API.v1.DeletePortfolios.Setup(app);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Endpoint could not setup correctly: " + e.Message);
		}
	}

	[TestMethod]
	public void SetupEndpoint_DeleteUsers()
	{
		try
		{
			WebApplication app = WebApplication.CreateBuilder().Build();
			API.v1.DeleteUsers.Setup(app);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Endpoint could not setup correctly: " + e.Message);
		}
	}

	[TestMethod]
	public void SetupEndpoint_PostCashTransactions()
	{
		try
		{
			WebApplication app = WebApplication.CreateBuilder().Build();
			API.v1.PostCashTransactions.Setup(app);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Endpoint could not setup correctly: " + e.Message);
		}
	}

	[TestMethod]
	public void SetupEndpoint_PostUserPreferences()
	{
		try
		{
			WebApplication app = WebApplication.CreateBuilder().Build();
			API.v1.PostUserPreferences.Setup(app);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Endpoint could not setup correctly: " + e.Message);
		}
	}

	[TestMethod]
	public void SetupEndpoint_GetUserPreferences()
	{
		try
		{
			WebApplication app = WebApplication.CreateBuilder().Build();
			API.v1.GetUserPreferences.Setup(app);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Endpoint could not setup correctly: " + e.Message);
		}
	}

	[TestMethod]
	public void SetupEndpoint_DeleteUserPreferences()
	{
		try
		{
			WebApplication app = WebApplication.CreateBuilder().Build();
			API.v1.DeleteUserPreferences.Setup(app);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Endpoint could not setup correctly: " + e.Message);
		}
	}

	[TestMethod]
	public void SetupEndpoint_DeleteCashTransactions()
	{
		try
		{
			WebApplication app = WebApplication.CreateBuilder().Build();
			API.v1.DeleteCashTransactions.Setup(app);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Endpoint could not setup correctly: " + e.Message);
		}
	}

	[TestMethod]
	public void SetupEndpoint_PutCashTransactions()
	{
		try
		{
			WebApplication app = WebApplication.CreateBuilder().Build();
			API.v1.PutCashTransactions.Setup(app);
		}
		catch (System.Exception e)
		{
			Assert.Fail("Endpoint could not setup correctly: " + e.Message);
		}
	}
}