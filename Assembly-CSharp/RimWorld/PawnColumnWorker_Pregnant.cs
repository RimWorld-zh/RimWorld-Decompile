using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200088D RID: 2189
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Pregnant : PawnColumnWorker_Icon
	{
		// Token: 0x060031EB RID: 12779 RVA: 0x001AEDA0 File Offset: 0x001AD1A0
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			Hediff_Pregnant pregnantHediff = PawnColumnWorker_Pregnant.GetPregnantHediff(pawn);
			return (pregnantHediff == null) ? null : PawnColumnWorker_Pregnant.Icon;
		}

		// Token: 0x060031EC RID: 12780 RVA: 0x001AEDD0 File Offset: 0x001AD1D0
		protected override string GetIconTip(Pawn pawn)
		{
			return PawnColumnWorker_Pregnant.GetTooltipText(pawn);
		}

		// Token: 0x060031ED RID: 12781 RVA: 0x001AEDEC File Offset: 0x001AD1EC
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

		// Token: 0x060031EE RID: 12782 RVA: 0x001AEE58 File Offset: 0x001AD258
		private static Hediff_Pregnant GetPregnantHediff(Pawn pawn)
		{
			return (Hediff_Pregnant)pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Pregnant, true);
		}

		// Token: 0x04001AD3 RID: 6867
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Pregnant", true);
	}
}
