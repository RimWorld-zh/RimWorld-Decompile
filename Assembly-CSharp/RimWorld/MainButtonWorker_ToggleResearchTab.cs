using System;
using Verse;

namespace RimWorld
{
	public class MainButtonWorker_ToggleResearchTab : MainButtonWorker_ToggleTab
	{
		public MainButtonWorker_ToggleResearchTab()
		{
		}

		public override float ButtonBarPercent
		{
			get
			{
				ResearchProjectDef currentProj = Find.ResearchManager.currentProj;
				float result;
				if (currentProj == null)
				{
					result = 0f;
				}
				else
				{
					result = currentProj.ProgressPercent;
				}
				return result;
			}
		}
	}
}
