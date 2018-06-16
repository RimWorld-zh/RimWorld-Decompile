using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DB0 RID: 3504
	public static class MouseoverSounds
	{
		// Token: 0x06004E21 RID: 20001 RVA: 0x0028D7DF File Offset: 0x0028BBDF
		public static void SilenceForNextFrame()
		{
			MouseoverSounds.forceSilenceUntilFrame = Time.frameCount + 1;
		}

		// Token: 0x06004E22 RID: 20002 RVA: 0x0028D7EE File Offset: 0x0028BBEE
		public static void DoRegion(Rect rect)
		{
			MouseoverSounds.DoRegion(rect, SoundDefOf.Mouseover_Standard);
		}

		// Token: 0x06004E23 RID: 20003 RVA: 0x0028D7FC File Offset: 0x0028BBFC
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

		// Token: 0x06004E24 RID: 20004 RVA: 0x0028D85C File Offset: 0x0028BC5C
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

		// Token: 0x04003417 RID: 13335
		private static List<MouseoverSounds.MouseoverRegionCall> frameCalls = new List<MouseoverSounds.MouseoverRegionCall>();

		// Token: 0x04003418 RID: 13336
		private static int lastUsedCallInd = -1;

		// Token: 0x04003419 RID: 13337
		private static MouseoverSounds.MouseoverRegionCall lastUsedCall;

		// Token: 0x0400341A RID: 13338
		private static int forceSilenceUntilFrame = -1;

		// Token: 0x02000DB1 RID: 3505
		private struct MouseoverRegionCall
		{
			// Token: 0x17000C8E RID: 3214
			// (get) Token: 0x06004E26 RID: 20006 RVA: 0x0028D94C File Offset: 0x0028BD4C
			public bool IsValid
			{
				get
				{
					return this.rect.x >= 0f;
				}
			}

			// Token: 0x17000C8F RID: 3215
			// (get) Token: 0x06004E27 RID: 20007 RVA: 0x0028D978 File Offset: 0x0028BD78
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

			// Token: 0x06004E28 RID: 20008 RVA: 0x0028D9B8 File Offset: 0x0028BDB8
			public bool Matches(MouseoverSounds.MouseoverRegionCall other)
			{
				return this.rect.Equals(other.rect);
			}

			// Token: 0x06004E29 RID: 20009 RVA: 0x0028D9EC File Offset: 0x0028BDEC
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

			// Token: 0x0400341B RID: 13339
			public bool mouseIsOver;

			// Token: 0x0400341C RID: 13340
			public Rect rect;

			// Token: 0x0400341D RID: 13341
			public SoundDef sound;
		}
	}
}
