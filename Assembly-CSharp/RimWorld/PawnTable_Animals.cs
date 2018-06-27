using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class PawnTable_Animals : PawnTable
	{
		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Pawn, int> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<Pawn, string> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<Pawn, float> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<Pawn, float> <>f__am$cache4;

		public PawnTable_Animals(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}

		protected override IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.Name == null || p.Name.Numerical, (!(p.Name is NameSingle)) ? 0 : ((NameSingle)p.Name).Number, p.def.label
			select p;
		}

		protected override IEnumerable<Pawn> PrimarySortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.RaceProps.petness descending, p.RaceProps.baseBodySize
			select p;
		}

		[CompilerGenerated]
		private static bool <LabelSortFunction>m__0(Pawn p)
		{
			return p.Name == null || p.Name.Numerical;
		}

		[CompilerGenerated]
		private static int <LabelSortFunction>m__1(Pawn p)
		{
			return (!(p.Name is NameSingle)) ? 0 : ((NameSingle)p.Name).Number;
		}

		[CompilerGenerated]
		private static string <LabelSortFunction>m__2(Pawn p)
		{
			return p.def.label;
		}

		[CompilerGenerated]
		private static float <PrimarySortFunction>m__3(Pawn p)
		{
			return p.RaceProps.petness;
		}

		[CompilerGenerated]
		private static float <PrimarySortFunction>m__4(Pawn p)
		{
			return p.RaceProps.baseBodySize;
		}
	}
}
