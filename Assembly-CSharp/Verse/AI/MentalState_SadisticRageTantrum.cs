using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Verse.AI
{
	public class MentalState_SadisticRageTantrum : MentalState_TantrumRandom
	{
		private int hits;

		public const int MaxHits = 7;

		public MentalState_SadisticRageTantrum()
		{
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.hits, "hits", 0, false);
		}

		protected override void GetPotentialTargets(List<Thing> outThings)
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, outThings, this.GetCustomValidator(), 0, 40);
		}

		protected override Predicate<Thing> GetCustomValidator()
		{
			return (Thing x) => TantrumMentalStateUtility.CanAttackPrisoner(this.pawn, x);
		}

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

		[CompilerGenerated]
		private bool <GetCustomValidator>m__0(Thing x)
		{
			return TantrumMentalStateUtility.CanAttackPrisoner(this.pawn, x);
		}
	}
}
