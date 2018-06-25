using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000713 RID: 1811
	public static class ForbidUtility
	{
		// Token: 0x060027C5 RID: 10181 RVA: 0x00154AF8 File Offset: 0x00152EF8
		public static void SetForbidden(this Thing t, bool value, bool warnOnFail = true)
		{
			if (t == null)
			{
				if (warnOnFail)
				{
					Log.Error("Tried to SetForbidden on null Thing.", false);
				}
			}
			else
			{
				ThingWithComps thingWithComps = t as ThingWithComps;
				if (thingWithComps == null)
				{
					if (warnOnFail)
					{
						Log.Error("Tried to SetForbidden on non-ThingWithComps Thing " + t, false);
					}
				}
				else
				{
					CompForbiddable comp = thingWithComps.GetComp<CompForbiddable>();
					if (comp == null)
					{
						if (warnOnFail)
						{
							Log.Error("Tried to SetForbidden on non-Forbiddable Thing " + t, false);
						}
					}
					else
					{
						comp.Forbidden = value;
					}
				}
			}
		}

		// Token: 0x060027C6 RID: 10182 RVA: 0x00154B80 File Offset: 0x00152F80
		public static void SetForbiddenIfOutsideHomeArea(this Thing t)
		{
			if (!t.Spawned)
			{
				Log.Error("SetForbiddenIfOutsideHomeArea unspawned thing " + t, false);
			}
			if (t.Position.InBounds(t.Map) && !t.Map.areaManager.Home[t.Position])
			{
				t.SetForbidden(true, false);
			}
		}

		// Token: 0x060027C7 RID: 10183 RVA: 0x00154BE8 File Offset: 0x00152FE8
		public static bool CaresAboutForbidden(Pawn pawn, bool cellTarget)
		{
			if (pawn.HostFaction != null)
			{
				if (pawn.HostFaction != Faction.OfPlayer || !pawn.Spawned || pawn.Map.IsPlayerHome || (pawn.GetRoom(RegionType.Set_Passable) != null && pawn.GetRoom(RegionType.Set_Passable).isPrisonCell) || (pawn.IsPrisoner && !pawn.guest.PrisonerIsSecure))
				{
					return false;
				}
			}
			return !pawn.InMentalState && (!cellTarget || !ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn));
		}

		// Token: 0x060027C8 RID: 10184 RVA: 0x00154CA8 File Offset: 0x001530A8
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

		// Token: 0x060027C9 RID: 10185 RVA: 0x00154CFC File Offset: 0x001530FC
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
				result = (lord != null && lord.extraForbiddenThings.Contains(t));
			}
			return result;
		}

		// Token: 0x060027CA RID: 10186 RVA: 0x00154D98 File Offset: 0x00153198
		public static bool IsForbiddenToPass(this Building_Door t, Pawn pawn)
		{
			return ForbidUtility.CaresAboutForbidden(pawn, false) && t.IsForbidden(pawn.Faction);
		}

		// Token: 0x060027CB RID: 10187 RVA: 0x00154DDC File Offset: 0x001531DC
		public static bool IsForbidden(this IntVec3 c, Pawn pawn)
		{
			return ForbidUtility.CaresAboutForbidden(pawn, true) && (!c.InAllowedArea(pawn) || (pawn.mindState.maxDistToSquadFlag > 0f && !c.InHorDistOf(pawn.DutyLocation(), pawn.mindState.maxDistToSquadFlag)));
		}

		// Token: 0x060027CC RID: 10188 RVA: 0x00154E54 File Offset: 0x00153254
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
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060027CD RID: 10189 RVA: 0x00154ECC File Offset: 0x001532CC
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
