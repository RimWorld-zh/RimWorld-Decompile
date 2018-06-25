using System;
using Verse;

namespace RimWorld
{
	public class RoomRoleWorker_Room : RoomRoleWorker
	{
		public RoomRoleWorker_Room()
		{
		}

		public override float GetScore(Room room)
		{
			return 0.99f;
		}
	}
}
