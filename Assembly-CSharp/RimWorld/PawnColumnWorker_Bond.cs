using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000887 RID: 2183
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Bond : PawnColumnWorker_Icon
	{
		// Token: 0x060031C6 RID: 12742 RVA: 0x001AE9C8 File Offset: 0x001ACDC8
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

		// Token: 0x060031C7 RID: 12743 RVA: 0x001AEA30 File Offset: 0x001ACE30
		protected override string GetIconTip(Pawn pawn)
		{
			return TrainableUtility.GetIconTooltipText(pawn);
		}

		// Token: 0x060031C8 RID: 12744 RVA: 0x001AEA4C File Offset: 0x001ACE4C
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetCompareValueFor(a).CompareTo(this.GetCompareValueFor(b));
		}

		// Token: 0x060031C9 RID: 12745 RVA: 0x001AEA78 File Offset: 0x001ACE78
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

		// Token: 0x060031CA RID: 12746 RVA: 0x001AEAE8 File Offset: 0x001ACEE8
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

		// Token: 0x04001AD0 RID: 6864
		private static readonly Texture2D BondIcon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Bond", true);

		// Token: 0x04001AD1 RID: 6865
		private static readonly Texture2D BondBrokenIcon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/BondBroken", true);
	}
}
