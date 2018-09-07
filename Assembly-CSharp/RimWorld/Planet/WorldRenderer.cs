using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	[StaticConstructorOnStartup]
	public class WorldRenderer
	{
		private List<WorldLayer> layers = new List<WorldLayer>();

		public WorldRenderMode wantedMode;

		private bool asynchronousRegenerationActive;

		public WorldRenderer()
		{
			foreach (Type type in typeof(WorldLayer).AllLeafSubclasses())
			{
				this.layers.Add((WorldLayer)Activator.CreateInstance(type));
			}
		}

		private bool ShouldRegenerateDirtyLayersInLongEvent
		{
			get
			{
				for (int i = 0; i < this.layers.Count; i++)
				{
					if (this.layers[i].Dirty)
					{
						if (this.layers[i] is WorldLayer_Terrain)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		public void SetAllLayersDirty()
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				this.layers[i].SetDirty();
			}
		}

		public void SetDirty<T>() where T : WorldLayer
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				if (this.layers[i] is T)
				{
					this.layers[i].SetDirty();
				}
			}
		}

		public void RegenerateAllLayersNow()
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				this.layers[i].RegenerateNow();
			}
		}

		private IEnumerable RegenerateDirtyLayersNow_Async()
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				if (this.layers[i].Dirty)
				{
					using (IEnumerator enumerator = this.layers[i].Regenerate().GetEnumerator())
					{
						for (;;)
						{
							try
							{
								if (!enumerator.MoveNext())
								{
									break;
								}
							}
							catch (Exception arg)
							{
								Log.Error("Could not regenerate WorldLayer: " + arg, false);
								break;
							}
							yield return enumerator.Current;
						}
					}
					yield return null;
				}
			}
			this.asynchronousRegenerationActive = false;
			yield break;
		}

		public void Notify_StaticWorldObjectPosChanged()
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				WorldLayer_WorldObjects worldLayer_WorldObjects = this.layers[i] as WorldLayer_WorldObjects;
				if (worldLayer_WorldObjects != null)
				{
					worldLayer_WorldObjects.SetDirty();
				}
			}
		}

		public void CheckActivateWorldCamera()
		{
			Find.WorldCamera.gameObject.SetActive(WorldRendererUtility.WorldRenderedNow);
		}

		public void DrawWorldLayers()
		{
			if (this.asynchronousRegenerationActive)
			{
				Log.Error("Called DrawWorldLayers() but already regenerating. This shouldn't ever happen because LongEventHandler should have stopped us.", false);
				return;
			}
			if (this.ShouldRegenerateDirtyLayersInLongEvent)
			{
				this.asynchronousRegenerationActive = true;
				LongEventHandler.QueueLongEvent(this.RegenerateDirtyLayersNow_Async(), "GeneratingPlanet", null);
				return;
			}
			WorldRendererUtility.UpdateWorldShadersParams();
			for (int i = 0; i < this.layers.Count; i++)
			{
				try
				{
					this.layers[i].Render();
				}
				catch (Exception arg)
				{
					Log.Error("Error drawing WorldLayer: " + arg, false);
				}
			}
		}

		public int GetTileIDFromRayHit(RaycastHit hit)
		{
			int i = 0;
			int count = this.layers.Count;
			while (i < count)
			{
				WorldLayer_Terrain worldLayer_Terrain = this.layers[i] as WorldLayer_Terrain;
				if (worldLayer_Terrain != null)
				{
					return worldLayer_Terrain.GetTileIDFromRayHit(hit);
				}
				i++;
			}
			return -1;
		}

		[CompilerGenerated]
		private sealed class <RegenerateDirtyLayersNow_Async>c__Iterator0 : IEnumerable, IEnumerable<object>, IEnumerator, IDisposable, IEnumerator<object>
		{
			internal int <i>__1;

			internal IEnumerator <enumerator>__2;

			internal WorldRenderer $this;

			internal object $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <RegenerateDirtyLayersNow_Async>c__Iterator0()
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
					Block_3:
					try
					{
						switch (num)
						{
						}
						try
						{
							if (!enumerator.MoveNext())
							{
								goto IL_EA;
							}
						}
						catch (Exception arg)
						{
							Log.Error("Could not regenerate WorldLayer: " + arg, false);
							goto IL_EA;
						}
						this.$current = enumerator.Current;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
						IL_EA:;
					}
					finally
					{
						if (!flag)
						{
							this.<>__Finally0();
						}
					}
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					IL_115:
					i++;
					break;
				default:
					return false;
				}
				if (i >= this.layers.Count)
				{
					this.asynchronousRegenerationActive = false;
					this.$PC = -1;
				}
				else
				{
					if (!this.layers[i].Dirty)
					{
						goto IL_115;
					}
					enumerator = this.layers[i].Regenerate().GetEnumerator();
					num = 4294967293u;
					goto Block_3;
				}
				return false;
			}

			object IEnumerator<object>.Current
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
						this.<>__Finally0();
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
				return this.System.Collections.Generic.IEnumerable<object>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<object> IEnumerable<object>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				WorldRenderer.<RegenerateDirtyLayersNow_Async>c__Iterator0 <RegenerateDirtyLayersNow_Async>c__Iterator = new WorldRenderer.<RegenerateDirtyLayersNow_Async>c__Iterator0();
				<RegenerateDirtyLayersNow_Async>c__Iterator.$this = this;
				return <RegenerateDirtyLayersNow_Async>c__Iterator;
			}

			private void <>__Finally0()
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}
	}
}
