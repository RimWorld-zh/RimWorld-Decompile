using System;
using System.Threading;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F57 RID: 3927
	public static class RealTime
	{
		// Token: 0x04003E51 RID: 15953
		public static float deltaTime;

		// Token: 0x04003E52 RID: 15954
		public static float realDeltaTime;

		// Token: 0x04003E53 RID: 15955
		public static RealtimeMoteList moteList = new RealtimeMoteList();

		// Token: 0x04003E54 RID: 15956
		public static int frameCount;

		// Token: 0x04003E55 RID: 15957
		private static float lastRealTime = 0f;

		// Token: 0x17000F43 RID: 3907
		// (get) Token: 0x06005EE1 RID: 24289 RVA: 0x00305FDC File Offset: 0x003043DC
		public static float LastRealTime
		{
			get
			{
				return RealTime.lastRealTime;
			}
		}

		// Token: 0x06005EE2 RID: 24290 RVA: 0x00305FF8 File Offset: 0x003043F8
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
	}
}
