using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000731 RID: 1841
	public class CompProperties_RoomIdentifier : CompProperties
	{
		// Token: 0x06002896 RID: 10390 RVA: 0x0015A9B6 File Offset: 0x00158DB6
		public CompProperties_RoomIdentifier()
		{
			this.compClass = typeof(CompRoomIdentifier);
		}

		// Token: 0x0400163F RID: 5695
		public RoomStatDef roomStat;
	}
}
