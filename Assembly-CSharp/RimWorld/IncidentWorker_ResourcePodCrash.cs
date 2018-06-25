using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200033F RID: 831
	public class IncidentWorker_ResourcePodCrash : IncidentWorker
	{
		// Token: 0x06000E2E RID: 3630 RVA: 0x00078BA8 File Offset: 0x00076FA8
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			List<Thing> things = ThingSetMakerDefOf.ResourcePod.root.Generate();
			IntVec3 intVec = DropCellFinder.RandomDropSpot(map);
			DropPodUtility.DropThingsNear(intVec, map, things, 110, false, true, true, true);
			Find.LetterStack.ReceiveLetter("LetterLabelCargoPodCrash".Translate(), "CargoPodCrash".Translate(), LetterDefOf.PositiveEvent, new TargetInfo(intVec, map, false), null, null);
			return true;
		}
	}
}
