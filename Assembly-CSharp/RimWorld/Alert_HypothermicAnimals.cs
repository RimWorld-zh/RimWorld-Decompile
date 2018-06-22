using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200078F RID: 1935
	public class Alert_HypothermicAnimals : Alert
	{
		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x06002AF1 RID: 10993 RVA: 0x0016B144 File Offset: 0x00169544
		private IEnumerable<Pawn> HypothermicAnimals
		{
			get
			{
				return from p in PawnsFinder.AllMaps_Spawned
				where p.RaceProps.Animal && p.Faction == null && p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia, false) != null
				select p;
			}
		}

		// Token: 0x06002AF2 RID: 10994 RVA: 0x0016B180 File Offset: 0x00169580
		public override string GetLabel()
		{
			return "Hypothermic wild animals (debug)";
		}

		// Token: 0x06002AF3 RID: 10995 RVA: 0x0016B19C File Offset: 0x0016959C
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

		// Token: 0x06002AF4 RID: 10996 RVA: 0x0016B248 File Offset: 0x00169648
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
