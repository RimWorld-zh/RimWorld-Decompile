using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F77 RID: 3959
	public class Billow : ModuleBase
	{
		// Token: 0x04003ED0 RID: 16080
		private double m_frequency = 1.0;

		// Token: 0x04003ED1 RID: 16081
		private double m_lacunarity = 2.0;

		// Token: 0x04003ED2 RID: 16082
		private QualityMode m_quality = QualityMode.Medium;

		// Token: 0x04003ED3 RID: 16083
		private int m_octaveCount = 6;

		// Token: 0x04003ED4 RID: 16084
		private double m_persistence = 0.5;

		// Token: 0x04003ED5 RID: 16085
		private int m_seed = 0;

		// Token: 0x06005F89 RID: 24457 RVA: 0x0030B4D4 File Offset: 0x003098D4
		public Billow() : base(0)
		{
		}

		// Token: 0x06005F8A RID: 24458 RVA: 0x0030B52C File Offset: 0x0030992C
		public Billow(double frequency, double lacunarity, double persistence, int octaves, int seed, QualityMode quality) : base(0)
		{
			this.Frequency = frequency;
			this.Lacunarity = lacunarity;
			this.OctaveCount = octaves;
			this.Persistence = persistence;
			this.Seed = seed;
			this.Quality = quality;
		}

		// Token: 0x17000F4A RID: 3914
		// (get) Token: 0x06005F8B RID: 24459 RVA: 0x0030B5B0 File Offset: 0x003099B0
		// (set) Token: 0x06005F8C RID: 24460 RVA: 0x0030B5CB File Offset: 0x003099CB
		public double Frequency
		{
			get
			{
				return this.m_frequency;
			}
			set
			{
				this.m_frequency = value;
			}
		}

		// Token: 0x17000F4B RID: 3915
		// (get) Token: 0x06005F8D RID: 24461 RVA: 0x0030B5D8 File Offset: 0x003099D8
		// (set) Token: 0x06005F8E RID: 24462 RVA: 0x0030B5F3 File Offset: 0x003099F3
		public double Lacunarity
		{
			get
			{
				return this.m_lacunarity;
			}
			set
			{
				this.m_lacunarity = value;
			}
		}

		// Token: 0x17000F4C RID: 3916
		// (get) Token: 0x06005F8F RID: 24463 RVA: 0x0030B600 File Offset: 0x00309A00
		// (set) Token: 0x06005F90 RID: 24464 RVA: 0x0030B61B File Offset: 0x00309A1B
		public QualityMode Quality
		{
			get
			{
				return this.m_quality;
			}
			set
			{
				this.m_quality = value;
			}
		}

		// Token: 0x17000F4D RID: 3917
		// (get) Token: 0x06005F91 RID: 24465 RVA: 0x0030B628 File Offset: 0x00309A28
		// (set) Token: 0x06005F92 RID: 24466 RVA: 0x0030B643 File Offset: 0x00309A43
		public int OctaveCount
		{
			get
			{
				return this.m_octaveCount;
			}
			set
			{
				this.m_octaveCount = Mathf.Clamp(value, 1, 30);
			}
		}

		// Token: 0x17000F4E RID: 3918
		// (get) Token: 0x06005F93 RID: 24467 RVA: 0x0030B658 File Offset: 0x00309A58
		// (set) Token: 0x06005F94 RID: 24468 RVA: 0x0030B673 File Offset: 0x00309A73
		public double Persistence
		{
			get
			{
				return this.m_persistence;
			}
			set
			{
				this.m_persistence = value;
			}
		}

		// Token: 0x17000F4F RID: 3919
		// (get) Token: 0x06005F95 RID: 24469 RVA: 0x0030B680 File Offset: 0x00309A80
		// (set) Token: 0x06005F96 RID: 24470 RVA: 0x0030B69B File Offset: 0x00309A9B
		public int Seed
		{
			get
			{
				return this.m_seed;
			}
			set
			{
				this.m_seed = value;
			}
		}

		// Token: 0x06005F97 RID: 24471 RVA: 0x0030B6A8 File Offset: 0x00309AA8
		public override double GetValue(double x, double y, double z)
		{
			double num = 0.0;
			double num2 = 1.0;
			x *= this.m_frequency;
			y *= this.m_frequency;
			z *= this.m_frequency;
			for (int i = 0; i < this.m_octaveCount; i++)
			{
				double x2 = Utils.MakeInt32Range(x);
				double y2 = Utils.MakeInt32Range(y);
				double z2 = Utils.MakeInt32Range(z);
				long seed = (long)(this.m_seed + i) & (long)((ulong)-1);
				double num3 = Utils.GradientCoherentNoise3D(x2, y2, z2, seed, this.m_quality);
				num3 = 2.0 * Math.Abs(num3) - 1.0;
				num += num3 * num2;
				x *= this.m_lacunarity;
				y *= this.m_lacunarity;
				z *= this.m_lacunarity;
				num2 *= this.m_persistence;
			}
			return num + 0.5;
		}
	}
}
