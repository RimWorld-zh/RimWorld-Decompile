using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A88 RID: 2696
	public class MentalState_TargetedTantrum : MentalState_Tantrum
	{
		// Token: 0x04002592 RID: 9618
		public const int MinMarketValue = 300;

		// Token: 0x04002593 RID: 9619
		private static List<Thing> tmpThings = new List<Thing>();

		// Token: 0x06003BD3 RID: 15315 RVA: 0x001F8C90 File Offset: 0x001F7090
		public override void MentalStateTick()
		{
			if (this.target == null || this.target.Destroyed)
			{
				base.RecoverFromState();
			}
			else if (!this.target.Spawned || !this.pawn.CanReach(this.target, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				Thing target = this.target;
				if (!this.TryFindNewTarget())
				{
					base.RecoverFromState();
				}
				else
				{
					Messages.Message("MessageTargetedTantrumChangedTarget".Translate(new object[]
					{
						this.pawn.LabelShort,
						target.Label,
						this.target.Label
					}).AdjustedFor(this.pawn, "PAWN"), this.pawn, MessageTypeDefOf.NegativeEvent, true);
					base.MentalStateTick();
				}
			}
			else
			{
				base.MentalStateTick();
			}
		}

		// Token: 0x06003BD4 RID: 15316 RVA: 0x001F8D80 File Offset: 0x001F7180
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.TryFindNewTarget();
		}

		// Token: 0x06003BD5 RID: 15317 RVA: 0x001F8D94 File Offset: 0x001F7194
		private bool TryFindNewTarget()
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, MentalState_TargetedTantrum.tmpThings, null, 300, 40);
			bool result = MentalState_TargetedTantrum.tmpThings.TryRandomElementByWeight((Thing x) => x.MarketValue * (float)x.stackCount, out this.target);
			MentalState_TargetedTantrum.tmpThings.Clear();
			return result;
		}

		// Token: 0x06003BD6 RID: 15318 RVA: 0x001F8E08 File Offset: 0x001F7208
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
				result = string.Format(this.def.beginLetter, this.pawn.LabelShort, this.target.Label).AdjustedFor(this.pawn, "PAWN").CapitalizeFirst();
			}
			return result;
		}
	}
}
