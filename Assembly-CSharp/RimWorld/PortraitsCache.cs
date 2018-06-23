using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007B4 RID: 1972
	[StaticConstructorOnStartup]
	public static class PortraitsCache
	{
		// Token: 0x04001784 RID: 6020
		private static List<RenderTexture> renderTexturesPool = new List<RenderTexture>();

		// Token: 0x04001785 RID: 6021
		private static List<PortraitsCache.CachedPortraitsWithParams> cachedPortraits = new List<PortraitsCache.CachedPortraitsWithParams>();

		// Token: 0x04001786 RID: 6022
		private static List<Pawn> toRemove = new List<Pawn>();

		// Token: 0x04001787 RID: 6023
		private static List<Pawn> toSetDirty = new List<Pawn>();

		// Token: 0x06002BB8 RID: 11192 RVA: 0x00172A48 File Offset: 0x00170E48
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

		// Token: 0x06002BB9 RID: 11193 RVA: 0x00172B24 File Offset: 0x00170F24
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

		// Token: 0x06002BBA RID: 11194 RVA: 0x00172BA2 File Offset: 0x00170FA2
		public static void PortraitsCacheUpdate()
		{
			PortraitsCache.RemoveExpiredCachedPortraits();
			PortraitsCache.SetAnimatedPortraitsDirty();
		}

		// Token: 0x06002BBB RID: 11195 RVA: 0x00172BB0 File Offset: 0x00170FB0
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

		// Token: 0x06002BBC RID: 11196 RVA: 0x00172C90 File Offset: 0x00171090
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

		// Token: 0x06002BBD RID: 11197 RVA: 0x00172D44 File Offset: 0x00171144
		private static void DestroyRenderTexture(RenderTexture rt)
		{
			rt.DiscardContents();
			UnityEngine.Object.Destroy(rt);
		}

		// Token: 0x06002BBE RID: 11198 RVA: 0x00172D54 File Offset: 0x00171154
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

		// Token: 0x06002BBF RID: 11199 RVA: 0x00172E64 File Offset: 0x00171264
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

		// Token: 0x06002BC0 RID: 11200 RVA: 0x00172FA4 File Offset: 0x001713A4
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

		// Token: 0x06002BC1 RID: 11201 RVA: 0x00173026 File Offset: 0x00171426
		private static void RenderPortrait(Pawn pawn, RenderTexture renderTexture, Vector3 cameraOffset, float cameraZoom)
		{
			Find.PortraitRenderer.RenderPortrait(pawn, renderTexture, cameraOffset, cameraZoom);
		}

		// Token: 0x06002BC2 RID: 11202 RVA: 0x00173038 File Offset: 0x00171438
		private static bool IsAnimated(Pawn pawn)
		{
			return Current.ProgramState == ProgramState.Playing && pawn.Drawer.renderer.graphics.flasher.FlashingNowOrRecently;
		}

		// Token: 0x020007B5 RID: 1973
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct CachedPortrait
		{
			// Token: 0x0400178B RID: 6027
			private const float CacheDuration = 1f;

			// Token: 0x06002BC4 RID: 11204 RVA: 0x001730AA File Offset: 0x001714AA
			public CachedPortrait(RenderTexture renderTexture, bool dirty, float lastUseTime)
			{
				this = default(PortraitsCache.CachedPortrait);
				this.RenderTexture = renderTexture;
				this.Dirty = dirty;
				this.LastUseTime = lastUseTime;
			}

			// Token: 0x170006D1 RID: 1745
			// (get) Token: 0x06002BC5 RID: 11205 RVA: 0x001730CC File Offset: 0x001714CC
			// (set) Token: 0x06002BC6 RID: 11206 RVA: 0x001730E6 File Offset: 0x001714E6
			public RenderTexture RenderTexture { get; private set; }

			// Token: 0x170006D2 RID: 1746
			// (get) Token: 0x06002BC7 RID: 11207 RVA: 0x001730F0 File Offset: 0x001714F0
			// (set) Token: 0x06002BC8 RID: 11208 RVA: 0x0017310A File Offset: 0x0017150A
			public bool Dirty { get; private set; }

			// Token: 0x170006D3 RID: 1747
			// (get) Token: 0x06002BC9 RID: 11209 RVA: 0x00173114 File Offset: 0x00171514
			// (set) Token: 0x06002BCA RID: 11210 RVA: 0x0017312E File Offset: 0x0017152E
			public float LastUseTime { get; private set; }

			// Token: 0x170006D4 RID: 1748
			// (get) Token: 0x06002BCB RID: 11211 RVA: 0x00173138 File Offset: 0x00171538
			public bool Expired
			{
				get
				{
					return Time.time - this.LastUseTime > 1f;
				}
			}
		}

		// Token: 0x020007B6 RID: 1974
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct CachedPortraitsWithParams
		{
			// Token: 0x06002BCC RID: 11212 RVA: 0x00173160 File Offset: 0x00171560
			public CachedPortraitsWithParams(Vector2 size, Vector3 cameraOffset, float cameraZoom)
			{
				this = default(PortraitsCache.CachedPortraitsWithParams);
				this.CachedPortraits = new Dictionary<Pawn, PortraitsCache.CachedPortrait>();
				this.Size = size;
				this.CameraOffset = cameraOffset;
				this.CameraZoom = cameraZoom;
			}

			// Token: 0x170006D5 RID: 1749
			// (get) Token: 0x06002BCD RID: 11213 RVA: 0x0017318C File Offset: 0x0017158C
			// (set) Token: 0x06002BCE RID: 11214 RVA: 0x001731A6 File Offset: 0x001715A6
			public Dictionary<Pawn, PortraitsCache.CachedPortrait> CachedPortraits { get; private set; }

			// Token: 0x170006D6 RID: 1750
			// (get) Token: 0x06002BCF RID: 11215 RVA: 0x001731B0 File Offset: 0x001715B0
			// (set) Token: 0x06002BD0 RID: 11216 RVA: 0x001731CA File Offset: 0x001715CA
			public Vector2 Size { get; private set; }

			// Token: 0x170006D7 RID: 1751
			// (get) Token: 0x06002BD1 RID: 11217 RVA: 0x001731D4 File Offset: 0x001715D4
			// (set) Token: 0x06002BD2 RID: 11218 RVA: 0x001731EE File Offset: 0x001715EE
			public Vector3 CameraOffset { get; private set; }

			// Token: 0x170006D8 RID: 1752
			// (get) Token: 0x06002BD3 RID: 11219 RVA: 0x001731F8 File Offset: 0x001715F8
			// (set) Token: 0x06002BD4 RID: 11220 RVA: 0x00173212 File Offset: 0x00171612
			public float CameraZoom { get; private set; }
		}
	}
}
