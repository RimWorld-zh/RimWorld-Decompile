using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public class Alert_ColonistsIdle : Alert
	{
		public const int MinDaysPassed = 1;

		private IEnumerable<Pawn> IdleColonists
		{
			get
			{
				Alert_ColonistsIdle.<>c__Iterator18A <>c__Iterator18A = new Alert_ColonistsIdle.<>c__Iterator18A();
				Alert_ColonistsIdle.<>c__Iterator18A expr_07 = <>c__Iterator18A;
				expr_07.$PC = -2;
				return expr_07;
			}
		}

		public override string GetLabel()
		{
			return string.Format("ColonistsIdle".Translate(), this.IdleColonists.Count<Pawn>().ToStringCached());
		}

		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn current in this.IdleColonists)
			{
				stringBuilder.AppendLine("    " + current.NameStringShort);
			}
			return string.Format("ColonistsIdleDesc".Translate(), stringBuilder.ToString());
		}

		public override AlertReport GetReport()
		{
			if (GenDate.DaysPassed < 1)
			{
				return AlertReport.Inactive;
			}
			return this.IdleColonists.FirstOrDefault<Pawn>();
		}
	}
}
