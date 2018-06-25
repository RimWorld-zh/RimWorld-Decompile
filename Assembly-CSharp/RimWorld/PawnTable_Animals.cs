using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200089B RID: 2203
	public class PawnTable_Animals : PawnTable
	{
		// Token: 0x0600327A RID: 12922 RVA: 0x001B2992 File Offset: 0x001B0D92
		public PawnTable_Animals(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}

		// Token: 0x0600327B RID: 12923 RVA: 0x001B29A0 File Offset: 0x001B0DA0
		protected override IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.Name.Numerical, (!(p.Name is NameSingle)) ? 0 : ((NameSingle)p.Name).Number, p.def.label
			select p;
		}

		// Token: 0x0600327C RID: 12924 RVA: 0x001B2A1C File Offset: 0x001B0E1C
		protected override IEnumerable<Pawn> PrimarySortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.RaceProps.petness descending, p.RaceProps.baseBodySize
			select p;
		}
	}
}
