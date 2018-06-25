using System;

namespace Verse
{
	// Token: 0x02000C21 RID: 3105
	public static class EdificeUtility
	{
		// Token: 0x060043FB RID: 17403 RVA: 0x0023D9A8 File Offset: 0x0023BDA8
		public static bool IsEdifice(this BuildableDef def)
		{
			ThingDef thingDef = def as ThingDef;
			return thingDef != null && thingDef.category == ThingCategory.Building && thingDef.building.isEdifice;
		}
	}
}
