using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000E89 RID: 3721
	public static class MessagesRepeatAvoider
	{
		// Token: 0x060057C3 RID: 22467 RVA: 0x002CFBD2 File Offset: 0x002CDFD2
		public static void Reset()
		{
			MessagesRepeatAvoider.lastShowTimes.Clear();
		}

		// Token: 0x060057C4 RID: 22468 RVA: 0x002CFBE0 File Offset: 0x002CDFE0
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

		// Token: 0x04003A09 RID: 14857
		private static Dictionary<string, float> lastShowTimes = new Dictionary<string, float>();
	}
}
