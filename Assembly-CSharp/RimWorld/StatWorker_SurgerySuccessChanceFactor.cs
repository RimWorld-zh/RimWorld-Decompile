using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C7 RID: 2503
	public class StatWorker_SurgerySuccessChanceFactor : StatWorker
	{
		// Token: 0x0600381E RID: 14366 RVA: 0x001DE9BC File Offset: 0x001DCDBC
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
