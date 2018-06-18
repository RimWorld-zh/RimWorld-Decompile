using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F8F RID: 3983
	public class Invert : ModuleBase
	{
		// Token: 0x06006016 RID: 24598 RVA: 0x0030BCB8 File Offset: 0x0030A0B8
		public Invert() : base(1)
		{
		}

		// Token: 0x06006017 RID: 24599 RVA: 0x0030BCC2 File Offset: 0x0030A0C2
		public Invert(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06006018 RID: 24600 RVA: 0x0030BCD8 File Offset: 0x0030A0D8
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			return -this.modules[0].GetValue(x, y, z);
		}
	}
}
