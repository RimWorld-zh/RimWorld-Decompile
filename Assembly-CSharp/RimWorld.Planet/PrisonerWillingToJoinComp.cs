using Verse;

namespace RimWorld.Planet
{
	public class PrisonerWillingToJoinComp : ImportantPawnComp, IThingHolder
	{
		protected override string PawnSaveKey
		{
			get
			{
				return "prisoner";
			}
		}

		protected override void RemovePawnOnWorldObjectRemoved()
		{
			base.pawn.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
		}

		public override string CompInspectStringExtra()
		{
			return (!base.pawn.Any) ? null : ("Prisoner".Translate() + ": " + base.pawn[0].LabelShort);
		}
	}
}
