using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E8B RID: 3723
	public static class MessagesRepeatAvoider
	{
		// Token: 0x04003A21 RID: 14881
		private static Dictionary<string, float> lastShowTimes = new Dictionary<string, float>();

		// Token: 0x060057E7 RID: 22503 RVA: 0x002D1AFA File Offset: 0x002CFEFA
		public static void Reset()
		{
			MessagesRepeatAvoider.lastShowTimes.Clear();
		}

		// Token: 0x060057E8 RID: 22504 RVA: 0x002D1B08 File Offset: 0x002CFF08
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
