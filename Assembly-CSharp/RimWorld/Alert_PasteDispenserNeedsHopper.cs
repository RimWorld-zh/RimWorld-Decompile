using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200079D RID: 1949
	public class Alert_PasteDispenserNeedsHopper : Alert
	{
		// Token: 0x06002B32 RID: 11058 RVA: 0x0016D342 File Offset: 0x0016B742
		public Alert_PasteDispenserNeedsHopper()
		{
			this.defaultLabel = "NeedFoodHopper".Translate();
			this.defaultExplanation = "NeedFoodHopperDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06002B33 RID: 11059 RVA: 0x0016D374 File Offset: 0x0016B774
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

		// Token: 0x06002B34 RID: 11060 RVA: 0x0016D398 File Offset: 0x0016B798
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadDispensers);
		}
	}
}
