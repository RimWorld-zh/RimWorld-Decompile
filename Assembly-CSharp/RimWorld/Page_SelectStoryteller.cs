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
		// (get) Token: 0x06002F9C RID: 12188 RVA: 0x00197818 File Offset: 0x00195C18
		public override string PageTitle
		{
			get
			{
				return "ChooseAIStoryteller".Translate();
			}
		}

		// Token: 0x06002F9D RID: 12189 RVA: 0x00197838 File Offset: 0x00195C38
		public override void PreOpen()
		{
			base.PreOpen();
			this.storyteller = (from d in DefDatabase<StorytellerDef>.AllDefs
			where d.listVisible
			orderby d.listOrder
			select d).First<StorytellerDef>();
		}

		// Token: 0x06002F9E RID: 12190 RVA: 0x001978A0 File Offset: 0x00195CA0
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			Rect mainRect = base.GetMainRect(rect, 0f, false);
			StorytellerUI.DrawStorytellerSelectionInterface(mainRect, ref this.storyteller, ref this.difficulty, this.selectedStorytellerInfoListing);
			base.DoBottomButtons(rect, null, null, null, true);
		}

		// Token: 0x06002F9F RID: 12191 RVA: 0x001978E8 File Offset: 0x00195CE8
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
