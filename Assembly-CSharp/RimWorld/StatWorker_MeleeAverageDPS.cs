using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	public class StatWorker_MeleeAverageDPS : StatWorker
	{
		public StatWorker_MeleeAverageDPS()
		{
		}

		public override bool ShouldShowFor(StatRequest req)
		{
			ThingDef thingDef = req.Def as ThingDef;
			return thingDef != null && thingDef.IsWeapon && !thingDef.tools.NullOrEmpty<Tool>();
		}

		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			Thing thing = req.Thing;
			Pawn_EquipmentTracker pawn_EquipmentTracker = (thing == null) ? null : (thing.ParentHolder as Pawn_EquipmentTracker);
			Pawn attacker = (pawn_EquipmentTracker == null) ? null : pawn_EquipmentTracker.pawn;
			IEnumerable<Verb_MeleeAttack> enumerable = null;
			if (req.HasThing && req.Thing.TryGetComp<CompEquippable>() != null)
			{
				enumerable = req.Thing.TryGetComp<CompEquippable>().AllVerbs.OfType<Verb_MeleeAttack>();
			}
			if (enumerable == null && req.Def is ThingDef)
			{
				enumerable = ((ThingDef)req.Def).GetConcreteExample(req.StuffDef).TryGetComp<CompEquippable>().AllVerbs.OfType<Verb_MeleeAttack>();
			}
			float num = enumerable.AverageWeighted((Verb_MeleeAttack verb) => verb.verbProps.AdjustedMeleeSelectionWeight(verb, attacker, thing), (Verb_MeleeAttack verb) => verb.verbProps.AdjustedMeleeDamageAmount(verb, attacker, thing));
			float num2 = enumerable.AverageWeighted((Verb_MeleeAttack verb) => verb.verbProps.AdjustedMeleeSelectionWeight(verb, attacker, thing), (Verb_MeleeAttack verb) => verb.verbProps.AdjustedCooldown(verb, attacker, thing));
			return num / num2;
		}

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
						stringBuilder.AppendLine(string.Format("  {0}: {1} ({2})", "Tool".Translate(), tool.LabelCap, capacity.defName));
						stringBuilder.AppendLine(string.Format("    {0} {1}", tool.AdjustedBaseMeleeDamageAmount(req.Thing, maneuverDef.verb.meleeDamageDef).ToString("F1"), "DamageLower".Translate()));
						stringBuilder.AppendLine(string.Format("    {0} {1}", tool.AdjustedCooldown(req.Thing).ToString("F2"), "SecondsPerAttackLower".Translate()));
						stringBuilder.AppendLine();
					}
				}
			}
			return stringBuilder.ToString();
		}

		[CompilerGenerated]
		private sealed class <GetValueUnfinalized>c__AnonStorey0
		{
			internal Pawn attacker;

			internal Thing thing;

			public <GetValueUnfinalized>c__AnonStorey0()
			{
			}

			internal float <>m__0(Verb_MeleeAttack verb)
			{
				return verb.verbProps.AdjustedMeleeSelectionWeight(verb, this.attacker, this.thing);
			}

			internal float <>m__1(Verb_MeleeAttack verb)
			{
				return verb.verbProps.AdjustedMeleeDamageAmount(verb, this.attacker, this.thing);
			}

			internal float <>m__2(Verb_MeleeAttack verb)
			{
				return verb.verbProps.AdjustedMeleeSelectionWeight(verb, this.attacker, this.thing);
			}

			internal float <>m__3(Verb_MeleeAttack verb)
			{
				return verb.verbProps.AdjustedCooldown(verb, this.attacker, this.thing);
			}
		}

		[CompilerGenerated]
		private sealed class <GetExplanationUnfinalized>c__AnonStorey1
		{
			internal ToolCapacityDef capacity;

			public <GetExplanationUnfinalized>c__AnonStorey1()
			{
			}

			internal bool <>m__0(ManeuverDef maneuver)
			{
				return maneuver.requiredCapacity == this.capacity;
			}
		}
	}
}
