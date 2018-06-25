using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000588 RID: 1416
	internal class DebugWorldLine
	{
		// Token: 0x04000FEC RID: 4076
		public Vector3 a;

		// Token: 0x04000FED RID: 4077
		public Vector3 b;

		// Token: 0x04000FEE RID: 4078
		public int ticksLeft;

		// Token: 0x04000FEF RID: 4079
		private bool onPlanetSurface;

		// Token: 0x06001B08 RID: 6920 RVA: 0x000E85E5 File Offset: 0x000E69E5
		public DebugWorldLine(Vector3 a, Vector3 b, bool onPlanetSurface)
		{
			this.a = a;
			this.b = b;
			this.onPlanetSurface = onPlanetSurface;
			this.ticksLeft = 100;
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x000E860B File Offset: 0x000E6A0B
		public DebugWorldLine(Vector3 a, Vector3 b, bool onPlanetSurface, int ticksLeft)
		{
			this.a = a;
			this.b = b;
			this.onPlanetSurface = onPlanetSurface;
			this.ticksLeft = ticksLeft;
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06001B0A RID: 6922 RVA: 0x000E8634 File Offset: 0x000E6A34
		// (set) Token: 0x06001B0B RID: 6923 RVA: 0x000E864F File Offset: 0x000E6A4F
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

		// Token: 0x06001B0C RID: 6924 RVA: 0x000E865C File Offset: 0x000E6A5C
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
	}
}
