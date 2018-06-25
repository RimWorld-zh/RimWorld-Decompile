using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000713 RID: 1811
	public static class ForbidUtility
	{
		// Token: 0x060027C4 RID: 10180 RVA: 0x00154D58 File Offset: 0x00153158
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

		// Token: 0x060027C5 RID: 10181 RVA: 0x00154DE0 File Offset: 0x001531E0
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

		// Token: 0x060027C6 RID: 10182 RVA: 0x00154E48 File Offset: 0x00153248
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

		// Token: 0x060027C7 RID: 10183 RVA: 0x00154F08 File Offset: 0x00153308
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

		// Token: 0x060027C8 RID: 10184 RVA: 0x00154F5C File Offset: 0x0015335C
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

		// Token: 0x060027C9 RID: 10185 RVA: 0x00154FF8 File Offset: 0x001533F8
		public static bool IsForbiddenToPass(this Building_Door t, Pawn pawn)
		{
			return ForbidUtility.CaresAboutForbidden(pawn, false) && t.IsForbidden(pawn.Faction);
		}

		// Token: 0x060027CA RID: 10186 RVA: 0x0015503C File Offset: 0x0015343C
		public static bool IsForbidden(this IntVec3 c, Pawn pawn)
		{
			return ForbidUtility.CaresAboutForbidden(pawn, true) && (!c.InAllowedArea(pawn) || (pawn.mindState.maxDistToSquadFlag > 0f && !c.InHorDistOf(pawn.DutyLocation(), pawn.mindState.maxDistToSquadFlag)));
		}

		// Token: 0x060027CB RID: 10187 RVA: 0x001550B4 File Offset: 0x001534B4
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

		// Token: 0x060027CC RID: 10188 RVA: 0x0015512C File Offset: 0x0015352C
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
