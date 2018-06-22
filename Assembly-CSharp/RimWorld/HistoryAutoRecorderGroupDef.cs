using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200029E RID: 670
	public class HistoryAutoRecorderGroupDef : Def
	{
		// Token: 0x06000B43 RID: 2883 RVA: 0x00065DF4 File Offset: 0x000641F4
		public static HistoryAutoRecorderGroupDef Named(string defName)
		{
			return DefDatabase<HistoryAutoRecorderGroupDef>.GetNamed(defName, true);
		}

		// Token: 0x0400060D RID: 1549
		public bool useFixedScale = false;

		// Token: 0x0400060E RID: 1550
		public Vector2 fixedScale = default(Vector2);

		// Token: 0x0400060F RID: 1551
		public bool integersOnly = false;

		// Token: 0x04000610 RID: 1552
		public List<HistoryAutoRecorderDef> historyAutoRecorderDefs = new List<HistoryAutoRecorderDef>();
	}
}
