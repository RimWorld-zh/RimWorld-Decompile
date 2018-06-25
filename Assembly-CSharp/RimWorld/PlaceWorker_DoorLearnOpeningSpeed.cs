using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C72 RID: 3186
	public class PlaceWorker_DoorLearnOpeningSpeed : PlaceWorker
	{
		// Token: 0x060045E5 RID: 17893 RVA: 0x0024E134 File Offset: 0x0024C534
		public override void PostPlace(Map map, BuildableDef def, IntVec3 loc, Rot4 rot)
		{
			Blueprint_Door blueprint_Door = (Blueprint_Door)loc.GetThingList(map).FirstOrDefault((Thing t) => t is Blueprint_Door);
			if (blueprint_Door != null && blueprint_Door.def.entityDefToBuild.GetStatValueAbstract(StatDefOf.DoorOpenSpeed, blueprint_Door.stuffToUse) < 0.65f)
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.DoorOpenSpeed, OpportunityType.Important);
			}
		}
	}
}
