using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F80 RID: 3968
	public class Perlin : ModuleBase
	{
		// Token: 0x04003EE8 RID: 16104
		private double m_frequency = 1.0;

		// Token: 0x04003EE9 RID: 16105
		private double m_lacunarity = 2.0;

		// Token: 0x04003EEA RID: 16106
		private QualityMode m_quality = QualityMode.Medium;

		// Token: 0x04003EEB RID: 16107
		private int m_octaveCount = 6;

		// Token: 0x04003EEC RID: 16108
		private double m_persistence = 0.5;

		// Token: 0x04003EED RID: 16109
		private int m_seed = 0;

		// Token: 0x06005FB1 RID: 24497 RVA: 0x0030BE10 File Offset: 0x0030A210
		public Perlin() : base(0)
		{
		}

		// Token: 0x06005FB2 RID: 24498 RVA: 0x0030BE68 File Offset: 0x0030A268
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
		// (get) Token: 0x06005FB3 RID: 24499 RVA: 0x0030BEEC File Offset: 0x0030A2EC
		// (set) Token: 0x06005FB4 RID: 24500 RVA: 0x0030BF07 File Offset: 0x0030A307
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
		// (get) Token: 0x06005FB5 RID: 24501 RVA: 0x0030BF14 File Offset: 0x0030A314
		// (set) Token: 0x06005FB6 RID: 24502 RVA: 0x0030BF2F File Offset: 0x0030A32F
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
		// (get) Token: 0x06005FB7 RID: 24503 RVA: 0x0030BF3C File Offset: 0x0030A33C
		// (set) Token: 0x06005FB8 RID: 24504 RVA: 0x0030BF57 File Offset: 0x0030A357
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
		// (get) Token: 0x06005FB9 RID: 24505 RVA: 0x0030BF64 File Offset: 0x0030A364
		// (set) Token: 0x06005FBA RID: 24506 RVA: 0x0030BF7F File Offset: 0x0030A37F
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
		// (get) Token: 0x06005FBB RID: 24507 RVA: 0x0030BF94 File Offset: 0x0030A394
		// (set) Token: 0x06005FBC RID: 24508 RVA: 0x0030BFAF File Offset: 0x0030A3AF
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
		// (get) Token: 0x06005FBD RID: 24509 RVA: 0x0030BFBC File Offset: 0x0030A3BC
		// (set) Token: 0x06005FBE RID: 24510 RVA: 0x0030BFD7 File Offset: 0x0030A3D7
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

		// Token: 0x06005FBF RID: 24511 RVA: 0x0030BFE4 File Offset: 0x0030A3E4
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
