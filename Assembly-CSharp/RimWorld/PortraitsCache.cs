using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007B6 RID: 1974
	[StaticConstructorOnStartup]
	public static class PortraitsCache
	{
		// Token: 0x04001788 RID: 6024
		private static List<RenderTexture> renderTexturesPool = new List<RenderTexture>();

		// Token: 0x04001789 RID: 6025
		private static List<PortraitsCache.CachedPortraitsWithParams> cachedPortraits = new List<PortraitsCache.CachedPortraitsWithParams>();

		// Token: 0x0400178A RID: 6026
		private static List<Pawn> toRemove = new List<Pawn>();

		// Token: 0x0400178B RID: 6027
		private static List<Pawn> toSetDirty = new List<Pawn>();

		// Token: 0x06002BBB RID: 11195 RVA: 0x00172DFC File Offset: 0x001711FC
		public static RenderTexture Get(Pawn pawn, Vector2 size, Vector3 cameraOffset = default(Vector3), float cameraZoom = 1f)
		{
			Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary = PortraitsCache.GetOrCreateCachedPortraitsWithParams(size, cameraOffset, cameraZoom).CachedPortraits;
			PortraitsCache.CachedPortrait cachedPortrait;
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
				dictionary.Add(pawn, new PortraitsCache.CachedPortrait(cachedPortrait.RenderTexture, false, Time.time));
				result = cachedPortrait.RenderTexture;
			}
			else
			{
				RenderTexture renderTexture = PortraitsCache.NewRenderTexture(size);
				PortraitsCache.RenderPortrait(pawn, renderTexture, cameraOffset, cameraZoom);
				dictionary.Add(pawn, new PortraitsCache.CachedPortrait(renderTexture, false, Time.time));
				result = renderTexture;
			}
			return result;
		}

		// Token: 0x06002BBC RID: 11196 RVA: 0x00172ED8 File Offset: 0x001712D8
		public static void SetDirty(Pawn pawn)
		{
			for (int i = 0; i < PortraitsCache.cachedPortraits.Count; i++)
			{
				Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary = PortraitsCache.cachedPortraits[i].CachedPortraits;
				PortraitsCache.CachedPortrait cachedPortrait;
				if (dictionary.TryGetValue(pawn, out cachedPortrait) && !cachedPortrait.Dirty)
				{
					dictionary.Remove(pawn);
					dictionary.Add(pawn, new PortraitsCache.CachedPortrait(cachedPortrait.RenderTexture, true, cachedPortrait.LastUseTime));
				}
			}
		}

		// Token: 0x06002BBD RID: 11197 RVA: 0x00172F56 File Offset: 0x00171356
		public static void PortraitsCacheUpdate()
		{
			PortraitsCache.RemoveExpiredCachedPortraits();
			PortraitsCache.SetAnimatedPortraitsDirty();
		}

		// Token: 0x06002BBE RID: 11198 RVA: 0x00172F64 File Offset: 0x00171364
		public static void Clear()
		{
			for (int i = 0; i < PortraitsCache.cachedPortraits.Count; i++)
			{
				foreach (KeyValuePair<Pawn, PortraitsCache.CachedPortrait> keyValuePair in PortraitsCache.cachedPortraits[i].CachedPortraits)
				{
					PortraitsCache.DestroyRenderTexture(keyValuePair.Value.RenderTexture);
				}
			}
			PortraitsCache.cachedPortraits.Clear();
			for (int j = 0; j < PortraitsCache.renderTexturesPool.Count; j++)
			{
				PortraitsCache.DestroyRenderTexture(PortraitsCache.renderTexturesPool[j]);
			}
			PortraitsCache.renderTexturesPool.Clear();
		}

		// Token: 0x06002BBF RID: 11199 RVA: 0x00173044 File Offset: 0x00171444
		private static PortraitsCache.CachedPortraitsWithParams GetOrCreateCachedPortraitsWithParams(Vector2 size, Vector3 cameraOffset, float cameraZoom)
		{
			for (int i = 0; i < PortraitsCache.cachedPortraits.Count; i++)
			{
				if (PortraitsCache.cachedPortraits[i].Size == size && PortraitsCache.cachedPortraits[i].CameraOffset == cameraOffset && PortraitsCache.cachedPortraits[i].CameraZoom == cameraZoom)
				{
					return PortraitsCache.cachedPortraits[i];
				}
			}
			PortraitsCache.CachedPortraitsWithParams cachedPortraitsWithParams = new PortraitsCache.CachedPortraitsWithParams(size, cameraOffset, cameraZoom);
			PortraitsCache.cachedPortraits.Add(cachedPortraitsWithParams);
			return cachedPortraitsWithParams;
		}

		// Token: 0x06002BC0 RID: 11200 RVA: 0x001730F8 File Offset: 0x001714F8
		private static void DestroyRenderTexture(RenderTexture rt)
		{
			rt.DiscardContents();
			UnityEngine.Object.Destroy(rt);
		}

		// Token: 0x06002BC1 RID: 11201 RVA: 0x00173108 File Offset: 0x00171508
		private static void RemoveExpiredCachedPortraits()
		{
			for (int i = 0; i < PortraitsCache.cachedPortraits.Count; i++)
			{
				Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary = PortraitsCache.cachedPortraits[i].CachedPortraits;
				PortraitsCache.toRemove.Clear();
				foreach (KeyValuePair<Pawn, PortraitsCache.CachedPortrait> keyValuePair in dictionary)
				{
					if (keyValuePair.Value.Expired)
					{
						PortraitsCache.toRemove.Add(keyValuePair.Key);
						PortraitsCache.renderTexturesPool.Add(keyValuePair.Value.RenderTexture);
					}
				}
				for (int j = 0; j < PortraitsCache.toRemove.Count; j++)
				{
					dictionary.Remove(PortraitsCache.toRemove[j]);
				}
				PortraitsCache.toRemove.Clear();
			}
		}

		// Token: 0x06002BC2 RID: 11202 RVA: 0x00173218 File Offset: 0x00171618
		private static void SetAnimatedPortraitsDirty()
		{
			for (int i = 0; i < PortraitsCache.cachedPortraits.Count; i++)
			{
				Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary = PortraitsCache.cachedPortraits[i].CachedPortraits;
				PortraitsCache.toSetDirty.Clear();
				foreach (KeyValuePair<Pawn, PortraitsCache.CachedPortrait> keyValuePair in dictionary)
				{
					if (PortraitsCache.IsAnimated(keyValuePair.Key) && !keyValuePair.Value.Dirty)
					{
						PortraitsCache.toSetDirty.Add(keyValuePair.Key);
					}
				}
				for (int j = 0; j < PortraitsCache.toSetDirty.Count; j++)
				{
					PortraitsCache.CachedPortrait cachedPortrait = dictionary[PortraitsCache.toSetDirty[j]];
					dictionary.Remove(PortraitsCache.toSetDirty[j]);
					dictionary.Add(PortraitsCache.toSetDirty[j], new PortraitsCache.CachedPortrait(cachedPortrait.RenderTexture, true, cachedPortrait.LastUseTime));
				}
				PortraitsCache.toSetDirty.Clear();
			}
		}

		// Token: 0x06002BC3 RID: 11203 RVA: 0x00173358 File Offset: 0x00171758
		private static RenderTexture NewRenderTexture(Vector2 size)
		{
			int num = PortraitsCache.renderTexturesPool.FindLastIndex((RenderTexture x) => x.width == (int)size.x && x.height == (int)size.y);
			RenderTexture result;
			if (num != -1)
			{
				RenderTexture renderTexture = PortraitsCache.renderTexturesPool[num];
				PortraitsCache.renderTexturesPool.RemoveAt(num);
				result = renderTexture;
			}
			else
			{
				RenderTexture renderTexture2 = new RenderTexture((int)size.x, (int)size.y, 24);
				result = renderTexture2;
			}
			return result;
		}

		// Token: 0x06002BC4 RID: 11204 RVA: 0x001733DA File Offset: 0x001717DA
		private static void RenderPortrait(Pawn pawn, RenderTexture renderTexture, Vector3 cameraOffset, float cameraZoom)
		{
			Find.PortraitRenderer.RenderPortrait(pawn, renderTexture, cameraOffset, cameraZoom);
		}

		// Token: 0x06002BC5 RID: 11205 RVA: 0x001733EC File Offset: 0x001717EC
		private static bool IsAnimated(Pawn pawn)
		{
			return Current.ProgramState == ProgramState.Playing && pawn.Drawer.renderer.graphics.flasher.FlashingNowOrRecently;
		}

		// Token: 0x020007B7 RID: 1975
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct CachedPortrait
		{
			// Token: 0x0400178F RID: 6031
			private const float CacheDuration = 1f;

			// Token: 0x06002BC7 RID: 11207 RVA: 0x0017345E File Offset: 0x0017185E
			public CachedPortrait(RenderTexture renderTexture, bool dirty, float lastUseTime)
			{
				this = default(PortraitsCache.CachedPortrait);
				this.RenderTexture = renderTexture;
				this.Dirty = dirty;
				this.LastUseTime = lastUseTime;
			}

			// Token: 0x170006D1 RID: 1745
			// (get) Token: 0x06002BC8 RID: 11208 RVA: 0x00173480 File Offset: 0x00171880
			// (set) Token: 0x06002BC9 RID: 11209 RVA: 0x0017349A File Offset: 0x0017189A
			public RenderTexture RenderTexture { get; private set; }

			// Token: 0x170006D2 RID: 1746
			// (get) Token: 0x06002BCA RID: 11210 RVA: 0x001734A4 File Offset: 0x001718A4
			// (set) Token: 0x06002BCB RID: 11211 RVA: 0x001734BE File Offset: 0x001718BE
			public bool Dirty { get; private set; }

			// Token: 0x170006D3 RID: 1747
			// (get) Token: 0x06002BCC RID: 11212 RVA: 0x001734C8 File Offset: 0x001718C8
			// (set) Token: 0x06002BCD RID: 11213 RVA: 0x001734E2 File Offset: 0x001718E2
			public float LastUseTime { get; private set; }

			// Token: 0x170006D4 RID: 1748
			// (get) Token: 0x06002BCE RID: 11214 RVA: 0x001734EC File Offset: 0x001718EC
			public bool Expired
			{
				get
				{
					return Time.time - this.LastUseTime > 1f;
				}
			}
		}

		// Token: 0x020007B8 RID: 1976
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct CachedPortraitsWithParams
		{
			// Token: 0x06002BCF RID: 11215 RVA: 0x00173514 File Offset: 0x00171914
			public CachedPortraitsWithParams(Vector2 size, Vector3 cameraOffset, float cameraZoom)
			{
				this = default(PortraitsCache.CachedPortraitsWithParams);
				this.CachedPortraits = new Dictionary<Pawn, PortraitsCache.CachedPortrait>();
				this.Size = size;
				this.CameraOffset = cameraOffset;
				this.CameraZoom = cameraZoom;
			}

			// Token: 0x170006D5 RID: 1749
			// (get) Token: 0x06002BD0 RID: 11216 RVA: 0x00173540 File Offset: 0x00171940
			// (set) Token: 0x06002BD1 RID: 11217 RVA: 0x0017355A File Offset: 0x0017195A
			public Dictionary<Pawn, PortraitsCache.CachedPortrait> CachedPortraits { get; private set; }

			// Token: 0x170006D6 RID: 1750
			// (get) Token: 0x06002BD2 RID: 11218 RVA: 0x00173564 File Offset: 0x00171964
			// (set) Token: 0x06002BD3 RID: 11219 RVA: 0x0017357E File Offset: 0x0017197E
			public Vector2 Size { get; private set; }

			// Token: 0x170006D7 RID: 1751
			// (get) Token: 0x06002BD4 RID: 11220 RVA: 0x00173588 File Offset: 0x00171988
			// (set) Token: 0x06002BD5 RID: 11221 RVA: 0x001735A2 File Offset: 0x001719A2
			public Vector3 CameraOffset { get; private set; }

			// Token: 0x170006D8 RID: 1752
			// (get) Token: 0x06002BD6 RID: 11222 RVA: 0x001735AC File Offset: 0x001719AC
			// (set) Token: 0x06002BD7 RID: 11223 RVA: 0x001735C6 File Offset: 0x001719C6
			public float CameraZoom { get; private set; }
		}
	}
}
