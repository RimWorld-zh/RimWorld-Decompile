using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A87 RID: 2695
	public class MentalState_TargetedTantrum : MentalState_Tantrum
	{
		// Token: 0x04002582 RID: 9602
		public const int MinMarketValue = 300;

		// Token: 0x04002583 RID: 9603
		private static List<Thing> tmpThings = new List<Thing>();

		// Token: 0x06003BD2 RID: 15314 RVA: 0x001F8964 File Offset: 0x001F6D64
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

		// Token: 0x06003BD3 RID: 15315 RVA: 0x001F8A54 File Offset: 0x001F6E54
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.TryFindNewTarget();
		}

		// Token: 0x06003BD4 RID: 15316 RVA: 0x001F8A68 File Offset: 0x001F6E68
		private bool TryFindNewTarget()
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, MentalState_TargetedTantrum.tmpThings, null, 300, 40);
			bool result = MentalState_TargetedTantrum.tmpThings.TryRandomElementByWeight((Thing x) => x.MarketValue * (float)x.stackCount, out this.target);
			MentalState_TargetedTantrum.tmpThings.Clear();
			return result;
		}

		// Token: 0x06003BD5 RID: 15317 RVA: 0x001F8ADC File Offset: 0x001F6EDC
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
