using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F80 RID: 3968
	public class RidgedMultifractal : ModuleBase
	{
		// Token: 0x04003EE6 RID: 16102
		private double m_frequency = 1.0;

		// Token: 0x04003EE7 RID: 16103
		private double m_lacunarity = 2.0;

		// Token: 0x04003EE8 RID: 16104
		private QualityMode m_quality = QualityMode.Medium;

		// Token: 0x04003EE9 RID: 16105
		private int m_octaveCount = 6;

		// Token: 0x04003EEA RID: 16106
		private int m_seed = 0;

		// Token: 0x04003EEB RID: 16107
		private double[] m_weights = new double[30];

		// Token: 0x06005FC0 RID: 24512 RVA: 0x0030BE78 File Offset: 0x0030A278
		public RidgedMultifractal() : base(0)
		{
			this.UpdateWeights();
		}

		// Token: 0x06005FC1 RID: 24513 RVA: 0x0030BED4 File Offset: 0x0030A2D4
		public RidgedMultifractal(double frequency, double lacunarity, int octaves, int seed, QualityMode quality) : base(0)
		{
			this.Frequency = frequency;
			this.Lacunarity = lacunarity;
			this.OctaveCount = octaves;
			this.Seed = seed;
			this.Quality = quality;
		}

		// Token: 0x17000F58 RID: 3928
		// (get) Token: 0x06005FC2 RID: 24514 RVA: 0x0030BF50 File Offset: 0x0030A350
		// (set) Token: 0x06005FC3 RID: 24515 RVA: 0x0030BF6B File Offset: 0x0030A36B
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

		// Token: 0x17000F59 RID: 3929
		// (get) Token: 0x06005FC4 RID: 24516 RVA: 0x0030BF78 File Offset: 0x0030A378
		// (set) Token: 0x06005FC5 RID: 24517 RVA: 0x0030BF93 File Offset: 0x0030A393
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

		// Token: 0x17000F5A RID: 3930
		// (get) Token: 0x06005FC6 RID: 24518 RVA: 0x0030BFA4 File Offset: 0x0030A3A4
		// (set) Token: 0x06005FC7 RID: 24519 RVA: 0x0030BFBF File Offset: 0x0030A3BF
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

		// Token: 0x17000F5B RID: 3931
		// (get) Token: 0x06005FC8 RID: 24520 RVA: 0x0030BFCC File Offset: 0x0030A3CC
		// (set) Token: 0x06005FC9 RID: 24521 RVA: 0x0030BFE7 File Offset: 0x0030A3E7
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

		// Token: 0x17000F5C RID: 3932
		// (get) Token: 0x06005FCA RID: 24522 RVA: 0x0030BFFC File Offset: 0x0030A3FC
		// (set) Token: 0x06005FCB RID: 24523 RVA: 0x0030C017 File Offset: 0x0030A417
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

		// Token: 0x06005FCC RID: 24524 RVA: 0x0030C024 File Offset: 0x0030A424
		private void UpdateWeights()
		{
			double num = 1.0;
			for (int i = 0; i < 30; i++)
			{
				this.m_weights[i] = Math.Pow(num, -1.0);
				num *= this.m_lacunarity;
			}
		}

		// Token: 0x06005FCD RID: 24525 RVA: 0x0030C074 File Offset: 0x0030A474
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
