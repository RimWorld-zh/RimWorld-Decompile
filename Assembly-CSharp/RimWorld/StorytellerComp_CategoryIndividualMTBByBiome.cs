using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000363 RID: 867
	public class StorytellerComp_CategoryIndividualMTBByBiome : StorytellerComp
	{
		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000F1A RID: 3866 RVA: 0x0007F9D4 File Offset: 0x0007DDD4
		protected StorytellerCompProperties_CategoryIndividualMTBByBiome Props
		{
			get
			{
				return (StorytellerCompProperties_CategoryIndividualMTBByBiome)this.props;
			}
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x0007F9F4 File Offset: 0x0007DDF4
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (target is World)
			{
				yield break;
			}
			List<IncidentDef> allIncidents = DefDatabase<IncidentDef>.AllDefsListForReading;
			int i = 0;
			while (i < allIncidents.Count)
			{
				IncidentDef inc = allIncidents[i];
				if (inc.category == this.Props.category)
				{
					BiomeDef biome = Find.WorldGrid[target.Tile].biome;
					if (inc.mtbDaysByBiome != null)
					{
						MTBByBiome entry = inc.mtbDaysByBiome.Find((MTBByBiome x) => x.biome == biome);
						if (entry != null)
						{
							float mtb = entry.mtbDays;
							if (this.Props.applyCaravanVisibility)
							{
								Caravan caravan = target as Caravan;
								if (caravan != null)
								{
									mtb /= caravan.Visibility;
								}
								else
								{
									Map map = target as Map;
									if (map != null && map.Parent.def.isTempIncidentMapOwner)
									{
										IEnumerable<Pawn> pawns = map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer).Concat(map.mapPawns.PrisonersOfColonySpawned);
										mtb /= CaravanVisibilityCalculator.Visibility(pawns, false, null);
									}
								}
							}
							if (Rand.MTBEventOccurs(mtb, 60000f, 1000f))
							{
								IncidentParms parms = this.GenerateParms(inc.category, target);
								if (inc.Worker.CanFireNow(parms))
								{
									yield return new FiringIncident(inc, this, parms);
								}
							}
						}
					}
				}
				IL_24E:
				i++;
				continue;
				goto IL_24E;
			}
			yield break;
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x0007FA28 File Offset: 0x0007DE28
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.category;
		}
	}
}
