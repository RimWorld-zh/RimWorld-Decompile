using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200058A RID: 1418
	internal class DebugWorldLine
	{
		// Token: 0x06001B0D RID: 6925 RVA: 0x000E8441 File Offset: 0x000E6841
		public DebugWorldLine(Vector3 a, Vector3 b, bool onPlanetSurface)
		{
			this.a = a;
			this.b = b;
			this.onPlanetSurface = onPlanetSurface;
			this.ticksLeft = 100;
		}

		// Token: 0x06001B0E RID: 6926 RVA: 0x000E8467 File Offset: 0x000E6867
		public DebugWorldLine(Vector3 a, Vector3 b, bool onPlanetSurface, int ticksLeft)
		{
			this.a = a;
			this.b = b;
			this.onPlanetSurface = onPlanetSurface;
			this.ticksLeft = ticksLeft;
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06001B0F RID: 6927 RVA: 0x000E8490 File Offset: 0x000E6890
		// (set) Token: 0x06001B10 RID: 6928 RVA: 0x000E84AB File Offset: 0x000E68AB
		public int TicksLeft
		{
			get
			{
				return this.ticksLeft;
			}
			set
			{
				this.ticksLeft = value;
			}
		}

		// Token: 0x06001B11 RID: 6929 RVA: 0x000E84B8 File Offset: 0x000E68B8
		public void Draw()
		{
			float num = Vector3.Distance(this.a, this.b);
			if (num >= 0.001f)
			{
				if (this.onPlanetSurface)
				{
					float averageTileSize = Find.WorldGrid.averageTileSize;
					int num2 = Mathf.Max(Mathf.RoundToInt(num / averageTileSize), 0);
					float num3 = 0.05f;
					for (int i = 0; i < num2; i++)
					{
						Vector3 vector = Vector3.Lerp(this.a, this.b, (float)i / (float)num2);
						Vector3 vector2 = Vector3.Lerp(this.a, this.b, (float)(i + 1) / (float)num2);
						vector = vector.normalized * (100f + num3);
						vector2 = vector2.normalized * (100f + num3);
						GenDraw.DrawWorldLineBetween(vector, vector2);
					}
				}
				else
				{
					GenDraw.DrawWorldLineBetween(this.a, this.b);
				}
			}
		}

		// Token: 0x04000FEF RID: 4079
		public Vector3 a;

		// Token: 0x04000FF0 RID: 4080
		public Vector3 b;

		// Token: 0x04000FF1 RID: 4081
		public int ticksLeft;

		// Token: 0x04000FF2 RID: 4082
		private bool onPlanetSurface;
	}
}
