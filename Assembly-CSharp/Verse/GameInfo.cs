using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BCA RID: 3018
	public sealed class GameInfo : IExposable
	{
		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x060041A9 RID: 16809 RVA: 0x00229DE4 File Offset: 0x002281E4
		public float RealPlayTimeInteracting
		{
			get
			{
				return this.realPlayTimeInteracting;
			}
		}

		// Token: 0x060041AA RID: 16810 RVA: 0x00229DFF File Offset: 0x002281FF
		public void GameInfoOnGUI()
		{
			if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseMove || Event.current.type == EventType.KeyDown)
			{
				this.lastInputRealTime = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x060041AB RID: 16811 RVA: 0x00229E3C File Offset: 0x0022823C
		public void GameInfoUpdate()
		{
			if (Time.realtimeSinceStartup < this.lastInputRealTime + 90f && Find.MainTabsRoot.OpenTab != MainButtonDefOf.Menu && Current.ProgramState == ProgramState.Playing && !Find.WindowStack.IsOpen<Dialog_Options>())
			{
				this.realPlayTimeInteracting += RealTime.realDeltaTime;
			}
		}

		// Token: 0x060041AC RID: 16812 RVA: 0x00229EA2 File Offset: 0x002282A2
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
