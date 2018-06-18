using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000ECD RID: 3789
	public class WindowStack
	{
		// Token: 0x17000E19 RID: 3609
		// (get) Token: 0x0600597C RID: 22908 RVA: 0x002DC51C File Offset: 0x002DA91C
		public int Count
		{
			get
			{
				return this.windows.Count;
			}
		}

		// Token: 0x17000E1A RID: 3610
		public Window this[int index]
		{
			get
			{
				return this.windows[index];
			}
		}

		// Token: 0x17000E1B RID: 3611
		// (get) Token: 0x0600597E RID: 22910 RVA: 0x002DC560 File Offset: 0x002DA960
		public IList<Window> Windows
		{
			get
			{
				return this.windows.AsReadOnly();
			}
		}

		// Token: 0x17000E1C RID: 3612
		// (get) Token: 0x0600597F RID: 22911 RVA: 0x002DC580 File Offset: 0x002DA980
		public FloatMenu FloatMenu
		{
			get
			{
				return this.WindowOfType<FloatMenu>();
			}
		}

		// Token: 0x17000E1D RID: 3613
		// (get) Token: 0x06005980 RID: 22912 RVA: 0x002DC59C File Offset: 0x002DA99C
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

		// Token: 0x17000E1E RID: 3614
		// (get) Token: 0x06005981 RID: 22913 RVA: 0x002DC5F0 File Offset: 0x002DA9F0
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

		// Token: 0x17000E1F RID: 3615
		// (get) Token: 0x06005982 RID: 22914 RVA: 0x002DC644 File Offset: 0x002DAA44
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

		// Token: 0x17000E20 RID: 3616
		// (get) Token: 0x06005983 RID: 22915 RVA: 0x002DC698 File Offset: 0x002DAA98
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

		// Token: 0x17000E21 RID: 3617
		// (get) Token: 0x06005984 RID: 22916 RVA: 0x002DC6EC File Offset: 0x002DAAEC
		public bool MouseObscuredNow
		{
			get
			{
				return this.GetWindowAt(UI.MousePosUIInvertedUseEventIfCan) != this.currentlyDrawnWindow;
			}
		}

		// Token: 0x17000E22 RID: 3618
		// (get) Token: 0x06005985 RID: 22917 RVA: 0x002DC718 File Offset: 0x002DAB18
		public bool CurrentWindowGetsInput
		{
			get
			{
				return this.GetsInput(this.currentlyDrawnWindow);
			}
		}

		// Token: 0x17000E23 RID: 3619
		// (get) Token: 0x06005986 RID: 22918 RVA: 0x002DC73C File Offset: 0x002DAB3C
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

		// Token: 0x06005987 RID: 22919 RVA: 0x002DC7A4 File Offset: 0x002DABA4
		public void WindowsUpdate()
		{
			this.AdjustWindowsIfResolutionChanged();
			for (int i = 0; i < this.windows.Count; i++)
			{
				this.windows[i].WindowUpdate();
			}
		}

		// Token: 0x06005988 RID: 22920 RVA: 0x002DC7E8 File Offset: 0x002DABE8
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

		// Token: 0x06005989 RID: 22921 RVA: 0x002DC89C File Offset: 0x002DAC9C
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

		// Token: 0x0600598A RID: 22922 RVA: 0x002DC9C4 File Offset: 0x002DADC4
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

		// Token: 0x0600598B RID: 22923 RVA: 0x002DCA19 File Offset: 0x002DAE19
		public void Notify_ManuallySetFocus(Window window)
		{
			this.focusedWindow = window;
			this.updateInternalWindowsOrderLater = true;
		}

		// Token: 0x0600598C RID: 22924 RVA: 0x002DCA2C File Offset: 0x002DAE2C
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

		// Token: 0x0600598D RID: 22925 RVA: 0x002DCAB4 File Offset: 0x002DAEB4
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

		// Token: 0x0600598E RID: 22926 RVA: 0x002DCB3C File Offset: 0x002DAF3C
		public void Notify_GameStartDialogOpened()
		{
			this.gameStartDialogOpen = true;
		}

		// Token: 0x0600598F RID: 22927 RVA: 0x002DCB46 File Offset: 0x002DAF46
		public void Notify_GameStartDialogClosed()
		{
			this.timeGameStartDialogClosed = Time.time;
			this.gameStartDialogOpen = false;
		}

		// Token: 0x06005990 RID: 22928 RVA: 0x002DCB5C File Offset: 0x002DAF5C
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

		// Token: 0x06005991 RID: 22929 RVA: 0x002DCBB0 File Offset: 0x002DAFB0
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

		// Token: 0x06005992 RID: 22930 RVA: 0x002DCC04 File Offset: 0x002DB004
		public bool IsOpen(Window window)
		{
			return this.windows.Contains(window);
		}

		// Token: 0x06005993 RID: 22931 RVA: 0x002DCC28 File Offset: 0x002DB028
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

		// Token: 0x06005994 RID: 22932 RVA: 0x002DCC98 File Offset: 0x002DB098
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

		// Token: 0x06005995 RID: 22933 RVA: 0x002DCD04 File Offset: 0x002DB104
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

		// Token: 0x06005996 RID: 22934 RVA: 0x002DCD54 File Offset: 0x002DB154
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

		// Token: 0x06005997 RID: 22935 RVA: 0x002DCE3C File Offset: 0x002DB23C
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

		// Token: 0x06005998 RID: 22936 RVA: 0x002DCEA0 File Offset: 0x002DB2A0
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

		// Token: 0x06005999 RID: 22937 RVA: 0x002DCF0C File Offset: 0x002DB30C
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

		// Token: 0x0600599A RID: 22938 RVA: 0x002DCFF0 File Offset: 0x002DB3F0
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

		// Token: 0x0600599B RID: 22939 RVA: 0x002DD054 File Offset: 0x002DB454
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

		// Token: 0x0600599C RID: 22940 RVA: 0x002DD0D8 File Offset: 0x002DB4D8
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

		// Token: 0x0600599D RID: 22941 RVA: 0x002DD1C0 File Offset: 0x002DB5C0
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

		// Token: 0x0600599E RID: 22942 RVA: 0x002DD224 File Offset: 0x002DB624
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

		// Token: 0x0600599F RID: 22943 RVA: 0x002DD2B0 File Offset: 0x002DB6B0
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

		// Token: 0x060059A0 RID: 22944 RVA: 0x002DD330 File Offset: 0x002DB730
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

		// Token: 0x060059A1 RID: 22945 RVA: 0x002DD3BC File Offset: 0x002DB7BC
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

		// Token: 0x060059A2 RID: 22946 RVA: 0x002DD458 File Offset: 0x002DB858
		private bool IsImmediateWindow(Window window)
		{
			return window.ID < 0;
		}

		// Token: 0x060059A3 RID: 22947 RVA: 0x002DD478 File Offset: 0x002DB878
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

		// Token: 0x04003BE3 RID: 15331
		public Window currentlyDrawnWindow;

		// Token: 0x04003BE4 RID: 15332
		private List<Window> windows = new List<Window>();

		// Token: 0x04003BE5 RID: 15333
		private List<int> immediateWindowsRequests = new List<int>();

		// Token: 0x04003BE6 RID: 15334
		private bool updateInternalWindowsOrderLater;

		// Token: 0x04003BE7 RID: 15335
		private Window focusedWindow;

		// Token: 0x04003BE8 RID: 15336
		private static int uniqueWindowID;

		// Token: 0x04003BE9 RID: 15337
		private bool gameStartDialogOpen;

		// Token: 0x04003BEA RID: 15338
		private float timeGameStartDialogClosed = -1f;

		// Token: 0x04003BEB RID: 15339
		private IntVec2 prevResolution = new IntVec2(UI.screenWidth, UI.screenHeight);

		// Token: 0x04003BEC RID: 15340
		private List<Window> windowStackOnGUITmpList = new List<Window>();

		// Token: 0x04003BED RID: 15341
		private List<Window> updateImmediateWindowsListTmpList = new List<Window>();

		// Token: 0x04003BEE RID: 15342
		private List<Window> removeWindowsOfTypeTmpList = new List<Window>();

		// Token: 0x04003BEF RID: 15343
		private List<Window> closeWindowsTmpList = new List<Window>();
	}
}
