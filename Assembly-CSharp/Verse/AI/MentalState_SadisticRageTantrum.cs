using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A85 RID: 2693
	public class MentalState_SadisticRageTantrum : MentalState_TantrumRandom
	{
		// Token: 0x06003BBA RID: 15290 RVA: 0x001F841A File Offset: 0x001F681A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.hits, "hits", 0, false);
		}

		// Token: 0x06003BBB RID: 15291 RVA: 0x001F8435 File Offset: 0x001F6835
		protected override void GetPotentialTargets(List<Thing> outThings)
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, outThings, this.GetCustomValidator(), 0, 40);
		}

		// Token: 0x06003BBC RID: 15292 RVA: 0x001F8458 File Offset: 0x001F6858
		protected override Predicate<Thing> GetCustomValidator()
		{
			return (Thing x) => TantrumMentalStateUtility.CanAttackPrisoner(this.pawn, x);
		}

		// Token: 0x06003BBD RID: 15293 RVA: 0x001F847C File Offset: 0x001F687C
		public override void Notify_AttackedTarget(LocalTargetInfo hitTarget)
		{
			base.Notify_AttackedTarget(hitTarget);
			if (this.target != null && hitTarget.Thing == this.target)
			{
				this.hits++;
				if (this.hits >= 7)
				{
					base.RecoverFromState();
				}
			}
		}

		// Token: 0x0400257E RID: 9598
		private int hits;

		// Token: 0x0400257F RID: 9599
		public const int MaxHits = 7;
	}
}
