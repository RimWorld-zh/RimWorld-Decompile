using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020002F0 RID: 752
	public class WorldObjectDef : Def
	{
		// Token: 0x04000818 RID: 2072
		public Type worldObjectClass = typeof(WorldObject);

		// Token: 0x04000819 RID: 2073
		public bool canHaveFaction = true;

		// Token: 0x0400081A RID: 2074
		public bool saved = true;

		// Token: 0x0400081B RID: 2075
		public List<WorldObjectCompProperties> comps = new List<WorldObjectCompProperties>();

		// Token: 0x0400081C RID: 2076
		public bool allowCaravanIncidentsWhichGenerateMap;

		// Token: 0x0400081D RID: 2077
		public bool isTempIncidentMapOwner;

		// Token: 0x0400081E RID: 2078
		public List<IncidentTargetTypeDef> incidentTargetTypes;

		// Token: 0x0400081F RID: 2079
		public bool selectable = true;

		// Token: 0x04000820 RID: 2080
		public bool neverMultiSelect;

		// Token: 0x04000821 RID: 2081
		public MapGeneratorDef mapGenerator = null;

		// Token: 0x04000822 RID: 2082
		public List<Type> inspectorTabs;

		// Token: 0x04000823 RID: 2083
		[Unsaved]
		public List<InspectTabBase> inspectorTabsResolved;

		// Token: 0x04000824 RID: 2084
		public bool useDynamicDrawer;

		// Token: 0x04000825 RID: 2085
		public bool expandingIcon;

		// Token: 0x04000826 RID: 2086
		[NoTranslate]
		public string expandingIconTexture;

		// Token: 0x04000827 RID: 2087
		public float expandingIconPriority;

		// Token: 0x04000828 RID: 2088
		[NoTranslate]
		public string texture;

		// Token: 0x04000829 RID: 2089
		[Unsaved]
		private Material material;

		// Token: 0x0400082A RID: 2090
		[Unsaved]
		private Texture2D expandingIconTextureInt;

		// Token: 0x0400082B RID: 2091
		public bool expandMore;

		// Token: 0x0400082C RID: 2092
		public bool blockExitGridUntilBattleIsWon;

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000C68 RID: 3176 RVA: 0x0006E334 File Offset: 0x0006C734
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
					if (this.material == null)
					{
						this.material = MaterialPool.MatFrom(this.texture, ShaderDatabase.WorldOverlayTransparentLit, WorldMaterials.WorldObjectRenderQueue);
					}
					result = this.material;
				}
				return result;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000C69 RID: 3177 RVA: 0x0006E394 File Offset: 0x0006C794
		public Texture2D ExpandingIconTexture
		{
			get
			{
				if (this.expandingIconTextureInt == null)
				{
					if (this.expandingIconTexture.NullOrEmpty())
					{
						return null;
					}
					this.expandingIconTextureInt = ContentFinder<Texture2D>.Get(this.expandingIconTexture, true);
				}
				return this.expandingIconTextureInt;
			}
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x0006E3EC File Offset: 0x0006C7EC
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
						Log.Error(string.Concat(new object[]
						{
							"Could not instantiate inspector tab of type ",
							this.inspectorTabs[i],
							": ",
							ex
						}), false);
					}
				}
			}
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x0006E4B0 File Offset: 0x0006C8B0
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].ResolveReferences(this);
			}
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x0006E4F4 File Offset: 0x0006C8F4
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return e;
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				foreach (string e2 in this.comps[i].ConfigErrors(this))
				{
					yield return e2;
				}
			}
			if (this.expandMore && !this.expandingIcon)
			{
				yield return "has expandMore but doesn't have any expanding icon";
			}
			yield break;
		}
	}
}
