using System;
using System.Threading;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F56 RID: 3926
	public static class RealTime
	{
		// Token: 0x04003E49 RID: 15945
		public static float deltaTime;

		// Token: 0x04003E4A RID: 15946
		public static float realDeltaTime;

		// Token: 0x04003E4B RID: 15947
		public static RealtimeMoteList moteList = new RealtimeMoteList();

		// Token: 0x04003E4C RID: 15948
		public static int frameCount;

		// Token: 0x04003E4D RID: 15949
		private static float lastRealTime = 0f;

		// Token: 0x17000F43 RID: 3907
		// (get) Token: 0x06005EE1 RID: 24289 RVA: 0x00305DBC File Offset: 0x003041BC
		public static float LastRealTime
		{
			get
			{
				return RealTime.lastRealTime;
			}
		}

		// Token: 0x06005EE2 RID: 24290 RVA: 0x00305DD8 File Offset: 0x003041D8
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
