using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020002D6 RID: 726
	public class SkillDef : Def
	{
		// Token: 0x04000736 RID: 1846
		[MustTranslate]
		public string skillLabel;

		// Token: 0x04000737 RID: 1847
		public bool usuallyDefinedInBackstories = true;

		// Token: 0x04000738 RID: 1848
		public bool pawnCreatorSummaryVisible = false;

		// Token: 0x04000739 RID: 1849
		public WorkTags disablingWorkTags = WorkTags.None;

		// Token: 0x0400073A RID: 1850
		public float listOrder = 0f;

		// Token: 0x06000BFD RID: 3069 RVA: 0x0006A72F File Offset: 0x00068B2F
		public override void PostLoad()
		{
			if (this.label == null)
			{
				this.label = this.skillLabel;
			}
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x0006A74C File Offset: 0x00068B4C
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
