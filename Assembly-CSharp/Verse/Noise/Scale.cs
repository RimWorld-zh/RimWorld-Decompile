using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F97 RID: 3991
	public class Scale : ModuleBase
	{
		// Token: 0x06006036 RID: 24630 RVA: 0x0030C4DD File Offset: 0x0030A8DD
		public Scale() : base(1)
		{
		}

		// Token: 0x06006037 RID: 24631 RVA: 0x0030C514 File Offset: 0x0030A914
		public Scale(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06006038 RID: 24632 RVA: 0x0030C554 File Offset: 0x0030A954
		public Scale(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x17000F7A RID: 3962
		// (get) Token: 0x06006039 RID: 24633 RVA: 0x0030C5B8 File Offset: 0x0030A9B8
		// (set) Token: 0x0600603A RID: 24634 RVA: 0x0030C5D3 File Offset: 0x0030A9D3
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

		// Token: 0x17000F7B RID: 3963
		// (get) Token: 0x0600603B RID: 24635 RVA: 0x0030C5E0 File Offset: 0x0030A9E0
		// (set) Token: 0x0600603C RID: 24636 RVA: 0x0030C5FB File Offset: 0x0030A9FB
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

		// Token: 0x17000F7C RID: 3964
		// (get) Token: 0x0600603D RID: 24637 RVA: 0x0030C608 File Offset: 0x0030AA08
		// (set) Token: 0x0600603E RID: 24638 RVA: 0x0030C623 File Offset: 0x0030AA23
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

		// Token: 0x0600603F RID: 24639 RVA: 0x0030C630 File Offset: 0x0030AA30
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			return this.modules[0].GetValue(x * this.m_x, y * this.m_y, z * this.m_z);
		}

		// Token: 0x04003F17 RID: 16151
		private double m_x = 1.0;

		// Token: 0x04003F18 RID: 16152
		private double m_y = 1.0;

		// Token: 0x04003F19 RID: 16153
		private double m_z = 1.0;
	}
}
