using System;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E4C RID: 3660
	[StaticConstructorOnStartup]
	public class EditWindow_Log : EditWindow
	{
		// Token: 0x0600562D RID: 22061 RVA: 0x002C6057 File Offset: 0x002C4457
		public EditWindow_Log()
		{
			this.optionalTitle = "Debug log";
		}

		// Token: 0x17000D7E RID: 3454
		// (get) Token: 0x0600562E RID: 22062 RVA: 0x002C6074 File Offset: 0x002C4474
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth / 2f, (float)UI.screenHeight / 2f);
			}
		}

		// Token: 0x17000D7F RID: 3455
		// (get) Token: 0x0600562F RID: 22063 RVA: 0x002C60A8 File Offset: 0x002C44A8
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D80 RID: 3456
		// (get) Token: 0x06005630 RID: 22064 RVA: 0x002C60C0 File Offset: 0x002C44C0
		// (set) Token: 0x06005631 RID: 22065 RVA: 0x002C60DA File Offset: 0x002C44DA
		private static LogMessage SelectedMessage
		{
			get
			{
				return EditWindow_Log.selectedMessage;
			}
			set
			{
				if (EditWindow_Log.selectedMessage != value)
				{
					EditWindow_Log.selectedMessage = value;
					if (UnityData.IsInMainThread && GUI.GetNameOfFocusedControl() == EditWindow_Log.MessageDetailsControlName)
					{
						UI.UnfocusCurrentControl();
					}
				}
			}
		}

		// Token: 0x06005632 RID: 22066 RVA: 0x002C6116 File Offset: 0x002C4516
		public static void TryAutoOpen()
		{
			if (EditWindow_Log.canAutoOpen)
			{
				EditWindow_Log.wantsToOpen = true;
			}
		}

		// Token: 0x06005633 RID: 22067 RVA: 0x002C6129 File Offset: 0x002C4529
		public static void ClearSelectedMessage()
		{
			EditWindow_Log.SelectedMessage = null;
			EditWindow_Log.detailsScrollPosition = Vector2.zero;
		}

		// Token: 0x06005634 RID: 22068 RVA: 0x002C613C File Offset: 0x002C453C
		public static void SelectLastMessage(bool expandDetailsPane = false)
		{
			EditWindow_Log.ClearSelectedMessage();
			EditWindow_Log.SelectedMessage = Log.Messages.LastOrDefault<LogMessage>();
			EditWindow_Log.messagesScrollPosition.y = (float)Log.Messages.Count<LogMessage>() * 30f;
			if (expandDetailsPane)
			{
				EditWindow_Log.detailsPaneHeight = 9999f;
			}
		}

		// Token: 0x06005635 RID: 22069 RVA: 0x002C6189 File Offset: 0x002C4589
		public static void ClearAll()
		{
			EditWindow_Log.ClearSelectedMessage();
			EditWindow_Log.messagesScrollPosition = Vector2.zero;
		}

		// Token: 0x06005636 RID: 22070 RVA: 0x002C619B File Offset: 0x002C459B
		public override void PostClose()
		{
			base.PostClose();
			EditWindow_Log.wantsToOpen = false;
		}

		// Token: 0x06005637 RID: 22071 RVA: 0x002C61AC File Offset: 0x002C45AC
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Tiny;
			WidgetRow widgetRow = new WidgetRow(0f, 0f, UIDirection.RightThenUp, 99999f, 4f);
			if (widgetRow.ButtonText("Clear", "Clear all log messages.", true, false))
			{
				Log.Clear();
				EditWindow_Log.ClearAll();
			}
			if (widgetRow.ButtonText("Trace big", "Set the stack trace to be large on screen.", true, false))
			{
				EditWindow_Log.detailsPaneHeight = 700f;
			}
			if (widgetRow.ButtonText("Trace medium", "Set the stack trace to be medium-sized on screen.", true, false))
			{
				EditWindow_Log.detailsPaneHeight = 300f;
			}
			if (widgetRow.ButtonText("Trace small", "Set the stack trace to be small on screen.", true, false))
			{
				EditWindow_Log.detailsPaneHeight = 100f;
			}
			if (EditWindow_Log.canAutoOpen)
			{
				if (widgetRow.ButtonText("Auto-open is ON", "", true, false))
				{
					EditWindow_Log.canAutoOpen = false;
				}
			}
			else if (widgetRow.ButtonText("Auto-open is OFF", "", true, false))
			{
				EditWindow_Log.canAutoOpen = true;
			}
			if (widgetRow.ButtonText("Copy to clipboard", "Copy all messages to the clipboard.", true, false))
			{
				this.CopyAllMessagesToClipboard();
			}
			Text.Font = GameFont.Small;
			Rect rect = new Rect(inRect);
			rect.yMin += 26f;
			rect.yMax = inRect.height;
			if (EditWindow_Log.selectedMessage != null)
			{
				rect.yMax -= EditWindow_Log.detailsPaneHeight;
			}
			Rect detailsRect = new Rect(inRect);
			detailsRect.yMin = rect.yMax;
			this.DoMessagesListing(rect);
			this.DoMessageDetails(detailsRect, inRect);
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Mouse.IsOver(rect))
			{
				EditWindow_Log.ClearSelectedMessage();
			}
			EditWindow_Log.detailsPaneHeight = Mathf.Max(EditWindow_Log.detailsPaneHeight, 10f);
			EditWindow_Log.detailsPaneHeight = Mathf.Min(EditWindow_Log.detailsPaneHeight, inRect.height - 80f);
		}

		// Token: 0x06005638 RID: 22072 RVA: 0x002C639C File Offset: 0x002C479C
		public static void Notify_MessageDequeued(LogMessage oldMessage)
		{
			if (EditWindow_Log.SelectedMessage == oldMessage)
			{
				EditWindow_Log.SelectedMessage = null;
			}
		}

		// Token: 0x06005639 RID: 22073 RVA: 0x002C63B0 File Offset: 0x002C47B0
		private void DoMessagesListing(Rect listingRect)
		{
			Rect viewRect = new Rect(0f, 0f, listingRect.width - 16f, this.listingViewHeight + 100f);
			Widgets.BeginScrollView(listingRect, ref EditWindow_Log.messagesScrollPosition, viewRect, true);
			float width = viewRect.width - 28f;
			Text.Font = GameFont.Tiny;
			float num = 0f;
			bool flag = false;
			foreach (LogMessage logMessage in Log.Messages)
			{
				float num2 = Text.CalcHeight(logMessage.text, width);
				if (num2 > 30f)
				{
					num2 = 30f;
				}
				GUI.color = new Color(1f, 1f, 1f, 0.7f);
				Rect rect = new Rect(4f, num, 28f, num2);
				Widgets.Label(rect, logMessage.repeats.ToStringCached());
				Rect rect2 = new Rect(28f, num, width, num2);
				if (EditWindow_Log.selectedMessage == logMessage)
				{
					GUI.DrawTexture(rect2, EditWindow_Log.SelectedMessageTex);
				}
				else if (flag)
				{
					GUI.DrawTexture(rect2, EditWindow_Log.AltMessageTex);
				}
				if (Widgets.ButtonInvisible(rect2, false))
				{
					EditWindow_Log.ClearSelectedMessage();
					EditWindow_Log.SelectedMessage = logMessage;
				}
				GUI.color = logMessage.Color;
				Widgets.Label(rect2, logMessage.text);
				num += num2;
				flag = !flag;
			}
			if (Event.current.type == EventType.Layout)
			{
				this.listingViewHeight = num;
			}
			Widgets.EndScrollView();
			GUI.color = Color.white;
		}

		// Token: 0x0600563A RID: 22074 RVA: 0x002C6578 File Offset: 0x002C4978
		private void DoMessageDetails(Rect detailsRect, Rect outRect)
		{
			if (EditWindow_Log.selectedMessage != null)
			{
				Rect rect = detailsRect;
				rect.height = 7f;
				Rect rect2 = detailsRect;
				rect2.yMin = rect.yMax;
				GUI.DrawTexture(rect, EditWindow_Log.StackTraceBorderTex);
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
				}
				if (Event.current.type == EventType.MouseDown && Mouse.IsOver(rect))
				{
					this.borderDragging = true;
					Event.current.Use();
				}
				if (this.borderDragging)
				{
					EditWindow_Log.detailsPaneHeight = outRect.height + Mathf.Round(3.5f) - Event.current.mousePosition.y;
				}
				if (Event.current.rawType == EventType.MouseUp)
				{
					this.borderDragging = false;
				}
				GUI.DrawTexture(rect2, EditWindow_Log.StackTraceAreaTex);
				string text = EditWindow_Log.selectedMessage.text + "\n" + EditWindow_Log.selectedMessage.StackTrace;
				GUI.SetNextControlName(EditWindow_Log.MessageDetailsControlName);
				Widgets.TextAreaScrollable(rect2, text, ref EditWindow_Log.detailsScrollPosition, true);
			}
		}

		// Token: 0x0600563B RID: 22075 RVA: 0x002C668C File Offset: 0x002C4A8C
		private void CopyAllMessagesToClipboard()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (LogMessage logMessage in Log.Messages)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendLine(logMessage.text);
				stringBuilder.Append(logMessage.StackTrace);
				if (stringBuilder[stringBuilder.Length - 1] != '\n')
				{
					stringBuilder.AppendLine();
				}
			}
			GUIUtility.systemCopyBuffer = stringBuilder.ToString();
		}

		// Token: 0x040038EE RID: 14574
		private static LogMessage selectedMessage = null;

		// Token: 0x040038EF RID: 14575
		private static Vector2 messagesScrollPosition;

		// Token: 0x040038F0 RID: 14576
		private static Vector2 detailsScrollPosition;

		// Token: 0x040038F1 RID: 14577
		private static float detailsPaneHeight = 100f;

		// Token: 0x040038F2 RID: 14578
		private static bool canAutoOpen = true;

		// Token: 0x040038F3 RID: 14579
		public static bool wantsToOpen = false;

		// Token: 0x040038F4 RID: 14580
		private float listingViewHeight;

		// Token: 0x040038F5 RID: 14581
		private bool borderDragging = false;

		// Token: 0x040038F6 RID: 14582
		private const float CountWidth = 28f;

		// Token: 0x040038F7 RID: 14583
		private const float Yinc = 25f;

		// Token: 0x040038F8 RID: 14584
		private const float DetailsPaneBorderHeight = 7f;

		// Token: 0x040038F9 RID: 14585
		private const float DetailsPaneMinHeight = 10f;

		// Token: 0x040038FA RID: 14586
		private const float ListingMinHeight = 80f;

		// Token: 0x040038FB RID: 14587
		private const float TopAreaHeight = 26f;

		// Token: 0x040038FC RID: 14588
		private const float MessageMaxHeight = 30f;

		// Token: 0x040038FD RID: 14589
		private static readonly Texture2D AltMessageTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.17f, 0.17f, 0.17f, 0.85f));

		// Token: 0x040038FE RID: 14590
		private static readonly Texture2D SelectedMessageTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.25f, 0.25f, 0.17f, 0.85f));

		// Token: 0x040038FF RID: 14591
		private static readonly Texture2D StackTraceAreaTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.1f, 0.1f, 0.1f, 0.5f));

		// Token: 0x04003900 RID: 14592
		private static readonly Texture2D StackTraceBorderTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.3f, 0.3f, 0.3f, 1f));

		// Token: 0x04003901 RID: 14593
		private static readonly string MessageDetailsControlName = "MessageDetailsTextArea";
	}
}
