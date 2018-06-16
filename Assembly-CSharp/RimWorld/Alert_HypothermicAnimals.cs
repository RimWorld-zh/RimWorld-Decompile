using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000793 RID: 1939
	public class Alert_HypothermicAnimals : Alert
	{
		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x06002AF6 RID: 10998 RVA: 0x0016AED8 File Offset: 0x001692D8
		private IEnumerable<Pawn> HypothermicAnimals
		{
			get
			{
				return from p in PawnsFinder.AllMaps_Spawned
				where p.RaceProps.Animal && p.Faction == null && p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia, false) != null
				select p;
			}
		}

		// Token: 0x06002AF7 RID: 10999 RVA: 0x0016AF14 File Offset: 0x00169314
		public override string GetLabel()
		{
			return "Hypothermic wild animals (debug)";
		}

		// Token: 0x06002AF8 RID: 11000 RVA: 0x0016AF30 File Offset: 0x00169330
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Debug alert:\n\nThese wild animals are hypothermic. This may indicate a bug (but it may not, if the animals are trapped or in some other wierd but legitimate situation):");
			foreach (Pawn pawn in this.HypothermicAnimals)
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"    ",
					pawn,
					" at ",
					pawn.Position
				}));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002AF9 RID: 11001 RVA: 0x0016AFDC File Offset: 0x001693DC
		public override AlertReport GetReport()
		{
			AlertReport result;
			if (!Prefs.DevMode)
			{
				result = false;
			}
			else
			{
				result = AlertReport.CulpritsAre(this.HypothermicAnimals);
			}
			return result;
		}
	}
}
