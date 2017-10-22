using Verse;

namespace RimWorld
{
	public class SpecialThingFilterWorker_Rotten : SpecialThingFilterWorker
	{
		public override bool Matches(Thing t)
		{
			ThingWithComps thingWithComps = t as ThingWithComps;
			bool result;
			if (thingWithComps == null)
			{
				result = false;
			}
			else
			{
				CompRottable comp = thingWithComps.GetComp<CompRottable>();
				result = (comp != null && !((CompProperties_Rottable)comp.props).rotDestroys && comp.Stage != RotStage.Fresh);
			}
			return result;
		}

		public override bool CanEverMatch(ThingDef def)
		{
			CompProperties_Rottable compProperties = def.GetCompProperties<CompProperties_Rottable>();
			return compProperties != null && !compProperties.rotDestroys;
		}
	}
}
