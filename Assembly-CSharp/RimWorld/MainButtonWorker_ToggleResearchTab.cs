using Verse;

namespace RimWorld
{
	public class MainButtonWorker_ToggleResearchTab : MainButtonWorker_ToggleTab
	{
		public override float ButtonBarPercent
		{
			get
			{
				ResearchProjectDef currentProj = Find.ResearchManager.currentProj;
				return (float)((currentProj != null) ? currentProj.ProgressPercent : 0.0);
			}
		}
	}
}
