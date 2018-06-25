using System;
using System.Globalization;
using System.Threading;

namespace Verse
{
	// Token: 0x02000EDF RID: 3807
	public static class CultureInfoUtility
	{
		// Token: 0x04003C7A RID: 15482
		private const string EnglishCulture = "en-US";

		// Token: 0x06005A19 RID: 23065 RVA: 0x002E4198 File Offset: 0x002E2598
		public static void EnsureEnglish()
		{
			if (Thread.CurrentThread.CurrentCulture.Name != "en-US")
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
			}
		}
	}
}
