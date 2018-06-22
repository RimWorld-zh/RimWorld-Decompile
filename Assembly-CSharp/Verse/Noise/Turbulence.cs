using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F9F RID: 3999
	public class Turbulence : ModuleBase
	{
		// Token: 0x0600609B RID: 24731 RVA: 0x0030F204 File Offset: 0x0030D604
		public Turbulence() : base(1)
		{
			this.m_xDistort = new Perlin();
			this.m_yDistort = new Perlin();
			this.m_zDistort = new Perlin();
		}

		// Token: 0x0600609C RID: 24732 RVA: 0x0030F260 File Offset: 0x0030D660
		public Turbulence(ModuleBase input) : base(1)
		{
			this.m_xDistort = new Perlin();
			this.m_yDistort = new Perlin();
			this.m_zDistort = new Perlin();
			this.modules[0] = input;
		}

		// Token: 0x0600609D RID: 24733 RVA: 0x0030F2C3 File Offset: 0x0030D6C3
		public Turbulence(double power, ModuleBase input) : this(new Perlin(), new Perlin(), new Perlin(), power, input)
		{
		}

		// Token: 0x0600609E RID: 24734 RVA: 0x0030F2E0 File Offset: 0x0030D6E0
		public Turbulence(Perlin x, Perlin y, Perlin z, double power, ModuleBase input) : base(1)
		{
			this.m_xDistort = x;
			this.m_yDistort = y;
			this.m_zDistort = z;
			this.modules[0] = input;
			this.Power = power;
		}

		// Token: 0x17000F8D RID: 3981
		// (get) Token: 0x0600609F RID: 24735 RVA: 0x0030F340 File Offset: 0x0030D740
		// (set) Token: 0x060060A0 RID: 24736 RVA: 0x0030F360 File Offset: 0x0030D760
		public double Frequency
		{
			get
			{
				return this.m_xDistort.Frequency;
			}
			set
			{
				this.m_xDistort.Frequency = value;
				this.m_yDistort.Frequency = value;
				this.m_zDistort.Frequency = value;
			}
		}

		// Token: 0x17000F8E RID: 3982
		// (get) Token: 0x060060A1 RID: 24737 RVA: 0x0030F388 File Offset: 0x0030D788
		// (set) Token: 0x060060A2 RID: 24738 RVA: 0x0030F3A3 File Offset: 0x0030D7A3
		public double Power
		{
			get
			{
				return this.m_power;
			}
			set
			{
				this.m_power = value;
			}
		}

		// Token: 0x17000F8F RID: 3983
		// (get) Token: 0x060060A3 RID: 24739 RVA: 0x0030F3B0 File Offset: 0x0030D7B0
		// (set) Token: 0x060060A4 RID: 24740 RVA: 0x0030F3D0 File Offset: 0x0030D7D0
		public int Roughness
		{
			get
			{
				return this.m_xDistort.OctaveCount;
			}
			set
			{
				this.m_xDistort.OctaveCount = value;
				this.m_yDistort.OctaveCount = value;
				this.m_zDistort.OctaveCount = value;
			}
		}

		// Token: 0x17000F90 RID: 3984
		// (get) Token: 0x060060A5 RID: 24741 RVA: 0x0030F3F8 File Offset: 0x0030D7F8
		// (set) Token: 0x060060A6 RID: 24742 RVA: 0x0030F418 File Offset: 0x0030D818
		public int Seed
		{
			get
			{
				return this.m_xDistort.Seed;
			}
			set
			{
				this.m_xDistort.Seed = value;
				this.m_yDistort.Seed = value + 1;
				this.m_zDistort.Seed = value + 2;
			}
		}

		// Token: 0x060060A7 RID: 24743 RVA: 0x0030F444 File Offset: 0x0030D844
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			double x2 = x + this.m_xDistort.GetValue(x + 0.189422607421875, y + 0.99371337890625, z + 0.4781646728515625) * this.m_power;
			double y2 = y + this.m_yDistort.GetValue(x + 0.4046478271484375, y + 0.276611328125, z + 0.9230499267578125) * this.m_power;
			double z2 = z + this.m_zDistort.GetValue(x + 0.82122802734375, y + 0.1710968017578125, z + 0.6842803955078125) * this.m_power;
			return this.modules[0].GetValue(x2, y2, z2);
		}

		// Token: 0x04003F3A RID: 16186
		private const double X0 = 0.189422607421875;

		// Token: 0x04003F3B RID: 16187
		private const double Y0 = 0.99371337890625;

		// Token: 0x04003F3C RID: 16188
		private const double Z0 = 0.4781646728515625;

		// Token: 0x04003F3D RID: 16189
		private const double X1 = 0.4046478271484375;

		// Token: 0x04003F3E RID: 16190
		private const double Y1 = 0.276611328125;

		// Token: 0x04003F3F RID: 16191
		private const double Z1 = 0.9230499267578125;

		// Token: 0x04003F40 RID: 16192
		private const double X2 = 0.82122802734375;

		// Token: 0x04003F41 RID: 16193
		private const double Y2 = 0.1710968017578125;

		// Token: 0x04003F42 RID: 16194
		private const double Z2 = 0.6842803955078125;

		// Token: 0x04003F43 RID: 16195
		private double m_power = 1.0;

		// Token: 0x04003F44 RID: 16196
		private Perlin m_xDistort = null;

		// Token: 0x04003F45 RID: 16197
		private Perlin m_yDistort = null;

		// Token: 0x04003F46 RID: 16198
		private Perlin m_zDistort = null;
	}
}
