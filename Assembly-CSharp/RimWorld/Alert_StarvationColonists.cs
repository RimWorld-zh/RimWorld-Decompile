using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200079D RID: 1949
	public class Alert_StarvationColonists : Alert
	{
		// Token: 0x06002B2D RID: 11053 RVA: 0x0016D06D File Offset: 0x0016B46D
		public Alert_StarvationColonists()
		{
			this.defaultLabel = "Starvation".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06002B2E RID: 11054 RVA: 0x0016D090 File Offset: 0x0016B490
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

		// Token: 0x06002B2F RID: 11055 RVA: 0x0016D0B4 File Offset: 0x0016B4B4
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.StarvingColonists)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort);
			}
			return string.Format("StarvationDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002B30 RID: 11056 RVA: 0x0016D144 File Offset: 0x0016B544
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.StarvingColonists);
		}
	}
}
