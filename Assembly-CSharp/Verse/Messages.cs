using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	[StaticConstructorOnStartup]
	public static class Messages
	{
		private class LiveMessage
		{
			private const float DefaultMessageLifespan = 13f;

			private const float FadeoutDuration = 0.6f;

			private int ID;

			public string text;

			private float startingTime;

			public int startingFrame;

			public GlobalTargetInfo lookTarget;

			private Vector2 cachedSize = new Vector2(-1f, -1f);

			public Rect lastDrawRect;

			private static int uniqueID;

			protected float Age
			{
				get
				{
					return RealTime.LastRealTime - this.startingTime;
				}
			}

			protected float TimeLeft
			{
				get
				{
					return (float)(13.0 - this.Age);
				}
			}

			public bool Expired
			{
				get
				{
					return this.TimeLeft <= 0.0;
				}
			}

			public float Alpha
			{
				get
				{
					if (this.TimeLeft < 0.60000002384185791)
					{
						return (float)(this.TimeLeft / 0.60000002384185791);
					}
					return 1f;
				}
			}

			public LiveMessage(string text)
			{
				this.text = text;
				this.lookTarget = GlobalTargetInfo.Invalid;
				this.startingFrame = RealTime.frameCount;
				this.startingTime = RealTime.LastRealTime;
				this.ID = LiveMessage.uniqueID++;
			}

			public LiveMessage(string text, GlobalTargetInfo lookTarget) : this(text)
			{
				this.lookTarget = lookTarget;
			}

			public Rect CalculateRect(float x, float y)
			{
				Text.Font = GameFont.Small;
				if (this.cachedSize.x < 0.0)
				{
					this.cachedSize = Text.CalcSize(this.text);
				}
				this.lastDrawRect = new Rect(x, y, this.cachedSize.x, this.cachedSize.y);
				this.lastDrawRect = this.lastDrawRect.ContractedBy(-2f);
				return this.lastDrawRect;
			}

			public void Draw(int xOffset, int yOffset)
			{
				Rect rect = this.CalculateRect((float)xOffset, (float)yOffset);
				Find.WindowStack.ImmediateWindow(Gen.HashCombineInt(this.ID, 45574281), rect, WindowLayer.Super, (Action)delegate
				{
					Text.Font = GameFont.Small;
					Text.Anchor = TextAnchor.MiddleLeft;
					Rect rect2 = rect.AtZero();
					float alpha = this.Alpha;
					GUI.color = new Color(1f, 1f, 1f, alpha);
					if (Messages.ShouldDrawMessageBackground)
					{
						GUI.color = new Color(0.15f, 0.15f, 0.15f, (float)(0.800000011920929 * alpha));
						GUI.DrawTexture(rect2, BaseContent.WhiteTex);
						GUI.color = new Color(1f, 1f, 1f, alpha);
					}
					if (this.lookTarget.IsValid)
					{
						UIHighlighter.HighlightOpportunity(rect2, "Messages");
						Widgets.DrawHighlightIfMouseover(rect2);
					}
					Rect rect3 = new Rect(2f, 0f, (float)(rect2.width - 2.0), rect2.height);
					Widgets.Label(rect3, this.text);
					if (Current.ProgramState == ProgramState.Playing && this.lookTarget.IsValid && Widgets.ButtonInvisible(rect2, false))
					{
						CameraJumper.TryJumpAndSelect(this.lookTarget);
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.ClickingMessages, KnowledgeAmount.Total);
					}
					Text.Anchor = TextAnchor.UpperLeft;
					GUI.color = Color.white;
					if (Mouse.IsOver(rect2))
					{
						Messages.mouseoverMessageIndex = Messages.liveMessages.IndexOf(this);
					}
				}, false, false, 0f);
			}
		}

		private const int MessageYInterval = 26;

		private const int MaxLiveMessages = 12;

		private static List<LiveMessage> liveMessages = new List<LiveMessage>();

		private static int mouseoverMessageIndex = -1;

		public static readonly Vector2 MessagesTopLeftStandard = new Vector2(140f, 16f);

		private static bool ShouldDrawMessageBackground
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing)
				{
					return true;
				}
				WindowStack windowStack = Find.WindowStack;
				for (int i = 0; i < windowStack.Count; i++)
				{
					if (windowStack[i].CausesMessageBackground())
					{
						return true;
					}
				}
				return false;
			}
		}

		public static void Update()
		{
			if (Current.ProgramState == ProgramState.Playing && Messages.mouseoverMessageIndex >= 0 && Messages.liveMessages.Count >= Messages.mouseoverMessageIndex + 1)
			{
				GlobalTargetInfo lookTarget = Messages.liveMessages[Messages.mouseoverMessageIndex].lookTarget;
				if (lookTarget.IsValid && lookTarget.IsMapTarget && lookTarget.Map == Find.VisibleMap)
				{
					GenDraw.DrawArrowPointingAt(((TargetInfo)lookTarget).CenterVector3, false);
				}
			}
			Messages.mouseoverMessageIndex = -1;
			Messages.liveMessages.RemoveAll((Predicate<LiveMessage>)((LiveMessage m) => m.Expired));
		}

		public static void Message(string text, GlobalTargetInfo lookTarget, MessageSound sound)
		{
			if (Messages.AcceptsMessage(text, lookTarget))
			{
				LiveMessage msg = new LiveMessage(text, lookTarget);
				Messages.Message(msg, sound);
			}
		}

		public static void Message(string text, MessageSound sound)
		{
			if (Messages.AcceptsMessage(text, TargetInfo.Invalid))
			{
				LiveMessage msg = new LiveMessage(text);
				Messages.Message(msg, sound);
			}
		}

		public static void MessagesDoGUI()
		{
			Text.Font = GameFont.Small;
			Vector2 messagesTopLeftStandard = Messages.MessagesTopLeftStandard;
			int xOffset = (int)messagesTopLeftStandard.x;
			Vector2 messagesTopLeftStandard2 = Messages.MessagesTopLeftStandard;
			int num = (int)messagesTopLeftStandard2.y;
			if (Current.Game != null && Find.ActiveLesson.ActiveLessonVisible)
			{
				num += (int)Find.ActiveLesson.Current.MessagesYOffset;
			}
			for (int num2 = Messages.liveMessages.Count - 1; num2 >= 0; num2--)
			{
				Messages.liveMessages[num2].Draw(xOffset, num);
				num += 26;
			}
		}

		public static bool CollidesWithAnyMessage(Rect rect, out float messageAlpha)
		{
			bool result = false;
			float num = 0f;
			for (int i = 0; i < Messages.liveMessages.Count; i++)
			{
				LiveMessage liveMessage = Messages.liveMessages[i];
				if (rect.Overlaps(liveMessage.lastDrawRect))
				{
					result = true;
					num = Mathf.Max(num, liveMessage.Alpha);
				}
			}
			messageAlpha = num;
			return result;
		}

		public static void Clear()
		{
			Messages.liveMessages.Clear();
		}

		public static void Notify_LoadedLevelChanged()
		{
			for (int i = 0; i < Messages.liveMessages.Count; i++)
			{
				Messages.liveMessages[i].lookTarget = GlobalTargetInfo.Invalid;
			}
		}

		private static bool AcceptsMessage(string text, GlobalTargetInfo lookTarget)
		{
			if (text.NullOrEmpty())
			{
				return false;
			}
			for (int i = 0; i < Messages.liveMessages.Count; i++)
			{
				if (Messages.liveMessages[i].text == text && Messages.liveMessages[i].lookTarget == lookTarget && Messages.liveMessages[i].startingFrame == RealTime.frameCount)
				{
					return false;
				}
			}
			return true;
		}

		private static void Message(LiveMessage msg, MessageSound sound)
		{
			Messages.liveMessages.Add(msg);
			while (Messages.liveMessages.Count > 12)
			{
				Messages.liveMessages.RemoveAt(0);
			}
			if (sound != 0)
			{
				SoundDef soundDef = null;
				switch (sound)
				{
				case MessageSound.Standard:
				{
					soundDef = SoundDefOf.MessageAlert;
					break;
				}
				case MessageSound.RejectInput:
				{
					soundDef = SoundDefOf.ClickReject;
					break;
				}
				case MessageSound.Benefit:
				{
					soundDef = SoundDefOf.MessageBenefit;
					break;
				}
				case MessageSound.Negative:
				{
					soundDef = SoundDefOf.MessageAlertNegative;
					break;
				}
				case MessageSound.SeriousAlert:
				{
					soundDef = SoundDefOf.MessageSeriousAlert;
					break;
				}
				}
				soundDef.PlayOneShotOnCamera(null);
			}
		}
	}
}
