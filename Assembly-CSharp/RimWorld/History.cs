using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002FA RID: 762
	public sealed class History : IExposable
	{
		// Token: 0x0400084C RID: 2124
		public Archive archive = new Archive();

		// Token: 0x0400084D RID: 2125
		private List<HistoryAutoRecorderGroup> autoRecorderGroups;

		// Token: 0x0400084E RID: 2126
		public SimpleCurveDrawerStyle curveDrawerStyle;

		// Token: 0x06000CB1 RID: 3249 RVA: 0x0006FD38 File Offset: 0x0006E138
		public History()
		{
			this.autoRecorderGroups = new List<HistoryAutoRecorderGroup>();
			foreach (HistoryAutoRecorderGroupDef def in DefDatabase<HistoryAutoRecorderGroupDef>.AllDefs)
			{
				HistoryAutoRecorderGroup historyAutoRecorderGroup = new HistoryAutoRecorderGroup();
				historyAutoRecorderGroup.def = def;
				historyAutoRecorderGroup.CreateRecorders();
				this.autoRecorderGroups.Add(historyAutoRecorderGroup);
			}
			this.curveDrawerStyle = new SimpleCurveDrawerStyle();
			this.curveDrawerStyle.DrawMeasures = true;
			this.curveDrawerStyle.DrawPoints = false;
			this.curveDrawerStyle.DrawBackground = true;
			this.curveDrawerStyle.DrawBackgroundLines = false;
			this.curveDrawerStyle.DrawLegend = true;
			this.curveDrawerStyle.DrawCurveMousePoint = true;
			this.curveDrawerStyle.OnlyPositiveValues = true;
			this.curveDrawerStyle.UseFixedSection = true;
			this.curveDrawerStyle.UseAntiAliasedLines = true;
			this.curveDrawerStyle.PointsRemoveOptimization = true;
			this.curveDrawerStyle.MeasureLabelsXCount = 10;
			this.curveDrawerStyle.MeasureLabelsYCount = 5;
			this.curveDrawerStyle.XIntegersOnly = true;
			this.curveDrawerStyle.LabelX = "Day".Translate();
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x0006FE88 File Offset: 0x0006E288
		public void HistoryTick()
		{
			for (int i = 0; i < this.autoRecorderGroups.Count; i++)
			{
				this.autoRecorderGroups[i].Tick();
			}
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x0006FEC8 File Offset: 0x0006E2C8
		public List<HistoryAutoRecorderGroup> Groups()
		{
			return this.autoRecorderGroups;
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x0006FEE4 File Offset: 0x0006E2E4
		public void ExposeData()
		{
			Scribe_Deep.Look<Archive>(ref this.archive, "archive", new object[0]);
			Scribe_Collections.Look<HistoryAutoRecorderGroup>(ref this.autoRecorderGroups, "autoRecorderGroups", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				BackCompatibility.HistoryLoadingVars(this);
			}
		}
	}
}
