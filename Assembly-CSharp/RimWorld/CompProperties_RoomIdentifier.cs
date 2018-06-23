using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200072D RID: 1837
	public class CompProperties_RoomIdentifier : CompProperties
	{
		// Token: 0x0400163D RID: 5693
		public RoomStatDef roomStat;

		// Token: 0x0600288F RID: 10383 RVA: 0x0015AB8E File Offset: 0x00158F8E
		public CompProperties_RoomIdentifier()
		{
			this.compClass = typeof(CompRoomIdentifier);
		}
	}
}
