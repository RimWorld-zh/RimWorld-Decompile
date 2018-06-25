using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public static class PortraitsCache
	{
		private static List<RenderTexture> renderTexturesPool = new List<RenderTexture>();

		private static List<PortraitsCache.CachedPortraitsWithParams> cachedPortraits = new List<PortraitsCache.CachedPortraitsWithParams>();

		private static List<Pawn> toRemove = new List<Pawn>();

		private static List<Pawn> toSetDirty = new List<Pawn>();

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

		public static void PortraitsCacheUpdate()
		{
			PortraitsCache.RemoveExpiredCachedPortraits();
			PortraitsCache.SetAnimatedPortraitsDirty();
		}

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

		private static void DestroyRenderTexture(RenderTexture rt)
		{
			rt.DiscardContents();
			UnityEngine.Object.Destroy(rt);
		}

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

		private static void RenderPortrait(Pawn pawn, RenderTexture renderTexture, Vector3 cameraOffset, float cameraZoom)
		{
			Find.PortraitRenderer.RenderPortrait(pawn, renderTexture, cameraOffset, cameraZoom);
		}

		private static bool IsAnimated(Pawn pawn)
		{
			return Current.ProgramState == ProgramState.Playing && pawn.Drawer.renderer.graphics.flasher.FlashingNowOrRecently;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static PortraitsCache()
		{
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct CachedPortrait
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			private RenderTexture <RenderTexture>k__BackingField;

			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			private bool <Dirty>k__BackingField;

			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			private float <LastUseTime>k__BackingField;

			private const float CacheDuration = 1f;

			public CachedPortrait(RenderTexture renderTexture, bool dirty, float lastUseTime)
			{
				this = default(PortraitsCache.CachedPortrait);
				this.RenderTexture = renderTexture;
				this.Dirty = dirty;
				this.LastUseTime = lastUseTime;
			}

			public RenderTexture RenderTexture
			{
				[CompilerGenerated]
				get
				{
					return this.<RenderTexture>k__BackingField;
				}
				[CompilerGenerated]
				private set
				{
					this.<RenderTexture>k__BackingField = value;
				}
			}

			public bool Dirty
			{
				[CompilerGenerated]
				get
				{
					return this.<Dirty>k__BackingField;
				}
				[CompilerGenerated]
				private set
				{
					this.<Dirty>k__BackingField = value;
				}
			}

			public float LastUseTime
			{
				[CompilerGenerated]
				get
				{
					return this.<LastUseTime>k__BackingField;
				}
				[CompilerGenerated]
				private set
				{
					this.<LastUseTime>k__BackingField = value;
				}
			}

			public bool Expired
			{
				get
				{
					return Time.time - this.LastUseTime > 1f;
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct CachedPortraitsWithParams
		{
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			private Dictionary<Pawn, PortraitsCache.CachedPortrait> <CachedPortraits>k__BackingField;

			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			private Vector2 <Size>k__BackingField;

			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			private Vector3 <CameraOffset>k__BackingField;

			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			[CompilerGenerated]
			private float <CameraZoom>k__BackingField;

			public CachedPortraitsWithParams(Vector2 size, Vector3 cameraOffset, float cameraZoom)
			{
				this = default(PortraitsCache.CachedPortraitsWithParams);
				this.CachedPortraits = new Dictionary<Pawn, PortraitsCache.CachedPortrait>();
				this.Size = size;
				this.CameraOffset = cameraOffset;
				this.CameraZoom = cameraZoom;
			}

			public Dictionary<Pawn, PortraitsCache.CachedPortrait> CachedPortraits
			{
				[CompilerGenerated]
				get
				{
					return this.<CachedPortraits>k__BackingField;
				}
				[CompilerGenerated]
				private set
				{
					this.<CachedPortraits>k__BackingField = value;
				}
			}

			public Vector2 Size
			{
				[CompilerGenerated]
				get
				{
					return this.<Size>k__BackingField;
				}
				[CompilerGenerated]
				private set
				{
					this.<Size>k__BackingField = value;
				}
			}

			public Vector3 CameraOffset
			{
				[CompilerGenerated]
				get
				{
					return this.<CameraOffset>k__BackingField;
				}
				[CompilerGenerated]
				private set
				{
					this.<CameraOffset>k__BackingField = value;
				}
			}

			public float CameraZoom
			{
				[CompilerGenerated]
				get
				{
					return this.<CameraZoom>k__BackingField;
				}
				[CompilerGenerated]
				private set
				{
					this.<CameraZoom>k__BackingField = value;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <NewRenderTexture>c__AnonStorey0
		{
			internal Vector2 size;

			public <NewRenderTexture>c__AnonStorey0()
			{
			}

			internal bool <>m__0(RenderTexture x)
			{
				return x.width == (int)this.size.x && x.height == (int)this.size.y;
			}
		}
	}
}
