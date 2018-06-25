using System;
using Verse;

namespace RimWorld.Planet
{
	public class PrisonerWillingToJoinComp : ImportantPawnComp, IThingHolder
	{
		public PrisonerWillingToJoinComp()
		{
		}

		protected override string PawnSaveKey
		{
			get
			{
				return "prisoner";
			}
		}

		protected override void RemovePawnOnWorldObjectRemoved()
		{
			this.pawn.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
		}

		public override string CompInspectStringExtra()
		{
			string result;
			if (this.pawn.Any)
			{
				result = "Prisoner".Translate() + ": " + this.pawn[0].LabelCap;
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
