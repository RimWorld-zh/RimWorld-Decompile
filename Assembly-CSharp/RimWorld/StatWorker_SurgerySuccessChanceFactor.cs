using System;
using Verse;

namespace RimWorld
{
	public class StatWorker_SurgerySuccessChanceFactor : StatWorker
	{
		public StatWorker_SurgerySuccessChanceFactor()
		{
		}

		public override bool ShouldShowFor(StatRequest req)
		{
			if (!base.ShouldShowFor(req))
			{
				return false;
			}
			BuildableDef def = req.Def;
			if (!(def is ThingDef))
			{
				return false;
			}
			ThingDef thingDef = def as ThingDef;
			return typeof(Building_Bed).IsAssignableFrom(thingDef.thingClass);
		}
	}
}
