using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A87 RID: 2695
	public abstract class MentalState_TantrumRandom : MentalState_Tantrum
	{
		// Token: 0x0400258E RID: 9614
		private int targetFoundTicks;

		// Token: 0x0400258F RID: 9615
		private const int CheckChooseNewTargetIntervalTicks = 500;

		// Token: 0x04002590 RID: 9616
		private const int MaxSameTargetAttackTicks = 1250;

		// Token: 0x04002591 RID: 9617
		private static List<Thing> candidates = new List<Thing>();

		// Token: 0x06003BC6 RID: 15302
		protected abstract void GetPotentialTargets(List<Thing> outThings);

		// Token: 0x06003BC7 RID: 15303 RVA: 0x001F8818 File Offset: 0x001F6C18
		protected virtual Predicate<Thing> GetCustomValidator()
		{
			return null;
		}

		// Token: 0x06003BC8 RID: 15304 RVA: 0x001F882E File Offset: 0x001F6C2E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.targetFoundTicks, "targetFoundTicks", 0, false);
		}

		// Token: 0x06003BC9 RID: 15305 RVA: 0x001F8849 File Offset: 0x001F6C49
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.ChooseNextTarget();
		}

		// Token: 0x06003BCA RID: 15306 RVA: 0x001F885C File Offset: 0x001F6C5C
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

		// Token: 0x06003BCB RID: 15307 RVA: 0x001F890C File Offset: 0x001F6D0C
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

		// Token: 0x06003BCC RID: 15308 RVA: 0x001F8A18 File Offset: 0x001F6E18
		private float GetCandidateWeight(Thing candidate)
		{
			float num = this.pawn.Position.DistanceTo(candidate.Position);
			float num2 = Mathf.Min(num / 40f, 1f);
			return (1f - num2) * (1f - num2) + 0.01f;
		}
	}
}
