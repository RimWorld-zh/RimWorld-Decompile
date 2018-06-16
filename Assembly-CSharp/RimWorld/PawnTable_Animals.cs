using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200089D RID: 2205
	public class PawnTable_Animals : PawnTable
	{
		// Token: 0x0600327B RID: 12923 RVA: 0x001B25A2 File Offset: 0x001B09A2
		public PawnTable_Animals(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}

		// Token: 0x0600327C RID: 12924 RVA: 0x001B25B0 File Offset: 0x001B09B0
		protected override IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.Name.Numerical, (!(p.Name is NameSingle)) ? 0 : ((NameSingle)p.Name).Number, p.def.label
			select p;
		}

		// Token: 0x0600327D RID: 12925 RVA: 0x001B262C File Offset: 0x001B0A2C
		protected override IEnumerable<Pawn> PrimarySortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.RaceProps.petness descending, p.RaceProps.baseBodySize
			select p;
		}
	}
}
