using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A85 RID: 2693
	public class MentalState_SadisticRageTantrum : MentalState_TantrumRandom
	{
		// Token: 0x06003BB8 RID: 15288 RVA: 0x001F8346 File Offset: 0x001F6746
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.hits, "hits", 0, false);
		}

		// Token: 0x06003BB9 RID: 15289 RVA: 0x001F8361 File Offset: 0x001F6761
		protected override void GetPotentialTargets(List<Thing> outThings)
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, outThings, this.GetCustomValidator(), 0, 40);
		}

		// Token: 0x06003BBA RID: 15290 RVA: 0x001F8384 File Offset: 0x001F6784
		protected override Predicate<Thing> GetCustomValidator()
		{
			return (Thing x) => TantrumMentalStateUtility.CanAttackPrisoner(this.pawn, x);
		}

		// Token: 0x06003BBB RID: 15291 RVA: 0x001F83A8 File Offset: 0x001F67A8
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
