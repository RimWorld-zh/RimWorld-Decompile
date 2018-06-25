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
