using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000BB7 RID: 2999
	public class WorkTypeDef : Def
	{
		// Token: 0x04002C6C RID: 11372
		public WorkTags workTags;

		// Token: 0x04002C6D RID: 11373
		[MustTranslate]
		public string labelShort;

		// Token: 0x04002C6E RID: 11374
		[MustTranslate]
		public string pawnLabel;

		// Token: 0x04002C6F RID: 11375
		[MustTranslate]
		public string gerundLabel;

		// Token: 0x04002C70 RID: 11376
		[MustTranslate]
		public string verb;

		// Token: 0x04002C71 RID: 11377
		public bool visible = true;

		// Token: 0x04002C72 RID: 11378
		public int naturalPriority = 0;

		// Token: 0x04002C73 RID: 11379
		public bool alwaysStartActive = false;

		// Token: 0x04002C74 RID: 11380
		public bool requireCapableColonist = false;

		// Token: 0x04002C75 RID: 11381
		public List<SkillDef> relevantSkills = new List<SkillDef>();

		// Token: 0x04002C76 RID: 11382
		[Unsaved]
		public List<WorkGiverDef> workGiversByPriority = new List<WorkGiverDef>();

		// Token: 0x060040F9 RID: 16633 RVA: 0x002253D8 File Offset: 0x002237D8
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

		// Token: 0x060040FA RID: 16634 RVA: 0x00225404 File Offset: 0x00223804
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

		// Token: 0x060040FB RID: 16635 RVA: 0x00225498 File Offset: 0x00223898
		public override int GetHashCode()
		{
			return Gen.HashCombine<string>(this.defName.GetHashCode(), this.gerundLabel);
		}
	}
}
