using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009CB RID: 2507
	public class StatWorker_SurgerySuccessChanceFactor : StatWorker
	{
		// Token: 0x06003824 RID: 14372 RVA: 0x001DE7E4 File Offset: 0x001DCBE4
		public override bool ShouldShowFor(StatRequest req)
		{
			bool result;
			if (!base.ShouldShowFor(req))
			{
				result = false;
			}
			else
			{
				BuildableDef def = req.Def;
				if (!(def is ThingDef))
				{
					result = false;
				}
				else
				{
					ThingDef thingDef = def as ThingDef;
					result = typeof(Building_Bed).IsAssignableFrom(thingDef.thingClass);
				}
			}
			return result;
		}
	}
}
