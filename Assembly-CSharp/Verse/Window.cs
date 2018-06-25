using System;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using UnityEngine.Profiling;
using Verse.Sound;

namespace Verse
{
	public abstract class Window
	{
		public WindowLayer layer = WindowLayer.Dialog;

		public string optionalTitle;

		public bool doCloseX;

		public bool doCloseButton;

		public bool closeOnAccept = true;

		public bool closeOnCancel = true;

		public bool forceCatchAcceptAndCancelEventEvenIfUnfocused;

		public bool closeOnClickedOutside;

		public bool forcePause;

		public bool preventCameraMotion = true;

		public bool preventDrawTutor;

		public bool doWindowBackground = true;

		public bool onlyOneOfTypeAllowed = true;

		public bool absorbInputAroundWindow;

		public bool resizeable;

		public bool draggable;

		public bool drawShadow = true;

		public bool focusWhenOpened = true;

		public float shadowAlpha = 1f;

		public SoundDef soundAppear;

		public SoundDef soundClose;

		public SoundDef soundAmbient;

		public bool silenceAmbientSound = false;

		protected const float StandardMargin = 18f;

		protected readonly Vector2 CloseButSize = new Vector2(120f, 40f);

		public int ID;

		public Rect windowRect;

		private Sustainer sustainerAmbient;

		private WindowResizer resizer;

		private bool resizeLater;

		private Rect resizeLaterRect;

		public Window()
		{
			this.soundAppear = SoundDefOf.DialogBoxAppear;
			this.soundClose = SoundDefOf.Click;
		}

