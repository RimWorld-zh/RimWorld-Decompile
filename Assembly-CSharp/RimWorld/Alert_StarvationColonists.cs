using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200079B RID: 1947
	public class Alert_StarvationColonists : Alert
	{
		// Token: 0x06002B29 RID: 11049 RVA: 0x0016CF1D File Offset: 0x0016B31D
		public Alert_StarvationColonists()
		{
			this.defaultLabel = "Starvation".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06002B2A RID: 11050 RVA: 0x0016CF40 File Offset: 0x0016B340
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

		// Token: 0x06002B2B RID: 11051 RVA: 0x0016CF64 File Offset: 0x0016B364
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.StarvingColonists)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort);
			}
			return string.Format("StarvationDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002B2C RID: 11052 RVA: 0x0016CFF4 File Offset: 0x0016B3F4
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.StarvingColonists);
		}
	}
}
