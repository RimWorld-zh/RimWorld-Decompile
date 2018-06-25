using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200029E RID: 670
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
		// (get) Token: 0x06000B41 RID: 2881 RVA: 0x00065E54 File Offset: 0x00064254
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
		// (get) Token: 0x06000B42 RID: 2882 RVA: 0x00065E90 File Offset: 0x00064290
		public string GraphLabelY
		{
			get
			{
				return (this.graphLabelY == null) ? "Value".Translate() : this.graphLabelY;
			}
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x00065EC8 File Offset: 0x000642C8
		public static HistoryAutoRecorderDef Named(string defName)
		{
			return DefDatabase<HistoryAutoRecorderDef>.GetNamed(defName, true);
		}
	}
}
