using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F78 RID: 3960
	public class Billow : ModuleBase
	{
		// Token: 0x04003ED8 RID: 16088
		private double m_frequency = 1.0;

		// Token: 0x04003ED9 RID: 16089
		private double m_lacunarity = 2.0;

		// Token: 0x04003EDA RID: 16090
		private QualityMode m_quality = QualityMode.Medium;

		// Token: 0x04003EDB RID: 16091
		private int m_octaveCount = 6;

		// Token: 0x04003EDC RID: 16092
		private double m_persistence = 0.5;

		// Token: 0x04003EDD RID: 16093
		private int m_seed = 0;

		// Token: 0x06005F89 RID: 24457 RVA: 0x0030B718 File Offset: 0x00309B18
		public Billow() : base(0)
		{
		}

		// Token: 0x06005F8A RID: 24458 RVA: 0x0030B770 File Offset: 0x00309B70
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
		// (get) Token: 0x06005F8B RID: 24459 RVA: 0x0030B7F4 File Offset: 0x00309BF4
		// (set) Token: 0x06005F8C RID: 24460 RVA: 0x0030B80F File Offset: 0x00309C0F
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
		// (get) Token: 0x06005F8D RID: 24461 RVA: 0x0030B81C File Offset: 0x00309C1C
		// (set) Token: 0x06005F8E RID: 24462 RVA: 0x0030B837 File Offset: 0x00309C37
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
		// (get) Token: 0x06005F8F RID: 24463 RVA: 0x0030B844 File Offset: 0x00309C44
		// (set) Token: 0x06005F90 RID: 24464 RVA: 0x0030B85F File Offset: 0x00309C5F
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
		// (get) Token: 0x06005F91 RID: 24465 RVA: 0x0030B86C File Offset: 0x00309C6C
		// (set) Token: 0x06005F92 RID: 24466 RVA: 0x0030B887 File Offset: 0x00309C87
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
		// (get) Token: 0x06005F93 RID: 24467 RVA: 0x0030B89C File Offset: 0x00309C9C
		// (set) Token: 0x06005F94 RID: 24468 RVA: 0x0030B8B7 File Offset: 0x00309CB7
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
		// (get) Token: 0x06005F95 RID: 24469 RVA: 0x0030B8C4 File Offset: 0x00309CC4
		// (set) Token: 0x06005F96 RID: 24470 RVA: 0x0030B8DF File Offset: 0x00309CDF
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

		// Token: 0x06005F97 RID: 24471 RVA: 0x0030B8EC File Offset: 0x00309CEC
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
