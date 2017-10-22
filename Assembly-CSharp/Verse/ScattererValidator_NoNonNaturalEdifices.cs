namespace Verse
{
	public class ScattererValidator_NoNonNaturalEdifices : ScattererValidator
	{
		public int radius = 1;

		public override bool Allows(IntVec3 c, Map map)
		{
			CellRect cellRect = CellRect.CenteredOn(c, this.radius);
			int num = cellRect.minZ;
			bool result;
			while (true)
			{
				if (num <= cellRect.maxZ)
				{
					for (int i = cellRect.minX; i <= cellRect.maxX; i++)
					{
						IntVec3 c2 = new IntVec3(i, 0, num);
						if (c2.GetEdifice(map) != null)
							goto IL_0040;
					}
					num++;
					continue;
				}
				result = true;
				break;
				IL_0040:
				result = false;
				break;
			}
			return result;
		}
	}
}
