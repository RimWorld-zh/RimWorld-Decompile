using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D28 RID: 3368
	public class Hediff_Alcohol : HediffWithComps
	{
		// Token: 0x04003239 RID: 12857
		private const int HangoverCheckInterval = 300;

		// Token: 0x06004A3E RID: 19006 RVA: 0x0026C0D0 File Offset: 0x0026A4D0
		public override void Tick()
		{
			base.Tick();
			if (this.CurStageIndex >= 3)
			{
				if (this.pawn.IsHashIntervalTick(300) && this.HangoverSusceptible(this.pawn))
				{
					Hediff hediff = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hangover, false);
					if (hediff != null)
					{
						hediff.Severity = 1f;
					}
					else
					{
						hediff = HediffMaker.MakeHediff(HediffDefOf.Hangover, this.pawn, null);
						hediff.Severity = 1f;
						this.pawn.health.AddHediff(hediff, null, null, null);
					}
				}
			}
		}

		// Token: 0x06004A3F RID: 19007 RVA: 0x0026C188 File Offset: 0x0026A588
		private bool HangoverSusceptible(Pawn pawn)
		{
			return true;
		}
	}
}
