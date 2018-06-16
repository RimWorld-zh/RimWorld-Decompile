using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007B8 RID: 1976
	[StaticConstructorOnStartup]
	public static class PortraitsCache
	{
		// Token: 0x06002BBD RID: 11197 RVA: 0x001727DC File Offset: 0x00170BDC
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

		// Token: 0x06002BBE RID: 11198 RVA: 0x001728B8 File Offset: 0x00170CB8
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

		// Token: 0x06002BBF RID: 11199 RVA: 0x00172936 File Offset: 0x00170D36
		public static void PortraitsCacheUpdate()
		{
			PortraitsCache.RemoveExpiredCachedPortraits();
			PortraitsCache.SetAnimatedPortraitsDirty();
		}

		// Token: 0x06002BC0 RID: 11200 RVA: 0x00172944 File Offset: 0x00170D44
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

		// Token: 0x06002BC1 RID: 11201 RVA: 0x00172A24 File Offset: 0x00170E24
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

		// Token: 0x06002BC2 RID: 11202 RVA: 0x00172AD8 File Offset: 0x00170ED8
		private static void DestroyRenderTexture(RenderTexture rt)
		{
			rt.DiscardContents();
			UnityEngine.Object.Destroy(rt);
		}

		// Token: 0x06002BC3 RID: 11203 RVA: 0x00172AE8 File Offset: 0x00170EE8
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

		// Token: 0x06002BC4 RID: 11204 RVA: 0x00172BF8 File Offset: 0x00170FF8
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

		// Token: 0x06002BC5 RID: 11205 RVA: 0x00172D38 File Offset: 0x00171138
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

		// Token: 0x06002BC6 RID: 11206 RVA: 0x00172DBA File Offset: 0x001711BA
		private static void RenderPortrait(Pawn pawn, RenderTexture renderTexture, Vector3 cameraOffset, float cameraZoom)
		{
			Find.PortraitRenderer.RenderPortrait(pawn, renderTexture, cameraOffset, cameraZoom);
		}

		// Token: 0x06002BC7 RID: 11207 RVA: 0x00172DCC File Offset: 0x001711CC
		private static bool IsAnimated(Pawn pawn)
		{
			return Current.ProgramState == ProgramState.Playing && pawn.Drawer.renderer.graphics.flasher.FlashingNowOrRecently;
		}

		// Token: 0x04001786 RID: 6022
		private static List<RenderTexture> renderTexturesPool = new List<RenderTexture>();

		// Token: 0x04001787 RID: 6023
		private static List<PortraitsCache.CachedPortraitsWithParams> cachedPortraits = new List<PortraitsCache.CachedPortraitsWithParams>();

		// Token: 0x04001788 RID: 6024
		private static List<Pawn> toRemove = new List<Pawn>();

		// Token: 0x04001789 RID: 6025
		private static List<Pawn> toSetDirty = new List<Pawn>();

		// Token: 0x020007B9 RID: 1977
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct CachedPortrait
		{
			// Token: 0x06002BC9 RID: 11209 RVA: 0x00172E3E File Offset: 0x0017123E
			public CachedPortrait(RenderTexture renderTexture, bool dirty, float lastUseTime)
			{
				this = default(PortraitsCache.CachedPortrait);
				this.RenderTexture = renderTexture;
				this.Dirty = dirty;
				this.LastUseTime = lastUseTime;
			}

			// Token: 0x170006D0 RID: 1744
			// (get) Token: 0x06002BCA RID: 11210 RVA: 0x00172E60 File Offset: 0x00171260
			// (set) Token: 0x06002BCB RID: 11211 RVA: 0x00172E7A File Offset: 0x0017127A
			public RenderTexture RenderTexture { get; private set; }

			// Token: 0x170006D1 RID: 1745
			// (get) Token: 0x06002BCC RID: 11212 RVA: 0x00172E84 File Offset: 0x00171284
			// (set) Token: 0x06002BCD RID: 11213 RVA: 0x00172E9E File Offset: 0x0017129E
			public bool Dirty { get; private set; }

			// Token: 0x170006D2 RID: 1746
			// (get) Token: 0x06002BCE RID: 11214 RVA: 0x00172EA8 File Offset: 0x001712A8
			// (set) Token: 0x06002BCF RID: 11215 RVA: 0x00172EC2 File Offset: 0x001712C2
			public float LastUseTime { get; private set; }

			// Token: 0x170006D3 RID: 1747
			// (get) Token: 0x06002BD0 RID: 11216 RVA: 0x00172ECC File Offset: 0x001712CC
			public bool Expired
			{
				get
				{
					return Time.time - this.LastUseTime > 1f;
				}
			}

			// Token: 0x0400178D RID: 6029
			private const float CacheDuration = 1f;
		}

		// Token: 0x020007BA RID: 1978
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct CachedPortraitsWithParams
		{
			// Token: 0x06002BD1 RID: 11217 RVA: 0x00172EF4 File Offset: 0x001712F4
			public CachedPortraitsWithParams(Vector2 size, Vector3 cameraOffset, float cameraZoom)
			{
				this = default(PortraitsCache.CachedPortraitsWithParams);
				this.CachedPortraits = new Dictionary<Pawn, PortraitsCache.CachedPortrait>();
				this.Size = size;
				this.CameraOffset = cameraOffset;
				this.CameraZoom = cameraZoom;
			}

			// Token: 0x170006D4 RID: 1748
			// (get) Token: 0x06002BD2 RID: 11218 RVA: 0x00172F20 File Offset: 0x00171320
			// (set) Token: 0x06002BD3 RID: 11219 RVA: 0x00172F3A File Offset: 0x0017133A
			public Dictionary<Pawn, PortraitsCache.CachedPortrait> CachedPortraits { get; private set; }

			// Token: 0x170006D5 RID: 1749
			// (get) Token: 0x06002BD4 RID: 11220 RVA: 0x00172F44 File Offset: 0x00171344
			// (set) Token: 0x06002BD5 RID: 11221 RVA: 0x00172F5E File Offset: 0x0017135E
			public Vector2 Size { get; private set; }

			// Token: 0x170006D6 RID: 1750
			// (get) Token: 0x06002BD6 RID: 11222 RVA: 0x00172F68 File Offset: 0x00171368
			// (set) Token: 0x06002BD7 RID: 11223 RVA: 0x00172F82 File Offset: 0x00171382
			public Vector3 CameraOffset { get; private set; }

			// Token: 0x170006D7 RID: 1751
			// (get) Token: 0x06002BD8 RID: 11224 RVA: 0x00172F8C File Offset: 0x0017138C
			// (set) Token: 0x06002BD9 RID: 11225 RVA: 0x00172FA6 File Offset: 0x001713A6
			public float CameraZoom { get; private set; }
		}
	}
}
