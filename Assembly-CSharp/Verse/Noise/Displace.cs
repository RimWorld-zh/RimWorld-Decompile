using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F8C RID: 3980
	public class Displace : ModuleBase
	{
		// Token: 0x06006003 RID: 24579 RVA: 0x0030B822 File Offset: 0x00309C22
		public Displace() : base(4)
		{
		}

		// Token: 0x06006004 RID: 24580 RVA: 0x0030B82C File Offset: 0x00309C2C
		public Displace(ModuleBase input, ModuleBase x, ModuleBase y, ModuleBase z) : base(4)
		{
			this.modules[0] = input;
			this.modules[1] = x;
			this.modules[2] = y;
			this.modules[3] = z;
		}

		// Token: 0x17000F74 RID: 3956
		// (get) Token: 0x06006005 RID: 24581 RVA: 0x0030B85C File Offset: 0x00309C5C
		// (set) Token: 0x06006006 RID: 24582 RVA: 0x0030B879 File Offset: 0x00309C79
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

		// Token: 0x17000F75 RID: 3957
		// (get) Token: 0x06006007 RID: 24583 RVA: 0x0030B894 File Offset: 0x00309C94
		// (set) Token: 0x06006008 RID: 24584 RVA: 0x0030B8B1 File Offset: 0x00309CB1
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

		// Token: 0x17000F76 RID: 3958
		// (get) Token: 0x06006009 RID: 24585 RVA: 0x0030B8CC File Offset: 0x00309CCC
		// (set) Token: 0x0600600A RID: 24586 RVA: 0x0030B8E9 File Offset: 0x00309CE9
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

		// Token: 0x0600600B RID: 24587 RVA: 0x0030B904 File Offset: 0x00309D04
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
