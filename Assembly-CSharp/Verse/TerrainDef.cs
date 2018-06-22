using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BA2 RID: 2978
	public class TerrainDef : BuildableDef
	{
		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x06004071 RID: 16497 RVA: 0x0021DFAC File Offset: 0x0021C3AC
		public bool Removable
		{
			get
			{
				return this.layerable;
			}
		}

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x06004072 RID: 16498 RVA: 0x0021DFC8 File Offset: 0x0021C3C8
		public bool IsCarpet
		{
			get
			{
				return this.researchPrerequisites != null && this.researchPrerequisites.Contains(ResearchProjectDefOf.CarpetMaking);
			}
		}

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x06004073 RID: 16499 RVA: 0x0021DFFC File Offset: 0x0021C3FC
		public bool IsRiver
		{
			get
			{
				return this.HasTag("River");
			}
		}

		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x06004074 RID: 16500 RVA: 0x0021E01C File Offset: 0x0021C41C
		public bool IsWater
		{
			get
			{
				return this.HasTag("Water");
			}
		}

		// Token: 0x06004075 RID: 16501 RVA: 0x0021E03C File Offset: 0x0021C43C
		public override void PostLoad()
		{
			this.placingDraggableDimensions = 2;
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				Shader shader = null;
				switch (this.edgeType)
				{
				case TerrainDef.TerrainEdgeType.Hard:
					shader = ShaderDatabase.TerrainHard;
					break;
				case TerrainDef.TerrainEdgeType.Fade:
					shader = ShaderDatabase.TerrainFade;
					break;
				case TerrainDef.TerrainEdgeType.FadeRough:
					shader = ShaderDatabase.TerrainFadeRough;
					break;
				case TerrainDef.TerrainEdgeType.Water:
					shader = ShaderDatabase.TerrainWater;
					break;
				}
				this.graphic = GraphicDatabase.Get<Graphic_Terrain>(this.texturePath, shader, Vector2.one, this.color, 2000 + this.renderPrecedence);
				if (shader == ShaderDatabase.TerrainFadeRough || shader == ShaderDatabase.TerrainWater)
				{
					this.graphic.MatSingle.SetTexture("_AlphaAddTex", TexGame.AlphaAddTex);
				}
				if (!this.waterDepthShader.NullOrEmpty())
				{
					this.waterDepthMaterial = MaterialAllocator.Create(ShaderDatabase.LoadShader(this.waterDepthShader));
					this.waterDepthMaterial.renderQueue = 2000 + this.renderPrecedence;
					this.waterDepthMaterial.SetTexture("_AlphaAddTex", TexGame.AlphaAddTex);
					if (this.waterDepthShaderParameters != null)
					{
						for (int i = 0; i < this.waterDepthShaderParameters.Count; i++)
						{
							this.waterDepthShaderParameters[i].Apply(this.waterDepthMaterial);
						}
					}
				}
			});
			base.PostLoad();
		}

		// Token: 0x06004076 RID: 16502 RVA: 0x0021E05D File Offset: 0x0021C45D
		protected override void ResolveIcon()
		{
			base.ResolveIcon();
			this.uiIconColor = this.color;
		}

		// Token: 0x06004077 RID: 16503 RVA: 0x0021E074 File Offset: 0x0021C474
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string err in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return err;
			}
			if (this.texturePath.NullOrEmpty())
			{
				yield return "missing texturePath";
			}
			if (this.fertility < 0f)
			{
				yield return "Terrain Def " + this + " has no fertility value set.";
			}
			if (this.renderPrecedence > 400)
			{
				yield return "Render order " + this.renderPrecedence + " is out of range (must be < 400)";
			}
			if (this.generatedFilth != null && this.acceptTerrainSourceFilth)
			{
				yield return this.defName + " makes terrain filth and also accepts it.";
			}
			if (this.Flammable() && this.burnedDef == null && !this.layerable)
			{
				yield return "flammable but burnedDef is null and not layerable";
			}
			if (this.burnedDef != null && this.burnedDef.Flammable())
			{
				yield return "burnedDef is flammable";
			}
			yield break;
		}

		// Token: 0x06004078 RID: 16504 RVA: 0x0021E0A0 File Offset: 0x0021C4A0
		public static TerrainDef Named(string defName)
		{
			return DefDatabase<TerrainDef>.GetNamed(defName, true);
		}

		// Token: 0x06004079 RID: 16505 RVA: 0x0021E0BC File Offset: 0x0021C4BC
		public bool HasTag(string tag)
		{
			return this.tags != null && this.tags.Contains(tag);
		}

		// Token: 0x0600407A RID: 16506 RVA: 0x0021E0EC File Offset: 0x0021C4EC
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry stat in this.<SpecialDisplayStats>__BaseCallProxy1())
			{
				yield return stat;
			}
			string[] affordance = (from ta in this.affordances.Distinct<TerrainAffordanceDef>()
			orderby ta.order
			select ta.label).ToArray<string>();
			if (affordance.Length > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Supports".Translate(), affordance.ToCommaList(false).CapitalizeFirst(), 0, "");
			}
			yield break;
		}

		// Token: 0x04002B64 RID: 11108
		[NoTranslate]
		public string texturePath;

		// Token: 0x04002B65 RID: 11109
		public TerrainDef.TerrainEdgeType edgeType = TerrainDef.TerrainEdgeType.Hard;

		// Token: 0x04002B66 RID: 11110
		[NoTranslate]
		public string waterDepthShader = null;

		// Token: 0x04002B67 RID: 11111
		public List<ShaderParameter> waterDepthShaderParameters = null;

		// Token: 0x04002B68 RID: 11112
		public int renderPrecedence = 0;

		// Token: 0x04002B69 RID: 11113
		public List<TerrainAffordanceDef> affordances = new List<TerrainAffordanceDef>();

		// Token: 0x04002B6A RID: 11114
		public bool layerable = false;

		// Token: 0x04002B6B RID: 11115
		[NoTranslate]
		public string scatterType = null;

		// Token: 0x04002B6C RID: 11116
		public bool takeFootprints = false;

		// Token: 0x04002B6D RID: 11117
		public bool takeSplashes = false;

		// Token: 0x04002B6E RID: 11118
		public bool avoidWander = false;

		// Token: 0x04002B6F RID: 11119
		public bool changeable = true;

		// Token: 0x04002B70 RID: 11120
		public TerrainDef smoothedTerrain = null;

		// Token: 0x04002B71 RID: 11121
		public bool holdSnow = true;

		// Token: 0x04002B72 RID: 11122
		public bool extinguishesFire = false;

		// Token: 0x04002B73 RID: 11123
		public Color color = Color.white;

		// Token: 0x04002B74 RID: 11124
		public TerrainDef driesTo = null;

		// Token: 0x04002B75 RID: 11125
		[NoTranslate]
		public List<string> tags = null;

		// Token: 0x04002B76 RID: 11126
		public TerrainDef burnedDef = null;

		// Token: 0x04002B77 RID: 11127
		public List<Tool> tools = null;

		// Token: 0x04002B78 RID: 11128
		public float extraDeteriorationFactor;

		// Token: 0x04002B79 RID: 11129
		public float destroyOnBombDamageThreshold = -1f;

		// Token: 0x04002B7A RID: 11130
		public bool destroyBuildingsOnDestroyed;

		// Token: 0x04002B7B RID: 11131
		public ThoughtDef traversedThought;

		// Token: 0x04002B7C RID: 11132
		public int extraDraftedPerceivedPathCost;

		// Token: 0x04002B7D RID: 11133
		public int extraNonDraftedPerceivedPathCost;

		// Token: 0x04002B7E RID: 11134
		public EffecterDef destroyEffect;

		// Token: 0x04002B7F RID: 11135
		public EffecterDef destroyEffectWater;

		// Token: 0x04002B80 RID: 11136
		public ThingDef generatedFilth = null;

		// Token: 0x04002B81 RID: 11137
		public bool acceptTerrainSourceFilth = false;

		// Token: 0x04002B82 RID: 11138
		public bool acceptFilth = true;

		// Token: 0x04002B83 RID: 11139
		[Unsaved]
		public Material waterDepthMaterial = null;

		// Token: 0x02000BA3 RID: 2979
		public enum TerrainEdgeType : byte
		{
			// Token: 0x04002B85 RID: 11141
			Hard,
			// Token: 0x04002B86 RID: 11142
			Fade,
			// Token: 0x04002B87 RID: 11143
			FadeRough,
			// Token: 0x04002B88 RID: 11144
			Water
		}
	}
}
