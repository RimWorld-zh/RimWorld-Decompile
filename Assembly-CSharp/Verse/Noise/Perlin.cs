using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000F7C RID: 3964
	public class Perlin : ModuleBase
	{
		// Token: 0x06005F80 RID: 24448 RVA: 0x003093CC File Offset: 0x003077CC
		public Perlin() : base(0)
		{
		}

		// Token: 0x06005F81 RID: 24449 RVA: 0x00309424 File Offset: 0x00307824
		public Perlin(double frequency, double lacunarity, double persistence, int octaves, int seed, QualityMode quality) : base(0)
		{
			this.Frequency = frequency;
			this.Lacunarity = lacunarity;
			this.OctaveCount = octaves;
			this.Persistence = persistence;
			this.Seed = seed;
			this.Quality = quality;
		}

		// Token: 0x17000F50 RID: 3920
		// (get) Token: 0x06005F82 RID: 24450 RVA: 0x003094A8 File Offset: 0x003078A8
		// (set) Token: 0x06005F83 RID: 24451 RVA: 0x003094C3 File Offset: 0x003078C3
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

		// Token: 0x17000F51 RID: 3921
		// (get) Token: 0x06005F84 RID: 24452 RVA: 0x003094D0 File Offset: 0x003078D0
		// (set) Token: 0x06005F85 RID: 24453 RVA: 0x003094EB File Offset: 0x003078EB
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

		// Token: 0x17000F52 RID: 3922
		// (get) Token: 0x06005F86 RID: 24454 RVA: 0x003094F8 File Offset: 0x003078F8
		// (set) Token: 0x06005F87 RID: 24455 RVA: 0x00309513 File Offset: 0x00307913
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

		// Token: 0x17000F53 RID: 3923
		// (get) Token: 0x06005F88 RID: 24456 RVA: 0x00309520 File Offset: 0x00307920
		// (set) Token: 0x06005F89 RID: 24457 RVA: 0x0030953B File Offset: 0x0030793B
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

		// Token: 0x17000F54 RID: 3924
		// (get) Token: 0x06005F8A RID: 24458 RVA: 0x00309550 File Offset: 0x00307950
		// (set) Token: 0x06005F8B RID: 24459 RVA: 0x0030956B File Offset: 0x0030796B
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

		// Token: 0x17000F55 RID: 3925
		// (get) Token: 0x06005F8C RID: 24460 RVA: 0x00309578 File Offset: 0x00307978
		// (set) Token: 0x06005F8D RID: 24461 RVA: 0x00309593 File Offset: 0x00307993
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

		// Token: 0x06005F8E RID: 24462 RVA: 0x003095A0 File Offset: 0x003079A0
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

		// Token: 0x04003ECC RID: 16076
		private double m_frequency = 1.0;

		// Token: 0x04003ECD RID: 16077
		private double m_lacunarity = 2.0;

		// Token: 0x04003ECE RID: 16078
		private QualityMode m_quality = QualityMode.Medium;

		// Token: 0x04003ECF RID: 16079
		private int m_octaveCount = 6;

		// Token: 0x04003ED0 RID: 16080
		private double m_persistence = 0.5;

		// Token: 0x04003ED1 RID: 16081
		private int m_seed = 0;
	}
}
