using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000679 RID: 1657
	public interface IConstructible
	{
		// Token: 0x060022D5 RID: 8917
		List<ThingDefCountClass> MaterialsNeeded();

		// Token: 0x060022D6 RID: 8918
		ThingDef UIStuff();
	}
}
