using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F94 RID: 3988
	public class Invert : ModuleBase
	{
		// Token: 0x06006049 RID: 24649 RVA: 0x0030E620 File Offset: 0x0030CA20
		public Invert() : base(1)
		{
		}

		// Token: 0x0600604A RID: 24650 RVA: 0x0030E62A File Offset: 0x0030CA2A
		public Invert(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x0600604B RID: 24651 RVA: 0x0030E640 File Offset: 0x0030CA40
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			return -this.modules[0].GetValue(x, y, z);
		}
	}
}
