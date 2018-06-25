using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E8A RID: 3722
	[StaticConstructorOnStartup]
	public static class Messages
	{
		// Token: 0x04003A1B RID: 14875
		private static List<Message> liveMessages = new List<Message>();

		// Token: 0x04003A1C RID: 14876
		private static int mouseoverMessageIndex = -1;

		// Token: 0x04003A1D RID: 14877
		public static readonly Vector2 MessagesTopLeftStandard = new Vector2(140f, 16f);

		// Token: 0x04003A1E RID: 14878
		private const int MessageYInterval = 26;

		// Token: 0x04003A1F RID: 14879
		private const int MaxLiveMessages = 12;

		// Token: 0x060057DA RID: 22490 RVA: 0x002D1714 File Offset: 0x002CFB14
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

		// Token: 0x060057DB RID: 22491 RVA: 0x002D1798 File Offset: 0x002CFB98
		public static void Message(string text, LookTargets lookTargets, MessageTypeDef def, bool historical = true)
		{
			if (Messages.AcceptsMessage(text, lookTargets))
			{
				Message msg = new Message(text, def, lookTargets);
				Messages.Message(msg, historical);
			}
		}

		// Token: 0x060057DC RID: 22492 RVA: 0x002D17C8 File Offset: 0x002CFBC8
		public static void Message(string text, MessageTypeDef def, bool historical = true)
		{
			if (Messages.AcceptsMessage(text, TargetInfo.Invalid))
			{
				Message msg = new Message(text, def);
				Messages.Message(msg, historical);
			}
		}

		// Token: 0x060057DD RID: 22493 RVA: 0x002D1800 File Offset: 0x002CFC00
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

		// Token: 0x060057DE RID: 22494 RVA: 0x002D1894 File Offset: 0x002CFC94
		public static bool IsLive(Message msg)
		{
			return Messages.liveMessages.Contains(msg);
		}

		// Token: 0x060057DF RID: 22495 RVA: 0x002D18B4 File Offset: 0x002CFCB4
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

		// Token: 0x060057E0 RID: 22496 RVA: 0x002D1950 File Offset: 0x002CFD50
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

		// Token: 0x060057E1 RID: 22497 RVA: 0x002D19BF File Offset: 0x002CFDBF
		public static void Clear()
		{
			Messages.liveMessages.Clear();
		}

		// Token: 0x060057E2 RID: 22498 RVA: 0x002D19CC File Offset: 0x002CFDCC
		public static void Notify_LoadedLevelChanged()
		{
			for (int i = 0; i < Messages.liveMessages.Count; i++)
			{
				Messages.liveMessages[i].lookTargets = null;
			}
		}

		// Token: 0x060057E3 RID: 22499 RVA: 0x002D1A08 File Offset: 0x002CFE08
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

		// Token: 0x060057E4 RID: 22500 RVA: 0x002D1AA5 File Offset: 0x002CFEA5
		public static void Notify_Mouseover(Message msg)
		{
			Messages.mouseoverMessageIndex = Messages.liveMessages.IndexOf(msg);
		}
	}
}
