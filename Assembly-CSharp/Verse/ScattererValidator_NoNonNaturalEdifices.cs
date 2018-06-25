using System;

namespace Verse
{
	// Token: 0x02000C60 RID: 3168
	public class ScattererValidator_NoNonNaturalEdifices : ScattererValidator
	{
		// Token: 0x04002FA8 RID: 12200
		public int radius = 1;

		// Token: 0x060045AC RID: 17836 RVA: 0x0024D1A4 File Offset: 0x0024B5A4
		public override bool Allows(IntVec3 c, Map map)
		{
			CellRect cellRect = CellRect.CenteredOn(c, this.radius);
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c2 = new IntVec3(j, 0, i);
					if (c2.GetEdifice(map) != null)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
