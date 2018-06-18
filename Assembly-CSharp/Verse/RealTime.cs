using System;
using System.Threading;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F52 RID: 3922
	public static class RealTime
	{
		// Token: 0x17000F40 RID: 3904
		// (get) Token: 0x06005EAF RID: 24239 RVA: 0x00303700 File Offset: 0x00301B00
		public static float LastRealTime
		{
			get
			{
				return RealTime.lastRealTime;
			}
		}

		// Token: 0x06005EB0 RID: 24240 RVA: 0x0030371C File Offset: 0x00301B1C
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

		// Token: 0x04003E34 RID: 15924
		public static float deltaTime;

		// Token: 0x04003E35 RID: 15925
		public static float realDeltaTime;

		// Token: 0x04003E36 RID: 15926
		public static RealtimeMoteList moteList = new RealtimeMoteList();

		// Token: 0x04003E37 RID: 15927
		public static int frameCount;

		// Token: 0x04003E38 RID: 15928
		private static float lastRealTime = 0f;
	}
}
