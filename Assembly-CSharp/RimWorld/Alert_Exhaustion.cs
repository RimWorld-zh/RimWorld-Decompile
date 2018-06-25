using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200079E RID: 1950
	public class Alert_Exhaustion : Alert
	{
		// Token: 0x06002B30 RID: 11056 RVA: 0x0016D5AD File Offset: 0x0016B9AD
		public Alert_Exhaustion()
		{
			this.defaultLabel = "Exhaustion".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06002B31 RID: 11057 RVA: 0x0016D5D0 File Offset: 0x0016B9D0
		private IEnumerable<Pawn> ExhaustedColonists
		{
			get
			{
				return from p in PawnsFinder.AllMaps_FreeColonistsSpawned
				where p.needs.rest != null && p.needs.rest.CurCategory == RestCategory.Exhausted
				select p;
			}
		}

		// Token: 0x06002B32 RID: 11058 RVA: 0x0016D60C File Offset: 0x0016BA0C
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.ExhaustedColonists)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort);
			}
			return string.Format("ExhaustionDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002B33 RID: 11059 RVA: 0x0016D69C File Offset: 0x0016BA9C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ExhaustedColonists);
		}
	}
}
