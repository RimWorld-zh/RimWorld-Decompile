using System;

namespace Verse
{
	// Token: 0x02000EF7 RID: 3831
	public struct CurveColumn
	{
		// Token: 0x04003CAC RID: 15532
		public float x;

		// Token: 0x04003CAD RID: 15533
		public SimpleCurve y;

		// Token: 0x06005B99 RID: 23449 RVA: 0x002EB048 File Offset: 0x002E9448
		public CurveColumn(float x, SimpleCurve y)
		{
			this.x = x;
			this.y = y;
		}
	}
}
