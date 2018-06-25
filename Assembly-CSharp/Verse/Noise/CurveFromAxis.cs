using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F7C RID: 3964
	public class CurveFromAxis : ModuleBase
	{
		// Token: 0x04003EE2 RID: 16098
		public SimpleCurve curve;

		// Token: 0x06005FA2 RID: 24482 RVA: 0x0030BBF7 File Offset: 0x00309FF7
		public CurveFromAxis() : base(0)
		{
		}

		// Token: 0x06005FA3 RID: 24483 RVA: 0x0030BC01 File Offset: 0x0030A001
		public CurveFromAxis(SimpleCurve curve) : base(0)
		{
			this.curve = curve;
		}

		// Token: 0x06005FA4 RID: 24484 RVA: 0x0030BC14 File Offset: 0x0030A014
		public override double GetValue(double x, double y, double z)
		{
			float x2 = Mathf.Abs((float)x);
			return (double)this.curve.Evaluate(x2);
		}
	}
}
