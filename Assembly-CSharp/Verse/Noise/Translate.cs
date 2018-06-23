using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F9E RID: 3998
	public class Translate : ModuleBase
	{
		// Token: 0x04003F37 RID: 16183
		private double m_x = 1.0;

		// Token: 0x04003F38 RID: 16184
		private double m_y = 1.0;

		// Token: 0x04003F39 RID: 16185
		private double m_z = 1.0;

		// Token: 0x06006091 RID: 24721 RVA: 0x0030F061 File Offset: 0x0030D461
		public Translate() : base(1)
		{
		}

		// Token: 0x06006092 RID: 24722 RVA: 0x0030F098 File Offset: 0x0030D498
		public Translate(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06006093 RID: 24723 RVA: 0x0030F0D8 File Offset: 0x0030D4D8
		public Translate(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x17000F8A RID: 3978
		// (get) Token: 0x06006094 RID: 24724 RVA: 0x0030F13C File Offset: 0x0030D53C
		// (set) Token: 0x06006095 RID: 24725 RVA: 0x0030F157 File Offset: 0x0030D557
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

		// Token: 0x17000F8B RID: 3979
		// (get) Token: 0x06006096 RID: 24726 RVA: 0x0030F164 File Offset: 0x0030D564
		// (set) Token: 0x06006097 RID: 24727 RVA: 0x0030F17F File Offset: 0x0030D57F
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

		// Token: 0x17000F8C RID: 3980
		// (get) Token: 0x06006098 RID: 24728 RVA: 0x0030F18C File Offset: 0x0030D58C
		// (set) Token: 0x06006099 RID: 24729 RVA: 0x0030F1A7 File Offset: 0x0030D5A7
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

		// Token: 0x0600609A RID: 24730 RVA: 0x0030F1B4 File Offset: 0x0030D5B4
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			return this.modules[0].GetValue(x + this.m_x, y + this.m_y, z + this.m_z);
		}
	}
}
