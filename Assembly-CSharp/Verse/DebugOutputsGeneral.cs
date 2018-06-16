using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E1B RID: 3611
	[HasDebugOutput]
	internal static class DebugOutputsGeneral
	{
		// Token: 0x06005275 RID: 21109 RVA: 0x002A2D8C File Offset: 0x002A118C
		[DebugOutput]
		public static void WeaponsRanged()
		{
			Func<ThingDef, int> damage = (ThingDef d) => (d.Verbs[0].defaultProjectile == null) ? 0 : d.Verbs[0].defaultProjectile.projectile.DamageAmount;
			Func<ThingDef, float> stoppingPower = (ThingDef d) => (d.Verbs[0].defaultProjectile == null) ? 0f : d.Verbs[0].defaultProjectile.projectile.stoppingPower;
			Func<ThingDef, float> warmup = (ThingDef d) => d.Verbs[0].warmupTime;
			Func<ThingDef, float> cooldown = (ThingDef d) => d.GetStatValueAbstract(StatDefOf.RangedWeapon_Cooldown, null);
			Func<ThingDef, int> burstShots = (ThingDef d) => d.Verbs[0].burstShotCount;
			Func<ThingDef, float> fullcycle = (ThingDef d) => warmup(d) + cooldown(d) + ((d.Verbs[0].burstShotCount - 1) * d.Verbs[0].ticksBetweenBurstShots).TicksToSeconds();
			Func<ThingDef, float> dpsMissless = delegate(ThingDef d)
			{
				int num = burstShots(d);
				float num2 = warmup(d) + cooldown(d);
				num2 += (float)(num - 1) * ((float)d.Verbs[0].ticksBetweenBurstShots / 60f);
				return (float)(damage(d) * num) / num2;
			};
			Func<ThingDef, float> accTouch = (ThingDef d) => d.GetStatValueAbstract(StatDefOf.AccuracyTouch, null);
			Func<ThingDef, float> accShort = (ThingDef d) => d.GetStatValueAbstract(StatDefOf.AccuracyShort, null);
			Func<ThingDef, float> accMed = (ThingDef d) => d.GetStatValueAbstract(StatDefOf.AccuracyMedium, null);
			Func<ThingDef, float> accLong = (ThingDef d) => d.GetStatValueAbstract(StatDefOf.AccuracyLong, null);
			Func<ThingDef, float> accAvg = (ThingDef d) => (accTouch(d) + accShort(d) + accMed(d) + accLong(d)) / 4f;
			Func<ThingDef, float> dpsAvg = (ThingDef d) => dpsMissless(d) * accAvg(d);
			IEnumerable<ThingDef> dataSources = (from d in DefDatabase<ThingDef>.AllDefs
			where d.IsRangedWeapon
			select d).OrderByDescending(dpsAvg);
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[24];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("damage", (ThingDef d) => damage(d).ToString());
			array[2] = new TableDataGetter<ThingDef>("stop\npower", (ThingDef d) => (stoppingPower(d) <= 0f) ? "" : stoppingPower(d).ToString("F1"));
			array[3] = new TableDataGetter<ThingDef>("warmup", (ThingDef d) => warmup(d).ToString("F2"));
			array[4] = new TableDataGetter<ThingDef>("burst\nshots", (ThingDef d) => burstShots(d).ToString());
			array[5] = new TableDataGetter<ThingDef>("cooldown", (ThingDef d) => cooldown(d).ToString("F2"));
			array[6] = new TableDataGetter<ThingDef>("full\ncycle", (ThingDef d) => fullcycle(d).ToString("F2"));
			array[7] = new TableDataGetter<ThingDef>("range", (ThingDef d) => d.Verbs[0].range.ToString("F1"));
			array[8] = new TableDataGetter<ThingDef>("projectile\nspeed", (ThingDef d) => (d.projectile == null) ? "" : d.projectile.speed.ToString("F0"));
			array[9] = new TableDataGetter<ThingDef>("dps\nmissless", (ThingDef d) => dpsMissless(d).ToString("F2"));
			array[10] = new TableDataGetter<ThingDef>("accuracy\ntouch (" + 4f + ")", (ThingDef d) => accTouch(d).ToStringPercent());
			array[11] = new TableDataGetter<ThingDef>("accuracy\nshort (" + 15f + ")", (ThingDef d) => accShort(d).ToStringPercent());
			array[12] = new TableDataGetter<ThingDef>("accuracy\nmed (" + 30f + ")", (ThingDef d) => accMed(d).ToStringPercent());
			array[13] = new TableDataGetter<ThingDef>("accuracy\nlong (" + 50f + ")", (ThingDef d) => accLong(d).ToStringPercent());
			array[14] = new TableDataGetter<ThingDef>("accuracy\navg", (ThingDef d) => accAvg(d).ToString("F2"));
			array[15] = new TableDataGetter<ThingDef>("forced\nmiss\nradius", (ThingDef d) => (d.Verbs[0].forcedMissRadius <= 0f) ? "" : d.Verbs[0].forcedMissRadius.ToString());
			array[16] = new TableDataGetter<ThingDef>("dps\ntouch", (ThingDef d) => (dpsMissless(d) * accTouch(d)).ToString("F2"));
			array[17] = new TableDataGetter<ThingDef>("dps\nshort", (ThingDef d) => (dpsMissless(d) * accShort(d)).ToString("F2"));
			array[18] = new TableDataGetter<ThingDef>("dps\nmed", (ThingDef d) => (dpsMissless(d) * accMed(d)).ToString("F2"));
			array[19] = new TableDataGetter<ThingDef>("dps\nlong", (ThingDef d) => (dpsMissless(d) * accLong(d)).ToString("F2"));
			array[20] = new TableDataGetter<ThingDef>("dps\navg", (ThingDef d) => dpsAvg(d).ToString("F2"));
			array[21] = new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.MarketValue, null).ToString("F0"));
			array[22] = new TableDataGetter<ThingDef>("work", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.WorkToMake, null).ToString("F0"));
			array[23] = new TableDataGetter<ThingDef>("dpsAvg*100 / market value", (ThingDef d) => (dpsAvg(d) * 100f / d.GetStatValueAbstract(StatDefOf.MarketValue, null)).ToString("F3"));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06005276 RID: 21110 RVA: 0x002A3280 File Offset: 0x002A1680
		[DebugOutput]
		public static void MeleeByStuff()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption("Stuffless", delegate()
			{
				DebugOutputsGeneral.DoTableInternalApparel(null);
			}, MenuOptionPriority.Default, null, null, 0f, null, null));
			foreach (ThingDef localStuff2 in from td in DefDatabase<ThingDef>.AllDefs
			where td.IsStuff
			orderby td.GetStatValueAbstract(StatDefOf.SharpDamageMultiplier, null) descending
			select td)
			{
				ThingDef localStuff = localStuff2;
				list.Add(new FloatMenuOption(string.Concat(new object[]
				{
					localStuff.defName,
					"(sharp ",
					localStuff.GetStatValueAbstract(StatDefOf.SharpDamageMultiplier, null),
					", blunt ",
					localStuff.GetStatValueAbstract(StatDefOf.BluntDamageMultiplier, null),
					")"
				}), delegate()
				{
					DebugOutputsGeneral.DoTablesInternalMelee(localStuff, false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06005277 RID: 21111 RVA: 0x002A33F8 File Offset: 0x002A17F8
		private static void DoTablesInternalMelee(ThingDef stuff, bool doRaces = false)
		{
			Func<Def, float> meleeDamageGetter = delegate(Def d)
			{
				Thing owner;
				List<Verb> concreteExampleVerbs = VerbUtility.GetConcreteExampleVerbs(d, out owner, stuff);
				float result;
				if (concreteExampleVerbs.OfType<Verb_MeleeAttack>().Any<Verb_MeleeAttack>())
				{
					result = concreteExampleVerbs.OfType<Verb_MeleeAttack>().AverageWeighted((Verb_MeleeAttack v) => v.verbProps.AdjustedMeleeSelectionWeight(v, null, owner), (Verb_MeleeAttack v) => v.verbProps.AdjustedMeleeDamageAmount(v, null, owner));
				}
				else
				{
					result = -1f;
				}
				return result;
			};
			Func<Def, float> rangedDamageGetter = delegate(Def d)
			{
				Thing thing;
				List<Verb> concreteExampleVerbs = VerbUtility.GetConcreteExampleVerbs(d, out thing, stuff);
				Verb verb = concreteExampleVerbs.OfType<Verb_LaunchProjectile>().FirstOrDefault<Verb_LaunchProjectile>();
				float result;
				if (verb != null && verb.GetProjectile() != null)
				{
					result = (float)verb.GetProjectile().projectile.DamageAmount;
				}
				else
				{
					result = -1f;
				}
				return result;
			};
			Func<Def, float> meleeWarmupGetter = (Def d) => 0f;
			Func<Def, float> rangedWarmupGetter = delegate(Def d)
			{
				Thing thing;
				List<Verb> concreteExampleVerbs = VerbUtility.GetConcreteExampleVerbs(d, out thing, stuff);
				Verb verb = concreteExampleVerbs.OfType<Verb_LaunchProjectile>().FirstOrDefault<Verb_LaunchProjectile>();
				float result;
				if (verb != null)
				{
					result = verb.verbProps.warmupTime;
				}
				else
				{
					result = -1f;
				}
				return result;
			};
			Func<Def, float> meleeCooldownGetter = delegate(Def d)
			{
				Thing owner;
				List<Verb> concreteExampleVerbs = VerbUtility.GetConcreteExampleVerbs(d, out owner, stuff);
				float result;
				if (concreteExampleVerbs.OfType<Verb_MeleeAttack>().Any<Verb_MeleeAttack>())
				{
					result = concreteExampleVerbs.OfType<Verb_MeleeAttack>().AverageWeighted((Verb_MeleeAttack v) => v.verbProps.AdjustedMeleeSelectionWeight(v, null, owner), (Verb_MeleeAttack v) => v.verbProps.AdjustedCooldown(v, null, owner));
				}
				else
				{
					result = -1f;
				}
				return result;
			};
			Func<Def, float> rangedCooldownGetter = delegate(Def d)
			{
				Thing thing;
				List<Verb> concreteExampleVerbs = VerbUtility.GetConcreteExampleVerbs(d, out thing, stuff);
				Verb verb = concreteExampleVerbs.OfType<Verb_LaunchProjectile>().FirstOrDefault<Verb_LaunchProjectile>();
				float result;
				if (verb != null)
				{
					result = verb.verbProps.defaultCooldownTime;
				}
				else
				{
					result = -1f;
				}
				return result;
			};
			Func<Def, float> meleeDpsGetter = (Def d) => meleeDamageGetter(d) / (meleeWarmupGetter(d) + meleeCooldownGetter(d));
			Func<Def, float> rangedDpsGetter = (Def d) => rangedDamageGetter(d) / (rangedWarmupGetter(d) + rangedCooldownGetter(d));
			Func<Def, float> dpsGetter = (Def d) => Mathf.Max(meleeDpsGetter(d), rangedDpsGetter(d));
			Func<Def, float> marketValueGetter = delegate(Def d)
			{
				ThingDef thingDef = d as ThingDef;
				float result;
				if (thingDef != null)
				{
					result = thingDef.GetStatValueAbstract(StatDefOf.MarketValue, stuff);
				}
				else
				{
					HediffDef hediffDef = d as HediffDef;
					if (hediffDef != null)
					{
						if (hediffDef.spawnThingOnRemoved == null)
						{
							result = 0f;
						}
						else
						{
							result = hediffDef.spawnThingOnRemoved.GetStatValueAbstract(StatDefOf.MarketValue, null);
						}
					}
					else
					{
						result = -1f;
					}
				}
				return result;
			};
			IEnumerable<Def> enumerable = (from d in DefDatabase<ThingDef>.AllDefs
			where d.IsWeapon
			select d).Cast<Def>().Concat((from h in DefDatabase<HediffDef>.AllDefs
			where h.CompProps<HediffCompProperties_VerbGiver>() != null
			select h).Cast<Def>());
			if (doRaces)
			{
				enumerable = enumerable.Concat((from d in DefDatabase<ThingDef>.AllDefs
				where d.race != null
				select d).Cast<Def>());
			}
			enumerable = from h in enumerable
			orderby dpsGetter(h) descending
			select h;
			IEnumerable<Def> dataSources = enumerable;
			TableDataGetter<Def>[] array = new TableDataGetter<Def>[12];
			array[0] = new TableDataGetter<Def>("defName", (Def d) => d.defName);
			array[1] = new TableDataGetter<Def>("m. damage", (Def d) => meleeDamageGetter(d).ToString());
			array[2] = new TableDataGetter<Def>("m. warmup", (Def d) => meleeWarmupGetter(d).ToString("F2"));
			array[3] = new TableDataGetter<Def>("m. cooldown", (Def d) => meleeCooldownGetter(d).ToString("F2"));
			array[4] = new TableDataGetter<Def>("m. dps", (Def d) => meleeDpsGetter(d).ToString("F2"));
			array[5] = new TableDataGetter<Def>("r. damage", (Def d) => rangedDamageGetter(d).ToString());
			array[6] = new TableDataGetter<Def>("r. warmup", (Def d) => rangedWarmupGetter(d).ToString("F2"));
			array[7] = new TableDataGetter<Def>("r. cooldown", (Def d) => rangedCooldownGetter(d).ToString("F2"));
			array[8] = new TableDataGetter<Def>("r. dps", (Def d) => rangedDpsGetter(d).ToString("F2"));
			array[9] = new TableDataGetter<Def>("best dps", (Def d) => dpsGetter(d).ToString("F2"));
			array[10] = new TableDataGetter<Def>("market value", (Def d) => marketValueGetter(d).ToString("F0"));
			array[11] = new TableDataGetter<Def>("work to make", delegate(Def d)
			{
				ThingDef thingDef = d as ThingDef;
				string result;
				if (thingDef == null)
				{
					result = "-";
				}
				else
				{
					result = thingDef.GetStatValueAbstract(StatDefOf.WorkToMake, stuff).ToString("F0");
				}
				return result;
			});
			DebugTables.MakeTablesDialog<Def>(dataSources, array);
		}

		// Token: 0x06005278 RID: 21112 RVA: 0x002A36D0 File Offset: 0x002A1AD0
		[DebugOutput]
		public static void ApparelByStuff()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption("Stuffless", delegate()
			{
				DebugOutputsGeneral.DoTableInternalApparel(null);
			}, MenuOptionPriority.Default, null, null, 0f, null, null));
			foreach (ThingDef localStuff2 in from td in DefDatabase<ThingDef>.AllDefs
			where td.IsStuff
			select td)
			{
				ThingDef localStuff = localStuff2;
				list.Add(new FloatMenuOption(localStuff.defName, delegate()
				{
					DebugOutputsGeneral.DoTableInternalApparel(localStuff);
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06005279 RID: 21113 RVA: 0x002A37D0 File Offset: 0x002A1BD0
		[DebugOutput]
		public static void ApparelArmor()
		{
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("label", (ThingDef x) => x.LabelCap));
			list.Add(new TableDataGetter<ThingDef>("none", delegate(ThingDef x)
			{
				string result;
				if (x.MadeFromStuff)
				{
					result = "";
				}
				else
				{
					result = string.Concat(new string[]
					{
						x.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp, null).ToStringPercent(),
						" / ",
						x.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt, null).ToStringPercent(),
						" / ",
						x.GetStatValueAbstract(StatDefOf.ArmorRating_Heat, null).ToStringPercent()
					});
				}
				return result;
			}));
			foreach (ThingDef stuffLocal2 in from x in DefDatabase<ThingDef>.AllDefs
			where x.IsStuff
			orderby x.BaseMarketValue
			select x)
			{
				ThingDef stuffLocal = stuffLocal2;
				if (DefDatabase<ThingDef>.AllDefs.Any((ThingDef x) => x.IsApparel && stuffLocal.stuffProps.CanMake(x)))
				{
					list.Add(new TableDataGetter<ThingDef>(stuffLocal.label.Shorten(), delegate(ThingDef x)
					{
						string result;
						if (!stuffLocal.stuffProps.CanMake(x))
						{
							result = "";
						}
						else
						{
							result = string.Concat(new string[]
							{
								x.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp, stuffLocal).ToStringPercent(),
								" / ",
								x.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt, stuffLocal).ToStringPercent(),
								" / ",
								x.GetStatValueAbstract(StatDefOf.ArmorRating_Heat, stuffLocal).ToStringPercent()
							});
						}
						return result;
					}));
				}
			}
			DebugTables.MakeTablesDialog<ThingDef>(from x in DefDatabase<ThingDef>.AllDefs
			where x.IsApparel
			orderby x.BaseMarketValue
			select x, list.ToArray());
		}

		// Token: 0x0600527A RID: 21114 RVA: 0x002A3974 File Offset: 0x002A1D74
		[DebugOutput]
		public static void ApparelInsulation()
		{
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("label", (ThingDef x) => x.LabelCap));
			list.Add(new TableDataGetter<ThingDef>("none", delegate(ThingDef x)
			{
				string result;
				if (x.MadeFromStuff)
				{
					result = "";
				}
				else
				{
					result = x.GetStatValueAbstract(StatDefOf.Insulation_Heat, null).ToStringTemperature("F1") + " / " + x.GetStatValueAbstract(StatDefOf.Insulation_Cold, null).ToStringTemperature("F1");
				}
				return result;
			}));
			foreach (ThingDef stuffLocal2 in from x in DefDatabase<ThingDef>.AllDefs
			where x.IsStuff
			orderby x.BaseMarketValue
			select x)
			{
				ThingDef stuffLocal = stuffLocal2;
				if (DefDatabase<ThingDef>.AllDefs.Any((ThingDef x) => x.IsApparel && stuffLocal.stuffProps.CanMake(x)))
				{
					list.Add(new TableDataGetter<ThingDef>(stuffLocal.label.Shorten(), delegate(ThingDef x)
					{
						string result;
						if (!stuffLocal.stuffProps.CanMake(x))
						{
							result = "";
						}
						else
						{
							result = x.GetStatValueAbstract(StatDefOf.Insulation_Heat, stuffLocal).ToString("F1") + ", " + x.GetStatValueAbstract(StatDefOf.Insulation_Cold, stuffLocal).ToString("F1");
						}
						return result;
					}));
				}
			}
			DebugTables.MakeTablesDialog<ThingDef>(from x in DefDatabase<ThingDef>.AllDefs
			where x.IsApparel
			orderby x.BaseMarketValue
			select x, list.ToArray());
		}

		// Token: 0x0600527B RID: 21115 RVA: 0x002A3B18 File Offset: 0x002A1F18
		private static void DoTableInternalApparel(ThingDef stuff)
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.IsApparel && (stuff == null || (d.MadeFromStuff && stuff.stuffProps.CanMake(d)))
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[14];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("bodyParts", (ThingDef d) => GenText.ToSpaceList(from bp in d.apparel.bodyPartGroups
			select bp.defName));
			array[2] = new TableDataGetter<ThingDef>("layers", (ThingDef d) => GenText.ToSpaceList(from l in d.apparel.layers
			select l.ToString()));
			array[3] = new TableDataGetter<ThingDef>("tags", (ThingDef d) => GenText.ToSpaceList(from t in d.apparel.tags
			select t.ToString()));
			array[4] = new TableDataGetter<ThingDef>("work", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.WorkToMake, stuff).ToString("F0"));
			array[5] = new TableDataGetter<ThingDef>("mktval", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.MarketValue, stuff).ToString("F0"));
			array[6] = new TableDataGetter<ThingDef>("insCold", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.Insulation_Cold, stuff).ToString("F1"));
			array[7] = new TableDataGetter<ThingDef>("insHeat", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.Insulation_Heat, stuff).ToString("F1"));
			array[8] = new TableDataGetter<ThingDef>("blunt", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt, stuff).ToString("F2"));
			array[9] = new TableDataGetter<ThingDef>("sharp", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp, stuff).ToString("F2"));
			array[10] = new TableDataGetter<ThingDef>("SEMultArmor", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffEffectMultiplierArmor, stuff).ToString("F2"));
			array[11] = new TableDataGetter<ThingDef>("SEMultInsuCold", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffEffectMultiplierInsulation_Cold, stuff).ToString("F2"));
			array[12] = new TableDataGetter<ThingDef>("SEMultInsuHeat", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffEffectMultiplierInsulation_Heat, stuff).ToString("F2"));
			array[13] = new TableDataGetter<ThingDef>("equipTime", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.EquipDelay, stuff).ToString("F1"));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x0600527C RID: 21116 RVA: 0x002A3CFC File Offset: 0x002A20FC
		[DebugOutput]
		public static void ThingsExistingList()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			IEnumerator enumerator = Enum.GetValues(typeof(ThingRequestGroup)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					ThingRequestGroup localRg2 = (ThingRequestGroup)obj;
					ThingRequestGroup localRg = localRg2;
					FloatMenuOption item = new FloatMenuOption(localRg.ToString(), delegate()
					{
						StringBuilder stringBuilder = new StringBuilder();
						List<Thing> list2 = Find.CurrentMap.listerThings.ThingsInGroup(localRg);
						stringBuilder.AppendLine(string.Concat(new object[]
						{
							"Global things in group ",
							localRg,
							" (count ",
							list2.Count,
							")"
						}));
						Log.Message(DebugLogsUtility.ThingListToUniqueCountString(list2), false);
					}, MenuOptionPriority.Default, null, null, 0f, null, null);
					list.Add(item);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x0600527D RID: 21117 RVA: 0x002A3DC4 File Offset: 0x002A21C4
		[DebugOutput]
		public static void ThingFillageAndPassability()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.passability != Traversability.Standable || thingDef.fillPercent > 0f)
				{
					stringBuilder.Append(string.Concat(new string[]
					{
						thingDef.defName,
						" - pass=",
						thingDef.passability.ToString(),
						", fill=",
						thingDef.fillPercent.ToStringPercent()
					}));
					if (thingDef.passability == Traversability.Impassable && thingDef.fillPercent < 0.1f)
					{
						stringBuilder.Append("   ALERT, impassable with low fill");
					}
					if (thingDef.passability != Traversability.Impassable && thingDef.fillPercent > 0.8f)
					{
						stringBuilder.Append("    ALERT, passabile with very high fill");
					}
					stringBuilder.AppendLine();
				}
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x0600527E RID: 21118 RVA: 0x002A3EEC File Offset: 0x002A22EC
		[DebugOutput]
		public static void ThingDamageData()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.useHitPoints
			orderby d.category, d.defName
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[7];
			array[0] = new TableDataGetter<ThingDef>("category", (ThingDef d) => d.category.ToString());
			array[1] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[2] = new TableDataGetter<ThingDef>("hp", (ThingDef d) => d.BaseMaxHitPoints.ToString());
			array[3] = new TableDataGetter<ThingDef>("flammability", (ThingDef d) => (d.BaseFlammability <= 0f) ? "" : d.BaseFlammability.ToString());
			array[4] = new TableDataGetter<ThingDef>("uses stuff", (ThingDef d) => d.MadeFromStuff.ToStringCheckBlank());
			array[5] = new TableDataGetter<ThingDef>("deterioration rate", (ThingDef d) => (d.GetStatValueAbstract(StatDefOf.DeteriorationRate, null) <= 0f) ? "" : d.GetStatValueAbstract(StatDefOf.DeteriorationRate, null).ToString());
			array[6] = new TableDataGetter<ThingDef>("days to deterioriate", (ThingDef d) => (d.GetStatValueAbstract(StatDefOf.DeteriorationRate, null) <= 0f) ? "" : ((float)d.BaseMaxHitPoints / d.GetStatValueAbstract(StatDefOf.DeteriorationRate, null)).ToString());
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x0600527F RID: 21119 RVA: 0x002A4098 File Offset: 0x002A2498
		[DebugOutput]
		public static void ThingMasses()
		{
			IOrderedEnumerable<ThingDef> orderedEnumerable = from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.category == ThingCategory.Item || x.Minifiable
			where x.thingClass != typeof(MinifiedThing) && x.thingClass != typeof(UnfinishedThing)
			orderby x.GetStatValueAbstract(StatDefOf.Mass, null), x.GetStatValueAbstract(StatDefOf.MarketValue, null)
			select x;
			Func<ThingDef, float, string> perPawn = (ThingDef d, float bodySize) => (bodySize * 35f / d.GetStatValueAbstract(StatDefOf.Mass, null)).ToString("F0");
			Func<ThingDef, string> perNutrition = delegate(ThingDef d)
			{
				string result;
				if (d.ingestible == null || d.GetStatValueAbstract(StatDefOf.Nutrition, null) == 0f)
				{
					result = "";
				}
				else
				{
					result = (d.GetStatValueAbstract(StatDefOf.Mass, null) / d.GetStatValueAbstract(StatDefOf.Nutrition, null)).ToString("F2");
				}
				return result;
			};
			IEnumerable<ThingDef> dataSources = orderedEnumerable;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[7];
			array[0] = new TableDataGetter<ThingDef>("defName", delegate(ThingDef d)
			{
				string result;
				if (d.Minifiable)
				{
					result = d.defName + " (minified)";
				}
				else
				{
					string text = d.defName;
					if (!d.EverHaulable)
					{
						text += " (not haulable)";
					}
					result = text;
				}
				return result;
			});
			array[1] = new TableDataGetter<ThingDef>("mass", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.Mass, null).ToString());
			array[2] = new TableDataGetter<ThingDef>("per human", (ThingDef d) => perPawn(d, ThingDefOf.Human.race.baseBodySize));
			array[3] = new TableDataGetter<ThingDef>("per muffalo", (ThingDef d) => perPawn(d, ThingDefOf.Muffalo.race.baseBodySize));
			array[4] = new TableDataGetter<ThingDef>("per dromedary", (ThingDef d) => perPawn(d, ThingDefOf.Dromedary.race.baseBodySize));
			array[5] = new TableDataGetter<ThingDef>("per nutrition", (ThingDef d) => perNutrition(d));
			array[6] = new TableDataGetter<ThingDef>("small volume", (ThingDef d) => (!d.smallVolume) ? "" : "small");
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06005280 RID: 21120 RVA: 0x002A4270 File Offset: 0x002A2670
		[DebugOutput]
		public static void ThingFillPercents()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.fillPercent > 0f
			orderby d.fillPercent descending
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[3];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("fillPercent", (ThingDef d) => d.fillPercent.ToStringPercent());
			array[2] = new TableDataGetter<ThingDef>("category", (ThingDef d) => d.category.ToString());
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06005281 RID: 21121 RVA: 0x002A4350 File Offset: 0x002A2750
		[DebugOutput]
		public static void ThingNutritions()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.ingestible != null
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[4];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("market value", (ThingDef d) => d.BaseMarketValue.ToString("F1"));
			array[2] = new TableDataGetter<ThingDef>("nutrition", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.Nutrition, null).ToString("F2"));
			array[3] = new TableDataGetter<ThingDef>("nutrition per value", (ThingDef d) => (d.GetStatValueAbstract(StatDefOf.Nutrition, null) / d.BaseMarketValue).ToString("F3"));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06005282 RID: 21122 RVA: 0x002A4438 File Offset: 0x002A2838
		public static void MakeTablePairsByThing(List<ThingStuffPair> pairList)
		{
			DefMap<ThingDef, float> totalCommMult = new DefMap<ThingDef, float>();
			DefMap<ThingDef, float> totalComm = new DefMap<ThingDef, float>();
			DefMap<ThingDef, int> pairCount = new DefMap<ThingDef, int>();
			foreach (ThingStuffPair thingStuffPair in pairList)
			{
				DefMap<ThingDef, float> defMap;
				ThingDef thing;
				(defMap = totalCommMult)[thing = thingStuffPair.thing] = defMap[thing] + thingStuffPair.commonalityMultiplier;
				ThingDef thing2;
				(defMap = totalComm)[thing2 = thingStuffPair.thing] = defMap[thing2] + thingStuffPair.Commonality;
				DefMap<ThingDef, int> pairCount2;
				ThingDef thing3;
				(pairCount2 = pairCount)[thing3 = thingStuffPair.thing] = pairCount2[thing3] + 1;
			}
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where pairList.Any((ThingStuffPair pa) => pa.thing == d)
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[5];
			array[0] = new TableDataGetter<ThingDef>("thing", (ThingDef t) => t.defName);
			array[1] = new TableDataGetter<ThingDef>("pair count", (ThingDef t) => pairCount[t].ToString());
			array[2] = new TableDataGetter<ThingDef>("total commonality multiplier ", (ThingDef t) => totalCommMult[t].ToString("F4"));
			array[3] = new TableDataGetter<ThingDef>("total commonality", (ThingDef t) => totalComm[t].ToString("F4"));
			array[4] = new TableDataGetter<ThingDef>("generateCommonality", (ThingDef t) => t.generateCommonality.ToString("F4"));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x06005283 RID: 21123 RVA: 0x002A45F0 File Offset: 0x002A29F0
		public static string ToStringEmptyZero(this float f, string format)
		{
			string result;
			if (f <= 0f)
			{
				result = "";
			}
			else
			{
				result = f.ToString(format);
			}
			return result;
		}

		// Token: 0x06005284 RID: 21124 RVA: 0x002A4624 File Offset: 0x002A2A24
		public static string ToStringPercentEmptyZero(this float f, string format = "F0")
		{
			string result;
			if (f <= 0f)
			{
				result = "";
			}
			else
			{
				result = f.ToStringPercent(format);
			}
			return result;
		}

		// Token: 0x06005285 RID: 21125 RVA: 0x002A4658 File Offset: 0x002A2A58
		public static string ToStringCheckBlank(this bool b)
		{
			return (!b) ? "" : "✓";
		}
	}
}
