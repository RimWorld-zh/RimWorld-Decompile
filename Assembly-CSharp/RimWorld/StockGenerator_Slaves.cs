using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000773 RID: 1907
	public class StockGenerator_Slaves : StockGenerator
	{
		// Token: 0x040016B8 RID: 5816
		private bool respectPopulationIntent = false;

		// Token: 0x06002A1D RID: 10781 RVA: 0x001652F4 File Offset: 0x001636F4
		public override IEnumerable<Thing> GenerateThings(int forTile)
		{
			if (this.respectPopulationIntent && Rand.Value > Find.Storyteller.intenderPopulation.PopulationIntent)
			{
				yield break;
			}
			int count = this.countRange.RandomInRange;
			for (int i = 0; i < count; i++)
			{
				Faction slaveFaction;
				if (!(from fac in Find.FactionManager.AllFactionsVisible
				where fac != Faction.OfPlayer && fac.def.humanlikeFaction
				select fac).TryRandomElement(out slaveFaction))
				{
					yield break;
				}
				PawnKindDef slave = PawnKindDefOf.Slave;
				Faction faction = slaveFaction;
				PawnGenerationRequest request = new PawnGenerationRequest(slave, faction, PawnGenerationContext.NonPlayer, forTile, false, false, false, false, true, false, 1f, !this.trader.orbital, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
				yield return PawnGenerator.GeneratePawn(request);
			}
			yield break;
		}

		// Token: 0x06002A1E RID: 10782 RVA: 0x00165328 File Offset: 0x00163728
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Pawn && thingDef.race.Humanlike && thingDef.tradeability != Tradeability.None;
		}
	}
}
