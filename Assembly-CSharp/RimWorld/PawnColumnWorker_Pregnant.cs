using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000889 RID: 2185
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Pregnant : PawnColumnWorker_Icon
	{
		// Token: 0x060031E4 RID: 12772 RVA: 0x001AEF88 File Offset: 0x001AD388
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			Hediff_Pregnant pregnantHediff = PawnColumnWorker_Pregnant.GetPregnantHediff(pawn);
			return (pregnantHediff == null) ? null : PawnColumnWorker_Pregnant.Icon;
		}

		// Token: 0x060031E5 RID: 12773 RVA: 0x001AEFB8 File Offset: 0x001AD3B8
		protected override string GetIconTip(Pawn pawn)
		{
			return PawnColumnWorker_Pregnant.GetTooltipText(pawn);
		}

		// Token: 0x060031E6 RID: 12774 RVA: 0x001AEFD4 File Offset: 0x001AD3D4
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

		// Token: 0x060031E7 RID: 12775 RVA: 0x001AF040 File Offset: 0x001AD440
		private static Hediff_Pregnant GetPregnantHediff(Pawn pawn)
		{
			return (Hediff_Pregnant)pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Pregnant, true);
		}

		// Token: 0x04001AD1 RID: 6865
		private static readonly Texture2D Icon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Pregnant", true);
	}
}
