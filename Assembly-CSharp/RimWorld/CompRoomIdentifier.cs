using Verse;

namespace RimWorld
{
	public class CompRoomIdentifier : ThingComp
	{
		private CompProperties_RoomIdentifier Props
		{
			get
			{
				return (CompProperties_RoomIdentifier)base.props;
			}
		}

		public override string CompInspectStringExtra()
		{
			Room room = base.parent.GetRoom(RegionType.Set_All);
			string str = (room != null && room.Role != RoomRoleDefOf.None) ? (room.Role.LabelCap + " (" + room.GetStatScoreStage(this.Props.roomStat).label + ", " + this.Props.roomStat.ScoreToString(room.GetStat(this.Props.roomStat)) + ")") : "Outdoors".Translate();
			return "Room".Translate() + ": " + str;
		}
	}
}
