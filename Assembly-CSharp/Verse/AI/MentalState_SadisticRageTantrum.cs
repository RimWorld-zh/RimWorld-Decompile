using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A83 RID: 2691
	public class MentalState_SadisticRageTantrum : MentalState_TantrumRandom
	{
		// Token: 0x0400257A RID: 9594
		private int hits;

		// Token: 0x0400257B RID: 9595
		public const int MaxHits = 7;

		// Token: 0x06003BB9 RID: 15289 RVA: 0x001F885A File Offset: 0x001F6C5A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.hits, "hits", 0, false);
		}

		// Token: 0x06003BBA RID: 15290 RVA: 0x001F8875 File Offset: 0x001F6C75
		protected override void GetPotentialTargets(List<Thing> outThings)
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, outThings, this.GetCustomValidator(), 0, 40);
		}

		// Token: 0x06003BBB RID: 15291 RVA: 0x001F8898 File Offset: 0x001F6C98
		protected override Predicate<Thing> GetCustomValidator()
		{
			return (Thing x) => TantrumMentalStateUtility.CanAttackPrisoner(this.pawn, x);
		}

		// Token: 0x06003BBC RID: 15292 RVA: 0x001F88BC File Offset: 0x001F6CBC
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
