using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000791 RID: 1937
	public class Alert_FireInHomeArea : Alert_Critical
	{
		// Token: 0x06002AEF RID: 10991 RVA: 0x0016ABF1 File Offset: 0x00168FF1
		public Alert_FireInHomeArea()
		{
			this.defaultLabel = "FireInHomeArea".Translate();
			this.defaultExplanation = "FireInHomeAreaDesc".Translate();
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06002AF0 RID: 10992 RVA: 0x0016AC1C File Offset: 0x0016901C
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

		// Token: 0x06002AF1 RID: 10993 RVA: 0x0016ACD4 File Offset: 0x001690D4
		public override AlertReport GetReport()
		{
			return this.FireInHomeArea;
		}
	}
}
