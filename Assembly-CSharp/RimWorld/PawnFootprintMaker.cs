using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200045E RID: 1118
	public class PawnFootprintMaker
	{
		// Token: 0x04000BE5 RID: 3045
		private Pawn pawn;

		// Token: 0x04000BE6 RID: 3046
		private Vector3 lastFootprintPlacePos;

		// Token: 0x04000BE7 RID: 3047
		private bool lastFootprintRight = false;

		// Token: 0x04000BE8 RID: 3048
		private const float FootprintIntervalDist = 0.632f;

		// Token: 0x04000BE9 RID: 3049
		private static readonly Vector3 FootprintOffset = new Vector3(0f, 0f, -0.3f);

		// Token: 0x04000BEA RID: 3050
		private const float LeftRightOffsetDist = 0.17f;

		// Token: 0x04000BEB RID: 3051
		private const float FootprintSplashSize = 2f;

		// Token: 0x0600139A RID: 5018 RVA: 0x000A96CB File Offset: 0x000A7ACB
		public PawnFootprintMaker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x000A96E4 File Offset: 0x000A7AE4
		public void FootprintMakerTick()
		{
			if (!this.pawn.RaceProps.makesFootprints)
			{
				TerrainDef terrain = this.pawn.Position.GetTerrain(this.pawn.Map);
				if (terrain == null || !terrain.takeSplashes)
				{
					return;
				}
			}
			if ((this.pawn.Drawer.DrawPos - this.lastFootprintPlacePos).MagnitudeHorizontalSquared() > 0.399424046f)
			{
				this.TryPlaceFootprint();
			}
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x000A976C File Offset: 0x000A7B6C
		private void TryPlaceFootprint()
		{
			Vector3 drawPos = this.pawn.Drawer.DrawPos;
			Vector3 normalized = (drawPos - this.lastFootprintPlacePos).normalized;
			float rot = normalized.AngleFlat();
			float angle = (float)((!this.lastFootprintRight) ? -90 : 90);
			Vector3 b = normalized.RotatedBy(angle) * 0.17f * Mathf.Sqrt(this.pawn.BodySize);
			Vector3 vector = drawPos + PawnFootprintMaker.FootprintOffset + b;
			IntVec3 c = vector.ToIntVec3();
			if (c.InBounds(this.pawn.Map))
			{
				TerrainDef terrain = c.GetTerrain(this.pawn.Map);
				if (terrain != null)
				{
					if (terrain.takeSplashes)
					{
						MoteMaker.MakeWaterSplash(vector, this.pawn.Map, Mathf.Sqrt(this.pawn.BodySize) * 2f, 1.5f);
					}
					if (this.pawn.RaceProps.makesFootprints && terrain.takeFootprints && this.pawn.Map.snowGrid.GetDepth(this.pawn.Position) >= 0.4f)
					{
						MoteMaker.PlaceFootprint(vector, this.pawn.Map, rot);
					}
				}
			}
			this.lastFootprintPlacePos = drawPos;
			this.lastFootprintRight = !this.lastFootprintRight;
		}
	}
}
