using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DAC RID: 3500
	public static class MouseoverSounds
	{
		// Token: 0x06004E34 RID: 20020 RVA: 0x0028ED6F File Offset: 0x0028D16F
		public static void SilenceForNextFrame()
		{
			MouseoverSounds.forceSilenceUntilFrame = Time.frameCount + 1;
		}

		// Token: 0x06004E35 RID: 20021 RVA: 0x0028ED7E File Offset: 0x0028D17E
		public static void DoRegion(Rect rect)
		{
			MouseoverSounds.DoRegion(rect, SoundDefOf.Mouseover_Standard);
		}

		// Token: 0x06004E36 RID: 20022 RVA: 0x0028ED8C File Offset: 0x0028D18C
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

		// Token: 0x06004E37 RID: 20023 RVA: 0x0028EDEC File Offset: 0x0028D1EC
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

		// Token: 0x04003420 RID: 13344
		private static List<MouseoverSounds.MouseoverRegionCall> frameCalls = new List<MouseoverSounds.MouseoverRegionCall>();

		// Token: 0x04003421 RID: 13345
		private static int lastUsedCallInd = -1;

		// Token: 0x04003422 RID: 13346
		private static MouseoverSounds.MouseoverRegionCall lastUsedCall;

		// Token: 0x04003423 RID: 13347
		private static int forceSilenceUntilFrame = -1;

		// Token: 0x02000DAD RID: 3501
		private struct MouseoverRegionCall
		{
			// Token: 0x17000C8F RID: 3215
			// (get) Token: 0x06004E39 RID: 20025 RVA: 0x0028EEDC File Offset: 0x0028D2DC
			public bool IsValid
			{
				get
				{
					return this.rect.x >= 0f;
				}
			}

			// Token: 0x17000C90 RID: 3216
			// (get) Token: 0x06004E3A RID: 20026 RVA: 0x0028EF08 File Offset: 0x0028D308
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

			// Token: 0x06004E3B RID: 20027 RVA: 0x0028EF48 File Offset: 0x0028D348
			public bool Matches(MouseoverSounds.MouseoverRegionCall other)
			{
				return this.rect.Equals(other.rect);
			}

			// Token: 0x06004E3C RID: 20028 RVA: 0x0028EF7C File Offset: 0x0028D37C
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

			// Token: 0x04003424 RID: 13348
			public bool mouseIsOver;

			// Token: 0x04003425 RID: 13349
			public Rect rect;

			// Token: 0x04003426 RID: 13350
			public SoundDef sound;
		}
	}
}
