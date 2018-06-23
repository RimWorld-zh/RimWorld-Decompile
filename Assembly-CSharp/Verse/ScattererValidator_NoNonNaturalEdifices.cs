using System;

namespace Verse
{
	// Token: 0x02000C5D RID: 3165
	public class ScattererValidator_NoNonNaturalEdifices : ScattererValidator
	{
		// Token: 0x04002FA1 RID: 12193
		public int radius = 1;

		// Token: 0x060045A9 RID: 17833 RVA: 0x0024CDE8 File Offset: 0x0024B1E8
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
