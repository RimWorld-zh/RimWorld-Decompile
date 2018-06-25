using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C9 RID: 2505
	public class StatWorker_SurgerySuccessChanceFactor : StatWorker
	{
		// Token: 0x06003822 RID: 14370 RVA: 0x001DEDD4 File Offset: 0x001DD1D4
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
