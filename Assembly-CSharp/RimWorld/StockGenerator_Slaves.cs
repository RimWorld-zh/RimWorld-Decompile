using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class StockGenerator_Slaves : StockGenerator
	{
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			if (!(Rand.Value > Find.Storyteller.intenderPopulation.PopulationIntent))
			{
				int count = base.countRange.RandomInRange;
				int i = 0;
				Faction slaveFaction;
				if (i < count && (from fac in Find.FactionManager.AllFactionsVisible
				where fac != Faction.OfPlayer && fac.def.humanlikeFaction
				select fac).TryRandomElement<Faction>(out slaveFaction))
				{
					PawnKindDef slave = PawnKindDefOf.Slave;
					PawnGenerationRequest request = new PawnGenerationRequest(slave, slaveFaction, PawnGenerationContext.NonPlayer, forTile, false, false, false, false, true, false, 1f, !base.trader.orbital, true, true, false, false, false, false, null, null, null, null, null, null, null);
					yield return (Thing)PawnGenerator.GeneratePawn(request);
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
		}

		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Pawn && thingDef.race.Humanlike && thingDef.tradeability != Tradeability.Never;
		}
	}
}
