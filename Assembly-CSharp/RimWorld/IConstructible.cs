using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200067B RID: 1659
	public interface IConstructible
	{
		// Token: 0x060022D9 RID: 8921
		List<ThingDefCountClass> MaterialsNeeded();

		// Token: 0x060022DA RID: 8922
		ThingDef UIStuff();
	}
}
