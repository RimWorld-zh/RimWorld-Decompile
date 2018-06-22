using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000677 RID: 1655
	public interface IConstructible
	{
		// Token: 0x060022D1 RID: 8913
		List<ThingDefCountClass> MaterialsNeeded();

		// Token: 0x060022D2 RID: 8914
		ThingDef UIStuff();
	}
}
