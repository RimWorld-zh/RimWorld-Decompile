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
	// Token: 0x02000BD5 RID: 3029
	public static class LongEventHandler
	{
		// Token: 0x04002D2D RID: 11565
		private static Queue<LongEventHandler.QueuedLongEvent> eventQueue = new Queue<LongEventHandler.QueuedLongEvent>();

		// Token: 0x04002D2E RID: 11566
		private static LongEventHandler.QueuedLongEvent currentEvent = null;

		// Token: 0x04002D2F RID: 11567
		private static Thread eventThread = null;

		// Token: 0x04002D30 RID: 11568
		private static AsyncOperation levelLoadOp = null;

		// Token: 0x04002D31 RID: 11569
		private static List<Action> toExecuteWhenFinished = new List<Action>();

		// Token: 0x04002D32 RID: 11570
		private static bool executingToExecuteWhenFinished = false;

		// Token: 0x04002D33 RID: 11571
		private static readonly object CurrentEventTextLock = new object();

		// Token: 0x04002D34 RID: 11572
		private static readonly Vector2 GUIRectSize = new Vector2(240f, 75f);

		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x0600421B RID: 16923 RVA: 0x0022D23C File Offset: 0x0022B63C
		public static bool ShouldWaitForEvent
		{
			get
			{
				return LongEventHandler.AnyEventNowOrWaiting && ((LongEventHandler.currentEvent != null && !LongEventHandler.currentEvent.UseStandardWindow) || (Find.UIRoot == null || Find.WindowStack == null));
			}
		}

		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x0600421C RID: 16924 RVA: 0x0022D2A0 File Offset: 0x0022B6A0
		public static bool CanApplyUIScaleNow
		{
			get
			{
				LongEventHandler.QueuedLongEvent queuedLongEvent = LongEventHandler.currentEvent;
				return queuedLongEvent == null || queuedLongEvent.levelToLoad.NullOrEmpty();
			}
		}

		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x0600421D RID: 16925 RVA: 0x0022D2D0 File Offset: 0x0022B6D0
		public static bool AnyEventNowOrWaiting
		{
			get
			{
				return LongEventHandler.currentEvent != null || LongEventHandler.eventQueue.Count > 0;
			}
		}

		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x0600421E RID: 16926 RVA: 0x0022D300 File Offset: 0x0022B700
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

		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x0600421F RID: 16927 RVA: 0x0022D35C File Offset: 0x0022B75C
		public static bool ForcePause
		{
			get
			{
				return LongEventHandler.AnyEventNowOrWaiting;
			}
		}

		// Token: 0x06004220 RID: 16928 RVA: 0x0022D378 File Offset: 0x0022B778
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

		// Token: 0x06004221 RID: 16929 RVA: 0x0022D3C4 File Offset: 0x0022B7C4
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

		// Token: 0x06004222 RID: 16930 RVA: 0x0022D414 File Offset: 0x0022B814
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

		// Token: 0x06004223 RID: 16931 RVA: 0x0022D465 File Offset: 0x0022B865
		public static void ClearQueuedEvents()
		{
			LongEventHandler.eventQueue.Clear();
		}

		// Token: 0x06004224 RID: 16932 RVA: 0x0022D474 File Offset: 0x0022B874
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

		// Token: 0x06004225 RID: 16933 RVA: 0x0022D5EC File Offset: 0x0022B9EC
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

		// Token: 0x06004226 RID: 16934 RVA: 0x0022D6AA File Offset: 0x0022BAAA
		public static void ExecuteWhenFinished(Action action)
		{
			LongEventHandler.toExecuteWhenFinished.Add(action);
			if ((LongEventHandler.currentEvent == null || LongEventHandler.currentEvent.ShouldWaitUntilDisplayed) && !LongEventHandler.executingToExecuteWhenFinished)
			{
				LongEventHandler.ExecuteToExecuteWhenFinished();
			}
		}

		// Token: 0x06004227 RID: 16935 RVA: 0x0022D6E0 File Offset: 0x0022BAE0
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

		// Token: 0x06004228 RID: 16936 RVA: 0x0022D734 File Offset: 0x0022BB34
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

		// Token: 0x06004229 RID: 16937 RVA: 0x0022D844 File Offset: 0x0022BC44
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

		// Token: 0x0600422A RID: 16938 RVA: 0x0022D920 File Offset: 0x0022BD20
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

		// Token: 0x0600422B RID: 16939 RVA: 0x0022DA10 File Offset: 0x0022BE10
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

		// Token: 0x0600422C RID: 16940 RVA: 0x0022DAB4 File Offset: 0x0022BEB4
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

		// Token: 0x0600422D RID: 16941 RVA: 0x0022DB50 File Offset: 0x0022BF50
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

		// Token: 0x02000BD6 RID: 3030
		private class QueuedLongEvent
		{
			// Token: 0x04002D37 RID: 11575
			public Action eventAction = null;

			// Token: 0x04002D38 RID: 11576
			public IEnumerator eventActionEnumerator = null;

			// Token: 0x04002D39 RID: 11577
			public string levelToLoad = null;

			// Token: 0x04002D3A RID: 11578
			public string eventTextKey = "";

			// Token: 0x04002D3B RID: 11579
			public string eventText = "";

			// Token: 0x04002D3C RID: 11580
			public bool doAsynchronously = false;

			// Token: 0x04002D3D RID: 11581
			public Action<Exception> exceptionHandler = null;

			// Token: 0x04002D3E RID: 11582
			public bool alreadyDisplayed = false;

			// Token: 0x04002D3F RID: 11583
			public bool canEverUseStandardWindow = true;

			// Token: 0x17000A62 RID: 2658
			// (get) Token: 0x06004232 RID: 16946 RVA: 0x0022DD8C File Offset: 0x0022C18C
			public bool UseAnimatedDots
			{
				get
				{
					return this.doAsynchronously || this.eventActionEnumerator != null;
				}
			}

			// Token: 0x17000A63 RID: 2659
			// (get) Token: 0x06004233 RID: 16947 RVA: 0x0022DDBC File Offset: 0x0022C1BC
			public bool ShouldWaitUntilDisplayed
			{
				get
				{
					return !this.alreadyDisplayed && this.UseStandardWindow && !this.eventText.NullOrEmpty();
				}
			}

			// Token: 0x17000A64 RID: 2660
			// (get) Token: 0x06004234 RID: 16948 RVA: 0x0022DDF8 File Offset: 0x0022C1F8
			public bool UseStandardWindow
			{
				get
				{
					return this.canEverUseStandardWindow && !this.doAsynchronously && this.eventActionEnumerator == null;
				}
			}
		}
	}
}
