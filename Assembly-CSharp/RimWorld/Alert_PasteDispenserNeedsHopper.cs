using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class Alert_PasteDispenserNeedsHopper : Alert
	{
		public Alert_PasteDispenserNeedsHopper()
		{
			base.defaultLabel = "NeedFoodHopper".Translate();
			base.defaultExplanation = "NeedFoodHopperDesc".Translate();
			base.defaultPriority = AlertPriority.High;
		}

		public override AlertReport GetReport()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				foreach (Building item in from b in maps[i].listerBuildings.allBuildingsColonist
				where b.def.IsFoodDispenser
				select b)
				{
					bool flag = false;
					ThingDef hopper = ThingDefOf.Hopper;
					foreach (IntVec3 item2 in GenAdj.CellsAdjacentCardinal(item))
					{
						if (item2.InBounds(maps[i]))
						{
							Thing edifice = item2.GetEdifice(item.Map);
							if (edifice != null && edifice.def == hopper)
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						return AlertReport.CulpritIs(item);
					}
				}
			}
			return AlertReport.Inactive;
		}
	}
}
