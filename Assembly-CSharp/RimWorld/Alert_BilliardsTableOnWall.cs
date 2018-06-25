using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A9 RID: 1961
	public class Alert_BilliardsTableOnWall : Alert
	{
		// Token: 0x06002B5F RID: 11103 RVA: 0x0016E967 File Offset: 0x0016CD67
		public Alert_BilliardsTableOnWall()
		{
			this.defaultLabel = "BilliardsNeedsSpace".Translate();
			this.defaultExplanation = "BilliardsNeedsSpaceDesc".Translate();
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06002B60 RID: 11104 RVA: 0x0016E990 File Offset: 0x0016CD90
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

		// Token: 0x06002B61 RID: 11105 RVA: 0x0016E9B4 File Offset: 0x0016CDB4
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadTables);
		}
	}
}
