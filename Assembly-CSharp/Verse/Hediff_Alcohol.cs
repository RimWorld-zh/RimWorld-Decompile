using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D29 RID: 3369
	public class Hediff_Alcohol : HediffWithComps
	{
		// Token: 0x06004A28 RID: 18984 RVA: 0x0026AB18 File Offset: 0x00268F18
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

		// Token: 0x06004A29 RID: 18985 RVA: 0x0026ABD0 File Offset: 0x00268FD0
		private bool HangoverSusceptible(Pawn pawn)
		{
			return true;
		}

		// Token: 0x0400322E RID: 12846
		private const int HangoverCheckInterval = 300;
	}
}
