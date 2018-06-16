using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000361 RID: 865
	public class StorytellerComp_CategoryIndividualMTBByBiome : StorytellerComp
	{
		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000F17 RID: 3863 RVA: 0x0007F688 File Offset: 0x0007DA88
		protected StorytellerCompProperties_CategoryIndividualMTBByBiome Props
		{
			get
			{
				return (StorytellerCompProperties_CategoryIndividualMTBByBiome)this.props;
			}
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x0007F6A8 File Offset: 0x0007DAA8
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

		// Token: 0x06000F19 RID: 3865 RVA: 0x0007F6DC File Offset: 0x0007DADC
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.category;
		}
	}
}
