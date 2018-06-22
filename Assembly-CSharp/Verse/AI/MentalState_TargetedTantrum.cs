using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A85 RID: 2693
	public class MentalState_TargetedTantrum : MentalState_Tantrum
	{
		// Token: 0x06003BCE RID: 15310 RVA: 0x001F8838 File Offset: 0x001F6C38
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

		// Token: 0x06003BCF RID: 15311 RVA: 0x001F8928 File Offset: 0x001F6D28
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.TryFindNewTarget();
		}

		// Token: 0x06003BD0 RID: 15312 RVA: 0x001F893C File Offset: 0x001F6D3C
		private bool TryFindNewTarget()
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, MentalState_TargetedTantrum.tmpThings, null, 300, 40);
			bool result = MentalState_TargetedTantrum.tmpThings.TryRandomElementByWeight((Thing x) => x.MarketValue * (float)x.stackCount, out this.target);
			MentalState_TargetedTantrum.tmpThings.Clear();
			return result;
		}

		// Token: 0x06003BD1 RID: 15313 RVA: 0x001F89B0 File Offset: 0x001F6DB0
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

		// Token: 0x04002581 RID: 9601
		public const int MinMarketValue = 300;

		// Token: 0x04002582 RID: 9602
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
