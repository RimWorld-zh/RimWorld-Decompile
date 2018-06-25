using System;

namespace Verse
{
	// Token: 0x02000EF8 RID: 3832
	public struct CurveColumn
	{
		// Token: 0x04003CB4 RID: 15540
		public float x;

		// Token: 0x04003CB5 RID: 15541
		public SimpleCurve y;

		// Token: 0x06005B99 RID: 23449 RVA: 0x002EB268 File Offset: 0x002E9668
		public CurveColumn(float x, SimpleCurve y)
		{
			this.x = x;
			this.y = y;
		}
	}
}
