using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	public class StatWorker_MeleeAverageArmorPenetration : StatWorker
	{
		[CompilerGenerated]
		private static Func<VerbUtility.VerbPropertiesWithSource, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<VerbUtility.VerbPropertiesWithSource, bool> <>f__am$cache1;

		public StatWorker_MeleeAverageArmorPenetration()
		{
		}

		public override bool ShouldShowFor(StatRequest req)
		{
			ThingDef thingDef = req.Def as ThingDef;
			return thingDef != null && thingDef.IsWeapon && !thingDef.tools.NullOrEmpty<Tool>();
		}

		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			ThingDef thingDef = req.Def as ThingDef;
			if (thingDef == null)
			{
				return 0f;
			}
			Pawn attacker = StatWorker_MeleeAverageDPS.GetCurrentWeaponUser(req.Thing);
			return (from x in VerbUtility.GetAllVerbProperties(thingDef.Verbs, thingDef.tools)
			where x.verbProps.IsMeleeAttack
			select x).AverageWeighted((VerbUtility.VerbPropertiesWithSource x) => x.verbProps.AdjustedMeleeSelectionWeight(x.tool, attacker, req.Thing, null, false), (VerbUtility.VerbPropertiesWithSource x) => x.verbProps.AdjustedArmorPenetration(x.tool, attacker, req.Thing, null));
		}

		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			ThingDef thingDef = req.Def as ThingDef;
			if (thingDef == null)
			{
				return null;
			}
			Pawn currentWeaponUser = StatWorker_MeleeAverageDPS.GetCurrentWeaponUser(req.Thing);
			IEnumerable<VerbUtility.VerbPropertiesWithSource> enumerable = from x in VerbUtility.GetAllVerbProperties(thingDef.Verbs, thingDef.tools)
			where x.verbProps.IsMeleeAttack
			select x;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (VerbUtility.VerbPropertiesWithSource verbPropertiesWithSource in enumerable)
			{
				float f = verbPropertiesWithSource.verbProps.AdjustedArmorPenetration(verbPropertiesWithSource.tool, currentWeaponUser, req.Thing, null);
				if (verbPropertiesWithSource.tool != null)
				{
					stringBuilder.AppendLine(string.Format("  {0}: {1} ({2})", "Tool".Translate(), verbPropertiesWithSource.tool.LabelCap, verbPropertiesWithSource.ToolCapacity.defName));
				}
				else
				{
					stringBuilder.AppendLine(string.Format("  {0}:", "StatsReport_NonToolAttack".Translate()));
				}
				stringBuilder.AppendLine("    " + f.ToStringPercent());
				stringBuilder.AppendLine();
			}
			return stringBuilder.ToString();
		}

		[CompilerGenerated]
		private static bool <GetValueUnfinalized>m__0(VerbUtility.VerbPropertiesWithSource x)
		{
			return x.verbProps.IsMeleeAttack;
		}

		[CompilerGenerated]
		private static bool <GetExplanationUnfinalized>m__1(VerbUtility.VerbPropertiesWithSource x)
		{
			return x.verbProps.IsMeleeAttack;
		}

		[CompilerGenerated]
		private sealed class <GetValueUnfinalized>c__AnonStorey0
		{
			internal Pawn attacker;

			internal StatRequest req;

			public <GetValueUnfinalized>c__AnonStorey0()
			{
			}

			internal float <>m__0(VerbUtility.VerbPropertiesWithSource x)
			{
				return x.verbProps.AdjustedMeleeSelectionWeight(x.tool, this.attacker, this.req.Thing, null, false);
			}

			internal float <>m__1(VerbUtility.VerbPropertiesWithSource x)
			{
				return x.verbProps.AdjustedArmorPenetration(x.tool, this.attacker, this.req.Thing, null);
			}
		}
	}
}
