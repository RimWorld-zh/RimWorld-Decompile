using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200089D RID: 2205
	public class PawnTable_Animals : PawnTable
	{
		// Token: 0x0600327D RID: 12925 RVA: 0x001B266A File Offset: 0x001B0A6A
		public PawnTable_Animals(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}

		// Token: 0x0600327E RID: 12926 RVA: 0x001B2678 File Offset: 0x001B0A78
		protected override IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.Name.Numerical, (!(p.Name is NameSingle)) ? 0 : ((NameSingle)p.Name).Number, p.def.label
			select p;
		}

		// Token: 0x0600327F RID: 12927 RVA: 0x001B26F4 File Offset: 0x001B0AF4
		protected override IEnumerable<Pawn> PrimarySortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.RaceProps.petness descending, p.RaceProps.baseBodySize
			select p;
		}
	}
}
