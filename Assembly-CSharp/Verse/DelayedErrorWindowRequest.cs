using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E11 RID: 3601
	public static class DelayedErrorWindowRequest
	{
		// Token: 0x0400357C RID: 13692
		private static List<DelayedErrorWindowRequest.Request> requests = new List<DelayedErrorWindowRequest.Request>();

		// Token: 0x060051A1 RID: 20897 RVA: 0x0029EA0C File Offset: 0x0029CE0C
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

		// Token: 0x060051A2 RID: 20898 RVA: 0x0029EAAC File Offset: 0x0029CEAC
		public static void Add(string text, string title = null)
		{
			DelayedErrorWindowRequest.Request item = default(DelayedErrorWindowRequest.Request);
			item.text = text;
			item.title = title;
			DelayedErrorWindowRequest.requests.Add(item);
		}

		// Token: 0x02000E12 RID: 3602
		private struct Request
		{
			// Token: 0x0400357D RID: 13693
			public string text;

			// Token: 0x0400357E RID: 13694
			public string title;
		}
	}
}
