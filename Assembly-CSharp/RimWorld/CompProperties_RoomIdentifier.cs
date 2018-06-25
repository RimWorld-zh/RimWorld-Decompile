using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200072F RID: 1839
	public class CompProperties_RoomIdentifier : CompProperties
	{
		// Token: 0x0400163D RID: 5693
		public RoomStatDef roomStat;

		// Token: 0x06002893 RID: 10387 RVA: 0x0015ACDE File Offset: 0x001590DE
		public CompProperties_RoomIdentifier()
		{
			this.compClass = typeof(CompRoomIdentifier);
		}
	}
}
