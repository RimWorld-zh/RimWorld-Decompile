using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A7 RID: 1959
	public class Alert_BilliardsTableOnWall : Alert
	{
		// Token: 0x06002B5B RID: 11099 RVA: 0x0016E817 File Offset: 0x0016CC17
		public Alert_BilliardsTableOnWall()
		{
			this.defaultLabel = "BilliardsNeedsSpace".Translate();
			this.defaultExplanation = "BilliardsNeedsSpaceDesc".Translate();
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06002B5C RID: 11100 RVA: 0x0016E840 File Offset: 0x0016CC40
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

		// Token: 0x06002B5D RID: 11101 RVA: 0x0016E864 File Offset: 0x0016CC64
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadTables);
		}
	}
}
