using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F70 RID: 3952
	public class AbsLatitudeCurve : ModuleBase
	{
		// Token: 0x06005F78 RID: 24440 RVA: 0x0030ADAF File Offset: 0x003091AF
		public AbsLatitudeCurve() : base(0)
		{
		}

		// Token: 0x06005F79 RID: 24441 RVA: 0x0030ADB9 File Offset: 0x003091B9
		public AbsLatitudeCurve(SimpleCurve curve, float planetRadius) : base(0)
		{
			this.curve = curve;
			this.planetRadius = planetRadius;
		}

		// Token: 0x06005F7A RID: 24442 RVA: 0x0030ADD4 File Offset: 0x003091D4
		public override double GetValue(double x, double y, double z)
		{
			float f = Mathf.Asin((float)(y / (double)this.planetRadius)) * 57.29578f;
			return (double)this.curve.Evaluate(Mathf.Abs(f));
		}

		// Token: 0x04003ECB RID: 16075
		public SimpleCurve curve;

		// Token: 0x04003ECC RID: 16076
		public float planetRadius;
	}
}
