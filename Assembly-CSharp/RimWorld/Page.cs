using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public abstract class Page : Window
	{
		public Page prev;

		public Page next;

		public Action nextAct;

		public static readonly Vector2 StandardSize = new Vector2(1020f, 764f);

		public const float TitleAreaHeight = 45f;

		public const float BottomButHeight = 38f;

		protected static readonly Vector2 BottomButSize = new Vector2(150f, 38f);

		public override Vector2 InitialSize
		{
			get
			{
				return Page.StandardSize;
			}
		}

		public virtual string PageTitle
		{
			get
			{
				return null;
			}
		}

		public Page()
		{
			base.forcePause = true;
			base.absorbInputAroundWindow = true;
		}

		protected void DrawPageTitle(Rect rect)
		{
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(0f, 0f, rect.width, 45f), this.PageTitle);
			Text.Font = GameFont.Small;
		}

		protected Rect GetMainRect(Rect rect, float extraTopSpace = 0f, bool ignoreTitle = false)
		{
			float num = 0f;
			if (!ignoreTitle)
			{
				num = (float)(45.0 + extraTopSpace);
			}
			return new Rect(0f, num, rect.width, (float)(rect.height - 38.0 - num - 17.0));
		}

		protected void DoBottomButtons(Rect rect, string nextLabel = null, string midLabel = null, Action midAct = null, bool showNext = true)
		{
			float num = (float)(rect.y + rect.height - 38.0);
			Text.Font = GameFont.Small;
			string label = "Back".Translate();
			float x = rect.x;
			float y = num;
			Vector2 bottomButSize = Page.BottomButSize;
			float x2 = bottomButSize.x;
			Vector2 bottomButSize2 = Page.BottomButSize;
			Rect rect2 = new Rect(x, y, x2, bottomButSize2.y);
			if (Widgets.ButtonText(rect2, label, true, false, true) && this.CanDoBack())
			{
				this.DoBack();
			}
			if (showNext)
			{
				if (nextLabel.NullOrEmpty())
				{
					nextLabel = "Next".Translate();
				}
				float num2 = rect.x + rect.width;
				Vector2 bottomButSize3 = Page.BottomButSize;
				float x3 = num2 - bottomButSize3.x;
				float y2 = num;
				Vector2 bottomButSize4 = Page.BottomButSize;
				float x4 = bottomButSize4.x;
				Vector2 bottomButSize5 = Page.BottomButSize;
				Rect rect3 = new Rect(x3, y2, x4, bottomButSize5.y);
				if (Widgets.ButtonText(rect3, nextLabel, true, false, true) && this.CanDoNext())
				{
					this.DoNext();
				}
				UIHighlighter.HighlightOpportunity(rect3, "NextPage");
			}
			if (midAct != null)
			{
				double num3 = rect.x + rect.width / 2.0;
				Vector2 bottomButSize6 = Page.BottomButSize;
				double x5 = num3 - bottomButSize6.x / 2.0;
				float y3 = num;
				Vector2 bottomButSize7 = Page.BottomButSize;
				float x6 = bottomButSize7.x;
				Vector2 bottomButSize8 = Page.BottomButSize;
				Rect rect4 = new Rect((float)x5, y3, x6, bottomButSize8.y);
				if (Widgets.ButtonText(rect4, midLabel, true, false, true))
				{
					midAct();
				}
			}
		}

		protected virtual bool CanDoBack()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction("GotoPrevPage");
		}

		protected virtual bool CanDoNext()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction("GotoNextPage");
		}

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
	}
}
