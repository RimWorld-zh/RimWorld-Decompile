using System;
using RimWorld;
using UnityEngine;
using UnityEngine.Profiling;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000ECA RID: 3786
	public abstract class Window
	{
		// Token: 0x06005987 RID: 22919 RVA: 0x00067690 File Offset: 0x00065A90
		public Window()
		{
			this.soundAppear = SoundDefOf.DialogBoxAppear;
			this.soundClose = SoundDefOf.Click;
		}

		// Token: 0x17000E18 RID: 3608
		// (get) Token: 0x06005988 RID: 22920 RVA: 0x0006771C File Offset: 0x00065B1C
		public virtual Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 500f);
			}
		}

		// Token: 0x17000E19 RID: 3609
		// (get) Token: 0x06005989 RID: 22921 RVA: 0x00067740 File Offset: 0x00065B40
		protected virtual float Margin
		{
			get
			{
				return 18f;
			}
		}

		// Token: 0x17000E1A RID: 3610
		// (get) Token: 0x0600598A RID: 22922 RVA: 0x0006775C File Offset: 0x00065B5C
		public virtual bool IsDebug
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000E1B RID: 3611
		// (get) Token: 0x0600598B RID: 22923 RVA: 0x00067774 File Offset: 0x00065B74
		public bool IsOpen
		{
			get
			{
				return Find.WindowStack.IsOpen(this);
			}
		}

		// Token: 0x0600598C RID: 22924 RVA: 0x00067794 File Offset: 0x00065B94
		public virtual void WindowUpdate()
		{
			if (this.sustainerAmbient != null)
			{
				this.sustainerAmbient.Maintain();
			}
		}

		// Token: 0x0600598D RID: 22925
		public abstract void DoWindowContents(Rect inRect);

		// Token: 0x0600598E RID: 22926 RVA: 0x000677AD File Offset: 0x00065BAD
		public virtual void ExtraOnGUI()
		{
		}

		// Token: 0x0600598F RID: 22927 RVA: 0x000677B0 File Offset: 0x00065BB0
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

		// Token: 0x06005990 RID: 22928 RVA: 0x00067816 File Offset: 0x00065C16
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

		// Token: 0x06005991 RID: 22929 RVA: 0x00067852 File Offset: 0x00065C52
		public virtual void PreClose()
		{
		}

		// Token: 0x06005992 RID: 22930 RVA: 0x00067855 File Offset: 0x00065C55
		public virtual void PostClose()
		{
		}

		// Token: 0x06005993 RID: 22931 RVA: 0x00067858 File Offset: 0x00065C58
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

		// Token: 0x06005994 RID: 22932 RVA: 0x00067905 File Offset: 0x00065D05
		public virtual void Close(bool doCloseSound = true)
		{
			Find.WindowStack.TryRemove(this, doCloseSound);
		}

		// Token: 0x06005995 RID: 22933 RVA: 0x00067918 File Offset: 0x00065D18
		public virtual bool CausesMessageBackground()
		{
			return false;
		}

		// Token: 0x06005996 RID: 22934 RVA: 0x00067930 File Offset: 0x00065D30
		protected virtual void SetInitialSizeAndPosition()
		{
			this.windowRect = new Rect(((float)UI.screenWidth - this.InitialSize.x) / 2f, ((float)UI.screenHeight - this.InitialSize.y) / 2f, this.InitialSize.x, this.InitialSize.y);
			this.windowRect = this.windowRect.Rounded();
		}

		// Token: 0x06005997 RID: 22935 RVA: 0x000679AC File Offset: 0x00065DAC
		public virtual void OnCancelKeyPressed()
		{
			if (this.closeOnCancel)
			{
				this.Close(true);
				Event.current.Use();
			}
		}

		// Token: 0x06005998 RID: 22936 RVA: 0x000679CD File Offset: 0x00065DCD
		public virtual void OnAcceptKeyPressed()
		{
			if (this.closeOnAccept)
			{
				this.Close(true);
				Event.current.Use();
			}
		}

		// Token: 0x06005999 RID: 22937 RVA: 0x000679EE File Offset: 0x00065DEE
		public virtual void Notify_ResolutionChanged()
		{
			this.SetInitialSizeAndPosition();
		}

		// Token: 0x04003BD0 RID: 15312
		public WindowLayer layer = WindowLayer.Dialog;

		// Token: 0x04003BD1 RID: 15313
		public string optionalTitle;

		// Token: 0x04003BD2 RID: 15314
		public bool doCloseX;

		// Token: 0x04003BD3 RID: 15315
		public bool doCloseButton;

		// Token: 0x04003BD4 RID: 15316
		public bool closeOnAccept = true;

		// Token: 0x04003BD5 RID: 15317
		public bool closeOnCancel = true;

		// Token: 0x04003BD6 RID: 15318
		public bool forceCatchAcceptAndCancelEventEvenIfUnfocused;

		// Token: 0x04003BD7 RID: 15319
		public bool closeOnClickedOutside;

		// Token: 0x04003BD8 RID: 15320
		public bool forcePause;

		// Token: 0x04003BD9 RID: 15321
		public bool preventCameraMotion = true;

		// Token: 0x04003BDA RID: 15322
		public bool preventDrawTutor;

		// Token: 0x04003BDB RID: 15323
		public bool doWindowBackground = true;

		// Token: 0x04003BDC RID: 15324
		public bool onlyOneOfTypeAllowed = true;

		// Token: 0x04003BDD RID: 15325
		public bool absorbInputAroundWindow;

		// Token: 0x04003BDE RID: 15326
		public bool resizeable;

		// Token: 0x04003BDF RID: 15327
		public bool draggable;

		// Token: 0x04003BE0 RID: 15328
		public bool drawShadow = true;

		// Token: 0x04003BE1 RID: 15329
		public bool focusWhenOpened = true;

		// Token: 0x04003BE2 RID: 15330
		public float shadowAlpha = 1f;

		// Token: 0x04003BE3 RID: 15331
		public SoundDef soundAppear;

		// Token: 0x04003BE4 RID: 15332
		public SoundDef soundClose;

		// Token: 0x04003BE5 RID: 15333
		public SoundDef soundAmbient;

		// Token: 0x04003BE6 RID: 15334
		public bool silenceAmbientSound = false;

		// Token: 0x04003BE7 RID: 15335
		protected const float StandardMargin = 18f;

		// Token: 0x04003BE8 RID: 15336
		protected readonly Vector2 CloseButSize = new Vector2(120f, 40f);

		// Token: 0x04003BE9 RID: 15337
		public int ID;

		// Token: 0x04003BEA RID: 15338
		public Rect windowRect;

		// Token: 0x04003BEB RID: 15339
		private Sustainer sustainerAmbient;

		// Token: 0x04003BEC RID: 15340
		private WindowResizer resizer;

		// Token: 0x04003BED RID: 15341
		private bool resizeLater;

		// Token: 0x04003BEE RID: 15342
		private Rect resizeLaterRect;
	}
}
