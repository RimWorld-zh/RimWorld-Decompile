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
	public abstract class BuildableDef : Def
	{
		public List<StatModifier> statBases = null;

		public Traversability passability = Traversability.Standable;

		public int pathCost = 0;

		public bool pathCostIgnoreRepeat = true;

		public float fertility = -1f;

		public List<ThingDefCountClass> costList = null;

		public int costStuffCount = 0;

		public List<StuffCategoryDef> stuffCategories = null;

		public int placingDraggableDimensions = 0;

		public bool clearBuildingArea = true;

		public Rot4 defaultPlacingRot = Rot4.North;

		public float resourcesFractionWhenDeconstructed = 0.75f;

		public TerrainAffordanceDef terrainAffordanceNeeded = null;

		public List<ThingDef> buildingPrerequisites = null;

		public List<ResearchProjectDef> researchPrerequisites = null;

		public int constructionSkillPrerequisite = 0;

		public TechLevel minTechLevelToBuild = TechLevel.Undefined;

		public TechLevel maxTechLevelToBuild = TechLevel.Undefined;

		public AltitudeLayer altitudeLayer = AltitudeLayer.Item;

		public EffecterDef repairEffect = null;

		public EffecterDef constructEffect = null;

		public bool menuHidden = false;

		public float specialDisplayRadius = 0f;

		public List<Type> placeWorkers = null;

		public DesignationCategoryDef designationCategory = null;

		public DesignatorDropdownGroupDef designatorDropdown = null;

		public KeyBindingDef designationHotKey = null;

		[NoTranslate]
		public string uiIconPath;

		public Vector2 uiIconOffset;

		[Unsaved]
		public ThingDef blueprintDef;

		[Unsaved]
		public ThingDef installBlueprintDef;

		[Unsaved]
		public ThingDef frameDef;

		[Unsaved]
		private List<PlaceWorker> placeWorkersInstantiatedInt = null;

		[Unsaved]
		public Graphic graphic = BaseContent.BadGraphic;

		[Unsaved]
		public Texture2D uiIcon = BaseContent.BadTex;

		[Unsaved]
		public float uiIconAngle;

		[Unsaved]
		public Color uiIconColor = Color.white;

		protected BuildableDef()
		{
		}

		public virtual IntVec2 Size
		{
			get
			{
				return new IntVec2(1, 1);
			}
		}

		public bool MadeFromStuff
		{
			get
			{
				return !this.stuffCategories.NullOrEmpty<StuffCategoryDef>();
			}
		}

		public bool BuildableByPlayer
		{
			get
			{
				return this.designationCategory != null;
			}
		}

		public Material DrawMatSingle
		{
			get
			{
				Material result;
				if (this.graphic == null)
				{
					result = null;
				}
				else
				{
					result = this.graphic.MatSingle;
				}
				return result;
			}
		}

		public float Altitude
		{
			get
			{
				return this.altitudeLayer.AltitudeFor();
			}
		}

		public List<PlaceWorker> PlaceWorkers
		{
			get
			{
				List<PlaceWorker> result;
				if (this.placeWorkers == null)
				{
					result = null;
				}
				else
				{
					this.placeWorkersInstantiatedInt = new List<PlaceWorker>();
					foreach (Type type in this.placeWorkers)
					{
						this.placeWorkersInstantiatedInt.Add((PlaceWorker)Activator.CreateInstance(type));
					}
					result = this.placeWorkersInstantiatedInt;
				}
				return result;
			}
		}

		public bool IsResearchFinished
		{
			get
			{
				if (this.researchPrerequisites != null)
				{
					for (int i = 0; i < this.researchPrerequisites.Count; i++)
					{
						if (!this.researchPrerequisites[i].IsFinished)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		public bool ForceAllowPlaceOver(BuildableDef other)
		{
			bool result;
			if (this.PlaceWorkers == null)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < this.PlaceWorkers.Count; i++)
				{
					if (this.PlaceWorkers[i].ForceAllowPlaceOver(other))
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		public override void PostLoad()
		{
			base.PostLoad();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (!this.uiIconPath.NullOrEmpty())
				{
					this.uiIcon = ContentFinder<Texture2D>.Get(this.uiIconPath, true);
				}
				else
				{
					this.ResolveIcon();
				}
			});
		}

		protected virtual void ResolveIcon()
		{
			if (this.graphic != null && this.graphic != BaseContent.BadGraphic)
			{
				Material material = this.graphic.ExtractInnerGraphicFor(null).MatAt(this.defaultPlacingRot, null);
				this.uiIcon = (Texture2D)material.mainTexture;
				this.uiIconColor = material.color;
			}
		}

		public override void ResolveReferences()
		{
			base.ResolveReferences();
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string error in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return error;
			}
			yield break;
		}

		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry stat in this.<SpecialDisplayStats>__BaseCallProxy1())
			{
				yield return stat;
			}
			IEnumerable<TerrainAffordanceDef> affdefs = Enumerable.Empty<TerrainAffordanceDef>();
			if (this.PlaceWorkers != null)
			{
				affdefs = affdefs.Concat(this.PlaceWorkers.SelectMany((PlaceWorker pw) => pw.DisplayAffordances()));
			}
			if (this.terrainAffordanceNeeded != null)
			{
				affdefs = affdefs.Concat(this.terrainAffordanceNeeded);
			}
			string[] affordances = (from ta in affdefs.Distinct<TerrainAffordanceDef>()
			orderby ta.order
			select ta.label).ToArray<string>();
			if (affordances.Length > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "TerrainRequirement".Translate(), affordances.ToCommaList(false).CapitalizeFirst(), 0, "");
			}
			yield break;
		}

		public override string ToString()
		{
			return this.defName;
		}

		public override int GetHashCode()
		{
			return this.defName.GetHashCode();
		}

		[CompilerGenerated]
		private void <PostLoad>m__0()
		{
			if (!this.uiIconPath.NullOrEmpty())
			{
				this.uiIcon = ContentFinder<Texture2D>.Get(this.uiIconPath, true);
			}
			else
			{
				this.ResolveIcon();
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
		private IEnumerable<StatDrawEntry> <SpecialDisplayStats>__BaseCallProxy1()
		{
			return base.SpecialDisplayStats();
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal IEnumerator<string> $locvar0;

			internal string <error>__1;

			internal BuildableDef $this;

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
						error = enumerator.Current;
						this.$current = error;
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
				BuildableDef.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new BuildableDef.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <SpecialDisplayStats>c__Iterator1 : IEnumerable, IEnumerable<StatDrawEntry>, IEnumerator, IDisposable, IEnumerator<StatDrawEntry>
		{
			internal IEnumerator<StatDrawEntry> $locvar0;

			internal StatDrawEntry <stat>__1;

			internal IEnumerable<TerrainAffordanceDef> <affdefs>__2;

			internal string[] <affordances>__2;

			internal BuildableDef $this;

			internal StatDrawEntry $current;

			internal bool $disposing;

			internal int $PC;

			private static Func<PlaceWorker, IEnumerable<TerrainAffordanceDef>> <>f__am$cache0;

			private static Func<TerrainAffordanceDef, int> <>f__am$cache1;

			private static Func<TerrainAffordanceDef, string> <>f__am$cache2;

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
					enumerator = base.<SpecialDisplayStats>__BaseCallProxy1().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_1EB;
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
				affdefs = Enumerable.Empty<TerrainAffordanceDef>();
				if (base.PlaceWorkers != null)
				{
					affdefs = affdefs.Concat(base.PlaceWorkers.SelectMany((PlaceWorker pw) => pw.DisplayAffordances()));
				}
				if (this.terrainAffordanceNeeded != null)
				{
					affdefs = affdefs.Concat(this.terrainAffordanceNeeded);
				}
				affordances = (from ta in affdefs.Distinct<TerrainAffordanceDef>()
				orderby ta.order
				select ta.label).ToArray<string>();
				if (affordances.Length <= 0)
				{
					goto IL_1EB;
				}
				this.$current = new StatDrawEntry(StatCategoryDefOf.Basics, "TerrainRequirement".Translate(), affordances.ToCommaList(false).CapitalizeFirst(), 0, "");
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
				IL_1EB:
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
				BuildableDef.<SpecialDisplayStats>c__Iterator1 <SpecialDisplayStats>c__Iterator = new BuildableDef.<SpecialDisplayStats>c__Iterator1();
				<SpecialDisplayStats>c__Iterator.$this = this;
				return <SpecialDisplayStats>c__Iterator;
			}

			private static IEnumerable<TerrainAffordanceDef> <>m__0(PlaceWorker pw)
			{
				return pw.DisplayAffordances();
			}

			private static int <>m__1(TerrainAffordanceDef ta)
			{
				return ta.order;
			}

			private static string <>m__2(TerrainAffordanceDef ta)
			{
				return ta.label;
			}
		}
	}
}
