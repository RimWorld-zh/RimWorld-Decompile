using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200088B RID: 2187
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Pregnant : PawnColumnWorker_Icon
	{
		// Token: 0x04001AD1 RID: 6865
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Pregnant", true);

		// Token: 0x060031E8 RID: 12776 RVA: 0x001AF0C8 File Offset: 0x001AD4C8
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			Hediff_Pregnant pregnantHediff = PawnColumnWorker_Pregnant.GetPregnantHediff(pawn);
			return (pregnantHediff == null) ? null : PawnColumnWorker_Pregnant.Icon;
		}

		// Token: 0x060031E9 RID: 12777 RVA: 0x001AF0F8 File Offset: 0x001AD4F8
		protected override string GetIconTip(Pawn pawn)
		{
			return PawnColumnWorker_Pregnant.GetTooltipText(pawn);
		}

		// Token: 0x060031EA RID: 12778 RVA: 0x001AF114 File Offset: 0x001AD514
		public static string GetTooltipText(Pawn pawn)
		{
			Hediff_Pregnant pregnantHediff = PawnColumnWorker_Pregnant.GetPregnantHediff(pawn);
			float gestationProgress = pregnantHediff.GestationProgress;
			int num = (int)(pawn.RaceProps.gestationPeriodDays * 60000f);
			int numTicks = (int)(gestationProgress * (float)num);
			return "PregnantIconDesc".Translate(new object[]
			{
				numTicks.ToStringTicksToDays("F0"),
				num.ToStringTicksToDays("F0")
			});
		}

		// Token: 0x060031EB RID: 12779 RVA: 0x001AF180 File Offset: 0x001AD580
		private static Hediff_Pregnant GetPregnantHediff(Pawn pawn)
		{
			return (Hediff_Pregnant)pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Pregnant, true);
		}
	}
}
