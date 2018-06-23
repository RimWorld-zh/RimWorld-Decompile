using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E0E RID: 3598
	public static class DelayedErrorWindowRequest
	{
		// Token: 0x04003575 RID: 13685
		private static List<DelayedErrorWindowRequest.Request> requests = new List<DelayedErrorWindowRequest.Request>();

		// Token: 0x0600519D RID: 20893 RVA: 0x0029E600 File Offset: 0x0029CA00
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

		// Token: 0x0600519E RID: 20894 RVA: 0x0029E6A0 File Offset: 0x0029CAA0
		public static void Add(string text, string title = null)
		{
			DelayedErrorWindowRequest.Request item = default(DelayedErrorWindowRequest.Request);
			item.text = text;
			item.title = title;
			DelayedErrorWindowRequest.requests.Add(item);
		}

		// Token: 0x02000E0F RID: 3599
		private struct Request
		{
			// Token: 0x04003576 RID: 13686
			public string text;

			// Token: 0x04003577 RID: 13687
			public string title;
		}
	}
}
