using Verse;

namespace RimWorld
{
	public class StatWorker_SurgerySuccessChanceFactor : StatWorker
	{
		public override bool ShouldShowFor(BuildableDef eDef)
		{
			if (!base.ShouldShowFor(eDef))
			{
				return false;
			}
			if (!(eDef is ThingDef))
			{
				return false;
			}
			ThingDef thingDef = eDef as ThingDef;
			if (typeof(Building_Bed).IsAssignableFrom(thingDef.thingClass))
			{
				return true;
			}
			return false;
		}
	}
}
