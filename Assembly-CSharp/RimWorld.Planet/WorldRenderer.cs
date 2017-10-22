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

		private bool asynchronousRegenerationActive;

		private bool ShouldRegenerateDirtyLayersInLongEvent
		{
			get
			{
				for (int i = 0; i < this.layers.Count; i++)
				{
					if (this.layers[i].Dirty && this.layers[i] is WorldLayer_Terrain)
					{
						return true;
					}
				}
				return false;
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
					foreach (object item in this.layers[i].Regenerate())
					{
						yield return item;
					}
					yield return (object)null;
				}
			}
			this.asynchronousRegenerationActive = false;
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
			while (num < count)
			{
				WorldLayer_Terrain worldLayer_Terrain = this.layers[num] as WorldLayer_Terrain;
				if (worldLayer_Terrain != null)
				{
					return worldLayer_Terrain.GetTileIDFromRayHit(hit);
				}
				num++;
			}
			return -1;
		}
	}
}
