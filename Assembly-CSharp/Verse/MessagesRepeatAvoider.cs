using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E88 RID: 3720
	public static class MessagesRepeatAvoider
	{
		// Token: 0x04003A19 RID: 14873
		private static Dictionary<string, float> lastShowTimes = new Dictionary<string, float>();

		// Token: 0x060057E3 RID: 22499 RVA: 0x002D17E2 File Offset: 0x002CFBE2
		public static void Reset()
		{
			MessagesRepeatAvoider.lastShowTimes.Clear();
		}

		// Token: 0x060057E4 RID: 22500 RVA: 0x002D17F0 File Offset: 0x002CFBF0
		public static bool MessageShowAllowed(string tag, float minSecondsSinceLastShow)
		{
			float num;
			if (!MessagesRepeatAvoider.lastShowTimes.TryGetValue(tag, out num))
			{
				num = -99999f;
			}
			bool flag = RealTime.LastRealTime > num + minSecondsSinceLastShow;
			if (flag)
			{
				MessagesRepeatAvoider.lastShowTimes[tag] = RealTime.LastRealTime;
			}
			return flag;
		}
	}
}
