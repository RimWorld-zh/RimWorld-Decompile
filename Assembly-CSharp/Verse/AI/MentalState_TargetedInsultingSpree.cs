using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A6E RID: 2670
	public class MentalState_TargetedInsultingSpree : MentalState_InsultingSpree
	{
		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x06003B59 RID: 15193 RVA: 0x001F739C File Offset: 0x001F579C
		public override string InspectLine
		{
			get
			{
				return string.Format(this.def.baseInspectLine, this.target.LabelShort);
			}
		}

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x06003B5A RID: 15194 RVA: 0x001F73CC File Offset: 0x001F57CC
		protected override bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return this.insultedTargetAtLeastOnce;
			}
		}

		// Token: 0x06003B5B RID: 15195 RVA: 0x001F73E8 File Offset: 0x001F57E8
		public override void MentalStateTick()
		{
			if (this.target != null && (!this.target.Spawned || !this.pawn.CanReach(this.target, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn)))
			{
				Pawn target = this.target;
				if (!this.TryFindNewTarget())
				{
					base.RecoverFromState();
				}
				else
				{
					Messages.Message("MessageTargetedInsultingSpreeChangedTarget".Translate(new object[]
					{
						this.pawn.LabelShort,
						target.Label,
						this.target.Label
					}).AdjustedFor(this.pawn, "PAWN"), this.pawn, MessageTypeDefOf.NegativeEvent, true);
					base.MentalStateTick();
				}
			}
			else if (this.target == null || !InsultingSpreeMentalStateUtility.CanChaseAndInsult(this.pawn, this.target, false, false))
			{
				base.RecoverFromState();
			}
			else
			{
				base.MentalStateTick();
			}
		}

		// Token: 0x06003B5C RID: 15196 RVA: 0x001F74EB File Offset: 0x001F58EB
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.TryFindNewTarget();
		}

		// Token: 0x06003B5D RID: 15197 RVA: 0x001F74FC File Offset: 0x001F58FC
		private bool TryFindNewTarget()
		{
			InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(this.pawn, MentalState_TargetedInsultingSpree.candidates, false);
			bool result = MentalState_TargetedInsultingSpree.candidates.TryRandomElement(out this.target);
			MentalState_TargetedInsultingSpree.candidates.Clear();
			return result;
		}

		// Token: 0x06003B5E RID: 15198 RVA: 0x001F7540 File Offset: 0x001F5940
		public override void PostEnd()
		{
			base.PostEnd();
			if (this.target != null && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Messages.Message("MessageNoLongerOnTargetedInsultingSpree".Translate(new object[]
				{
					this.pawn.LabelShort,
					this.target.Label
				}), this.pawn, MessageTypeDefOf.SituationResolved, true);
			}
		}

		// Token: 0x06003B5F RID: 15199 RVA: 0x001F75B4 File Offset: 0x001F59B4
		public override string GetBeginLetterText()
		{
			string result;
			if (this.target == null)
			{
				Log.Error("No target. This should have been checked in this mental state's worker.", false);
				result = "";
			}
			else
			{
				result = string.Format(this.def.beginLetter, this.pawn.LabelShort, this.target.LabelShort).AdjustedFor(this.pawn, "PAWN").CapitalizeFirst();
			}
			return result;
		}

		// Token: 0x04002564 RID: 9572
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
