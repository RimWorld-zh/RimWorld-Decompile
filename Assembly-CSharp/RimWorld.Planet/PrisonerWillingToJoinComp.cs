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
			if (base.pawn.Any)
			{
				return "Prisoner".Translate() + ": " + base.pawn[0].LabelShort;
			}
			return null;
		}
	}
}
