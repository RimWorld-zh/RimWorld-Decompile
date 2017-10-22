using UnityEngine;

namespace RimWorld
{
	public static class IncidentParmsUtility
	{
		public static PawnGroupMakerParms GetDefaultPawnGroupMakerParms(IncidentParms parms)
		{
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
			pawnGroupMakerParms.tile = parms.target.Tile;
			pawnGroupMakerParms.points = parms.points;
			pawnGroupMakerParms.faction = parms.faction;
			pawnGroupMakerParms.traderKind = parms.traderKind;
			pawnGroupMakerParms.generateFightersOnly = parms.generateFightersOnly;
			pawnGroupMakerParms.raidStrategy = parms.raidStrategy;
			pawnGroupMakerParms.forceOneIncap = parms.raidForceOneIncap;
			return pawnGroupMakerParms;
		}

		public static void AdjustPointsForGroupArrivalParams(IncidentParms parms)
		{
			if (parms.raidStrategy != null)
			{
				parms.points *= parms.raidStrategy.pointsFactor;
			}
			switch (parms.raidArrivalMode)
			{
			case PawnsArriveMode.EdgeWalkIn:
			{
				parms.points *= 1f;
				break;
			}
			case PawnsArriveMode.EdgeDrop:
			{
				parms.points *= 1f;
				break;
			}
			case PawnsArriveMode.CenterDrop:
			{
				parms.points *= 0.45f;
				break;
			}
			}
			if (parms.raidStrategy != null)
			{
				parms.points = Mathf.Max(parms.points, (float)(parms.raidStrategy.Worker.MinimumPoints(parms.faction) * 1.0499999523162842));
			}
		}
	}
}
