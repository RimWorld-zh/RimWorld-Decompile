using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000FA0 RID: 4000
	public class Turbulence : ModuleBase
	{
		// Token: 0x06006074 RID: 24692 RVA: 0x0030D084 File Offset: 0x0030B484
		public Turbulence() : base(1)
		{
			this.m_xDistort = new Perlin();
			this.m_yDistort = new Perlin();
			this.m_zDistort = new Perlin();
		}

		// Token: 0x06006075 RID: 24693 RVA: 0x0030D0E0 File Offset: 0x0030B4E0
		public Turbulence(ModuleBase input) : base(1)
		{
			this.m_xDistort = new Perlin();
			this.m_yDistort = new Perlin();
			this.m_zDistort = new Perlin();
			this.modules[0] = input;
		}

		// Token: 0x06006076 RID: 24694 RVA: 0x0030D143 File Offset: 0x0030B543
		public Turbulence(double power, ModuleBase input) : this(new Perlin(), new Perlin(), new Perlin(), power, input)
		{
		}

		// Token: 0x06006077 RID: 24695 RVA: 0x0030D160 File Offset: 0x0030B560
		public Turbulence(Perlin x, Perlin y, Perlin z, double power, ModuleBase input) : base(1)
		{
			this.m_xDistort = x;
			this.m_yDistort = y;
			this.m_zDistort = z;
			this.modules[0] = input;
			this.Power = power;
		}

		// Token: 0x17000F8A RID: 3978
		// (get) Token: 0x06006078 RID: 24696 RVA: 0x0030D1C0 File Offset: 0x0030B5C0
		// (set) Token: 0x06006079 RID: 24697 RVA: 0x0030D1E0 File Offset: 0x0030B5E0
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

		// Token: 0x17000F8B RID: 3979
		// (get) Token: 0x0600607A RID: 24698 RVA: 0x0030D208 File Offset: 0x0030B608
		// (set) Token: 0x0600607B RID: 24699 RVA: 0x0030D223 File Offset: 0x0030B623
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

		// Token: 0x17000F8C RID: 3980
		// (get) Token: 0x0600607C RID: 24700 RVA: 0x0030D230 File Offset: 0x0030B630
		// (set) Token: 0x0600607D RID: 24701 RVA: 0x0030D250 File Offset: 0x0030B650
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

		// Token: 0x17000F8D RID: 3981
		// (get) Token: 0x0600607E RID: 24702 RVA: 0x0030D278 File Offset: 0x0030B678
		// (set) Token: 0x0600607F RID: 24703 RVA: 0x0030D298 File Offset: 0x0030B698
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

		// Token: 0x06006080 RID: 24704 RVA: 0x0030D2C4 File Offset: 0x0030B6C4
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			double x2 = x + this.m_xDistort.GetValue(x + 0.189422607421875, y + 0.99371337890625, z + 0.4781646728515625) * this.m_power;
			double y2 = y + this.m_yDistort.GetValue(x + 0.4046478271484375, y + 0.276611328125, z + 0.9230499267578125) * this.m_power;
			double z2 = z + this.m_zDistort.GetValue(x + 0.82122802734375, y + 0.1710968017578125, z + 0.6842803955078125) * this.m_power;
			return this.modules[0].GetValue(x2, y2, z2);
		}

		// Token: 0x04003F29 RID: 16169
		private const double X0 = 0.189422607421875;

		// Token: 0x04003F2A RID: 16170
		private const double Y0 = 0.99371337890625;

		// Token: 0x04003F2B RID: 16171
		private const double Z0 = 0.4781646728515625;

		// Token: 0x04003F2C RID: 16172
		private const double X1 = 0.4046478271484375;

		// Token: 0x04003F2D RID: 16173
		private const double Y1 = 0.276611328125;

		// Token: 0x04003F2E RID: 16174
		private const double Z1 = 0.9230499267578125;

		// Token: 0x04003F2F RID: 16175
		private const double X2 = 0.82122802734375;

		// Token: 0x04003F30 RID: 16176
		private const double Y2 = 0.1710968017578125;

		// Token: 0x04003F31 RID: 16177
		private const double Z2 = 0.6842803955078125;

		// Token: 0x04003F32 RID: 16178
		private double m_power = 1.0;

		// Token: 0x04003F33 RID: 16179
		private Perlin m_xDistort = null;

		// Token: 0x04003F34 RID: 16180
		private Perlin m_yDistort = null;

		// Token: 0x04003F35 RID: 16181
		private Perlin m_zDistort = null;
	}
}
