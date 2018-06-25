using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F7B RID: 3963
	public class CurveFromAxis : ModuleBase
	{
		// Token: 0x04003EDA RID: 16090
		public SimpleCurve curve;

		// Token: 0x06005FA2 RID: 24482 RVA: 0x0030B9B3 File Offset: 0x00309DB3
		public CurveFromAxis() : base(0)
		{
		}

		// Token: 0x06005FA3 RID: 24483 RVA: 0x0030B9BD File Offset: 0x00309DBD
		public CurveFromAxis(SimpleCurve curve) : base(0)
		{
			this.curve = curve;
		}

		// Token: 0x06005FA4 RID: 24484 RVA: 0x0030B9D0 File Offset: 0x00309DD0
		public override double GetValue(double x, double y, double z)
		{
			float x2 = Mathf.Abs((float)x);
			return (double)this.curve.Evaluate(x2);
		}
	}
}
