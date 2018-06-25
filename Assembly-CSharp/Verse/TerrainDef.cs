using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BA5 RID: 2981
	public class TerrainDef : BuildableDef
	{
		// Token: 0x04002B6B RID: 11115
		[NoTranslate]
		public string texturePath;

		// Token: 0x04002B6C RID: 11116
		public TerrainDef.TerrainEdgeType edgeType = TerrainDef.TerrainEdgeType.Hard;

		// Token: 0x04002B6D RID: 11117
		[NoTranslate]
		public string waterDepthShader = null;

		// Token: 0x04002B6E RID: 11118
		public List<ShaderParameter> waterDepthShaderParameters = null;

		// Token: 0x04002B6F RID: 11119
		public int renderPrecedence = 0;

		// Token: 0x04002B70 RID: 11120
		public List<TerrainAffordanceDef> affordances = new List<TerrainAffordanceDef>();

		// Token: 0x04002B71 RID: 11121
		public bool layerable = false;

		// Token: 0x04002B72 RID: 11122
		[NoTranslate]
		public string scatterType = null;

		// Token: 0x04002B73 RID: 11123
		public bool takeFootprints = false;

		// Token: 0x04002B74 RID: 11124
		public bool takeSplashes = false;

		// Token: 0x04002B75 RID: 11125
		public bool avoidWander = false;

		// Token: 0x04002B76 RID: 11126
		public bool changeable = true;

		// Token: 0x04002B77 RID: 11127
		public TerrainDef smoothedTerrain = null;

		// Token: 0x04002B78 RID: 11128
		public bool holdSnow = true;

		// Token: 0x04002B79 RID: 11129
		public bool extinguishesFire = false;

		// Token: 0x04002B7A RID: 11130
		public Color color = Color.white;

		// Token: 0x04002B7B RID: 11131
		public TerrainDef driesTo = null;

		// Token: 0x04002B7C RID: 11132
		[NoTranslate]
		public List<string> tags = null;

		// Token: 0x04002B7D RID: 11133
		public TerrainDef burnedDef = null;

		// Token: 0x04002B7E RID: 11134
		public List<Tool> tools = null;

		// Token: 0x04002B7F RID: 11135
		public float extraDeteriorationFactor;

		// Token: 0x04002B80 RID: 11136
		public float destroyOnBombDamageThreshold = -1f;

		// Token: 0x04002B81 RID: 11137
		public bool destroyBuildingsOnDestroyed;

		// Token: 0x04002B82 RID: 11138
		public ThoughtDef traversedThought;

		// Token: 0x04002B83 RID: 11139
		public int extraDraftedPerceivedPathCost;

		// Token: 0x04002B84 RID: 11140
		public int extraNonDraftedPerceivedPathCost;

		// Token: 0x04002B85 RID: 11141
		public EffecterDef destroyEffect;

		// Token: 0x04002B86 RID: 11142
		public EffecterDef destroyEffectWater;

		// Token: 0x04002B87 RID: 11143
		public ThingDef generatedFilth = null;

		// Token: 0x04002B88 RID: 11144
		public bool acceptTerrainSourceFilth = false;

		// Token: 0x04002B89 RID: 11145
		public bool acceptFilth = true;

		// Token: 0x04002B8A RID: 11146
		[Unsaved]
		public Material waterDepthMaterial = null;

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x06004074 RID: 16500 RVA: 0x0021E368 File Offset: 0x0021C768
		public bool Removable
		{
			get
			{
				return this.layerable;
			}
		}

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x06004075 RID: 16501 RVA: 0x0021E384 File Offset: 0x0021C784
		public bool IsCarpet
		{
			get
			{
				return this.researchPrerequisites != null && this.researchPrerequisites.Contains(ResearchProjectDefOf.CarpetMaking);
			}
		}

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x06004076 RID: 16502 RVA: 0x0021E3B8 File Offset: 0x0021C7B8
		public bool IsRiver
		{
			get
			{
				return this.HasTag("River");
			}
		}

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x06004077 RID: 16503 RVA: 0x0021E3D8 File Offset: 0x0021C7D8
		public bool IsWater
		{
			get
			{
				return this.HasTag("Water");
			}
		}

		// Token: 0x06004078 RID: 16504 RVA: 0x0021E3F8 File Offset: 0x0021C7F8
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

		// Token: 0x06004079 RID: 16505 RVA: 0x0021E419 File Offset: 0x0021C819
		protected override void ResolveIcon()
		{
			base.ResolveIcon();
			this.uiIconColor = this.color;
		}

		// Token: 0x0600407A RID: 16506 RVA: 0x0021E430 File Offset: 0x0021C830
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

		// Token: 0x0600407B RID: 16507 RVA: 0x0021E45C File Offset: 0x0021C85C
		public static TerrainDef Named(string defName)
		{
			return DefDatabase<TerrainDef>.GetNamed(defName, true);
		}

		// Token: 0x0600407C RID: 16508 RVA: 0x0021E478 File Offset: 0x0021C878
		public bool HasTag(string tag)
		{
			return this.tags != null && this.tags.Contains(tag);
		}

		// Token: 0x0600407D RID: 16509 RVA: 0x0021E4A8 File Offset: 0x0021C8A8
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

		// Token: 0x02000BA6 RID: 2982
		public enum TerrainEdgeType : byte
		{
			// Token: 0x04002B8C RID: 11148
			Hard,
			// Token: 0x04002B8D RID: 11149
			Fade,
			// Token: 0x04002B8E RID: 11150
			FadeRough,
			// Token: 0x04002B8F RID: 11151
			Water
		}
	}
}
