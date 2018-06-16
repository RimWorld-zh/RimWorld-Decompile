using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A76 RID: 2678
	public class MentalState_MurderousRage : MentalState
	{
		// Token: 0x06003B7D RID: 15229 RVA: 0x001F73E2 File Offset: 0x001F57E2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
		}

		// Token: 0x06003B7E RID: 15230 RVA: 0x001F73FC File Offset: 0x001F57FC
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x06003B7F RID: 15231 RVA: 0x001F7412 File Offset: 0x001F5812
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.TryFindNewTarget();
		}

		// Token: 0x06003B80 RID: 15232 RVA: 0x001F7424 File Offset: 0x001F5824
		public override void MentalStateTick()
		{
			base.MentalStateTick();
			if (this.target != null && this.target.Dead)
			{
				base.RecoverFromState();
			}
			if (this.pawn.IsHashIntervalTick(120) && !this.IsTargetStillValidAndReachable())
			{
				if (!this.TryFindNewTarget())
				{
					base.RecoverFromState();
				}
				else
				{
					Messages.Message("MessageMurderousRageChangedTarget".Translate(new object[]
					{
						this.pawn.LabelShort,
						this.target.Label
					}).AdjustedFor(this.pawn), this.pawn, MessageTypeDefOf.NegativeEvent, true);
					base.MentalStateTick();
				}
			}
		}

		// Token: 0x06003B81 RID: 15233 RVA: 0x001F74E4 File Offset: 0x001F58E4
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

		// Token: 0x06003B82 RID: 15234 RVA: 0x001F7554 File Offset: 0x001F5954
		private bool TryFindNewTarget()
		{
			this.target = MurderousRageMentalStateUtility.FindPawnToKill(this.pawn);
			return this.target != null;
		}

		// Token: 0x06003B83 RID: 15235 RVA: 0x001F7588 File Offset: 0x001F5988
		public bool IsTargetStillValidAndReachable()
		{
			return this.target != null && this.target.SpawnedParentOrMe != null && (!(this.target.SpawnedParentOrMe is Pawn) || this.target.SpawnedParentOrMe == this.target) && this.pawn.CanReach(this.target.SpawnedParentOrMe, PathEndMode.Touch, Danger.Deadly, true, TraverseMode.ByPawn);
		}

		// Token: 0x04002572 RID: 9586
		public Pawn target;

		// Token: 0x04002573 RID: 9587
		private const int NoLongerValidTargetCheckInterval = 120;
	}
}
