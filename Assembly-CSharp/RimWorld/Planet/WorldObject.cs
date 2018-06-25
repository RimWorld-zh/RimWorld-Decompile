using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	[StaticConstructorOnStartup]
	public class WorldObject : IExposable, ILoadReferenceable, ISelectable
	{
		public WorldObjectDef def;

		public int ID = -1;

		private int tileInt = -1;

		private Faction factionInt;

		public int creationGameTicks = -1;

		private List<WorldObjectComp> comps = new List<WorldObjectComp>();

		private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		private const float BaseDrawSize = 0.7f;

		public WorldObject()
		{
		}

		public List<WorldObjectComp> AllComps
		{
			get
			{
				return this.comps;
			}
		}

		public int Tile
		{
			get
			{
				return this.tileInt;
			}
			set
			{
				if (this.tileInt != value)
				{
					this.tileInt = value;
					if (this.Spawned)
					{
						if (!this.def.useDynamicDrawer)
						{
							Find.World.renderer.Notify_StaticWorldObjectPosChanged();
						}
					}
					this.PositionChanged();
				}
			}
		}

		public bool Spawned
		{
			get
			{
				return Find.WorldObjects.Contains(this);
			}
		}

		public virtual Vector3 DrawPos
		{
			get
			{
				return Find.WorldGrid.GetTileCenter(this.Tile);
			}
		}

		public Faction Faction
		{
			get
			{
				return this.factionInt;
			}
		}

		public virtual string Label
		{
			get
			{
				return this.def.label;
			}
		}

		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		public virtual string LabelShort
		{
			get
			{
				return this.Label;
			}
		}

		public virtual string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst();
			}
		}

		public virtual Material Material
		{
			get
			{
				return this.def.Material;
			}
		}

		public virtual bool SelectableNow
		{
			get
			{
				return this.def.selectable;
			}
		}

		public virtual bool NeverMultiSelect
		{
			get
			{
				return this.def.neverMultiSelect;
			}
		}

		public virtual Texture2D ExpandingIcon
		{
			get
			{
				return this.def.ExpandingIconTexture ?? ((Texture2D)this.Material.mainTexture);
			}
		}

		public virtual Color ExpandingIconColor
		{
			get
			{
				return this.Material.color;
			}
		}

		public virtual float ExpandingIconPriority
		{
			get
			{
				return this.def.expandingIconPriority;
			}
		}

		public virtual bool ExpandMore
		{
			get
			{
				return this.def.expandMore;
			}
		}

		public virtual bool AppendFactionToInspectString
		{
			get
			{
				return true;
			}
		}

		public IThingHolder ParentHolder
		{
			get
			{
				return (!this.Spawned) ? null : Find.World;
			}
		}

		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats
		{
			get
			{
				yield break;
			}
		}

		public BiomeDef Biome
		{
			get
			{
				return (!this.Spawned) ? null : Find.WorldGrid[this.Tile].biome;
			}
		}

		public virtual IEnumerable<IncidentTargetTypeDef> AcceptedTypes()
		{
			if (this.def.incidentTargetTypes != null)
			{
				foreach (IncidentTargetTypeDef type in this.def.incidentTargetTypes)
				{
					yield return type;
				}
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				foreach (IncidentTargetTypeDef type2 in this.comps[i].AcceptedTypes())
				{
					yield return type2;
				}
			}
			yield break;
		}

		public virtual void ExposeData()
		{
			Scribe_Defs.Look<WorldObjectDef>(ref this.def, "def");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.InitializeComps();
			}
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Values.Look<int>(ref this.tileInt, "tile", -1, false);
			Scribe_References.Look<Faction>(ref this.factionInt, "faction", false);
			Scribe_Values.Look<int>(ref this.creationGameTicks, "creationGameTicks", 0, false);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostExposeData();
			}
		}

		private void InitializeComps()
		{
			for (int i = 0; i < this.def.comps.Count; i++)
			{
				WorldObjectComp worldObjectComp = (WorldObjectComp)Activator.CreateInstance(this.def.comps[i].compClass);
				worldObjectComp.parent = this;
				this.comps.Add(worldObjectComp);
				worldObjectComp.Initialize(this.def.comps[i]);
			}
		}

		public virtual void SetFaction(Faction newFaction)
		{
			if (!this.def.canHaveFaction && newFaction != null)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to set faction to ",
					newFaction,
					" but this world object (",
					this,
					") cannot have faction."
				}), false);
			}
			else
			{
				this.factionInt = newFaction;
			}
		}

		public virtual string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.Faction != null && this.AppendFactionToInspectString)
			{
				stringBuilder.Append("Faction".Translate() + ": " + this.Faction.Name);
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				string text = this.comps[i].CompInspectStringExtra();
				if (!text.NullOrEmpty())
				{
					if (Prefs.DevMode && char.IsWhiteSpace(text[text.Length - 1]))
					{
						Log.ErrorOnce(this.comps[i].GetType() + " CompInspectStringExtra ended with whitespace: " + text, 25612, false);
						text = text.TrimEndNewlines();
					}
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(text);
				}
			}
			return stringBuilder.ToString();
		}

		public virtual void Tick()
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompTick();
			}
		}

		public virtual void ExtraSelectionOverlaysOnGUI()
		{
		}

		public virtual void DrawExtraSelectionOverlays()
		{
		}

		public virtual void PostMake()
		{
			this.InitializeComps();
		}

		public virtual void PostAdd()
		{
		}

		protected virtual void PositionChanged()
		{
		}

		public virtual void SpawnSetup()
		{
			if (!this.def.useDynamicDrawer)
			{
				Find.World.renderer.Notify_StaticWorldObjectPosChanged();
			}
			if (this.def.useDynamicDrawer)
			{
				Find.WorldDynamicDrawManager.RegisterDrawable(this);
			}
		}

		public virtual void PostRemove()
		{
			if (!this.def.useDynamicDrawer)
			{
				Find.World.renderer.Notify_StaticWorldObjectPosChanged();
			}
			if (this.def.useDynamicDrawer)
			{
				Find.WorldDynamicDrawManager.DeRegisterDrawable(this);
			}
			Find.WorldSelector.Deselect(this);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostPostRemove();
			}
		}

		public virtual void Print(LayerSubMesh subMesh)
		{
			float averageTileSize = Find.WorldGrid.averageTileSize;
			WorldRendererUtility.PrintQuadTangentialToPlanet(this.DrawPos, 0.7f * averageTileSize, 0.015f, subMesh, false, true, true);
		}

		public virtual void Draw()
		{
			float averageTileSize = Find.WorldGrid.averageTileSize;
			float transitionPct = ExpandableWorldObjectsUtility.TransitionPct;
			if (this.def.expandingIcon && transitionPct > 0f)
			{
				Color color = this.Material.color;
				float num = 1f - transitionPct;
				WorldObject.propertyBlock.SetColor(ShaderPropertyIDs.Color, new Color(color.r, color.g, color.b, color.a * num));
				Vector3 drawPos = this.DrawPos;
				float size = 0.7f * averageTileSize;
				float altOffset = 0.015f;
				Material material = this.Material;
				MaterialPropertyBlock materialPropertyBlock = WorldObject.propertyBlock;
				WorldRendererUtility.DrawQuadTangentialToPlanet(drawPos, size, altOffset, material, false, false, materialPropertyBlock);
			}
			else
			{
				WorldRendererUtility.DrawQuadTangentialToPlanet(this.DrawPos, 0.7f * averageTileSize, 0.015f, this.Material, false, false, null);
			}
		}

		public T GetComponent<T>() where T : WorldObjectComp
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				T t = this.comps[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return (T)((object)null);
		}

		public WorldObjectComp GetComponent(Type type)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (type.IsAssignableFrom(this.comps[i].GetType()))
				{
					return this.comps[i];
				}
			}
			return null;
		}

		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				foreach (Gizmo gizmo in this.comps[i].GetGizmos())
				{
					yield return gizmo;
				}
			}
			yield break;
		}

		public virtual IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				foreach (Gizmo g in this.comps[i].GetCaravanGizmos(caravan))
				{
					yield return g;
				}
			}
			yield break;
		}

		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				foreach (FloatMenuOption f in this.comps[i].GetFloatMenuOptions(caravan))
				{
					yield return f;
				}
			}
			yield break;
		}

		public virtual IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				foreach (FloatMenuOption f in this.comps[i].GetTransportPodsFloatMenuOptions(pods, representative))
				{
					yield return f;
				}
			}
			yield break;
		}

		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			IEnumerable<InspectTabBase> result;
			if (this.def.inspectorTabsResolved != null)
			{
				result = this.def.inspectorTabsResolved;
			}
			else
			{
				result = Enumerable.Empty<InspectTabBase>();
			}
			return result;
		}

		public virtual bool AllMatchingObjectsOnScreenMatchesWith(WorldObject other)
		{
			return this.Faction == other.Faction;
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.GetType().Name,
				" ",
				this.LabelCap,
				" (tile=",
				this.Tile,
				")"
			});
		}

		public override int GetHashCode()
		{
			return this.ID;
		}

		public string GetUniqueLoadID()
		{
			return "WorldObject_" + this.ID;
		}

		public virtual string GetDescription()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.def.description);
			for (int i = 0; i < this.comps.Count; i++)
			{
				string descriptionPart = this.comps[i].GetDescriptionPart();
				if (!descriptionPart.NullOrEmpty())
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendLine();
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(descriptionPart);
				}
			}
			return stringBuilder.ToString();
		}

		// Note: this type is marked as 'beforefieldinit'.
		static WorldObject()
		{
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<StatDrawEntry>, IEnumerator, IDisposable, IEnumerator<StatDrawEntry>
		{
			internal StatDrawEntry $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				bool flag = this.$PC != 0;
				this.$PC = -1;
				if (!flag)
				{
				}
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
				return new WorldObject.<>c__Iterator0();
			}
		}

		[CompilerGenerated]
		private sealed class <AcceptedTypes>c__Iterator1 : IEnumerable, IEnumerable<IncidentTargetTypeDef>, IEnumerator, IDisposable, IEnumerator<IncidentTargetTypeDef>
		{
			internal List<IncidentTargetTypeDef>.Enumerator $locvar0;

			internal IncidentTargetTypeDef <type>__1;

			internal int <i>__2;

			internal IEnumerator<IncidentTargetTypeDef> $locvar1;

			internal IncidentTargetTypeDef <type>__3;

			internal WorldObject $this;

			internal IncidentTargetTypeDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <AcceptedTypes>c__Iterator1()
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
					if (this.def.incidentTargetTypes == null)
					{
						goto IL_CF;
					}
					enumerator = this.def.incidentTargetTypes.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					Block_4:
					try
					{
						switch (num)
						{
						}
						if (enumerator2.MoveNext())
						{
							type2 = enumerator2.Current;
							this.$current = type2;
							if (!this.$disposing)
							{
								this.$PC = 2;
							}
							flag = true;
							return true;
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
					}
					i++;
					goto IL_18B;
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
						type = enumerator.Current;
						this.$current = type;
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
						((IDisposable)enumerator).Dispose();
					}
				}
				IL_CF:
				i = 0;
				IL_18B:
				if (i < this.comps.Count)
				{
					enumerator2 = this.comps[i].AcceptedTypes().GetEnumerator();
					num = 4294967293u;
					goto Block_4;
				}
				this.$PC = -1;
				return false;
			}

			IncidentTargetTypeDef IEnumerator<IncidentTargetTypeDef>.Current
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
						((IDisposable)enumerator).Dispose();
					}
					break;
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.IncidentTargetTypeDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IncidentTargetTypeDef> IEnumerable<IncidentTargetTypeDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				WorldObject.<AcceptedTypes>c__Iterator1 <AcceptedTypes>c__Iterator = new WorldObject.<AcceptedTypes>c__Iterator1();
				<AcceptedTypes>c__Iterator.$this = this;
				return <AcceptedTypes>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetGizmos>c__Iterator2 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal int <i>__1;

			internal IEnumerator<Gizmo> $locvar0;

			internal Gizmo <gizmo>__2;

			internal WorldObject $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetGizmos>c__Iterator2()
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
					i = 0;
					break;
				case 1u:
					Block_2:
					try
					{
						switch (num)
						{
						}
						if (enumerator.MoveNext())
						{
							gizmo = enumerator.Current;
							this.$current = gizmo;
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
					i++;
					break;
				default:
					return false;
				}
				if (i < this.comps.Count)
				{
					enumerator = this.comps[i].GetGizmos().GetEnumerator();
					num = 4294967293u;
					goto Block_2;
				}
				this.$PC = -1;
				return false;
			}

			Gizmo IEnumerator<Gizmo>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.Gizmo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				WorldObject.<GetGizmos>c__Iterator2 <GetGizmos>c__Iterator = new WorldObject.<GetGizmos>c__Iterator2();
				<GetGizmos>c__Iterator.$this = this;
				return <GetGizmos>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetCaravanGizmos>c__Iterator3 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal int <i>__1;

			internal Caravan caravan;

			internal IEnumerator<Gizmo> $locvar0;

			internal Gizmo <g>__2;

			internal WorldObject $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetCaravanGizmos>c__Iterator3()
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
					i = 0;
					break;
				case 1u:
					Block_2:
					try
					{
						switch (num)
						{
						}
						if (enumerator.MoveNext())
						{
							g = enumerator.Current;
							this.$current = g;
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
					i++;
					break;
				default:
					return false;
				}
				if (i < this.comps.Count)
				{
					enumerator = this.comps[i].GetCaravanGizmos(caravan).GetEnumerator();
					num = 4294967293u;
					goto Block_2;
				}
				this.$PC = -1;
				return false;
			}

			Gizmo IEnumerator<Gizmo>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.Gizmo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				WorldObject.<GetCaravanGizmos>c__Iterator3 <GetCaravanGizmos>c__Iterator = new WorldObject.<GetCaravanGizmos>c__Iterator3();
				<GetCaravanGizmos>c__Iterator.$this = this;
				<GetCaravanGizmos>c__Iterator.caravan = caravan;
				return <GetCaravanGizmos>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetFloatMenuOptions>c__Iterator4 : IEnumerable, IEnumerable<FloatMenuOption>, IEnumerator, IDisposable, IEnumerator<FloatMenuOption>
		{
			internal int <i>__1;

			internal Caravan caravan;

			internal IEnumerator<FloatMenuOption> $locvar0;

			internal FloatMenuOption <f>__2;

			internal WorldObject $this;

			internal FloatMenuOption $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetFloatMenuOptions>c__Iterator4()
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
					i = 0;
					break;
				case 1u:
					Block_2:
					try
					{
						switch (num)
						{
						}
						if (enumerator.MoveNext())
						{
							f = enumerator.Current;
							this.$current = f;
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
					i++;
					break;
				default:
					return false;
				}
				if (i < this.comps.Count)
				{
					enumerator = this.comps[i].GetFloatMenuOptions(caravan).GetEnumerator();
					num = 4294967293u;
					goto Block_2;
				}
				this.$PC = -1;
				return false;
			}

			FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.FloatMenuOption>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				WorldObject.<GetFloatMenuOptions>c__Iterator4 <GetFloatMenuOptions>c__Iterator = new WorldObject.<GetFloatMenuOptions>c__Iterator4();
				<GetFloatMenuOptions>c__Iterator.$this = this;
				<GetFloatMenuOptions>c__Iterator.caravan = caravan;
				return <GetFloatMenuOptions>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetTransportPodsFloatMenuOptions>c__Iterator5 : IEnumerable, IEnumerable<FloatMenuOption>, IEnumerator, IDisposable, IEnumerator<FloatMenuOption>
		{
			internal int <i>__1;

			internal IEnumerable<IThingHolder> pods;

			internal CompLaunchable representative;

			internal IEnumerator<FloatMenuOption> $locvar0;

			internal FloatMenuOption <f>__2;

			internal WorldObject $this;

			internal FloatMenuOption $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetTransportPodsFloatMenuOptions>c__Iterator5()
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
					i = 0;
					break;
				case 1u:
					Block_2:
					try
					{
						switch (num)
						{
						}
						if (enumerator.MoveNext())
						{
							f = enumerator.Current;
							this.$current = f;
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
					i++;
					break;
				default:
					return false;
				}
				if (i < this.comps.Count)
				{
					enumerator = this.comps[i].GetTransportPodsFloatMenuOptions(pods, representative).GetEnumerator();
					num = 4294967293u;
					goto Block_2;
				}
				this.$PC = -1;
				return false;
			}

			FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.FloatMenuOption>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				WorldObject.<GetTransportPodsFloatMenuOptions>c__Iterator5 <GetTransportPodsFloatMenuOptions>c__Iterator = new WorldObject.<GetTransportPodsFloatMenuOptions>c__Iterator5();
				<GetTransportPodsFloatMenuOptions>c__Iterator.$this = this;
				<GetTransportPodsFloatMenuOptions>c__Iterator.pods = pods;
				<GetTransportPodsFloatMenuOptions>c__Iterator.representative = representative;
				return <GetTransportPodsFloatMenuOptions>c__Iterator;
			}
		}
	}
}
