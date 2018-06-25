using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005A0 RID: 1440
	[StaticConstructorOnStartup]
	public class WorldRenderer
	{
		// Token: 0x04001056 RID: 4182
		private List<WorldLayer> layers = new List<WorldLayer>();

		// Token: 0x04001057 RID: 4183
		public WorldRenderMode wantedMode;

		// Token: 0x04001058 RID: 4184
		private bool asynchronousRegenerationActive = false;

		// Token: 0x06001B7A RID: 7034 RVA: 0x000ED888 File Offset: 0x000EBC88
		public WorldRenderer()
		{
			foreach (Type type in typeof(WorldLayer).AllLeafSubclasses())
			{
				this.layers.Add((WorldLayer)Activator.CreateInstance(type));
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06001B7B RID: 7035 RVA: 0x000ED918 File Offset: 0x000EBD18
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

		// Token: 0x06001B7C RID: 7036 RVA: 0x000ED984 File Offset: 0x000EBD84
		public void SetAllLayersDirty()
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				this.layers[i].SetDirty();
			}
		}

		// Token: 0x06001B7D RID: 7037 RVA: 0x000ED9C4 File Offset: 0x000EBDC4
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

		// Token: 0x06001B7E RID: 7038 RVA: 0x000EDA18 File Offset: 0x000EBE18
		public void RegenerateAllLayersNow()
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				this.layers[i].RegenerateNow();
			}
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x000EDA58 File Offset: 0x000EBE58
		private IEnumerable RegenerateDirtyLayersNow_Async()
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				if (this.layers[i].Dirty)
				{
					IEnumerator enumerator = this.layers[i].Regenerate().GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object result = enumerator.Current;
							yield return result;
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					yield return null;
				}
			}
			this.asynchronousRegenerationActive = false;
			yield break;
		}

		// Token: 0x06001B80 RID: 7040 RVA: 0x000EDA84 File Offset: 0x000EBE84
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

		// Token: 0x06001B81 RID: 7041 RVA: 0x000EDACE File Offset: 0x000EBECE
		public void CheckActivateWorldCamera()
		{
			Find.WorldCamera.gameObject.SetActive(WorldRendererUtility.WorldRenderedNow);
		}

		// Token: 0x06001B82 RID: 7042 RVA: 0x000EDAE8 File Offset: 0x000EBEE8
		public void DrawWorldLayers()
		{
			if (this.asynchronousRegenerationActive)
			{
				Log.Error("Called DrawWorldLayers() but already regenerating. This shouldn't ever happen because LongEventHandler should have stopped us.", false);
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

		// Token: 0x06001B83 RID: 7043 RVA: 0x000EDB70 File Offset: 0x000EBF70
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
	}
}
