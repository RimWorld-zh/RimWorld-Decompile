using System;
using Verse;

namespace RimWorld
{
	public class RoomOutline
	{
		public CellRect rect;

		public RoomOutline(CellRect rect)
		{
			this.rect = rect;
		}

		public int CellsCountIgnoringWalls
		{
			get
			{
				int result;
				if (this.rect.Width <= 2 || this.rect.Height <= 2)
				{
					result = 0;
				}
				else
				{
					result = (this.rect.Width - 2) * (this.rect.Height - 2);
				}
				return result;
			}
		}
	}
}
