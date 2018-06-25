using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000CA1 RID: 3233
	public static class RoofCollapserImmediate
	{
		// Token: 0x04003067 RID: 12391
		private static readonly IntRange ThinRoofCrushDamageRange = new IntRange(15, 30);

		// Token: 0x0600472E RID: 18222 RVA: 0x00259387 File Offset: 0x00257787
		public static void DropRoofInCells(IntVec3 c, Map map, List<Thing> outCrushedThings = null)
		{
			if (c.Roofed(map))
			{
				RoofCollapserImmediate.DropRoofInCellPhaseOne(c, map, outCrushedThings);
				RoofCollapserImmediate.DropRoofInCellPhaseTwo(c, map);
				SoundDefOf.Roof_Collapse.PlayOneShot(new TargetInfo(c, map, false));
			}
		}

		// Token: 0x0600472F RID: 18223 RVA: 0x002593C4 File Offset: 0x002577C4
		public static void DropRoofInCells(IEnumerable<IntVec3> cells, Map map, List<Thing> outCrushedThings = null)
		{
			IntVec3 cell = IntVec3.Invalid;
			foreach (IntVec3 c in cells)
			{
				if (c.Roofed(map))
				{
					RoofCollapserImmediate.DropRoofInCellPhaseOne(c, map, outCrushedThings);
				}
			}
			foreach (IntVec3 intVec in cells)
			{
				if (intVec.Roofed(map))
				{
					RoofCollapserImmediate.DropRoofInCellPhaseTwo(intVec, map);
					cell = intVec;
				}
			}
			if (cell.IsValid)
			{
				SoundDefOf.Roof_Collapse.PlayOneShot(new TargetInfo(cell, map, false));
			}
		}

		// Token: 0x06004730 RID: 18224 RVA: 0x002594B0 File Offset: 0x002578B0
		public static void DropRoofInCells(List<IntVec3> cells, Map map, List<Thing> outCrushedThings = null)
		{
			if (!cells.NullOrEmpty<IntVec3>())
			{
				IntVec3 cell = IntVec3.Invalid;
				for (int i = 0; i < cells.Count; i++)
				{
					if (cells[i].Roofed(map))
					{
						RoofCollapserImmediate.DropRoofInCellPhaseOne(cells[i], map, outCrushedThings);
					}
				}
				for (int j = 0; j < cells.Count; j++)
				{
					if (cells[j].Roofed(map))
					{
						RoofCollapserImmediate.DropRoofInCellPhaseTwo(cells[j], map);
						cell = cells[j];
					}
				}
				if (cell.IsValid)
				{
					SoundDefOf.Roof_Collapse.PlayOneShot(new TargetInfo(cell, map, false));
				}
			}
		}

		// Token: 0x06004731 RID: 18225 RVA: 0x00259574 File Offset: 0x00257974
		private static void DropRoofInCellPhaseOne(IntVec3 c, Map map, List<Thing> outCrushedThings)
		{
			RoofDef roofDef = map.roofGrid.RoofAt(c);
			if (roofDef != null)
			{
				if (roofDef.collapseLeavingThingDef != null && roofDef.collapseLeavingThingDef.passability == Traversability.Impassable)
				{
					for (int i = 0; i < 2; i++)
					{
						List<Thing> thingList = c.GetThingList(map);
						for (int j = thingList.Count - 1; j >= 0; j--)
						{
							Thing thing = thingList[j];
							RoofCollapserImmediate.TryAddToCrushedThingsList(thing, outCrushedThings);
							Pawn pawn = thing as Pawn;
							DamageInfo dinfo;
							if (pawn != null)
							{
								DamageDef crush = DamageDefOf.Crush;
								float amount = 99999f;
								BodyPartRecord brain = pawn.health.hediffSet.GetBrain();
								dinfo = new DamageInfo(crush, amount, -1f, null, brain, null, DamageInfo.SourceCategory.Collapse, null);
							}
							else
							{
								dinfo = new DamageInfo(DamageDefOf.Crush, 99999f, -1f, null, null, null, DamageInfo.SourceCategory.Collapse, null);
								dinfo.SetBodyRegion(BodyPartHeight.Top, BodyPartDepth.Outside);
							}
							BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = null;
							if (i == 0 && pawn != null)
							{
								battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(pawn, RulePackDefOf.DamageEvent_Ceiling, null);
								Find.BattleLog.Add(battleLogEntry_DamageTaken);
							}
							thing.TakeDamage(dinfo).AssociateWithLog(battleLogEntry_DamageTaken);
							if (!thing.Destroyed && thing.def.destroyable)
							{
								thing.Destroy(DestroyMode.Vanish);
							}
						}
					}
				}
				else
				{
					List<Thing> thingList2 = c.GetThingList(map);
					for (int k = thingList2.Count - 1; k >= 0; k--)
					{
						Thing thing2 = thingList2[k];
						if (thing2.def.category == ThingCategory.Item || thing2.def.category == ThingCategory.Plant || thing2.def.category == ThingCategory.Building || thing2.def.category == ThingCategory.Pawn)
						{
							RoofCollapserImmediate.TryAddToCrushedThingsList(thing2, outCrushedThings);
							float num = (float)RoofCollapserImmediate.ThinRoofCrushDamageRange.RandomInRange;
							if (thing2.def.building != null)
							{
								num *= thing2.def.building.roofCollapseDamageMultiplier;
							}
							BattleLogEntry_DamageTaken battleLogEntry_DamageTaken2 = null;
							if (thing2 is Pawn)
							{
								battleLogEntry_DamageTaken2 = new BattleLogEntry_DamageTaken((Pawn)thing2, RulePackDefOf.DamageEvent_Ceiling, null);
								Find.BattleLog.Add(battleLogEntry_DamageTaken2);
							}
							DamageInfo dinfo2 = new DamageInfo(DamageDefOf.Crush, (float)GenMath.RoundRandom(num), -1f, null, null, null, DamageInfo.SourceCategory.Collapse, null);
							dinfo2.SetBodyRegion(BodyPartHeight.Top, BodyPartDepth.Outside);
							thing2.TakeDamage(dinfo2).AssociateWithLog(battleLogEntry_DamageTaken2);
						}
					}
				}
				if (roofDef.collapseLeavingThingDef != null)
				{
					Thing thing3 = GenSpawn.Spawn(roofDef.collapseLeavingThingDef, c, map, WipeMode.Vanish);
					if (thing3.def.rotatable)
					{
						thing3.Rotation = Rot4.Random;
					}
				}
				for (int l = 0; l < 1; l++)
				{
					Vector3 vector = c.ToVector3Shifted();
					vector += Gen.RandomHorizontalVector(0.6f);
					MoteMaker.ThrowDustPuff(vector, map, 2f);
				}
			}
		}

		// Token: 0x06004732 RID: 18226 RVA: 0x0025987C File Offset: 0x00257C7C
		private static void DropRoofInCellPhaseTwo(IntVec3 c, Map map)
		{
			RoofDef roofDef = map.roofGrid.RoofAt(c);
			if (roofDef != null)
			{
				if (roofDef.filthLeaving != null)
				{
					FilthMaker.MakeFilth(c, map, roofDef.filthLeaving, 1);
				}
				if (roofDef.VanishOnCollapse)
				{
					map.roofGrid.SetRoof(c, null);
				}
				CellRect bound = CellRect.CenteredOn(c, 2);
				foreach (Pawn pawn2 in from pawn in map.mapPawns.AllPawnsSpawned
				where bound.Contains(pawn.Position)
				select pawn)
				{
					TaleRecorder.RecordTale(TaleDefOf.CollapseDodged, new object[]
					{
						pawn2
					});
				}
			}
		}

		// Token: 0x06004733 RID: 18227 RVA: 0x00259958 File Offset: 0x00257D58
		private static void TryAddToCrushedThingsList(Thing t, List<Thing> outCrushedThings)
		{
			if (outCrushedThings != null)
			{
				if (!outCrushedThings.Contains(t) && RoofCollapserImmediate.WorthMentioningInCrushLetter(t))
				{
					outCrushedThings.Add(t);
				}
			}
		}

		// Token: 0x06004734 RID: 18228 RVA: 0x00259984 File Offset: 0x00257D84
		private static bool WorthMentioningInCrushLetter(Thing t)
		{
			bool result;
			if (!t.def.destroyable)
			{
				result = false;
			}
			else
			{
				ThingCategory category = t.def.category;
				result = (category == ThingCategory.Building || category == ThingCategory.Pawn || (category == ThingCategory.Item && t.MarketValue > 0.01f));
			}
			return result;
		}
	}
}
