using System;

namespace Steamworks
{
	// Token: 0x02000002 RID: 2
	public static class CallbackDispatcher
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static void ExceptionHandler(Exception e)
		{
			Console.WriteLine(e.Message);
		}
	}
}
