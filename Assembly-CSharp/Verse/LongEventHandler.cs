using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RimWorld;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Verse
{
	// Token: 0x02000BD9 RID: 3033
	public static class LongEventHandler
	{
		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x06004217 RID: 16919 RVA: 0x0022CAA8 File Offset: 0x0022AEA8
		public static bool ShouldWaitForEvent
		{
			get
			{
				return LongEventHandler.AnyEventNowOrWaiting && ((LongEventHandler.currentEvent != null && !LongEventHandler.currentEvent.UseStandardWindow) || (Find.UIRoot == null || Find.WindowStack == null));
			}
		}

		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x06004218 RID: 16920 RVA: 0x0022CB0C File Offset: 0x0022AF0C
		public static bool CanApplyUIScaleNow
		{
			get
			{
				LongEventHandler.QueuedLongEvent queuedLongEvent = LongEventHandler.currentEvent;
				return queuedLongEvent == null || queuedLongEvent.levelToLoad.NullOrEmpty();
			}
		}

		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x06004219 RID: 16921 RVA: 0x0022CB3C File Offset: 0x0022AF3C
		public static bool AnyEventNowOrWaiting
		{
			get
			{
				return LongEventHandler.currentEvent != null || LongEventHandler.eventQueue.Count > 0;
			}
		}

		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x0600421A RID: 16922 RVA: 0x0022CB6C File Offset: 0x0022AF6C
		private static bool AnyEventWhichDoesntUseStandardWindowNowOrWaiting
		{
			get
			{
				LongEventHandler.QueuedLongEvent queuedLongEvent = LongEventHandler.currentEvent;
				bool result;
				if (queuedLongEvent != null && !queuedLongEvent.UseStandardWindow)
				{
					result = true;
				}
				else
				{
					result = LongEventHandler.eventQueue.Any((LongEventHandler.QueuedLongEvent x) => !x.UseStandardWindow);
				}
				return result;
			}
		}

		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x0600421B RID: 16923 RVA: 0x0022CBC8 File Offset: 0x0022AFC8
		public static bool ForcePause
		{
			get
			{
				return LongEventHandler.AnyEventNowOrWaiting;
			}
		}

		// Token: 0x0600421C RID: 16924 RVA: 0x0022CBE4 File Offset: 0x0022AFE4
		public static void QueueLongEvent(Action action, string textKey, bool doAsynchronously, Action<Exception> exceptionHandler)
		{
			LongEventHandler.QueuedLongEvent queuedLongEvent = new LongEventHandler.QueuedLongEvent();
			queuedLongEvent.eventAction = action;
			queuedLongEvent.eventTextKey = textKey;
			queuedLongEvent.doAsynchronously = doAsynchronously;
			queuedLongEvent.exceptionHandler = exceptionHandler;
			queuedLongEvent.canEverUseStandardWindow = !LongEventHandler.AnyEventWhichDoesntUseStandardWindowNowOrWaiting;
			LongEventHandler.eventQueue.Enqueue(queuedLongEvent);
		}

		// Token: 0x0600421D RID: 16925 RVA: 0x0022CC30 File Offset: 0x0022B030
		public static void QueueLongEvent(IEnumerable action, string textKey, Action<Exception> exceptionHandler = null)
		{
			LongEventHandler.QueuedLongEvent queuedLongEvent = new LongEventHandler.QueuedLongEvent();
			queuedLongEvent.eventActionEnumerator = action.GetEnumerator();
			queuedLongEvent.eventTextKey = textKey;
			queuedLongEvent.doAsynchronously = false;
			queuedLongEvent.exceptionHandler = exceptionHandler;
			queuedLongEvent.canEverUseStandardWindow = !LongEventHandler.AnyEventWhichDoesntUseStandardWindowNowOrWaiting;
			LongEventHandler.eventQueue.Enqueue(queuedLongEvent);
		}

		// Token: 0x0600421E RID: 16926 RVA: 0x0022CC80 File Offset: 0x0022B080
		public static void QueueLongEvent(Action preLoadLevelAction, string levelToLoad, string textKey, bool doAsynchronously, Action<Exception> exceptionHandler)
		{
			LongEventHandler.QueuedLongEvent queuedLongEvent = new LongEventHandler.QueuedLongEvent();
			queuedLongEvent.eventAction = preLoadLevelAction;
			queuedLongEvent.levelToLoad = levelToLoad;
			queuedLongEvent.eventTextKey = textKey;
			queuedLongEvent.doAsynchronously = doAsynchronously;
			queuedLongEvent.exceptionHandler = exceptionHandler;
			queuedLongEvent.canEverUseStandardWindow = !LongEventHandler.AnyEventWhichDoesntUseStandardWindowNowOrWaiting;
			LongEventHandler.eventQueue.Enqueue(queuedLongEvent);
		}

		// Token: 0x0600421F RID: 16927 RVA: 0x0022CCD1 File Offset: 0x0022B0D1
		public static void ClearQueuedEvents()
		{
			LongEventHandler.eventQueue.Clear();
		}

		// Token: 0x06004220 RID: 16928 RVA: 0x0022CCE0 File Offset: 0x0022B0E0
		public static void LongEventsOnGUI()
		{
			if (LongEventHandler.currentEvent != null)
			{
				float num = LongEventHandler.GUIRectSize.x;
				object currentEventTextLock = LongEventHandler.CurrentEventTextLock;
				lock (currentEventTextLock)
				{
					Text.Font = GameFont.Small;
					num = Mathf.Max(num, Text.CalcSize(LongEventHandler.currentEvent.eventText + "...").x + 40f);
				}
				Rect rect = new Rect(((float)UI.screenWidth - num) / 2f, ((float)UI.screenHeight - LongEventHandler.GUIRectSize.y) / 2f, num, LongEventHandler.GUIRectSize.y);
				rect = rect.Rounded();
				if (!LongEventHandler.currentEvent.UseStandardWindow || Find.UIRoot == null || Find.WindowStack == null)
				{
					if (UIMenuBackgroundManager.background == null)
					{
						UIMenuBackgroundManager.background = new UI_BackgroundMain();
					}
					UIMenuBackgroundManager.background.BackgroundOnGUI();
					Widgets.DrawShadowAround(rect);
					Widgets.DrawWindowBackground(rect);
					LongEventHandler.DrawLongEventWindowContents(rect);
				}
				else
				{
					Find.WindowStack.ImmediateWindow(62893994, rect, WindowLayer.Super, delegate
					{
						LongEventHandler.DrawLongEventWindowContents(rect.AtZero());
					}, true, false, 1f);
				}
			}
		}

		// Token: 0x06004221 RID: 16929 RVA: 0x0022CE58 File Offset: 0x0022B258
		public static void LongEventsUpdate(out bool sceneChanged)
		{
			sceneChanged = false;
			if (LongEventHandler.currentEvent != null)
			{
				if (LongEventHandler.currentEvent.eventActionEnumerator != null)
				{
					LongEventHandler.UpdateCurrentEnumeratorEvent();
				}
				else if (LongEventHandler.currentEvent.doAsynchronously)
				{
					LongEventHandler.UpdateCurrentAsynchronousEvent();
				}
				else
				{
					LongEventHandler.UpdateCurrentSynchronousEvent(out sceneChanged);
				}
			}
			if (LongEventHandler.currentEvent == null)
			{
				if (LongEventHandler.eventQueue.Count > 0)
				{
					LongEventHandler.currentEvent = LongEventHandler.eventQueue.Dequeue();
					if (LongEventHandler.currentEvent.eventTextKey == null)
					{
						LongEventHandler.currentEvent.eventText = "";
					}
					else
					{
						LongEventHandler.currentEvent.eventText = LongEventHandler.currentEvent.eventTextKey.Translate();
					}
				}
			}
		}

		// Token: 0x06004222 RID: 16930 RVA: 0x0022CF16 File Offset: 0x0022B316
		public static void ExecuteWhenFinished(Action action)
		{
			LongEventHandler.toExecuteWhenFinished.Add(action);
			if ((LongEventHandler.currentEvent == null || LongEventHandler.currentEvent.ShouldWaitUntilDisplayed) && !LongEventHandler.executingToExecuteWhenFinished)
			{
				LongEventHandler.ExecuteToExecuteWhenFinished();
			}
		}

		// Token: 0x06004223 RID: 16931 RVA: 0x0022CF4C File Offset: 0x0022B34C
		public static void SetCurrentEventText(string newText)
		{
			object currentEventTextLock = LongEventHandler.CurrentEventTextLock;
			lock (currentEventTextLock)
			{
				if (LongEventHandler.currentEvent != null)
				{
					LongEventHandler.currentEvent.eventText = newText;
				}
			}
		}

		// Token: 0x06004224 RID: 16932 RVA: 0x0022CFA0 File Offset: 0x0022B3A0
		private static void UpdateCurrentEnumeratorEvent()
		{
			try
			{
				float num = Time.realtimeSinceStartup + 0.1f;
				while (LongEventHandler.currentEvent.eventActionEnumerator.MoveNext())
				{
					if (num <= Time.realtimeSinceStartup)
					{
						return;
					}
				}
				IDisposable disposable = LongEventHandler.currentEvent.eventActionEnumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
				LongEventHandler.currentEvent = null;
				LongEventHandler.eventThread = null;
				LongEventHandler.levelLoadOp = null;
				LongEventHandler.ExecuteToExecuteWhenFinished();
			}
			catch (Exception ex)
			{
				Log.Error("Exception from long event: " + ex, false);
				if (LongEventHandler.currentEvent != null)
				{
					IDisposable disposable2 = LongEventHandler.currentEvent.eventActionEnumerator as IDisposable;
					if (disposable2 != null)
					{
						disposable2.Dispose();
					}
					if (LongEventHandler.currentEvent.exceptionHandler != null)
					{
						LongEventHandler.currentEvent.exceptionHandler(ex);
					}
				}
				LongEventHandler.currentEvent = null;
				LongEventHandler.eventThread = null;
				LongEventHandler.levelLoadOp = null;
			}
		}

		// Token: 0x06004225 RID: 16933 RVA: 0x0022D0B0 File Offset: 0x0022B4B0
		private static void UpdateCurrentAsynchronousEvent()
		{
			if (LongEventHandler.eventThread == null)
			{
				LongEventHandler.eventThread = new Thread(delegate()
				{
					LongEventHandler.RunEventFromAnotherThread(LongEventHandler.currentEvent.eventAction);
				});
				LongEventHandler.eventThread.Start();
			}
			else if (!LongEventHandler.eventThread.IsAlive)
			{
				bool flag = false;
				if (!LongEventHandler.currentEvent.levelToLoad.NullOrEmpty())
				{
					if (LongEventHandler.levelLoadOp == null)
					{
						LongEventHandler.levelLoadOp = SceneManager.LoadSceneAsync(LongEventHandler.currentEvent.levelToLoad);
					}
					else if (LongEventHandler.levelLoadOp.isDone)
					{
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					LongEventHandler.currentEvent = null;
					LongEventHandler.eventThread = null;
					LongEventHandler.levelLoadOp = null;
					LongEventHandler.ExecuteToExecuteWhenFinished();
				}
			}
		}

		// Token: 0x06004226 RID: 16934 RVA: 0x0022D18C File Offset: 0x0022B58C
		private static void UpdateCurrentSynchronousEvent(out bool sceneChanged)
		{
			sceneChanged = false;
			if (!LongEventHandler.currentEvent.ShouldWaitUntilDisplayed)
			{
				try
				{
					if (LongEventHandler.currentEvent.eventAction != null)
					{
						LongEventHandler.currentEvent.eventAction();
					}
					if (!LongEventHandler.currentEvent.levelToLoad.NullOrEmpty())
					{
						SceneManager.LoadScene(LongEventHandler.currentEvent.levelToLoad);
						sceneChanged = true;
					}
					LongEventHandler.currentEvent = null;
					LongEventHandler.eventThread = null;
					LongEventHandler.levelLoadOp = null;
					LongEventHandler.ExecuteToExecuteWhenFinished();
				}
				catch (Exception ex)
				{
					Log.Error("Exception from long event: " + ex, false);
					if (LongEventHandler.currentEvent != null && LongEventHandler.currentEvent.exceptionHandler != null)
					{
						LongEventHandler.currentEvent.exceptionHandler(ex);
					}
					LongEventHandler.currentEvent = null;
					LongEventHandler.eventThread = null;
					LongEventHandler.levelLoadOp = null;
				}
			}
		}

		// Token: 0x06004227 RID: 16935 RVA: 0x0022D27C File Offset: 0x0022B67C
		private static void RunEventFromAnotherThread(Action action)
		{
			CultureInfoUtility.EnsureEnglish();
			try
			{
				if (action != null)
				{
					action();
				}
			}
			catch (Exception ex)
			{
				Log.Error("Exception from asynchronous event: " + ex, false);
				try
				{
					if (LongEventHandler.currentEvent != null && LongEventHandler.currentEvent.exceptionHandler != null)
					{
						LongEventHandler.currentEvent.exceptionHandler(ex);
					}
				}
				catch (Exception arg)
				{
					Log.Error("Exception was thrown while trying to handle exception. Exception: " + arg, false);
				}
			}
		}

		// Token: 0x06004228 RID: 16936 RVA: 0x0022D320 File Offset: 0x0022B720
		private static void ExecuteToExecuteWhenFinished()
		{
			if (LongEventHandler.executingToExecuteWhenFinished)
			{
				Log.Warning("Already executing.", false);
			}
			else
			{
				LongEventHandler.executingToExecuteWhenFinished = true;
				for (int i = 0; i < LongEventHandler.toExecuteWhenFinished.Count; i++)
				{
					try
					{
						LongEventHandler.toExecuteWhenFinished[i]();
					}
					catch (Exception arg)
					{
						Log.Error("Could not execute post-long-event action. Exception: " + arg, false);
					}
				}
				LongEventHandler.toExecuteWhenFinished.Clear();
				LongEventHandler.executingToExecuteWhenFinished = false;
			}
		}

		// Token: 0x06004229 RID: 16937 RVA: 0x0022D3BC File Offset: 0x0022B7BC
		private static void DrawLongEventWindowContents(Rect rect)
		{
			if (LongEventHandler.currentEvent != null)
			{
				if (Event.current.type == EventType.Repaint)
				{
					LongEventHandler.currentEvent.alreadyDisplayed = true;
				}
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.MiddleCenter;
				float num = 0f;
				if (LongEventHandler.levelLoadOp != null)
				{
					float f = 1f;
					if (!LongEventHandler.levelLoadOp.isDone)
					{
						f = LongEventHandler.levelLoadOp.progress;
					}
					string text = "LoadingAssets".Translate() + " " + f.ToStringPercent();
					num = Text.CalcSize(text).x;
					Widgets.Label(rect, text);
				}
				else
				{
					object currentEventTextLock = LongEventHandler.CurrentEventTextLock;
					lock (currentEventTextLock)
					{
						num = Text.CalcSize(LongEventHandler.currentEvent.eventText).x;
						Widgets.Label(rect, LongEventHandler.currentEvent.eventText);
					}
				}
				Text.Anchor = TextAnchor.MiddleLeft;
				rect.xMin = rect.center.x + num / 2f;
				Widgets.Label(rect, LongEventHandler.currentEvent.UseAnimatedDots ? GenText.MarchingEllipsis(0f) : "...");
				Text.Anchor = TextAnchor.UpperLeft;
			}
		}

		// Token: 0x04002D28 RID: 11560
		private static Queue<LongEventHandler.QueuedLongEvent> eventQueue = new Queue<LongEventHandler.QueuedLongEvent>();

		// Token: 0x04002D29 RID: 11561
		private static LongEventHandler.QueuedLongEvent currentEvent = null;

		// Token: 0x04002D2A RID: 11562
		private static Thread eventThread = null;

		// Token: 0x04002D2B RID: 11563
		private static AsyncOperation levelLoadOp = null;

		// Token: 0x04002D2C RID: 11564
		private static List<Action> toExecuteWhenFinished = new List<Action>();

		// Token: 0x04002D2D RID: 11565
		private static bool executingToExecuteWhenFinished = false;

		// Token: 0x04002D2E RID: 11566
		private static readonly object CurrentEventTextLock = new object();

		// Token: 0x04002D2F RID: 11567
		private static readonly Vector2 GUIRectSize = new Vector2(240f, 75f);

		// Token: 0x02000BDA RID: 3034
		private class QueuedLongEvent
		{
			// Token: 0x17000A60 RID: 2656
			// (get) Token: 0x0600422E RID: 16942 RVA: 0x0022D5F8 File Offset: 0x0022B9F8
			public bool UseAnimatedDots
			{
				get
				{
					return this.doAsynchronously || this.eventActionEnumerator != null;
				}
			}

			// Token: 0x17000A61 RID: 2657
			// (get) Token: 0x0600422F RID: 16943 RVA: 0x0022D628 File Offset: 0x0022BA28
			public bool ShouldWaitUntilDisplayed
			{
				get
				{
					return !this.alreadyDisplayed && this.UseStandardWindow && !this.eventText.NullOrEmpty();
				}
			}

			// Token: 0x17000A62 RID: 2658
			// (get) Token: 0x06004230 RID: 16944 RVA: 0x0022D664 File Offset: 0x0022BA64
			public bool UseStandardWindow
			{
				get
				{
					return this.canEverUseStandardWindow && !this.doAsynchronously && this.eventActionEnumerator == null;
				}
			}

			// Token: 0x04002D32 RID: 11570
			public Action eventAction = null;

			// Token: 0x04002D33 RID: 11571
			public IEnumerator eventActionEnumerator = null;

			// Token: 0x04002D34 RID: 11572
			public string levelToLoad = null;

			// Token: 0x04002D35 RID: 11573
			public string eventTextKey = "";

			// Token: 0x04002D36 RID: 11574
			public string eventText = "";

			// Token: 0x04002D37 RID: 11575
			public bool doAsynchronously = false;

			// Token: 0x04002D38 RID: 11576
			public Action<Exception> exceptionHandler = null;

			// Token: 0x04002D39 RID: 11577
			public bool alreadyDisplayed = false;

			// Token: 0x04002D3A RID: 11578
			public bool canEverUseStandardWindow = true;
		}
	}
}
