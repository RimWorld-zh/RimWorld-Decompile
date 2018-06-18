using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000791 RID: 1937
	public class Alert_FireInHomeArea : Alert_Critical
	{
		// Token: 0x06002AF1 RID: 10993 RVA: 0x0016AC85 File Offset: 0x00169085
		public Alert_FireInHomeArea()
		{
			this.defaultLabel = "FireInHomeArea".Translate();
			this.defaultExplanation = "FireInHomeAreaDesc".Translate();
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06002AF2 RID: 10994 RVA: 0x0016ACB0 File Offset: 0x001690B0
		private Fire FireInHomeArea
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThingDefOf.Fire);
					for (int j = 0; j < list.Count; j++)
					{
						Thing thing = list[j];
						if (maps[i].areaManager.Home[thing.Position] && !thing.Position.Fogged(thing.Map))
						{
							return (Fire)thing;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x06002AF3 RID: 10995 RVA: 0x0016AD68 File Offset: 0x00169168
		public override AlertReport GetReport()
		{
			return this.FireInHomeArea;
		}
	}
}
