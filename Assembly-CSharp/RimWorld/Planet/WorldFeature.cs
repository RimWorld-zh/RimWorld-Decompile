using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000570 RID: 1392
	public class WorldFeature : IExposable, ILoadReferenceable
	{
		// Token: 0x04000F66 RID: 3942
		public int uniqueID;

		// Token: 0x04000F67 RID: 3943
		public FeatureDef def;

		// Token: 0x04000F68 RID: 3944
		public string name;

		// Token: 0x04000F69 RID: 3945
		public Vector3 drawCenter;

		// Token: 0x04000F6A RID: 3946
		public float drawAngle;

		// Token: 0x04000F6B RID: 3947
		public float maxDrawSizeInTiles;

		// Token: 0x04000F6C RID: 3948
		public float alpha;

		// Token: 0x04000F6D RID: 3949
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
				new CurvePoint(50f, 100f),
				true
			},
			{
				new CurvePoint(100f, 200f),
				true
			},
			{
				new CurvePoint(200f, 400f),
				true
			}
		};

		// Token: 0x04000F6E RID: 3950
		[TweakValue("Interface.World", 0f, 40f)]
		protected static float FeatureSizePoint10 = 15f;

		// Token: 0x04000F6F RID: 3951
		[TweakValue("Interface.World", 0f, 100f)]
		protected static float FeatureSizePoint25 = 40f;

		// Token: 0x04000F70 RID: 3952
		[TweakValue("Interface.World", 0f, 200f)]
		protected static float FeatureSizePoint50 = 100f;

		// Token: 0x04000F71 RID: 3953
		[TweakValue("Interface.World", 0f, 400f)]
		protected static float FeatureSizePoint100 = 200f;

		// Token: 0x04000F72 RID: 3954
		[TweakValue("Interface.World", 0f, 800f)]
		protected static float FeatureSizePoint200 = 400f;

		// Token: 0x06001A63 RID: 6755 RVA: 0x000E4843 File Offset: 0x000E2C43
		protected static void FeatureSizePoint10_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x06001A64 RID: 6756 RVA: 0x000E484B File Offset: 0x000E2C4B
		protected static void FeatureSizePoint25_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x000E4853 File Offset: 0x000E2C53
		protected static void FeatureSizePoint50_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x06001A66 RID: 6758 RVA: 0x000E485B File Offset: 0x000E2C5B
		protected static void FeatureSizePoint100_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x06001A67 RID: 6759 RVA: 0x000E4863 File Offset: 0x000E2C63
		protected static void FeatureSizePoint200_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x06001A68 RID: 6760 RVA: 0x000E486C File Offset: 0x000E2C6C
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
		// (get) Token: 0x06001A69 RID: 6761 RVA: 0x000E4950 File Offset: 0x000E2D50
		public float EffectiveDrawSize
		{
			get
			{
				return WorldFeature.EffectiveDrawSizeCurve.Evaluate(this.maxDrawSizeInTiles);
			}
		}

		// Token: 0x06001A6A RID: 6762 RVA: 0x000E4978 File Offset: 0x000E2D78
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

		// Token: 0x06001A6B RID: 6763 RVA: 0x000E4A14 File Offset: 0x000E2E14
		public string GetUniqueLoadID()
		{
			return "WorldFeature_" + this.uniqueID;
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06001A6C RID: 6764 RVA: 0x000E4A40 File Offset: 0x000E2E40
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
