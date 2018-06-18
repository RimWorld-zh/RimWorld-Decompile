using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000839 RID: 2105
	public class Page_SelectStoryteller : Page
	{
		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06002F9E RID: 12190 RVA: 0x001978AC File Offset: 0x00195CAC
		public override string PageTitle
		{
			get
			{
				return "ChooseAIStoryteller".Translate();
			}
		}

		// Token: 0x06002F9F RID: 12191 RVA: 0x001978CC File Offset: 0x00195CCC
		public override void PreOpen()
		{
			base.PreOpen();
			this.storyteller = (from d in DefDatabase<StorytellerDef>.AllDefs
			where d.listVisible
			orderby d.listOrder
			select d).First<StorytellerDef>();
		}

		// Token: 0x06002FA0 RID: 12192 RVA: 0x00197934 File Offset: 0x00195D34
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			Rect mainRect = base.GetMainRect(rect, 0f, false);
			StorytellerUI.DrawStorytellerSelectionInterface(mainRect, ref this.storyteller, ref this.difficulty, this.selectedStorytellerInfoListing);
			base.DoBottomButtons(rect, null, null, null, true);
		}

		// Token: 0x06002FA1 RID: 12193 RVA: 0x0019797C File Offset: 0x00195D7C
		protected override bool CanDoNext()
		{
			bool result;
			if (!base.CanDoNext())
			{
				result = false;
			}
			else
			{
				if (this.difficulty == null)
				{
					if (!Prefs.DevMode)
					{
						Messages.Message("MustChooseDifficulty".Translate(), MessageTypeDefOf.RejectInput, false);
						return false;
					}
					Messages.Message("Difficulty has been automatically selected (debug mode only)", MessageTypeDefOf.SilentInput, false);
					this.difficulty = DifficultyDefOf.Hard;
				}
				Current.Game.storyteller = new Storyteller(this.storyteller, this.difficulty);
				result = true;
			}
			return result;
		}

		// Token: 0x040019B5 RID: 6581
		private StorytellerDef storyteller;

		// Token: 0x040019B6 RID: 6582
		private DifficultyDef difficulty;

		// Token: 0x040019B7 RID: 6583
		private Listing_Standard selectedStorytellerInfoListing = new Listing_Standard();
	}
}
