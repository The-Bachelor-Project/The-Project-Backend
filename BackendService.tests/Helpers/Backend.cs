namespace BackendService.tests;

class Backend{
	private static bool isRunning = false;
	public static void Start(){
		if(!isRunning){
			Thread thread = new Thread(Program.Main);
			thread.Start();
			isRunning = true;
		}
	}
}

