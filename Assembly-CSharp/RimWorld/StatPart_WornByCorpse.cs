using Verse;

namespace RimWorld
{
	public class StatPart_WornByCorpse : StatPart
	{
		private const float Factor = 0.1f;

		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Apparel apparel = req.Thing as Apparel;
				if (apparel != null && apparel.WornByCorpse)
				{
					val *= 0.1f;
				}
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
			string result;
			if (req.HasThing)
			{
				Apparel apparel = req.Thing as Apparel;
				if (apparel != null && apparel.WornByCorpse)
				{
					result = "StatsReport_WornByCorpse".Translate() + ": x" + 0.1f.ToStringPercent();
					goto IL_0058;
				}
			}
			result = (string)null;
			goto IL_0058;
			IL_0058:
			return result;
		}
	}
}
