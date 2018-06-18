using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F70 RID: 3952
	public class AbsLatitudeCurve : ModuleBase
	{
		// Token: 0x06005F4F RID: 24399 RVA: 0x00308D0B File Offset: 0x0030710B
		public AbsLatitudeCurve() : base(0)
		{
		}

		// Token: 0x06005F50 RID: 24400 RVA: 0x00308D15 File Offset: 0x00307115
		public AbsLatitudeCurve(SimpleCurve curve, float planetRadius) : base(0)
		{
			this.curve = curve;
			this.planetRadius = planetRadius;
		}

		// Token: 0x06005F51 RID: 24401 RVA: 0x00308D30 File Offset: 0x00307130
		public override double GetValue(double x, double y, double z)
		{
			float f = Mathf.Asin((float)(y / (double)this.planetRadius)) * 57.29578f;
			return (double)this.curve.Evaluate(Mathf.Abs(f));
		}

		// Token: 0x04003EB9 RID: 16057
		public SimpleCurve curve;

		// Token: 0x04003EBA RID: 16058
		public float planetRadius;
	}
}
