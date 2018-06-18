using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F8B RID: 3979
	public class Displace : ModuleBase
	{
		// Token: 0x06006001 RID: 24577 RVA: 0x0030B8FE File Offset: 0x00309CFE
		public Displace() : base(4)
		{
		}

		// Token: 0x06006002 RID: 24578 RVA: 0x0030B908 File Offset: 0x00309D08
		public Displace(ModuleBase input, ModuleBase x, ModuleBase y, ModuleBase z) : base(4)
		{
			this.modules[0] = input;
			this.modules[1] = x;
			this.modules[2] = y;
			this.modules[3] = z;
		}

		// Token: 0x17000F73 RID: 3955
		// (get) Token: 0x06006003 RID: 24579 RVA: 0x0030B938 File Offset: 0x00309D38
		// (set) Token: 0x06006004 RID: 24580 RVA: 0x0030B955 File Offset: 0x00309D55
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

		// Token: 0x17000F74 RID: 3956
		// (get) Token: 0x06006005 RID: 24581 RVA: 0x0030B970 File Offset: 0x00309D70
		// (set) Token: 0x06006006 RID: 24582 RVA: 0x0030B98D File Offset: 0x00309D8D
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

		// Token: 0x17000F75 RID: 3957
		// (get) Token: 0x06006007 RID: 24583 RVA: 0x0030B9A8 File Offset: 0x00309DA8
		// (set) Token: 0x06006008 RID: 24584 RVA: 0x0030B9C5 File Offset: 0x00309DC5
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

		// Token: 0x06006009 RID: 24585 RVA: 0x0030B9E0 File Offset: 0x00309DE0
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
