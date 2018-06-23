using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000835 RID: 2101
	public class Page_SelectStoryteller : Page
	{
		// Token: 0x040019B3 RID: 6579
		private StorytellerDef storyteller;

		// Token: 0x040019B4 RID: 6580
		private DifficultyDef difficulty;

		// Token: 0x040019B5 RID: 6581
		private Listing_Standard selectedStorytellerInfoListing = new Listing_Standard();

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06002F97 RID: 12183 RVA: 0x00197A8C File Offset: 0x00195E8C
		public override string PageTitle
		{
			get
			{
				return "ChooseAIStoryteller".Translate();
			}
		}

		// Token: 0x06002F98 RID: 12184 RVA: 0x00197AAC File Offset: 0x00195EAC
		public override void PreOpen()
		{
			base.PreOpen();
			this.storyteller = (from d in DefDatabase<StorytellerDef>.AllDefs
			where d.listVisible
			orderby d.listOrder
			select d).First<StorytellerDef>();
		}

		// Token: 0x06002F99 RID: 12185 RVA: 0x00197B14 File Offset: 0x00195F14
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			Rect mainRect = base.GetMainRect(rect, 0f, false);
			StorytellerUI.DrawStorytellerSelectionInterface(mainRect, ref this.storyteller, ref this.difficulty, this.selectedStorytellerInfoListing);
			base.DoBottomButtons(rect, null, null, null, true);
		}

		// Token: 0x06002F9A RID: 12186 RVA: 0x00197B5C File Offset: 0x00195F5C
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
	}
}
