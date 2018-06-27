using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	public class Alert_NeedJoySources : Alert
	{
		private static List<JoyKindDef> countedJoyKinds = new List<JoyKindDef>();

		private static List<JoyKindDef> listedJoyKinds = new List<JoyKindDef>();

		[CompilerGenerated]
		private static Func<Building, bool> <>f__am$cache0;

		public Alert_NeedJoySources()
		{
			this.defaultLabel = "NeedJoySource".Translate();
		}

		public override string GetExplanation()
		{
			Map map = this.BadMap();
			int num = Alert_NeedJoySources.JoyKindCount(map);
			string label = map.info.parent.Label;
			ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(map);
			int joyKindsNeeded = expectationDef.joyKindsNeeded;
			return "NeedJoySourceDesc".Translate(new object[]
			{
				num,
				label,
				expectationDef.label,
				joyKindsNeeded,
				Alert_NeedJoySources.JoyKindsOnMapString(map)
			});
		}

		public override AlertReport GetReport()
		{
			return this.BadMap() != null;
		}

		private Map BadMap()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (this.NeedJoySource(maps[i]))
				{
					return maps[i];
				}
			}
			return null;
		}

		private bool NeedJoySource(Map map)
		{
			bool result;
			if (!map.IsPlayerHome)
			{
				result = false;
			}
			else if (!map.mapPawns.AnyColonistSpawned)
			{
				result = false;
			}
			else
			{
				int num = Alert_NeedJoySources.JoyKindCount(map);
				int joyKindsNeeded = ExpectationsUtility.CurrentExpectationFor(map).joyKindsNeeded;
				result = (num < joyKindsNeeded);
			}
			return result;
		}

		private static IEnumerable<Thing> JoySourceBuildings(Map map)
		{
			return (from b in map.listerBuildings.allBuildingsColonist
			where b.def.building.joyKind != null
			select b).Cast<Thing>();
		}

		private static int JoyKindCount(Map map)
		{
			for (int i = 0; i < DefDatabase<JoyKindDef>.AllDefsListForReading.Count; i++)
			{
				JoyKindDef joyKindDef = DefDatabase<JoyKindDef>.AllDefsListForReading[i];
				if (!joyKindDef.needsThing)
				{
					Alert_NeedJoySources.countedJoyKinds.Add(joyKindDef);
				}
			}
			foreach (Thing thing in Alert_NeedJoySources.JoySourceBuildings(map))
			{
				if (!Alert_NeedJoySources.countedJoyKinds.Contains(thing.def.building.joyKind))
				{
					Alert_NeedJoySources.countedJoyKinds.Add(thing.def.building.joyKind);
				}
			}
			foreach (Thing thing2 in map.listerThings.ThingsInGroup(ThingRequestGroup.Drug))
			{
				if (thing2.def.IsIngestible && thing2.def.ingestible.joyKind != null && !Alert_NeedJoySources.countedJoyKinds.Contains(thing2.def.ingestible.joyKind) && !thing2.Position.Fogged(map))
				{
					Alert_NeedJoySources.countedJoyKinds.Add(thing2.def.ingestible.joyKind);
				}
			}
			foreach (Thing thing3 in map.listerThings.ThingsInGroup(ThingRequestGroup.FoodSourceNotPlantOrTree))
			{
				if (thing3.def.IsIngestible && thing3.def.ingestible.joyKind != null && !Alert_NeedJoySources.countedJoyKinds.Contains(thing3.def.ingestible.joyKind) && !thing3.Position.Fogged(map))
				{
					Alert_NeedJoySources.countedJoyKinds.Add(thing3.def.ingestible.joyKind);
				}
			}
			int count = Alert_NeedJoySources.countedJoyKinds.Count;
			Alert_NeedJoySources.countedJoyKinds.Clear();
			return count;
		}

		public static string JoyKindsOnMapString(Map map)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("AvailableRecreationTypes".Translate() + ":");
			for (int i = 0; i < DefDatabase<JoyKindDef>.AllDefsListForReading.Count; i++)
			{
				JoyKindDef joyKindDef = DefDatabase<JoyKindDef>.AllDefsListForReading[i];
				if (!joyKindDef.needsThing)
				{
					Alert_NeedJoySources.CheckAppendJoyKind(stringBuilder, null, joyKindDef, map);
				}
			}
			foreach (Thing thing in Alert_NeedJoySources.JoySourceBuildings(map))
			{
				Alert_NeedJoySources.CheckAppendJoyKind(stringBuilder, thing, thing.def.building.joyKind, map);
			}
			foreach (Thing thing2 in map.listerThings.ThingsInGroup(ThingRequestGroup.Drug))
			{
				if (thing2.def.IsIngestible && thing2.def.ingestible.joyKind != null)
				{
					Alert_NeedJoySources.CheckAppendJoyKind(stringBuilder, thing2, thing2.def.ingestible.joyKind, map);
				}
			}
			foreach (Thing thing3 in map.listerThings.ThingsInGroup(ThingRequestGroup.FoodSourceNotPlantOrTree))
			{
				if (thing3.def.IsIngestible && thing3.def.ingestible.joyKind != null)
				{
					Alert_NeedJoySources.CheckAppendJoyKind(stringBuilder, thing3, thing3.def.ingestible.joyKind, map);
				}
			}
			Alert_NeedJoySources.listedJoyKinds.Clear();
			return stringBuilder.ToString().TrimEndNewlines();
		}

		private static void CheckAppendJoyKind(StringBuilder sb, Thing t, JoyKindDef kind, Map map)
		{
			if (!Alert_NeedJoySources.listedJoyKinds.Contains(kind))
			{
				if (t == null)
				{
					sb.AppendLine("   " + kind.LabelCap);
				}
				else
				{
					if (t.def.category == ThingCategory.Item && t.Position.Fogged(map))
					{
						return;
					}
					sb.AppendLine(string.Concat(new string[]
					{
						"   ",
						kind.LabelCap,
						" (",
						t.def.label,
						")"
					}));
				}
				Alert_NeedJoySources.listedJoyKinds.Add(kind);
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Alert_NeedJoySources()
		{
		}

		[CompilerGenerated]
		private static bool <JoySourceBuildings>m__0(Building b)
		{
			return b.def.building.joyKind != null;
		}
	}
}
