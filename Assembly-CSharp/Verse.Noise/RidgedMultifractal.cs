using System;
using UnityEngine;

namespace Verse.Noise
{
	public class RidgedMultifractal : ModuleBase
	{
		private double m_frequency = 1.0;

		private double m_lacunarity = 2.0;

		private QualityMode m_quality = QualityMode.Medium;

		private int m_octaveCount = 6;

		private int m_seed;

		private double[] m_weights = new double[30];

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

		public RidgedMultifractal()
			: base(0)
		{
			this.UpdateWeights();
		}

		public RidgedMultifractal(double frequency, double lacunarity, int octaves, int seed, QualityMode quality)
			: base(0)
		{
			this.Frequency = frequency;
			this.Lacunarity = lacunarity;
			this.OctaveCount = octaves;
			this.Seed = seed;
			this.Quality = quality;
		}

		private void UpdateWeights()
		{
			double num = 1.0;
			for (int i = 0; i < 30; i++)
			{
				this.m_weights[i] = Math.Pow(num, -1.0);
				num *= this.m_lacunarity;
			}
		}

		public override double GetValue(double x, double y, double z)
		{
			x *= this.m_frequency;
			y *= this.m_frequency;
			z *= this.m_frequency;
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 1.0;
			double num4 = 1.0;
			double num5 = 2.0;
			for (int i = 0; i < this.m_octaveCount; i++)
			{
				double x2 = Utils.MakeInt32Range(x);
				double y2 = Utils.MakeInt32Range(y);
				double z2 = Utils.MakeInt32Range(z);
				long seed = this.m_seed + i & 0x7FFFFFFF;
				num = Utils.GradientCoherentNoise3D(x2, y2, z2, seed, this.m_quality);
				num = Math.Abs(num);
				num = num4 - num;
				num *= num;
				num *= num3;
				num3 = num * num5;
				if (num3 > 1.0)
				{
					num3 = 1.0;
				}
				if (num3 < 0.0)
				{
					num3 = 0.0;
				}
				num2 += num * this.m_weights[i];
				x *= this.m_lacunarity;
				y *= this.m_lacunarity;
				z *= this.m_lacunarity;
			}
			return num2 * 1.25 - 1.0;
		}
	}
}
