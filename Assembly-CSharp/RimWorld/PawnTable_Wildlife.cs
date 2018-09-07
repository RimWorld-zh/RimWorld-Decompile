using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class PawnTable_Wildlife : PawnTable
	{
		[CompilerGenerated]
		private static Func<Pawn, float> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Pawn, string> <>f__am$cache1;

		public PawnTable_Wildlife(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}

		protected override IEnumerable<Pawn> PrimarySortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.RaceProps.baseBodySize descending, p.def.label
			select p;
		}

		[CompilerGenerated]
		private static float <PrimarySortFunction>m__0(Pawn p)
		{
			return p.RaceProps.baseBodySize;
		}

		[CompilerGenerated]
		private static string <PrimarySortFunction>m__1(Pawn p)
		{
			return p.def.label;
		}
	}
}
