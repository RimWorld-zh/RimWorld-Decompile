using System;

namespace Verse.Noise
{
	// Token: 0x02000F73 RID: 3955
	public class AxisAsValueZ : ModuleBase
	{
		// Token: 0x06005F56 RID: 24406 RVA: 0x00308CB2 File Offset: 0x003070B2
		public AxisAsValueZ() : base(0)
		{
		}

		// Token: 0x06005F57 RID: 24407 RVA: 0x00308CBC File Offset: 0x003070BC
		public override double GetValue(double x, double y, double z)
		{
			return z;
		}
	}
}
