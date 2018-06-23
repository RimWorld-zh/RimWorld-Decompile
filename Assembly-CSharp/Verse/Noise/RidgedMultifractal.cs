using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F7C RID: 3964
	public class RidgedMultifractal : ModuleBase
	{
		// Token: 0x04003EE3 RID: 16099
		private double m_frequency = 1.0;

		// Token: 0x04003EE4 RID: 16100
		private double m_lacunarity = 2.0;

		// Token: 0x04003EE5 RID: 16101
		private QualityMode m_quality = QualityMode.Medium;

		// Token: 0x04003EE6 RID: 16102
		private int m_octaveCount = 6;

		// Token: 0x04003EE7 RID: 16103
		private int m_seed = 0;

		// Token: 0x04003EE8 RID: 16104
		private double[] m_weights = new double[30];

		// Token: 0x06005FB6 RID: 24502 RVA: 0x0030B7F8 File Offset: 0x00309BF8
		public RidgedMultifractal() : base(0)
		{
			this.UpdateWeights();
		}

		// Token: 0x06005FB7 RID: 24503 RVA: 0x0030B854 File Offset: 0x00309C54
		public RidgedMultifractal(double frequency, double lacunarity, int octaves, int seed, QualityMode quality) : base(0)
		{
			this.Frequency = frequency;
			this.Lacunarity = lacunarity;
			this.OctaveCount = octaves;
			this.Seed = seed;
			this.Quality = quality;
		}

		// Token: 0x17000F59 RID: 3929
		// (get) Token: 0x06005FB8 RID: 24504 RVA: 0x0030B8D0 File Offset: 0x00309CD0
		// (set) Token: 0x06005FB9 RID: 24505 RVA: 0x0030B8EB File Offset: 0x00309CEB
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

		// Token: 0x17000F5A RID: 3930
		// (get) Token: 0x06005FBA RID: 24506 RVA: 0x0030B8F8 File Offset: 0x00309CF8
		// (set) Token: 0x06005FBB RID: 24507 RVA: 0x0030B913 File Offset: 0x00309D13
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

		// Token: 0x17000F5B RID: 3931
		// (get) Token: 0x06005FBC RID: 24508 RVA: 0x0030B924 File Offset: 0x00309D24
		// (set) Token: 0x06005FBD RID: 24509 RVA: 0x0030B93F File Offset: 0x00309D3F
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

		// Token: 0x17000F5C RID: 3932
		// (get) Token: 0x06005FBE RID: 24510 RVA: 0x0030B94C File Offset: 0x00309D4C
		// (set) Token: 0x06005FBF RID: 24511 RVA: 0x0030B967 File Offset: 0x00309D67
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

		// Token: 0x17000F5D RID: 3933
		// (get) Token: 0x06005FC0 RID: 24512 RVA: 0x0030B97C File Offset: 0x00309D7C
		// (set) Token: 0x06005FC1 RID: 24513 RVA: 0x0030B997 File Offset: 0x00309D97
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

		// Token: 0x06005FC2 RID: 24514 RVA: 0x0030B9A4 File Offset: 0x00309DA4
		private void UpdateWeights()
		{
			double num = 1.0;
			for (int i = 0; i < 30; i++)
			{
				this.m_weights[i] = Math.Pow(num, -1.0);
				num *= this.m_lacunarity;
			}
		}

		// Token: 0x06005FC3 RID: 24515 RVA: 0x0030B9F4 File Offset: 0x00309DF4
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
	}
}
