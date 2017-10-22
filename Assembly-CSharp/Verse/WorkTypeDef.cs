using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	public class WorkTypeDef : Def
	{
		public WorkTags workTags;

		[MustTranslate]
		public string labelShort;

		[MustTranslate]
		public string pawnLabel;

		[MustTranslate]
		public string gerundLabel;

		[MustTranslate]
		public string verb;

		public bool visible = true;

		public int naturalPriority;

		public bool alwaysStartActive;

		public bool requireCapableColonist;

		public List<SkillDef> relevantSkills = new List<SkillDef>();

		[Unsaved]
		public List<WorkGiverDef> workGiversByPriority = new List<WorkGiverDef>();

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string item in base.ConfigErrors())
			{
				yield return item;
			}
			if (this.naturalPriority >= 0 && this.naturalPriority <= 10000)
				yield break;
			yield return "naturalPriority is " + this.naturalPriority + ", but it must be between 0 and 10000";
		}

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

		public override int GetHashCode()
		{
			return Gen.HashCombine(base.defName.GetHashCode(), this.gerundLabel);
		}
	}
}
