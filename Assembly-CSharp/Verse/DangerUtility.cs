using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020009E5 RID: 2533
	public static class DangerUtility
	{
		// Token: 0x060038C9 RID: 14537 RVA: 0x001E4A2C File Offset: 0x001E2E2C
		public static Danger NormalMaxDanger(this Pawn p)
		{
			Danger result;
			if (p.CurJob != null && p.CurJob.playerForced)
			{
				result = Danger.Deadly;
			}
			else if (FloatMenuMakerMap.makingFor == p)
			{
				result = Danger.Deadly;
			}
			else if (p.Faction == Faction.OfPlayer)
			{
				if (p.health.hediffSet.HasTemperatureInjury(TemperatureInjuryStage.Minor))
				{
					if (GenTemperature.FactionOwnsPassableRoomInTemperatureRange(p.Faction, p.SafeTemperatureRange(), p.MapHeld))
					{
						return Danger.None;
					}
				}
				result = Danger.Some;
			}
			else
			{
				result = Danger.Some;
			}
			return result;
		}

		// Token: 0x060038CA RID: 14538 RVA: 0x001E4ACC File Offset: 0x001E2ECC
		public static Danger GetDangerFor(this IntVec3 c, Pawn p, Map map)
		{
			Map mapHeld = p.MapHeld;
			Danger result;
			if (mapHeld == null || mapHeld != map)
			{
				result = Danger.None;
			}
			else
			{
				Region region = c.GetRegion(mapHeld, RegionType.Set_All);
				if (region == null)
				{
					result = Danger.None;
				}
				else
				{
					result = region.DangerFor(p);
				}
			}
			return result;
		}
	}
}
