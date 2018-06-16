using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200079F RID: 1951
	public class Alert_StarvationColonists : Alert
	{
		// Token: 0x06002B2E RID: 11054 RVA: 0x0016CCB1 File Offset: 0x0016B0B1
		public Alert_StarvationColonists()
		{
			this.defaultLabel = "Starvation".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06002B2F RID: 11055 RVA: 0x0016CCD4 File Offset: 0x0016B0D4
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

		// Token: 0x06002B30 RID: 11056 RVA: 0x0016CCF8 File Offset: 0x0016B0F8
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.StarvingColonists)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort);
			}
			return string.Format("StarvationDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002B31 RID: 11057 RVA: 0x0016CD88 File Offset: 0x0016B188
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.StarvingColonists);
		}
	}
}
