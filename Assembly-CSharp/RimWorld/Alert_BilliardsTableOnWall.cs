using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020007AB RID: 1963
	public class Alert_BilliardsTableOnWall : Alert
	{
		// Token: 0x06002B60 RID: 11104 RVA: 0x0016E5AB File Offset: 0x0016C9AB
		public Alert_BilliardsTableOnWall()
		{
			this.defaultLabel = "BilliardsNeedsSpace".Translate();
			this.defaultExplanation = "BilliardsNeedsSpaceDesc".Translate();
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06002B61 RID: 11105 RVA: 0x0016E5D4 File Offset: 0x0016C9D4
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

		// Token: 0x06002B62 RID: 11106 RVA: 0x0016E5F8 File Offset: 0x0016C9F8
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadTables);
		}
	}
}
