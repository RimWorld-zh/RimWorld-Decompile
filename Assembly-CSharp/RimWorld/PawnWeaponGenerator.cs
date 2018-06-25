using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200048D RID: 1165
	public static class PawnWeaponGenerator
	{
		// Token: 0x04000C58 RID: 3160
		private static List<ThingStuffPair> allWeaponPairs;

		// Token: 0x04000C59 RID: 3161
		private static List<ThingStuffPair> workingWeapons = new List<ThingStuffPair>();

		// Token: 0x0600148C RID: 5260 RVA: 0x000B4D98 File Offset: 0x000B3198
		public static void Reset()
		{
			Predicate<ThingDef> isWeapon = (ThingDef td) => td.equipmentType == EquipmentType.Primary && !td.weaponTags.NullOrEmpty<string>();
			PawnWeaponGenerator.allWeaponPairs = ThingStuffPair.AllWith(isWeapon);
			using (IEnumerator<ThingDef> enumerator = (from td in DefDatabase<ThingDef>.AllDefs
			where isWeapon(td)
			select td).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef thingDef = enumerator.Current;
					float num = (from pa in PawnWeaponGenerator.allWeaponPairs
					where pa.thing == thingDef
					select pa).Sum((ThingStuffPair pa) => pa.Commonality);
					float num2 = thingDef.generateCommonality / num;
					if (num2 != 1f)
					{
						for (int i = 0; i < PawnWeaponGenerator.allWeaponPairs.Count; i++)
						{
							ThingStuffPair thingStuffPair = PawnWeaponGenerator.allWeaponPairs[i];
							if (thingStuffPair.thing == thingDef)
							{
								PawnWeaponGenerator.allWeaponPairs[i] = new ThingStuffPair(thingStuffPair.thing, thingStuffPair.stuff, thingStuffPair.commonalityMultiplier * num2);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x000B4F08 File Offset: 0x000B3308
		public static void TryGenerateWeaponFor(Pawn pawn)
		{
			PawnWeaponGenerator.workingWeapons.Clear();
			if (pawn.kindDef.weaponTags != null && pawn.kindDef.weaponTags.Count != 0)
			{
				if (pawn.RaceProps.ToolUser)
				{
					if (pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
					{
						if (pawn.story == null || !pawn.story.WorkTagIsDisabled(WorkTags.Violent))
						{
							float randomInRange = pawn.kindDef.weaponMoney.RandomInRange;
							for (int i = 0; i < PawnWeaponGenerator.allWeaponPairs.Count; i++)
							{
								ThingStuffPair w = PawnWeaponGenerator.allWeaponPairs[i];
								if (w.Price <= randomInRange)
								{
									if (pawn.kindDef.weaponTags == null || pawn.kindDef.weaponTags.Any((string tag) => w.thing.weaponTags.Contains(tag)))
									{
										if (w.thing.generateAllowChance < 1f)
										{
											if (!Rand.ChanceSeeded(w.thing.generateAllowChance, pawn.thingIDNumber ^ (int)w.thing.shortHash ^ 28554824))
											{
												goto IL_16F;
											}
										}
										PawnWeaponGenerator.workingWeapons.Add(w);
									}
								}
								IL_16F:;
							}
							if (PawnWeaponGenerator.workingWeapons.Count != 0)
							{
								pawn.equipment.DestroyAllEquipment(DestroyMode.Vanish);
								ThingStuffPair thingStuffPair;
								if (PawnWeaponGenerator.workingWeapons.TryRandomElementByWeight((ThingStuffPair w) => w.Commonality * w.Price, out thingStuffPair))
								{
									ThingWithComps thingWithComps = (ThingWithComps)ThingMaker.MakeThing(thingStuffPair.thing, thingStuffPair.stuff);
									PawnGenerator.PostProcessGeneratedGear(thingWithComps, pawn);
									pawn.equipment.AddEquipment(thingWithComps);
								}
								PawnWeaponGenerator.workingWeapons.Clear();
							}
						}
					}
				}
			}
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x000B5124 File Offset: 0x000B3524
		public static bool IsDerpWeapon(ThingDef thing, ThingDef stuff)
		{
			bool result;
			if (stuff == null)
			{
				result = false;
			}
			else
			{
				if (thing.IsMeleeWeapon)
				{
					if (thing.tools.NullOrEmpty<Tool>())
					{
						return false;
					}
					DamageDef damageDef = ThingUtility.PrimaryMeleeWeaponDamageType(thing);
					if (damageDef == null)
					{
						return false;
					}
					DamageArmorCategoryDef armorCategory = damageDef.armorCategory;
					if (armorCategory != null && armorCategory.multStat != null && stuff.GetStatValueAbstract(armorCategory.multStat, null) < 0.7f)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x000B51B8 File Offset: 0x000B35B8
		public static float CheapestNonDerpPriceFor(ThingDef weaponDef)
		{
			float num = 9999999f;
			for (int i = 0; i < PawnWeaponGenerator.allWeaponPairs.Count; i++)
			{
				ThingStuffPair thingStuffPair = PawnWeaponGenerator.allWeaponPairs[i];
				if (thingStuffPair.thing == weaponDef && !PawnWeaponGenerator.IsDerpWeapon(thingStuffPair.thing, thingStuffPair.stuff))
				{
					if (thingStuffPair.Price < num)
					{
						num = thingStuffPair.Price;
					}
				}
			}
			return num;
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x000B523C File Offset: 0x000B363C
		[DebugOutput]
		internal static void WeaponPairs()
		{
			IEnumerable<ThingStuffPair> dataSources = from p in PawnWeaponGenerator.allWeaponPairs
			orderby p.thing.defName descending
			select p;
			TableDataGetter<ThingStuffPair>[] array = new TableDataGetter<ThingStuffPair>[7];
			array[0] = new TableDataGetter<ThingStuffPair>("thing", (ThingStuffPair p) => p.thing.defName);
			array[1] = new TableDataGetter<ThingStuffPair>("stuff", (ThingStuffPair p) => (p.stuff == null) ? "" : p.stuff.defName);
			array[2] = new TableDataGetter<ThingStuffPair>("price", (ThingStuffPair p) => p.Price.ToString());
			array[3] = new TableDataGetter<ThingStuffPair>("commonality", (ThingStuffPair p) => p.Commonality.ToString("F5"));
			array[4] = new TableDataGetter<ThingStuffPair>("commMult", (ThingStuffPair p) => p.commonalityMultiplier.ToString("F5"));
			array[5] = new TableDataGetter<ThingStuffPair>("generateCommonality", (ThingStuffPair p) => p.thing.generateCommonality.ToString("F2"));
			array[6] = new TableDataGetter<ThingStuffPair>("derp", (ThingStuffPair p) => (!PawnWeaponGenerator.IsDerpWeapon(p.thing, p.stuff)) ? "" : "D");
			DebugTables.MakeTablesDialog<ThingStuffPair>(dataSources, array);
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x000B53A2 File Offset: 0x000B37A2
		[DebugOutput]
		internal static void WeaponPairsByThing()
		{
			DebugOutputsGeneral.MakeTablePairsByThing(PawnWeaponGenerator.allWeaponPairs);
		}
	}
}
