using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D2B RID: 3371
	public class Hediff_Hangover : HediffWithComps
	{
		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x06004A2D RID: 18989 RVA: 0x0026AC18 File Offset: 0x00269018
		public override bool Visible
		{
			get
			{
				return !this.pawn.health.hediffSet.HasHediff(HediffDefOf.AlcoholHigh, false) && base.Visible;
			}
		}
	}
}
