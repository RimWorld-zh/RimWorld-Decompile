using System;
using RimWorld;
using UnityEngine;
using UnityEngine.Profiling;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000ECB RID: 3787
	public abstract class Window
	{
		// Token: 0x06005966 RID: 22886 RVA: 0x00067628 File Offset: 0x00065A28
		public Window()
		{
			this.soundAppear = SoundDefOf.DialogBoxAppear;
			this.soundClose = SoundDefOf.Click;
		}

		// Token: 0x17000E15 RID: 3605
		// (get) Token: 0x06005967 RID: 22887 RVA: 0x000676B4 File Offset: 0x00065AB4
		public virtual Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 500f);
			}
		}

		// Token: 0x17000E16 RID: 3606
		// (get) Token: 0x06005968 RID: 22888 RVA: 0x000676D8 File Offset: 0x00065AD8
		protected virtual float Margin
		{
			get
			{
				return 18f;
			}
		}

		// Token: 0x17000E17 RID: 3607
		// (get) Token: 0x06005969 RID: 22889 RVA: 0x000676F4 File Offset: 0x00065AF4
		public virtual bool IsDebug
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000E18 RID: 3608
		// (get) Token: 0x0600596A RID: 22890 RVA: 0x0006770C File Offset: 0x00065B0C
		public bool IsOpen
		{
			get
			{
				return Find.WindowStack.IsOpen(this);
			}
		}

		// Token: 0x0600596B RID: 22891 RVA: 0x0006772C File Offset: 0x00065B2C
		public virtual void WindowUpdate()
		{
			if (this.sustainerAmbient != null)
			{
				this.sustainerAmbient.Maintain();
			}
		}

		// Token: 0x0600596C RID: 22892
		public abstract void DoWindowContents(Rect inRect);

		// Token: 0x0600596D RID: 22893 RVA: 0x00067745 File Offset: 0x00065B45
		public virtual void ExtraOnGUI()
		{
		}

		// Token: 0x0600596E RID: 22894 RVA: 0x00067748 File Offset: 0x00065B48
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

		// Token: 0x0600596F RID: 22895 RVA: 0x000677AE File Offset: 0x00065BAE
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

		// Token: 0x06005970 RID: 22896 RVA: 0x000677EA File Offset: 0x00065BEA
		public virtual void PreClose()
		{
		}

		// Token: 0x06005971 RID: 22897 RVA: 0x000677ED File Offset: 0x00065BED
		public virtual void PostClose()
		{
		}

		// Token: 0x06005972 RID: 22898 RVA: 0x000677F0 File Offset: 0x00065BF0
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

		// Token: 0x06005973 RID: 22899 RVA: 0x0006789D File Offset: 0x00065C9D
		public virtual void Close(bool doCloseSound = true)
		{
			Find.WindowStack.TryRemove(this, doCloseSound);
		}

		// Token: 0x06005974 RID: 22900 RVA: 0x000678B0 File Offset: 0x00065CB0
		public virtual bool CausesMessageBackground()
		{
			return false;
		}

		// Token: 0x06005975 RID: 22901 RVA: 0x000678C8 File Offset: 0x00065CC8
		protected virtual void SetInitialSizeAndPosition()
		{
			this.windowRect = new Rect(((float)UI.screenWidth - this.InitialSize.x) / 2f, ((float)UI.screenHeight - this.InitialSize.y) / 2f, this.InitialSize.x, this.InitialSize.y);
			this.windowRect = this.windowRect.Rounded();
		}

		// Token: 0x06005976 RID: 22902 RVA: 0x00067944 File Offset: 0x00065D44
		public virtual void OnCancelKeyPressed()
		{
			if (this.closeOnCancel)
			{
				this.Close(true);
				Event.current.Use();
			}
		}

		// Token: 0x06005977 RID: 22903 RVA: 0x00067965 File Offset: 0x00065D65
		public virtual void OnAcceptKeyPressed()
		{
			if (this.closeOnAccept)
			{
				this.Close(true);
				Event.current.Use();
			}
		}

		// Token: 0x06005978 RID: 22904 RVA: 0x00067986 File Offset: 0x00065D86
		public virtual void Notify_ResolutionChanged()
		{
			this.SetInitialSizeAndPosition();
		}

		// Token: 0x04003BC0 RID: 15296
		public WindowLayer layer = WindowLayer.Dialog;

		// Token: 0x04003BC1 RID: 15297
		public string optionalTitle;

		// Token: 0x04003BC2 RID: 15298
		public bool doCloseX;

		// Token: 0x04003BC3 RID: 15299
		public bool doCloseButton;

		// Token: 0x04003BC4 RID: 15300
		public bool closeOnAccept = true;

		// Token: 0x04003BC5 RID: 15301
		public bool closeOnCancel = true;

		// Token: 0x04003BC6 RID: 15302
		public bool forceCatchAcceptAndCancelEventEvenIfUnfocused;

		// Token: 0x04003BC7 RID: 15303
		public bool closeOnClickedOutside;

		// Token: 0x04003BC8 RID: 15304
		public bool forcePause;

		// Token: 0x04003BC9 RID: 15305
		public bool preventCameraMotion = true;

		// Token: 0x04003BCA RID: 15306
		public bool preventDrawTutor;

		// Token: 0x04003BCB RID: 15307
		public bool doWindowBackground = true;

		// Token: 0x04003BCC RID: 15308
		public bool onlyOneOfTypeAllowed = true;

		// Token: 0x04003BCD RID: 15309
		public bool absorbInputAroundWindow;

		// Token: 0x04003BCE RID: 15310
		public bool resizeable;

		// Token: 0x04003BCF RID: 15311
		public bool draggable;

		// Token: 0x04003BD0 RID: 15312
		public bool drawShadow = true;

		// Token: 0x04003BD1 RID: 15313
		public bool focusWhenOpened = true;

		// Token: 0x04003BD2 RID: 15314
		public float shadowAlpha = 1f;

		// Token: 0x04003BD3 RID: 15315
		public SoundDef soundAppear;

		// Token: 0x04003BD4 RID: 15316
		public SoundDef soundClose;

		// Token: 0x04003BD5 RID: 15317
		public SoundDef soundAmbient;

		// Token: 0x04003BD6 RID: 15318
		public bool silenceAmbientSound = false;

		// Token: 0x04003BD7 RID: 15319
		protected const float StandardMargin = 18f;

		// Token: 0x04003BD8 RID: 15320
		protected readonly Vector2 CloseButSize = new Vector2(120f, 40f);

		// Token: 0x04003BD9 RID: 15321
		public int ID;

		// Token: 0x04003BDA RID: 15322
		public Rect windowRect;

		// Token: 0x04003BDB RID: 15323
		private Sustainer sustainerAmbient;

		// Token: 0x04003BDC RID: 15324
		private WindowResizer resizer;

		// Token: 0x04003BDD RID: 15325
		private bool resizeLater;

		// Token: 0x04003BDE RID: 15326
		private Rect resizeLaterRect;
	}
}
