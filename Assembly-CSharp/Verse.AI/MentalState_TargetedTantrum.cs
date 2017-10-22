using RimWorld;
using System;
using System.Collections.Generic;

namespace Verse.AI
{
	public class MentalState_TargetedTantrum : MentalState_Tantrum
	{
		public const int MinMarketValue = 300;

		private static List<Thing> tmpThings = new List<Thing>();

		public override void MentalStateTick()
		{
			if (base.target == null || base.target.Destroyed)
			{
				base.RecoverFromState();
			}
			else if (!base.target.Spawned || !base.pawn.CanReach(base.target, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				Thing target = base.target;
				if (!this.TryFindNewTarget())
				{
					base.RecoverFromState();
				}
				else
				{
					Messages.Message("MessageTargetedTantrumChangedTarget".Translate(base.pawn.LabelShort, target.Label, base.target.Label).AdjustedFor(base.pawn), (Thing)base.pawn, MessageTypeDefOf.NegativeEvent);
					base.MentalStateTick();
				}
			}
			else
			{
				base.MentalStateTick();
			}
		}

		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.TryFindNewTarget();
		}

		private bool TryFindNewTarget()
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(base.pawn, base.pawn.Position, MentalState_TargetedTantrum.tmpThings, null, 300, 40);
			bool result = ((IEnumerable<Thing>)MentalState_TargetedTantrum.tmpThings).TryRandomElementByWeight<Thing>((Func<Thing, float>)((Thing x) => x.MarketValue * (float)x.stackCount), out base.target);
			MentalState_TargetedTantrum.tmpThings.Clear();
			return result;
		}

		public override string GetBeginLetterText()
		{
			string result;
			if (base.target == null)
			{
				Log.Error("No target. This should have been checked in this mental state's worker.");
				result = "";
			}
			else
			{
				result = string.Format(base.def.beginLetter, base.pawn.Label, base.target.Label).AdjustedFor(base.pawn).CapitalizeFirst();
			}
			return result;
		}
	}
}
