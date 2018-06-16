using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A1 RID: 1953
	public class Alert_PasteDispenserNeedsHopper : Alert
	{
		// Token: 0x06002B37 RID: 11063 RVA: 0x0016D0D6 File Offset: 0x0016B4D6
		public Alert_PasteDispenserNeedsHopper()
		{
			this.defaultLabel = "NeedFoodHopper".Translate();
			this.defaultExplanation = "NeedFoodHopperDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06002B38 RID: 11064 RVA: 0x0016D108 File Offset: 0x0016B508
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

		// Token: 0x06002B39 RID: 11065 RVA: 0x0016D12C File Offset: 0x0016B52C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadDispensers);
		}
	}
}
