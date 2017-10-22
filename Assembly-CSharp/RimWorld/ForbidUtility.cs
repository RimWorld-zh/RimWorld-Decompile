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
			return (byte)((pawn.HostFaction == null || (pawn.HostFaction == Faction.OfPlayer && pawn.Spawned && !pawn.Map.IsPlayerHome && (pawn.GetRoom(RegionType.Set_Passable) == null || !pawn.GetRoom(RegionType.Set_Passable).isPrisonCell) && (!pawn.IsPrisoner || pawn.guest.PrisonerIsSecure))) ? ((!pawn.InMentalState) ? ((!cellTarget || !ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn)) ? 1 : 0) : 0) : 0) != 0;
		}

		public static bool InAllowedArea(this IntVec3 c, Pawn forPawn)
		{
			bool result;
			if (forPawn.playerSettings != null)
			{
				Area effectiveAreaRestrictionInPawnCurrentMap = forPawn.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap;
				if (effectiveAreaRestrictionInPawnCurrentMap != null && effectiveAreaRestrictionInPawnCurrentMap.TrueCount > 0 && !effectiveAreaRestrictionInPawnCurrentMap[c])
				{
					result = false;
					goto IL_0046;
				}
			}
			result = true;
			goto IL_0046;
			IL_0046:
			return result;
		}

		public static bool IsForbidden(this Thing t, Pawn pawn)
		{
			bool result;
			if (!ForbidUtility.CaresAboutForbidden(pawn, false))
			{
				result = false;
			}
			else if (t.Spawned && t.Position.IsForbidden(pawn))
			{
				result = true;
			}
			else if (t.IsForbidden(pawn.Faction) || t.IsForbidden(pawn.HostFaction))
			{
				result = true;
			}
			else
			{
				Lord lord = pawn.GetLord();
				result = ((byte)((lord != null && lord.extraForbiddenThings.Contains(t)) ? 1 : 0) != 0);
			}
			return result;
		}

		public static bool IsForbiddenToPass(this Thing t, Pawn pawn)
		{
			return (byte)(ForbidUtility.CaresAboutForbidden(pawn, false) ? ((t.Spawned && t.Position.IsForbidden(pawn) && !(t is Building_Door)) ? 1 : (t.IsForbidden(pawn.Faction) ? 1 : 0)) : 0) != 0;
		}

		public static bool IsForbidden(this IntVec3 c, Pawn pawn)
		{
			return (byte)(ForbidUtility.CaresAboutForbidden(pawn, true) ? ((!c.InAllowedArea(pawn)) ? 1 : ((pawn.mindState.maxDistToSquadFlag > 0.0 && !c.InHorDistOf(pawn.DutyLocation(), pawn.mindState.maxDistToSquadFlag)) ? 1 : 0)) : 0) != 0;
		}

		public static bool IsForbiddenEntirely(this Region r, Pawn pawn)
		{
			bool result;
			if (!ForbidUtility.CaresAboutForbidden(pawn, true))
			{
				result = false;
			}
			else
			{
				if (pawn.playerSettings != null)
				{
					Area effectiveAreaRestriction = pawn.playerSettings.EffectiveAreaRestriction;
					if (effectiveAreaRestriction != null && effectiveAreaRestriction.TrueCount > 0 && effectiveAreaRestriction.Map == r.Map && r.OverlapWith(effectiveAreaRestriction) == AreaOverlap.None)
					{
						result = true;
						goto IL_006a;
					}
				}
				result = false;
			}
			goto IL_006a;
			IL_006a:
			return result;
		}

		public static bool IsForbidden(this Thing t, Faction faction)
		{
			bool result;
			if (faction == null)
			{
				result = false;
			}
			else if (faction != Faction.OfPlayer)
			{
				result = false;
			}
			else
			{
				ThingWithComps thingWithComps = t as ThingWithComps;
				if (thingWithComps == null)
				{
					result = false;
				}
				else
				{
					CompForbiddable comp = thingWithComps.GetComp<CompForbiddable>();
					result = (comp != null && comp.Forbidden);
				}
			}
			return result;
		}
	}
}
