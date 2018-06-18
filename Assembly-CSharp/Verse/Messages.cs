using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E88 RID: 3720
	[StaticConstructorOnStartup]
	public static class Messages
	{
		// Token: 0x060057B6 RID: 22454 RVA: 0x002CF7EC File Offset: 0x002CDBEC
		public static void Update()
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (Messages.mouseoverMessageIndex >= 0 && Messages.mouseoverMessageIndex < Messages.liveMessages.Count)
				{
					Messages.liveMessages[Messages.mouseoverMessageIndex].lookTargets.TryHighlight(true, true, false);
				}
			}
			Messages.mouseoverMessageIndex = -1;
			Messages.liveMessages.RemoveAll((Message m) => m.Expired);
		}

		// Token: 0x060057B7 RID: 22455 RVA: 0x002CF870 File Offset: 0x002CDC70
		public static void Message(string text, LookTargets lookTargets, MessageTypeDef def, bool historical = true)
		{
			if (Messages.AcceptsMessage(text, lookTargets))
			{
				Message msg = new Message(text, def, lookTargets);
				Messages.Message(msg, historical);
			}
		}

		// Token: 0x060057B8 RID: 22456 RVA: 0x002CF8A0 File Offset: 0x002CDCA0
		public static void Message(string text, MessageTypeDef def, bool historical = true)
		{
			if (Messages.AcceptsMessage(text, TargetInfo.Invalid))
			{
				Message msg = new Message(text, def);
				Messages.Message(msg, historical);
			}
		}

		// Token: 0x060057B9 RID: 22457 RVA: 0x002CF8D8 File Offset: 0x002CDCD8
		public static void Message(Message msg, bool historical = true)
		{
			if (Messages.AcceptsMessage(msg.text, msg.lookTargets))
			{
				if (historical && Find.Archive != null)
				{
					Find.Archive.Add(msg);
				}
				Messages.liveMessages.Add(msg);
				while (Messages.liveMessages.Count > 12)
				{
					Messages.liveMessages.RemoveAt(0);
				}
				if (msg.def.sound != null)
				{
					msg.def.sound.PlayOneShotOnCamera(null);
				}
			}
		}

		// Token: 0x060057BA RID: 22458 RVA: 0x002CF96C File Offset: 0x002CDD6C
		public static bool IsLive(Message msg)
		{
			return Messages.liveMessages.Contains(msg);
		}

		// Token: 0x060057BB RID: 22459 RVA: 0x002CF98C File Offset: 0x002CDD8C
		public static void MessagesDoGUI()
		{
			Text.Font = GameFont.Small;
			int xOffset = (int)Messages.MessagesTopLeftStandard.x;
			int num = (int)Messages.MessagesTopLeftStandard.y;
			if (Current.Game != null && Find.ActiveLesson.ActiveLessonVisible)
			{
				num += (int)Find.ActiveLesson.Current.MessagesYOffset;
			}
			for (int i = Messages.liveMessages.Count - 1; i >= 0; i--)
			{
				Messages.liveMessages[i].Draw(xOffset, num);
				num += 26;
			}
		}

		// Token: 0x060057BC RID: 22460 RVA: 0x002CFA28 File Offset: 0x002CDE28
		public static bool CollidesWithAnyMessage(Rect rect, out float messageAlpha)
		{
			bool result = false;
			float num = 0f;
			for (int i = 0; i < Messages.liveMessages.Count; i++)
			{
				Message message = Messages.liveMessages[i];
				if (rect.Overlaps(message.lastDrawRect))
				{
					result = true;
					num = Mathf.Max(num, message.Alpha);
				}
			}
			messageAlpha = num;
			return result;
		}

		// Token: 0x060057BD RID: 22461 RVA: 0x002CFA97 File Offset: 0x002CDE97
		public static void Clear()
		{
			Messages.liveMessages.Clear();
		}

		// Token: 0x060057BE RID: 22462 RVA: 0x002CFAA4 File Offset: 0x002CDEA4
		public static void Notify_LoadedLevelChanged()
		{
			for (int i = 0; i < Messages.liveMessages.Count; i++)
			{
				Messages.liveMessages[i].lookTargets = null;
			}
		}

		// Token: 0x060057BF RID: 22463 RVA: 0x002CFAE0 File Offset: 0x002CDEE0
		private static bool AcceptsMessage(string text, LookTargets lookTargets)
		{
			bool result;
			if (text.NullOrEmpty())
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < Messages.liveMessages.Count; i++)
				{
					if (Messages.liveMessages[i].text == text && Messages.liveMessages[i].startingFrame == RealTime.frameCount && LookTargets.SameTargets(Messages.liveMessages[i].lookTargets, lookTargets))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060057C0 RID: 22464 RVA: 0x002CFB7D File Offset: 0x002CDF7D
		public static void Notify_Mouseover(Message msg)
		{
			Messages.mouseoverMessageIndex = Messages.liveMessages.IndexOf(msg);
		}

		// Token: 0x04003A03 RID: 14851
		private static List<Message> liveMessages = new List<Message>();

		// Token: 0x04003A04 RID: 14852
		private static int mouseoverMessageIndex = -1;

		// Token: 0x04003A05 RID: 14853
		public static readonly Vector2 MessagesTopLeftStandard = new Vector2(140f, 16f);

		// Token: 0x04003A06 RID: 14854
		private const int MessageYInterval = 26;

		// Token: 0x04003A07 RID: 14855
		private const int MaxLiveMessages = 12;
	}
}
