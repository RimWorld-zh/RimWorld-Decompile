using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	[HasDebugOutput]
	public static class DebugOutputsGeneral
	{
		[CompilerGenerated]
		private static Func<ThingDef, int> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<ThingDef, int> <>f__am$cache5;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache6;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache7;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache8;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache9;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cacheA;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cacheB;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cacheC;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cacheD;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cacheE;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cacheF;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache10;

		[CompilerGenerated]
		private static Action <>f__am$cache11;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache12;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache13;

		[CompilerGenerated]
		private static Func<Def, float> <>f__am$cache14;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache15;

		[CompilerGenerated]
		private static Func<HediffDef, bool> <>f__am$cache16;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache17;

		[CompilerGenerated]
		private static Func<Def, string> <>f__am$cache18;

		[CompilerGenerated]
		private static Action <>f__am$cache19;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache1A;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache1B;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache1C;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache1D;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache1E;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache1F;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache20;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache21;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache22;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache23;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache24;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache25;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache26;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache27;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache28;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache29;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache2A;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache2B;

		[CompilerGenerated]
		private static Func<ThingDef, ThingCategory> <>f__am$cache2C;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache2D;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache2E;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache2F;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache30;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache31;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache32;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache33;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache34;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache35;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache36;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache37;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache38;

		[CompilerGenerated]
		private static Func<ThingDef, float, string> <>f__am$cache39;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache3A;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache3B;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache3C;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache3D;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache3E;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache3F;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache40;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache41;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache42;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache43;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache44;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache45;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache46;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache47;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache48;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache49;

		[CompilerGenerated]
		private static Func<BodyPartGroupDef, string> <>f__am$cache4A;

		[CompilerGenerated]
		private static Func<ApparelLayerDef, string> <>f__am$cache4B;

		[CompilerGenerated]
		private static Func<string, string> <>f__am$cache4C;

		[DebugOutput]
		public static void WeaponsRanged()
		{
			Func<ThingDef, int> damage = (ThingDef d) => (d.Verbs[0].defaultProjectile == null) ? 0 : d.Verbs[0].defaultProjectile.projectile.DamageAmount;
			Func<ThingDef, float> armorPenetration = (ThingDef d) => (d.Verbs[0].defaultProjectile == null) ? 0f : d.Verbs[0].defaultProjectile.projectile.ArmorPenetration;
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
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[25];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("damage", (ThingDef d) => damage(d).ToString());
			array[2] = new TableDataGetter<ThingDef>("AP", (ThingDef d) => armorPenetration(d).ToStringPercent());
			array[3] = new TableDataGetter<ThingDef>("stop\npower", (ThingDef d) => (stoppingPower(d) <= 0f) ? "" : stoppingPower(d).ToString("F1"));
			array[4] = new TableDataGetter<ThingDef>("warmup", (ThingDef d) => warmup(d).ToString("F2"));
			array[5] = new TableDataGetter<ThingDef>("burst\nshots", (ThingDef d) => burstShots(d).ToString());
			array[6] = new TableDataGetter<ThingDef>("cooldown", (ThingDef d) => cooldown(d).ToString("F2"));
			array[7] = new TableDataGetter<ThingDef>("full\ncycle", (ThingDef d) => fullcycle(d).ToString("F2"));
			array[8] = new TableDataGetter<ThingDef>("range", (ThingDef d) => d.Verbs[0].range.ToString("F1"));
			array[9] = new TableDataGetter<ThingDef>("projectile\nspeed", (ThingDef d) => (d.projectile == null) ? "" : d.projectile.speed.ToString("F0"));
			array[10] = new TableDataGetter<ThingDef>("dps\nmissless", (ThingDef d) => dpsMissless(d).ToString("F2"));
			array[11] = new TableDataGetter<ThingDef>("accuracy\ntouch (" + 4f + ")", (ThingDef d) => accTouch(d).ToStringPercent());
			array[12] = new TableDataGetter<ThingDef>("accuracy\nshort (" + 15f + ")", (ThingDef d) => accShort(d).ToStringPercent());
			array[13] = new TableDataGetter<ThingDef>("accuracy\nmed (" + 30f + ")", (ThingDef d) => accMed(d).ToStringPercent());
			array[14] = new TableDataGetter<ThingDef>("accuracy\nlong (" + 50f + ")", (ThingDef d) => accLong(d).ToStringPercent());
			array[15] = new TableDataGetter<ThingDef>("accuracy\navg", (ThingDef d) => accAvg(d).ToString("F2"));
			array[16] = new TableDataGetter<ThingDef>("forced\nmiss\nradius", (ThingDef d) => (d.Verbs[0].forcedMissRadius <= 0f) ? "" : d.Verbs[0].forcedMissRadius.ToString());
			array[17] = new TableDataGetter<ThingDef>("dps\ntouch", (ThingDef d) => (dpsMissless(d) * accTouch(d)).ToString("F2"));
			array[18] = new TableDataGetter<ThingDef>("dps\nshort", (ThingDef d) => (dpsMissless(d) * accShort(d)).ToString("F2"));
			array[19] = new TableDataGetter<ThingDef>("dps\nmed", (ThingDef d) => (dpsMissless(d) * accMed(d)).ToString("F2"));
			array[20] = new TableDataGetter<ThingDef>("dps\nlong", (ThingDef d) => (dpsMissless(d) * accLong(d)).ToString("F2"));
			array[21] = new TableDataGetter<ThingDef>("dps\navg", (ThingDef d) => dpsAvg(d).ToString("F2"));
			array[22] = new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.MarketValue, null).ToString("F0"));
			array[23] = new TableDataGetter<ThingDef>("work", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.WorkToMake, null).ToString("F0"));
			array[24] = new TableDataGetter<ThingDef>("dpsAvg*100 / market value", (ThingDef d) => (dpsAvg(d) * 100f / d.GetStatValueAbstract(StatDefOf.MarketValue, null)).ToString("F3"));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

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

		public static string ToStringCheckBlank(this bool b)
		{
			return (!b) ? "" : "✓";
		}

		[CompilerGenerated]
		private static int <WeaponsRanged>m__0(ThingDef d)
		{
			return (d.Verbs[0].defaultProjectile == null) ? 0 : d.Verbs[0].defaultProjectile.projectile.DamageAmount;
		}

		[CompilerGenerated]
		private static float <WeaponsRanged>m__1(ThingDef d)
		{
			return (d.Verbs[0].defaultProjectile == null) ? 0f : d.Verbs[0].defaultProjectile.projectile.ArmorPenetration;
		}

		[CompilerGenerated]
		private static float <WeaponsRanged>m__2(ThingDef d)
		{
			return (d.Verbs[0].defaultProjectile == null) ? 0f : d.Verbs[0].defaultProjectile.projectile.stoppingPower;
		}

		[CompilerGenerated]
		private static float <WeaponsRanged>m__3(ThingDef d)
		{
			return d.Verbs[0].warmupTime;
		}

		[CompilerGenerated]
		private static float <WeaponsRanged>m__4(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.RangedWeapon_Cooldown, null);
		}

		[CompilerGenerated]
		private static int <WeaponsRanged>m__5(ThingDef d)
		{
			return d.Verbs[0].burstShotCount;
		}

		[CompilerGenerated]
		private static float <WeaponsRanged>m__6(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.AccuracyTouch, null);
		}

		[CompilerGenerated]
		private static float <WeaponsRanged>m__7(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.AccuracyShort, null);
		}

		[CompilerGenerated]
		private static float <WeaponsRanged>m__8(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.AccuracyMedium, null);
		}

		[CompilerGenerated]
		private static float <WeaponsRanged>m__9(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.AccuracyLong, null);
		}

		[CompilerGenerated]
		private static bool <WeaponsRanged>m__A(ThingDef d)
		{
			return d.IsRangedWeapon;
		}

		[CompilerGenerated]
		private static string <WeaponsRanged>m__B(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <WeaponsRanged>m__C(ThingDef d)
		{
			return d.Verbs[0].range.ToString("F1");
		}

		[CompilerGenerated]
		private static string <WeaponsRanged>m__D(ThingDef d)
		{
			return (d.projectile == null) ? "" : d.projectile.speed.ToString("F0");
		}

		[CompilerGenerated]
		private static string <WeaponsRanged>m__E(ThingDef d)
		{
			return (d.Verbs[0].forcedMissRadius <= 0f) ? "" : d.Verbs[0].forcedMissRadius.ToString();
		}

		[CompilerGenerated]
		private static string <WeaponsRanged>m__F(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.MarketValue, null).ToString("F0");
		}

		[CompilerGenerated]
		private static string <WeaponsRanged>m__10(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.WorkToMake, null).ToString("F0");
		}

		[CompilerGenerated]
		private static void <MeleeByStuff>m__11()
		{
			DebugOutputsGeneral.DoTableInternalApparel(null);
		}

		[CompilerGenerated]
		private static bool <MeleeByStuff>m__12(ThingDef td)
		{
			return td.IsStuff;
		}

		[CompilerGenerated]
		private static float <MeleeByStuff>m__13(ThingDef td)
		{
			return td.GetStatValueAbstract(StatDefOf.SharpDamageMultiplier, null);
		}

		[CompilerGenerated]
		private static float <DoTablesInternalMelee>m__14(Def d)
		{
			return 0f;
		}

		[CompilerGenerated]
		private static bool <DoTablesInternalMelee>m__15(ThingDef d)
		{
			return d.IsWeapon;
		}

		[CompilerGenerated]
		private static bool <DoTablesInternalMelee>m__16(HediffDef h)
		{
			return h.CompProps<HediffCompProperties_VerbGiver>() != null;
		}

		[CompilerGenerated]
		private static bool <DoTablesInternalMelee>m__17(ThingDef d)
		{
			return d.race != null;
		}

		[CompilerGenerated]
		private static string <DoTablesInternalMelee>m__18(Def d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static void <ApparelByStuff>m__19()
		{
			DebugOutputsGeneral.DoTableInternalApparel(null);
		}

		[CompilerGenerated]
		private static bool <ApparelByStuff>m__1A(ThingDef td)
		{
			return td.IsStuff;
		}

		[CompilerGenerated]
		private static string <ApparelArmor>m__1B(ThingDef x)
		{
			return x.LabelCap;
		}

		[CompilerGenerated]
		private static string <ApparelArmor>m__1C(ThingDef x)
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
		}

		[CompilerGenerated]
		private static bool <ApparelArmor>m__1D(ThingDef x)
		{
			return x.IsStuff;
		}

		[CompilerGenerated]
		private static float <ApparelArmor>m__1E(ThingDef x)
		{
			return x.BaseMarketValue;
		}

		[CompilerGenerated]
		private static bool <ApparelArmor>m__1F(ThingDef x)
		{
			return x.IsApparel;
		}

		[CompilerGenerated]
		private static float <ApparelArmor>m__20(ThingDef x)
		{
			return x.BaseMarketValue;
		}

		[CompilerGenerated]
		private static string <ApparelInsulation>m__21(ThingDef x)
		{
			return x.LabelCap;
		}

		[CompilerGenerated]
		private static string <ApparelInsulation>m__22(ThingDef x)
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
		}

		[CompilerGenerated]
		private static bool <ApparelInsulation>m__23(ThingDef x)
		{
			return x.IsStuff;
		}

		[CompilerGenerated]
		private static float <ApparelInsulation>m__24(ThingDef x)
		{
			return x.BaseMarketValue;
		}

		[CompilerGenerated]
		private static bool <ApparelInsulation>m__25(ThingDef x)
		{
			return x.IsApparel;
		}

		[CompilerGenerated]
		private static float <ApparelInsulation>m__26(ThingDef x)
		{
			return x.BaseMarketValue;
		}

		[CompilerGenerated]
		private static string <DoTableInternalApparel>m__27(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <DoTableInternalApparel>m__28(ThingDef d)
		{
			return GenText.ToSpaceList(from bp in d.apparel.bodyPartGroups
			select bp.defName);
		}

		[CompilerGenerated]
		private static string <DoTableInternalApparel>m__29(ThingDef d)
		{
			return GenText.ToSpaceList(from l in d.apparel.layers
			select l.ToString());
		}

		[CompilerGenerated]
		private static string <DoTableInternalApparel>m__2A(ThingDef d)
		{
			return GenText.ToSpaceList(from t in d.apparel.tags
			select t.ToString());
		}

		[CompilerGenerated]
		private static bool <ThingDamageData>m__2B(ThingDef d)
		{
			return d.useHitPoints;
		}

		[CompilerGenerated]
		private static ThingCategory <ThingDamageData>m__2C(ThingDef d)
		{
			return d.category;
		}

		[CompilerGenerated]
		private static string <ThingDamageData>m__2D(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <ThingDamageData>m__2E(ThingDef d)
		{
			return d.category.ToString();
		}

		[CompilerGenerated]
		private static string <ThingDamageData>m__2F(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <ThingDamageData>m__30(ThingDef d)
		{
			return d.BaseMaxHitPoints.ToString();
		}

		[CompilerGenerated]
		private static string <ThingDamageData>m__31(ThingDef d)
		{
			return (d.BaseFlammability <= 0f) ? "" : d.BaseFlammability.ToString();
		}

		[CompilerGenerated]
		private static string <ThingDamageData>m__32(ThingDef d)
		{
			return d.MadeFromStuff.ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <ThingDamageData>m__33(ThingDef d)
		{
			return (d.GetStatValueAbstract(StatDefOf.DeteriorationRate, null) <= 0f) ? "" : d.GetStatValueAbstract(StatDefOf.DeteriorationRate, null).ToString();
		}

		[CompilerGenerated]
		private static string <ThingDamageData>m__34(ThingDef d)
		{
			return (d.GetStatValueAbstract(StatDefOf.DeteriorationRate, null) <= 0f) ? "" : ((float)d.BaseMaxHitPoints / d.GetStatValueAbstract(StatDefOf.DeteriorationRate, null)).ToString();
		}

		[CompilerGenerated]
		private static bool <ThingMasses>m__35(ThingDef x)
		{
			return x.category == ThingCategory.Item || x.Minifiable;
		}

		[CompilerGenerated]
		private static bool <ThingMasses>m__36(ThingDef x)
		{
			return x.thingClass != typeof(MinifiedThing) && x.thingClass != typeof(UnfinishedThing);
		}

		[CompilerGenerated]
		private static float <ThingMasses>m__37(ThingDef x)
		{
			return x.GetStatValueAbstract(StatDefOf.Mass, null);
		}

		[CompilerGenerated]
		private static float <ThingMasses>m__38(ThingDef x)
		{
			return x.GetStatValueAbstract(StatDefOf.MarketValue, null);
		}

		[CompilerGenerated]
		private static string <ThingMasses>m__39(ThingDef d, float bodySize)
		{
			return (bodySize * 35f / d.GetStatValueAbstract(StatDefOf.Mass, null)).ToString("F0");
		}

		[CompilerGenerated]
		private static string <ThingMasses>m__3A(ThingDef d)
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
		}

		[CompilerGenerated]
		private static string <ThingMasses>m__3B(ThingDef d)
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
		}

		[CompilerGenerated]
		private static string <ThingMasses>m__3C(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.Mass, null).ToString();
		}

		[CompilerGenerated]
		private static string <ThingMasses>m__3D(ThingDef d)
		{
			return (!d.smallVolume) ? "" : "small";
		}

		[CompilerGenerated]
		private static bool <ThingFillPercents>m__3E(ThingDef d)
		{
			return d.fillPercent > 0f;
		}

		[CompilerGenerated]
		private static float <ThingFillPercents>m__3F(ThingDef d)
		{
			return d.fillPercent;
		}

		[CompilerGenerated]
		private static string <ThingFillPercents>m__40(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <ThingFillPercents>m__41(ThingDef d)
		{
			return d.fillPercent.ToStringPercent();
		}

		[CompilerGenerated]
		private static string <ThingFillPercents>m__42(ThingDef d)
		{
			return d.category.ToString();
		}

		[CompilerGenerated]
		private static bool <ThingNutritions>m__43(ThingDef d)
		{
			return d.ingestible != null;
		}

		[CompilerGenerated]
		private static string <ThingNutritions>m__44(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <ThingNutritions>m__45(ThingDef d)
		{
			return d.BaseMarketValue.ToString("F1");
		}

		[CompilerGenerated]
		private static string <ThingNutritions>m__46(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.Nutrition, null).ToString("F2");
		}

		[CompilerGenerated]
		private static string <ThingNutritions>m__47(ThingDef d)
		{
			return (d.GetStatValueAbstract(StatDefOf.Nutrition, null) / d.BaseMarketValue).ToString("F3");
		}

		[CompilerGenerated]
		private static string <MakeTablePairsByThing>m__48(ThingDef t)
		{
			return t.defName;
		}

		[CompilerGenerated]
		private static string <MakeTablePairsByThing>m__49(ThingDef t)
		{
			return t.generateCommonality.ToString("F4");
		}

		[CompilerGenerated]
		private static string <DoTableInternalApparel>m__4A(BodyPartGroupDef bp)
		{
			return bp.defName;
		}

		[CompilerGenerated]
		private static string <DoTableInternalApparel>m__4B(ApparelLayerDef l)
		{
			return l.ToString();
		}

		[CompilerGenerated]
		private static string <DoTableInternalApparel>m__4C(string t)
		{
			return t.ToString();
		}

		[CompilerGenerated]
		private sealed class <WeaponsRanged>c__AnonStorey0
		{
			internal Func<ThingDef, float> warmup;

			internal Func<ThingDef, float> cooldown;

			internal Func<ThingDef, int> burstShots;

			internal Func<ThingDef, int> damage;

			internal Func<ThingDef, float> accTouch;

			internal Func<ThingDef, float> accShort;

			internal Func<ThingDef, float> accMed;

			internal Func<ThingDef, float> accLong;

			internal Func<ThingDef, float> dpsMissless;

			internal Func<ThingDef, float> accAvg;

			internal Func<ThingDef, float> armorPenetration;

			internal Func<ThingDef, float> stoppingPower;

			internal Func<ThingDef, float> fullcycle;

			internal Func<ThingDef, float> dpsAvg;

			public <WeaponsRanged>c__AnonStorey0()
			{
			}

			internal float <>m__0(ThingDef d)
			{
				return this.warmup(d) + this.cooldown(d) + ((d.Verbs[0].burstShotCount - 1) * d.Verbs[0].ticksBetweenBurstShots).TicksToSeconds();
			}

			internal float <>m__1(ThingDef d)
			{
				int num = this.burstShots(d);
				float num2 = this.warmup(d) + this.cooldown(d);
				num2 += (float)(num - 1) * ((float)d.Verbs[0].ticksBetweenBurstShots / 60f);
				return (float)(this.damage(d) * num) / num2;
			}

			internal float <>m__2(ThingDef d)
			{
				return (this.accTouch(d) + this.accShort(d) + this.accMed(d) + this.accLong(d)) / 4f;
			}

			internal float <>m__3(ThingDef d)
			{
				return this.dpsMissless(d) * this.accAvg(d);
			}

			internal string <>m__4(ThingDef d)
			{
				return this.damage(d).ToString();
			}

			internal string <>m__5(ThingDef d)
			{
				return this.armorPenetration(d).ToStringPercent();
			}

			internal string <>m__6(ThingDef d)
			{
				return (this.stoppingPower(d) <= 0f) ? "" : this.stoppingPower(d).ToString("F1");
			}

			internal string <>m__7(ThingDef d)
			{
				return this.warmup(d).ToString("F2");
			}

			internal string <>m__8(ThingDef d)
			{
				return this.burstShots(d).ToString();
			}

			internal string <>m__9(ThingDef d)
			{
				return this.cooldown(d).ToString("F2");
			}

			internal string <>m__A(ThingDef d)
			{
				return this.fullcycle(d).ToString("F2");
			}

			internal string <>m__B(ThingDef d)
			{
				return this.dpsMissless(d).ToString("F2");
			}

			internal string <>m__C(ThingDef d)
			{
				return this.accTouch(d).ToStringPercent();
			}

			internal string <>m__D(ThingDef d)
			{
				return this.accShort(d).ToStringPercent();
			}

			internal string <>m__E(ThingDef d)
			{
				return this.accMed(d).ToStringPercent();
			}

			internal string <>m__F(ThingDef d)
			{
				return this.accLong(d).ToStringPercent();
			}

			internal string <>m__10(ThingDef d)
			{
				return this.accAvg(d).ToString("F2");
			}

			internal string <>m__11(ThingDef d)
			{
				return (this.dpsMissless(d) * this.accTouch(d)).ToString("F2");
			}

			internal string <>m__12(ThingDef d)
			{
				return (this.dpsMissless(d) * this.accShort(d)).ToString("F2");
			}

			internal string <>m__13(ThingDef d)
			{
				return (this.dpsMissless(d) * this.accMed(d)).ToString("F2");
			}

			internal string <>m__14(ThingDef d)
			{
				return (this.dpsMissless(d) * this.accLong(d)).ToString("F2");
			}

			internal string <>m__15(ThingDef d)
			{
				return this.dpsAvg(d).ToString("F2");
			}

			internal string <>m__16(ThingDef d)
			{
				return (this.dpsAvg(d) * 100f / d.GetStatValueAbstract(StatDefOf.MarketValue, null)).ToString("F3");
			}
		}

		[CompilerGenerated]
		private sealed class <MeleeByStuff>c__AnonStorey1
		{
			internal ThingDef localStuff;

			public <MeleeByStuff>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				DebugOutputsGeneral.DoTablesInternalMelee(this.localStuff, false);
			}
		}

		[CompilerGenerated]
		private sealed class <DoTablesInternalMelee>c__AnonStorey2
		{
			internal ThingDef stuff;

			internal Func<Def, float> meleeDamageGetter;

			internal Func<Def, float> meleeWarmupGetter;

			internal Func<Def, float> meleeCooldownGetter;

			internal Func<Def, float> rangedDamageGetter;

			internal Func<Def, float> rangedWarmupGetter;

			internal Func<Def, float> rangedCooldownGetter;

			internal Func<Def, float> meleeDpsGetter;

			internal Func<Def, float> rangedDpsGetter;

			internal Func<Def, float> dpsGetter;

			internal Func<Def, float> marketValueGetter;

			public <DoTablesInternalMelee>c__AnonStorey2()
			{
			}

			internal float <>m__0(Def d)
			{
				Thing owner;
				List<Verb> concreteExampleVerbs = VerbUtility.GetConcreteExampleVerbs(d, out owner, this.stuff);
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
			}

			internal float <>m__1(Def d)
			{
				Thing thing;
				List<Verb> concreteExampleVerbs = VerbUtility.GetConcreteExampleVerbs(d, out thing, this.stuff);
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
			}

			internal float <>m__2(Def d)
			{
				Thing thing;
				List<Verb> concreteExampleVerbs = VerbUtility.GetConcreteExampleVerbs(d, out thing, this.stuff);
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
			}

			internal float <>m__3(Def d)
			{
				Thing owner;
				List<Verb> concreteExampleVerbs = VerbUtility.GetConcreteExampleVerbs(d, out owner, this.stuff);
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
			}

			internal float <>m__4(Def d)
			{
				Thing thing;
				List<Verb> concreteExampleVerbs = VerbUtility.GetConcreteExampleVerbs(d, out thing, this.stuff);
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
			}

			internal float <>m__5(Def d)
			{
				return this.meleeDamageGetter(d) / (this.meleeWarmupGetter(d) + this.meleeCooldownGetter(d));
			}

			internal float <>m__6(Def d)
			{
				return this.rangedDamageGetter(d) / (this.rangedWarmupGetter(d) + this.rangedCooldownGetter(d));
			}

			internal float <>m__7(Def d)
			{
				return Mathf.Max(this.meleeDpsGetter(d), this.rangedDpsGetter(d));
			}

			internal float <>m__8(Def d)
			{
				ThingDef thingDef = d as ThingDef;
				float result;
				if (thingDef != null)
				{
					result = thingDef.GetStatValueAbstract(StatDefOf.MarketValue, this.stuff);
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
			}

			internal float <>m__9(Def h)
			{
				return this.dpsGetter(h);
			}

			internal string <>m__A(Def d)
			{
				return this.meleeDamageGetter(d).ToString();
			}

			internal string <>m__B(Def d)
			{
				return this.meleeWarmupGetter(d).ToString("F2");
			}

			internal string <>m__C(Def d)
			{
				return this.meleeCooldownGetter(d).ToString("F2");
			}

			internal string <>m__D(Def d)
			{
				return this.meleeDpsGetter(d).ToString("F2");
			}

			internal string <>m__E(Def d)
			{
				return this.rangedDamageGetter(d).ToString();
			}

			internal string <>m__F(Def d)
			{
				return this.rangedWarmupGetter(d).ToString("F2");
			}

			internal string <>m__10(Def d)
			{
				return this.rangedCooldownGetter(d).ToString("F2");
			}

			internal string <>m__11(Def d)
			{
				return this.rangedDpsGetter(d).ToString("F2");
			}

			internal string <>m__12(Def d)
			{
				return this.dpsGetter(d).ToString("F2");
			}

			internal string <>m__13(Def d)
			{
				return this.marketValueGetter(d).ToString("F0");
			}

			internal string <>m__14(Def d)
			{
				ThingDef thingDef = d as ThingDef;
				string result;
				if (thingDef == null)
				{
					result = "-";
				}
				else
				{
					result = thingDef.GetStatValueAbstract(StatDefOf.WorkToMake, this.stuff).ToString("F0");
				}
				return result;
			}

			private sealed class <DoTablesInternalMelee>c__AnonStorey3
			{
				internal Thing owner;

				internal DebugOutputsGeneral.<DoTablesInternalMelee>c__AnonStorey2 <>f__ref$2;

				public <DoTablesInternalMelee>c__AnonStorey3()
				{
				}

				internal float <>m__0(Verb_MeleeAttack v)
				{
					return v.verbProps.AdjustedMeleeSelectionWeight(v, null, this.owner);
				}

				internal float <>m__1(Verb_MeleeAttack v)
				{
					return v.verbProps.AdjustedMeleeDamageAmount(v, null, this.owner);
				}
			}

			private sealed class <DoTablesInternalMelee>c__AnonStorey4
			{
				internal Thing owner;

				internal DebugOutputsGeneral.<DoTablesInternalMelee>c__AnonStorey2 <>f__ref$2;

				public <DoTablesInternalMelee>c__AnonStorey4()
				{
				}

				internal float <>m__0(Verb_MeleeAttack v)
				{
					return v.verbProps.AdjustedMeleeSelectionWeight(v, null, this.owner);
				}

				internal float <>m__1(Verb_MeleeAttack v)
				{
					return v.verbProps.AdjustedCooldown(v, null, this.owner);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <ApparelByStuff>c__AnonStorey5
		{
			internal ThingDef localStuff;

			public <ApparelByStuff>c__AnonStorey5()
			{
			}

			internal void <>m__0()
			{
				DebugOutputsGeneral.DoTableInternalApparel(this.localStuff);
			}
		}

		[CompilerGenerated]
		private sealed class <ApparelArmor>c__AnonStorey6
		{
			internal ThingDef stuffLocal;

			public <ApparelArmor>c__AnonStorey6()
			{
			}

			internal bool <>m__0(ThingDef x)
			{
				return x.IsApparel && this.stuffLocal.stuffProps.CanMake(x);
			}

			internal string <>m__1(ThingDef x)
			{
				string result;
				if (!this.stuffLocal.stuffProps.CanMake(x))
				{
					result = "";
				}
				else
				{
					result = string.Concat(new string[]
					{
						x.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp, this.stuffLocal).ToStringPercent(),
						" / ",
						x.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt, this.stuffLocal).ToStringPercent(),
						" / ",
						x.GetStatValueAbstract(StatDefOf.ArmorRating_Heat, this.stuffLocal).ToStringPercent()
					});
				}
				return result;
			}
		}

		[CompilerGenerated]
		private sealed class <ApparelInsulation>c__AnonStorey7
		{
			internal ThingDef stuffLocal;

			public <ApparelInsulation>c__AnonStorey7()
			{
			}

			internal bool <>m__0(ThingDef x)
			{
				return x.IsApparel && this.stuffLocal.stuffProps.CanMake(x);
			}

			internal string <>m__1(ThingDef x)
			{
				string result;
				if (!this.stuffLocal.stuffProps.CanMake(x))
				{
					result = "";
				}
				else
				{
					result = x.GetStatValueAbstract(StatDefOf.Insulation_Heat, this.stuffLocal).ToString("F1") + ", " + x.GetStatValueAbstract(StatDefOf.Insulation_Cold, this.stuffLocal).ToString("F1");
				}
				return result;
			}
		}

		[CompilerGenerated]
		private sealed class <DoTableInternalApparel>c__AnonStorey8
		{
			internal ThingDef stuff;

			public <DoTableInternalApparel>c__AnonStorey8()
			{
			}

			internal bool <>m__0(ThingDef d)
			{
				return d.IsApparel && (this.stuff == null || (d.MadeFromStuff && this.stuff.stuffProps.CanMake(d)));
			}

			internal string <>m__1(ThingDef d)
			{
				return d.GetStatValueAbstract(StatDefOf.WorkToMake, this.stuff).ToString("F0");
			}

			internal string <>m__2(ThingDef d)
			{
				return d.GetStatValueAbstract(StatDefOf.MarketValue, this.stuff).ToString("F0");
			}

			internal string <>m__3(ThingDef d)
			{
				return d.GetStatValueAbstract(StatDefOf.Insulation_Cold, this.stuff).ToString("F1");
			}

			internal string <>m__4(ThingDef d)
			{
				return d.GetStatValueAbstract(StatDefOf.Insulation_Heat, this.stuff).ToString("F1");
			}

			internal string <>m__5(ThingDef d)
			{
				return d.GetStatValueAbstract(StatDefOf.ArmorRating_Blunt, this.stuff).ToString("F2");
			}

			internal string <>m__6(ThingDef d)
			{
				return d.GetStatValueAbstract(StatDefOf.ArmorRating_Sharp, this.stuff).ToString("F2");
			}

			internal string <>m__7(ThingDef d)
			{
				return d.GetStatValueAbstract(StatDefOf.StuffEffectMultiplierArmor, this.stuff).ToString("F2");
			}

			internal string <>m__8(ThingDef d)
			{
				return d.GetStatValueAbstract(StatDefOf.StuffEffectMultiplierInsulation_Cold, this.stuff).ToString("F2");
			}

			internal string <>m__9(ThingDef d)
			{
				return d.GetStatValueAbstract(StatDefOf.StuffEffectMultiplierInsulation_Heat, this.stuff).ToString("F2");
			}

			internal string <>m__A(ThingDef d)
			{
				return d.GetStatValueAbstract(StatDefOf.EquipDelay, this.stuff).ToString("F1");
			}
		}

		[CompilerGenerated]
		private sealed class <ThingsExistingList>c__AnonStorey9
		{
			internal ThingRequestGroup localRg;

			public <ThingsExistingList>c__AnonStorey9()
			{
			}

			internal void <>m__0()
			{
				StringBuilder stringBuilder = new StringBuilder();
				List<Thing> list = Find.CurrentMap.listerThings.ThingsInGroup(this.localRg);
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"Global things in group ",
					this.localRg,
					" (count ",
					list.Count,
					")"
				}));
				Log.Message(DebugLogsUtility.ThingListToUniqueCountString(list), false);
			}
		}

		[CompilerGenerated]
		private sealed class <ThingMasses>c__AnonStoreyA
		{
			internal Func<ThingDef, float, string> perPawn;

			internal Func<ThingDef, string> perNutrition;

			public <ThingMasses>c__AnonStoreyA()
			{
			}

			internal string <>m__0(ThingDef d)
			{
				return this.perPawn(d, ThingDefOf.Human.race.baseBodySize);
			}

			internal string <>m__1(ThingDef d)
			{
				return this.perPawn(d, ThingDefOf.Muffalo.race.baseBodySize);
			}

			internal string <>m__2(ThingDef d)
			{
				return this.perPawn(d, ThingDefOf.Dromedary.race.baseBodySize);
			}

			internal string <>m__3(ThingDef d)
			{
				return this.perNutrition(d);
			}
		}

		[CompilerGenerated]
		private sealed class <MakeTablePairsByThing>c__AnonStoreyB
		{
			internal List<ThingStuffPair> pairList;

			internal DefMap<ThingDef, int> pairCount;

			internal DefMap<ThingDef, float> totalCommMult;

			internal DefMap<ThingDef, float> totalComm;

			public <MakeTablePairsByThing>c__AnonStoreyB()
			{
			}

			internal bool <>m__0(ThingDef d)
			{
				return this.pairList.Any((ThingStuffPair pa) => pa.thing == d);
			}

			internal string <>m__1(ThingDef t)
			{
				return this.pairCount[t].ToString();
			}

			internal string <>m__2(ThingDef t)
			{
				return this.totalCommMult[t].ToString("F4");
			}

			internal string <>m__3(ThingDef t)
			{
				return this.totalComm[t].ToString("F4");
			}

			private sealed class <MakeTablePairsByThing>c__AnonStoreyC
			{
				internal ThingDef d;

				internal DebugOutputsGeneral.<MakeTablePairsByThing>c__AnonStoreyB <>f__ref$11;

				public <MakeTablePairsByThing>c__AnonStoreyC()
				{
				}

				internal bool <>m__0(ThingStuffPair pa)
				{
					return pa.thing == this.d;
				}
			}
		}
	}
}
