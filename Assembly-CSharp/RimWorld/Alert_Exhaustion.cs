using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200079C RID: 1948
	public class Alert_Exhaustion : Alert
	{
		// Token: 0x06002B2D RID: 11053 RVA: 0x0016D1F9 File Offset: 0x0016B5F9
		public Alert_Exhaustion()
		{
			this.defaultLabel = "Exhaustion".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06002B2E RID: 11054 RVA: 0x0016D21C File Offset: 0x0016B61C
		private IEnumerable<Pawn> ExhaustedColonists
		{
			get
			{
				return from p in PawnsFinder.AllMaps_FreeColonistsSpawned
				where p.needs.rest != null && p.needs.rest.CurCategory == RestCategory.Exhausted
				select p;
			}
		}

		// Token: 0x06002B2F RID: 11055 RVA: 0x0016D258 File Offset: 0x0016B658
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.ExhaustedColonists)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort);
			}
			return string.Format("ExhaustionDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002B30 RID: 11056 RVA: 0x0016D2E8 File Offset: 0x0016B6E8
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ExhaustedColonists);
		}
	}
}
