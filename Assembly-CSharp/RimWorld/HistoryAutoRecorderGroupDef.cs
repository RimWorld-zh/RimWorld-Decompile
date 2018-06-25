using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020002A0 RID: 672
	public class HistoryAutoRecorderGroupDef : Def
	{
		// Token: 0x0400060F RID: 1551
		public bool useFixedScale = false;

		// Token: 0x04000610 RID: 1552
		public Vector2 fixedScale = default(Vector2);

		// Token: 0x04000611 RID: 1553
		public bool integersOnly = false;

		// Token: 0x04000612 RID: 1554
		public List<HistoryAutoRecorderDef> historyAutoRecorderDefs = new List<HistoryAutoRecorderDef>();

		// Token: 0x06000B46 RID: 2886 RVA: 0x00065F40 File Offset: 0x00064340
		public static HistoryAutoRecorderGroupDef Named(string defName)
		{
			return DefDatabase<HistoryAutoRecorderGroupDef>.GetNamed(defName, true);
		}
	}
}
