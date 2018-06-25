using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DAF RID: 3503
	public static class MouseoverSounds
	{
		// Token: 0x04003427 RID: 13351
		private static List<MouseoverSounds.MouseoverRegionCall> frameCalls = new List<MouseoverSounds.MouseoverRegionCall>();

		// Token: 0x04003428 RID: 13352
		private static int lastUsedCallInd = -1;

		// Token: 0x04003429 RID: 13353
		private static MouseoverSounds.MouseoverRegionCall lastUsedCall;

		// Token: 0x0400342A RID: 13354
		private static int forceSilenceUntilFrame = -1;

		// Token: 0x06004E38 RID: 20024 RVA: 0x0028F17B File Offset: 0x0028D57B
		public static void SilenceForNextFrame()
		{
			MouseoverSounds.forceSilenceUntilFrame = Time.frameCount + 1;
		}

		// Token: 0x06004E39 RID: 20025 RVA: 0x0028F18A File Offset: 0x0028D58A
		public static void DoRegion(Rect rect)
		{
			MouseoverSounds.DoRegion(rect, SoundDefOf.Mouseover_Standard);
		}

		// Token: 0x06004E3A RID: 20026 RVA: 0x0028F198 File Offset: 0x0028D598
		public static void DoRegion(Rect rect, SoundDef sound)
		{
			if (sound != null)
			{
				if (Event.current.type == EventType.Repaint)
				{
					MouseoverSounds.MouseoverRegionCall item = default(MouseoverSounds.MouseoverRegionCall);
					item.rect = rect;
					item.sound = sound;
					item.mouseIsOver = Mouse.IsOver(rect);
					MouseoverSounds.frameCalls.Add(item);
				}
			}
		}

		// Token: 0x06004E3B RID: 20027 RVA: 0x0028F1F8 File Offset: 0x0028D5F8
		public static void ResolveFrame()
		{
			for (int i = 0; i < MouseoverSounds.frameCalls.Count; i++)
			{
				if (MouseoverSounds.frameCalls[i].mouseIsOver)
				{
					if (MouseoverSounds.lastUsedCallInd != i && !MouseoverSounds.frameCalls[i].Matches(MouseoverSounds.lastUsedCall))
					{
						if (MouseoverSounds.forceSilenceUntilFrame < Time.frameCount)
						{
							MouseoverSounds.frameCalls[i].sound.PlayOneShotOnCamera(null);
						}
					}
					MouseoverSounds.lastUsedCallInd = i;
					MouseoverSounds.lastUsedCall = MouseoverSounds.frameCalls[i];
					MouseoverSounds.frameCalls.Clear();
					return;
				}
			}
			MouseoverSounds.lastUsedCall = MouseoverSounds.MouseoverRegionCall.Invalid;
			MouseoverSounds.lastUsedCallInd = -1;
			MouseoverSounds.frameCalls.Clear();
		}

		// Token: 0x02000DB0 RID: 3504
		private struct MouseoverRegionCall
		{
			// Token: 0x0400342B RID: 13355
			public bool mouseIsOver;

			// Token: 0x0400342C RID: 13356
			public Rect rect;

			// Token: 0x0400342D RID: 13357
			public SoundDef sound;

			// Token: 0x17000C8E RID: 3214
			// (get) Token: 0x06004E3D RID: 20029 RVA: 0x0028F2E8 File Offset: 0x0028D6E8
			public bool IsValid
			{
				get
				{
					return this.rect.x >= 0f;
				}
			}

			// Token: 0x17000C8F RID: 3215
			// (get) Token: 0x06004E3E RID: 20030 RVA: 0x0028F314 File Offset: 0x0028D714
			public static MouseoverSounds.MouseoverRegionCall Invalid
			{
				get
				{
					return new MouseoverSounds.MouseoverRegionCall
					{
						rect = new Rect(-1000f, -1000f, 0f, 0f)
					};
				}
			}

			// Token: 0x06004E3F RID: 20031 RVA: 0x0028F354 File Offset: 0x0028D754
			public bool Matches(MouseoverSounds.MouseoverRegionCall other)
			{
				return this.rect.Equals(other.rect);
			}

			// Token: 0x06004E40 RID: 20032 RVA: 0x0028F388 File Offset: 0x0028D788
			public override string ToString()
			{
				string result;
				if (!this.IsValid)
				{
					result = "(Invalid)";
				}
				else
				{
					result = string.Concat(new object[]
					{
						"(rect=",
						this.rect,
						(!this.mouseIsOver) ? "" : "mouseIsOver",
						")"
					});
				}
				return result;
			}
		}
	}
}
