using System;

namespace Steamworks
{
	public static class CallbackDispatcher
	{
		public static void ExceptionHandler(Exception e)
		{
			Console.WriteLine(e.Message);
		}
	}
}
