namespace API.v1;

public class PostUserPreferencesBody
{
	public String setting { get; set; }
	public String value { get; set; }

	public PostUserPreferencesBody(String setting, String value)
	{
		this.setting = setting;
		this.value = value;
	}

}
