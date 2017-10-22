using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public abstract class StatPart
	{
		[Unsaved]
		public StatDef parentStat;

		public abstract void TransformValue(StatRequest req, ref float val);

		public abstract string ExplanationPart(StatRequest req);

		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}
	}
}
