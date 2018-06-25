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
	// Token: 0x02000BD8 RID: 3032
	public static class LongEventHandler
	{
		// Token: 0x04002D34 RID: 11572
		private static Queue<LongEventHandler.QueuedLongEvent> eventQueue = new Queue<LongEventHandler.QueuedLongEvent>();

		// Token: 0x04002D35 RID: 11573
		private static LongEventHandler.QueuedLongEvent currentEvent = null;

		// Token: 0x04002D36 RID: 11574
		private static Thread eventThread = null;

		// Token: 0x04002D37 RID: 11575
		private static AsyncOperation levelLoadOp = null;

		// Token: 0x04002D38 RID: 11576
		private static List<Action> toExecuteWhenFinished = new List<Action>();

		// Token: 0x04002D39 RID: 11577
		private static bool executingToExecuteWhenFinished = false;

		// Token: 0x04002D3A RID: 11578
		private static readonly object CurrentEventTextLock = new object();

		// Token: 0x04002D3B RID: 11579
		private static readonly Vector2 GUIRectSize = new Vector2(240f, 75f);

		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x0600421E RID: 16926 RVA: 0x0022D5F8 File Offset: 0x0022B9F8
		public static bool ShouldWaitForEvent
		{
			get
			{
				return LongEventHandler.AnyEventNowOrWaiting && ((LongEventHandler.currentEvent != null && !LongEventHandler.currentEvent.UseStandardWindow) || (Find.UIRoot == null || Find.WindowStack == null));
			}
		}

		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x0600421F RID: 16927 RVA: 0x0022D65C File Offset: 0x0022BA5C
		public static bool CanApplyUIScaleNow
		{
			get
			{
				LongEventHandler.QueuedLongEvent queuedLongEvent = LongEventHandler.currentEvent;
				return queuedLongEvent == null || queuedLongEvent.levelToLoad.NullOrEmpty();
			}
		}

		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x06004220 RID: 16928 RVA: 0x0022D68C File Offset: 0x0022BA8C
		public static bool AnyEventNowOrWaiting
		{
			get
			{
				return LongEventHandler.currentEvent != null || LongEventHandler.eventQueue.Count > 0;
			}
		}

		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x06004221 RID: 16929 RVA: 0x0022D6BC File Offset: 0x0022BABC
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

		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x06004222 RID: 16930 RVA: 0x0022D718 File Offset: 0x0022BB18
		public static bool ForcePause
		{
			get
			{
				return LongEventHandler.AnyEventNowOrWaiting;
			}
		}

		// Token: 0x06004223 RID: 16931 RVA: 0x0022D734 File Offset: 0x0022BB34
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

		// Token: 0x06004224 RID: 16932 RVA: 0x0022D780 File Offset: 0x0022BB80
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

		// Token: 0x06004225 RID: 16933 RVA: 0x0022D7D0 File Offset: 0x0022BBD0
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

		// Token: 0x06004226 RID: 16934 RVA: 0x0022D821 File Offset: 0x0022BC21
		public static void ClearQueuedEvents()
		{
			LongEventHandler.eventQueue.Clear();
		}

		// Token: 0x06004227 RID: 16935 RVA: 0x0022D830 File Offset: 0x0022BC30
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

		// Token: 0x06004228 RID: 16936 RVA: 0x0022D9A8 File Offset: 0x0022BDA8
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

		// Token: 0x06004229 RID: 16937 RVA: 0x0022DA66 File Offset: 0x0022BE66
		public static void ExecuteWhenFinished(Action action)
		{
			LongEventHandler.toExecuteWhenFinished.Add(action);
			if ((LongEventHandler.currentEvent == null || LongEventHandler.currentEvent.ShouldWaitUntilDisplayed) && !LongEventHandler.executingToExecuteWhenFinished)
			{
				LongEventHandler.ExecuteToExecuteWhenFinished();
			}
		}

		// Token: 0x0600422A RID: 16938 RVA: 0x0022DA9C File Offset: 0x0022BE9C
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

		// Token: 0x0600422B RID: 16939 RVA: 0x0022DAF0 File Offset: 0x0022BEF0
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

		// Token: 0x0600422C RID: 16940 RVA: 0x0022DC00 File Offset: 0x0022C000
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

		// Token: 0x0600422D RID: 16941 RVA: 0x0022DCDC File Offset: 0x0022C0DC
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

		// Token: 0x0600422E RID: 16942 RVA: 0x0022DDCC File Offset: 0x0022C1CC
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

		// Token: 0x0600422F RID: 16943 RVA: 0x0022DE70 File Offset: 0x0022C270
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

		// Token: 0x06004230 RID: 16944 RVA: 0x0022DF0C File Offset: 0x0022C30C
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

		// Token: 0x02000BD9 RID: 3033
		private class QueuedLongEvent
		{
			// Token: 0x04002D3E RID: 11582
			public Action eventAction = null;

			// Token: 0x04002D3F RID: 11583
			public IEnumerator eventActionEnumerator = null;

			// Token: 0x04002D40 RID: 11584
			public string levelToLoad = null;

			// Token: 0x04002D41 RID: 11585
			public string eventTextKey = "";

			// Token: 0x04002D42 RID: 11586
			public string eventText = "";

			// Token: 0x04002D43 RID: 11587
			public bool doAsynchronously = false;

			// Token: 0x04002D44 RID: 11588
			public Action<Exception> exceptionHandler = null;

			// Token: 0x04002D45 RID: 11589
			public bool alreadyDisplayed = false;

			// Token: 0x04002D46 RID: 11590
			public bool canEverUseStandardWindow = true;

			// Token: 0x17000A61 RID: 2657
			// (get) Token: 0x06004235 RID: 16949 RVA: 0x0022E148 File Offset: 0x0022C548
			public bool UseAnimatedDots
			{
				get
				{
					return this.doAsynchronously || this.eventActionEnumerator != null;
				}
			}

			// Token: 0x17000A62 RID: 2658
			// (get) Token: 0x06004236 RID: 16950 RVA: 0x0022E178 File Offset: 0x0022C578
			public bool ShouldWaitUntilDisplayed
			{
				get
				{
					return !this.alreadyDisplayed && this.UseStandardWindow && !this.eventText.NullOrEmpty();
				}
			}

			// Token: 0x17000A63 RID: 2659
			// (get) Token: 0x06004237 RID: 16951 RVA: 0x0022E1B4 File Offset: 0x0022C5B4
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
