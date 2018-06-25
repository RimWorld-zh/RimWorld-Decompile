using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse.AI
{
	public class MentalState_InsultingSpreeAll : MentalState_InsultingSpree
	{
		private int targetFoundTicks;

		private const int CheckChooseNewTargetIntervalTicks = 250;

		private const int MaxSameTargetChaseTicks = 1250;

		private static List<Pawn> candidates = new List<Pawn>();

		public MentalState_InsultingSpreeAll()
		{
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
			if (this.target != null && !InsultingSpreeMentalStateUtility.CanChaseAndInsult(this.pawn, this.target, false, true))
			{
				this.ChooseNextTarget();
			}
			if (this.pawn.IsHashIntervalTick(250) && (this.target == null || this.insultedTargetAtLeastOnce))
			{
				this.ChooseNextTarget();
			}
			base.MentalStateTick();
		}

		private void ChooseNextTarget()
		{
			InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(this.pawn, MentalState_InsultingSpreeAll.candidates, true);
			if (!MentalState_InsultingSpreeAll.candidates.Any<Pawn>())
			{
				this.target = null;
				this.insultedTargetAtLeastOnce = false;
				this.targetFoundTicks = -1;
			}
			else
			{
				Pawn pawn;
				if (this.target != null && Find.TickManager.TicksGame - this.targetFoundTicks > 1250 && MentalState_InsultingSpreeAll.candidates.Any((Pawn x) => x != this.target))
				{
					pawn = (from x in MentalState_InsultingSpreeAll.candidates
					where x != this.target
					select x).RandomElementByWeight((Pawn x) => this.GetCandidateWeight(x));
				}
				else
				{
					pawn = MentalState_InsultingSpreeAll.candidates.RandomElementByWeight((Pawn x) => this.GetCandidateWeight(x));
				}
				if (pawn != this.target)
				{
					this.target = pawn;
					this.insultedTargetAtLeastOnce = false;
					this.targetFoundTicks = Find.TickManager.TicksGame;
				}
			}
		}

		private float GetCandidateWeight(Pawn candidate)
		{
			float num = this.pawn.Position.DistanceTo(candidate.Position);
			float num2 = Mathf.Min(num / 40f, 1f);
			return 1f - num2 + 0.01f;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static MentalState_InsultingSpreeAll()
		{
		}

		[CompilerGenerated]
		private bool <ChooseNextTarget>m__0(Pawn x)
		{
			return x != this.target;
		}

		[CompilerGenerated]
		private bool <ChooseNextTarget>m__1(Pawn x)
		{
			return x != this.target;
		}

		[CompilerGenerated]
		private float <ChooseNextTarget>m__2(Pawn x)
		{
			return this.GetCandidateWeight(x);
		}

		[CompilerGenerated]
		private float <ChooseNextTarget>m__3(Pawn x)
		{
			return this.GetCandidateWeight(x);
		}
	}
}
