using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200079D RID: 1949
	public class Alert_StarvationColonists : Alert
	{
		// Token: 0x06002B2C RID: 11052 RVA: 0x0016D2D1 File Offset: 0x0016B6D1
		public Alert_StarvationColonists()
		{
			this.defaultLabel = "Starvation".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06002B2D RID: 11053 RVA: 0x0016D2F4 File Offset: 0x0016B6F4
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

		// Token: 0x06002B2E RID: 11054 RVA: 0x0016D318 File Offset: 0x0016B718
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.StarvingColonists)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort);
			}
			return string.Format("StarvationDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002B2F RID: 11055 RVA: 0x0016D3A8 File Offset: 0x0016B7A8
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.StarvingColonists);
		}
	}
}
