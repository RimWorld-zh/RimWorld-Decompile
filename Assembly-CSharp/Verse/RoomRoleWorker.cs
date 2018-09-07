using System;

namespace Verse
{
	public abstract class RoomRoleWorker
	{
		protected RoomRoleWorker()
		{
		}

		public abstract float GetScore(Room room);
	}
}
