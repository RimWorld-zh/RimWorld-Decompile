using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000FA4 RID: 4004
	public class Turbulence : ModuleBase
	{
		// Token: 0x04003F45 RID: 16197
		private const double X0 = 0.189422607421875;

		// Token: 0x04003F46 RID: 16198
		private const double Y0 = 0.99371337890625;

		// Token: 0x04003F47 RID: 16199
		private const double Z0 = 0.4781646728515625;

		// Token: 0x04003F48 RID: 16200
		private const double X1 = 0.4046478271484375;

		// Token: 0x04003F49 RID: 16201
		private const double Y1 = 0.276611328125;

		// Token: 0x04003F4A RID: 16202
		private const double Z1 = 0.9230499267578125;

		// Token: 0x04003F4B RID: 16203
		private const double X2 = 0.82122802734375;

		// Token: 0x04003F4C RID: 16204
		private const double Y2 = 0.1710968017578125;

		// Token: 0x04003F4D RID: 16205
		private const double Z2 = 0.6842803955078125;

		// Token: 0x04003F4E RID: 16206
		private double m_power = 1.0;

		// Token: 0x04003F4F RID: 16207
		private Perlin m_xDistort = null;

		// Token: 0x04003F50 RID: 16208
		private Perlin m_yDistort = null;

		// Token: 0x04003F51 RID: 16209
		private Perlin m_zDistort = null;

		// Token: 0x060060A5 RID: 24741 RVA: 0x0030FAC8 File Offset: 0x0030DEC8
		public Turbulence() : base(1)
		{
			this.m_xDistort = new Perlin();
			this.m_yDistort = new Perlin();
			this.m_zDistort = new Perlin();
		}

		// Token: 0x060060A6 RID: 24742 RVA: 0x0030FB24 File Offset: 0x0030DF24
		public Turbulence(ModuleBase input) : base(1)
		{
			this.m_xDistort = new Perlin();
			this.m_yDistort = new Perlin();
			this.m_zDistort = new Perlin();
			this.modules[0] = input;
		}

		// Token: 0x060060A7 RID: 24743 RVA: 0x0030FB87 File Offset: 0x0030DF87
		public Turbulence(double power, ModuleBase input) : this(new Perlin(), new Perlin(), new Perlin(), power, input)
		{
		}

		// Token: 0x060060A8 RID: 24744 RVA: 0x0030FBA4 File Offset: 0x0030DFA4
		public Turbulence(Perlin x, Perlin y, Perlin z, double power, ModuleBase input) : base(1)
		{
			this.m_xDistort = x;
			this.m_yDistort = y;
			this.m_zDistort = z;
			this.modules[0] = input;
			this.Power = power;
		}

		// Token: 0x17000F8C RID: 3980
		// (get) Token: 0x060060A9 RID: 24745 RVA: 0x0030FC04 File Offset: 0x0030E004
		// (set) Token: 0x060060AA RID: 24746 RVA: 0x0030FC24 File Offset: 0x0030E024
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

		// Token: 0x17000F8D RID: 3981
		// (get) Token: 0x060060AB RID: 24747 RVA: 0x0030FC4C File Offset: 0x0030E04C
		// (set) Token: 0x060060AC RID: 24748 RVA: 0x0030FC67 File Offset: 0x0030E067
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

		// Token: 0x17000F8E RID: 3982
		// (get) Token: 0x060060AD RID: 24749 RVA: 0x0030FC74 File Offset: 0x0030E074
		// (set) Token: 0x060060AE RID: 24750 RVA: 0x0030FC94 File Offset: 0x0030E094
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

		// Token: 0x17000F8F RID: 3983
		// (get) Token: 0x060060AF RID: 24751 RVA: 0x0030FCBC File Offset: 0x0030E0BC
		// (set) Token: 0x060060B0 RID: 24752 RVA: 0x0030FCDC File Offset: 0x0030E0DC
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

		// Token: 0x060060B1 RID: 24753 RVA: 0x0030FD08 File Offset: 0x0030E108
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			double x2 = x + this.m_xDistort.GetValue(x + 0.189422607421875, y + 0.99371337890625, z + 0.4781646728515625) * this.m_power;
			double y2 = y + this.m_yDistort.GetValue(x + 0.4046478271484375, y + 0.276611328125, z + 0.9230499267578125) * this.m_power;
			double z2 = z + this.m_zDistort.GetValue(x + 0.82122802734375, y + 0.1710968017578125, z + 0.6842803955078125) * this.m_power;
			return this.modules[0].GetValue(x2, y2, z2);
		}
	}
}
