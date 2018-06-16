using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200067B RID: 1659
	public interface IConstructible
	{
		// Token: 0x060022D7 RID: 8919
		List<ThingDefCountClass> MaterialsNeeded();

		// Token: 0x060022D8 RID: 8920
		ThingDef UIStuff();
	}
}
