using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BCA RID: 3018
	public sealed class GameInfo : IExposable
	{
		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x060041A7 RID: 16807 RVA: 0x00229D6C File Offset: 0x0022816C
		public float RealPlayTimeInteracting
		{
			get
			{
				return this.realPlayTimeInteracting;
			}
		}

		// Token: 0x060041A8 RID: 16808 RVA: 0x00229D87 File Offset: 0x00228187
		public void GameInfoOnGUI()
		{
			if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseMove || Event.current.type == EventType.KeyDown)
			{
				this.lastInputRealTime = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x060041A9 RID: 16809 RVA: 0x00229DC4 File Offset: 0x002281C4
		public void GameInfoUpdate()
		{
			if (Time.realtimeSinceStartup < this.lastInputRealTime + 90f && Find.MainTabsRoot.OpenTab != MainButtonDefOf.Menu && Current.ProgramState == ProgramState.Playing && !Find.WindowStack.IsOpen<Dialog_Options>())
			{
				this.realPlayTimeInteracting += RealTime.realDeltaTime;
			}
		}

		// Token: 0x060041AA RID: 16810 RVA: 0x00229E2A File Offset: 0x0022822A
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.realPlayTimeInteracting, "realPlayTimeInteracting", 0f, false);
			Scribe_Values.Look<bool>(ref this.permadeathMode, "permadeathMode", false, false);
			Scribe_Values.Look<string>(ref this.permadeathModeUniqueName, "permadeathModeUniqueName", null, false);
		}

		// Token: 0x04002CD1 RID: 11473
		public bool permadeathMode = false;

		// Token: 0x04002CD2 RID: 11474
		public string permadeathModeUniqueName = null;

		// Token: 0x04002CD3 RID: 11475
		private float realPlayTimeInteracting = 0f;

		// Token: 0x04002CD4 RID: 11476
		private float lastInputRealTime = 0f;
	}
}
