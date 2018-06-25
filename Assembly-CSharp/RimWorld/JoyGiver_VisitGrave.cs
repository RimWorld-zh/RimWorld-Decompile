using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JoyGiver_VisitGrave : JoyGiver
	{
		public JoyGiver_VisitGrave()
		{
		}

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

		[CompilerGenerated]
		private sealed class <TryGiveJob>c__AnonStorey0
		{
			internal Pawn pawn;

			internal bool allowedOutside;

			public <TryGiveJob>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing x)
			{
				Building_Grave building_Grave = (Building_Grave)x;
				return x.Faction == Faction.OfPlayer && building_Grave.HasCorpse && !building_Grave.IsForbidden(this.pawn) && building_Grave.Corpse.InnerPawn.Faction == Faction.OfPlayer && (this.allowedOutside || building_Grave.Position.Roofed(building_Grave.Map)) && this.pawn.CanReserveAndReach(x, PathEndMode.Touch, Danger.None, 1, -1, null, false) && building_Grave.IsPoliticallyProper(this.pawn);
			}

			internal float <>m__1(Thing x)
			{
				float lengthHorizontal = (x.Position - this.pawn.Position).LengthHorizontal;
				return Mathf.Max(150f - lengthHorizontal, 5f);
			}
		}
	}
}
