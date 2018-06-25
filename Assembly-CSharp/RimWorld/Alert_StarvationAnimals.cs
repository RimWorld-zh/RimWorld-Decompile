using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020007A8 RID: 1960
	public class Alert_StarvationAnimals : Alert
	{
		// Token: 0x06002B58 RID: 11096 RVA: 0x0016E701 File Offset: 0x0016CB01
		public Alert_StarvationAnimals()
		{
			this.defaultLabel = "StarvationAnimals".Translate();
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06002B59 RID: 11097 RVA: 0x0016E71C File Offset: 0x0016CB1C
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

		// Token: 0x06002B5A RID: 11098 RVA: 0x0016E77C File Offset: 0x0016CB7C
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

		// Token: 0x06002B5B RID: 11099 RVA: 0x0016E87C File Offset: 0x0016CC7C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.StarvingAnimals);
		}
	}
}
