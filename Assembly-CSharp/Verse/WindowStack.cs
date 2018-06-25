using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000ECE RID: 3790
	public class WindowStack
	{
		// Token: 0x04003BF3 RID: 15347
		public Window currentlyDrawnWindow;

		// Token: 0x04003BF4 RID: 15348
		private List<Window> windows = new List<Window>();

		// Token: 0x04003BF5 RID: 15349
		private List<int> immediateWindowsRequests = new List<int>();

		// Token: 0x04003BF6 RID: 15350
		private bool updateInternalWindowsOrderLater;

		// Token: 0x04003BF7 RID: 15351
		private Window focusedWindow;

		// Token: 0x04003BF8 RID: 15352
		private static int uniqueWindowID;

		// Token: 0x04003BF9 RID: 15353
		private bool gameStartDialogOpen;

		// Token: 0x04003BFA RID: 15354
		private float timeGameStartDialogClosed = -1f;

		// Token: 0x04003BFB RID: 15355
		private IntVec2 prevResolution = new IntVec2(UI.screenWidth, UI.screenHeight);

		// Token: 0x04003BFC RID: 15356
		private List<Window> windowStackOnGUITmpList = new List<Window>();

		// Token: 0x04003BFD RID: 15357
		private List<Window> updateImmediateWindowsListTmpList = new List<Window>();

		// Token: 0x04003BFE RID: 15358
		private List<Window> removeWindowsOfTypeTmpList = new List<Window>();

		// Token: 0x04003BFF RID: 15359
		private List<Window> closeWindowsTmpList = new List<Window>();

		// Token: 0x17000E1B RID: 3611
		// (get) Token: 0x060059A0 RID: 22944 RVA: 0x002DE288 File Offset: 0x002DC688
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
		// (get) Token: 0x060059A2 RID: 22946 RVA: 0x002DE2CC File Offset: 0x002DC6CC
		public IList<Window> Windows
		{
			get
			{
				return this.windows.AsReadOnly();
			}
		}

		// Token: 0x17000E1E RID: 3614
		// (get) Token: 0x060059A3 RID: 22947 RVA: 0x002DE2EC File Offset: 0x002DC6EC
		public FloatMenu FloatMenu
		{
			get
			{
				return this.WindowOfType<FloatMenu>();
			}
		}

		// Token: 0x17000E1F RID: 3615
		// (get) Token: 0x060059A4 RID: 22948 RVA: 0x002DE308 File Offset: 0x002DC708
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
		// (get) Token: 0x060059A5 RID: 22949 RVA: 0x002DE35C File Offset: 0x002DC75C
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
		// (get) Token: 0x060059A6 RID: 22950 RVA: 0x002DE3B0 File Offset: 0x002DC7B0
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
		// (get) Token: 0x060059A7 RID: 22951 RVA: 0x002DE404 File Offset: 0x002DC804
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
		// (get) Token: 0x060059A8 RID: 22952 RVA: 0x002DE458 File Offset: 0x002DC858
		public bool MouseObscuredNow
		{
			get
			{
				return this.GetWindowAt(UI.MousePosUIInvertedUseEventIfCan) != this.currentlyDrawnWindow;
			}
		}

		// Token: 0x17000E24 RID: 3620
		// (get) Token: 0x060059A9 RID: 22953 RVA: 0x002DE484 File Offset: 0x002DC884
		public bool CurrentWindowGetsInput
		{
			get
			{
				return this.GetsInput(this.currentlyDrawnWindow);
			}
		}

		// Token: 0x17000E25 RID: 3621
		// (get) Token: 0x060059AA RID: 22954 RVA: 0x002DE4A8 File Offset: 0x002DC8A8
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

		// Token: 0x060059AB RID: 22955 RVA: 0x002DE510 File Offset: 0x002DC910
		public void WindowsUpdate()
		{
			this.AdjustWindowsIfResolutionChanged();
			for (int i = 0; i < this.windows.Count; i++)
			{
				this.windows[i].WindowUpdate();
			}
		}

		// Token: 0x060059AC RID: 22956 RVA: 0x002DE554 File Offset: 0x002DC954
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

		// Token: 0x060059AD RID: 22957 RVA: 0x002DE608 File Offset: 0x002DCA08
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

		// Token: 0x060059AE RID: 22958 RVA: 0x002DE730 File Offset: 0x002DCB30
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

		// Token: 0x060059AF RID: 22959 RVA: 0x002DE785 File Offset: 0x002DCB85
		public void Notify_ManuallySetFocus(Window window)
		{
			this.focusedWindow = window;
			this.updateInternalWindowsOrderLater = true;
		}

		// Token: 0x060059B0 RID: 22960 RVA: 0x002DE798 File Offset: 0x002DCB98
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

		// Token: 0x060059B1 RID: 22961 RVA: 0x002DE820 File Offset: 0x002DCC20
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

		// Token: 0x060059B2 RID: 22962 RVA: 0x002DE8A8 File Offset: 0x002DCCA8
		public void Notify_GameStartDialogOpened()
		{
			this.gameStartDialogOpen = true;
		}

		// Token: 0x060059B3 RID: 22963 RVA: 0x002DE8B2 File Offset: 0x002DCCB2
		public void Notify_GameStartDialogClosed()
		{
			this.timeGameStartDialogClosed = Time.time;
			this.gameStartDialogOpen = false;
		}

		// Token: 0x060059B4 RID: 22964 RVA: 0x002DE8C8 File Offset: 0x002DCCC8
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

		// Token: 0x060059B5 RID: 22965 RVA: 0x002DE91C File Offset: 0x002DCD1C
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

		// Token: 0x060059B6 RID: 22966 RVA: 0x002DE970 File Offset: 0x002DCD70
		public bool IsOpen(Window window)
		{
			return this.windows.Contains(window);
		}

		// Token: 0x060059B7 RID: 22967 RVA: 0x002DE994 File Offset: 0x002DCD94
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

		// Token: 0x060059B8 RID: 22968 RVA: 0x002DEA04 File Offset: 0x002DCE04
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

		// Token: 0x060059B9 RID: 22969 RVA: 0x002DEA70 File Offset: 0x002DCE70
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

		// Token: 0x060059BA RID: 22970 RVA: 0x002DEAC0 File Offset: 0x002DCEC0
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

		// Token: 0x060059BB RID: 22971 RVA: 0x002DEBA8 File Offset: 0x002DCFA8
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

		// Token: 0x060059BC RID: 22972 RVA: 0x002DEC0C File Offset: 0x002DD00C
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

		// Token: 0x060059BD RID: 22973 RVA: 0x002DEC78 File Offset: 0x002DD078
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

		// Token: 0x060059BE RID: 22974 RVA: 0x002DED5C File Offset: 0x002DD15C
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

		// Token: 0x060059BF RID: 22975 RVA: 0x002DEDC0 File Offset: 0x002DD1C0
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

		// Token: 0x060059C0 RID: 22976 RVA: 0x002DEE44 File Offset: 0x002DD244
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

		// Token: 0x060059C1 RID: 22977 RVA: 0x002DEF2C File Offset: 0x002DD32C
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

		// Token: 0x060059C2 RID: 22978 RVA: 0x002DEF90 File Offset: 0x002DD390
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

		// Token: 0x060059C3 RID: 22979 RVA: 0x002DF01C File Offset: 0x002DD41C
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

		// Token: 0x060059C4 RID: 22980 RVA: 0x002DF09C File Offset: 0x002DD49C
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

		// Token: 0x060059C5 RID: 22981 RVA: 0x002DF128 File Offset: 0x002DD528
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

		// Token: 0x060059C6 RID: 22982 RVA: 0x002DF1C4 File Offset: 0x002DD5C4
		private bool IsImmediateWindow(Window window)
		{
			return window.ID < 0;
		}

		// Token: 0x060059C7 RID: 22983 RVA: 0x002DF1E4 File Offset: 0x002DD5E4
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
