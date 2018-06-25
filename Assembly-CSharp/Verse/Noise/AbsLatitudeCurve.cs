using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F74 RID: 3956
	public class AbsLatitudeCurve : ModuleBase
	{
		// Token: 0x04003ECE RID: 16078
		public SimpleCurve curve;

		// Token: 0x04003ECF RID: 16079
		public float planetRadius;

		// Token: 0x06005F82 RID: 24450 RVA: 0x0030B42F File Offset: 0x0030982F
		public AbsLatitudeCurve() : base(0)
		{
		}

		// Token: 0x06005F83 RID: 24451 RVA: 0x0030B439 File Offset: 0x00309839
		public AbsLatitudeCurve(SimpleCurve curve, float planetRadius) : base(0)
		{
			this.curve = curve;
			this.planetRadius = planetRadius;
		}

		// Token: 0x06005F84 RID: 24452 RVA: 0x0030B454 File Offset: 0x00309854
		public override double GetValue(double x, double y, double z)
		{
			float f = Mathf.Asin((float)(y / (double)this.planetRadius)) * 57.29578f;
			return (double)this.curve.Evaluate(Mathf.Abs(f));
		}
	}
}
