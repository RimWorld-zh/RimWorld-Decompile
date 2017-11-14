using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.AI
{
	public class MentalState_InsultingSpreeAll : MentalState_InsultingSpree
	{
		private int targetFoundTicks;

		private const int CheckChooseNewTargetIntervalTicks = 250;

		private const int MaxSameTargetChaseTicks = 1250;

		private static List<Pawn> candidates = new List<Pawn>();

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.targetFoundTicks, "targetFoundTicks", 0, false);
		}

		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.ChooseNextTarget();
		}

		public override void MentalStateTick()
		{
			if (base.target != null && !InsultingSpreeMentalStateUtility.CanChaseAndInsult(base.pawn, base.target, false, true))
			{
				this.ChooseNextTarget();
			}
			if (base.pawn.IsHashIntervalTick(250) && (base.target == null || base.insultedTargetAtLeastOnce))
			{
				this.ChooseNextTarget();
			}
			base.MentalStateTick();
		}

		private void ChooseNextTarget()
		{
			InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(base.pawn, MentalState_InsultingSpreeAll.candidates, true);
			if (!MentalState_InsultingSpreeAll.candidates.Any())
			{
				base.target = null;
				base.insultedTargetAtLeastOnce = false;
				this.targetFoundTicks = -1;
			}
			else
			{
				Pawn pawn = (base.target == null || Find.TickManager.TicksGame - this.targetFoundTicks <= 1250 || !MentalState_InsultingSpreeAll.candidates.Any((Pawn x) => x != base.target)) ? MentalState_InsultingSpreeAll.candidates.RandomElementByWeight((Pawn x) => this.GetCandidateWeight(x)) : (from x in MentalState_InsultingSpreeAll.candidates
				where x != base.target
				select x).RandomElementByWeight((Pawn x) => this.GetCandidateWeight(x));
				if (pawn != base.target)
				{
					base.target = pawn;
					base.insultedTargetAtLeastOnce = false;
					this.targetFoundTicks = Find.TickManager.TicksGame;
				}
			}
		}

		private float GetCandidateWeight(Pawn candidate)
		{
			float num = base.pawn.Position.DistanceTo(candidate.Position);
			float num2 = Mathf.Min((float)(num / 40.0), 1f);
			return (float)(1.0 - num2 + 0.0099999997764825821);
		}
	}
}
