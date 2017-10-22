using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class Alert_FireInHomeArea : Alert_Critical
	{
		private Fire FireInHomeArea
		{
			get
			{
				List<Map> maps = Find.Maps;
				int num = 0;
				Fire result;
				while (true)
				{
					Thing thing;
					if (num < maps.Count)
					{
						List<Thing> list = maps[num].listerThings.ThingsOfDef(ThingDefOf.Fire);
						for (int i = 0; i < list.Count; i++)
						{
							thing = list[i];
							if (((Area)maps[num].areaManager.Home)[thing.Position] && !thing.Position.Fogged(thing.Map))
								goto IL_0071;
						}
						num++;
						continue;
					}
					result = null;
					break;
					IL_0071:
					result = (Fire)thing;
					break;
				}
				return result;
			}
		}

		public Alert_FireInHomeArea()
		{
			base.defaultLabel = "FireInHomeArea".Translate();
			base.defaultExplanation = "FireInHomeAreaDesc".Translate();
		}

		public override AlertReport GetReport()
		{
			Fire fireInHomeArea = this.FireInHomeArea;
			return (fireInHomeArea == null) ? false : AlertReport.CulpritIs((Thing)fireInHomeArea);
		}
	}
}
