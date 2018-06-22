using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A6 RID: 1958
	public class Alert_StarvationAnimals : Alert
	{
		// Token: 0x06002B54 RID: 11092 RVA: 0x0016E5B1 File Offset: 0x0016C9B1
		public Alert_StarvationAnimals()
		{
			this.defaultLabel = "StarvationAnimals".Translate();
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06002B55 RID: 11093 RVA: 0x0016E5CC File Offset: 0x0016C9CC
		private IEnumerable<Pawn> StarvingAnimals
		{
			get
			{
				return from p in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_NoCryptosleep
				where p.HostFaction == null && !p.RaceProps.Humanlike
				where p.needs.food != null && (p.needs.food.TicksStarving > 30000 || (p.health.hediffSet.HasHediff(HediffDefOf.Pregnant, true) && p.needs.food.TicksStarving > 5000))
				select p;
			}
		}

		// Token: 0x06002B56 RID: 11094 RVA: 0x0016E62C File Offset: 0x0016CA2C
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in from a in this.StarvingAnimals
			orderby a.def.label
			select a)
			{
				stringBuilder.Append("    " + pawn.LabelShort.CapitalizeFirst());
				if (pawn.Name.IsValid && !pawn.Name.Numerical)
				{
					stringBuilder.Append(" (" + pawn.def.label + ")");
				}
				stringBuilder.AppendLine();
			}
			return string.Format("StarvationAnimalsDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002B57 RID: 11095 RVA: 0x0016E72C File Offset: 0x0016CB2C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.StarvingAnimals);
		}
	}
}
