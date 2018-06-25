using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000FA3 RID: 4003
	public class Translate : ModuleBase
	{
		// Token: 0x04003F42 RID: 16194
		private double m_x = 1.0;

		// Token: 0x04003F43 RID: 16195
		private double m_y = 1.0;

		// Token: 0x04003F44 RID: 16196
		private double m_z = 1.0;

		// Token: 0x0600609B RID: 24731 RVA: 0x0030F925 File Offset: 0x0030DD25
		public Translate() : base(1)
		{
		}

		// Token: 0x0600609C RID: 24732 RVA: 0x0030F95C File Offset: 0x0030DD5C
		public Translate(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x0600609D RID: 24733 RVA: 0x0030F99C File Offset: 0x0030DD9C
		public Translate(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x17000F89 RID: 3977
		// (get) Token: 0x0600609E RID: 24734 RVA: 0x0030FA00 File Offset: 0x0030DE00
		// (set) Token: 0x0600609F RID: 24735 RVA: 0x0030FA1B File Offset: 0x0030DE1B
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
		// (get) Token: 0x060060A0 RID: 24736 RVA: 0x0030FA28 File Offset: 0x0030DE28
		// (set) Token: 0x060060A1 RID: 24737 RVA: 0x0030FA43 File Offset: 0x0030DE43
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
		// (get) Token: 0x060060A2 RID: 24738 RVA: 0x0030FA50 File Offset: 0x0030DE50
		// (set) Token: 0x060060A3 RID: 24739 RVA: 0x0030FA6B File Offset: 0x0030DE6B
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

		// Token: 0x060060A4 RID: 24740 RVA: 0x0030FA78 File Offset: 0x0030DE78
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			return this.modules[0].GetValue(x + this.m_x, y + this.m_y, z + this.m_z);
		}
	}
}
