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
		// Token: 0x17000E1A RID: 3610
		// (get) Token: 0x0600597E RID: 22910 RVA: 0x002DC4E4 File Offset: 0x002DA8E4
		public int Count
		{
			get
			{
				return this.windows.Count;
			}
		}

		// Token: 0x17000E1B RID: 3611
		public Window this[int index]
		{
			get
			{
				return this.windows[index];
			}
		}

		// Token: 0x17000E1C RID: 3612
		// (get) Token: 0x06005980 RID: 22912 RVA: 0x002DC528 File Offset: 0x002DA928
		public IList<Window> Windows
		{
			get
			{
				return this.windows.AsReadOnly();
			}
		}

		// Token: 0x17000E1D RID: 3613
		// (get) Token: 0x06005981 RID: 22913 RVA: 0x002DC548 File Offset: 0x002DA948
		public FloatMenu FloatMenu
		{
			get
			{
				return this.WindowOfType<FloatMenu>();
			}
		}

		// Token: 0x17000E1E RID: 3614
		// (get) Token: 0x06005982 RID: 22914 RVA: 0x002DC564 File Offset: 0x002DA964
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

		// Token: 0x17000E1F RID: 3615
		// (get) Token: 0x06005983 RID: 22915 RVA: 0x002DC5B8 File Offset: 0x002DA9B8
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

		// Token: 0x17000E20 RID: 3616
		// (get) Token: 0x06005984 RID: 22916 RVA: 0x002DC60C File Offset: 0x002DAA0C
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

		// Token: 0x17000E21 RID: 3617
		// (get) Token: 0x06005985 RID: 22917 RVA: 0x002DC660 File Offset: 0x002DAA60
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

		// Token: 0x17000E22 RID: 3618
		// (get) Token: 0x06005986 RID: 22918 RVA: 0x002DC6B4 File Offset: 0x002DAAB4
		public bool MouseObscuredNow
		{
			get
			{
				return this.GetWindowAt(UI.MousePosUIInvertedUseEventIfCan) != this.currentlyDrawnWindow;
			}
		}

		// Token: 0x17000E23 RID: 3619
		// (get) Token: 0x06005987 RID: 22919 RVA: 0x002DC6E0 File Offset: 0x002DAAE0
		public bool CurrentWindowGetsInput
		{
			get
			{
				return this.GetsInput(this.currentlyDrawnWindow);
			}
		}

		// Token: 0x17000E24 RID: 3620
		// (get) Token: 0x06005988 RID: 22920 RVA: 0x002DC704 File Offset: 0x002DAB04
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

		// Token: 0x06005989 RID: 22921 RVA: 0x002DC76C File Offset: 0x002DAB6C
		public void WindowsUpdate()
		{
			this.AdjustWindowsIfResolutionChanged();
			for (int i = 0; i < this.windows.Count; i++)
			{
				this.windows[i].WindowUpdate();
			}
		}

		// Token: 0x0600598A RID: 22922 RVA: 0x002DC7B0 File Offset: 0x002DABB0
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

		// Token: 0x0600598B RID: 22923 RVA: 0x002DC864 File Offset: 0x002DAC64
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

		// Token: 0x0600598C RID: 22924 RVA: 0x002DC98C File Offset: 0x002DAD8C
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

		// Token: 0x0600598D RID: 22925 RVA: 0x002DC9E1 File Offset: 0x002DADE1
		public void Notify_ManuallySetFocus(Window window)
		{
			this.focusedWindow = window;
			this.updateInternalWindowsOrderLater = true;
		}

		// Token: 0x0600598E RID: 22926 RVA: 0x002DC9F4 File Offset: 0x002DADF4
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

		// Token: 0x0600598F RID: 22927 RVA: 0x002DCA7C File Offset: 0x002DAE7C
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

		// Token: 0x06005990 RID: 22928 RVA: 0x002DCB04 File Offset: 0x002DAF04
		public void Notify_GameStartDialogOpened()
		{
			this.gameStartDialogOpen = true;
		}

		// Token: 0x06005991 RID: 22929 RVA: 0x002DCB0E File Offset: 0x002DAF0E
		public void Notify_GameStartDialogClosed()
		{
			this.timeGameStartDialogClosed = Time.time;
			this.gameStartDialogOpen = false;
		}

		// Token: 0x06005992 RID: 22930 RVA: 0x002DCB24 File Offset: 0x002DAF24
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

		// Token: 0x06005993 RID: 22931 RVA: 0x002DCB78 File Offset: 0x002DAF78
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

		// Token: 0x06005994 RID: 22932 RVA: 0x002DCBCC File Offset: 0x002DAFCC
		public bool IsOpen(Window window)
		{
			return this.windows.Contains(window);
		}

		// Token: 0x06005995 RID: 22933 RVA: 0x002DCBF0 File Offset: 0x002DAFF0
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

		// Token: 0x06005996 RID: 22934 RVA: 0x002DCC60 File Offset: 0x002DB060
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

		// Token: 0x06005997 RID: 22935 RVA: 0x002DCCCC File Offset: 0x002DB0CC
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

		// Token: 0x06005998 RID: 22936 RVA: 0x002DCD1C File Offset: 0x002DB11C
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

		// Token: 0x06005999 RID: 22937 RVA: 0x002DCE04 File Offset: 0x002DB204
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

		// Token: 0x0600599A RID: 22938 RVA: 0x002DCE68 File Offset: 0x002DB268
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

		// Token: 0x0600599B RID: 22939 RVA: 0x002DCED4 File Offset: 0x002DB2D4
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

		// Token: 0x0600599C RID: 22940 RVA: 0x002DCFB8 File Offset: 0x002DB3B8
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

		// Token: 0x0600599D RID: 22941 RVA: 0x002DD01C File Offset: 0x002DB41C
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

		// Token: 0x0600599E RID: 22942 RVA: 0x002DD0A0 File Offset: 0x002DB4A0
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

		// Token: 0x0600599F RID: 22943 RVA: 0x002DD188 File Offset: 0x002DB588
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

		// Token: 0x060059A0 RID: 22944 RVA: 0x002DD1EC File Offset: 0x002DB5EC
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

		// Token: 0x060059A1 RID: 22945 RVA: 0x002DD278 File Offset: 0x002DB678
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

		// Token: 0x060059A2 RID: 22946 RVA: 0x002DD2F8 File Offset: 0x002DB6F8
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

		// Token: 0x060059A3 RID: 22947 RVA: 0x002DD384 File Offset: 0x002DB784
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

		// Token: 0x060059A4 RID: 22948 RVA: 0x002DD420 File Offset: 0x002DB820
		private bool IsImmediateWindow(Window window)
		{
			return window.ID < 0;
		}

		// Token: 0x060059A5 RID: 22949 RVA: 0x002DD440 File Offset: 0x002DB840
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

		// Token: 0x04003BE4 RID: 15332
		public Window currentlyDrawnWindow;

		// Token: 0x04003BE5 RID: 15333
		private List<Window> windows = new List<Window>();

		// Token: 0x04003BE6 RID: 15334
		private List<int> immediateWindowsRequests = new List<int>();

		// Token: 0x04003BE7 RID: 15335
		private bool updateInternalWindowsOrderLater;

		// Token: 0x04003BE8 RID: 15336
		private Window focusedWindow;

		// Token: 0x04003BE9 RID: 15337
		private static int uniqueWindowID;

		// Token: 0x04003BEA RID: 15338
		private bool gameStartDialogOpen;

		// Token: 0x04003BEB RID: 15339
		private float timeGameStartDialogClosed = -1f;

		// Token: 0x04003BEC RID: 15340
		private IntVec2 prevResolution = new IntVec2(UI.screenWidth, UI.screenHeight);

		// Token: 0x04003BED RID: 15341
		private List<Window> windowStackOnGUITmpList = new List<Window>();

		// Token: 0x04003BEE RID: 15342
		private List<Window> updateImmediateWindowsListTmpList = new List<Window>();

		// Token: 0x04003BEF RID: 15343
		private List<Window> removeWindowsOfTypeTmpList = new List<Window>();

		// Token: 0x04003BF0 RID: 15344
		private List<Window> closeWindowsTmpList = new List<Window>();
	}
}
