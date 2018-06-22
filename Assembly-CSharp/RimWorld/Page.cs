using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200082C RID: 2092
	public abstract class Page : Window
	{
		// Token: 0x06002F2A RID: 12074 RVA: 0x0019393E File Offset: 0x00191D3E
		public Page()
		{
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.forceCatchAcceptAndCancelEventEvenIfUnfocused = true;
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06002F2B RID: 12075 RVA: 0x0019396C File Offset: 0x00191D6C
		public override Vector2 InitialSize
		{
			get
			{
				return Page.StandardSize;
			}
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06002F2C RID: 12076 RVA: 0x00193988 File Offset: 0x00191D88
		public virtual string PageTitle
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06002F2D RID: 12077 RVA: 0x0019399E File Offset: 0x00191D9E
		protected void DrawPageTitle(Rect rect)
		{
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(0f, 0f, rect.width, 45f), this.PageTitle);
			Text.Font = GameFont.Small;
		}

		// Token: 0x06002F2E RID: 12078 RVA: 0x001939D4 File Offset: 0x00191DD4
		protected Rect GetMainRect(Rect rect, float extraTopSpace = 0f, bool ignoreTitle = false)
		{
			float num = 0f;
			if (!ignoreTitle)
			{
				num = 45f + extraTopSpace;
			}
			return new Rect(0f, num, rect.width, rect.height - 38f - num - 17f);
		}

		// Token: 0x06002F2F RID: 12079 RVA: 0x00193A24 File Offset: 0x00191E24
		protected void DoBottomButtons(Rect rect, string nextLabel = null, string midLabel = null, Action midAct = null, bool showNext = true)
		{
			float y = rect.y + rect.height - 38f;
			Text.Font = GameFont.Small;
			string label = "Back".Translate();
			Rect rect2 = new Rect(rect.x, y, Page.BottomButSize.x, Page.BottomButSize.y);
			if (Widgets.ButtonText(rect2, label, true, false, true) || KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				if (this.CanDoBack())
				{
					this.DoBack();
				}
			}
			if (showNext)
			{
				if (nextLabel.NullOrEmpty())
				{
					nextLabel = "Next".Translate();
				}
				Rect rect3 = new Rect(rect.x + rect.width - Page.BottomButSize.x, y, Page.BottomButSize.x, Page.BottomButSize.y);
				if (Widgets.ButtonText(rect3, nextLabel, true, false, true) || KeyBindingDefOf.Accept.KeyDownEvent)
				{
					if (this.CanDoNext())
					{
						this.DoNext();
					}
				}
				UIHighlighter.HighlightOpportunity(rect3, "NextPage");
			}
			if (midAct != null)
			{
				Rect rect4 = new Rect(rect.x + rect.width / 2f - Page.BottomButSize.x / 2f, y, Page.BottomButSize.x, Page.BottomButSize.y);
				if (Widgets.ButtonText(rect4, midLabel, true, false, true))
				{
					midAct();
				}
			}
		}

		// Token: 0x06002F30 RID: 12080 RVA: 0x00193BC4 File Offset: 0x00191FC4
		protected virtual bool CanDoBack()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction("GotoPrevPage");
		}

		// Token: 0x06002F31 RID: 12081 RVA: 0x00193BFC File Offset: 0x00191FFC
		protected virtual bool CanDoNext()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction("GotoNextPage");
		}

		// Token: 0x06002F32 RID: 12082 RVA: 0x00193C34 File Offset: 0x00192034
		protected virtual void DoNext()
		{
			if (this.next != null)
			{
				Find.WindowStack.Add(this.next);
			}
			if (this.nextAct != null)
			{
				this.nextAct();
			}
			TutorSystem.Notify_Event("PageClosed");
			TutorSystem.Notify_Event("GoToNextPage");
			this.Close(true);
		}

		// Token: 0x06002F33 RID: 12083 RVA: 0x00193C98 File Offset: 0x00192098
		protected virtual void DoBack()
		{
			if (this.prev != null)
			{
				Find.WindowStack.Add(this.prev);
			}
			TutorSystem.Notify_Event("PageClosed");
			TutorSystem.Notify_Event("GoToPrevPage");
			this.Close(true);
		}

		// Token: 0x06002F34 RID: 12084 RVA: 0x00193CE8 File Offset: 0x001920E8
		public override void OnCancelKeyPressed()
		{
			if (Find.World == null || !Find.WorldRoutePlanner.Active)
			{
				if (this.CanDoBack())
				{
					this.DoBack();
				}
				else
				{
					this.Close(true);
				}
				Event.current.Use();
				base.OnCancelKeyPressed();
			}
		}

		// Token: 0x06002F35 RID: 12085 RVA: 0x00193D41 File Offset: 0x00192141
		public override void OnAcceptKeyPressed()
		{
			if (Find.World == null || !Find.WorldRoutePlanner.Active)
			{
				if (this.CanDoNext())
				{
					this.DoNext();
				}
				Event.current.Use();
			}
		}

		// Token: 0x04001978 RID: 6520
		public Page prev;

		// Token: 0x04001979 RID: 6521
		public Page next;

		// Token: 0x0400197A RID: 6522
		public Action nextAct;

		// Token: 0x0400197B RID: 6523
		public static readonly Vector2 StandardSize = new Vector2(1020f, 764f);

		// Token: 0x0400197C RID: 6524
		public const float TitleAreaHeight = 45f;

		// Token: 0x0400197D RID: 6525
		public const float BottomButHeight = 38f;

		// Token: 0x0400197E RID: 6526
		protected static readonly Vector2 BottomButSize = new Vector2(150f, 38f);
	}
}
