using Verse;

namespace RimWorld
{
	public class ThoughtWorker_HospitalPatientRoomStats : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Building_Bed building_Bed = p.CurrentBed();
			if (building_Bed != null && building_Bed.Medical)
			{
				Room room = p.GetRoom(RegionType.Set_Passable);
				if (room != null && room.Role == RoomRoleDefOf.Hospital)
				{
					int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(room.GetStat(RoomStatDefOf.Impressiveness));
					if (base.def.stages[scoreStageIndex] != null)
					{
						return ThoughtState.ActiveAtStage(scoreStageIndex);
					}
					return ThoughtState.Inactive;
				}
				return ThoughtState.Inactive;
			}
			return ThoughtState.Inactive;
		}
	}
}
