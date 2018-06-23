using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A84 RID: 2692
	public abstract class MentalState_TantrumRandom : MentalState_Tantrum
	{
		// Token: 0x0400257D RID: 9597
		private int targetFoundTicks;

		// Token: 0x0400257E RID: 9598
		private const int CheckChooseNewTargetIntervalTicks = 500;

		// Token: 0x0400257F RID: 9599
		private const int MaxSameTargetAttackTicks = 1250;

		// Token: 0x04002580 RID: 9600
		private static List<Thing> candidates = new List<Thing>();

		// Token: 0x06003BC1 RID: 15297
		protected abstract void GetPotentialTargets(List<Thing> outThings);

		// Token: 0x06003BC2 RID: 15298 RVA: 0x001F83C0 File Offset: 0x001F67C0
		protected virtual Predicate<Thing> GetCustomValidator()
		{
			return null;
		}

		// Token: 0x06003BC3 RID: 15299 RVA: 0x001F83D6 File Offset: 0x001F67D6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.targetFoundTicks, "targetFoundTicks", 0, false);
		}

		// Token: 0x06003BC4 RID: 15300 RVA: 0x001F83F1 File Offset: 0x001F67F1
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.ChooseNextTarget();
		}

		// Token: 0x06003BC5 RID: 15301 RVA: 0x001F8404 File Offset: 0x001F6804
		public override void MentalStateTick()
		{
			if (this.target != null && (!this.target.Spawned || !this.pawn.CanReach(this.target, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn) || (this.target is Pawn && ((Pawn)this.target).Downed)))
			{
				this.ChooseNextTarget();
			}
			if (this.pawn.IsHashIntervalTick(500) && (this.target == null || this.hitTargetAtLeastOnce))
			{
				this.ChooseNextTarget();
			}
			base.MentalStateTick();
		}

		// Token: 0x06003BC6 RID: 15302 RVA: 0x001F84B4 File Offset: 0x001F68B4
		private void ChooseNextTarget()
		{
			MentalState_TantrumRandom.candidates.Clear();
			this.GetPotentialTargets(MentalState_TantrumRandom.candidates);
			if (!MentalState_TantrumRandom.candidates.Any<Thing>())
			{
				this.target = null;
				this.hitTargetAtLeastOnce = false;
				this.targetFoundTicks = -1;
			}
			else
			{
				Thing thing;
				if (this.target != null && Find.TickManager.TicksGame - this.targetFoundTicks > 1250 && MentalState_TantrumRandom.candidates.Any((Thing x) => x != this.target))
				{
					thing = (from x in MentalState_TantrumRandom.candidates
					where x != this.target
					select x).RandomElementByWeight((Thing x) => this.GetCandidateWeight(x));
				}
				else
				{
					thing = MentalState_TantrumRandom.candidates.RandomElementByWeight((Thing x) => this.GetCandidateWeight(x));
				}
				if (thing != this.target)
				{
					this.target = thing;
					this.hitTargetAtLeastOnce = false;
					this.targetFoundTicks = Find.TickManager.TicksGame;
				}
			}
			MentalState_TantrumRandom.candidates.Clear();
		}

		// Token: 0x06003BC7 RID: 15303 RVA: 0x001F85C0 File Offset: 0x001F69C0
		private float GetCandidateWeight(Thing candidate)
		{
			float num = this.pawn.Position.DistanceTo(candidate.Position);
			float num2 = Mathf.Min(num / 40f, 1f);
			return (1f - num2) * (1f - num2) + 0.01f;
		}
	}
}
