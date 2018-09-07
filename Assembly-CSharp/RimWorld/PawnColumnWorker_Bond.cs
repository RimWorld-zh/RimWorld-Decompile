using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Bond : PawnColumnWorker_Icon
	{
		private static readonly Texture2D BondIcon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Bond", true);

		private static readonly Texture2D BondBrokenIcon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/BondBroken", true);

		public PawnColumnWorker_Bond()
		{
		}

		protected override Texture2D GetIconFor(Pawn pawn)
		{
			IEnumerable<Pawn> allColonistBondsFor = TrainableUtility.GetAllColonistBondsFor(pawn);
			if (!allColonistBondsFor.Any<Pawn>())
			{
				return null;
			}
			if (allColonistBondsFor.Any((Pawn bond) => bond == pawn.playerSettings.Master))
			{
				return PawnColumnWorker_Bond.BondIcon;
			}
			return PawnColumnWorker_Bond.BondBrokenIcon;
		}

		protected override string GetIconTip(Pawn pawn)
		{
			return TrainableUtility.GetIconTooltipText(pawn);
		}

		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetCompareValueFor(a).CompareTo(this.GetCompareValueFor(b));
		}

		public int GetCompareValueFor(Pawn a)
		{
			Texture2D iconFor = this.GetIconFor(a);
			if (iconFor == null)
			{
				return 0;
			}
			if (iconFor == PawnColumnWorker_Bond.BondBrokenIcon)
			{
				return 1;
			}
			if (iconFor == PawnColumnWorker_Bond.BondIcon)
			{
				return 2;
			}
			Log.ErrorOnce("Unknown bond type when trying to sort", 20536378, false);
			return 0;
		}

		protected override void PaintedIcon(Pawn pawn)
		{
			if (this.GetIconFor(pawn) != PawnColumnWorker_Bond.BondBrokenIcon)
			{
				return;
			}
			if (!pawn.training.HasLearned(TrainableDefOf.Obedience))
			{
				return;
			}
			pawn.playerSettings.Master = (from master in TrainableUtility.GetAllColonistBondsFor(pawn)
			where TrainableUtility.CanBeMaster(master, pawn, true)
			select master).FirstOrDefault<Pawn>();
		}

		// Note: this type is marked as 'beforefieldinit'.
		static PawnColumnWorker_Bond()
		{
		}

		[CompilerGenerated]
		private sealed class <GetIconFor>c__AnonStorey0
		{
			internal Pawn pawn;

			public <GetIconFor>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Pawn bond)
			{
				return bond == this.pawn.playerSettings.Master;
			}
		}

		[CompilerGenerated]
		private sealed class <PaintedIcon>c__AnonStorey1
		{
			internal Pawn pawn;

			public <PaintedIcon>c__AnonStorey1()
			{
			}

			internal bool <>m__0(Pawn master)
			{
				return TrainableUtility.CanBeMaster(master, this.pawn, true);
			}
		}
	}
}
