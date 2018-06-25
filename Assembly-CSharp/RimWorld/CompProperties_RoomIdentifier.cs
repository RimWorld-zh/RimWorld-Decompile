using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200072F RID: 1839
	public class CompProperties_RoomIdentifier : CompProperties
	{
		// Token: 0x04001641 RID: 5697
		public RoomStatDef roomStat;

		// Token: 0x06002892 RID: 10386 RVA: 0x0015AF3E File Offset: 0x0015933E
		public CompProperties_RoomIdentifier()
		{
			this.compClass = typeof(CompRoomIdentifier);
		}
	}
}
