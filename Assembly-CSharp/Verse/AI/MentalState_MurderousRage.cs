using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A74 RID: 2676
	public class MentalState_MurderousRage : MentalState
	{
		// Token: 0x0400256E RID: 9582
		public Pawn target;

		// Token: 0x0400256F RID: 9583
		private const int NoLongerValidTargetCheckInterval = 120;

		// Token: 0x06003B7E RID: 15230 RVA: 0x001F78EE File Offset: 0x001F5CEE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
		}

		// Token: 0x06003B7F RID: 15231 RVA: 0x001F7908 File Offset: 0x001F5D08
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x06003B80 RID: 15232 RVA: 0x001F791E File Offset: 0x001F5D1E
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.TryFindNewTarget();
		}

		// Token: 0x06003B81 RID: 15233 RVA: 0x001F7930 File Offset: 0x001F5D30
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
					}).AdjustedFor(this.pawn, "PAWN"), this.pawn, MessageTypeDefOf.NegativeEvent, true);
					base.MentalStateTick();
				}
			}
		}

		// Token: 0x06003B82 RID: 15234 RVA: 0x001F79F4 File Offset: 0x001F5DF4
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

		// Token: 0x06003B83 RID: 15235 RVA: 0x001F7A68 File Offset: 0x001F5E68
		private bool TryFindNewTarget()
		{
			this.target = MurderousRageMentalStateUtility.FindPawnToKill(this.pawn);
			return this.target != null;
		}

		// Token: 0x06003B84 RID: 15236 RVA: 0x001F7A9C File Offset: 0x001F5E9C
		public bool IsTargetStillValidAndReachable()
		{
			return this.target != null && this.target.SpawnedParentOrMe != null && (!(this.target.SpawnedParentOrMe is Pawn) || this.target.SpawnedParentOrMe == this.target) && this.pawn.CanReach(this.target.SpawnedParentOrMe, PathEndMode.Touch, Danger.Deadly, true, TraverseMode.ByPawn);
		}
	}
}
