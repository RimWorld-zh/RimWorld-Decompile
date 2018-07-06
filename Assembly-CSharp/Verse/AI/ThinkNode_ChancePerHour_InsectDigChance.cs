using System;

namespace Verse.AI
{
	public class ThinkNode_ChancePerHour_InsectDigChance : ThinkNode_ChancePerHour
	{
		private const float BaseMtbHours = 18f;

		public ThinkNode_ChancePerHour_InsectDigChance()
		{
		}

		protected override float MtbHours(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_Passable);
			float result;
			if (room == null)
			{
				result = 18f;
			}
			else
			{
				int num = (!room.IsHuge) ? room.CellCount : 9999;
				float num2 = GenMath.LerpDoubleClamped(2f, 25f, 6f, 1f, (float)num);
				result = 18f / num2;
			}
			return result;
		}
	}
}
