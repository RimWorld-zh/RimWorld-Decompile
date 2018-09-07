using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class PlaceWorker_DoorLearnOpeningSpeed : PlaceWorker
	{
		[CompilerGenerated]
		private static Func<Thing, bool> <>f__am$cache0;

		public PlaceWorker_DoorLearnOpeningSpeed()
		{
		}

		public override void PostPlace(Map map, BuildableDef def, IntVec3 loc, Rot4 rot)
		{
			Blueprint_Door blueprint_Door = (Blueprint_Door)loc.GetThingList(map).FirstOrDefault((Thing t) => t is Blueprint_Door);
			if (blueprint_Door != null && blueprint_Door.def.entityDefToBuild.GetStatValueAbstract(StatDefOf.DoorOpenSpeed, blueprint_Door.stuffToUse) < 0.65f)
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.DoorOpenSpeed, OpportunityType.Important);
			}
		}

		[CompilerGenerated]
		private static bool <PostPlace>m__0(Thing t)
		{
			return t is Blueprint_Door;
		}
	}
}
