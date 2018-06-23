using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020002D4 RID: 724
	public class SkillDef : Def
	{
		// Token: 0x04000734 RID: 1844
		[MustTranslate]
		public string skillLabel;

		// Token: 0x04000735 RID: 1845
		public bool usuallyDefinedInBackstories = true;

		// Token: 0x04000736 RID: 1846
		public bool pawnCreatorSummaryVisible = false;

		// Token: 0x04000737 RID: 1847
		public WorkTags disablingWorkTags = WorkTags.None;

		// Token: 0x06000BFA RID: 3066 RVA: 0x0006A5D8 File Offset: 0x000689D8
		public override void PostLoad()
		{
			if (this.label == null)
			{
				this.label = this.skillLabel;
			}
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x0006A5F4 File Offset: 0x000689F4
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
