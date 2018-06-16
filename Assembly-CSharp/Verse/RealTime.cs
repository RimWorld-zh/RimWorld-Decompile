using System;
using System.Threading;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F53 RID: 3923
	public static class RealTime
	{
		// Token: 0x17000F41 RID: 3905
		// (get) Token: 0x06005EB1 RID: 24241 RVA: 0x00303624 File Offset: 0x00301A24
		public static float LastRealTime
		{
			get
			{
				return RealTime.lastRealTime;
			}
		}

		// Token: 0x06005EB2 RID: 24242 RVA: 0x00303640 File Offset: 0x00301A40
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

		// Token: 0x04003E35 RID: 15925
		public static float deltaTime;

		// Token: 0x04003E36 RID: 15926
		public static float realDeltaTime;

		// Token: 0x04003E37 RID: 15927
		public static RealtimeMoteList moteList = new RealtimeMoteList();

		// Token: 0x04003E38 RID: 15928
		public static int frameCount;

		// Token: 0x04003E39 RID: 15929
		private static float lastRealTime = 0f;
	}
}
