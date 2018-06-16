using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D2A RID: 3370
	public class Hediff_Alcohol : HediffWithComps
	{
		// Token: 0x06004A2A RID: 18986 RVA: 0x0026AB40 File Offset: 0x00268F40
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

		// Token: 0x06004A2B RID: 18987 RVA: 0x0026ABF8 File Offset: 0x00268FF8
		private bool HangoverSusceptible(Pawn pawn)
		{
			return true;
		}

		// Token: 0x04003230 RID: 12848
		private const int HangoverCheckInterval = 300;
	}
}
