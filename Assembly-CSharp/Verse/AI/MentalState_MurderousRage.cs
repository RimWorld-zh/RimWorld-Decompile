using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A72 RID: 2674
	public class MentalState_MurderousRage : MentalState
	{
		// Token: 0x0400256D RID: 9581
		public Pawn target;

		// Token: 0x0400256E RID: 9582
		private const int NoLongerValidTargetCheckInterval = 120;

		// Token: 0x06003B7A RID: 15226 RVA: 0x001F77C2 File Offset: 0x001F5BC2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
		}

		// Token: 0x06003B7B RID: 15227 RVA: 0x001F77DC File Offset: 0x001F5BDC
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x06003B7C RID: 15228 RVA: 0x001F77F2 File Offset: 0x001F5BF2
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.TryFindNewTarget();
		}

		// Token: 0x06003B7D RID: 15229 RVA: 0x001F7804 File Offset: 0x001F5C04
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

		// Token: 0x06003B7E RID: 15230 RVA: 0x001F78C8 File Offset: 0x001F5CC8
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

		// Token: 0x06003B7F RID: 15231 RVA: 0x001F793C File Offset: 0x001F5D3C
		private bool TryFindNewTarget()
		{
			this.target = MurderousRageMentalStateUtility.FindPawnToKill(this.pawn);
			return this.target != null;
		}

		// Token: 0x06003B80 RID: 15232 RVA: 0x001F7970 File Offset: 0x001F5D70
		public bool IsTargetStillValidAndReachable()
		{
			return this.target != null && this.target.SpawnedParentOrMe != null && (!(this.target.SpawnedParentOrMe is Pawn) || this.target.SpawnedParentOrMe == this.target) && this.pawn.CanReach(this.target.SpawnedParentOrMe, PathEndMode.Touch, Danger.Deadly, true, TraverseMode.ByPawn);
		}
	}
}
