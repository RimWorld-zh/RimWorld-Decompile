using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BC9 RID: 3017
	public sealed class GameInfo : IExposable
	{
		// Token: 0x04002CDD RID: 11485
		public bool permadeathMode = false;

		// Token: 0x04002CDE RID: 11486
		public string permadeathModeUniqueName = null;

		// Token: 0x04002CDF RID: 11487
		private float realPlayTimeInteracting = 0f;

		// Token: 0x04002CE0 RID: 11488
		private float lastInputRealTime = 0f;

		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x060041AE RID: 16814 RVA: 0x0022A874 File Offset: 0x00228C74
		public float RealPlayTimeInteracting
		{
			get
			{
				return this.realPlayTimeInteracting;
			}
		}

		// Token: 0x060041AF RID: 16815 RVA: 0x0022A88F File Offset: 0x00228C8F
		public void GameInfoOnGUI()
		{
			if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseMove || Event.current.type == EventType.KeyDown)
			{
				this.lastInputRealTime = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x060041B0 RID: 16816 RVA: 0x0022A8CC File Offset: 0x00228CCC
		public void GameInfoUpdate()
		{
			if (Time.realtimeSinceStartup < this.lastInputRealTime + 90f && Find.MainTabsRoot.OpenTab != MainButtonDefOf.Menu && Current.ProgramState == ProgramState.Playing && !Find.WindowStack.IsOpen<Dialog_Options>())
			{
				this.realPlayTimeInteracting += RealTime.realDeltaTime;
			}
		}

		// Token: 0x060041B1 RID: 16817 RVA: 0x0022A932 File Offset: 0x00228D32
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.realPlayTimeInteracting, "realPlayTimeInteracting", 0f, false);
			Scribe_Values.Look<bool>(ref this.permadeathMode, "permadeathMode", false, false);
			Scribe_Values.Look<string>(ref this.permadeathModeUniqueName, "permadeathModeUniqueName", null, false);
		}
	}
}
