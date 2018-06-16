using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000731 RID: 1841
	public class CompProperties_RoomIdentifier : CompProperties
	{
		// Token: 0x06002894 RID: 10388 RVA: 0x0015A922 File Offset: 0x00158D22
		public CompProperties_RoomIdentifier()
		{
			this.compClass = typeof(CompRoomIdentifier);
		}

		// Token: 0x0400163F RID: 5695
		public RoomStatDef roomStat;
	}
}
