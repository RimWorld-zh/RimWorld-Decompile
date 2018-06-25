using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.Sound
{
	public static class MouseoverSounds
	{
		private static List<MouseoverSounds.MouseoverRegionCall> frameCalls = new List<MouseoverSounds.MouseoverRegionCall>();

		private static int lastUsedCallInd = -1;

		private static MouseoverSounds.MouseoverRegionCall lastUsedCall;

		private static int forceSilenceUntilFrame = -1;

		public static void SilenceForNextFrame()
		{
			MouseoverSounds.forceSilenceUntilFrame = Time.frameCount + 1;
		}

		public static void DoRegion(Rect rect)
		{
			MouseoverSounds.DoRegion(rect, SoundDefOf.Mouseover_Standard);
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static MouseoverSounds()
		{
		}

		private struct MouseoverRegionCall
		{
			public bool mouseIsOver;

			public Rect rect;

			public SoundDef sound;

			public bool IsValid
			{
				get
				{
					return this.rect.x >= 0f;
				}
			}

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

			public bool Matches(MouseoverSounds.MouseoverRegionCall other)
			{
				return this.rect.Equals(other.rect);
			}

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
