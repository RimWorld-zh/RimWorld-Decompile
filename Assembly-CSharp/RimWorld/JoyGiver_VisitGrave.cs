using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JoyGiver_VisitGrave : JoyGiver
	{
		public override Job TryGiveJob(Pawn pawn)
		{
			bool allowedOutside = JoyUtility.EnjoyableOutsideNow(pawn, null);
			IEnumerable<Thing> source = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Grave).Where((Func<Thing, bool>)delegate(Thing x)
			{
				Building_Grave building_Grave = (Building_Grave)x;
				return x.Faction == Faction.OfPlayer && building_Grave.HasCorpse && !building_Grave.IsForbidden(pawn) && building_Grave.Corpse.InnerPawn.Faction == Faction.OfPlayer && (allowedOutside || building_Grave.Position.Roofed(building_Grave.Map)) && pawn.CanReserveAndReach(x, PathEndMode.Touch, Danger.None, 1, -1, null, false) && building_Grave.IsPoliticallyProper(pawn);
			});
			Thing t = default(Thing);
			return source.TryRandomElementByWeight<Thing>((Func<Thing, float>)delegate(Thing x)
			{
				float lengthHorizontal = (x.Position - pawn.Position).LengthHorizontal;
				return Mathf.Max((float)(150.0 - lengthHorizontal), 5f);
			}, out t) ? new Job(base.def.jobDef, t) : null;
		}
	}
}
