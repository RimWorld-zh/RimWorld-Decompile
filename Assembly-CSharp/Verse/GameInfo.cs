using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BC6 RID: 3014
	public sealed class GameInfo : IExposable
	{
		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x060041AB RID: 16811 RVA: 0x0022A4B8 File Offset: 0x002288B8
		public float RealPlayTimeInteracting
		{
			get
			{
				return this.realPlayTimeInteracting;
			}
		}

		// Token: 0x060041AC RID: 16812 RVA: 0x0022A4D3 File Offset: 0x002288D3
		public void GameInfoOnGUI()
		{
			if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseMove || Event.current.type == EventType.KeyDown)
			{
				this.lastInputRealTime = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x060041AD RID: 16813 RVA: 0x0022A510 File Offset: 0x00228910
		public void GameInfoUpdate()
		{
			if (Time.realtimeSinceStartup < this.lastInputRealTime + 90f && Find.MainTabsRoot.OpenTab != MainButtonDefOf.Menu && Current.ProgramState == ProgramState.Playing && !Find.WindowStack.IsOpen<Dialog_Options>())
			{
				this.realPlayTimeInteracting += RealTime.realDeltaTime;
			}
		}

		// Token: 0x060041AE RID: 16814 RVA: 0x0022A576 File Offset: 0x00228976
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.realPlayTimeInteracting, "realPlayTimeInteracting", 0f, false);
			Scribe_Values.Look<bool>(ref this.permadeathMode, "permadeathMode", false, false);
			Scribe_Values.Look<string>(ref this.permadeathModeUniqueName, "permadeathModeUniqueName", null, false);
		}

		// Token: 0x04002CD6 RID: 11478
		public bool permadeathMode = false;

		// Token: 0x04002CD7 RID: 11479
		public string permadeathModeUniqueName = null;

		// Token: 0x04002CD8 RID: 11480
		private float realPlayTimeInteracting = 0f;

		// Token: 0x04002CD9 RID: 11481
		private float lastInputRealTime = 0f;
	}
}
