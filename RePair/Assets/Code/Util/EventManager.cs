public class EventManager
{
	public delegate void GlobalMessage();
	public static event GlobalMessage OnGameStart;
	static public void InvokeGameStart()
	{
		if (OnGameStart != null)
		{
			OnGameStart();
		}
	}
}

