using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002F8 RID: 760
	public sealed class History : IExposable
	{
		// Token: 0x04000849 RID: 2121
		public Archive archive = new Archive();

		// Token: 0x0400084A RID: 2122
		private List<HistoryAutoRecorderGroup> autoRecorderGroups;

		// Token: 0x0400084B RID: 2123
		public SimpleCurveDrawerStyle curveDrawerStyle;

		// Token: 0x06000CAE RID: 3246 RVA: 0x0006FBE0 File Offset: 0x0006DFE0
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

		// Token: 0x06000CAF RID: 3247 RVA: 0x0006FD30 File Offset: 0x0006E130
		public void HistoryTick()
		{
			for (int i = 0; i < this.autoRecorderGroups.Count; i++)
			{
				this.autoRecorderGroups[i].Tick();
			}
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x0006FD70 File Offset: 0x0006E170
		public List<HistoryAutoRecorderGroup> Groups()
		{
			return this.autoRecorderGroups;
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x0006FD8C File Offset: 0x0006E18C
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
