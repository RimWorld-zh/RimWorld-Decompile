using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F75 RID: 3957
	public class AbsLatitudeCurve : ModuleBase
	{
		// Token: 0x04003ED6 RID: 16086
		public SimpleCurve curve;

		// Token: 0x04003ED7 RID: 16087
		public float planetRadius;

		// Token: 0x06005F82 RID: 24450 RVA: 0x0030B673 File Offset: 0x00309A73
		public AbsLatitudeCurve() : base(0)
		{
		}

		// Token: 0x06005F83 RID: 24451 RVA: 0x0030B67D File Offset: 0x00309A7D
		public AbsLatitudeCurve(SimpleCurve curve, float planetRadius) : base(0)
		{
			this.curve = curve;
			this.planetRadius = planetRadius;
		}

		// Token: 0x06005F84 RID: 24452 RVA: 0x0030B698 File Offset: 0x00309A98
		public override double GetValue(double x, double y, double z)
		{
			float f = Mathf.Asin((float)(y / (double)this.planetRadius)) * 57.29578f;
			return (double)this.curve.Evaluate(Mathf.Abs(f));
		}
	}
}
