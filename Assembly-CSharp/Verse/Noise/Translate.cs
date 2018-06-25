using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000FA2 RID: 4002
	public class Translate : ModuleBase
	{
		// Token: 0x04003F3A RID: 16186
		private double m_x = 1.0;

		// Token: 0x04003F3B RID: 16187
		private double m_y = 1.0;

		// Token: 0x04003F3C RID: 16188
		private double m_z = 1.0;

		// Token: 0x0600609B RID: 24731 RVA: 0x0030F6E1 File Offset: 0x0030DAE1
		public Translate() : base(1)
		{
		}

		// Token: 0x0600609C RID: 24732 RVA: 0x0030F718 File Offset: 0x0030DB18
		public Translate(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x0600609D RID: 24733 RVA: 0x0030F758 File Offset: 0x0030DB58
		public Translate(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x17000F89 RID: 3977
		// (get) Token: 0x0600609E RID: 24734 RVA: 0x0030F7BC File Offset: 0x0030DBBC
		// (set) Token: 0x0600609F RID: 24735 RVA: 0x0030F7D7 File Offset: 0x0030DBD7
		public double X
		{
			get
			{
				return this.m_x;
			}
			set
			{
				this.m_x = value;
			}
		}

		// Token: 0x17000F8A RID: 3978
		// (get) Token: 0x060060A0 RID: 24736 RVA: 0x0030F7E4 File Offset: 0x0030DBE4
		// (set) Token: 0x060060A1 RID: 24737 RVA: 0x0030F7FF File Offset: 0x0030DBFF
		public double Y
		{
			get
			{
				return this.m_y;
			}
			set
			{
				this.m_y = value;
			}
		}

		// Token: 0x17000F8B RID: 3979
		// (get) Token: 0x060060A2 RID: 24738 RVA: 0x0030F80C File Offset: 0x0030DC0C
		// (set) Token: 0x060060A3 RID: 24739 RVA: 0x0030F827 File Offset: 0x0030DC27
		public double Z
		{
			get
			{
				return this.m_z;
			}
			set
			{
				this.m_z = value;
			}
		}

		// Token: 0x060060A4 RID: 24740 RVA: 0x0030F834 File Offset: 0x0030DC34
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			return this.modules[0].GetValue(x + this.m_x, y + this.m_y, z + this.m_z);
		}
	}
}
