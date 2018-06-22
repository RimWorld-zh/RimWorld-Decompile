using System;
using System.Globalization;
using System.Threading;

namespace Verse
{
	// Token: 0x02000EDC RID: 3804
	public static class CultureInfoUtility
	{
		// Token: 0x06005A16 RID: 23062 RVA: 0x002E3E58 File Offset: 0x002E2258
		public static void EnsureEnglish()
		{
			if (Thread.CurrentThread.CurrentCulture.Name != "en-US")
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
			}
		}

		// Token: 0x04003C72 RID: 15474
		private const string EnglishCulture = "en-US";
	}
}
