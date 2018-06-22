using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F8B RID: 3979
	public class Displace : ModuleBase
	{
		// Token: 0x0600602A RID: 24618 RVA: 0x0030D9A2 File Offset: 0x0030BDA2
		public Displace() : base(4)
		{
		}

		// Token: 0x0600602B RID: 24619 RVA: 0x0030D9AC File Offset: 0x0030BDAC
		public Displace(ModuleBase input, ModuleBase x, ModuleBase y, ModuleBase z) : base(4)
		{
			this.modules[0] = input;
			this.modules[1] = x;
			this.modules[2] = y;
			this.modules[3] = z;
		}

		// Token: 0x17000F77 RID: 3959
		// (get) Token: 0x0600602C RID: 24620 RVA: 0x0030D9DC File Offset: 0x0030BDDC
		// (set) Token: 0x0600602D RID: 24621 RVA: 0x0030D9F9 File Offset: 0x0030BDF9
		public ModuleBase X
		{
			get
			{
				return this.modules[1];
			}
			set
			{
				Debug.Assert(value != null);
				this.modules[1] = value;
			}
		}

		// Token: 0x17000F78 RID: 3960
		// (get) Token: 0x0600602E RID: 24622 RVA: 0x0030DA14 File Offset: 0x0030BE14
		// (set) Token: 0x0600602F RID: 24623 RVA: 0x0030DA31 File Offset: 0x0030BE31
		public ModuleBase Y
		{
			get
			{
				return this.modules[2];
			}
			set
			{
				Debug.Assert(value != null);
				this.modules[2] = value;
			}
		}

		// Token: 0x17000F79 RID: 3961
		// (get) Token: 0x06006030 RID: 24624 RVA: 0x0030DA4C File Offset: 0x0030BE4C
		// (set) Token: 0x06006031 RID: 24625 RVA: 0x0030DA69 File Offset: 0x0030BE69
		public ModuleBase Z
		{
			get
			{
				return this.modules[3];
			}
			set
			{
				Debug.Assert(value != null);
				this.modules[3] = value;
			}
		}

		// Token: 0x06006032 RID: 24626 RVA: 0x0030DA84 File Offset: 0x0030BE84
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			Debug.Assert(this.modules[1] != null);
			Debug.Assert(this.modules[2] != null);
			Debug.Assert(this.modules[3] != null);
			double x2 = x + this.modules[1].GetValue(x, y, z);
			double y2 = y + this.modules[2].GetValue(x, y, z);
			double z2 = z + this.modules[3].GetValue(x, y, z);
			return this.modules[0].GetValue(x2, y2, z2);
		}
	}
}
