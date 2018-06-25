using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E8A RID: 3722
	public static class MessagesRepeatAvoider
	{
		// Token: 0x04003A19 RID: 14873
		private static Dictionary<string, float> lastShowTimes = new Dictionary<string, float>();

		// Token: 0x060057E7 RID: 22503 RVA: 0x002D190E File Offset: 0x002CFD0E
		public static void Reset()
		{
			MessagesRepeatAvoider.lastShowTimes.Clear();
		}

		// Token: 0x060057E8 RID: 22504 RVA: 0x002D191C File Offset: 0x002CFD1C
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
