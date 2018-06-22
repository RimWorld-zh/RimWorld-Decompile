using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200078D RID: 1933
	public class Alert_FireInHomeArea : Alert_Critical
	{
		// Token: 0x06002AEA RID: 10986 RVA: 0x0016AE5D File Offset: 0x0016925D
		public Alert_FireInHomeArea()
		{
			this.defaultLabel = "FireInHomeArea".Translate();
			this.defaultExplanation = "FireInHomeAreaDesc".Translate();
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x06002AEB RID: 10987 RVA: 0x0016AE88 File Offset: 0x00169288
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

		// Token: 0x06002AEC RID: 10988 RVA: 0x0016AF40 File Offset: 0x00169340
		public override AlertReport GetReport()
		{
			return this.FireInHomeArea;
		}
	}
}
