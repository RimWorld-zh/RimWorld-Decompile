using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020007AA RID: 1962
	public class Alert_StarvationAnimals : Alert
	{
		// Token: 0x06002B5B RID: 11099 RVA: 0x0016E3D9 File Offset: 0x0016C7D9
		public Alert_StarvationAnimals()
		{
			this.defaultLabel = "StarvationAnimals".Translate();
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06002B5C RID: 11100 RVA: 0x0016E3F4 File Offset: 0x0016C7F4
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

		// Token: 0x06002B5D RID: 11101 RVA: 0x0016E454 File Offset: 0x0016C854
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

		// Token: 0x06002B5E RID: 11102 RVA: 0x0016E554 File Offset: 0x0016C954
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.StarvingAnimals);
		}
	}
}
