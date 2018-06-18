using System;
using System.Globalization;
using System.Threading;

namespace Verse
{
	// Token: 0x02000EDD RID: 3805
	public static class CultureInfoUtility
	{
		// Token: 0x060059F5 RID: 23029 RVA: 0x002E2044 File Offset: 0x002E0444
		public static void EnsureEnglish()
		{
			if (Thread.CurrentThread.CurrentCulture.Name != "en-US")
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
			}
		}

		// Token: 0x04003C62 RID: 15458
		private const string EnglishCulture = "en-US";
	}
}
