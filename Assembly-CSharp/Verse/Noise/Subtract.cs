using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F9C RID: 3996
	public class Subtract : ModuleBase
	{
		// Token: 0x06006082 RID: 24706 RVA: 0x0030ECF5 File Offset: 0x0030D0F5
		public Subtract() : base(2)
		{
		}

		// Token: 0x06006083 RID: 24707 RVA: 0x0030ECFF File Offset: 0x0030D0FF
		public Subtract(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06006084 RID: 24708 RVA: 0x0030ED1C File Offset: 0x0030D11C
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			return this.modules[0].GetValue(x, y, z) - this.modules[1].GetValue(x, y, z);
		}
	}
}
