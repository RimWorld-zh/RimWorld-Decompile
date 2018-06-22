using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000104 RID: 260
	public class JoyGiver_VisitGrave : JoyGiver
	{
		// Token: 0x06000575 RID: 1397 RVA: 0x0003B4CC File Offset: 0x000398CC
		public override Job TryGiveJob(Pawn pawn)
		{
			bool allowedOutside = JoyUtility.EnjoyableOutsideNow(pawn, null);
			IEnumerable<Thing> source = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Grave).Where(delegate(Thing x)
			{
				Building_Grave building_Grave = (Building_Grave)x;
				return x.Faction == Faction.OfPlayer && building_Grave.HasCorpse && !building_Grave.IsForbidden(pawn) && building_Grave.Corpse.InnerPawn.Faction == Faction.OfPlayer && (allowedOutside || building_Grave.Position.Roofed(building_Grave.Map)) && pawn.CanReserveAndReach(x, PathEndMode.Touch, Danger.None, 1, -1, null, false) && building_Grave.IsPoliticallyProper(pawn);
			});
			Thing t;
			Job result;
			if (!source.TryRandomElementByWeight(delegate(Thing x)
			{
				float lengthHorizontal = (x.Position - pawn.Position).LengthHorizontal;
				return Mathf.Max(150f - lengthHorizontal, 5f);
			}, out t))
			{
				result = null;
			}
			else
			{
				result = new Job(this.def.jobDef, t);
			}
			return result;
		}
	}
}
