using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000BB8 RID: 3000
	public class WorkTypeDef : Def
	{
		// Token: 0x060040F2 RID: 16626 RVA: 0x00224874 File Offset: 0x00222C74
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return e;
			}
			if (this.naturalPriority < 0 || this.naturalPriority > 10000)
			{
				yield return "naturalPriority is " + this.naturalPriority + ", but it must be between 0 and 10000";
			}
			yield break;
		}

		// Token: 0x060040F3 RID: 16627 RVA: 0x002248A0 File Offset: 0x00222CA0
		public override void ResolveReferences()
		{
			foreach (WorkGiverDef item in from d in DefDatabase<WorkGiverDef>.AllDefs
			where d.workType == this
			orderby d.priorityInType descending
			select d)
			{
				this.workGiversByPriority.Add(item);
			}
		}

		// Token: 0x060040F4 RID: 16628 RVA: 0x00224934 File Offset: 0x00222D34
		public override int GetHashCode()
		{
			return Gen.HashCombine<string>(this.defName.GetHashCode(), this.gerundLabel);
		}

		// Token: 0x04002C60 RID: 11360
		public WorkTags workTags;

		// Token: 0x04002C61 RID: 11361
		[MustTranslate]
		public string labelShort;

		// Token: 0x04002C62 RID: 11362
		[MustTranslate]
		public string pawnLabel;

		// Token: 0x04002C63 RID: 11363
		[MustTranslate]
		public string gerundLabel;

		// Token: 0x04002C64 RID: 11364
		[MustTranslate]
		public string verb;

		// Token: 0x04002C65 RID: 11365
		public bool visible = true;

		// Token: 0x04002C66 RID: 11366
		public int naturalPriority = 0;

		// Token: 0x04002C67 RID: 11367
		public bool alwaysStartActive = false;

		// Token: 0x04002C68 RID: 11368
		public bool requireCapableColonist = false;

		// Token: 0x04002C69 RID: 11369
		public List<SkillDef> relevantSkills = new List<SkillDef>();

		// Token: 0x04002C6A RID: 11370
		[Unsaved]
		public List<WorkGiverDef> workGiversByPriority = new List<WorkGiverDef>();
	}
}
