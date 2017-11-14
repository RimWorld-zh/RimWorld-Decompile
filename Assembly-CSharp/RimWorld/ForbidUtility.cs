using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public static class ForbidUtility
	{
		public static void SetForbidden(this Thing t, bool value, bool warnOnFail = true)
		{
			if (t == null)
			{
				if (warnOnFail)
				{
					Log.Error("Tried to SetForbidden on null Thing.");
				}
			}
			else
			{
				ThingWithComps thingWithComps = t as ThingWithComps;
				if (thingWithComps == null)
				{
					if (warnOnFail)
					{
						Log.Error("Tried to SetForbidden on non-ThingWithComps Thing " + t);
					}
				}
				else
				{
					CompForbiddable comp = thingWithComps.GetComp<CompForbiddable>();
					if (comp == null)
					{
						if (warnOnFail)
						{
							Log.Error("Tried to SetForbidden on non-Forbiddable Thing " + t);
						}
					}
					else
					{
						comp.Forbidden = value;
					}
				}
			}
		}

		public static void SetForbiddenIfOutsideHomeArea(this Thing t)
		{
			if (!t.Spawned)
			{
				Log.Error("SetForbiddenIfOutsideHomeArea unspawned thing " + t);
			}
			if (t.Position.InBounds(t.Map) && !((Area)t.Map.areaManager.Home)[t.Position])
			{
				t.SetForbidden(true, false);
			}
		}

		public static bool CaresAboutForbidden(Pawn pawn, bool cellTarget)
		{
			if (pawn.HostFaction != null && (pawn.HostFaction != Faction.OfPlayer || !pawn.Spawned || pawn.Map.IsPlayerHome || (pawn.GetRoom(RegionType.Set_Passable) != null && pawn.GetRoom(RegionType.Set_Passable).isPrisonCell) || (pawn.IsPrisoner && !pawn.guest.PrisonerIsSecure)))
			{
				return false;
			}
			if (pawn.InMentalState)
			{
				return false;
			}
			if (cellTarget && ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn))
			{
				return false;
			}
			return true;
		}

		public static bool InAllowedArea(this IntVec3 c, Pawn forPawn)
		{
			if (forPawn.playerSettings != null)
			{
				Area effectiveAreaRestrictionInPawnCurrentMap = forPawn.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap;
				if (effectiveAreaRestrictionInPawnCurrentMap != null && effectiveAreaRestrictionInPawnCurrentMap.TrueCount > 0 && !effectiveAreaRestrictionInPawnCurrentMap[c])
				{
					return false;
				}
			}
			return true;
		}

		public static bool IsForbidden(this Thing t, Pawn pawn)
		{
			if (!ForbidUtility.CaresAboutForbidden(pawn, false))
			{
				return false;
			}
			if (t.Spawned && t.Position.IsForbidden(pawn))
			{
				return true;
			}
			if (!t.IsForbidden(pawn.Faction) && !t.IsForbidden(pawn.HostFaction))
			{
				Lord lord = pawn.GetLord();
				if (lord != null && lord.extraForbiddenThings.Contains(t))
				{
					return true;
				}
				return false;
			}
			return true;
		}

		public static bool IsForbiddenToPass(this Thing t, Pawn pawn)
		{
			if (!ForbidUtility.CaresAboutForbidden(pawn, false))
			{
				return false;
			}
			if (t.Spawned && t.Position.IsForbidden(pawn) && !(t is Building_Door))
			{
				return true;
			}
			if (t.IsForbidden(pawn.Faction))
			{
				return true;
			}
			return false;
		}

		public static bool IsForbidden(this IntVec3 c, Pawn pawn)
		{
			if (!ForbidUtility.CaresAboutForbidden(pawn, true))
			{
				return false;
			}
			if (!c.InAllowedArea(pawn))
			{
				return true;
			}
			if (pawn.mindState.maxDistToSquadFlag > 0.0 && !c.InHorDistOf(pawn.DutyLocation(), pawn.mindState.maxDistToSquadFlag))
			{
				return true;
			}
			return false;
		}

		public static bool IsForbiddenEntirely(this Region r, Pawn pawn)
		{
			if (!ForbidUtility.CaresAboutForbidden(pawn, true))
			{
				return false;
			}
			if (pawn.playerSettings != null)
			{
				Area effectiveAreaRestriction = pawn.playerSettings.EffectiveAreaRestriction;
				if (effectiveAreaRestriction != null && effectiveAreaRestriction.TrueCount > 0 && effectiveAreaRestriction.Map == r.Map && r.OverlapWith(effectiveAreaRestriction) == AreaOverlap.None)
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsForbidden(this Thing t, Faction faction)
		{
			if (faction == null)
			{
				return false;
			}
			if (faction != Faction.OfPlayer)
			{
				return false;
			}
			ThingWithComps thingWithComps = t as ThingWithComps;
			if (thingWithComps == null)
			{
				return false;
			}
			CompForbiddable comp = thingWithComps.GetComp<CompForbiddable>();
			if (comp == null)
			{
				return false;
			}
			return comp.Forbidden;
		}
	}
}
