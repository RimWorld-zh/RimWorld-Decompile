using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
using UnityEngine;

namespace Verse
{
	public class TerrainDef : BuildableDef
	{
		[NoTranslate]
		public string texturePath;

		public TerrainDef.TerrainEdgeType edgeType;

		[NoTranslate]
		public string waterDepthShader;

		public List<ShaderParameter> waterDepthShaderParameters;

		public int renderPrecedence;

		public List<TerrainAffordanceDef> affordances = new List<TerrainAffordanceDef>();

		public bool layerable;

		[NoTranslate]
		public string scatterType;

		public bool takeFootprints;

		public bool takeSplashes;

		public bool avoidWander;

		public bool changeable = true;

		public TerrainDef smoothedTerrain;

		public bool holdSnow = true;

		public bool extinguishesFire;

		public Color color = Color.white;

		public TerrainDef driesTo;

		[NoTranslate]
		public List<string> tags;

		public TerrainDef burnedDef;

		public List<Tool> tools;

		public float extraDeteriorationFactor;

		public float destroyOnBombDamageThreshold = -1f;

		public bool destroyBuildingsOnDestroyed;

		public ThoughtDef traversedThought;

		public int extraDraftedPerceivedPathCost;

		public int extraNonDraftedPerceivedPathCost;

		public EffecterDef destroyEffect;

		public EffecterDef destroyEffectWater;

		public ThingDef generatedFilth;

		public bool acceptTerrainSourceFilth;

		public bool acceptFilth = true;

		[Unsaved]
		public Material waterDepthMaterial;

		public TerrainDef()
		{
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
				return this.researchPrerequisites != null && this.researchPrerequisites.Contains(ResearchProjectDefOf.CarpetMaking);
			}
		}

		public bool IsRiver
		{
			get
			{
				return this.HasTag("River");
			}
		}

		public bool IsWater
		{
			get
			{
				return this.HasTag("Water");
			}
		}

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
						for (int j = 0; j < this.waterDepthShaderParameters.Count; j++)
						{
							this.waterDepthShaderParameters[j].Apply(this.waterDepthMaterial);
						}
					}
				}
			});
			if (this.tools != null)
			{
				for (int i = 0; i < this.tools.Count; i++)
				{
					this.tools[i].id = i.ToString();
				}
			}
			base.PostLoad();
		}

		protected override void ResolveIcon()
		{
			base.ResolveIcon();
			this.uiIconColor = this.color;
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string err in base.ConfigErrors())
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

		public static TerrainDef Named(string defName)
		{
			return DefDatabase<TerrainDef>.GetNamed(defName, true);
		}

		public bool HasTag(string tag)
		{
			return this.tags != null && this.tags.Contains(tag);
		}

		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry stat in base.SpecialDisplayStats(req))
			{
				yield return stat;
			}
			string[] affordance = (from ta in this.affordances.Distinct<TerrainAffordanceDef>()
			orderby ta.order
			select ta.label).ToArray<string>();
			if (affordance.Length > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Supports".Translate(), affordance.ToCommaList(false).CapitalizeFirst(), 0, string.Empty);
			}
			yield break;
		}

		[CompilerGenerated]
		private void <PostLoad>m__0()
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
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<string> <ConfigErrors>__BaseCallProxy0()
		{
			return base.ConfigErrors();
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<StatDrawEntry> <SpecialDisplayStats>__BaseCallProxy1(StatRequest req)
		{
			return base.SpecialDisplayStats(req);
		}

		public enum TerrainEdgeType : byte
		{
			Hard,
			Fade,
			FadeRough,
			Water
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal IEnumerator<string> $locvar0;

			internal string <err>__1;

			internal TerrainDef $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ConfigErrors>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = base.<ConfigErrors>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_FC;
				case 3u:
					goto IL_140;
				case 4u:
					goto IL_18E;
				case 5u:
					goto IL_1DD;
				case 6u:
					goto IL_22C;
				case 7u:
					goto IL_270;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						err = enumerator.Current;
						this.$current = err;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				if (this.texturePath.NullOrEmpty())
				{
					this.$current = "missing texturePath";
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_FC:
				if (this.fertility < 0f)
				{
					this.$current = "Terrain Def " + this + " has no fertility value set.";
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_140:
				if (this.renderPrecedence > 400)
				{
					this.$current = "Render order " + this.renderPrecedence + " is out of range (must be < 400)";
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				IL_18E:
				if (this.generatedFilth != null && this.acceptTerrainSourceFilth)
				{
					this.$current = this.defName + " makes terrain filth and also accepts it.";
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
				IL_1DD:
				if (this.Flammable() && this.burnedDef == null && !this.layerable)
				{
					this.$current = "flammable but burnedDef is null and not layerable";
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				}
				IL_22C:
				if (this.burnedDef != null && this.burnedDef.Flammable())
				{
					this.$current = "burnedDef is flammable";
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				}
				IL_270:
				this.$PC = -1;
				return false;
			}

			string IEnumerator<string>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				TerrainDef.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new TerrainDef.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <SpecialDisplayStats>c__Iterator1 : IEnumerable, IEnumerable<StatDrawEntry>, IEnumerator, IDisposable, IEnumerator<StatDrawEntry>
		{
			internal StatRequest req;

			internal IEnumerator<StatDrawEntry> $locvar0;

			internal StatDrawEntry <stat>__1;

			internal string[] <affordance>__2;

			internal TerrainDef $this;

			internal StatDrawEntry $current;

			internal bool $disposing;

			internal int $PC;

			private static Func<TerrainAffordanceDef, int> <>f__am$cache0;

			private static Func<TerrainAffordanceDef, string> <>f__am$cache1;

			[DebuggerHidden]
			public <SpecialDisplayStats>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = base.<SpecialDisplayStats>__BaseCallProxy1(req).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_16C;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						stat = enumerator.Current;
						this.$current = stat;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				affordance = (from ta in this.affordances.Distinct<TerrainAffordanceDef>()
				orderby ta.order
				select ta.label).ToArray<string>();
				if (affordance.Length <= 0)
				{
					goto IL_16C;
				}
				this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "Supports".Translate(), affordance.ToCommaList(false).CapitalizeFirst(), 0, string.Empty);
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
				IL_16C:
				this.$PC = -1;
				return false;
			}

			StatDrawEntry IEnumerator<StatDrawEntry>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<RimWorld.StatDrawEntry>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				TerrainDef.<SpecialDisplayStats>c__Iterator1 <SpecialDisplayStats>c__Iterator = new TerrainDef.<SpecialDisplayStats>c__Iterator1();
				<SpecialDisplayStats>c__Iterator.$this = this;
				<SpecialDisplayStats>c__Iterator.req = req;
				return <SpecialDisplayStats>c__Iterator;
			}

			private static int <>m__0(TerrainAffordanceDef ta)
			{
				return ta.order;
			}

			private static string <>m__1(TerrainAffordanceDef ta)
			{
				return ta.label;
			}
		}
	}
}
