using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200029C RID: 668
	public class HistoryAutoRecorderDef : Def
	{
		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000B3F RID: 2879 RVA: 0x00065C9C File Offset: 0x0006409C
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
		// (get) Token: 0x06000B40 RID: 2880 RVA: 0x00065CD8 File Offset: 0x000640D8
		public string GraphLabelY
		{
			get
			{
				return (this.graphLabelY == null) ? "Value".Translate() : this.graphLabelY;
			}
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x00065D10 File Offset: 0x00064110
		public static HistoryAutoRecorderDef Named(string defName)
		{
			return DefDatabase<HistoryAutoRecorderDef>.GetNamed(defName, true);
		}

		// Token: 0x04000609 RID: 1545
		public Type workerClass;

		// Token: 0x0400060A RID: 1546
		public int recordTicksFrequency = 60000;

		// Token: 0x0400060B RID: 1547
		public Color graphColor = Color.green;

		// Token: 0x0400060C RID: 1548
		[MustTranslate]
		public string graphLabelY;

		// Token: 0x0400060D RID: 1549
		[Unsaved]
		private HistoryAutoRecorderWorker workerInt = null;
	}
}
