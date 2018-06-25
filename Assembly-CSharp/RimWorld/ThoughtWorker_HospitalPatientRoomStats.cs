using System;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_HospitalPatientRoomStats : ThoughtWorker
	{
		public ThoughtWorker_HospitalPatientRoomStats()
		{
		}

		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Building_Bed building_Bed = p.CurrentBed();
			ThoughtState result;
			if (building_Bed == null || !building_Bed.Medical)
			{
				result = ThoughtState.Inactive;
			}
			else
			{
				Room room = p.GetRoom(RegionType.Set_Passable);
				if (room == null || room.Role != RoomRoleDefOf.Hospital)
				{
					result = ThoughtState.Inactive;
				}
				else
				{
					int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(room.GetStat(RoomStatDefOf.Impressiveness));
					if (this.def.stages[scoreStageIndex] != null)
					{
						result = ThoughtState.ActiveAtStage(scoreStageIndex);
					}
					else
					{
						result = ThoughtState.Inactive;
					}
				}
			}
			return result;
		}
	}
}
