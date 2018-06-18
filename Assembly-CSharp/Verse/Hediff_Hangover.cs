using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D2A RID: 3370
	public class Hediff_Hangover : HediffWithComps
	{
		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x06004A2B RID: 18987 RVA: 0x0026ABF0 File Offset: 0x00268FF0
		public override bool Visible
		{
			get
			{
				return !this.pawn.health.hediffSet.HasHediff(HediffDefOf.AlcoholHigh, false) && base.Visible;
			}
		}
	}
}
