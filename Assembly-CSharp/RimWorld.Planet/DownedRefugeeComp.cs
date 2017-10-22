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
					base.pawn[num].Kill(default(DamageInfo?), null);
				}
			}
			base.pawn.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		public override string CompInspectStringExtra()
		{
			return (!base.pawn.Any) ? null : ("Refugee".Translate() + ": " + base.pawn[0].LabelShort);
		}
	}
}
