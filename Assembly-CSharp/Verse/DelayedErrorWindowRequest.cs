using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E12 RID: 3602
	public static class DelayedErrorWindowRequest
	{
		// Token: 0x0600518B RID: 20875 RVA: 0x0029D040 File Offset: 0x0029B440
		public static void DelayedErrorWindowRequestOnGUI()
		{
			try
			{
				for (int i = 0; i < DelayedErrorWindowRequest.requests.Count; i++)
				{
					WindowStack windowStack = Find.WindowStack;
					string text = DelayedErrorWindowRequest.requests[i].text;
					string buttonAText = "OK".Translate();
					string title = DelayedErrorWindowRequest.requests[i].title;
					windowStack.Add(new Dialog_MessageBox(text, buttonAText, null, null, null, title, false, null, null));
				}
			}
			finally
			{
				DelayedErrorWindowRequest.requests.Clear();
			}
		}

		// Token: 0x0600518C RID: 20876 RVA: 0x0029D0E0 File Offset: 0x0029B4E0
		public static void Add(string text, string title = null)
		{
			DelayedErrorWindowRequest.Request item = default(DelayedErrorWindowRequest.Request);
			item.text = text;
			item.title = title;
			DelayedErrorWindowRequest.requests.Add(item);
		}

		// Token: 0x04003570 RID: 13680
		private static List<DelayedErrorWindowRequest.Request> requests = new List<DelayedErrorWindowRequest.Request>();

		// Token: 0x02000E13 RID: 3603
		private struct Request
		{
			// Token: 0x04003571 RID: 13681
			public string text;

			// Token: 0x04003572 RID: 13682
			public string title;
		}
	}
}
