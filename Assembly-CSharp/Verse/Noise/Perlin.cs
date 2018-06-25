using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F7F RID: 3967
	public class Perlin : ModuleBase
	{
		// Token: 0x04003EE0 RID: 16096
		private double m_frequency = 1.0;

		// Token: 0x04003EE1 RID: 16097
		private double m_lacunarity = 2.0;

		// Token: 0x04003EE2 RID: 16098
		private QualityMode m_quality = QualityMode.Medium;

		// Token: 0x04003EE3 RID: 16099
		private int m_octaveCount = 6;

		// Token: 0x04003EE4 RID: 16100
		private double m_persistence = 0.5;

		// Token: 0x04003EE5 RID: 16101
		private int m_seed = 0;

		// Token: 0x06005FB1 RID: 24497 RVA: 0x0030BBCC File Offset: 0x00309FCC
		public Perlin() : base(0)
		{
		}

		// Token: 0x06005FB2 RID: 24498 RVA: 0x0030BC24 File Offset: 0x0030A024
		public Perlin(double frequency, double lacunarity, double persistence, int octaves, int seed, QualityMode quality) : base(0)
		{
			this.Frequency = frequency;
			this.Lacunarity = lacunarity;
			this.OctaveCount = octaves;
			this.Persistence = persistence;
			this.Seed = seed;
			this.Quality = quality;
		}

		// Token: 0x17000F52 RID: 3922
		// (get) Token: 0x06005FB3 RID: 24499 RVA: 0x0030BCA8 File Offset: 0x0030A0A8
		// (set) Token: 0x06005FB4 RID: 24500 RVA: 0x0030BCC3 File Offset: 0x0030A0C3
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

		// Token: 0x17000F53 RID: 3923
		// (get) Token: 0x06005FB5 RID: 24501 RVA: 0x0030BCD0 File Offset: 0x0030A0D0
		// (set) Token: 0x06005FB6 RID: 24502 RVA: 0x0030BCEB File Offset: 0x0030A0EB
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

		// Token: 0x17000F54 RID: 3924
		// (get) Token: 0x06005FB7 RID: 24503 RVA: 0x0030BCF8 File Offset: 0x0030A0F8
		// (set) Token: 0x06005FB8 RID: 24504 RVA: 0x0030BD13 File Offset: 0x0030A113
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

		// Token: 0x17000F55 RID: 3925
		// (get) Token: 0x06005FB9 RID: 24505 RVA: 0x0030BD20 File Offset: 0x0030A120
		// (set) Token: 0x06005FBA RID: 24506 RVA: 0x0030BD3B File Offset: 0x0030A13B
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

		// Token: 0x17000F56 RID: 3926
		// (get) Token: 0x06005FBB RID: 24507 RVA: 0x0030BD50 File Offset: 0x0030A150
		// (set) Token: 0x06005FBC RID: 24508 RVA: 0x0030BD6B File Offset: 0x0030A16B
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

		// Token: 0x17000F57 RID: 3927
		// (get) Token: 0x06005FBD RID: 24509 RVA: 0x0030BD78 File Offset: 0x0030A178
		// (set) Token: 0x06005FBE RID: 24510 RVA: 0x0030BD93 File Offset: 0x0030A193
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

		// Token: 0x06005FBF RID: 24511 RVA: 0x0030BDA0 File Offset: 0x0030A1A0
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
				num += num3 * num2;
				x *= this.m_lacunarity;
				y *= this.m_lacunarity;
				z *= this.m_lacunarity;
				num2 *= this.m_persistence;
			}
			return num;
		}
	}
}
