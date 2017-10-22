using Verse;

namespace RimWorld
{
	public class StatWorker_SurgerySuccessChanceFactor : StatWorker
	{
		public override bool ShouldShowFor(BuildableDef eDef)
		{
			bool result;
			if (!base.ShouldShowFor(eDef))
			{
				result = false;
			}
			else if (!(eDef is ThingDef))
			{
				result = false;
			}
			else
			{
				ThingDef thingDef = eDef as ThingDef;
				result = ((byte)(typeof(Building_Bed).IsAssignableFrom(thingDef.thingClass) ? 1 : 0) != 0);
			}
			return result;
		}
	}
}
