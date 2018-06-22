using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200072E RID: 1838
	public class CompRoomIdentifier : ThingComp
	{
		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x06002891 RID: 10385 RVA: 0x0015ABB0 File Offset: 0x00158FB0
		private CompProperties_RoomIdentifier Props
		{
			get
			{
				return (CompProperties_RoomIdentifier)this.props;
			}
		}

		// Token: 0x06002892 RID: 10386 RVA: 0x0015ABD0 File Offset: 0x00158FD0
		public override string CompInspectStringExtra()
		{
			Room room = this.parent.GetRoom(RegionType.Set_All);
			string str;
			if (room == null || room.Role == RoomRoleDefOf.None)
			{
				str = "Outdoors".Translate();
			}
			else
			{
				str = string.Concat(new string[]
				{
					room.Role.LabelCap,
					" (",
					room.GetStatScoreStage(this.Props.roomStat).label,
					", ",
					this.Props.roomStat.ScoreToString(room.GetStat(this.Props.roomStat)),
					")"
				});
			}
			return "Room".Translate() + ": " + str;
		}
	}
}
