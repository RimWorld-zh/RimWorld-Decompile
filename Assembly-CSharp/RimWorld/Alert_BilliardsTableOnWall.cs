using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class Alert_BilliardsTableOnWall : Alert
	{
		public Alert_BilliardsTableOnWall()
		{
			base.defaultLabel = "BilliardsNeedsSpace".Translate();
			base.defaultExplanation = "BilliardsNeedsSpaceDesc".Translate();
		}

		private Thing BadTable()
		{
			List<Map> maps = Find.Maps;
			int num = 0;
			Thing result;
			while (true)
			{
				List<Thing> list;
				int i;
				if (num < maps.Count)
				{
					list = maps[num].listerThings.ThingsOfDef(ThingDefOf.BilliardsTable);
					for (i = 0; i < list.Count; i++)
					{
						if (list[i].Faction == Faction.OfPlayer && !JoyGiver_PlayBilliards.ThingHasStandableSpaceOnAllSides(list[i]))
							goto IL_0055;
					}
					num++;
					continue;
				}
				result = null;
				break;
				IL_0055:
				result = list[i];
				break;
			}
			return result;
		}

		public override AlertReport GetReport()
		{
			Thing thing = this.BadTable();
			return (thing != null) ? AlertReport.CulpritIs(thing) : false;
		}
	}
}
