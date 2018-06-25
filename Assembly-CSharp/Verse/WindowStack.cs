using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000ECF RID: 3791
	public class WindowStack
	{
		// Token: 0x04003BFB RID: 15355
		public Window currentlyDrawnWindow;

		// Token: 0x04003BFC RID: 15356
		private List<Window> windows = new List<Window>();

		// Token: 0x04003BFD RID: 15357
		private List<int> immediateWindowsRequests = new List<int>();

		// Token: 0x04003BFE RID: 15358
		private bool updateInternalWindowsOrderLater;

		// Token: 0x04003BFF RID: 15359
		private Window focusedWindow;

		// Token: 0x04003C00 RID: 15360
		private static int uniqueWindowID;

		// Token: 0x04003C01 RID: 15361
		private bool gameStartDialogOpen;

		// Token: 0x04003C02 RID: 15362
		private float timeGameStartDialogClosed = -1f;

		// Token: 0x04003C03 RID: 15363
		private IntVec2 prevResolution = new IntVec2(UI.screenWidth, UI.screenHeight);

		// Token: 0x04003C04 RID: 15364
		private List<Window> windowStackOnGUITmpList = new List<Window>();

		// Token: 0x04003C05 RID: 15365
		private List<Window> updateImmediateWindowsListTmpList = new List<Window>();

		// Token: 0x04003C06 RID: 15366
		private List<Window> removeWindowsOfTypeTmpList = new List<Window>();

		// Token: 0x04003C07 RID: 15367
		private List<Window> closeWindowsTmpList = new List<Window>();

		// Token: 0x17000E1B RID: 3611
		// (get) Token: 0x060059A0 RID: 22944 RVA: 0x002DE474 File Offset: 0x002DC874
		public int Count
		{
			get
			{
				return this.windows.Count;
			}
		}

		// Token: 0x17000E1C RID: 3612
		public Window this[int index]
		{
			get
			{
				return this.windows[index];
			}
		}

		// Token: 0x17000E1D RID: 3613
		// (get) Token: 0x060059A2 RID: 22946 RVA: 0x002DE4B8 File Offset: 0x002DC8B8
		public IList<Window> Windows
		{
			get
			{
				return this.windows.AsReadOnly();
			}
		}

		// Token: 0x17000E1E RID: 3614
		// (get) Token: 0x060059A3 RID: 22947 RVA: 0x002DE4D8 File Offset: 0x002DC8D8
		public FloatMenu FloatMenu
		{
			get
			{
				return this.WindowOfType<FloatMenu>();
			}
		}

		// Token: 0x17000E1F RID: 3615
		// (get) Token: 0x060059A4 RID: 22948 RVA: 0x002DE4F4 File Offset: 0x002DC8F4
		public bool WindowsForcePause
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (this.windows[i].forcePause)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000E20 RID: 3616
		// (get) Token: 0x060059A5 RID: 22949 RVA: 0x002DE548 File Offset: 0x002DC948
		public bool WindowsPreventCameraMotion
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (this.windows[i].preventCameraMotion)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000E21 RID: 3617
		// (get) Token: 0x060059A6 RID: 22950 RVA: 0x002DE59C File Offset: 0x002DC99C
		public bool WindowsPreventDrawTutor
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (this.windows[i].preventDrawTutor)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000E22 RID: 3618
		// (get) Token: 0x060059A7 RID: 22951 RVA: 0x002DE5F0 File Offset: 0x002DC9F0
		public float SecondsSinceClosedGameStartDialog
		{
			get
			{
				float result;
				if (this.gameStartDialogOpen)
				{
					result = 0f;
				}
				else if (this.timeGameStartDialogClosed < 0f)
				{
					result = 9999999f;
				}
				else
				{
					result = Time.time - this.timeGameStartDialogClosed;
				}
				return result;
			}
		}

		// Token: 0x17000E23 RID: 3619
		// (get) Token: 0x060059A8 RID: 22952 RVA: 0x002DE644 File Offset: 0x002DCA44
		public bool MouseObscuredNow
		{
			get
			{
				return this.GetWindowAt(UI.MousePosUIInvertedUseEventIfCan) != this.currentlyDrawnWindow;
			}
		}

		// Token: 0x17000E24 RID: 3620
		// (get) Token: 0x060059A9 RID: 22953 RVA: 0x002DE670 File Offset: 0x002DCA70
		public bool CurrentWindowGetsInput
		{
			get
			{
				return this.GetsInput(this.currentlyDrawnWindow);
			}
		}

		// Token: 0x17000E25 RID: 3621
		// (get) Token: 0x060059AA RID: 22954 RVA: 0x002DE694 File Offset: 0x002DCA94
		public bool NonImmediateDialogWindowOpen
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (!(this.windows[i] is ImmediateWindow) && this.windows[i].layer == WindowLayer.Dialog)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060059AB RID: 22955 RVA: 0x002DE6FC File Offset: 0x002DCAFC
		public void WindowsUpdate()
		{
			this.AdjustWindowsIfResolutionChanged();
			for (int i = 0; i < this.windows.Count; i++)
			{
				this.windows[i].WindowUpdate();
			}
		}

		// Token: 0x060059AC RID: 22956 RVA: 0x002DE740 File Offset: 0x002DCB40
		public void HandleEventsHighPriority()
		{
			if (Event.current.type == EventType.MouseDown && this.GetWindowAt(UI.GUIToScreenPoint(Event.current.mousePosition)) == null)
			{
				bool flag = this.CloseWindowsBecauseClicked(null);
				if (flag)
				{
					Event.current.Use();
				}
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				this.Notify_PressedCancel();
			}
			if (KeyBindingDefOf.Accept.KeyDownEvent)
			{
				this.Notify_PressedAccept();
			}
			if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.KeyDown)
			{
				if (!this.GetsInput(null))
				{
					Event.current.Use();
				}
			}
		}

		// Token: 0x060059AD RID: 22957 RVA: 0x002DE7F4 File Offset: 0x002DCBF4
		public void WindowStackOnGUI()
		{
			this.windowStackOnGUITmpList.Clear();
			this.windowStackOnGUITmpList.AddRange(this.windows);
			for (int i = this.windowStackOnGUITmpList.Count - 1; i >= 0; i--)
			{
				this.windowStackOnGUITmpList[i].ExtraOnGUI();
			}
			this.UpdateImmediateWindowsList();
			this.windowStackOnGUITmpList.Clear();
			this.windowStackOnGUITmpList.AddRange(this.windows);
			for (int j = 0; j < this.windowStackOnGUITmpList.Count; j++)
			{
				if (this.windowStackOnGUITmpList[j].drawShadow)
				{
					GUI.color = new Color(1f, 1f, 1f, this.windowStackOnGUITmpList[j].shadowAlpha);
					Widgets.DrawShadowAround(this.windowStackOnGUITmpList[j].windowRect);
					GUI.color = Color.white;
				}
				this.windowStackOnGUITmpList[j].WindowOnGUI();
			}
			if (this.updateInternalWindowsOrderLater)
			{
				this.updateInternalWindowsOrderLater = false;
				this.UpdateInternalWindowsOrder();
			}
		}

		// Token: 0x060059AE RID: 22958 RVA: 0x002DE91C File Offset: 0x002DCD1C
		public void Notify_ClickedInsideWindow(Window window)
		{
			if (this.GetsInput(window))
			{
				this.windows.Remove(window);
				this.InsertAtCorrectPositionInList(window);
				this.focusedWindow = window;
			}
			else
			{
				Event.current.Use();
			}
			this.CloseWindowsBecauseClicked(window);
			this.updateInternalWindowsOrderLater = true;
		}

		// Token: 0x060059AF RID: 22959 RVA: 0x002DE971 File Offset: 0x002DCD71
		public void Notify_ManuallySetFocus(Window window)
		{
			this.focusedWindow = window;
			this.updateInternalWindowsOrderLater = true;
		}

		// Token: 0x060059B0 RID: 22960 RVA: 0x002DE984 File Offset: 0x002DCD84
		public void Notify_PressedCancel()
		{
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if ((this.windows[i].closeOnCancel || this.windows[i].forceCatchAcceptAndCancelEventEvenIfUnfocused) && this.GetsInput(this.windows[i]))
				{
					this.windows[i].OnCancelKeyPressed();
					break;
				}
			}
		}

		// Token: 0x060059B1 RID: 22961 RVA: 0x002DEA0C File Offset: 0x002DCE0C
		public void Notify_PressedAccept()
		{
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if ((this.windows[i].closeOnAccept || this.windows[i].forceCatchAcceptAndCancelEventEvenIfUnfocused) && this.GetsInput(this.windows[i]))
				{
					this.windows[i].OnAcceptKeyPressed();
					break;
				}
			}
		}

		// Token: 0x060059B2 RID: 22962 RVA: 0x002DEA94 File Offset: 0x002DCE94
		public void Notify_GameStartDialogOpened()
		{
			this.gameStartDialogOpen = true;
		}

		// Token: 0x060059B3 RID: 22963 RVA: 0x002DEA9E File Offset: 0x002DCE9E
		public void Notify_GameStartDialogClosed()
		{
			this.timeGameStartDialogClosed = Time.time;
			this.gameStartDialogOpen = false;
		}

		// Token: 0x060059B4 RID: 22964 RVA: 0x002DEAB4 File Offset: 0x002DCEB4
		public bool IsOpen<WindowType>()
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i] is WindowType)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060059B5 RID: 22965 RVA: 0x002DEB08 File Offset: 0x002DCF08
		public bool IsOpen(Type type)
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i].GetType() == type)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060059B6 RID: 22966 RVA: 0x002DEB5C File Offset: 0x002DCF5C
		public bool IsOpen(Window window)
		{
			return this.windows.Contains(window);
		}

		// Token: 0x060059B7 RID: 22967 RVA: 0x002DEB80 File Offset: 0x002DCF80
		public WindowType WindowOfType<WindowType>() where WindowType : class
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i] is WindowType)
				{
					return this.windows[i] as WindowType;
				}
			}
			return (WindowType)((object)null);
		}

		// Token: 0x060059B8 RID: 22968 RVA: 0x002DEBF0 File Offset: 0x002DCFF0
		public bool GetsInput(Window window)
		{
			int i = this.windows.Count - 1;
			while (i >= 0)
			{
				bool result;
				if (this.windows[i] == window)
				{
					result = true;
				}
				else
				{
					if (!this.windows[i].absorbInputAroundWindow)
					{
						i--;
						continue;
					}
					result = false;
				}
				return result;
			}
			return true;
		}

		// Token: 0x060059B9 RID: 22969 RVA: 0x002DEC5C File Offset: 0x002DD05C
		public void Add(Window window)
		{
			this.RemoveWindowsOfType(window.GetType());
			window.ID = WindowStack.uniqueWindowID++;
			window.PreOpen();
			this.InsertAtCorrectPositionInList(window);
			this.FocusAfterInsertIfShould(window);
			this.updateInternalWindowsOrderLater = true;
			window.PostOpen();
		}

		// Token: 0x060059BA RID: 22970 RVA: 0x002DECAC File Offset: 0x002DD0AC
		public void ImmediateWindow(int ID, Rect rect, WindowLayer layer, Action doWindowFunc, bool doBackground = true, bool absorbInputAroundWindow = false, float shadowAlpha = 1f)
		{
			if (Event.current.type == EventType.Repaint)
			{
				if (ID == 0)
				{
					Log.Warning("Used 0 as immediate window ID.", false);
				}
				else
				{
					ID = -Math.Abs(ID);
					bool flag = false;
					for (int i = 0; i < this.windows.Count; i++)
					{
						if (this.windows[i].ID == ID)
						{
							ImmediateWindow immediateWindow = (ImmediateWindow)this.windows[i];
							immediateWindow.windowRect = rect;
							immediateWindow.doWindowFunc = doWindowFunc;
							immediateWindow.layer = layer;
							immediateWindow.doWindowBackground = doBackground;
							immediateWindow.absorbInputAroundWindow = absorbInputAroundWindow;
							immediateWindow.shadowAlpha = shadowAlpha;
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.AddNewImmediateWindow(ID, rect, layer, doWindowFunc, doBackground, absorbInputAroundWindow, shadowAlpha);
					}
					this.immediateWindowsRequests.Add(ID);
				}
			}
		}

		// Token: 0x060059BB RID: 22971 RVA: 0x002DED94 File Offset: 0x002DD194
		public bool TryRemove(Type windowType, bool doCloseSound = true)
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i].GetType() == windowType)
				{
					return this.TryRemove(this.windows[i], doCloseSound);
				}
			}
			return false;
		}

		// Token: 0x060059BC RID: 22972 RVA: 0x002DEDF8 File Offset: 0x002DD1F8
		public bool TryRemoveAssignableFromType(Type windowType, bool doCloseSound = true)
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (windowType.IsAssignableFrom(this.windows[i].GetType()))
				{
					return this.TryRemove(this.windows[i], doCloseSound);
				}
			}
			return false;
		}

		// Token: 0x060059BD RID: 22973 RVA: 0x002DEE64 File Offset: 0x002DD264
		public bool TryRemove(Window window, bool doCloseSound = true)
		{
			bool flag = false;
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i] == window)
				{
					flag = true;
					break;
				}
			}
			bool result;
			if (!flag)
			{
				result = false;
			}
			else
			{
				if (doCloseSound && window.soundClose != null)
				{
					window.soundClose.PlayOneShotOnCamera(null);
				}
				window.PreClose();
				this.windows.Remove(window);
				window.PostClose();
				if (this.focusedWindow == window)
				{
					if (this.windows.Count > 0)
					{
						this.focusedWindow = this.windows[this.windows.Count - 1];
					}
					else
					{
						this.focusedWindow = null;
					}
					this.updateInternalWindowsOrderLater = true;
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060059BE RID: 22974 RVA: 0x002DEF48 File Offset: 0x002DD348
		public Window GetWindowAt(Vector2 pos)
		{
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if (this.windows[i].windowRect.Contains(pos))
				{
					return this.windows[i];
				}
			}
			return null;
		}

		// Token: 0x060059BF RID: 22975 RVA: 0x002DEFAC File Offset: 0x002DD3AC
		private void AddNewImmediateWindow(int ID, Rect rect, WindowLayer layer, Action doWindowFunc, bool doBackground, bool absorbInputAroundWindow, float shadowAlpha)
		{
			if (ID >= 0)
			{
				Log.Error("Invalid immediate window ID.", false);
			}
			else
			{
				ImmediateWindow immediateWindow = new ImmediateWindow();
				immediateWindow.ID = ID;
				immediateWindow.layer = layer;
				immediateWindow.doWindowFunc = doWindowFunc;
				immediateWindow.doWindowBackground = doBackground;
				immediateWindow.absorbInputAroundWindow = absorbInputAroundWindow;
				immediateWindow.shadowAlpha = shadowAlpha;
				immediateWindow.PreOpen();
				immediateWindow.windowRect = rect;
				this.InsertAtCorrectPositionInList(immediateWindow);
				this.FocusAfterInsertIfShould(immediateWindow);
				this.updateInternalWindowsOrderLater = true;
				immediateWindow.PostOpen();
			}
		}

		// Token: 0x060059C0 RID: 22976 RVA: 0x002DF030 File Offset: 0x002DD430
		private void UpdateImmediateWindowsList()
		{
			if (Event.current.type == EventType.Repaint)
			{
				this.updateImmediateWindowsListTmpList.Clear();
				this.updateImmediateWindowsListTmpList.AddRange(this.windows);
				for (int i = 0; i < this.updateImmediateWindowsListTmpList.Count; i++)
				{
					if (this.IsImmediateWindow(this.updateImmediateWindowsListTmpList[i]))
					{
						bool flag = false;
						for (int j = 0; j < this.immediateWindowsRequests.Count; j++)
						{
							if (this.immediateWindowsRequests[j] == this.updateImmediateWindowsListTmpList[i].ID)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							this.TryRemove(this.updateImmediateWindowsListTmpList[i], true);
						}
					}
				}
				this.immediateWindowsRequests.Clear();
			}
		}

		// Token: 0x060059C1 RID: 22977 RVA: 0x002DF118 File Offset: 0x002DD518
		private void InsertAtCorrectPositionInList(Window window)
		{
			int index = 0;
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (window.layer >= this.windows[i].layer)
				{
					index = i + 1;
				}
			}
			this.windows.Insert(index, window);
			this.updateInternalWindowsOrderLater = true;
		}

		// Token: 0x060059C2 RID: 22978 RVA: 0x002DF17C File Offset: 0x002DD57C
		private void FocusAfterInsertIfShould(Window window)
		{
			if (window.focusWhenOpened)
			{
				for (int i = this.windows.Count - 1; i >= 0; i--)
				{
					if (this.windows[i] == window)
					{
						this.focusedWindow = this.windows[i];
						this.updateInternalWindowsOrderLater = true;
						break;
					}
					if (this.windows[i] == this.focusedWindow)
					{
						break;
					}
				}
			}
		}

		// Token: 0x060059C3 RID: 22979 RVA: 0x002DF208 File Offset: 0x002DD608
		private void AdjustWindowsIfResolutionChanged()
		{
			IntVec2 a = new IntVec2(UI.screenWidth, UI.screenHeight);
			if (a != this.prevResolution)
			{
				this.prevResolution = a;
				for (int i = 0; i < this.windows.Count; i++)
				{
					this.windows[i].Notify_ResolutionChanged();
				}
				if (Current.ProgramState == ProgramState.Playing)
				{
					Find.ColonistBar.MarkColonistsDirty();
				}
			}
		}

		// Token: 0x060059C4 RID: 22980 RVA: 0x002DF288 File Offset: 0x002DD688
		private void RemoveWindowsOfType(Type type)
		{
			this.removeWindowsOfTypeTmpList.Clear();
			this.removeWindowsOfTypeTmpList.AddRange(this.windows);
			for (int i = 0; i < this.removeWindowsOfTypeTmpList.Count; i++)
			{
				if (this.removeWindowsOfTypeTmpList[i].onlyOneOfTypeAllowed && this.removeWindowsOfTypeTmpList[i].GetType() == type)
				{
					this.TryRemove(this.removeWindowsOfTypeTmpList[i], true);
				}
			}
		}

		// Token: 0x060059C5 RID: 22981 RVA: 0x002DF314 File Offset: 0x002DD714
		private bool CloseWindowsBecauseClicked(Window clickedWindow)
		{
			this.closeWindowsTmpList.Clear();
			this.closeWindowsTmpList.AddRange(this.windows);
			bool result = false;
			for (int i = this.closeWindowsTmpList.Count - 1; i >= 0; i--)
			{
				if (this.closeWindowsTmpList[i] == clickedWindow)
				{
					break;
				}
				if (this.closeWindowsTmpList[i].closeOnClickedOutside)
				{
					result = true;
					this.TryRemove(this.closeWindowsTmpList[i], true);
				}
			}
			return result;
		}

		// Token: 0x060059C6 RID: 22982 RVA: 0x002DF3B0 File Offset: 0x002DD7B0
		private bool IsImmediateWindow(Window window)
		{
			return window.ID < 0;
		}

		// Token: 0x060059C7 RID: 22983 RVA: 0x002DF3D0 File Offset: 0x002DD7D0
		private void UpdateInternalWindowsOrder()
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				GUI.BringWindowToFront(this.windows[i].ID);
			}
			if (this.focusedWindow != null)
			{
				GUI.FocusWindow(this.focusedWindow.ID);
			}
		}
	}
}
