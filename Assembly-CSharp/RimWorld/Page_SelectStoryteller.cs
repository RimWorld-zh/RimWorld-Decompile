using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000837 RID: 2103
	public class Page_SelectStoryteller : Page
	{
		// Token: 0x040019B7 RID: 6583
		private StorytellerDef storyteller;

		// Token: 0x040019B8 RID: 6584
		private DifficultyDef difficulty;

		// Token: 0x040019B9 RID: 6585
		private Listing_Standard selectedStorytellerInfoListing = new Listing_Standard();

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06002F9A RID: 12186 RVA: 0x00197E44 File Offset: 0x00196244
		public override string PageTitle
		{
			get
			{
				return "ChooseAIStoryteller".Translate();
			}
		}

		// Token: 0x06002F9B RID: 12187 RVA: 0x00197E64 File Offset: 0x00196264
		public override void PreOpen()
		{
			base.PreOpen();
			this.storyteller = (from d in DefDatabase<StorytellerDef>.AllDefs
			where d.listVisible
			orderby d.listOrder
			select d).First<StorytellerDef>();
		}

		// Token: 0x06002F9C RID: 12188 RVA: 0x00197ECC File Offset: 0x001962CC
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			Rect mainRect = base.GetMainRect(rect, 0f, false);
			StorytellerUI.DrawStorytellerSelectionInterface(mainRect, ref this.storyteller, ref this.difficulty, this.selectedStorytellerInfoListing);
			base.DoBottomButtons(rect, null, null, null, true);
		}

		// Token: 0x06002F9D RID: 12189 RVA: 0x00197F14 File Offset: 0x00196314
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
					this.difficulty = DifficultyDefOf.ExtraHard;
				}
				Current.Game.storyteller = new Storyteller(this.storyteller, this.difficulty);
				result = true;
			}
			return result;
		}
	}
}
