using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld.Planet;
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

		public bool expandMore;

		public bool blockExitGridUntilBattleIsWon;

		public WorldObjectDef()
		{
		}

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

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<string> <ConfigErrors>__BaseCallProxy0()
		{
			return base.ConfigErrors();
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal IEnumerator<string> $locvar0;

			internal string <e>__1;

			internal int <i>__2;

			internal IEnumerator<string> $locvar1;

			internal string <e>__3;

			internal WorldObjectDef $this;

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
					Block_3:
					try
					{
						switch (num)
						{
						}
						if (enumerator2.MoveNext())
						{
							e2 = enumerator2.Current;
							this.$current = e2;
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
					goto IL_17E;
				case 3u:
					IL_1D8:
					this.$PC = -1;
					return false;
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
						e = enumerator.Current;
						this.$current = e;
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
				i = 0;
				IL_17E:
				if (i < this.comps.Count)
				{
					enumerator2 = this.comps[i].ConfigErrors(this).GetEnumerator();
					num = 4294967293u;
					goto Block_3;
				}
				if (this.expandMore && !this.expandingIcon)
				{
					this.$current = "has expandMore but doesn't have any expanding icon";
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				goto IL_1D8;
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
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				WorldObjectDef.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new WorldObjectDef.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}
		}
	}
}
