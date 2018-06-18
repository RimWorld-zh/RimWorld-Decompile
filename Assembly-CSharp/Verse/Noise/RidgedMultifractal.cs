using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F7C RID: 3964
	public class RidgedMultifractal : ModuleBase
	{
		// Token: 0x06005F8D RID: 24461 RVA: 0x00309754 File Offset: 0x00307B54
		public RidgedMultifractal() : base(0)
		{
			this.UpdateWeights();
		}

		// Token: 0x06005F8E RID: 24462 RVA: 0x003097B0 File Offset: 0x00307BB0
		public RidgedMultifractal(double frequency, double lacunarity, int octaves, int seed, QualityMode quality) : base(0)
		{
			this.Frequency = frequency;
			this.Lacunarity = lacunarity;
			this.OctaveCount = octaves;
			this.Seed = seed;
			this.Quality = quality;
		}

		// Token: 0x17000F55 RID: 3925
		// (get) Token: 0x06005F8F RID: 24463 RVA: 0x0030982C File Offset: 0x00307C2C
		// (set) Token: 0x06005F90 RID: 24464 RVA: 0x00309847 File Offset: 0x00307C47
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

		// Token: 0x17000F56 RID: 3926
		// (get) Token: 0x06005F91 RID: 24465 RVA: 0x00309854 File Offset: 0x00307C54
		// (set) Token: 0x06005F92 RID: 24466 RVA: 0x0030986F File Offset: 0x00307C6F
		public double Lacunarity
		{
			get
			{
				return this.m_lacunarity;
			}
			set
			{
				this.m_lacunarity = value;
				this.UpdateWeights();
			}
		}

		// Token: 0x17000F57 RID: 3927
		// (get) Token: 0x06005F93 RID: 24467 RVA: 0x00309880 File Offset: 0x00307C80
		// (set) Token: 0x06005F94 RID: 24468 RVA: 0x0030989B File Offset: 0x00307C9B
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

		// Token: 0x17000F58 RID: 3928
		// (get) Token: 0x06005F95 RID: 24469 RVA: 0x003098A8 File Offset: 0x00307CA8
		// (set) Token: 0x06005F96 RID: 24470 RVA: 0x003098C3 File Offset: 0x00307CC3
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

		// Token: 0x17000F59 RID: 3929
		// (get) Token: 0x06005F97 RID: 24471 RVA: 0x003098D8 File Offset: 0x00307CD8
		// (set) Token: 0x06005F98 RID: 24472 RVA: 0x003098F3 File Offset: 0x00307CF3
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

		// Token: 0x06005F99 RID: 24473 RVA: 0x00309900 File Offset: 0x00307D00
		private void UpdateWeights()
		{
			double num = 1.0;
			for (int i = 0; i < 30; i++)
			{
				this.m_weights[i] = Math.Pow(num, -1.0);
				num *= this.m_lacunarity;
			}
		}

		// Token: 0x06005F9A RID: 24474 RVA: 0x00309950 File Offset: 0x00307D50
		public override double GetValue(double x, double y, double z)
		{
			x *= this.m_frequency;
			y *= this.m_frequency;
			z *= this.m_frequency;
			double num = 0.0;
			double num2 = 1.0;
			double num3 = 1.0;
			double num4 = 2.0;
			for (int i = 0; i < this.m_octaveCount; i++)
			{
				double x2 = Utils.MakeInt32Range(x);
				double y2 = Utils.MakeInt32Range(y);
				double z2 = Utils.MakeInt32Range(z);
				long seed = (long)(this.m_seed + i & int.MaxValue);
				double num5 = Utils.GradientCoherentNoise3D(x2, y2, z2, seed, this.m_quality);
				num5 = Math.Abs(num5);
				num5 = num3 - num5;
				num5 *= num5;
				num5 *= num2;
				num2 = num5 * num4;
				if (num2 > 1.0)
				{
					num2 = 1.0;
				}
				if (num2 < 0.0)
				{
					num2 = 0.0;
				}
				num += num5 * this.m_weights[i];
				x *= this.m_lacunarity;
				y *= this.m_lacunarity;
				z *= this.m_lacunarity;
			}
			return num * 1.25 - 1.0;
		}

		// Token: 0x04003ED1 RID: 16081
		private double m_frequency = 1.0;

		// Token: 0x04003ED2 RID: 16082
		private double m_lacunarity = 2.0;

		// Token: 0x04003ED3 RID: 16083
		private QualityMode m_quality = QualityMode.Medium;

		// Token: 0x04003ED4 RID: 16084
		private int m_octaveCount = 6;

		// Token: 0x04003ED5 RID: 16085
		private int m_seed = 0;

		// Token: 0x04003ED6 RID: 16086
		private double[] m_weights = new double[30];
	}
}
