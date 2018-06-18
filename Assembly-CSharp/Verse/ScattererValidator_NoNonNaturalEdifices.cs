using System;

namespace Verse
{
	// Token: 0x02000C60 RID: 3168
	public class ScattererValidator_NoNonNaturalEdifices : ScattererValidator
	{
		// Token: 0x060045A0 RID: 17824 RVA: 0x0024BA18 File Offset: 0x00249E18
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

		// Token: 0x04002F97 RID: 12183
		public int radius = 1;
	}
}
