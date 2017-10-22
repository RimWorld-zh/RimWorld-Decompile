using System;
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
				while (i < count && (from fac in Find.FactionManager.AllFactionsVisible
				where fac != Faction.OfPlayer && fac.def.humanlikeFaction
				select fac).TryRandomElement<Faction>(out slaveFaction))
				{
					PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.Slave, slaveFaction, PawnGenerationContext.NonPlayer, forTile, false, false, false, false, true, false, 1f, !base.trader.orbital, true, true, false, false, null, default(float?), default(float?), default(Gender?), default(float?), (string)null);
					yield return (Thing)PawnGenerator.GeneratePawn(request);
					i++;
				}
			}
		}

		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Pawn && thingDef.race.Humanlike;
		}
	}
}
