using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000679 RID: 1657
	public interface IConstructible
	{
		// Token: 0x060022D4 RID: 8916
		List<ThingDefCountClass> MaterialsNeeded();

		// Token: 0x060022D5 RID: 8917
		ThingDef UIStuff();
	}
}
