using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F17 RID: 3863
	public static class Log
	{
		// Token: 0x04003D7E RID: 15742
		private static LogMessageQueue messageQueue = new LogMessageQueue();

		// Token: 0x04003D7F RID: 15743
		private static HashSet<int> usedKeys = new HashSet<int>();

		// Token: 0x04003D80 RID: 15744
		public static bool openOnMessage = false;

		// Token: 0x04003D81 RID: 15745
		private static bool currentlyLoggingError;

		// Token: 0x04003D82 RID: 15746
		private static int messageCount;

		// Token: 0x04003D83 RID: 15747
		private const int StopLoggingAtMessageCount = 1000;

		// Token: 0x17000EDF RID: 3807
		// (get) Token: 0x06005CAB RID: 23723 RVA: 0x002F0BD4 File Offset: 0x002EEFD4
		public static IEnumerable<LogMessage> Messages
		{
			get
			{
				return Log.messageQueue.Messages;
			}
		}

		// Token: 0x17000EE0 RID: 3808
		// (get) Token: 0x06005CAC RID: 23724 RVA: 0x002F0BF4 File Offset: 0x002EEFF4
		private static bool ReachedMaxMessagesLimit
		{
			get
			{
				return Log.messageCount >= 1000 && !UnityData.isDebugBuild;
			}
		}

		// Token: 0x06005CAD RID: 23725 RVA: 0x002F0C24 File Offset: 0x002EF024
		public static void ResetMessageCount()
		{
			bool reachedMaxMessagesLimit = Log.ReachedMaxMessagesLimit;
			Log.messageCount = 0;
			if (reachedMaxMessagesLimit)
			{
				Log.Message("Message logging is now once again on.", false);
			}
		}

		// Token: 0x06005CAE RID: 23726 RVA: 0x002F0C4F File Offset: 0x002EF04F
		public static void Message(string text, bool ignoreStopLoggingLimit = false)
		{
			if (ignoreStopLoggingLimit || !Log.ReachedMaxMessagesLimit)
			{
				Debug.Log(text);
				Log.messageQueue.Enqueue(new LogMessage(LogMessageType.Message, text, StackTraceUtility.ExtractStackTrace()));
				Log.PostMessage();
			}
		}

		// Token: 0x06005CAF RID: 23727 RVA: 0x002F0C88 File Offset: 0x002EF088
		public static void Warning(string text, bool ignoreStopLoggingLimit = false)
		{
			if (ignoreStopLoggingLimit || !Log.ReachedMaxMessagesLimit)
			{
				Debug.LogWarning(text);
				Log.messageQueue.Enqueue(new LogMessage(LogMessageType.Warning, text, StackTraceUtility.ExtractStackTrace()));
				Log.PostMessage();
			}
		}

		// Token: 0x06005CB0 RID: 23728 RVA: 0x002F0CC4 File Offset: 0x002EF0C4
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

		// Token: 0x06005CB1 RID: 23729 RVA: 0x002F0D98 File Offset: 0x002EF198
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

		// Token: 0x06005CB2 RID: 23730 RVA: 0x002F0DD8 File Offset: 0x002EF1D8
		internal static void Clear()
		{
			EditWindow_Log.ClearSelectedMessage();
			Log.messageQueue.Clear();
			Log.ResetMessageCount();
		}

		// Token: 0x06005CB3 RID: 23731 RVA: 0x002F0DEF File Offset: 0x002EF1EF
		public static void TryOpenLogWindow()
		{
			if (StaticConstructorOnStartupUtility.coreStaticAssetsLoaded || UnityData.IsInMainThread)
			{
				EditWindow_Log.TryAutoOpen();
			}
		}

		// Token: 0x06005CB4 RID: 23732 RVA: 0x002F0E0C File Offset: 0x002EF20C
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
	}
}
