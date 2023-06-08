using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;

namespace BackendService;

public class Program
{
	public static void Main()
	{
		API.v1.Application.Setup(true);
	}
}

