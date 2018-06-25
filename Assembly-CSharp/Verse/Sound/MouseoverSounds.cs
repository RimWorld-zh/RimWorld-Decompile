using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DAE RID: 3502
	public static class MouseoverSounds
	{
		// Token: 0x04003420 RID: 13344
		private static List<MouseoverSounds.MouseoverRegionCall> frameCalls = new List<MouseoverSounds.MouseoverRegionCall>();

		// Token: 0x04003421 RID: 13345
		private static int lastUsedCallInd = -1;

		// Token: 0x04003422 RID: 13346
		private static MouseoverSounds.MouseoverRegionCall lastUsedCall;

		// Token: 0x04003423 RID: 13347
		private static int forceSilenceUntilFrame = -1;

		// Token: 0x06004E38 RID: 20024 RVA: 0x0028EE9B File Offset: 0x0028D29B
		public static void SilenceForNextFrame()
		{
			MouseoverSounds.forceSilenceUntilFrame = Time.frameCount + 1;
		}

		// Token: 0x06004E39 RID: 20025 RVA: 0x0028EEAA File Offset: 0x0028D2AA
		public static void DoRegion(Rect rect)
		{
			MouseoverSounds.DoRegion(rect, SoundDefOf.Mouseover_Standard);
		}

		// Token: 0x06004E3A RID: 20026 RVA: 0x0028EEB8 File Offset: 0x0028D2B8
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

		// Token: 0x06004E3B RID: 20027 RVA: 0x0028EF18 File Offset: 0x0028D318
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

		// Token: 0x02000DAF RID: 3503
		private struct MouseoverRegionCall
		{
			// Token: 0x04003424 RID: 13348
			public bool mouseIsOver;

			// Token: 0x04003425 RID: 13349
			public Rect rect;

			// Token: 0x04003426 RID: 13350
			public SoundDef sound;

			// Token: 0x17000C8E RID: 3214
			// (get) Token: 0x06004E3D RID: 20029 RVA: 0x0028F008 File Offset: 0x0028D408
			public bool IsValid
			{
				get
				{
					return this.rect.x >= 0f;
				}
			}

			// Token: 0x17000C8F RID: 3215
			// (get) Token: 0x06004E3E RID: 20030 RVA: 0x0028F034 File Offset: 0x0028D434
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

			// Token: 0x06004E3F RID: 20031 RVA: 0x0028F074 File Offset: 0x0028D474
			public bool Matches(MouseoverSounds.MouseoverRegionCall other)
			{
				return this.rect.Equals(other.rect);
			}

			// Token: 0x06004E40 RID: 20032 RVA: 0x0028F0A8 File Offset: 0x0028D4A8
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
