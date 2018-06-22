using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A81 RID: 2689
	public class MentalState_SadisticRageTantrum : MentalState_TantrumRandom
	{
		// Token: 0x06003BB5 RID: 15285 RVA: 0x001F872E File Offset: 0x001F6B2E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.hits, "hits", 0, false);
		}

		// Token: 0x06003BB6 RID: 15286 RVA: 0x001F8749 File Offset: 0x001F6B49
		protected override void GetPotentialTargets(List<Thing> outThings)
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, outThings, this.GetCustomValidator(), 0, 40);
		}

		// Token: 0x06003BB7 RID: 15287 RVA: 0x001F876C File Offset: 0x001F6B6C
		protected override Predicate<Thing> GetCustomValidator()
		{
			return (Thing x) => TantrumMentalStateUtility.CanAttackPrisoner(this.pawn, x);
		}

		// Token: 0x06003BB8 RID: 15288 RVA: 0x001F8790 File Offset: 0x001F6B90
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

		// Token: 0x04002579 RID: 9593
		private int hits;

		// Token: 0x0400257A RID: 9594
		public const int MaxHits = 7;
	}
}
