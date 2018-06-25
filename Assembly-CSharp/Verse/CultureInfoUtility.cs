using System;
using System.Globalization;
using System.Threading;

namespace Verse
{
	// Token: 0x02000EDE RID: 3806
	public static class CultureInfoUtility
	{
		// Token: 0x04003C72 RID: 15474
		private const string EnglishCulture = "en-US";

		// Token: 0x06005A19 RID: 23065 RVA: 0x002E3F78 File Offset: 0x002E2378
		public static void EnsureEnglish()
		{
			if (Thread.CurrentThread.CurrentCulture.Name != "en-US")
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
			}
		}
	}
}
