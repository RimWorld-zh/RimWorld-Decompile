using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F78 RID: 3960
	public class CurveFromAxis : ModuleBase
	{
		// Token: 0x06005F71 RID: 24433 RVA: 0x003091B3 File Offset: 0x003075B3
		public CurveFromAxis() : base(0)
		{
		}

		// Token: 0x06005F72 RID: 24434 RVA: 0x003091BD File Offset: 0x003075BD
		public CurveFromAxis(SimpleCurve curve) : base(0)
		{
			this.curve = curve;
		}

		// Token: 0x06005F73 RID: 24435 RVA: 0x003091D0 File Offset: 0x003075D0
		public override double GetValue(double x, double y, double z)
		{
			float x2 = Mathf.Abs((float)x);
			return (double)this.curve.Evaluate(x2);
		}

		// Token: 0x04003EC6 RID: 16070
		public SimpleCurve curve;
	}
}
