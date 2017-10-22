using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JoyGiver_ViewArt : JoyGiver
	{
		private static List<Thing> candidates = new List<Thing>();

		public override Job TryGiveJob(Pawn pawn)
		{
			bool allowedOutside = JoyUtility.EnjoyableOutsideNow(pawn, null);
			try
			{
				JoyGiver_ViewArt.candidates.AddRange(pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Art).Where((Func<Thing, bool>)delegate(Thing thing)
				{
					if (thing.Faction == Faction.OfPlayer && !thing.IsForbidden(pawn) && (allowedOutside || thing.Position.Roofed(thing.Map)) && pawn.CanReserveAndReach(thing, PathEndMode.Touch, Danger.None, 1, -1, null, false))
					{
						CompArt compArt = thing.TryGetComp<CompArt>();
						if (compArt == null)
						{
							Log.Error("No CompArt on thing being considered for viewing: " + thing);
							return false;
						}
						if (compArt.CanShowArt && compArt.Props.canBeEnjoyedAsArt)
						{
							Room room = thing.GetRoom(RegionType.Set_Passable);
							if (room == null)
							{
								return false;
							}
							if (room.Role != RoomRoleDefOf.Bedroom && room.Role != RoomRoleDefOf.Barracks && room.Role != RoomRoleDefOf.PrisonCell && room.Role != RoomRoleDefOf.PrisonBarracks && room.Role != RoomRoleDefOf.Hospital)
							{
								goto IL_0139;
							}
							if (pawn.ownership != null && pawn.ownership.OwnedRoom != null && pawn.ownership.OwnedRoom == room)
							{
								goto IL_0139;
							}
							return false;
						}
						return false;
					}
					return false;
					IL_0139:
					return true;
				}));
				Thing t = default(Thing);
				if (!((IEnumerable<Thing>)JoyGiver_ViewArt.candidates).TryRandomElementByWeight<Thing>((Func<Thing, float>)((Thing target) => Mathf.Max(target.GetStatValue(StatDefOf.Beauty, true), 0.5f)), out t))
				{
					return null;
				}
				return new Job(base.def.jobDef, t);
				IL_00a2:
				Job result;
				return result;
			}
			finally
			{
				JoyGiver_ViewArt.candidates.Clear();
			}
		}
	}
}
