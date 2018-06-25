using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class SkillDef : Def
	{
		[MustTranslate]
		public string skillLabel;

		public bool usuallyDefinedInBackstories = true;

		public bool pawnCreatorSummaryVisible = false;

		public WorkTags disablingWorkTags = WorkTags.None;

		public float listOrder = 0f;

		public SkillDef()
		{
		}

		public override void PostLoad()
		{
			if (this.label == null)
			{
				this.label = this.skillLabel;
			}
		}

		public bool IsDisabled(WorkTags combinedDisabledWorkTags, IEnumerable<WorkTypeDef> disabledWorkTypes)
		{
			bool result;
			if ((combinedDisabledWorkTags & this.disablingWorkTags) != WorkTags.None)
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
							{
								return false;
							}
							flag = true;
						}
					}
				}
				result = flag;
			}
			return result;
		}
	}
}
