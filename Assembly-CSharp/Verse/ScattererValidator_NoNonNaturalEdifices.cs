using System;

namespace Verse
{
	// Token: 0x02000C61 RID: 3169
	public class ScattererValidator_NoNonNaturalEdifices : ScattererValidator
	{
		// Token: 0x060045A2 RID: 17826 RVA: 0x0024BA40 File Offset: 0x00249E40
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

		// Token: 0x04002F99 RID: 12185
		public int radius = 1;
	}
}
