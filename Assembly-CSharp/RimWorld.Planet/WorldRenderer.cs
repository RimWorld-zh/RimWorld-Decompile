using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	[StaticConstructorOnStartup]
	public class WorldRenderer
	{
		private List<WorldLayer> layers = new List<WorldLayer>();

		public WorldRenderMode wantedMode;

		private bool asynchronousRegenerationActive = false;

		private bool ShouldRegenerateDirtyLayersInLongEvent
		{
			get
			{
				int num = 0;
				bool result;
				while (true)
				{
					if (num < this.layers.Count)
					{
						if (this.layers[num].Dirty && this.layers[num] is WorldLayer_Terrain)
						{
							result = true;
							break;
						}
						num++;
						continue;
					}
					result = false;
					break;
				}
				return result;
			}
		}

		public WorldRenderer()
		{
			foreach (Type item in typeof(WorldLayer).AllLeafSubclasses())
			{
				this.layers.Add((WorldLayer)Activator.CreateInstance(item));
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
					IEnumerator enumerator = this.layers[i].Regenerate().GetEnumerator();
					try
					{
						if (enumerator.MoveNext())
						{
							object result = enumerator.Current;
							yield return result;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
					finally
					{
						IDisposable disposable;
						IDisposable disposable2 = disposable = (enumerator as IDisposable);
						if (disposable != null)
						{
							disposable2.Dispose();
						}
					}
					yield return (object)null;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			this.asynchronousRegenerationActive = false;
			yield break;
			IL_0162:
			/*Error near IL_0163: Unexpected return in MoveNext()*/;
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
				Log.Error("Called DrawWorldLayers() but already regenerating. This shouldn't ever happen because LongEventHandler should have stopped us.");
			}
			else if (this.ShouldRegenerateDirtyLayersInLongEvent)
			{
				this.asynchronousRegenerationActive = true;
				LongEventHandler.QueueLongEvent(this.RegenerateDirtyLayersNow_Async(), "GeneratingPlanet", null);
			}
			else
			{
				WorldRendererUtility.UpdateWorldShadersParams();
				for (int i = 0; i < this.layers.Count; i++)
				{
					this.layers[i].Render();
				}
			}
		}

		public int GetTileIDFromRayHit(RaycastHit hit)
		{
			int num = 0;
			int count = this.layers.Count;
			int result;
			while (true)
			{
				if (num < count)
				{
					WorldLayer_Terrain worldLayer_Terrain = this.layers[num] as WorldLayer_Terrain;
					if (worldLayer_Terrain != null)
					{
						result = worldLayer_Terrain.GetTileIDFromRayHit(hit);
						break;
					}
					num++;
					continue;
				}
				result = -1;
				break;
			}
			return result;
		}
	}
}
