using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public class Alert_Hypothermia : Alert_Critical
	{
		private IEnumerable<Pawn> HypothermiaDangerColonists
		{
			get
			{
				Alert_Hypothermia.<>c__Iterator183 <>c__Iterator = new Alert_Hypothermia.<>c__Iterator183();
				Alert_Hypothermia.<>c__Iterator183 expr_07 = <>c__Iterator;
				expr_07.$PC = -2;
				return expr_07;
			}
		}

		public Alert_Hypothermia()
		{
			this.defaultLabel = "AlertHypothermia".Translate();
		}

		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn current in this.HypothermiaDangerColonists)
			{
				stringBuilder.AppendLine("    " + current.NameStringShort);
			}
			return "AlertHypothermiaDesc".Translate(new object[]
			{
				stringBuilder.ToString()
			});
		}

		public override AlertReport GetReport()
		{
			Pawn pawn = this.HypothermiaDangerColonists.FirstOrDefault<Pawn>();
			if (pawn == null)
			{
				return false;
			}
			return AlertReport.CulpritIs(pawn);
		}
	}
}
