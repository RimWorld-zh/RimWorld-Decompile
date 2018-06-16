using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F71 RID: 3953
	public class AbsLatitudeCurve : ModuleBase
	{
		// Token: 0x06005F51 RID: 24401 RVA: 0x00308C2F File Offset: 0x0030702F
		public AbsLatitudeCurve() : base(0)
		{
		}

		// Token: 0x06005F52 RID: 24402 RVA: 0x00308C39 File Offset: 0x00307039
		public AbsLatitudeCurve(SimpleCurve curve, float planetRadius) : base(0)
		{
			this.curve = curve;
			this.planetRadius = planetRadius;
		}

		// Token: 0x06005F53 RID: 24403 RVA: 0x00308C54 File Offset: 0x00307054
		public override double GetValue(double x, double y, double z)
		{
			float f = Mathf.Asin((float)(y / (double)this.planetRadius)) * 57.29578f;
			return (double)this.curve.Evaluate(Mathf.Abs(f));
		}

		// Token: 0x04003EBA RID: 16058
		public SimpleCurve curve;

		// Token: 0x04003EBB RID: 16059
		public float planetRadius;
	}
}
