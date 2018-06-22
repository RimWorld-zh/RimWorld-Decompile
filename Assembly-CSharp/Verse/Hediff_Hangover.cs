using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D27 RID: 3367
	public class Hediff_Hangover : HediffWithComps
	{
		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x06004A3D RID: 19005 RVA: 0x0026C07C File Offset: 0x0026A47C
		public override bool Visible
		{
			get
			{
				return !this.pawn.health.hediffSet.HasHediff(HediffDefOf.AlcoholHigh, false) && base.Visible;
			}
		}
	}
}
