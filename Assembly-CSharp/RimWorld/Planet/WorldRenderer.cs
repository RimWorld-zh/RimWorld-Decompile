using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200059E RID: 1438
	[StaticConstructorOnStartup]
	public class WorldRenderer
	{
		// Token: 0x06001B77 RID: 7031 RVA: 0x000ED4D0 File Offset: 0x000EB8D0
		public WorldRenderer()
		{
			foreach (Type type in typeof(WorldLayer).AllLeafSubclasses())
			{
				this.layers.Add((WorldLayer)Activator.CreateInstance(type));
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06001B78 RID: 7032 RVA: 0x000ED560 File Offset: 0x000EB960
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

		// Token: 0x06001B79 RID: 7033 RVA: 0x000ED5CC File Offset: 0x000EB9CC
		public void SetAllLayersDirty()
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				this.layers[i].SetDirty();
			}
		}

		// Token: 0x06001B7A RID: 7034 RVA: 0x000ED60C File Offset: 0x000EBA0C
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

		// Token: 0x06001B7B RID: 7035 RVA: 0x000ED660 File Offset: 0x000EBA60
		public void RegenerateAllLayersNow()
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				this.layers[i].RegenerateNow();
			}
		}

		// Token: 0x06001B7C RID: 7036 RVA: 0x000ED6A0 File Offset: 0x000EBAA0
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

		// Token: 0x06001B7D RID: 7037 RVA: 0x000ED6CC File Offset: 0x000EBACC
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

		// Token: 0x06001B7E RID: 7038 RVA: 0x000ED716 File Offset: 0x000EBB16
		public void CheckActivateWorldCamera()
		{
			Find.WorldCamera.gameObject.SetActive(WorldRendererUtility.WorldRenderedNow);
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x000ED730 File Offset: 0x000EBB30
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

		// Token: 0x06001B80 RID: 7040 RVA: 0x000ED7B8 File Offset: 0x000EBBB8
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

		// Token: 0x04001052 RID: 4178
		private List<WorldLayer> layers = new List<WorldLayer>();

		// Token: 0x04001053 RID: 4179
		public WorldRenderMode wantedMode;

		// Token: 0x04001054 RID: 4180
		private bool asynchronousRegenerationActive = false;
	}
}
