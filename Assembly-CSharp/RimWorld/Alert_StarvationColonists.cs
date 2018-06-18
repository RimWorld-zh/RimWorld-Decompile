using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200079F RID: 1951
	public class Alert_StarvationColonists : Alert
	{
		// Token: 0x06002B30 RID: 11056 RVA: 0x0016CD45 File Offset: 0x0016B145
		public Alert_StarvationColonists()
		{
			this.defaultLabel = "Starvation".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06002B31 RID: 11057 RVA: 0x0016CD68 File Offset: 0x0016B168
		private IEnumerable<Pawn> StarvingColonists
		{
			get
			{
				foreach (Pawn p in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep)
				{
					if (p.needs.food != null && p.needs.food.Starving)
					{
						yield return p;
					}
				}
				yield break;
			}
		}

		// Token: 0x06002B32 RID: 11058 RVA: 0x0016CD8C File Offset: 0x0016B18C
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.StarvingColonists)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort);
			}
			return string.Format("StarvationDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002B33 RID: 11059 RVA: 0x0016CE1C File Offset: 0x0016B21C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.StarvingColonists);
		}
	}
}
