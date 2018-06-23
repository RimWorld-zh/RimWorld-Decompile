using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200029C RID: 668
	public class HistoryAutoRecorderDef : Def
	{
		// Token: 0x04000608 RID: 1544
		public Type workerClass;

		// Token: 0x04000609 RID: 1545
		public int recordTicksFrequency = 60000;

		// Token: 0x0400060A RID: 1546
		public Color graphColor = Color.green;

		// Token: 0x0400060B RID: 1547
		[MustTranslate]
		public string graphLabelY;

		// Token: 0x0400060C RID: 1548
		[Unsaved]
		private HistoryAutoRecorderWorker workerInt = null;

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000B3D RID: 2877 RVA: 0x00065D04 File Offset: 0x00064104
		public HistoryAutoRecorderWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (HistoryAutoRecorderWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000B3E RID: 2878 RVA: 0x00065D40 File Offset: 0x00064140
		public string GraphLabelY
		{
			get
			{
				return (this.graphLabelY == null) ? "Value".Translate() : this.graphLabelY;
			}
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00065D78 File Offset: 0x00064178
		public static HistoryAutoRecorderDef Named(string defName)
		{
			return DefDatabase<HistoryAutoRecorderDef>.GetNamed(defName, true);
		}
	}
}
