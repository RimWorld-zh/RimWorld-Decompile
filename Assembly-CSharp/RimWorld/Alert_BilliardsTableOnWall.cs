using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007AB RID: 1963
	public class Alert_BilliardsTableOnWall : Alert
	{
		// Token: 0x06002B62 RID: 11106 RVA: 0x0016E63F File Offset: 0x0016CA3F
		public Alert_BilliardsTableOnWall()
		{
			this.defaultLabel = "BilliardsNeedsSpace".Translate();
			this.defaultExplanation = "BilliardsNeedsSpaceDesc".Translate();
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06002B63 RID: 11107 RVA: 0x0016E668 File Offset: 0x0016CA68
		private IEnumerable<Thing> BadTables
		{
			get
			{
				List<Map> maps = Find.Maps;
				Faction ofPlayer = Faction.OfPlayer;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> bList = maps[i].listerThings.ThingsOfDef(ThingDefOf.BilliardsTable);
					for (int j = 0; j < bList.Count; j++)
					{
						if (bList[j].Faction == ofPlayer && !JoyGiver_PlayBilliards.ThingHasStandableSpaceOnAllSides(bList[j]))
						{
							yield return bList[j];
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x06002B64 RID: 11108 RVA: 0x0016E68C File Offset: 0x0016CA8C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadTables);
		}
	}
}
