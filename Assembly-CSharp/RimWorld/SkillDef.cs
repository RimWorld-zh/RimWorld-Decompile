using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class SkillDef : Def
	{
		public string skillLabel;

		public bool usuallyDefinedInBackstories = true;

		public bool pawnCreatorSummaryVisible = false;

		public WorkTags disablingWorkTags = WorkTags.None;

		public override void PostLoad()
		{
			if (base.label == null)
			{
				base.label = this.skillLabel;
			}
		}

		public bool IsDisabled(WorkTags combinedDisabledWorkTags, IEnumerable<WorkTypeDef> disabledWorkTypes)
		{
			bool result;
			if ((combinedDisabledWorkTags & this.disablingWorkTags) != 0)
			{
				result = true;
			}
			else
			{
				List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
				bool flag = false;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					WorkTypeDef workTypeDef = allDefsListForReading[i];
					for (int j = 0; j < workTypeDef.relevantSkills.Count; j++)
					{
						if (workTypeDef.relevantSkills[j] == this)
						{
							if (!disabledWorkTypes.Contains(workTypeDef))
								goto IL_0059;
							flag = true;
						}
					}
				}
				result = ((byte)(flag ? 1 : 0) != 0);
			}
			goto IL_00a2;
			IL_00a2:
			return result;
			IL_0059:
			result = false;
			goto IL_00a2;
		}
	}
}
