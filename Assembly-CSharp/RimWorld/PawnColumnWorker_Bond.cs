using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000883 RID: 2179
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Bond : PawnColumnWorker_Icon
	{
		// Token: 0x060031BF RID: 12735 RVA: 0x001AEBB0 File Offset: 0x001ACFB0
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			IEnumerable<Pawn> allColonistBondsFor = TrainableUtility.GetAllColonistBondsFor(pawn);
			Texture2D result;
			if (!allColonistBondsFor.Any<Pawn>())
			{
				result = null;
			}
			else if (allColonistBondsFor.Any((Pawn bond) => bond == pawn.playerSettings.Master))
			{
				result = PawnColumnWorker_Bond.BondIcon;
			}
			else
			{
				result = PawnColumnWorker_Bond.BondBrokenIcon;
			}
			return result;
		}

		// Token: 0x060031C0 RID: 12736 RVA: 0x001AEC18 File Offset: 0x001AD018
		protected override string GetIconTip(Pawn pawn)
		{
			return TrainableUtility.GetIconTooltipText(pawn);
		}

		// Token: 0x060031C1 RID: 12737 RVA: 0x001AEC34 File Offset: 0x001AD034
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetCompareValueFor(a).CompareTo(this.GetCompareValueFor(b));
		}

		// Token: 0x060031C2 RID: 12738 RVA: 0x001AEC60 File Offset: 0x001AD060
		public int GetCompareValueFor(Pawn a)
		{
			Texture2D iconFor = this.GetIconFor(a);
			int result;
			if (iconFor == null)
			{
				result = 0;
			}
			else if (iconFor == PawnColumnWorker_Bond.BondBrokenIcon)
			{
				result = 1;
			}
			else if (iconFor == PawnColumnWorker_Bond.BondIcon)
			{
				result = 2;
			}
			else
			{
				Log.ErrorOnce("Unknown bond type when trying to sort", 20536378, false);
				result = 0;
			}
			return result;
		}

		// Token: 0x060031C3 RID: 12739 RVA: 0x001AECD0 File Offset: 0x001AD0D0
		protected override void PaintedIcon(Pawn pawn)
		{
			if (!(this.GetIconFor(pawn) != PawnColumnWorker_Bond.BondBrokenIcon))
			{
				if (pawn.training.HasLearned(TrainableDefOf.Obedience))
				{
					pawn.playerSettings.Master = (from master in TrainableUtility.GetAllColonistBondsFor(pawn)
					where TrainableUtility.CanBeMaster(master, pawn, true)
					select master).FirstOrDefault<Pawn>();
				}
			}
		}

		// Token: 0x04001ACE RID: 6862
		private static readonly Texture2D BondIcon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Bond", true);

		// Token: 0x04001ACF RID: 6863
		private static readonly Texture2D BondBrokenIcon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/BondBroken", true);
	}
}
