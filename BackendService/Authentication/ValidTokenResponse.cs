public class ValidFunctionResponse
{
	public ValidFunctionResponse(int isValid, int familyID, String userID)
	{
		this.isValid = isValid;
		this.familyID = familyID;
		this.userID = userID;
	}
	public int isValid { get; set; }
	public int familyID { get; set; }
	public String userID { get; set; }
}