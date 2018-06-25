using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000730 RID: 1840
	public class CompRoomIdentifier : ThingComp
	{
		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x06002895 RID: 10389 RVA: 0x0015AD00 File Offset: 0x00159100
		private CompProperties_RoomIdentifier Props
		{
			get
			{
				return (CompProperties_RoomIdentifier)this.props;
			}
		}

		// Token: 0x06002896 RID: 10390 RVA: 0x0015AD20 File Offset: 0x00159120
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
