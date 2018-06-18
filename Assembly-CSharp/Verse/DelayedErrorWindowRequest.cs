using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E11 RID: 3601
	public static class DelayedErrorWindowRequest
	{
		// Token: 0x06005189 RID: 20873 RVA: 0x0029D020 File Offset: 0x0029B420
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

		// Token: 0x0600518A RID: 20874 RVA: 0x0029D0C0 File Offset: 0x0029B4C0
		public static void Add(string text, string title = null)
		{
			DelayedErrorWindowRequest.Request item = default(DelayedErrorWindowRequest.Request);
			item.text = text;
			item.title = title;
			DelayedErrorWindowRequest.requests.Add(item);
		}

		// Token: 0x0400356E RID: 13678
		private static List<DelayedErrorWindowRequest.Request> requests = new List<DelayedErrorWindowRequest.Request>();

		// Token: 0x02000E12 RID: 3602
		private struct Request
		{
			// Token: 0x0400356F RID: 13679
			public string text;

			// Token: 0x04003570 RID: 13680
			public string title;
		}
	}
}
