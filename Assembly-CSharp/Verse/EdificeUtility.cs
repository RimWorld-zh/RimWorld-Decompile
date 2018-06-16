using System;

namespace Verse
{
	// Token: 0x02000C23 RID: 3107
	public static class EdificeUtility
	{
		// Token: 0x060043F1 RID: 17393 RVA: 0x0023C52C File Offset: 0x0023A92C
		public static bool IsEdifice(this BuildableDef def)
		{
			ThingDef thingDef = def as ThingDef;
			return thingDef != null && thingDef.category == ThingCategory.Building && thingDef.building.isEdifice;
		}
	}
}
