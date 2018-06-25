using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A70 RID: 2672
	public class MentalState_InsultingSpreeAll : MentalState_InsultingSpree
	{
		// Token: 0x04002571 RID: 9585
		private int targetFoundTicks;

		// Token: 0x04002572 RID: 9586
		private const int CheckChooseNewTargetIntervalTicks = 250;

		// Token: 0x04002573 RID: 9587
		private const int MaxSameTargetChaseTicks = 1250;

		// Token: 0x04002574 RID: 9588
		private static List<Pawn> candidates = new List<Pawn>();

		// Token: 0x06003B53 RID: 15187 RVA: 0x001F757E File Offset: 0x001F597E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.targetFoundTicks, "targetFoundTicks", 0, false);
		}

		// Token: 0x06003B54 RID: 15188 RVA: 0x001F7599 File Offset: 0x001F5999
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.ChooseNextTarget();
		}

		// Token: 0x06003B55 RID: 15189 RVA: 0x001F75AC File Offset: 0x001F59AC
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

		// Token: 0x06003B56 RID: 15190 RVA: 0x001F761C File Offset: 0x001F5A1C
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

		// Token: 0x06003B57 RID: 15191 RVA: 0x001F771C File Offset: 0x001F5B1C
		private float GetCandidateWeight(Pawn candidate)
		{
			float num = this.pawn.Position.DistanceTo(candidate.Position);
			float num2 = Mathf.Min(num / 40f, 1f);
			return 1f - num2 + 0.01f;
		}
	}
}
