using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F90 RID: 3984
	public class Displace : ModuleBase
	{
		// Token: 0x06006034 RID: 24628 RVA: 0x0030E266 File Offset: 0x0030C666
		public Displace() : base(4)
		{
		}

		// Token: 0x06006035 RID: 24629 RVA: 0x0030E270 File Offset: 0x0030C670
		public Displace(ModuleBase input, ModuleBase x, ModuleBase y, ModuleBase z) : base(4)
		{
			this.modules[0] = input;
			this.modules[1] = x;
			this.modules[2] = y;
			this.modules[3] = z;
		}

		// Token: 0x17000F76 RID: 3958
		// (get) Token: 0x06006036 RID: 24630 RVA: 0x0030E2A0 File Offset: 0x0030C6A0
		// (set) Token: 0x06006037 RID: 24631 RVA: 0x0030E2BD File Offset: 0x0030C6BD
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

		// Token: 0x17000F77 RID: 3959
		// (get) Token: 0x06006038 RID: 24632 RVA: 0x0030E2D8 File Offset: 0x0030C6D8
		// (set) Token: 0x06006039 RID: 24633 RVA: 0x0030E2F5 File Offset: 0x0030C6F5
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

		// Token: 0x17000F78 RID: 3960
		// (get) Token: 0x0600603A RID: 24634 RVA: 0x0030E310 File Offset: 0x0030C710
		// (set) Token: 0x0600603B RID: 24635 RVA: 0x0030E32D File Offset: 0x0030C72D
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

		// Token: 0x0600603C RID: 24636 RVA: 0x0030E348 File Offset: 0x0030C748
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
