using System;

namespace Verse
{
	// Token: 0x02000C1F RID: 3103
	public static class EdificeUtility
	{
		// Token: 0x060043F8 RID: 17400 RVA: 0x0023D8CC File Offset: 0x0023BCCC
		public static bool IsEdifice(this BuildableDef def)
		{
			ThingDef thingDef = def as ThingDef;
			return thingDef != null && thingDef.category == ThingCategory.Building && thingDef.building.isEdifice;
		}
	}
}
