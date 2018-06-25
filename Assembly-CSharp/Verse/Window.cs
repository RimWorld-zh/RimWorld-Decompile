using System;
using RimWorld;
using UnityEngine;
using UnityEngine.Profiling;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000ECD RID: 3789
	public abstract class Window
	{
		// Token: 0x04003BD8 RID: 15320
		public WindowLayer layer = WindowLayer.Dialog;

		// Token: 0x04003BD9 RID: 15321
		public string optionalTitle;

		// Token: 0x04003BDA RID: 15322
		public bool doCloseX;

		// Token: 0x04003BDB RID: 15323
		public bool doCloseButton;

		// Token: 0x04003BDC RID: 15324
		public bool closeOnAccept = true;

		// Token: 0x04003BDD RID: 15325
		public bool closeOnCancel = true;

		// Token: 0x04003BDE RID: 15326
		public bool forceCatchAcceptAndCancelEventEvenIfUnfocused;

		// Token: 0x04003BDF RID: 15327
		public bool closeOnClickedOutside;

		// Token: 0x04003BE0 RID: 15328
		public bool forcePause;

		// Token: 0x04003BE1 RID: 15329
		public bool preventCameraMotion = true;

		// Token: 0x04003BE2 RID: 15330
		public bool preventDrawTutor;

		// Token: 0x04003BE3 RID: 15331
		public bool doWindowBackground = true;

		// Token: 0x04003BE4 RID: 15332
		public bool onlyOneOfTypeAllowed = true;

		// Token: 0x04003BE5 RID: 15333
		public bool absorbInputAroundWindow;

		// Token: 0x04003BE6 RID: 15334
		public bool resizeable;

		// Token: 0x04003BE7 RID: 15335
		public bool draggable;

		// Token: 0x04003BE8 RID: 15336
		public bool drawShadow = true;

		// Token: 0x04003BE9 RID: 15337
		public bool focusWhenOpened = true;

		// Token: 0x04003BEA RID: 15338
		public float shadowAlpha = 1f;

		// Token: 0x04003BEB RID: 15339
		public SoundDef soundAppear;

		// Token: 0x04003BEC RID: 15340
		public SoundDef soundClose;

		// Token: 0x04003BED RID: 15341
		public SoundDef soundAmbient;

		// Token: 0x04003BEE RID: 15342
		public bool silenceAmbientSound = false;

		// Token: 0x04003BEF RID: 15343
		protected const float StandardMargin = 18f;

		// Token: 0x04003BF0 RID: 15344
		protected readonly Vector2 CloseButSize = new Vector2(120f, 40f);

		// Token: 0x04003BF1 RID: 15345
		public int ID;

		// Token: 0x04003BF2 RID: 15346
		public Rect windowRect;

		// Token: 0x04003BF3 RID: 15347
		private Sustainer sustainerAmbient;

		// Token: 0x04003BF4 RID: 15348
		private WindowResizer resizer;

		// Token: 0x04003BF5 RID: 15349
		private bool resizeLater;

		// Token: 0x04003BF6 RID: 15350
		private Rect resizeLaterRect;

		// Token: 0x0600598A RID: 22922 RVA: 0x000677DC File Offset: 0x00065BDC
		public Window()
		{
			this.soundAppear = SoundDefOf.DialogBoxAppear;
			this.soundClose = SoundDefOf.Click;
		}

		// Token: 0x17000E17 RID: 3607
		// (get) Token: 0x0600598B RID: 22923 RVA: 0x00067868 File Offset: 0x00065C68
		public virtual Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 500f);
			}
		}

		// Token: 0x17000E18 RID: 3608
		// (get) Token: 0x0600598C RID: 22924 RVA: 0x0006788C File Offset: 0x00065C8C
		protected virtual float Margin
		{
			get
			{
				return 18f;
			}
		}

		// Token: 0x17000E19 RID: 3609
		// (get) Token: 0x0600598D RID: 22925 RVA: 0x000678A8 File Offset: 0x00065CA8
		public virtual bool IsDebug
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000E1A RID: 3610
		// (get) Token: 0x0600598E RID: 22926 RVA: 0x000678C0 File Offset: 0x00065CC0
		public bool IsOpen
		{
			get
			{
				return Find.WindowStack.IsOpen(this);
			}
		}

		// Token: 0x0600598F RID: 22927 RVA: 0x000678E0 File Offset: 0x00065CE0
		public virtual void WindowUpdate()
		{
			if (this.sustainerAmbient != null)
			{
				this.sustainerAmbient.Maintain();
			}
		}

		// Token: 0x06005990 RID: 22928
		public abstract void DoWindowContents(Rect inRect);

		// Token: 0x06005991 RID: 22929 RVA: 0x000678F9 File Offset: 0x00065CF9
		public virtual void ExtraOnGUI()
		{
		}

		// Token: 0x06005992 RID: 22930 RVA: 0x000678FC File Offset: 0x00065CFC
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

		// Token: 0x06005993 RID: 22931 RVA: 0x00067962 File Offset: 0x00065D62
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

		// Token: 0x06005994 RID: 22932 RVA: 0x0006799E File Offset: 0x00065D9E
		public virtual void PreClose()
		{
		}

		// Token: 0x06005995 RID: 22933 RVA: 0x000679A1 File Offset: 0x00065DA1
		public virtual void PostClose()
		{
		}

		// Token: 0x06005996 RID: 22934 RVA: 0x000679A4 File Offset: 0x00065DA4
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

		// Token: 0x06005997 RID: 22935 RVA: 0x00067A51 File Offset: 0x00065E51
		public virtual void Close(bool doCloseSound = true)
		{
			Find.WindowStack.TryRemove(this, doCloseSound);
		}

		// Token: 0x06005998 RID: 22936 RVA: 0x00067A64 File Offset: 0x00065E64
		public virtual bool CausesMessageBackground()
		{
			return false;
		}

		// Token: 0x06005999 RID: 22937 RVA: 0x00067A7C File Offset: 0x00065E7C
		protected virtual void SetInitialSizeAndPosition()
		{
			this.windowRect = new Rect(((float)UI.screenWidth - this.InitialSize.x) / 2f, ((float)UI.screenHeight - this.InitialSize.y) / 2f, this.InitialSize.x, this.InitialSize.y);
			this.windowRect = this.windowRect.Rounded();
		}

		// Token: 0x0600599A RID: 22938 RVA: 0x00067AF8 File Offset: 0x00065EF8
		public virtual void OnCancelKeyPressed()
		{
			if (this.closeOnCancel)
			{
				this.Close(true);
				Event.current.Use();
			}
		}

		// Token: 0x0600599B RID: 22939 RVA: 0x00067B19 File Offset: 0x00065F19
		public virtual void OnAcceptKeyPressed()
		{
			if (this.closeOnAccept)
			{
				this.Close(true);
				Event.current.Use();
			}
		}

		// Token: 0x0600599C RID: 22940 RVA: 0x00067B3A File Offset: 0x00065F3A
		public virtual void Notify_ResolutionChanged()
		{
			this.SetInitialSizeAndPosition();
		}
	}
}
