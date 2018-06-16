using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200083A RID: 2106
	public class Page_SelectStorytellerInGame : Page
	{
		// Token: 0x06002FA2 RID: 12194 RVA: 0x001979BA File Offset: 0x00195DBA
		public Page_SelectStorytellerInGame()
		{
			this.doCloseButton = true;
			this.doCloseX = true;
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06002FA3 RID: 12195 RVA: 0x001979DC File Offset: 0x00195DDC
		public override string PageTitle
		{
			get
			{
				return "ChooseAIStoryteller".Translate();
			}
		}

		// Token: 0x06002FA4 RID: 12196 RVA: 0x001979FC File Offset: 0x00195DFC
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			Rect mainRect = base.GetMainRect(rect, 0f, false);
			Storyteller storyteller = Current.Game.storyteller;
			StorytellerDef def = Current.Game.storyteller.def;
			StorytellerUI.DrawStorytellerSelectionInterface(mainRect, ref storyteller.def, ref storyteller.difficulty, this.selectedStorytellerInfoListing);
			if (storyteller.def != def)
			{
				storyteller.Notify_DefChanged();
			}
		}

		// Token: 0x040019BA RID: 6586
		private Listing_Standard selectedStorytellerInfoListing = new Listing_Standard();
	}
}
