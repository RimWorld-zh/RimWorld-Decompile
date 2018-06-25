using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F8F RID: 3983
	public class Displace : ModuleBase
	{
		// Token: 0x06006034 RID: 24628 RVA: 0x0030E022 File Offset: 0x0030C422
		public Displace() : base(4)
		{
		}

		// Token: 0x06006035 RID: 24629 RVA: 0x0030E02C File Offset: 0x0030C42C
		public Displace(ModuleBase input, ModuleBase x, ModuleBase y, ModuleBase z) : base(4)
		{
			this.modules[0] = input;
			this.modules[1] = x;
			this.modules[2] = y;
			this.modules[3] = z;
		}

		// Token: 0x17000F76 RID: 3958
		// (get) Token: 0x06006036 RID: 24630 RVA: 0x0030E05C File Offset: 0x0030C45C
		// (set) Token: 0x06006037 RID: 24631 RVA: 0x0030E079 File Offset: 0x0030C479
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
		// (get) Token: 0x06006038 RID: 24632 RVA: 0x0030E094 File Offset: 0x0030C494
		// (set) Token: 0x06006039 RID: 24633 RVA: 0x0030E0B1 File Offset: 0x0030C4B1
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
		// (get) Token: 0x0600603A RID: 24634 RVA: 0x0030E0CC File Offset: 0x0030C4CC
		// (set) Token: 0x0600603B RID: 24635 RVA: 0x0030E0E9 File Offset: 0x0030C4E9
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

		// Token: 0x0600603C RID: 24636 RVA: 0x0030E104 File Offset: 0x0030C504
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
