using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200029E RID: 670
	public class HistoryAutoRecorderGroupDef : Def
	{
		// Token: 0x06000B45 RID: 2885 RVA: 0x00065D8C File Offset: 0x0006418C
		public static HistoryAutoRecorderGroupDef Named(string defName)
		{
			return DefDatabase<HistoryAutoRecorderGroupDef>.GetNamed(defName, true);
		}

		// Token: 0x0400060E RID: 1550
		public bool useFixedScale = false;

		// Token: 0x0400060F RID: 1551
		public Vector2 fixedScale = default(Vector2);

		// Token: 0x04000610 RID: 1552
		public bool integersOnly = false;

		// Token: 0x04000611 RID: 1553
		public List<HistoryAutoRecorderDef> historyAutoRecorderDefs = new List<HistoryAutoRecorderDef>();
	}
}
