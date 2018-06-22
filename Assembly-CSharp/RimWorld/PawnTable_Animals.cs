using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000899 RID: 2201
	public class PawnTable_Animals : PawnTable
	{
		// Token: 0x06003276 RID: 12918 RVA: 0x001B2852 File Offset: 0x001B0C52
		public PawnTable_Animals(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}

		// Token: 0x06003277 RID: 12919 RVA: 0x001B2860 File Offset: 0x001B0C60
		protected override IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.Name.Numerical, (!(p.Name is NameSingle)) ? 0 : ((NameSingle)p.Name).Number, p.def.label
			select p;
		}

		// Token: 0x06003278 RID: 12920 RVA: 0x001B28DC File Offset: 0x001B0CDC
		protected override IEnumerable<Pawn> PrimarySortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.RaceProps.petness descending, p.RaceProps.baseBodySize
			select p;
		}
	}
}
