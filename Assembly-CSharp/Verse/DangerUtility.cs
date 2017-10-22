using RimWorld;

namespace Verse
{
	public static class DangerUtility
	{
		public static Danger NormalMaxDanger(this Pawn p)
		{
			return (Danger)((p.CurJob == null || !p.CurJob.playerForced) ? ((FloatMenuMakerMap.makingFor != p) ? ((p.Faction != Faction.OfPlayer) ? 2 : ((p.health.hediffSet.HasTemperatureInjury(TemperatureInjuryStage.Minor) && GenTemperature.FactionOwnsPassableRoomInTemperatureRange(p.Faction, p.SafeTemperatureRange(), p.MapHeld)) ? 1 : 2)) : 3) : 3);
		}

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
				result = ((region == null) ? Danger.None : region.DangerFor(p));
			}
			return result;
		}
	}
}
