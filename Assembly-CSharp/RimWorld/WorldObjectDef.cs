using RimWorld.Planet;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class WorldObjectDef : Def
	{
		public Type worldObjectClass = typeof(WorldObject);

		public bool canHaveFaction = true;

		public bool saved = true;

		public List<WorldObjectCompProperties> comps = new List<WorldObjectCompProperties>();

		public bool allowCaravanIncidentsWhichGenerateMap;

		public bool isTempIncidentMapOwner;

		public List<IncidentTargetTypeDef> incidentTargetTypes;

		public bool selectable = true;

		public bool neverMultiSelect;

		public MapGeneratorDef mapGenerator = null;

		public List<Type> inspectorTabs;

		[Unsaved]
		public List<InspectTabBase> inspectorTabsResolved;

		public bool useDynamicDrawer;

		public bool expandingIcon;

		[NoTranslate]
		public string expandingIconTexture;

		public float expandingIconPriority;

		[NoTranslate]
		public string texture;

		[Unsaved]
		private Material material;

		[Unsaved]
		private Texture2D expandingIconTextureInt;

		public bool blockExitGridUntilBattleIsWon;

		public Material Material
		{
			get
			{
				Material result;
				if (this.texture.NullOrEmpty())
				{
					result = null;
				}
				else
				{
					if ((UnityEngine.Object)this.material == (UnityEngine.Object)null)
					{
						this.material = MaterialPool.MatFrom(this.texture, ShaderDatabase.WorldOverlayTransparentLit, WorldMaterials.WorldObjectRenderQueue);
					}
					result = this.material;
				}
				return result;
			}
		}

		public Texture2D ExpandingIconTexture
		{
			get
			{
				Texture2D result;
				if ((UnityEngine.Object)this.expandingIconTextureInt == (UnityEngine.Object)null)
				{
					if (this.expandingIconTexture.NullOrEmpty())
					{
						result = null;
						goto IL_0049;
					}
					this.expandingIconTextureInt = ContentFinder<Texture2D>.Get(this.expandingIconTexture, true);
				}
				result = this.expandingIconTextureInt;
				goto IL_0049;
				IL_0049:
				return result;
			}
		}

		public override void PostLoad()
		{
			base.PostLoad();
			if (this.inspectorTabs != null)
			{
				for (int i = 0; i < this.inspectorTabs.Count; i++)
				{
					if (this.inspectorTabsResolved == null)
					{
						this.inspectorTabsResolved = new List<InspectTabBase>();
					}
					try
					{
						this.inspectorTabsResolved.Add(InspectTabManager.GetSharedInstance(this.inspectorTabs[i]));
					}
					catch (Exception ex)
					{
						Log.Error("Could not instantiate inspector tab of type " + this.inspectorTabs[i] + ": " + ex);
					}
				}
			}
		}

		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].ResolveReferences(this);
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string e2 = enumerator.Current;
					yield return e2;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				using (IEnumerator<string> enumerator2 = this.comps[i].ConfigErrors(this).GetEnumerator())
				{
					if (enumerator2.MoveNext())
					{
						string e = enumerator2.Current;
						yield return e;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			yield break;
			IL_019e:
			/*Error near IL_019f: Unexpected return in MoveNext()*/;
		}
	}
}