		public virtual Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 500f);
			}
		}

		protected virtual float Margin
		{
			get
			{
				return 18f;
			}
		}

		public virtual bool IsDebug
		{
			get
			{
				return false;
			}
		}

		public bool IsOpen
		{
			get
			{
				return Find.WindowStack.IsOpen(this);
			}
		}

		public virtual void WindowUpdate()
		{
			if (this.sustainerAmbient != null)
			{
				this.sustainerAmbient.Maintain();
			}
		}

		public abstract void DoWindowContents(Rect inRect);

		public virtual void ExtraOnGUI()
		{
		}

		public virtual void PreOpen()
		{
			this.SetInitialSizeAndPosition();
			if (this.layer == WindowLayer.Dialog)
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					Find.DesignatorManager.Dragger.EndDrag();
					Find.DesignatorManager.Deselect();
					Find.Selector.Notify_DialogOpened();
				}
				if (Find.World != null)
				{
					Find.WorldSelector.Notify_DialogOpened();
				}
			}
		}

		public virtual void PostOpen()
		{
			if (this.soundAppear != null)
			{
				this.soundAppear.PlayOneShotOnCamera(null);
			}
			if (this.soundAmbient != null)
			{
				this.sustainerAmbient = this.soundAmbient.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.PerFrame));
			}
		}

		public virtual void PreClose()
		{
		}

		public virtual void PostClose()
		{
		}

		public virtual void WindowOnGUI()
		{
			if (this.resizeable)
			{
				if (this.resizer == null)
				{
					this.resizer = new WindowResizer();
				}
				if (this.resizeLater)
				{
					this.resizeLater = false;
					this.windowRect = this.resizeLaterRect;
				}
			}
			this.windowRect = this.windowRect.Rounded();
			Rect winRect = this.windowRect.AtZero();
			this.windowRect = GUI.Window(this.ID, this.windowRect, delegate(int x)
			{
				Profiler.BeginSample("WindowOnGUI: " + this.GetType().Name);
				UnityGUIBugsFixer.OnGUI();
				Find.WindowStack.currentlyDrawnWindow = this;
				if (this.doWindowBackground)
				{
					Widgets.DrawWindowBackground(winRect);
				}
				if (KeyBindingDefOf.Cancel.KeyDownEvent)
				{
					Find.WindowStack.Notify_PressedCancel();
				}
				if (KeyBindingDefOf.Accept.KeyDownEvent)
				{
					Find.WindowStack.Notify_PressedAccept();
				}
				if (Event.current.type == EventType.MouseDown)
				{
					Find.WindowStack.Notify_ClickedInsideWindow(this);
				}
				if (Event.current.type == EventType.KeyDown && !Find.WindowStack.GetsInput(this))
				{
					Event.current.Use();
				}
				if (!this.optionalTitle.NullOrEmpty())
				{
					GUI.Label(new Rect(this.Margin, this.Margin, this.windowRect.width, 25f), this.optionalTitle);
				}
				if (this.doCloseX)
				{
					if (Widgets.CloseButtonFor(winRect))
					{
						this.Close(true);
					}
				}
				if (this.resizeable)
				{
					if (Event.current.type != EventType.Repaint)
					{
						Rect lhs = this.resizer.DoResizeControl(this.windowRect);
						if (lhs != this.windowRect)
						{
							this.resizeLater = true;
							this.resizeLaterRect = lhs;
						}
					}
				}
				Rect rect = winRect.ContractedBy(this.Margin);
				if (!this.optionalTitle.NullOrEmpty())
				{
					rect.yMin += this.Margin + 25f;
				}
				GUI.BeginGroup(rect);
				try
				{
					this.DoWindowContents(rect.AtZero());
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception filling window for ",
						this.GetType(),
						": ",
						ex
					}), false);
				}
				GUI.EndGroup();
				if (this.resizeable)
				{
					if (Event.current.type == EventType.Repaint)
					{
						this.resizer.DoResizeControl(this.windowRect);
					}
				}
				if (this.doCloseButton)
				{
					Text.Font = GameFont.Small;
					Rect rect2 = new Rect(winRect.width / 2f - this.CloseButSize.x / 2f, winRect.height - 55f, this.CloseButSize.x, this.CloseButSize.y);
					if (Widgets.ButtonText(rect2, "CloseButton".Translate(), true, false, true))
					{
						this.Close(true);
					}
				}
				if (KeyBindingDefOf.Cancel.KeyDownEvent && this.IsOpen)
				{
					this.OnCancelKeyPressed();
				}
				if (this.draggable)
				{
					GUI.DragWindow();
				}
				else if (Event.current.type == EventType.MouseDown)
				{
					Event.current.Use();
				}
				ScreenFader.OverlayOnGUI(winRect.size);
				Find.WindowStack.currentlyDrawnWindow = null;
				Profiler.EndSample();
			}, "", Widgets.EmptyStyle);
		}

		public virtual void Close(bool doCloseSound = true)
		{
			Find.WindowStack.TryRemove(this, doCloseSound);
		}

		public virtual bool CausesMessageBackground()
		{
			return false;
		}

		protected virtual void SetInitialSizeAndPosition()
		{
			this.windowRect = new Rect(((float)UI.screenWidth - this.InitialSize.x) / 2f, ((float)UI.screenHeight - this.InitialSize.y) / 2f, this.InitialSize.x, this.InitialSize.y);
			this.windowRect = this.windowRect.Rounded();
		}

		public virtual void OnCancelKeyPressed()
		{
			if (this.closeOnCancel)
			{
				this.Close(true);
				Event.current.Use();
			}
		}

		public virtual void OnAcceptKeyPressed()
		{
			if (this.closeOnAccept)
			{
				this.Close(true);
				Event.current.Use();
			}
		}

		public virtual void Notify_ResolutionChanged()
		{
			this.SetInitialSizeAndPosition();
		}

		[CompilerGenerated]
		private sealed class <WindowOnGUI>c__AnonStorey0
		{
			internal Rect winRect;

			internal Window $this;

			public <WindowOnGUI>c__AnonStorey0()
			{
			}

			internal void <>m__0(int x)
			{
				Profiler.BeginSample("WindowOnGUI: " + this.$this.GetType().Name);
				UnityGUIBugsFixer.OnGUI();
				Find.WindowStack.currentlyDrawnWindow = this.$this;
				if (this.$this.doWindowBackground)
				{
					Widgets.DrawWindowBackground(this.winRect);
				}
				if (KeyBindingDefOf.Cancel.KeyDownEvent)
				{
					Find.WindowStack.Notify_PressedCancel();
				}
				if (KeyBindingDefOf.Accept.KeyDownEvent)
				{
					Find.WindowStack.Notify_PressedAccept();
				}
				if (Event.current.type == EventType.MouseDown)
				{
					Find.WindowStack.Notify_ClickedInsideWindow(this.$this);
				}
				if (Event.current.type == EventType.KeyDown && !Find.WindowStack.GetsInput(this.$this))
				{
					Event.current.Use();
				}
				if (!this.$this.optionalTitle.NullOrEmpty())
				{
					GUI.Label(new Rect(this.$this.Margin, this.$this.Margin, this.$this.windowRect.width, 25f), this.$this.optionalTitle);
				}
				if (this.$this.doCloseX)
				{
					if (Widgets.CloseButtonFor(this.winRect))
					{
						this.$this.Close(true);
					}
				}
				if (this.$this.resizeable)
				{
					if (Event.current.type != EventType.Repaint)
					{
						Rect rect = this.$this.resizer.DoResizeControl(this.$this.windowRect);
						if (rect != this.$this.windowRect)
						{
							this.$this.resizeLater = true;
							this.$this.resizeLaterRect = rect;
						}
					}
				}
				Rect rect2 = this.winRect.ContractedBy(this.$this.Margin);
				if (!this.$this.optionalTitle.NullOrEmpty())
				{
					rect2.yMin += this.$this.Margin + 25f;
				}
				GUI.BeginGroup(rect2);
				try
				{
					this.$this.DoWindowContents(rect2.AtZero());
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception filling window for ",
						this.$this.GetType(),
						": ",
						ex
					}), false);
				}
				GUI.EndGroup();
				if (this.$this.resizeable)
				{
					if (Event.current.type == EventType.Repaint)
					{
						this.$this.resizer.DoResizeControl(this.$this.windowRect);
					}
				}
				if (this.$this.doCloseButton)
				{
					Text.Font = GameFont.Small;
					Rect rect3 = new Rect(this.winRect.width / 2f - this.$this.CloseButSize.x / 2f, this.winRect.height - 55f, this.$this.CloseButSize.x, this.$this.CloseButSize.y);
					if (Widgets.ButtonText(rect3, "CloseButton".Translate(), true, false, true))
					{
						this.$this.Close(true);
					}
				}
				if (KeyBindingDefOf.Cancel.KeyDownEvent && this.$this.IsOpen)
				{
					this.$this.OnCancelKeyPressed();
				}
				if (this.$this.draggable)
				{
					GUI.DragWindow();
				}
				else if (Event.current.type == EventType.MouseDown)
				{
					Event.current.Use();
				}
				ScreenFader.OverlayOnGUI(this.winRect.size);
				Find.WindowStack.currentlyDrawnWindow = null;
				Profiler.EndSample();
			}
		}
	}
}
