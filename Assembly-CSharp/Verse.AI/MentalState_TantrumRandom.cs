using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.AI
{
	public abstract class MentalState_TantrumRandom : MentalState_Tantrum
	{
		private int targetFoundTicks;

		private const int CheckChooseNewTargetIntervalTicks = 500;

		private const int MaxSameTargetAttackTicks = 1250;

		private static List<Thing> candidates = new List<Thing>();

		protected abstract void GetPotentialTargets(List<Thing> outThings);

		protected virtual Predicate<Thing> GetCustomValidator()
		{
			return null;
		}

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
			if (base.target != null && (!base.target.Spawned || !base.pawn.CanReach(base.target, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn) || (base.target is Pawn && ((Pawn)base.target).Downed)))
			{
				this.ChooseNextTarget();
			}
			if (base.pawn.IsHashIntervalTick(500) && (base.target == null || base.hitTargetAtLeastOnce))
			{
				this.ChooseNextTarget();
			}
			base.MentalStateTick();
		}

		private void ChooseNextTarget()
		{
			MentalState_TantrumRandom.candidates.Clear();
			this.GetPotentialTargets(MentalState_TantrumRandom.candidates);
			if (!MentalState_TantrumRandom.candidates.Any())
			{
				base.target = null;
				base.hitTargetAtLeastOnce = false;
				this.targetFoundTicks = -1;
			}
			else
			{
				Thing thing = (base.target == null || Find.TickManager.TicksGame - this.targetFoundTicks <= 1250 || !MentalState_TantrumRandom.candidates.Any((Predicate<Thing>)((Thing x) => x != base.target))) ? MentalState_TantrumRandom.candidates.RandomElementByWeight((Func<Thing, float>)((Thing x) => this.GetCandidateWeight(x))) : (from x in MentalState_TantrumRandom.candidates
				where x != base.target
				select x).RandomElementByWeight((Func<Thing, float>)((Thing x) => this.GetCandidateWeight(x)));
				if (thing != base.target)
				{
					base.target = thing;
					base.hitTargetAtLeastOnce = false;
					this.targetFoundTicks = Find.TickManager.TicksGame;
				}
			}
			MentalState_TantrumRandom.candidates.Clear();
		}

		private float GetCandidateWeight(Thing candidate)
		{
			float num = base.pawn.Position.DistanceTo(candidate.Position);
			float num2 = Mathf.Min((float)(num / 40.0), 1f);
			return (float)((1.0 - num2) * (1.0 - num2) + 0.0099999997764825821);
		}
	}
}
