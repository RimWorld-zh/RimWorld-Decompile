using RimWorld;
using System;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	public abstract class Window
	{
		public WindowLayer layer = WindowLayer.Dialog;

		public string optionalTitle;

		public bool doCloseX;

		public bool doCloseButton;

		public bool closeOnEscapeKey = true;

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

		public bool silenceAmbientSound;

		protected const float StandardMargin = 18f;

		protected readonly Vector2 CloseButSize = new Vector2(120f, 40f);

		public int ID;

		public Rect windowRect;

		private Sustainer sustainerAmbient;

		private WindowResizer resizer;

		private bool resizeLater;

		private Rect resizeLaterRect;

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

		public Window()
		{
			this.soundAppear = SoundDefOf.DialogBoxAppear;
			this.soundClose = SoundDefOf.Click;
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
				UnityGUIBugsFixer.OnGUI();
				Find.WindowStack.currentlyDrawnWindow = this;
				if (this.doWindowBackground)
				{
					Widgets.DrawWindowBackground(winRect);
				}
				if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
				{
					Find.WindowStack.Notify_PressedEscape();
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
				if (this.doCloseX && Widgets.CloseButtonFor(winRect))
				{
					this.Close(true);
				}
				if (this.resizeable && Event.current.type != EventType.Repaint)
				{
					Rect lhs = this.resizer.DoResizeControl(this.windowRect);
					if (lhs != this.windowRect)
					{
						this.resizeLater = true;
						this.resizeLaterRect = lhs;
					}
				}
				Rect rect = winRect.ContractedBy(this.Margin);
				if (!this.optionalTitle.NullOrEmpty())
				{
					rect.yMin += (float)(this.Margin + 25.0);
				}
				GUI.BeginGroup(rect);
				try
				{
					this.DoWindowContents(rect.AtZero());
				}
				catch (Exception ex)
				{
					Log.Error("Exception filling window for " + base.GetType().ToString() + ": " + ex);
				}
				GUI.EndGroup();
				if (this.resizeable && Event.current.type == EventType.Repaint)
				{
					this.resizer.DoResizeControl(this.windowRect);
				}
				if (this.doCloseButton)
				{
					Text.Font = GameFont.Small;
					double num = winRect.width / 2.0;
					Vector2 closeButSize = this.CloseButSize;
					double x2 = num - closeButSize.x / 2.0;
					double y = winRect.height - 55.0;
					Vector2 closeButSize2 = this.CloseButSize;
					float x3 = closeButSize2.x;
					Vector2 closeButSize3 = this.CloseButSize;
					Rect rect2 = new Rect((float)x2, (float)y, x3, closeButSize3.y);
					if (Widgets.ButtonText(rect2, "CloseButton".Translate(), true, false, true))
					{
						this.Close(true);
					}
				}
				if (this.closeOnEscapeKey && Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Escape || Event.current.keyCode == KeyCode.Return))
				{
					this.Close(true);
					Event.current.Use();
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
			}, string.Empty, Widgets.EmptyStyle);
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
			float num = (float)UI.screenWidth;
			Vector2 initialSize = this.InitialSize;
			double x = (num - initialSize.x) / 2.0;
			float num2 = (float)UI.screenHeight;
			Vector2 initialSize2 = this.InitialSize;
			double y = (num2 - initialSize2.y) / 2.0;
			Vector2 initialSize3 = this.InitialSize;
			float x2 = initialSize3.x;
			Vector2 initialSize4 = this.InitialSize;
			this.windowRect = new Rect((float)x, (float)y, x2, initialSize4.y);
			this.windowRect = this.windowRect.Rounded();
		}

		public virtual void Notify_ResolutionChanged()
		{
			this.SetInitialSizeAndPosition();
		}
	}
}
