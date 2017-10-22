using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	public class WindowStack
	{
		public Window currentlyDrawnWindow;

		private List<Window> windows = new List<Window>();

		private List<int> immediateWindowsRequests = new List<int>();

		private bool updateInternalWindowsOrderLater;

		private Window focusedWindow;

		private static int uniqueWindowID;

		private bool gameStartDialogOpen;

		private float timeGameStartDialogClosed = -1f;

		private IntVec2 prevResolution = new IntVec2(UI.screenWidth, UI.screenHeight);

		private List<Window> windowStackOnGUITmpList = new List<Window>();

		private List<Window> updateImmediateWindowsListTmpList = new List<Window>();

		private List<Window> removeWindowsOfTypeTmpList = new List<Window>();

		private List<Window> closeWindowsTmpList = new List<Window>();

		public int Count
		{
			get
			{
				return this.windows.Count;
			}
		}

		public Window this[int index]
		{
			get
			{
				return this.windows[index];
			}
		}

		public IList<Window> Windows
		{
			get
			{
				return this.windows.AsReadOnly();
			}
		}

		public FloatMenu FloatMenu
		{
			get
			{
				return this.WindowOfType<FloatMenu>();
			}
		}

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

		public float SecondsSinceClosedGameStartDialog
		{
			get
			{
				if (this.gameStartDialogOpen)
				{
					return 0f;
				}
				if (this.timeGameStartDialogClosed < 0.0)
				{
					return 9999999f;
				}
				return Time.time - this.timeGameStartDialogClosed;
			}
		}

		public bool MouseObscuredNow
		{
			get
			{
				Vector3 v = (Event.current == null) ? UI.MousePositionOnUIInverted : UI.GUIToScreenPoint(Event.current.mousePosition);
				return this.GetWindowAt(v) != this.currentlyDrawnWindow;
			}
		}

		public bool CurrentWindowGetsInput
		{
			get
			{
				return this.GetsInput(this.currentlyDrawnWindow);
			}
		}

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

		public void WindowsUpdate()
		{
			this.AdjustWindowsIfResolutionChanged();
			for (int i = 0; i < this.windows.Count; i++)
			{
				this.windows[i].WindowUpdate();
			}
		}

		public void HandleEventsHighPriority()
		{
			if (Event.current.type == EventType.MouseDown && this.GetWindowAt(UI.GUIToScreenPoint(Event.current.mousePosition)) == null && this.CloseWindowsBecauseClicked(null))
			{
				Event.current.Use();
			}
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
			{
				this.Notify_PressedEscape();
			}
			if (Event.current.type != 0 && Event.current.type != EventType.KeyDown)
				return;
			if (!this.GetsInput(null))
			{
				Event.current.Use();
			}
		}

		public void WindowStackOnGUI()
		{
			this.windowStackOnGUITmpList.Clear();
			this.windowStackOnGUITmpList.AddRange(this.windows);
			for (int num = this.windowStackOnGUITmpList.Count - 1; num >= 0; num--)
			{
				this.windowStackOnGUITmpList[num].ExtraOnGUI();
			}
			this.UpdateImmediateWindowsList();
			this.windowStackOnGUITmpList.Clear();
			this.windowStackOnGUITmpList.AddRange(this.windows);
			for (int i = 0; i < this.windowStackOnGUITmpList.Count; i++)
			{
				if (this.windowStackOnGUITmpList[i].drawShadow)
				{
					GUI.color = new Color(1f, 1f, 1f, this.windowStackOnGUITmpList[i].shadowAlpha);
					Widgets.DrawShadowAround(this.windowStackOnGUITmpList[i].windowRect);
					GUI.color = Color.white;
				}
				this.windowStackOnGUITmpList[i].WindowOnGUI();
			}
			if (this.updateInternalWindowsOrderLater)
			{
				this.updateInternalWindowsOrderLater = false;
				this.UpdateInternalWindowsOrder();
			}
		}

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

		public void Notify_ManuallySetFocus(Window window)
		{
			this.focusedWindow = window;
			this.updateInternalWindowsOrderLater = true;
		}

		public void Notify_PressedEscape()
		{
			int num = this.windows.Count - 1;
			while (true)
			{
				if (num >= 0)
				{
					if (this.windows[num].closeOnEscapeKey && this.GetsInput(this.windows[num]))
						break;
					num--;
					continue;
				}
				return;
			}
			Event.current.Use();
			this.TryRemove(this.windows[num], true);
		}

		public void Notify_GameStartDialogOpened()
		{
			this.gameStartDialogOpen = true;
		}

		public void Notify_GameStartDialogClosed()
		{
			this.timeGameStartDialogClosed = Time.time;
			this.gameStartDialogOpen = false;
		}

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

		public WindowType WindowOfType<WindowType>() where WindowType : Window
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i] is WindowType)
				{
					return (WindowType)this.windows[i];
				}
			}
			return (WindowType)null;
		}

		public bool GetsInput(Window window)
		{
			for (int num = this.windows.Count - 1; num >= 0; num--)
			{
				if (this.windows[num] == window)
				{
					return true;
				}
				if (this.windows[num].absorbInputAroundWindow)
				{
					return false;
				}
			}
			return true;
		}

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

		public void ImmediateWindow(int ID, Rect rect, WindowLayer layer, Action doWindowFunc, bool doBackground = true, bool absorbInputAroundWindow = false, float shadowAlpha = 1f)
		{
			if (Event.current.type == EventType.Repaint)
			{
				if (ID == 0)
				{
					Log.Warning("Used 0 as immediate window ID.");
				}
				else
				{
					ID = -Math.Abs(ID);
					bool flag = false;
					int num = 0;
					while (num < this.windows.Count)
					{
						if (this.windows[num].ID != ID)
						{
							num++;
							continue;
						}
						ImmediateWindow immediateWindow = (ImmediateWindow)this.windows[num];
						immediateWindow.windowRect = rect;
						immediateWindow.doWindowFunc = doWindowFunc;
						immediateWindow.layer = layer;
						immediateWindow.doWindowBackground = doBackground;
						immediateWindow.absorbInputAroundWindow = absorbInputAroundWindow;
						immediateWindow.shadowAlpha = shadowAlpha;
						flag = true;
						break;
					}
					if (!flag)
					{
						this.AddNewImmediateWindow(ID, rect, layer, doWindowFunc, doBackground, absorbInputAroundWindow, shadowAlpha);
					}
					this.immediateWindowsRequests.Add(ID);
				}
			}
		}

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

		public bool TryRemove(Window window, bool doCloseSound = true)
		{
			bool flag = false;
			int num = 0;
			while (num < this.windows.Count)
			{
				if (this.windows[num] != window)
				{
					num++;
					continue;
				}
				flag = true;
				break;
			}
			if (!flag)
			{
				return false;
			}
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
			return true;
		}

		public Window GetWindowAt(Vector2 pos)
		{
			for (int num = this.windows.Count - 1; num >= 0; num--)
			{
				if (this.windows[num].windowRect.Contains(pos))
				{
					return this.windows[num];
				}
			}
			return null;
		}

		private void AddNewImmediateWindow(int ID, Rect rect, WindowLayer layer, Action doWindowFunc, bool doBackground, bool absorbInputAroundWindow, float shadowAlpha)
		{
			if (ID >= 0)
			{
				Log.Error("Invalid immediate window ID.");
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
						int num = 0;
						while (num < this.immediateWindowsRequests.Count)
						{
							if (this.immediateWindowsRequests[num] != this.updateImmediateWindowsListTmpList[i].ID)
							{
								num++;
								continue;
							}
							flag = true;
							break;
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

		private void FocusAfterInsertIfShould(Window window)
		{
			if (window.focusWhenOpened)
			{
				int num = this.windows.Count - 1;
				while (true)
				{
					if (num >= 0)
					{
						if (this.windows[num] == window)
						{
							break;
						}
						if (this.windows[num] != this.focusedWindow)
						{
							num--;
							continue;
						}
					}
					return;
				}
				this.focusedWindow = this.windows[num];
				this.updateInternalWindowsOrderLater = true;
			}
		}

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

		private bool CloseWindowsBecauseClicked(Window clickedWindow)
		{
			this.closeWindowsTmpList.Clear();
			this.closeWindowsTmpList.AddRange(this.windows);
			bool result = false;
			int num = this.closeWindowsTmpList.Count - 1;
			while (num >= 0 && this.closeWindowsTmpList[num] != clickedWindow)
			{
				if (this.closeWindowsTmpList[num].closeOnClickedOutside)
				{
					result = true;
					this.TryRemove(this.closeWindowsTmpList[num], true);
				}
				num--;
			}
			return result;
		}

		private bool IsImmediateWindow(Window window)
		{
			return window.ID < 0;
		}

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
