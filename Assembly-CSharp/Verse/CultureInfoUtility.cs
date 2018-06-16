using System;
using System.Globalization;
using System.Threading;

namespace Verse
{
	// Token: 0x02000EDE RID: 3806
	public static class CultureInfoUtility
	{
		// Token: 0x060059F7 RID: 23031 RVA: 0x002E1F6C File Offset: 0x002E036C
		public static void EnsureEnglish()
		{
			if (Thread.CurrentThread.CurrentCulture.Name != "en-US")
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
			}
		}

		// Token: 0x04003C63 RID: 15459
		private const string EnglishCulture = "en-US";
	}
}
