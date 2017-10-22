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
					bool result;
					if (thing.Faction != Faction.OfPlayer || thing.IsForbidden(pawn) || (!allowedOutside && !thing.Position.Roofed(thing.Map)) || !pawn.CanReserveAndReach(thing, PathEndMode.Touch, Danger.None, 1, -1, null, false) || !thing.IsPoliticallyProper(pawn))
					{
						result = false;
					}
					else
					{
						CompArt compArt = thing.TryGetComp<CompArt>();
						if (compArt == null)
						{
							Log.Error("No CompArt on thing being considered for viewing: " + thing);
							result = false;
						}
						else if (!compArt.CanShowArt || !compArt.Props.canBeEnjoyedAsArt)
						{
							result = false;
						}
						else
						{
							Room room = thing.GetRoom(RegionType.Set_Passable);
							if (room == null)
							{
								result = false;
							}
							else
							{
								if (room.Role != RoomRoleDefOf.Bedroom && room.Role != RoomRoleDefOf.Barracks && room.Role != RoomRoleDefOf.PrisonCell && room.Role != RoomRoleDefOf.PrisonBarracks && room.Role != RoomRoleDefOf.Hospital)
								{
									goto IL_0167;
								}
								if (pawn.ownership != null && pawn.ownership.OwnedRoom != null && pawn.ownership.OwnedRoom == room)
								{
									goto IL_0167;
								}
								result = false;
							}
						}
					}
					goto IL_016e;
					IL_0167:
					result = true;
					goto IL_016e;
					IL_016e:
					return result;
				}));
				Thing t = default(Thing);
				if (!((IEnumerable<Thing>)JoyGiver_ViewArt.candidates).TryRandomElementByWeight<Thing>((Func<Thing, float>)((Thing target) => Mathf.Max(target.GetStatValue(StatDefOf.Beauty, true), 0.5f)), out t))
				{
					return null;
				}
				return new Job(base.def.jobDef, t);
			}
			finally
			{
				JoyGiver_ViewArt.candidates.Clear();
			}
		}
	}
}
