using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats
		{
			get
			{
				yield break;
			}
		}

		public virtual IEnumerable<IncidentTargetTypeDef> AcceptedTypes()
		{
			if (this.def.incidentTargetTypes != null)
			{
				using (List<IncidentTargetTypeDef>.Enumerator enumerator = this.def.incidentTargetTypes.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						IncidentTargetTypeDef type2 = enumerator.Current;
						yield return type2;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				using (IEnumerator<IncidentTargetTypeDef> enumerator2 = this.comps[i].AcceptedTypes().GetEnumerator())
				{
					if (enumerator2.MoveNext())
					{
						IncidentTargetTypeDef type = enumerator2.Current;
						yield return type;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			yield break;
			IL_01af:
			/*Error near IL_01b0: Unexpected return in MoveNext()*/;
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
				Log.Warning("Tried to set faction to " + newFaction + " but this world object (" + this + ") cannot have faction.");
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
			WorldRendererUtility.PrintQuadTangentialToPlanet(this.DrawPos, (float)(0.699999988079071 * averageTileSize), 0.015f, subMesh, false, true, true);
		}

		public virtual void Draw()
		{
			float averageTileSize = Find.WorldGrid.averageTileSize;
			float transitionPct = ExpandableWorldObjectsUtility.TransitionPct;
			if (this.def.expandingIcon && transitionPct > 0.0)
			{
				Color color = this.Material.color;
				float num = (float)(1.0 - transitionPct);
				WorldObject.propertyBlock.SetColor(ShaderPropertyIDs.Color, new Color(color.r, color.g, color.b, color.a * num));
				Vector3 drawPos = this.DrawPos;
				float size = (float)(0.699999988079071 * averageTileSize);
				float altOffset = 0.015f;
				Material material = this.Material;
				MaterialPropertyBlock materialPropertyBlock = WorldObject.propertyBlock;
				WorldRendererUtility.DrawQuadTangentialToPlanet(drawPos, size, altOffset, material, false, false, materialPropertyBlock);
			}
			else
			{
				WorldRendererUtility.DrawQuadTangentialToPlanet(this.DrawPos, (float)(0.699999988079071 * averageTileSize), 0.015f, this.Material, false, false, null);
			}
		}

		public T GetComponent<T>() where T : WorldObjectComp
		{
			int num = 0;
			T result;
			while (true)
			{
				if (num < this.comps.Count)
				{
					T val = (T)(this.comps[num] as T);
					if (val != null)
					{
						result = val;
						break;
					}
					num++;
					continue;
				}
				result = (T)null;
				break;
			}
			return result;
		}

		public WorldObjectComp GetComponent(Type type)
		{
			int num = 0;
			WorldObjectComp result;
			while (true)
			{
				if (num < this.comps.Count)
				{
					if (type.IsAssignableFrom(this.comps[num].GetType()))
					{
						result = this.comps[num];
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				using (IEnumerator<Gizmo> enumerator = this.comps[i].GetGizmos().GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						Gizmo gizmo = enumerator.Current;
						yield return gizmo;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			yield break;
			IL_0104:
			/*Error near IL_0105: Unexpected return in MoveNext()*/;
		}

		public virtual IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			yield break;
		}

		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				using (IEnumerator<FloatMenuOption> enumerator = this.comps[i].GetFloatMenuOptions(caravan).GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						FloatMenuOption f = enumerator.Current;
						yield return f;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			yield break;
			IL_010a:
			/*Error near IL_010b: Unexpected return in MoveNext()*/;
		}

		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return (this.def.inspectorTabsResolved == null) ? Enumerable.Empty<InspectTabBase>() : this.def.inspectorTabsResolved;
		}

		public override string ToString()
		{
			return base.GetType().Name + " " + this.LabelCap + " (tile=" + this.Tile + ")";
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
	}
}
