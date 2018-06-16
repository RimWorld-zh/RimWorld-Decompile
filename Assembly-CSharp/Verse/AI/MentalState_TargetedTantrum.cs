using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A89 RID: 2697
	public class MentalState_TargetedTantrum : MentalState_Tantrum
	{
		// Token: 0x06003BD1 RID: 15313 RVA: 0x001F8450 File Offset: 0x001F6850
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
					}).AdjustedFor(this.pawn), this.pawn, MessageTypeDefOf.NegativeEvent, true);
					base.MentalStateTick();
				}
			}
			else
			{
				base.MentalStateTick();
			}
		}

		// Token: 0x06003BD2 RID: 15314 RVA: 0x001F853B File Offset: 0x001F693B
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.TryFindNewTarget();
		}

		// Token: 0x06003BD3 RID: 15315 RVA: 0x001F854C File Offset: 0x001F694C
		private bool TryFindNewTarget()
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, MentalState_TargetedTantrum.tmpThings, null, 300, 40);
			bool result = MentalState_TargetedTantrum.tmpThings.TryRandomElementByWeight((Thing x) => x.MarketValue * (float)x.stackCount, out this.target);
			MentalState_TargetedTantrum.tmpThings.Clear();
			return result;
		}

		// Token: 0x06003BD4 RID: 15316 RVA: 0x001F85C0 File Offset: 0x001F69C0
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
				result = string.Format(this.def.beginLetter, this.pawn.LabelShort, this.target.Label).AdjustedFor(this.pawn).CapitalizeFirst();
			}
			return result;
		}

		// Token: 0x04002586 RID: 9606
		public const int MinMarketValue = 300;

		// Token: 0x04002587 RID: 9607
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
