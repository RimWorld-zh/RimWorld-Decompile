using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A72 RID: 2674
	public class MentalState_TargetedInsultingSpree : MentalState_InsultingSpree
	{
		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x06003B5C RID: 15196 RVA: 0x001F6FC4 File Offset: 0x001F53C4
		public override string InspectLine
		{
			get
			{
				return string.Format(this.def.baseInspectLine, this.target.LabelShort);
			}
		}

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x06003B5D RID: 15197 RVA: 0x001F6FF4 File Offset: 0x001F53F4
		protected override bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return this.insultedTargetAtLeastOnce;
			}
		}

		// Token: 0x06003B5E RID: 15198 RVA: 0x001F7010 File Offset: 0x001F5410
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
					}).AdjustedFor(this.pawn), this.pawn, MessageTypeDefOf.NegativeEvent, true);
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

		// Token: 0x06003B5F RID: 15199 RVA: 0x001F710E File Offset: 0x001F550E
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.TryFindNewTarget();
		}

		// Token: 0x06003B60 RID: 15200 RVA: 0x001F7120 File Offset: 0x001F5520
		private bool TryFindNewTarget()
		{
			InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(this.pawn, MentalState_TargetedInsultingSpree.candidates, false);
			bool result = MentalState_TargetedInsultingSpree.candidates.TryRandomElement(out this.target);
			MentalState_TargetedInsultingSpree.candidates.Clear();
			return result;
		}

		// Token: 0x06003B61 RID: 15201 RVA: 0x001F7164 File Offset: 0x001F5564
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

		// Token: 0x06003B62 RID: 15202 RVA: 0x001F71D8 File Offset: 0x001F55D8
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
				result = string.Format(this.def.beginLetter, this.pawn.LabelShort, this.target.LabelShort).AdjustedFor(this.pawn).CapitalizeFirst();
			}
			return result;
		}

		// Token: 0x04002569 RID: 9577
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
