using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000572 RID: 1394
	public class WorldFeature : IExposable, ILoadReferenceable
	{
		// Token: 0x04000F6A RID: 3946
		public int uniqueID;

		// Token: 0x04000F6B RID: 3947
		public FeatureDef def;

		// Token: 0x04000F6C RID: 3948
		public string name;

		// Token: 0x04000F6D RID: 3949
		public Vector3 drawCenter;

		// Token: 0x04000F6E RID: 3950
		public float drawAngle;

		// Token: 0x04000F6F RID: 3951
		public float maxDrawSizeInTiles;

		// Token: 0x04000F70 RID: 3952
		public float alpha;

		// Token: 0x04000F71 RID: 3953
		protected static SimpleCurve EffectiveDrawSizeCurve = new SimpleCurve
		{
			{
				new CurvePoint(10f, 15f),
				true
			},
			{
				new CurvePoint(25f, 40f),
				true
			},
			{
				new CurvePoint(50f, 90f),
				true
			},
			{
				new CurvePoint(100f, 150f),
				true
			},
			{
				new CurvePoint(200f, 200f),
				true
			}
		};

		// Token: 0x04000F72 RID: 3954
		[TweakValue("Interface.World", 0f, 40f)]
		protected static float FeatureSizePoint10 = 15f;

		// Token: 0x04000F73 RID: 3955
		[TweakValue("Interface.World", 0f, 100f)]
		protected static float FeatureSizePoint25 = 40f;

		// Token: 0x04000F74 RID: 3956
		[TweakValue("Interface.World", 0f, 200f)]
		protected static float FeatureSizePoint50 = 90f;

		// Token: 0x04000F75 RID: 3957
		[TweakValue("Interface.World", 0f, 400f)]
		protected static float FeatureSizePoint100 = 150f;

		// Token: 0x04000F76 RID: 3958
		[TweakValue("Interface.World", 0f, 800f)]
		protected static float FeatureSizePoint200 = 200f;

		// Token: 0x06001A66 RID: 6758 RVA: 0x000E4BFB File Offset: 0x000E2FFB
		protected static void FeatureSizePoint10_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x06001A67 RID: 6759 RVA: 0x000E4C03 File Offset: 0x000E3003
		protected static void FeatureSizePoint25_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x06001A68 RID: 6760 RVA: 0x000E4C0B File Offset: 0x000E300B
		protected static void FeatureSizePoint50_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x06001A69 RID: 6761 RVA: 0x000E4C13 File Offset: 0x000E3013
		protected static void FeatureSizePoint100_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x06001A6A RID: 6762 RVA: 0x000E4C1B File Offset: 0x000E301B
		protected static void FeatureSizePoint200_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x000E4C24 File Offset: 0x000E3024
		private static void TweakChanged()
		{
			Find.WorldFeatures.textsCreated = false;
			WorldFeature.EffectiveDrawSizeCurve[0] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[0].x, WorldFeature.FeatureSizePoint10);
			WorldFeature.EffectiveDrawSizeCurve[1] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[1].x, WorldFeature.FeatureSizePoint25);
			WorldFeature.EffectiveDrawSizeCurve[2] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[2].x, WorldFeature.FeatureSizePoint50);
			WorldFeature.EffectiveDrawSizeCurve[3] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[3].x, WorldFeature.FeatureSizePoint100);
			WorldFeature.EffectiveDrawSizeCurve[4] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[4].x, WorldFeature.FeatureSizePoint200);
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06001A6C RID: 6764 RVA: 0x000E4D08 File Offset: 0x000E3108
		public float EffectiveDrawSize
		{
			get
			{
				return WorldFeature.EffectiveDrawSizeCurve.Evaluate(this.maxDrawSizeInTiles);
			}
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x000E4D30 File Offset: 0x000E3130
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.uniqueID, "uniqueID", 0, false);
			Scribe_Defs.Look<FeatureDef>(ref this.def, "def");
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<Vector3>(ref this.drawCenter, "drawCenter", default(Vector3), false);
			Scribe_Values.Look<float>(ref this.drawAngle, "drawAngle", 0f, false);
			Scribe_Values.Look<float>(ref this.maxDrawSizeInTiles, "maxDrawSizeInTiles", 0f, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				BackCompatibility.WorldFeatureLoadingVars(this);
			}
		}

		// Token: 0x06001A6E RID: 6766 RVA: 0x000E4DCC File Offset: 0x000E31CC
		public string GetUniqueLoadID()
		{
			return "WorldFeature_" + this.uniqueID;
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06001A6F RID: 6767 RVA: 0x000E4DF8 File Offset: 0x000E31F8
		public IEnumerable<int> Tiles
		{
			get
			{
				WorldGrid worldGrid = Find.WorldGrid;
				int tilesCount = worldGrid.TilesCount;
				for (int i = 0; i < tilesCount; i++)
				{
					Tile t = worldGrid[i];
					if (t.feature == this)
					{
						yield return i;
					}
				}
				yield break;
			}
		}
	}
}
