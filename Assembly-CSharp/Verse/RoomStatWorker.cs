using System;

namespace Verse
{
	public abstract class RoomStatWorker
	{
		public RoomStatDef def;

		protected RoomStatWorker()
		{
		}

		public abstract float GetScore(Room room);
	}
}
