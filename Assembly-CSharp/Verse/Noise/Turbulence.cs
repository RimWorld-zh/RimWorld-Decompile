using System;
using System.Diagnostics;

namespace Verse.Noise
{
	// Token: 0x02000F9F RID: 3999
	public class Turbulence : ModuleBase
	{
		// Token: 0x06006072 RID: 24690 RVA: 0x0030D160 File Offset: 0x0030B560
		public Turbulence() : base(1)
		{
			this.m_xDistort = new Perlin();
			this.m_yDistort = new Perlin();
			this.m_zDistort = new Perlin();
		}

		// Token: 0x06006073 RID: 24691 RVA: 0x0030D1BC File Offset: 0x0030B5BC
		public Turbulence(ModuleBase input) : base(1)
		{
			this.m_xDistort = new Perlin();
			this.m_yDistort = new Perlin();
			this.m_zDistort = new Perlin();
			this.modules[0] = input;
		}

		// Token: 0x06006074 RID: 24692 RVA: 0x0030D21F File Offset: 0x0030B61F
		public Turbulence(double power, ModuleBase input) : this(new Perlin(), new Perlin(), new Perlin(), power, input)
		{
		}

		// Token: 0x06006075 RID: 24693 RVA: 0x0030D23C File Offset: 0x0030B63C
		public Turbulence(Perlin x, Perlin y, Perlin z, double power, ModuleBase input) : base(1)
		{
			this.m_xDistort = x;
			this.m_yDistort = y;
			this.m_zDistort = z;
			this.modules[0] = input;
			this.Power = power;
		}

		// Token: 0x17000F89 RID: 3977
		// (get) Token: 0x06006076 RID: 24694 RVA: 0x0030D29C File Offset: 0x0030B69C
		// (set) Token: 0x06006077 RID: 24695 RVA: 0x0030D2BC File Offset: 0x0030B6BC
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

		// Token: 0x17000F8A RID: 3978
		// (get) Token: 0x06006078 RID: 24696 RVA: 0x0030D2E4 File Offset: 0x0030B6E4
		// (set) Token: 0x06006079 RID: 24697 RVA: 0x0030D2FF File Offset: 0x0030B6FF
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

		// Token: 0x17000F8B RID: 3979
		// (get) Token: 0x0600607A RID: 24698 RVA: 0x0030D30C File Offset: 0x0030B70C
		// (set) Token: 0x0600607B RID: 24699 RVA: 0x0030D32C File Offset: 0x0030B72C
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

		// Token: 0x17000F8C RID: 3980
		// (get) Token: 0x0600607C RID: 24700 RVA: 0x0030D354 File Offset: 0x0030B754
		// (set) Token: 0x0600607D RID: 24701 RVA: 0x0030D374 File Offset: 0x0030B774
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

		// Token: 0x0600607E RID: 24702 RVA: 0x0030D3A0 File Offset: 0x0030B7A0
		public override double GetValue(double x, double y, double z)
		{
			Debug.Assert(this.modules[0] != null);
			double x2 = x + this.m_xDistort.GetValue(x + 0.189422607421875, y + 0.99371337890625, z + 0.4781646728515625) * this.m_power;
			double y2 = y + this.m_yDistort.GetValue(x + 0.4046478271484375, y + 0.276611328125, z + 0.9230499267578125) * this.m_power;
			double z2 = z + this.m_zDistort.GetValue(x + 0.82122802734375, y + 0.1710968017578125, z + 0.6842803955078125) * this.m_power;
			return this.modules[0].GetValue(x2, y2, z2);
		}

		// Token: 0x04003F28 RID: 16168
		private const double X0 = 0.189422607421875;

		// Token: 0x04003F29 RID: 16169
		private const double Y0 = 0.99371337890625;

		// Token: 0x04003F2A RID: 16170
		private const double Z0 = 0.4781646728515625;

		// Token: 0x04003F2B RID: 16171
		private const double X1 = 0.4046478271484375;

		// Token: 0x04003F2C RID: 16172
		private const double Y1 = 0.276611328125;

		// Token: 0x04003F2D RID: 16173
		private const double Z1 = 0.9230499267578125;

		// Token: 0x04003F2E RID: 16174
		private const double X2 = 0.82122802734375;

		// Token: 0x04003F2F RID: 16175
		private const double Y2 = 0.1710968017578125;

		// Token: 0x04003F30 RID: 16176
		private const double Z2 = 0.6842803955078125;

		// Token: 0x04003F31 RID: 16177
		private double m_power = 1.0;

		// Token: 0x04003F32 RID: 16178
		private Perlin m_xDistort = null;

		// Token: 0x04003F33 RID: 16179
		private Perlin m_yDistort = null;

		// Token: 0x04003F34 RID: 16180
		private Perlin m_zDistort = null;
	}
}
