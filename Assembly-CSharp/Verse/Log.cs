using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F13 RID: 3859
	public static class Log
	{
		// Token: 0x17000EE0 RID: 3808
		// (get) Token: 0x06005CA1 RID: 23713 RVA: 0x002F0554 File Offset: 0x002EE954
		public static IEnumerable<LogMessage> Messages
		{
			get
			{
				return Log.messageQueue.Messages;
			}
		}

		// Token: 0x17000EE1 RID: 3809
		// (get) Token: 0x06005CA2 RID: 23714 RVA: 0x002F0574 File Offset: 0x002EE974
		private static bool ReachedMaxMessagesLimit
		{
			get
			{
				return Log.messageCount >= 1000 && !UnityData.isDebugBuild;
			}
		}

		// Token: 0x06005CA3 RID: 23715 RVA: 0x002F05A4 File Offset: 0x002EE9A4
		public static void ResetMessageCount()
		{
			bool reachedMaxMessagesLimit = Log.ReachedMaxMessagesLimit;
			Log.messageCount = 0;
			if (reachedMaxMessagesLimit)
			{
				Log.Message("Message logging is now once again on.", false);
			}
		}

		// Token: 0x06005CA4 RID: 23716 RVA: 0x002F05CF File Offset: 0x002EE9CF
		public static void Message(string text, bool ignoreStopLoggingLimit = false)
		{
			if (ignoreStopLoggingLimit || !Log.ReachedMaxMessagesLimit)
			{
				Debug.Log(text);
				Log.messageQueue.Enqueue(new LogMessage(LogMessageType.Message, text, StackTraceUtility.ExtractStackTrace()));
				Log.PostMessage();
			}
		}

		// Token: 0x06005CA5 RID: 23717 RVA: 0x002F0608 File Offset: 0x002EEA08
		public static void Warning(string text, bool ignoreStopLoggingLimit = false)
		{
			if (ignoreStopLoggingLimit || !Log.ReachedMaxMessagesLimit)
			{
				Debug.LogWarning(text);
				Log.messageQueue.Enqueue(new LogMessage(LogMessageType.Warning, text, StackTraceUtility.ExtractStackTrace()));
				Log.PostMessage();
			}
		}

		// Token: 0x06005CA6 RID: 23718 RVA: 0x002F0644 File Offset: 0x002EEA44
		public static void Error(string text, bool ignoreStopLoggingLimit = false)
		{
			if (ignoreStopLoggingLimit || !Log.ReachedMaxMessagesLimit)
			{
				Debug.LogError(text);
				if (!Log.currentlyLoggingError)
				{
					Log.currentlyLoggingError = true;
					try
					{
						if (Prefs.PauseOnError && Current.ProgramState == ProgramState.Playing)
						{
							Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
						}
						Log.messageQueue.Enqueue(new LogMessage(LogMessageType.Error, text, StackTraceUtility.ExtractStackTrace()));
						Log.PostMessage();
						if (!PlayDataLoader.Loaded || Prefs.DevMode)
						{
							Log.TryOpenLogWindow();
						}
					}
					catch (Exception arg)
					{
						Debug.LogError("An error occurred while logging an error: " + arg);
					}
					finally
					{
						Log.currentlyLoggingError = false;
					}
				}
			}
		}

		// Token: 0x06005CA7 RID: 23719 RVA: 0x002F0718 File Offset: 0x002EEB18
		public static void ErrorOnce(string text, int key, bool ignoreStopLoggingLimit = false)
		{
			if (ignoreStopLoggingLimit || !Log.ReachedMaxMessagesLimit)
			{
				if (!Log.usedKeys.Contains(key))
				{
					Log.usedKeys.Add(key);
					Log.Error(text, ignoreStopLoggingLimit);
				}
			}
		}

		// Token: 0x06005CA8 RID: 23720 RVA: 0x002F0758 File Offset: 0x002EEB58
		internal static void Clear()
		{
			EditWindow_Log.ClearSelectedMessage();
			Log.messageQueue.Clear();
			Log.ResetMessageCount();
		}

		// Token: 0x06005CA9 RID: 23721 RVA: 0x002F076F File Offset: 0x002EEB6F
		public static void TryOpenLogWindow()
		{
			if (StaticConstructorOnStartupUtility.coreStaticAssetsLoaded || UnityData.IsInMainThread)
			{
				EditWindow_Log.TryAutoOpen();
			}
		}

		// Token: 0x06005CAA RID: 23722 RVA: 0x002F078C File Offset: 0x002EEB8C
		private static void PostMessage()
		{
			if (Log.openOnMessage)
			{
				Log.TryOpenLogWindow();
				EditWindow_Log.SelectLastMessage(true);
			}
			Log.messageCount++;
			if (Log.messageCount == 1000 && Log.ReachedMaxMessagesLimit)
			{
				Log.Warning("Reached max messages limit. Stopping logging to avoid spam.", true);
			}
		}

		// Token: 0x04003D7B RID: 15739
		private static LogMessageQueue messageQueue = new LogMessageQueue();

		// Token: 0x04003D7C RID: 15740
		private static HashSet<int> usedKeys = new HashSet<int>();

		// Token: 0x04003D7D RID: 15741
		public static bool openOnMessage = false;

		// Token: 0x04003D7E RID: 15742
		private static bool currentlyLoggingError;

		// Token: 0x04003D7F RID: 15743
		private static int messageCount;

		// Token: 0x04003D80 RID: 15744
		private const int StopLoggingAtMessageCount = 1000;
	}
}
