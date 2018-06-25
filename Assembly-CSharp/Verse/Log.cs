using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F18 RID: 3864
	public static class Log
	{
		// Token: 0x04003D86 RID: 15750
		private static LogMessageQueue messageQueue = new LogMessageQueue();

		// Token: 0x04003D87 RID: 15751
		private static HashSet<int> usedKeys = new HashSet<int>();

		// Token: 0x04003D88 RID: 15752
		public static bool openOnMessage = false;

		// Token: 0x04003D89 RID: 15753
		private static bool currentlyLoggingError;

		// Token: 0x04003D8A RID: 15754
		private static int messageCount;

		// Token: 0x04003D8B RID: 15755
		private const int StopLoggingAtMessageCount = 1000;

		// Token: 0x17000EDF RID: 3807
		// (get) Token: 0x06005CAB RID: 23723 RVA: 0x002F0DF4 File Offset: 0x002EF1F4
		public static IEnumerable<LogMessage> Messages
		{
			get
			{
				return Log.messageQueue.Messages;
			}
		}

		// Token: 0x17000EE0 RID: 3808
		// (get) Token: 0x06005CAC RID: 23724 RVA: 0x002F0E14 File Offset: 0x002EF214
		private static bool ReachedMaxMessagesLimit
		{
			get
			{
				return Log.messageCount >= 1000 && !UnityData.isDebugBuild;
			}
		}

		// Token: 0x06005CAD RID: 23725 RVA: 0x002F0E44 File Offset: 0x002EF244
		public static void ResetMessageCount()
		{
			bool reachedMaxMessagesLimit = Log.ReachedMaxMessagesLimit;
			Log.messageCount = 0;
			if (reachedMaxMessagesLimit)
			{
				Log.Message("Message logging is now once again on.", false);
			}
		}

		// Token: 0x06005CAE RID: 23726 RVA: 0x002F0E6F File Offset: 0x002EF26F
		public static void Message(string text, bool ignoreStopLoggingLimit = false)
		{
			if (ignoreStopLoggingLimit || !Log.ReachedMaxMessagesLimit)
			{
				Debug.Log(text);
				Log.messageQueue.Enqueue(new LogMessage(LogMessageType.Message, text, StackTraceUtility.ExtractStackTrace()));
				Log.PostMessage();
			}
		}

		// Token: 0x06005CAF RID: 23727 RVA: 0x002F0EA8 File Offset: 0x002EF2A8
		public static void Warning(string text, bool ignoreStopLoggingLimit = false)
		{
			if (ignoreStopLoggingLimit || !Log.ReachedMaxMessagesLimit)
			{
				Debug.LogWarning(text);
				Log.messageQueue.Enqueue(new LogMessage(LogMessageType.Warning, text, StackTraceUtility.ExtractStackTrace()));
				Log.PostMessage();
			}
		}

		// Token: 0x06005CB0 RID: 23728 RVA: 0x002F0EE4 File Offset: 0x002EF2E4
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

		// Token: 0x06005CB1 RID: 23729 RVA: 0x002F0FB8 File Offset: 0x002EF3B8
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

		// Token: 0x06005CB2 RID: 23730 RVA: 0x002F0FF8 File Offset: 0x002EF3F8
		internal static void Clear()
		{
			EditWindow_Log.ClearSelectedMessage();
			Log.messageQueue.Clear();
			Log.ResetMessageCount();
		}

		// Token: 0x06005CB3 RID: 23731 RVA: 0x002F100F File Offset: 0x002EF40F
		public static void TryOpenLogWindow()
		{
			if (StaticConstructorOnStartupUtility.coreStaticAssetsLoaded || UnityData.IsInMainThread)
			{
				EditWindow_Log.TryAutoOpen();
			}
		}

		// Token: 0x06005CB4 RID: 23732 RVA: 0x002F102C File Offset: 0x002EF42C
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
