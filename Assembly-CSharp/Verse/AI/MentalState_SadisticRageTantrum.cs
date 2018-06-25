using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A84 RID: 2692
	public class MentalState_SadisticRageTantrum : MentalState_TantrumRandom
	{
		// Token: 0x0400258A RID: 9610
		private int hits;

		// Token: 0x0400258B RID: 9611
		public const int MaxHits = 7;

		// Token: 0x06003BBA RID: 15290 RVA: 0x001F8B86 File Offset: 0x001F6F86
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.hits, "hits", 0, false);
		}

		// Token: 0x06003BBB RID: 15291 RVA: 0x001F8BA1 File Offset: 0x001F6FA1
		protected override void GetPotentialTargets(List<Thing> outThings)
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, outThings, this.GetCustomValidator(), 0, 40);
		}

		// Token: 0x06003BBC RID: 15292 RVA: 0x001F8BC4 File Offset: 0x001F6FC4
		protected override Predicate<Thing> GetCustomValidator()
		{
			return (Thing x) => TantrumMentalStateUtility.CanAttackPrisoner(this.pawn, x);
		}

		// Token: 0x06003BBD RID: 15293 RVA: 0x001F8BE8 File Offset: 0x001F6FE8
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
	}
}
