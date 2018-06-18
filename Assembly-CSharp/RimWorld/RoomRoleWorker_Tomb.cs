using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000439 RID: 1081
	public class RoomRoleWorker_Tomb : RoomRoleWorker
	{
		// Token: 0x060012D3 RID: 4819 RVA: 0x000A2AEC File Offset: 0x000A0EEC
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				if (containedAndAdjacentThings[i] is Building_Sarcophagus)
				{
					num++;
				}
			}
			return 50f * (float)num;
		}
	}
}
