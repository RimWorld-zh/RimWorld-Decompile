using RimWorld.Planet;
using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class StorytellerComp_CategoryIndividualMTBByBiome : StorytellerComp
	{
		protected StorytellerCompProperties_CategoryIndividualMTBByBiome Props
		{
			get
			{
				return (StorytellerCompProperties_CategoryIndividualMTBByBiome)base.props;
			}
		}

		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (!(target is World))
			{
				List<IncidentDef> allIncidents = DefDatabase<IncidentDef>.AllDefsListForReading;
				int i = 0;
				IncidentDef inc;
				while (true)
				{
					if (i < allIncidents.Count)
					{
						inc = allIncidents[i];
						if (inc.category == this.Props.category)
						{
							_003CMakeIntervalIncidents_003Ec__Iterator0 _003CMakeIntervalIncidents_003Ec__Iterator = (_003CMakeIntervalIncidents_003Ec__Iterator0)/*Error near IL_0097: stateMachine*/;
							BiomeDef biome = Find.WorldGrid[target.Tile].biome;
							if (inc.mtbDaysByBiome != null)
							{
								MTBByBiome entry = inc.mtbDaysByBiome.Find((Predicate<MTBByBiome>)((MTBByBiome x) => x.biome == biome));
								if (entry != null)
								{
									float mtb = entry.mtbDays;
									if (this.Props.applyCaravanStealthFactor)
									{
										Caravan caravan = target as Caravan;
										if (caravan != null)
										{
											mtb *= CaravanIncidentUtility.CalculateCaravanStealthFactor(caravan.PawnsListForReading.Count);
										}
										else
										{
											Map map = target as Map;
											if (map != null && map.info.parent.def.isTempIncidentMapOwner)
											{
												int pawnCount = map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer).Count + map.mapPawns.PrisonersOfColonySpawnedCount;
												mtb *= CaravanIncidentUtility.CalculateCaravanStealthFactor(pawnCount);
											}
										}
									}
									if (Rand.MTBEventOccurs(mtb, 60000f, 1000f) && inc.Worker.CanFireNow(target))
										break;
								}
							}
						}
						i++;
						continue;
					}
					yield break;
				}
				yield return new FiringIncident(inc, this, this.GenerateParms(inc.category, target));
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}
	}
}
