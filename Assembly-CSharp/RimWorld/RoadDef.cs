using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002C4 RID: 708
	public class RoadDef : Def
	{
		// Token: 0x06000BCC RID: 3020 RVA: 0x00069540 File Offset: 0x00067940
		public float GetLayerWidth(RoadWorldLayerDef def)
		{
			if (this.cachedLayerWidth == null)
			{
				this.cachedLayerWidth = new float[DefDatabase<RoadWorldLayerDef>.DefCount];
				for (int i = 0; i < DefDatabase<RoadWorldLayerDef>.DefCount; i++)
				{
					RoadWorldLayerDef roadWorldLayerDef = DefDatabase<RoadWorldLayerDef>.AllDefsListForReading[i];
					if (this.worldRenderSteps != null)
					{
						foreach (RoadDef.WorldRenderStep worldRenderStep in this.worldRenderSteps)
						{
							if (worldRenderStep.layer == roadWorldLayerDef)
							{
								this.cachedLayerWidth[(int)roadWorldLayerDef.index] = worldRenderStep.width;
							}
						}
					}
				}
			}
			return this.cachedLayerWidth[(int)def.index];
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x0006961C File Offset: 0x00067A1C
		public override void ClearCachedData()
		{
			base.ClearCachedData();
			this.cachedLayerWidth = null;
		}

		// Token: 0x040006F0 RID: 1776
		public int priority = 0;

		// Token: 0x040006F1 RID: 1777
		public bool ancientOnly = false;

		// Token: 0x040006F2 RID: 1778
		public float movementCostMultiplier = 1f;

		// Token: 0x040006F3 RID: 1779
		public int tilesPerSegment = 15;

		// Token: 0x040006F4 RID: 1780
		public RoadPathingDef pathingMode = null;

		// Token: 0x040006F5 RID: 1781
		public List<RoadDefGenStep> roadGenSteps;

		// Token: 0x040006F6 RID: 1782
		public List<RoadDef.WorldRenderStep> worldRenderSteps;

		// Token: 0x040006F7 RID: 1783
		[NoTranslate]
		public string worldTransitionGroup = "";

		// Token: 0x040006F8 RID: 1784
		public float distortionFrequency = 1f;

		// Token: 0x040006F9 RID: 1785
		public float distortionIntensity = 0f;

		// Token: 0x040006FA RID: 1786
		[Unsaved]
		private float[] cachedLayerWidth = null;

		// Token: 0x020002C5 RID: 709
		public class WorldRenderStep
		{
			// Token: 0x040006FB RID: 1787
			public RoadWorldLayerDef layer;

			// Token: 0x040006FC RID: 1788
			public float width;
		}
	}
}
