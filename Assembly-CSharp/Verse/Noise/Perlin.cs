using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F7B RID: 3963
	public class Perlin : ModuleBase
	{
		// Token: 0x04003EDD RID: 16093
		private double m_frequency = 1.0;

		// Token: 0x04003EDE RID: 16094
		private double m_lacunarity = 2.0;

		// Token: 0x04003EDF RID: 16095
		private QualityMode m_quality = QualityMode.Medium;

		// Token: 0x04003EE0 RID: 16096
		private int m_octaveCount = 6;

		// Token: 0x04003EE1 RID: 16097
		private double m_persistence = 0.5;

		// Token: 0x04003EE2 RID: 16098
		private int m_seed = 0;

		// Token: 0x06005FA7 RID: 24487 RVA: 0x0030B54C File Offset: 0x0030994C
		public Perlin() : base(0)
		{
		}

		// Token: 0x06005FA8 RID: 24488 RVA: 0x0030B5A4 File Offset: 0x003099A4
		public Perlin(double frequency, double lacunarity, double persistence, int octaves, int seed, QualityMode quality) : base(0)
		{
			this.Frequency = frequency;
			this.Lacunarity = lacunarity;
			this.OctaveCount = octaves;
			this.Persistence = persistence;
			this.Seed = seed;
			this.Quality = quality;
		}

		// Token: 0x17000F53 RID: 3923
		// (get) Token: 0x06005FA9 RID: 24489 RVA: 0x0030B628 File Offset: 0x00309A28
		// (set) Token: 0x06005FAA RID: 24490 RVA: 0x0030B643 File Offset: 0x00309A43
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

		// Token: 0x17000F54 RID: 3924
		// (get) Token: 0x06005FAB RID: 24491 RVA: 0x0030B650 File Offset: 0x00309A50
		// (set) Token: 0x06005FAC RID: 24492 RVA: 0x0030B66B File Offset: 0x00309A6B
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

		// Token: 0x17000F55 RID: 3925
		// (get) Token: 0x06005FAD RID: 24493 RVA: 0x0030B678 File Offset: 0x00309A78
		// (set) Token: 0x06005FAE RID: 24494 RVA: 0x0030B693 File Offset: 0x00309A93
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

		// Token: 0x17000F56 RID: 3926
		// (get) Token: 0x06005FAF RID: 24495 RVA: 0x0030B6A0 File Offset: 0x00309AA0
		// (set) Token: 0x06005FB0 RID: 24496 RVA: 0x0030B6BB File Offset: 0x00309ABB
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

		// Token: 0x17000F57 RID: 3927
		// (get) Token: 0x06005FB1 RID: 24497 RVA: 0x0030B6D0 File Offset: 0x00309AD0
		// (set) Token: 0x06005FB2 RID: 24498 RVA: 0x0030B6EB File Offset: 0x00309AEB
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

		// Token: 0x17000F58 RID: 3928
		// (get) Token: 0x06005FB3 RID: 24499 RVA: 0x0030B6F8 File Offset: 0x00309AF8
		// (set) Token: 0x06005FB4 RID: 24500 RVA: 0x0030B713 File Offset: 0x00309B13
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

		// Token: 0x06005FB5 RID: 24501 RVA: 0x0030B720 File Offset: 0x00309B20
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
