using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F77 RID: 3959
	public class CurveFromAxis : ModuleBase
	{
		// Token: 0x04003ED7 RID: 16087
		public SimpleCurve curve;

		// Token: 0x06005F98 RID: 24472 RVA: 0x0030B333 File Offset: 0x00309733
		public CurveFromAxis() : base(0)
		{
		}

		// Token: 0x06005F99 RID: 24473 RVA: 0x0030B33D File Offset: 0x0030973D
		public CurveFromAxis(SimpleCurve curve) : base(0)
		{
			this.curve = curve;
		}

		// Token: 0x06005F9A RID: 24474 RVA: 0x0030B350 File Offset: 0x00309750
		public override double GetValue(double x, double y, double z)
		{
			float x2 = Mathf.Abs((float)x);
			return (double)this.curve.Evaluate(x2);
		}
	}
}
