using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001A2 RID: 418
	public class LordToilData_MarriageCeremony : LordToilData
	{
		// Token: 0x040003A8 RID: 936
		public CellRect spectateRect;

		// Token: 0x040003A9 RID: 937
		public SpectateRectSide spectateRectAllowedSides = SpectateRectSide.All;

		// Token: 0x060008AD RID: 2221 RVA: 0x00051EC0 File Offset: 0x000502C0
		public override void ExposeData()
		{
			Scribe_Values.Look<CellRect>(ref this.spectateRect, "spectateRect", default(CellRect), false);
			Scribe_Values.Look<SpectateRectSide>(ref this.spectateRectAllowedSides, "spectateRectAllowedSides", SpectateRectSide.None, false);
		}
	}
}
