using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000775 RID: 1909
	public class StockGenerator_Slaves : StockGenerator
	{
		// Token: 0x06002A1E RID: 10782 RVA: 0x00164F38 File Offset: 0x00163338
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

		// Token: 0x06002A1F RID: 10783 RVA: 0x00164F6C File Offset: 0x0016336C
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Pawn && thingDef.race.Humanlike && thingDef.tradeability != Tradeability.None;
		}

		// Token: 0x040016BA RID: 5818
		private bool respectPopulationIntent = false;
	}
}
