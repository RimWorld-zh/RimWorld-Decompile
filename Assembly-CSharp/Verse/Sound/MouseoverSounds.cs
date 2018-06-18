using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DAF RID: 3503
	public static class MouseoverSounds
	{
		// Token: 0x06004E1F RID: 19999 RVA: 0x0028D7BF File Offset: 0x0028BBBF
		public static void SilenceForNextFrame()
		{
			MouseoverSounds.forceSilenceUntilFrame = Time.frameCount + 1;
		}

		// Token: 0x06004E20 RID: 20000 RVA: 0x0028D7CE File Offset: 0x0028BBCE
		public static void DoRegion(Rect rect)
		{
			MouseoverSounds.DoRegion(rect, SoundDefOf.Mouseover_Standard);
		}

		// Token: 0x06004E21 RID: 20001 RVA: 0x0028D7DC File Offset: 0x0028BBDC
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

		// Token: 0x06004E22 RID: 20002 RVA: 0x0028D83C File Offset: 0x0028BC3C
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

		// Token: 0x04003415 RID: 13333
		private static List<MouseoverSounds.MouseoverRegionCall> frameCalls = new List<MouseoverSounds.MouseoverRegionCall>();

		// Token: 0x04003416 RID: 13334
		private static int lastUsedCallInd = -1;

		// Token: 0x04003417 RID: 13335
		private static MouseoverSounds.MouseoverRegionCall lastUsedCall;

		// Token: 0x04003418 RID: 13336
		private static int forceSilenceUntilFrame = -1;

		// Token: 0x02000DB0 RID: 3504
		private struct MouseoverRegionCall
		{
			// Token: 0x17000C8D RID: 3213
			// (get) Token: 0x06004E24 RID: 20004 RVA: 0x0028D92C File Offset: 0x0028BD2C
			public bool IsValid
			{
				get
				{
					return this.rect.x >= 0f;
				}
			}

			// Token: 0x17000C8E RID: 3214
			// (get) Token: 0x06004E25 RID: 20005 RVA: 0x0028D958 File Offset: 0x0028BD58
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

			// Token: 0x06004E26 RID: 20006 RVA: 0x0028D998 File Offset: 0x0028BD98
			public bool Matches(MouseoverSounds.MouseoverRegionCall other)
			{
				return this.rect.Equals(other.rect);
			}

			// Token: 0x06004E27 RID: 20007 RVA: 0x0028D9CC File Offset: 0x0028BDCC
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

			// Token: 0x04003419 RID: 13337
			public bool mouseIsOver;

			// Token: 0x0400341A RID: 13338
			public Rect rect;

			// Token: 0x0400341B RID: 13339
			public SoundDef sound;
		}
	}
}
