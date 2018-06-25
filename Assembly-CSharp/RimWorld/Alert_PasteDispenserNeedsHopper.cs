using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200079F RID: 1951
	public class Alert_PasteDispenserNeedsHopper : Alert
	{
		// Token: 0x06002B36 RID: 11062 RVA: 0x0016D492 File Offset: 0x0016B892
		public Alert_PasteDispenserNeedsHopper()
		{
			this.defaultLabel = "NeedFoodHopper".Translate();
			this.defaultExplanation = "NeedFoodHopperDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06002B37 RID: 11063 RVA: 0x0016D4C4 File Offset: 0x0016B8C4
		private IEnumerable<Thing> BadDispensers
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Building disp in maps[i].listerBuildings.allBuildingsColonist)
					{
						if (disp.def.IsFoodDispenser)
						{
							bool foundHopper = false;
							ThingDef hopperDef = ThingDefOf.Hopper;
							foreach (IntVec3 c in GenAdj.CellsAdjacentCardinal(disp))
							{
								if (c.InBounds(maps[i]))
								{
									Thing building = c.GetEdifice(disp.Map);
									if (building != null && building.def == hopperDef)
									{
										foundHopper = true;
										break;
									}
								}
							}
							if (!foundHopper)
							{
								yield return disp;
							}
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x06002B38 RID: 11064 RVA: 0x0016D4E8 File Offset: 0x0016B8E8
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadDispensers);
		}
	}
}
