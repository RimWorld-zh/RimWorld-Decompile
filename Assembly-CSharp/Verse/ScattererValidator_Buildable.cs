namespace Verse
{
	public class ScattererValidator_Buildable : ScattererValidator
	{
		public int radius = 1;

		public TerrainAffordance affordance = TerrainAffordance.Heavy;

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
						if (!c2.InBounds(map))
							goto IL_0040;
						if (c2.InNoBuildEdgeArea(map))
							goto IL_0054;
						if (!c2.GetTerrain(map).affordances.Contains(this.affordance))
							goto IL_0078;
					}
					num++;
					continue;
				}
				result = true;
				break;
				IL_0078:
				result = false;
				break;
				IL_0040:
				result = false;
				break;
				IL_0054:
				result = false;
				break;
			}
			return result;
		}
	}
}
