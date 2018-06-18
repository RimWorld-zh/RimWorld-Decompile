using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000732 RID: 1842
	public class CompRoomIdentifier : ThingComp
	{
		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x06002898 RID: 10392 RVA: 0x0015A9D8 File Offset: 0x00158DD8
		private CompProperties_RoomIdentifier Props
		{
			get
			{
				return (CompProperties_RoomIdentifier)this.props;
			}
		}

		// Token: 0x06002899 RID: 10393 RVA: 0x0015A9F8 File Offset: 0x00158DF8
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
