using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D2A RID: 3370
	public class Hediff_Hangover : HediffWithComps
	{
		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x06004A41 RID: 19009 RVA: 0x0026C488 File Offset: 0x0026A888
		public override bool Visible
		{
			get
			{
				return !this.pawn.health.hediffSet.HasHediff(HediffDefOf.AlcoholHigh, false) && base.Visible;
			}
		}
	}
}
