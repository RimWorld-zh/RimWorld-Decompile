using Verse;

namespace RimWorld
{
	public static class FeedPatientUtility
	{
		public static bool ShouldBeFed(Pawn p)
		{
			bool result;
			if (p.GetPosture() == PawnPosture.Standing)
			{
				result = false;
			}
			else
			{
				if (p.RaceProps.Humanlike)
				{
					if (p.Faction != Faction.OfPlayer && p.HostFaction != Faction.OfPlayer)
					{
						result = false;
						goto IL_00ff;
					}
					if (!p.InBed())
					{
						result = false;
						goto IL_00ff;
					}
					goto IL_0089;
				}
				Building_Bed building_Bed = p.CurrentBed();
				if (building_Bed != null && building_Bed.Faction == Faction.OfPlayer)
				{
					goto IL_0089;
				}
				result = false;
			}
			goto IL_00ff;
			IL_00ff:
			return result;
			IL_0089:
			if (!p.RaceProps.EatsFood)
			{
				result = false;
			}
			else if (!HealthAIUtility.ShouldSeekMedicalRest(p))
			{
				result = false;
			}
			else
			{
				if (p.HostFaction != null)
				{
					if (p.HostFaction != Faction.OfPlayer)
					{
						result = false;
						goto IL_00ff;
					}
					if (p.guest != null && !p.guest.CanBeBroughtFood)
					{
						result = false;
						goto IL_00ff;
					}
				}
				result = true;
			}
			goto IL_00ff;
		}
	}
}
