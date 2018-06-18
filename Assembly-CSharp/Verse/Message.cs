using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E87 RID: 3719
	public class Message : IArchivable, IExposable
	{
		// Token: 0x060057A3 RID: 22435 RVA: 0x002CF258 File Offset: 0x002CD658
		public Message()
		{
		}

		// Token: 0x060057A4 RID: 22436 RVA: 0x002CF278 File Offset: 0x002CD678
		public Message(string text, MessageTypeDef def)
		{
			this.text = text;
			this.def = def;
			this.startingFrame = RealTime.frameCount;
			this.startingTime = RealTime.LastRealTime;
			this.startingTick = GenTicks.TicksGame;
			this.ID = Rand.Int;
		}

		// Token: 0x060057A5 RID: 22437 RVA: 0x002CF2DB File Offset: 0x002CD6DB
		public Message(string text, MessageTypeDef def, LookTargets lookTargets) : this(text, def)
		{
			this.lookTargets = lookTargets;
		}

		// Token: 0x17000DDF RID: 3551
		// (get) Token: 0x060057A6 RID: 22438 RVA: 0x002CF2F0 File Offset: 0x002CD6F0
		protected float Age
		{
			get
			{
				return RealTime.LastRealTime - this.startingTime;
			}
		}

		// Token: 0x17000DE0 RID: 3552
		// (get) Token: 0x060057A7 RID: 22439 RVA: 0x002CF314 File Offset: 0x002CD714
		protected float TimeLeft
		{
			get
			{
				return 13f - this.Age;
			}
		}

		// Token: 0x17000DE1 RID: 3553
		// (get) Token: 0x060057A8 RID: 22440 RVA: 0x002CF338 File Offset: 0x002CD738
		public bool Expired
		{
			get
			{
				return this.TimeLeft <= 0f;
			}
		}

		// Token: 0x17000DE2 RID: 3554
		// (get) Token: 0x060057A9 RID: 22441 RVA: 0x002CF360 File Offset: 0x002CD760
		public float Alpha
		{
			get
			{
				float result;
				if (this.TimeLeft < 0.6f)
				{
					result = this.TimeLeft / 0.6f;
				}
				else
				{
					result = 1f;
				}
				return result;
			}
		}

		// Token: 0x17000DE3 RID: 3555
		// (get) Token: 0x060057AA RID: 22442 RVA: 0x002CF39C File Offset: 0x002CD79C
		private static bool ShouldDrawBackground
		{
			get
			{
				bool result;
				if (Current.ProgramState != ProgramState.Playing)
				{
					result = true;
				}
				else
				{
					WindowStack windowStack = Find.WindowStack;
					for (int i = 0; i < windowStack.Count; i++)
					{
						if (windowStack[i].CausesMessageBackground())
						{
							return true;
						}
					}
					result = false;
				}
				return result;
			}
		}

		// Token: 0x17000DD8 RID: 3544
		// (get) Token: 0x060057AB RID: 22443 RVA: 0x002CF400 File Offset: 0x002CD800
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000DD9 RID: 3545
		// (get) Token: 0x060057AC RID: 22444 RVA: 0x002CF418 File Offset: 0x002CD818
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x17000DDA RID: 3546
		// (get) Token: 0x060057AD RID: 22445 RVA: 0x002CF434 File Offset: 0x002CD834
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.text.Flatten();
			}
		}

		// Token: 0x17000DDB RID: 3547
		// (get) Token: 0x060057AE RID: 22446 RVA: 0x002CF454 File Offset: 0x002CD854
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.text;
			}
		}

		// Token: 0x17000DDC RID: 3548
		// (get) Token: 0x060057AF RID: 22447 RVA: 0x002CF470 File Offset: 0x002CD870
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.startingTick;
			}
		}

		// Token: 0x17000DDD RID: 3549
		// (get) Token: 0x060057B0 RID: 22448 RVA: 0x002CF48C File Offset: 0x002CD88C
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return !Messages.IsLive(this);
			}
		}

		// Token: 0x17000DDE RID: 3550
		// (get) Token: 0x060057B1 RID: 22449 RVA: 0x002CF4AC File Offset: 0x002CD8AC
		LookTargets IArchivable.LookTargets
		{
			get
			{
				return this.lookTargets;
			}
		}

		// Token: 0x060057B2 RID: 22450 RVA: 0x002CF4C8 File Offset: 0x002CD8C8
		public void ExposeData()
		{
			Scribe_Defs.Look<MessageTypeDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.ID, "ID", 0, false);
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<float>(ref this.startingTime, "startingTime", 0f, false);
			Scribe_Values.Look<int>(ref this.startingFrame, "startingFrame", 0, false);
			Scribe_Values.Look<int>(ref this.startingTick, "startingTick", 0, false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", new object[0]);
		}

		// Token: 0x060057B3 RID: 22451 RVA: 0x002CF55C File Offset: 0x002CD95C
		public Rect CalculateRect(float x, float y)
		{
			Text.Font = GameFont.Small;
			if (this.cachedSize.x < 0f)
			{
				this.cachedSize = Text.CalcSize(this.text);
			}
			this.lastDrawRect = new Rect(x, y, this.cachedSize.x, this.cachedSize.y);
			this.lastDrawRect = this.lastDrawRect.ContractedBy(-2f);
			return this.lastDrawRect;
		}

		// Token: 0x060057B4 RID: 22452 RVA: 0x002CF5DC File Offset: 0x002CD9DC
		public void Draw(int xOffset, int yOffset)
		{
			Rect rect = this.CalculateRect((float)xOffset, (float)yOffset);
			Find.WindowStack.ImmediateWindow(Gen.HashCombineInt(this.ID, 45574281), rect, WindowLayer.Super, delegate
			{
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.MiddleLeft;
				Rect rect = rect.AtZero();
				float alpha = this.Alpha;
				GUI.color = new Color(1f, 1f, 1f, alpha);
				if (Message.ShouldDrawBackground)
				{
					GUI.color = new Color(0.15f, 0.15f, 0.15f, 0.8f * alpha);
					GUI.DrawTexture(rect, BaseContent.WhiteTex);
					GUI.color = new Color(1f, 1f, 1f, alpha);
				}
				if (CameraJumper.CanJump(this.lookTargets.TryGetPrimaryTarget()))
				{
					UIHighlighter.HighlightOpportunity(rect, "Messages");
					Widgets.DrawHighlightIfMouseover(rect);
				}
				Rect rect2 = new Rect(2f, 0f, rect.width - 2f, rect.height);
				Widgets.Label(rect2, this.text);
				if (Current.ProgramState == ProgramState.Playing && CameraJumper.CanJump(this.lookTargets.TryGetPrimaryTarget()))
				{
					if (Widgets.ButtonInvisible(rect, false))
					{
						CameraJumper.TryJumpAndSelect(this.lookTargets.TryGetPrimaryTarget());
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.ClickingMessages, KnowledgeAmount.Total);
					}
				}
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				if (Mouse.IsOver(rect))
				{
					Messages.Notify_Mouseover(this);
				}
			}, false, false, 0f);
		}

		// Token: 0x060057B5 RID: 22453 RVA: 0x002CF63C File Offset: 0x002CDA3C
		void IArchivable.OpenArchived()
		{
			Find.WindowStack.Add(new Dialog_MessageBox(this.text, null, null, null, null, null, false, null, null));
		}

		// Token: 0x040039F8 RID: 14840
		public MessageTypeDef def;

		// Token: 0x040039F9 RID: 14841
		private int ID;

		// Token: 0x040039FA RID: 14842
		public string text;

		// Token: 0x040039FB RID: 14843
		private float startingTime;

		// Token: 0x040039FC RID: 14844
		public int startingFrame;

		// Token: 0x040039FD RID: 14845
		public int startingTick;

		// Token: 0x040039FE RID: 14846
		public LookTargets lookTargets;

		// Token: 0x040039FF RID: 14847
		private Vector2 cachedSize = new Vector2(-1f, -1f);

		// Token: 0x04003A00 RID: 14848
		public Rect lastDrawRect;

		// Token: 0x04003A01 RID: 14849
		private const float DefaultMessageLifespan = 13f;

		// Token: 0x04003A02 RID: 14850
		private const float FadeoutDuration = 0.6f;
	}
}
