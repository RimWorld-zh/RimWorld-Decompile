using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020002D4 RID: 724
	public class SkillDef : Def
	{
		// Token: 0x06000BFC RID: 3068 RVA: 0x0006A570 File Offset: 0x00068970
		public override void PostLoad()
		{
			if (this.label == null)
			{
				this.label = this.skillLabel;
			}
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x0006A58C File Offset: 0x0006898C
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

		// Token: 0x04000735 RID: 1845
		[MustTranslate]
		public string skillLabel;

		// Token: 0x04000736 RID: 1846
		public bool usuallyDefinedInBackstories = true;

		// Token: 0x04000737 RID: 1847
		public bool pawnCreatorSummaryVisible = false;

		// Token: 0x04000738 RID: 1848
		public WorkTags disablingWorkTags = WorkTags.None;
	}
}
