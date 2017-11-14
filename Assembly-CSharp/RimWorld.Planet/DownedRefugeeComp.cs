using Verse;

namespace RimWorld.Planet
{
	public class DownedRefugeeComp : ImportantPawnComp, IThingHolder
	{
		protected override string PawnSaveKey
		{
			get
			{
				return "refugee";
			}
		}

		protected override void RemovePawnOnWorldObjectRemoved()
		{
			for (int num = base.pawn.Count - 1; num >= 0; num--)
			{
				if (!base.pawn[num].Dead)
				{
					base.pawn[num].Kill(null, null);
				}
			}
			base.pawn.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		public override string CompInspectStringExtra()
		{
			if (base.pawn.Any)
			{
				return "Refugee".Translate() + ": " + base.pawn[0].LabelShort;
			}
			return null;
		}
	}
}
