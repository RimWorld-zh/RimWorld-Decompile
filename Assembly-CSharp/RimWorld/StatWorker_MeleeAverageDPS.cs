using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C5 RID: 2501
	public class StatWorker_MeleeAverageDPS : StatWorker
	{
		// Token: 0x0600380B RID: 14347 RVA: 0x001DDB2C File Offset: 0x001DBF2C
		public override bool ShouldShowFor(StatRequest req)
		{
			ThingDef thingDef = req.Def as ThingDef;
			return thingDef != null && thingDef.IsWeapon && !thingDef.tools.NullOrEmpty<Tool>();
		}

		// Token: 0x0600380C RID: 14348 RVA: 0x001DDB70 File Offset: 0x001DBF70
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			Thing thing = req.Thing;
			IThingHolder thingHolder = (thing != null) ? thing.ParentHolder : null;
			List<Verb_MeleeAttack> list = null;
			if (req.HasThing && req.Thing.TryGetComp<CompEquippable>() != null)
			{
				list = req.Thing.TryGetComp<CompEquippable>().AllVerbs.OfType<Verb_MeleeAttack>().ToList<Verb_MeleeAttack>();
			}
			if (list == null && req.Def is ThingDef)
			{
				list = ((ThingDef)req.Def).GetConcreteExample(req.StuffDef).TryGetComp<CompEquippable>().AllVerbs.OfType<Verb_MeleeAttack>().ToList<Verb_MeleeAttack>();
			}
			float num = list.AverageWeighted((Verb_MeleeAttack verb) => verb.verbProps.AdjustedMeleeSelectionWeight(verb, thingHolder as Pawn, thing), (Verb_MeleeAttack verb) => verb.verbProps.AdjustedMeleeDamageAmount(verb, thingHolder as Pawn, thing));
			float num2 = list.AverageWeighted((Verb_MeleeAttack verb) => verb.verbProps.AdjustedMeleeSelectionWeight(verb, thingHolder as Pawn, thing), (Verb_MeleeAttack verb) => verb.verbProps.AdjustedCooldown(verb, thingHolder as Pawn, thing));
			return num / num2;
		}

		// Token: 0x0600380D RID: 14349 RVA: 0x001DDC7C File Offset: 0x001DC07C
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			StringBuilder stringBuilder = new StringBuilder();
			ThingDef thingDef = req.Def as ThingDef;
			for (int i = 0; i < thingDef.tools.Count; i++)
			{
				Tool tool = thingDef.tools[i];
				for (int j = 0; j < tool.capacities.Count; j++)
				{
					ToolCapacityDef capacity = tool.capacities[j];
					IEnumerable<ManeuverDef> source = from maneuver in DefDatabase<ManeuverDef>.AllDefsListForReading
					where maneuver.requiredCapacity == capacity
					select maneuver;
					if (source.Count<ManeuverDef>() != 1)
					{
						Log.ErrorOnce(string.Format("{0} maneuvers when trying to get dps for weapon {1} tool {2} capacity {3}; average DPS explanation may be incorrect", new object[]
						{
							source.Count<ManeuverDef>(),
							thingDef.label,
							tool.Id,
							capacity.defName
						}), 40417826, false);
					}
					ManeuverDef maneuverDef = source.FirstOrDefault<ManeuverDef>();
					if (maneuverDef != null)
					{
						stringBuilder.AppendLine(string.Format("  {0}: {1} ({2})", "Tool".Translate(), tool.Id.CapitalizeFirst(), capacity.defName));
						stringBuilder.AppendLine(string.Format("    {0} {1}", tool.AdjustedMeleeDamageAmount(req.Thing, maneuverDef.verb.meleeDamageDef).ToString("F1"), "DamageLower".Translate()));
						stringBuilder.AppendLine(string.Format("    {0} {1}", tool.AdjustedCooldown(req.Thing).ToString("F2"), "SecondsPerAttackLower".Translate()));
						stringBuilder.AppendLine();
					}
				}
			}
			return stringBuilder.ToString();
		}
	}
}
