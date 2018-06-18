using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F13 RID: 3859
	public static class Log
	{
		// Token: 0x17000EDC RID: 3804
		// (get) Token: 0x06005C79 RID: 23673 RVA: 0x002EE528 File Offset: 0x002EC928
		public static IEnumerable<LogMessage> Messages
		{
			get
			{
				return Log.messageQueue.Messages;
			}
		}

		// Token: 0x17000EDD RID: 3805
		// (get) Token: 0x06005C7A RID: 23674 RVA: 0x002EE548 File Offset: 0x002EC948
		private static bool ReachedMaxMessagesLimit
		{
			get
			{
				return Log.messageCount >= 1000 && !UnityData.isDebugBuild;
			}
		}

		// Token: 0x06005C7B RID: 23675 RVA: 0x002EE578 File Offset: 0x002EC978
		public static void ResetMessageCount()
		{
			bool reachedMaxMessagesLimit = Log.ReachedMaxMessagesLimit;
			Log.messageCount = 0;
			if (reachedMaxMessagesLimit)
			{
				Log.Message("Message logging is now once again on.", false);
			}
		}

		// Token: 0x06005C7C RID: 23676 RVA: 0x002EE5A3 File Offset: 0x002EC9A3
		public static void Message(string text, bool ignoreStopLoggingLimit = false)
		{
			if (ignoreStopLoggingLimit || !Log.ReachedMaxMessagesLimit)
			{
				Debug.Log(text);
				Log.messageQueue.Enqueue(new LogMessage(LogMessageType.Message, text, StackTraceUtility.ExtractStackTrace()));
				Log.PostMessage();
			}
		}

		// Token: 0x06005C7D RID: 23677 RVA: 0x002EE5DC File Offset: 0x002EC9DC
		public static void Warning(string text, bool ignoreStopLoggingLimit = false)
		{
			if (ignoreStopLoggingLimit || !Log.ReachedMaxMessagesLimit)
			{
				Debug.LogWarning(text);
				Log.messageQueue.Enqueue(new LogMessage(LogMessageType.Warning, text, StackTraceUtility.ExtractStackTrace()));
				Log.PostMessage();
			}
		}

		// Token: 0x06005C7E RID: 23678 RVA: 0x002EE618 File Offset: 0x002ECA18
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

		// Token: 0x06005C7F RID: 23679 RVA: 0x002EE6EC File Offset: 0x002ECAEC
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

		// Token: 0x06005C80 RID: 23680 RVA: 0x002EE72C File Offset: 0x002ECB2C
		internal static void Clear()
		{
			EditWindow_Log.ClearSelectedMessage();
			Log.messageQueue.Clear();
			Log.ResetMessageCount();
		}

		// Token: 0x06005C81 RID: 23681 RVA: 0x002EE743 File Offset: 0x002ECB43
		public static void TryOpenLogWindow()
		{
			if (StaticConstructorOnStartupUtility.coreStaticAssetsLoaded || UnityData.IsInMainThread)
			{
				EditWindow_Log.TryAutoOpen();
			}
		}

		// Token: 0x06005C82 RID: 23682 RVA: 0x002EE760 File Offset: 0x002ECB60
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

		// Token: 0x04003D69 RID: 15721
		private static LogMessageQueue messageQueue = new LogMessageQueue();

		// Token: 0x04003D6A RID: 15722
		private static HashSet<int> usedKeys = new HashSet<int>();

		// Token: 0x04003D6B RID: 15723
		public static bool openOnMessage = false;

		// Token: 0x04003D6C RID: 15724
		private static bool currentlyLoggingError;

		// Token: 0x04003D6D RID: 15725
		private static int messageCount;

		// Token: 0x04003D6E RID: 15726
		private const int StopLoggingAtMessageCount = 1000;
	}
}
