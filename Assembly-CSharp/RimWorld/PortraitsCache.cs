using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public static class PortraitsCache
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct CachedPortrait
		{
			private const float CacheDuration = 1f;

			public RenderTexture RenderTexture
			{
				get;
				private set;
			}

			public bool Dirty
			{
				get;
				private set;
			}

			public float LastUseTime
			{
				get;
				private set;
			}

			public bool Expired
			{
				get
				{
					return Time.time - this.LastUseTime > 1.0;
				}
			}

			public CachedPortrait(RenderTexture renderTexture, bool dirty, float lastUseTime)
			{
				this = default(CachedPortrait);
				this.RenderTexture = renderTexture;
				this.Dirty = dirty;
				this.LastUseTime = lastUseTime;
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct CachedPortraitsWithParams
		{
			public Dictionary<Pawn, CachedPortrait> CachedPortraits
			{
				get;
				private set;
			}

			public Vector2 Size
			{
				get;
				private set;
			}

			public Vector3 CameraOffset
			{
				get;
				private set;
			}

			public float CameraZoom
			{
				get;
				private set;
			}

			public CachedPortraitsWithParams(Vector2 size, Vector3 cameraOffset, float cameraZoom)
			{
				this = default(CachedPortraitsWithParams);
				this.CachedPortraits = new Dictionary<Pawn, CachedPortrait>();
				this.Size = size;
				this.CameraOffset = cameraOffset;
				this.CameraZoom = cameraZoom;
			}
		}

		private static List<RenderTexture> renderTexturesPool = new List<RenderTexture>();

		private static List<CachedPortraitsWithParams> cachedPortraits = new List<CachedPortraitsWithParams>();

		private static List<Pawn> toRemove = new List<Pawn>();

		private static List<Pawn> toSetDirty = new List<Pawn>();

		public static RenderTexture Get(Pawn pawn, Vector2 size, Vector3 cameraOffset = default(Vector3), float cameraZoom = 1f)
		{
			Dictionary<Pawn, CachedPortrait> dictionary = PortraitsCache.GetOrCreateCachedPortraitsWithParams(size, cameraOffset, cameraZoom).CachedPortraits;
			CachedPortrait cachedPortrait = default(CachedPortrait);
			RenderTexture result;
			if (dictionary.TryGetValue(pawn, out cachedPortrait))
			{
				if (!cachedPortrait.RenderTexture.IsCreated())
				{
					cachedPortrait.RenderTexture.Create();
					PortraitsCache.RenderPortrait(pawn, cachedPortrait.RenderTexture, cameraOffset, cameraZoom);
				}
				else if (cachedPortrait.Dirty)
				{
					PortraitsCache.RenderPortrait(pawn, cachedPortrait.RenderTexture, cameraOffset, cameraZoom);
				}
				dictionary.Remove(pawn);
				dictionary.Add(pawn, new CachedPortrait(cachedPortrait.RenderTexture, false, Time.time));
				result = cachedPortrait.RenderTexture;
			}
			else
			{
				RenderTexture renderTexture = PortraitsCache.NewRenderTexture(size);
				PortraitsCache.RenderPortrait(pawn, renderTexture, cameraOffset, cameraZoom);
				dictionary.Add(pawn, new CachedPortrait(renderTexture, false, Time.time));
				result = renderTexture;
			}
			return result;
		}

		public static void SetDirty(Pawn pawn)
		{
			for (int i = 0; i < PortraitsCache.cachedPortraits.Count; i++)
			{
				Dictionary<Pawn, CachedPortrait> dictionary = PortraitsCache.cachedPortraits[i].CachedPortraits;
				CachedPortrait cachedPortrait = default(CachedPortrait);
				if (dictionary.TryGetValue(pawn, out cachedPortrait) && !cachedPortrait.Dirty)
				{
					dictionary.Remove(pawn);
					dictionary.Add(pawn, new CachedPortrait(cachedPortrait.RenderTexture, true, cachedPortrait.LastUseTime));
				}
			}
		}

		public static void PortraitsCacheUpdate()
		{
			PortraitsCache.RemoveExpiredCachedPortraits();
			PortraitsCache.SetAnimatedPortraitsDirty();
		}

		public static void Clear()
		{
			for (int i = 0; i < PortraitsCache.cachedPortraits.Count; i++)
			{
				foreach (KeyValuePair<Pawn, CachedPortrait> cachedPortrait in PortraitsCache.cachedPortraits[i].CachedPortraits)
				{
					PortraitsCache.DestroyRenderTexture(cachedPortrait.Value.RenderTexture);
				}
			}
			PortraitsCache.cachedPortraits.Clear();
			for (int j = 0; j < PortraitsCache.renderTexturesPool.Count; j++)
			{
				PortraitsCache.DestroyRenderTexture(PortraitsCache.renderTexturesPool[j]);
			}
			PortraitsCache.renderTexturesPool.Clear();
		}

		private static CachedPortraitsWithParams GetOrCreateCachedPortraitsWithParams(Vector2 size, Vector3 cameraOffset, float cameraZoom)
		{
			int num = 0;
			CachedPortraitsWithParams result;
			while (true)
			{
				if (num < PortraitsCache.cachedPortraits.Count)
				{
					if (PortraitsCache.cachedPortraits[num].Size == size && PortraitsCache.cachedPortraits[num].CameraOffset == cameraOffset && PortraitsCache.cachedPortraits[num].CameraZoom == cameraZoom)
					{
						result = PortraitsCache.cachedPortraits[num];
						break;
					}
					num++;
					continue;
				}
				CachedPortraitsWithParams cachedPortraitsWithParams = new CachedPortraitsWithParams(size, cameraOffset, cameraZoom);
				PortraitsCache.cachedPortraits.Add(cachedPortraitsWithParams);
				result = cachedPortraitsWithParams;
				break;
			}
			return result;
		}

		private static void DestroyRenderTexture(RenderTexture rt)
		{
			rt.DiscardContents();
			UnityEngine.Object.Destroy(rt);
		}

		private static void RemoveExpiredCachedPortraits()
		{
			for (int i = 0; i < PortraitsCache.cachedPortraits.Count; i++)
			{
				Dictionary<Pawn, CachedPortrait> dictionary = PortraitsCache.cachedPortraits[i].CachedPortraits;
				PortraitsCache.toRemove.Clear();
				foreach (KeyValuePair<Pawn, CachedPortrait> item in dictionary)
				{
					if (item.Value.Expired)
					{
						PortraitsCache.toRemove.Add(item.Key);
						PortraitsCache.renderTexturesPool.Add(item.Value.RenderTexture);
					}
				}
				for (int j = 0; j < PortraitsCache.toRemove.Count; j++)
				{
					dictionary.Remove(PortraitsCache.toRemove[j]);
				}
				PortraitsCache.toRemove.Clear();
			}
		}

		private static void SetAnimatedPortraitsDirty()
		{
			for (int i = 0; i < PortraitsCache.cachedPortraits.Count; i++)
			{
				Dictionary<Pawn, CachedPortrait> dictionary = PortraitsCache.cachedPortraits[i].CachedPortraits;
				PortraitsCache.toSetDirty.Clear();
				foreach (KeyValuePair<Pawn, CachedPortrait> item in dictionary)
				{
					if (PortraitsCache.IsAnimated(item.Key) && !item.Value.Dirty)
					{
						PortraitsCache.toSetDirty.Add(item.Key);
					}
				}
				for (int j = 0; j < PortraitsCache.toSetDirty.Count; j++)
				{
					CachedPortrait cachedPortrait = dictionary[PortraitsCache.toSetDirty[j]];
					dictionary.Remove(PortraitsCache.toSetDirty[j]);
					dictionary.Add(PortraitsCache.toSetDirty[j], new CachedPortrait(cachedPortrait.RenderTexture, true, cachedPortrait.LastUseTime));
				}
				PortraitsCache.toSetDirty.Clear();
			}
		}

		private static RenderTexture NewRenderTexture(Vector2 size)
		{
			int num = PortraitsCache.renderTexturesPool.FindLastIndex((Predicate<RenderTexture>)((RenderTexture x) => x.width == (int)size.x && x.height == (int)size.y));
			RenderTexture result;
			if (num != -1)
			{
				RenderTexture renderTexture = PortraitsCache.renderTexturesPool[num];
				PortraitsCache.renderTexturesPool.RemoveAt(num);
				result = renderTexture;
			}
			else
			{
				RenderTexture renderTexture2 = result = new RenderTexture((int)size.x, (int)size.y, 24);
			}
			return result;
		}

		private static void RenderPortrait(Pawn pawn, RenderTexture renderTexture, Vector3 cameraOffset, float cameraZoom)
		{
			Find.PortraitRenderer.RenderPortrait(pawn, renderTexture, cameraOffset, cameraZoom);
		}

		private static bool IsAnimated(Pawn pawn)
		{
			return (byte)((Current.ProgramState == ProgramState.Playing && pawn.Drawer.renderer.graphics.flasher.FlashingNowOrRecently) ? 1 : 0) != 0;
		}
	}
}
