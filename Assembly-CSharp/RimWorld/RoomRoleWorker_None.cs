using System;
using Verse;

namespace RimWorld
{
	public class RoomRoleWorker_None : RoomRoleWorker
	{
		public RoomRoleWorker_None()
		{
		}

		public override float GetScore(Room room)
		{
			return -1f;
		}
	}
}
