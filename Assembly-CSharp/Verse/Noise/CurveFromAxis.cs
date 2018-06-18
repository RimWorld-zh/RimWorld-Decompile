using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F77 RID: 3959
	public class CurveFromAxis : ModuleBase
	{
		// Token: 0x06005F6F RID: 24431 RVA: 0x0030928F File Offset: 0x0030768F
		public CurveFromAxis() : base(0)
		{
		}

		// Token: 0x06005F70 RID: 24432 RVA: 0x00309299 File Offset: 0x00307699
		public CurveFromAxis(SimpleCurve curve) : base(0)
		{
			this.curve = curve;
		}

		// Token: 0x06005F71 RID: 24433 RVA: 0x003092AC File Offset: 0x003076AC
		public override double GetValue(double x, double y, double z)
		{
			float x2 = Mathf.Abs((float)x);
			return (double)this.curve.Evaluate(x2);
		}

		// Token: 0x04003EC5 RID: 16069
		public SimpleCurve curve;
	}
}
