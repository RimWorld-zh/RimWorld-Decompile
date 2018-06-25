using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F87 RID: 3975
	public class Abs : ModuleBase
	{
		// Token: 0x0600600E RID: 24590 RVA: 0x0030D7D1 File Offset: 0x0030BBD1
		public Abs() : base(1)
		{
		}

		// Token: 0x0600600F RID: 24591 RVA: 0x0030D7DB File Offset: 0x0030BBDB
		public Abs(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06006010 RID: 24592 RVA: 0x0030D7F0 File Offset: 0x0030BBF0
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			return Math.Abs(this.modules[0].GetValue(x, y, z));
		}
	}
}
