using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	public class TerrainDef : BuildableDef
	{
		public enum TerrainEdgeType : byte
		{
			Hard = 0,
			Fade = 1,
			FadeRough = 2,
			Water = 3
		}

		[NoTranslate]
		public string texturePath;

		public TerrainEdgeType edgeType;

		[NoTranslate]
		public string waterDepthShader;

		public List<ShaderParameter> waterDepthShaderParameters;

		public int renderPrecedence;

		public List<TerrainAffordance> affordances = new List<TerrainAffordance>();

		public bool layerable;

		[NoTranslate]
		public string scatterType;

		public bool takeFootprints;

		public bool takeSplashes;

		public bool avoidWander;

		public bool changeable = true;

		public TerrainDef smoothedTerrain;

		public bool holdSnow = true;

		public Color color = Color.white;

		public TerrainDef driesTo;

		[NoTranslate]
		public List<string> tags;

		public TerrainDef burnedDef;

		public ThingDef terrainFilthDef;

		public bool acceptTerrainSourceFilth;

		public bool acceptFilth = true;

		[Unsaved]
		public Material waterDepthMaterial;

		public override Color IconDrawColor
		{
			get
			{
				return this.color;
			}
		}

		public bool Removable
		{
			get
			{
				return this.layerable;
			}
		}

		public bool IsCarpet
		{
			get
			{
				return base.researchPrerequisites != null && base.researchPrerequisites.Contains(ResearchProjectDefOf.CarpetMaking);
			}
		}

		public override void PostLoad()
		{
			base.placingDraggableDimensions = 2;
			LongEventHandler.ExecuteWhenFinished((Action)delegate
			{
				Shader shader = null;
				switch (this.edgeType)
				{
				case TerrainEdgeType.Hard:
				{
					shader = ShaderDatabase.TerrainHard;
					break;
				}
				case TerrainEdgeType.Fade:
				{
					shader = ShaderDatabase.TerrainFade;
					break;
				}
				case TerrainEdgeType.FadeRough:
				{
					shader = ShaderDatabase.TerrainFadeRough;
					break;
				}
				case TerrainEdgeType.Water:
				{
					shader = ShaderDatabase.TerrainWater;
					break;
				}
				}
				base.graphic = GraphicDatabase.Get<Graphic_Terrain>(this.texturePath, shader, Vector2.one, this.color, 2000 + this.renderPrecedence);
				if ((UnityEngine.Object)shader == (UnityEngine.Object)ShaderDatabase.TerrainFadeRough || (UnityEngine.Object)shader == (UnityEngine.Object)ShaderDatabase.TerrainWater)
				{
					base.graphic.MatSingle.SetTexture("_AlphaAddTex", TexGame.AlphaAddTex);
				}
				if (!this.waterDepthShader.NullOrEmpty())
				{
					this.waterDepthMaterial = new Material(ShaderDatabase.LoadShader(this.waterDepthShader));
					this.waterDepthMaterial.renderQueue = 2000 + this.renderPrecedence;
					this.waterDepthMaterial.SetTexture("_AlphaAddTex", TexGame.AlphaAddTex);
					if (this.waterDepthShaderParameters != null)
					{
						for (int i = 0; i < this.waterDepthShaderParameters.Count; i++)
						{
							this.waterDepthMaterial.SetFloat(this.waterDepthShaderParameters[i].name, this.waterDepthShaderParameters[i].value);
						}
					}
				}
			});
			base.PostLoad();
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string item in base.ConfigErrors())
			{
				yield return item;
			}
			if (this.texturePath.NullOrEmpty())
			{
				yield return "missing texturePath";
			}
			if (base.fertility < 0.0)
			{
				yield return "Terrain Def " + this + " has no fertility value set.";
			}
			if (this.renderPrecedence > 400)
			{
				yield return "Render order " + this.renderPrecedence + " is out of range (must be < 400)";
			}
			if (this.terrainFilthDef != null && this.acceptTerrainSourceFilth)
			{
				yield return base.defName + " makes terrain filth and also accepts it.";
			}
			if (this.Flammable() && this.burnedDef == null)
			{
				yield return "flammable but burnedDef is null";
			}
			if (this.burnedDef != null && this.burnedDef.Flammable())
			{
				yield return "burnedDef is flammable";
			}
		}

		public static TerrainDef Named(string defName)
		{
			return DefDatabase<TerrainDef>.GetNamed(defName, true);
		}

		public bool HasTag(string tag)
		{
			return this.tags != null && this.tags.Contains(tag);
		}
	}
}
