using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F90 RID: 3984
	public class Invert : ModuleBase
	{
		// Token: 0x06006018 RID: 24600 RVA: 0x0030BBDC File Offset: 0x00309FDC
		public Invert() : base(1)
		{
		}

		// Token: 0x06006019 RID: 24601 RVA: 0x0030BBE6 File Offset: 0x00309FE6
		public Invert(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x0600601A RID: 24602 RVA: 0x0030BBFC File Offset: 0x00309FFC
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			return -this.modules[0].GetValue(x, y, z);
		}
	}
}
