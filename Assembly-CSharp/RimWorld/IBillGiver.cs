using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000028 RID: 40
	public interface IBillGiver
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000172 RID: 370
		Map Map { get; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000173 RID: 371
		BillStack BillStack { get; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000174 RID: 372
		IEnumerable<IntVec3> IngredientStackCells { get; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000175 RID: 373
		string LabelShort { get; }

		// Token: 0x06000176 RID: 374
		bool CurrentlyUsableForBills();

		// Token: 0x06000177 RID: 375
		bool UsableForBillsAfterFueling();
	}
}
