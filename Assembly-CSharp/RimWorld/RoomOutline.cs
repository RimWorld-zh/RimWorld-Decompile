using Verse;

namespace RimWorld
{
	public class RoomOutline
	{
		public CellRect rect;

		public int CellsCountIgnoringWalls
		{
			get
			{
				return (this.rect.Width > 2 && this.rect.Height > 2) ? ((this.rect.Width - 2) * (this.rect.Height - 2)) : 0;
			}
		}

		public RoomOutline(CellRect rect)
		{
			this.rect = rect;
		}
	}
}
