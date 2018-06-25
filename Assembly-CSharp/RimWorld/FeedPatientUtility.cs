using System;
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
				if (p.NonHumanlikeOrWildMan())
				{
					Building_Bed building_Bed = p.CurrentBed();
					if (building_Bed == null || building_Bed.Faction != Faction.OfPlayer)
					{
						return false;
					}
				}
				else
				{
					if (p.Faction != Faction.OfPlayer && p.HostFaction != Faction.OfPlayer)
					{
						return false;
					}
					if (!p.InBed())
					{
						return false;
					}
				}
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
							return false;
						}
						if (p.guest != null && !p.guest.CanBeBroughtFood)
						{
							return false;
						}
					}
					result = true;
				}
			}
			return result;
		}
	}
}
