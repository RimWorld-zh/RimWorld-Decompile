using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E8A RID: 3722
	public static class MessagesRepeatAvoider
	{
		// Token: 0x060057C5 RID: 22469 RVA: 0x002CFBD2 File Offset: 0x002CDFD2
		public static void Reset()
		{
			MessagesRepeatAvoider.lastShowTimes.Clear();
		}

		// Token: 0x060057C6 RID: 22470 RVA: 0x002CFBE0 File Offset: 0x002CDFE0
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

		// Token: 0x04003A0B RID: 14859
		private static Dictionary<string, float> lastShowTimes = new Dictionary<string, float>();
	}
}
