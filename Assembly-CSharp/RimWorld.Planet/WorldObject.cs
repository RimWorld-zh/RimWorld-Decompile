using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	[StaticConstructorOnStartup]
	public class WorldObject : IExposable, ILoadReferenceable, ISelectable
	{
		private const float BaseDrawSize = 0.7f;

		public WorldObjectDef def;

		public int ID = -1;

		private int tileInt = -1;

		private Faction factionInt;

		public int creationGameTicks = -1;

		private List<WorldObjectComp> comps = new List<WorldObjectComp>();

		private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

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
					if (this.Spawned && !this.def.useDynamicDrawer)
					{
						Find.World.renderer.Notify_StaticWorldObjectPosChanged();
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
				}));
				return;
			}
			this.factionInt = newFaction;
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
						Log.ErrorOnce(this.comps[i].GetType() + " CompInspectStringExtra ended with whitespace: " + text, 25612);
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
				MaterialPropertyBlock materialPropertyBlock = WorldObject.propertyBlock;
				WorldRendererUtility.DrawQuadTangentialToPlanet(this.DrawPos, 0.7f * averageTileSize, 0.015f, this.Material, false, false, materialPropertyBlock);
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

		[DebuggerHidden]
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			WorldObject.<GetGizmos>c__IteratorFB <GetGizmos>c__IteratorFB = new WorldObject.<GetGizmos>c__IteratorFB();
			<GetGizmos>c__IteratorFB.<>f__this = this;
			WorldObject.<GetGizmos>c__IteratorFB expr_0E = <GetGizmos>c__IteratorFB;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		[DebuggerHidden]
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			WorldObject.<GetFloatMenuOptions>c__IteratorFC <GetFloatMenuOptions>c__IteratorFC = new WorldObject.<GetFloatMenuOptions>c__IteratorFC();
			<GetFloatMenuOptions>c__IteratorFC.caravan = caravan;
			<GetFloatMenuOptions>c__IteratorFC.<$>caravan = caravan;
			<GetFloatMenuOptions>c__IteratorFC.<>f__this = this;
			WorldObject.<GetFloatMenuOptions>c__IteratorFC expr_1C = <GetFloatMenuOptions>c__IteratorFC;
			expr_1C.$PC = -2;
			return expr_1C;
		}

		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			if (this.def.inspectorTabsResolved != null)
			{
				return this.def.inspectorTabsResolved;
			}
			return Enumerable.Empty<InspectTabBase>();
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
	}
}
