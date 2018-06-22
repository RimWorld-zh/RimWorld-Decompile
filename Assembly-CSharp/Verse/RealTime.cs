using System;
using System.Threading;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F52 RID: 3922
	public static class RealTime
	{
		// Token: 0x17000F44 RID: 3908
		// (get) Token: 0x06005ED7 RID: 24279 RVA: 0x0030573C File Offset: 0x00303B3C
		public static float LastRealTime
		{
			get
			{
				return RealTime.lastRealTime;
			}
		}

		// Token: 0x06005ED8 RID: 24280 RVA: 0x00305758 File Offset: 0x00303B58
		public static void Update()
		{
			RealTime.frameCount = Time.frameCount;
			RealTime.deltaTime = Time.deltaTime;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			RealTime.realDeltaTime = realtimeSinceStartup - RealTime.lastRealTime;
			RealTime.lastRealTime = realtimeSinceStartup;
			if (Current.ProgramState == ProgramState.Playing)
			{
				RealTime.moteList.MoteListUpdate();
			}
			else
			{
				RealTime.moteList.Clear();
			}
			if (DebugSettings.lowFPS)
			{
				if (Time.deltaTime < 100f)
				{
					Thread.Sleep((int)(100f - Time.deltaTime));
				}
			}
		}

		// Token: 0x04003E46 RID: 15942
		public static float deltaTime;

		// Token: 0x04003E47 RID: 15943
		public static float realDeltaTime;

		// Token: 0x04003E48 RID: 15944
		public static RealtimeMoteList moteList = new RealtimeMoteList();

		// Token: 0x04003E49 RID: 15945
		public static int frameCount;

		// Token: 0x04003E4A RID: 15946
		private static float lastRealTime = 0f;
	}
}
