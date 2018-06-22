using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000ECC RID: 3788
	public class WindowStack
	{
		// Token: 0x17000E1C RID: 3612
		// (get) Token: 0x0600599D RID: 22941 RVA: 0x002DE168 File Offset: 0x002DC568
		public int Count
		{
			get
			{
				return this.windows.Count;
			}
		}

		// Token: 0x17000E1D RID: 3613
		public Window this[int index]
		{
			get
			{
				return this.windows[index];
			}
		}

		// Token: 0x17000E1E RID: 3614
		// (get) Token: 0x0600599F RID: 22943 RVA: 0x002DE1AC File Offset: 0x002DC5AC
		public IList<Window> Windows
		{
			get
			{
				return this.windows.AsReadOnly();
			}
		}

		// Token: 0x17000E1F RID: 3615
		// (get) Token: 0x060059A0 RID: 22944 RVA: 0x002DE1CC File Offset: 0x002DC5CC
		public FloatMenu FloatMenu
		{
			get
			{
				return this.WindowOfType<FloatMenu>();
			}
		}

		// Token: 0x17000E20 RID: 3616
		// (get) Token: 0x060059A1 RID: 22945 RVA: 0x002DE1E8 File Offset: 0x002DC5E8
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

		// Token: 0x17000E21 RID: 3617
		// (get) Token: 0x060059A2 RID: 22946 RVA: 0x002DE23C File Offset: 0x002DC63C
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

		// Token: 0x17000E22 RID: 3618
		// (get) Token: 0x060059A3 RID: 22947 RVA: 0x002DE290 File Offset: 0x002DC690
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

		// Token: 0x17000E23 RID: 3619
		// (get) Token: 0x060059A4 RID: 22948 RVA: 0x002DE2E4 File Offset: 0x002DC6E4
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

		// Token: 0x17000E24 RID: 3620
		// (get) Token: 0x060059A5 RID: 22949 RVA: 0x002DE338 File Offset: 0x002DC738
		public bool MouseObscuredNow
		{
			get
			{
				return this.GetWindowAt(UI.MousePosUIInvertedUseEventIfCan) != this.currentlyDrawnWindow;
			}
		}

		// Token: 0x17000E25 RID: 3621
		// (get) Token: 0x060059A6 RID: 22950 RVA: 0x002DE364 File Offset: 0x002DC764
		public bool CurrentWindowGetsInput
		{
			get
			{
				return this.GetsInput(this.currentlyDrawnWindow);
			}
		}

		// Token: 0x17000E26 RID: 3622
		// (get) Token: 0x060059A7 RID: 22951 RVA: 0x002DE388 File Offset: 0x002DC788
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

		// Token: 0x060059A8 RID: 22952 RVA: 0x002DE3F0 File Offset: 0x002DC7F0
		public void WindowsUpdate()
		{
			this.AdjustWindowsIfResolutionChanged();
			for (int i = 0; i < this.windows.Count; i++)
			{
				this.windows[i].WindowUpdate();
			}
		}

		// Token: 0x060059A9 RID: 22953 RVA: 0x002DE434 File Offset: 0x002DC834
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

		// Token: 0x060059AA RID: 22954 RVA: 0x002DE4E8 File Offset: 0x002DC8E8
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

		// Token: 0x060059AB RID: 22955 RVA: 0x002DE610 File Offset: 0x002DCA10
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

		// Token: 0x060059AC RID: 22956 RVA: 0x002DE665 File Offset: 0x002DCA65
		public void Notify_ManuallySetFocus(Window window)
		{
			this.focusedWindow = window;
			this.updateInternalWindowsOrderLater = true;
		}

		// Token: 0x060059AD RID: 22957 RVA: 0x002DE678 File Offset: 0x002DCA78
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

		// Token: 0x060059AE RID: 22958 RVA: 0x002DE700 File Offset: 0x002DCB00
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

		// Token: 0x060059AF RID: 22959 RVA: 0x002DE788 File Offset: 0x002DCB88
		public void Notify_GameStartDialogOpened()
		{
			this.gameStartDialogOpen = true;
		}

		// Token: 0x060059B0 RID: 22960 RVA: 0x002DE792 File Offset: 0x002DCB92
		public void Notify_GameStartDialogClosed()
		{
			this.timeGameStartDialogClosed = Time.time;
			this.gameStartDialogOpen = false;
		}

		// Token: 0x060059B1 RID: 22961 RVA: 0x002DE7A8 File Offset: 0x002DCBA8
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

		// Token: 0x060059B2 RID: 22962 RVA: 0x002DE7FC File Offset: 0x002DCBFC
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

		// Token: 0x060059B3 RID: 22963 RVA: 0x002DE850 File Offset: 0x002DCC50
		public bool IsOpen(Window window)
		{
			return this.windows.Contains(window);
		}

		// Token: 0x060059B4 RID: 22964 RVA: 0x002DE874 File Offset: 0x002DCC74
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

		// Token: 0x060059B5 RID: 22965 RVA: 0x002DE8E4 File Offset: 0x002DCCE4
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

		// Token: 0x060059B6 RID: 22966 RVA: 0x002DE950 File Offset: 0x002DCD50
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

		// Token: 0x060059B7 RID: 22967 RVA: 0x002DE9A0 File Offset: 0x002DCDA0
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

		// Token: 0x060059B8 RID: 22968 RVA: 0x002DEA88 File Offset: 0x002DCE88
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

		// Token: 0x060059B9 RID: 22969 RVA: 0x002DEAEC File Offset: 0x002DCEEC
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

		// Token: 0x060059BA RID: 22970 RVA: 0x002DEB58 File Offset: 0x002DCF58
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

		// Token: 0x060059BB RID: 22971 RVA: 0x002DEC3C File Offset: 0x002DD03C
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

		// Token: 0x060059BC RID: 22972 RVA: 0x002DECA0 File Offset: 0x002DD0A0
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

		// Token: 0x060059BD RID: 22973 RVA: 0x002DED24 File Offset: 0x002DD124
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

		// Token: 0x060059BE RID: 22974 RVA: 0x002DEE0C File Offset: 0x002DD20C
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

		// Token: 0x060059BF RID: 22975 RVA: 0x002DEE70 File Offset: 0x002DD270
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

		// Token: 0x060059C0 RID: 22976 RVA: 0x002DEEFC File Offset: 0x002DD2FC
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

		// Token: 0x060059C1 RID: 22977 RVA: 0x002DEF7C File Offset: 0x002DD37C
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

		// Token: 0x060059C2 RID: 22978 RVA: 0x002DF008 File Offset: 0x002DD408
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

		// Token: 0x060059C3 RID: 22979 RVA: 0x002DF0A4 File Offset: 0x002DD4A4
		private bool IsImmediateWindow(Window window)
		{
			return window.ID < 0;
		}

		// Token: 0x060059C4 RID: 22980 RVA: 0x002DF0C4 File Offset: 0x002DD4C4
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
	}
}
