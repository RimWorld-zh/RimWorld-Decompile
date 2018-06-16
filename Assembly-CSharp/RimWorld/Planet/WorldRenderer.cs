using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005A2 RID: 1442
	[StaticConstructorOnStartup]
	public class WorldRenderer
	{
		// Token: 0x06001B7F RID: 7039 RVA: 0x000ED410 File Offset: 0x000EB810
		public WorldRenderer()
		{
			foreach (Type type in typeof(WorldLayer).AllLeafSubclasses())
			{
				this.layers.Add((WorldLayer)Activator.CreateInstance(type));
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06001B80 RID: 7040 RVA: 0x000ED4A0 File Offset: 0x000EB8A0
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

		// Token: 0x06001B81 RID: 7041 RVA: 0x000ED50C File Offset: 0x000EB90C
		public void SetAllLayersDirty()
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				this.layers[i].SetDirty();
			}
		}

		// Token: 0x06001B82 RID: 7042 RVA: 0x000ED54C File Offset: 0x000EB94C
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

		// Token: 0x06001B83 RID: 7043 RVA: 0x000ED5A0 File Offset: 0x000EB9A0
		public void RegenerateAllLayersNow()
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				this.layers[i].RegenerateNow();
			}
		}

		// Token: 0x06001B84 RID: 7044 RVA: 0x000ED5E0 File Offset: 0x000EB9E0
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

		// Token: 0x06001B85 RID: 7045 RVA: 0x000ED60C File Offset: 0x000EBA0C
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

		// Token: 0x06001B86 RID: 7046 RVA: 0x000ED656 File Offset: 0x000EBA56
		public void CheckActivateWorldCamera()
		{
			Find.WorldCamera.gameObject.SetActive(WorldRendererUtility.WorldRenderedNow);
		}

		// Token: 0x06001B87 RID: 7047 RVA: 0x000ED670 File Offset: 0x000EBA70
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

		// Token: 0x06001B88 RID: 7048 RVA: 0x000ED6F8 File Offset: 0x000EBAF8
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

		// Token: 0x04001055 RID: 4181
		private List<WorldLayer> layers = new List<WorldLayer>();

		// Token: 0x04001056 RID: 4182
		public WorldRenderMode wantedMode;

		// Token: 0x04001057 RID: 4183
		private bool asynchronousRegenerationActive = false;
	}
}
