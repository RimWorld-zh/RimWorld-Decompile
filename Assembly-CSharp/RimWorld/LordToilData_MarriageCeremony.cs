using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001A2 RID: 418
	public class LordToilData_MarriageCeremony : LordToilData
	{
		// Token: 0x040003A9 RID: 937
		public CellRect spectateRect;

		// Token: 0x040003AA RID: 938
		public SpectateRectSide spectateRectAllowedSides = SpectateRectSide.All;

		// Token: 0x060008AC RID: 2220 RVA: 0x00051EBC File Offset: 0x000502BC
		public override void ExposeData()
		{
			Scribe_Values.Look<CellRect>(ref this.spectateRect, "spectateRect", default(CellRect), false);
			Scribe_Values.Look<SpectateRectSide>(ref this.spectateRectAllowedSides, "spectateRectAllowedSides", SpectateRectSide.None, false);
		}
	}
}
