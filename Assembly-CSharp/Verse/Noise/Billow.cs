using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F73 RID: 3955
	public class Billow : ModuleBase
	{
		// Token: 0x06005F7F RID: 24447 RVA: 0x0030AE54 File Offset: 0x00309254
		public Billow() : base(0)
		{
		}

		// Token: 0x06005F80 RID: 24448 RVA: 0x0030AEAC File Offset: 0x003092AC
		public Billow(double frequency, double lacunarity, double persistence, int octaves, int seed, QualityMode quality) : base(0)
		{
			this.Frequency = frequency;
			this.Lacunarity = lacunarity;
			this.OctaveCount = octaves;
			this.Persistence = persistence;
			this.Seed = seed;
			this.Quality = quality;
		}

		// Token: 0x17000F4B RID: 3915
		// (get) Token: 0x06005F81 RID: 24449 RVA: 0x0030AF30 File Offset: 0x00309330
		// (set) Token: 0x06005F82 RID: 24450 RVA: 0x0030AF4B File Offset: 0x0030934B
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

		// Token: 0x17000F4C RID: 3916
		// (get) Token: 0x06005F83 RID: 24451 RVA: 0x0030AF58 File Offset: 0x00309358
		// (set) Token: 0x06005F84 RID: 24452 RVA: 0x0030AF73 File Offset: 0x00309373
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

		// Token: 0x17000F4D RID: 3917
		// (get) Token: 0x06005F85 RID: 24453 RVA: 0x0030AF80 File Offset: 0x00309380
		// (set) Token: 0x06005F86 RID: 24454 RVA: 0x0030AF9B File Offset: 0x0030939B
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

		// Token: 0x17000F4E RID: 3918
		// (get) Token: 0x06005F87 RID: 24455 RVA: 0x0030AFA8 File Offset: 0x003093A8
		// (set) Token: 0x06005F88 RID: 24456 RVA: 0x0030AFC3 File Offset: 0x003093C3
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

		// Token: 0x17000F4F RID: 3919
		// (get) Token: 0x06005F89 RID: 24457 RVA: 0x0030AFD8 File Offset: 0x003093D8
		// (set) Token: 0x06005F8A RID: 24458 RVA: 0x0030AFF3 File Offset: 0x003093F3
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

		// Token: 0x17000F50 RID: 3920
		// (get) Token: 0x06005F8B RID: 24459 RVA: 0x0030B000 File Offset: 0x00309400
		// (set) Token: 0x06005F8C RID: 24460 RVA: 0x0030B01B File Offset: 0x0030941B
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

		// Token: 0x06005F8D RID: 24461 RVA: 0x0030B028 File Offset: 0x00309428
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

		// Token: 0x04003ECD RID: 16077
		private double m_frequency = 1.0;

		// Token: 0x04003ECE RID: 16078
		private double m_lacunarity = 2.0;

		// Token: 0x04003ECF RID: 16079
		private QualityMode m_quality = QualityMode.Medium;

		// Token: 0x04003ED0 RID: 16080
		private int m_octaveCount = 6;

		// Token: 0x04003ED1 RID: 16081
		private double m_persistence = 0.5;

		// Token: 0x04003ED2 RID: 16082
		private int m_seed = 0;
	}
}
