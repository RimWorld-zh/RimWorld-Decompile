using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200083F RID: 2111
	public static class FloatMenuMakerMap
	{
		// Token: 0x06002FBC RID: 12220 RVA: 0x00199420 File Offset: 0x00197820
		private static bool CanTakeOrder(Pawn pawn)
		{
			return pawn.IsColonistPlayerControlled;
		}

		// Token: 0x06002FBD RID: 12221 RVA: 0x0019943C File Offset: 0x0019783C
		public static void TryMakeFloatMenu(Pawn pawn)
		{
			if (FloatMenuMakerMap.CanTakeOrder(pawn))
			{
				if (pawn.Downed)
				{
					Messages.Message("IsIncapped".Translate(new object[]
					{
						pawn.LabelCap
					}), pawn, MessageTypeDefOf.RejectInput, false);
				}
				else if (pawn.Map == Find.CurrentMap)
				{
					List<FloatMenuOption> list = FloatMenuMakerMap.ChoicesAtFor(UI.MouseMapPosition(), pawn);
					if (list.Count != 0)
					{
						bool flag = true;
						FloatMenuOption floatMenuOption = null;
						for (int i = 0; i < list.Count; i++)
						{
							if (list[i].Disabled || !list[i].autoTakeable)
							{
								flag = false;
								break;
							}
							if (floatMenuOption == null || list[i].autoTakeablePriority > floatMenuOption.autoTakeablePriority)
							{
								floatMenuOption = list[i];
							}
						}
						if (flag && floatMenuOption != null)
						{
							floatMenuOption.Chosen(true);
						}
						else
						{
							FloatMenuMap floatMenuMap = new FloatMenuMap(list, pawn.LabelCap, UI.MouseMapPosition());
							floatMenuMap.givesColonistOrders = true;
							Find.WindowStack.Add(floatMenuMap);
						}
					}
				}
			}
		}

		// Token: 0x06002FBE RID: 12222 RVA: 0x00199574 File Offset: 0x00197974
		public static List<FloatMenuOption> ChoicesAtFor(Vector3 clickPos, Pawn pawn)
		{
			IntVec3 intVec = IntVec3.FromVector3(clickPos);
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			List<FloatMenuOption> result;
			if (!intVec.InBounds(pawn.Map) || !FloatMenuMakerMap.CanTakeOrder(pawn))
			{
				result = list;
			}
			else if (pawn.Map != Find.CurrentMap)
			{
				result = list;
			}
			else
			{
				FloatMenuMakerMap.makingFor = pawn;
				try
				{
					if (intVec.Fogged(pawn.Map))
					{
						if (pawn.Drafted)
						{
							FloatMenuOption floatMenuOption = FloatMenuMakerMap.GotoLocationOption(intVec, pawn);
							if (floatMenuOption != null && !floatMenuOption.Disabled)
							{
								list.Add(floatMenuOption);
							}
						}
					}
					else
					{
						if (pawn.Drafted)
						{
							FloatMenuMakerMap.AddDraftedOrders(clickPos, pawn, list);
						}
						if (pawn.RaceProps.Humanlike)
						{
							FloatMenuMakerMap.AddHumanlikeOrders(clickPos, pawn, list);
						}
						if (!pawn.Drafted)
						{
							FloatMenuMakerMap.AddUndraftedOrders(clickPos, pawn, list);
						}
						foreach (FloatMenuOption item in pawn.GetExtraFloatMenuOptionsFor(intVec))
						{
							list.Add(item);
						}
					}
				}
				finally
				{
					FloatMenuMakerMap.makingFor = null;
				}
				result = list;
			}
			return result;
		}

		// Token: 0x06002FBF RID: 12223 RVA: 0x001996D0 File Offset: 0x00197AD0
		private static void AddDraftedOrders(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
		{
			IntVec3 clickCell = IntVec3.FromVector3(clickPos);
			foreach (LocalTargetInfo attackTarg in GenUI.TargetsAt(clickPos, TargetingParameters.ForAttackHostile(), true))
			{
				FloatMenuMakerMap.<AddDraftedOrders>c__AnonStorey0 <AddDraftedOrders>c__AnonStorey2 = new FloatMenuMakerMap.<AddDraftedOrders>c__AnonStorey0();
				<AddDraftedOrders>c__AnonStorey2.attackTarg = attackTarg;
				string label;
				Action action;
				MenuOptionPriority priority;
				Thing thing;
				if (pawn.equipment.Primary != null && !pawn.equipment.PrimaryEq.PrimaryVerb.verbProps.IsMeleeAttack)
				{
					string str;
					Action rangedAct = FloatMenuUtility.GetRangedAttackAction(pawn, <AddDraftedOrders>c__AnonStorey2.attackTarg, out str);
					string text = "FireAt".Translate(new object[]
					{
						<AddDraftedOrders>c__AnonStorey2.attackTarg.Thing.Label
					});
					label = "";
					action = null;
					priority = MenuOptionPriority.High;
					thing = attackTarg.Thing;
					FloatMenuOption floatMenuOption = new FloatMenuOption(label, action, priority, null, thing, 0f, null, null);
					if (rangedAct == null)
					{
						text = text + " (" + str + ")";
					}
					else
					{
						floatMenuOption.autoTakeable = (!<AddDraftedOrders>c__AnonStorey2.attackTarg.HasThing || <AddDraftedOrders>c__AnonStorey2.attackTarg.Thing.HostileTo(Faction.OfPlayer));
						floatMenuOption.autoTakeablePriority = 40f;
						floatMenuOption.action = delegate()
						{
							MoteMaker.MakeStaticMote(<AddDraftedOrders>c__AnonStorey2.attackTarg.Thing.DrawPos, <AddDraftedOrders>c__AnonStorey2.attackTarg.Thing.Map, ThingDefOf.Mote_FeedbackShoot, 1f);
							rangedAct();
						};
					}
					floatMenuOption.Label = text;
					opts.Add(floatMenuOption);
				}
				string str2;
				Action meleeAct = FloatMenuUtility.GetMeleeAttackAction(pawn, <AddDraftedOrders>c__AnonStorey2.attackTarg, out str2);
				Pawn pawn2 = <AddDraftedOrders>c__AnonStorey2.attackTarg.Thing as Pawn;
				string text2;
				if (pawn2 != null && pawn2.Downed)
				{
					text2 = "MeleeAttackToDeath".Translate(new object[]
					{
						<AddDraftedOrders>c__AnonStorey2.attackTarg.Thing.Label
					});
				}
				else
				{
					text2 = "MeleeAttack".Translate(new object[]
					{
						<AddDraftedOrders>c__AnonStorey2.attackTarg.Thing.Label
					});
				}
				MenuOptionPriority menuOptionPriority = (!<AddDraftedOrders>c__AnonStorey2.attackTarg.HasThing || !pawn.HostileTo(<AddDraftedOrders>c__AnonStorey2.attackTarg.Thing)) ? MenuOptionPriority.VeryLow : MenuOptionPriority.AttackEnemy;
				label = "";
				action = null;
				priority = menuOptionPriority;
				thing = <AddDraftedOrders>c__AnonStorey2.attackTarg.Thing;
				FloatMenuOption floatMenuOption2 = new FloatMenuOption(label, action, priority, null, thing, 0f, null, null);
				if (meleeAct == null)
				{
					text2 = text2 + " (" + str2 + ")";
				}
				else
				{
					floatMenuOption2.autoTakeable = (!<AddDraftedOrders>c__AnonStorey2.attackTarg.HasThing || <AddDraftedOrders>c__AnonStorey2.attackTarg.Thing.HostileTo(Faction.OfPlayer));
					floatMenuOption2.autoTakeablePriority = 30f;
					floatMenuOption2.action = delegate()
					{
						MoteMaker.MakeStaticMote(<AddDraftedOrders>c__AnonStorey2.attackTarg.Thing.DrawPos, <AddDraftedOrders>c__AnonStorey2.attackTarg.Thing.Map, ThingDefOf.Mote_FeedbackMelee, 1f);
						meleeAct();
					};
				}
				floatMenuOption2.Label = text2;
				opts.Add(floatMenuOption2);
			}
			if (pawn.RaceProps.Humanlike && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				foreach (LocalTargetInfo localTargetInfo in GenUI.TargetsAt(clickPos, TargetingParameters.ForArrest(pawn), true))
				{
					LocalTargetInfo dest = localTargetInfo;
					if (!pawn.CanReach(dest, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						opts.Add(new FloatMenuOption("CannotArrest".Translate() + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					else
					{
						Pawn pTarg = (Pawn)dest.Thing;
						Action action2 = delegate()
						{
							Building_Bed building_Bed = RestUtility.FindBedFor(pTarg, pawn, true, false, false);
							if (building_Bed == null)
							{
								building_Bed = RestUtility.FindBedFor(pTarg, pawn, true, false, true);
							}
							if (building_Bed == null)
							{
								Messages.Message("CannotArrest".Translate() + ": " + "NoPrisonerBed".Translate(), pTarg, MessageTypeDefOf.RejectInput, false);
							}
							else
							{
								Job job = new Job(JobDefOf.Arrest, pTarg, building_Bed);
								job.count = 1;
								pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
								if (pTarg.Faction != null)
								{
									TutorUtility.DoModalDialogIfNotKnown(ConceptDefOf.ArrestingCreatesEnemies);
								}
							}
						};
						string label = "TryToArrest".Translate(new object[]
						{
							dest.Thing.LabelCap
						});
						Action action = action2;
						MenuOptionPriority priority = MenuOptionPriority.High;
						Thing thing = dest.Thing;
						opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action, priority, null, thing, 0f, null, null), pawn, pTarg, "ReservedBy"));
					}
				}
			}
			FloatMenuMakerMap.AddJobGiverWorkOrders(clickCell, pawn, opts, true);
			FloatMenuOption floatMenuOption3 = FloatMenuMakerMap.GotoLocationOption(clickCell, pawn);
			if (floatMenuOption3 != null)
			{
				opts.Add(floatMenuOption3);
			}
		}

		// Token: 0x06002FC0 RID: 12224 RVA: 0x00199BF0 File Offset: 0x00197FF0
		private static void AddHumanlikeOrders(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
		{
			IntVec3 c = IntVec3.FromVector3(clickPos);
			foreach (Thing t2 in c.GetThingList(pawn.Map))
			{
				Thing t = t2;
				if (t.def.ingestible != null && pawn.RaceProps.CanEverEat(t) && t.IngestibleNow)
				{
					string text;
					if (t.def.ingestible.ingestCommandString.NullOrEmpty())
					{
						text = "ConsumeThing".Translate(new object[]
						{
							t.LabelShort
						});
					}
					else
					{
						text = string.Format(t.def.ingestible.ingestCommandString, t.LabelShort);
					}
					if (!t.IsSociallyProper(pawn))
					{
						text = text + " (" + "ReservedForPrisoners".Translate() + ")";
					}
					FloatMenuOption item7;
					if (t.def.IsNonMedicalDrug && pawn.IsTeetotaler())
					{
						item7 = new FloatMenuOption(text + " (" + TraitDefOf.DrugDesire.DataAtDegree(-1).label + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else if (!pawn.CanReach(t, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						item7 = new FloatMenuOption(text + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else
					{
						MenuOptionPriority priority = (!(t is Corpse)) ? MenuOptionPriority.Default : MenuOptionPriority.Low;
						item7 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, delegate()
						{
							t.SetForbidden(false, true);
							Job job = new Job(JobDefOf.Ingest, t);
							job.count = FoodUtility.WillIngestStackCountOf(pawn, t.def, t.GetStatValue(StatDefOf.Nutrition, true));
							pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
						}, priority, null, null, 0f, null, null), pawn, t, "ReservedBy");
					}
					opts.Add(item7);
				}
			}
			using (IEnumerator<LocalTargetInfo> enumerator2 = GenUI.TargetsAt(clickPos, TargetingParameters.ForQuestPawnsWhoWillJoinColony(pawn), true).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					LocalTargetInfo toHelp = enumerator2.Current;
					Pawn pawn2 = (Pawn)toHelp.Thing;
					FloatMenuOption item2;
					if (!pawn.CanReach(toHelp, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						item2 = new FloatMenuOption("CannotGoNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else
					{
						Action action = delegate()
						{
							IntVec3 c2 = RCellFinder.BestOrderedGotoDestNear(toHelp.Cell, pawn);
							pawn.jobs.TryTakeOrderedJob(new Job(JobDefOf.Goto, c2), JobTag.Misc);
						};
						string text2 = (!pawn2.IsPrisoner) ? "OfferHelp".Translate() : "FreePrisoner".Translate();
						string label = text2;
						Action action2 = action;
						MenuOptionPriority priority2 = MenuOptionPriority.RescueOrCapture;
						Pawn revalidateClickTarget = pawn2;
						item2 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action2, priority2, null, revalidateClickTarget, 0f, null, null), pawn, pawn2, "ReservedBy");
					}
					opts.Add(item2);
				}
			}
			if (pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				foreach (LocalTargetInfo localTargetInfo in GenUI.TargetsAt(clickPos, TargetingParameters.ForRescue(pawn), true))
				{
					Pawn victim = (Pawn)localTargetInfo.Thing;
					if (!victim.InBed() && pawn.CanReserveAndReach(victim, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, true))
					{
						if (!victim.mindState.willJoinColonyIfRescued)
						{
							if (!victim.IsPrisonerOfColony && !victim.InMentalState)
							{
								if (victim.Faction == Faction.OfPlayer || victim.Faction == null || !victim.Faction.HostileTo(Faction.OfPlayer))
								{
									string label = "Rescue".Translate(new object[]
									{
										victim.LabelCap
									});
									Action action2 = delegate()
									{
										Building_Bed building_Bed = RestUtility.FindBedFor(victim, pawn, false, false, false);
										if (building_Bed == null)
										{
											building_Bed = RestUtility.FindBedFor(victim, pawn, false, false, true);
										}
										if (building_Bed == null)
										{
											string str2;
											if (victim.RaceProps.Animal)
											{
												str2 = "NoAnimalBed".Translate();
											}
											else
											{
												str2 = "NoNonPrisonerBed".Translate();
											}
											Messages.Message("CannotRescue".Translate() + ": " + str2, victim, MessageTypeDefOf.RejectInput, false);
										}
										else
										{
											Job job = new Job(JobDefOf.Rescue, victim, building_Bed);
											job.count = 1;
											pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
											PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Rescuing, KnowledgeAmount.Total);
										}
									};
									MenuOptionPriority priority2 = MenuOptionPriority.RescueOrCapture;
									Pawn revalidateClickTarget = victim;
									opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action2, priority2, null, revalidateClickTarget, 0f, null, null), pawn, victim, "ReservedBy"));
								}
							}
							if (!victim.NonHumanlikeOrWildMan() && (victim.InMentalState || victim.Faction != Faction.OfPlayer || (victim.Downed && (victim.guilt.IsGuilty || victim.IsPrisonerOfColony))))
							{
								string label = "Capture".Translate(new object[]
								{
									victim.LabelCap
								});
								Action action2 = delegate()
								{
									Building_Bed building_Bed = RestUtility.FindBedFor(victim, pawn, true, false, false);
									if (building_Bed == null)
									{
										building_Bed = RestUtility.FindBedFor(victim, pawn, true, false, true);
									}
									if (building_Bed == null)
									{
										Messages.Message("CannotCapture".Translate() + ": " + "NoPrisonerBed".Translate(), victim, MessageTypeDefOf.RejectInput, false);
									}
									else
									{
										Job job = new Job(JobDefOf.Capture, victim, building_Bed);
										job.count = 1;
										pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
										PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Capturing, KnowledgeAmount.Total);
									}
								};
								MenuOptionPriority priority2 = MenuOptionPriority.RescueOrCapture;
								Pawn revalidateClickTarget = victim;
								opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action2, priority2, null, revalidateClickTarget, 0f, null, null), pawn, victim, "ReservedBy"));
							}
						}
					}
				}
				foreach (LocalTargetInfo localTargetInfo2 in GenUI.TargetsAt(clickPos, TargetingParameters.ForRescue(pawn), true))
				{
					LocalTargetInfo localTargetInfo3 = localTargetInfo2;
					Pawn victim = (Pawn)localTargetInfo3.Thing;
					if (victim.Downed && pawn.CanReserveAndReach(victim, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, true) && Building_CryptosleepCasket.FindCryptosleepCasketFor(victim, pawn, true) != null)
					{
						string text3 = "CarryToCryptosleepCasket".Translate(new object[]
						{
							localTargetInfo3.Thing.LabelCap
						});
						JobDef jDef = JobDefOf.CarryToCryptosleepCasket;
						Action action3 = delegate()
						{
							Building_CryptosleepCasket building_CryptosleepCasket = Building_CryptosleepCasket.FindCryptosleepCasketFor(victim, pawn, false);
							if (building_CryptosleepCasket == null)
							{
								building_CryptosleepCasket = Building_CryptosleepCasket.FindCryptosleepCasketFor(victim, pawn, true);
							}
							if (building_CryptosleepCasket == null)
							{
								Messages.Message("CannotCarryToCryptosleepCasket".Translate() + ": " + "NoCryptosleepCasket".Translate(), victim, MessageTypeDefOf.RejectInput, false);
							}
							else
							{
								Job job = new Job(jDef, victim, building_CryptosleepCasket);
								job.count = 1;
								pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
							}
						};
						string label = text3;
						Action action2 = action3;
						Pawn revalidateClickTarget = victim;
						opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action2, MenuOptionPriority.Default, null, revalidateClickTarget, 0f, null, null), pawn, victim, "ReservedBy"));
					}
				}
			}
			foreach (LocalTargetInfo stripTarg2 in GenUI.TargetsAt(clickPos, TargetingParameters.ForStrip(pawn), true))
			{
				LocalTargetInfo stripTarg = stripTarg2;
				FloatMenuOption item3;
				if (!pawn.CanReach(stripTarg, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					item3 = new FloatMenuOption("CannotStrip".Translate(new object[]
					{
						stripTarg.Thing.LabelCap
					}) + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
				}
				else
				{
					item3 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Strip".Translate(new object[]
					{
						stripTarg.Thing.LabelCap
					}), delegate()
					{
						stripTarg.Thing.SetForbidden(false, false);
						pawn.jobs.TryTakeOrderedJob(new Job(JobDefOf.Strip, stripTarg), JobTag.Misc);
					}, MenuOptionPriority.Default, null, null, 0f, null, null), pawn, stripTarg, "ReservedBy");
				}
				opts.Add(item3);
			}
			if (pawn.equipment != null)
			{
				ThingWithComps equipment = null;
				List<Thing> thingList = c.GetThingList(pawn.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i].TryGetComp<CompEquippable>() != null)
					{
						equipment = (ThingWithComps)thingList[i];
						break;
					}
				}
				if (equipment != null)
				{
					string labelShort = equipment.LabelShort;
					FloatMenuOption item4;
					if (equipment.def.IsWeapon && pawn.story.WorkTagIsDisabled(WorkTags.Violent))
					{
						item4 = new FloatMenuOption("CannotEquip".Translate(new object[]
						{
							labelShort
						}) + " (" + "IsIncapableOfViolenceLower".Translate(new object[]
						{
							pawn.LabelShort
						}) + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else if (!pawn.CanReach(equipment, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						item4 = new FloatMenuOption("CannotEquip".Translate(new object[]
						{
							labelShort
						}) + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
					{
						item4 = new FloatMenuOption("CannotEquip".Translate(new object[]
						{
							labelShort
						}) + " (" + "Incapable".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else
					{
						string text4 = "Equip".Translate(new object[]
						{
							labelShort
						});
						if (equipment.def.IsRangedWeapon && pawn.story != null && pawn.story.traits.HasTrait(TraitDefOf.Brawler))
						{
							text4 = text4 + " " + "EquipWarningBrawler".Translate();
						}
						item4 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text4, delegate()
						{
							equipment.SetForbidden(false, true);
							pawn.jobs.TryTakeOrderedJob(new Job(JobDefOf.Equip, equipment), JobTag.Misc);
							MoteMaker.MakeStaticMote(equipment.DrawPos, equipment.Map, ThingDefOf.Mote_FeedbackEquip, 1f);
							PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.EquippingWeapons, KnowledgeAmount.Total);
						}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, equipment, "ReservedBy");
					}
					opts.Add(item4);
				}
			}
			if (pawn.apparel != null)
			{
				Apparel apparel = pawn.Map.thingGrid.ThingAt<Apparel>(c);
				if (apparel != null)
				{
					FloatMenuOption item5;
					if (!pawn.CanReach(apparel, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						item5 = new FloatMenuOption("CannotWear".Translate(new object[]
						{
							apparel.Label
						}) + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else if (!ApparelUtility.HasPartsToWear(pawn, apparel.def))
					{
						item5 = new FloatMenuOption("CannotWear".Translate(new object[]
						{
							apparel.Label
						}) + " (" + "CannotWearBecauseOfMissingBodyParts".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else
					{
						item5 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("ForceWear".Translate(new object[]
						{
							apparel.LabelShort
						}), delegate()
						{
							apparel.SetForbidden(false, true);
							Job job = new Job(JobDefOf.Wear, apparel);
							pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
						}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, apparel, "ReservedBy");
					}
					opts.Add(item5);
				}
			}
			if (pawn.IsFormingCaravan())
			{
				Thing item = c.GetFirstItem(pawn.Map);
				if (item != null && item.def.EverHaulable)
				{
					Pawn packTarget = GiveToPackAnimalUtility.UsablePackAnimalWithTheMostFreeSpace(pawn) ?? pawn;
					JobDef jobDef = (packTarget != pawn) ? JobDefOf.GiveToPackAnimal : JobDefOf.TakeInventory;
					if (!pawn.CanReach(item, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						opts.Add(new FloatMenuOption("CannotLoadIntoCaravan".Translate(new object[]
						{
							item.Label
						}) + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					else if (MassUtility.WillBeOverEncumberedAfterPickingUp(packTarget, item, 1))
					{
						opts.Add(new FloatMenuOption("CannotLoadIntoCaravan".Translate(new object[]
						{
							item.Label
						}) + " (" + "TooHeavy".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					else
					{
						LordJob_FormAndSendCaravan lordJob = (LordJob_FormAndSendCaravan)pawn.GetLord().LordJob;
						float capacityLeft = CaravanFormingUtility.CapacityLeft(lordJob);
						if (item.stackCount == 1)
						{
							float capacityLeft4 = capacityLeft - item.GetStatValue(StatDefOf.Mass, true);
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(CaravanFormingUtility.AppendOverweightInfo("LoadIntoCaravan".Translate(new object[]
							{
								item.Label
							}), capacityLeft4), delegate()
							{
								item.SetForbidden(false, false);
								Job job = new Job(jobDef, item);
								job.count = 1;
								job.checkEncumbrance = (packTarget == pawn);
								pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
							}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, item, "ReservedBy"));
						}
						else
						{
							if (MassUtility.WillBeOverEncumberedAfterPickingUp(packTarget, item, item.stackCount))
							{
								opts.Add(new FloatMenuOption("CannotLoadIntoCaravanAll".Translate(new object[]
								{
									item.Label
								}) + " (" + "TooHeavy".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
							}
							else
							{
								float capacityLeft2 = capacityLeft - (float)item.stackCount * item.GetStatValue(StatDefOf.Mass, true);
								opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(CaravanFormingUtility.AppendOverweightInfo("LoadIntoCaravanAll".Translate(new object[]
								{
									item.Label
								}), capacityLeft2), delegate()
								{
									item.SetForbidden(false, false);
									Job job = new Job(jobDef, item);
									job.count = item.stackCount;
									job.checkEncumbrance = (packTarget == pawn);
									pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
								}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, item, "ReservedBy"));
							}
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("LoadIntoCaravanSome".Translate(new object[]
							{
								item.LabelNoCount
							}), delegate()
							{
								int to = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(packTarget, item), item.stackCount);
								Dialog_Slider window = new Dialog_Slider(delegate(int val)
								{
									float capacityLeft3 = capacityLeft - (float)val * item.GetStatValue(StatDefOf.Mass, true);
									return CaravanFormingUtility.AppendOverweightInfo(string.Format("LoadIntoCaravanCount".Translate(new object[]
									{
										item.LabelNoCount
									}), val), capacityLeft3);
								}, 1, to, delegate(int count)
								{
									item.SetForbidden(false, false);
									Job job = new Job(jobDef, item);
									job.count = count;
									job.checkEncumbrance = (packTarget == pawn);
									pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
								}, int.MinValue);
								Find.WindowStack.Add(window);
							}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, item, "ReservedBy"));
						}
					}
				}
			}
			if (!pawn.Map.IsPlayerHome && !pawn.IsFormingCaravan())
			{
				Thing item = c.GetFirstItem(pawn.Map);
				if (item != null && item.def.EverHaulable)
				{
					if (!pawn.CanReach(item, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						opts.Add(new FloatMenuOption("CannotPickUp".Translate(new object[]
						{
							item.Label
						}) + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					else if (MassUtility.WillBeOverEncumberedAfterPickingUp(pawn, item, 1))
					{
						opts.Add(new FloatMenuOption("CannotPickUp".Translate(new object[]
						{
							item.Label
						}) + " (" + "TooHeavy".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					else if (item.stackCount == 1)
					{
						opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PickUp".Translate(new object[]
						{
							item.Label
						}), delegate()
						{
							item.SetForbidden(false, false);
							Job job = new Job(JobDefOf.TakeInventory, item);
							job.count = 1;
							job.checkEncumbrance = true;
							pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
						}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, item, "ReservedBy"));
					}
					else
					{
						if (MassUtility.WillBeOverEncumberedAfterPickingUp(pawn, item, item.stackCount))
						{
							opts.Add(new FloatMenuOption("CannotPickUpAll".Translate(new object[]
							{
								item.Label
							}) + " (" + "TooHeavy".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
						else
						{
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PickUpAll".Translate(new object[]
							{
								item.Label
							}), delegate()
							{
								item.SetForbidden(false, false);
								Job job = new Job(JobDefOf.TakeInventory, item);
								job.count = item.stackCount;
								job.checkEncumbrance = true;
								pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
							}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, item, "ReservedBy"));
						}
						opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PickUpSome".Translate(new object[]
						{
							item.LabelNoCount
						}), delegate()
						{
							int to = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(pawn, item), item.stackCount);
							Dialog_Slider window = new Dialog_Slider("PickUpCount".Translate(new object[]
							{
								item.LabelNoCount
							}), 1, to, delegate(int count)
							{
								item.SetForbidden(false, false);
								Job job = new Job(JobDefOf.TakeInventory, item);
								job.count = count;
								job.checkEncumbrance = true;
								pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
							}, int.MinValue);
							Find.WindowStack.Add(window);
						}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, item, "ReservedBy"));
					}
				}
			}
			if (!pawn.Map.IsPlayerHome && !pawn.IsFormingCaravan())
			{
				Thing item = c.GetFirstItem(pawn.Map);
				if (item != null && item.def.EverHaulable)
				{
					Pawn bestPackAnimal = GiveToPackAnimalUtility.UsablePackAnimalWithTheMostFreeSpace(pawn);
					if (bestPackAnimal != null)
					{
						if (!pawn.CanReach(item, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
						{
							opts.Add(new FloatMenuOption("CannotGiveToPackAnimal".Translate(new object[]
							{
								item.Label
							}) + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
						else if (MassUtility.WillBeOverEncumberedAfterPickingUp(bestPackAnimal, item, 1))
						{
							opts.Add(new FloatMenuOption("CannotGiveToPackAnimal".Translate(new object[]
							{
								item.Label
							}) + " (" + "TooHeavy".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
						else if (item.stackCount == 1)
						{
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("GiveToPackAnimal".Translate(new object[]
							{
								item.Label
							}), delegate()
							{
								item.SetForbidden(false, false);
								Job job = new Job(JobDefOf.GiveToPackAnimal, item);
								job.count = 1;
								pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
							}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, item, "ReservedBy"));
						}
						else
						{
							if (MassUtility.WillBeOverEncumberedAfterPickingUp(bestPackAnimal, item, item.stackCount))
							{
								opts.Add(new FloatMenuOption("CannotGiveToPackAnimalAll".Translate(new object[]
								{
									item.Label
								}) + " (" + "TooHeavy".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
							}
							else
							{
								opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("GiveToPackAnimalAll".Translate(new object[]
								{
									item.Label
								}), delegate()
								{
									item.SetForbidden(false, false);
									Job job = new Job(JobDefOf.GiveToPackAnimal, item);
									job.count = item.stackCount;
									pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
								}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, item, "ReservedBy"));
							}
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("GiveToPackAnimalSome".Translate(new object[]
							{
								item.LabelNoCount
							}), delegate()
							{
								int to = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(bestPackAnimal, item), item.stackCount);
								Dialog_Slider window = new Dialog_Slider("GiveToPackAnimalCount".Translate(new object[]
								{
									item.LabelNoCount
								}), 1, to, delegate(int count)
								{
									item.SetForbidden(false, false);
									Job job = new Job(JobDefOf.GiveToPackAnimal, item);
									job.count = count;
									pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
								}, int.MinValue);
								Find.WindowStack.Add(window);
							}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, item, "ReservedBy"));
						}
					}
				}
			}
			if (!pawn.Map.IsPlayerHome && pawn.Map.exitMapGrid.MapUsesExitGrid)
			{
				foreach (LocalTargetInfo target in GenUI.TargetsAt(clickPos, TargetingParameters.ForRescue(pawn), true))
				{
					Pawn p = (Pawn)target.Thing;
					if (p.RaceProps.Humanlike || p.Faction == Faction.OfPlayer)
					{
						if (!pawn.CanReach(p, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
						{
							opts.Add(new FloatMenuOption("CannotCarryToExit".Translate(new object[]
							{
								p.Label
							}) + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
						else
						{
							IntVec3 exitSpot;
							if (!RCellFinder.TryFindBestExitSpot(pawn, out exitSpot, TraverseMode.ByPawn))
							{
								opts.Add(new FloatMenuOption("CannotCarryToExit".Translate(new object[]
								{
									p.Label
								}) + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
							}
							else
							{
								string label2 = (p.Faction != Faction.OfPlayer && !p.IsPrisonerOfColony) ? "CarryToExitAndCapture".Translate(new object[]
								{
									p.Label
								}) : "CarryToExit".Translate(new object[]
								{
									p.Label
								});
								opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label2, delegate()
								{
									Job job = new Job(JobDefOf.CarryDownedPawnToExit, p, exitSpot);
									job.count = 1;
									pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
								}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, target, "ReservedBy"));
							}
						}
					}
				}
			}
			if (pawn.equipment != null && pawn.equipment.Primary != null && GenUI.TargetsAt(clickPos, TargetingParameters.ForSelf(pawn), true).Any<LocalTargetInfo>())
			{
				Action action4 = delegate()
				{
					pawn.jobs.TryTakeOrderedJob(new Job(JobDefOf.DropEquipment, pawn.equipment.Primary), JobTag.Misc);
				};
				string label = "Drop".Translate(new object[]
				{
					pawn.equipment.Primary.Label
				});
				Action action2 = action4;
				Pawn revalidateClickTarget = pawn;
				opts.Add(new FloatMenuOption(label, action2, MenuOptionPriority.Default, null, revalidateClickTarget, 0f, null, null));
			}
			foreach (LocalTargetInfo localTargetInfo4 in GenUI.TargetsAt(clickPos, TargetingParameters.ForTrade(), true))
			{
				LocalTargetInfo dest = localTargetInfo4;
				if (!pawn.CanReach(dest, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					opts.Add(new FloatMenuOption("CannotTrade".Translate() + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				else if (pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
				{
					opts.Add(new FloatMenuOption("CannotPrioritizeWorkTypeDisabled".Translate(new object[]
					{
						SkillDefOf.Social.LabelCap
					}), null, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				else
				{
					Pawn pTarg = (Pawn)dest.Thing;
					Action action5 = delegate()
					{
						Job job = new Job(JobDefOf.TradeWithPawn, pTarg);
						job.playerForced = true;
						pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.InteractingWithTraders, KnowledgeAmount.Total);
					};
					string str = "";
					if (pTarg.Faction != null)
					{
						str = " (" + pTarg.Faction.Name + ")";
					}
					string label = "TradeWith".Translate(new object[]
					{
						pTarg.LabelShort + ", " + pTarg.TraderKind.label
					}) + str;
					Action action2 = action5;
					MenuOptionPriority priority2 = MenuOptionPriority.InitiateSocial;
					Thing thing = dest.Thing;
					opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action2, priority2, null, thing, 0f, null, null), pawn, pTarg, "ReservedBy"));
				}
			}
			foreach (Thing thing2 in pawn.Map.thingGrid.ThingsAt(c))
			{
				foreach (FloatMenuOption item6 in thing2.GetFloatMenuOptions(pawn))
				{
					opts.Add(item6);
				}
			}
		}

		// Token: 0x06002FC1 RID: 12225 RVA: 0x0019B960 File Offset: 0x00199D60
		private static void AddUndraftedOrders(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
		{
			if (FloatMenuMakerMap.equivalenceGroupTempStorage == null || FloatMenuMakerMap.equivalenceGroupTempStorage.Length != DefDatabase<WorkGiverEquivalenceGroupDef>.DefCount)
			{
				FloatMenuMakerMap.equivalenceGroupTempStorage = new FloatMenuOption[DefDatabase<WorkGiverEquivalenceGroupDef>.DefCount];
			}
			IntVec3 intVec = IntVec3.FromVector3(clickPos);
			bool flag = false;
			bool flag2 = false;
			foreach (Thing t in pawn.Map.thingGrid.ThingsAt(intVec))
			{
				flag2 = true;
				if (pawn.CanReach(t, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					flag = true;
					break;
				}
			}
			if (flag2 && !flag)
			{
				opts.Add(new FloatMenuOption("(" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
			}
			else
			{
				FloatMenuMakerMap.AddJobGiverWorkOrders(intVec, pawn, opts, false);
			}
		}

		// Token: 0x06002FC2 RID: 12226 RVA: 0x0019BA64 File Offset: 0x00199E64
		private static void AddJobGiverWorkOrders(IntVec3 clickCell, Pawn pawn, List<FloatMenuOption> opts, bool drafted)
		{
			if (pawn.thinker.TryGetMainTreeThinkNode<JobGiver_Work>() != null)
			{
				foreach (Thing thing in pawn.Map.thingGrid.ThingsAt(clickCell))
				{
					bool flag = false;
					foreach (WorkTypeDef workTypeDef in DefDatabase<WorkTypeDef>.AllDefsListForReading)
					{
						for (int i = 0; i < workTypeDef.workGiversByPriority.Count; i++)
						{
							WorkGiverDef workGiver = workTypeDef.workGiversByPriority[i];
							if (!drafted || workGiver.canBeDoneWhileDrafted)
							{
								WorkGiver_Scanner workGiver_Scanner = workGiver.Worker as WorkGiver_Scanner;
								if (workGiver_Scanner != null && workGiver_Scanner.def.directOrderable && !workGiver_Scanner.ShouldSkip(pawn, true))
								{
									JobFailReason.Clear();
									if (workGiver_Scanner.PotentialWorkThingRequest.Accepts(thing) || (workGiver_Scanner.PotentialWorkThingsGlobal(pawn) != null && workGiver_Scanner.PotentialWorkThingsGlobal(pawn).Contains(thing)))
									{
										Action action = null;
										PawnCapacityDef pawnCapacityDef = workGiver_Scanner.MissingRequiredCapacity(pawn);
										string text;
										if (pawnCapacityDef != null)
										{
											text = "CannotMissingHealthActivities".Translate(new object[]
											{
												pawnCapacityDef.label
											});
										}
										else
										{
											Job job;
											if (!workGiver_Scanner.HasJobOnThing(pawn, thing, true))
											{
												job = null;
											}
											else
											{
												job = workGiver_Scanner.JobOnThing(pawn, thing, true);
											}
											if (job == null)
											{
												if (JobFailReason.HaveReason)
												{
													if (!JobFailReason.CustomJobString.NullOrEmpty())
													{
														text = "CannotGenericWorkCustom".Translate(new object[]
														{
															JobFailReason.CustomJobString
														});
													}
													else
													{
														text = "CannotGenericWork".Translate(new object[]
														{
															workGiver_Scanner.def.verb,
															thing.LabelShort
														});
													}
													text = text + " (" + JobFailReason.Reason + ")";
												}
												else
												{
													if (!thing.IsForbidden(pawn))
													{
														goto IL_6F2;
													}
													if (!thing.Position.InAllowedArea(pawn))
													{
														text = "CannotPrioritizeForbiddenOutsideAllowedArea".Translate() + " (" + pawn.playerSettings.EffectiveAreaRestriction.Label + ")";
													}
													else
													{
														text = "CannotPrioritizeForbidden".Translate(new object[]
														{
															thing.Label
														});
													}
												}
											}
											else
											{
												WorkTypeDef workType = workGiver_Scanner.def.workType;
												if (pawn.story != null && pawn.story.WorkTagIsDisabled(workGiver_Scanner.def.workTags))
												{
													text = "CannotPrioritizeWorkGiverDisabled".Translate(new object[]
													{
														workGiver_Scanner.def.label
													});
												}
												else if (pawn.jobs.curJob != null && pawn.jobs.curJob.JobIsSameAs(job))
												{
													text = "CannotGenericAlreadyAm".Translate(new object[]
													{
														workGiver_Scanner.def.gerund,
														thing.LabelShort
													});
												}
												else if (pawn.workSettings.GetPriority(workType) == 0)
												{
													if (pawn.story.WorkTypeIsDisabled(workType))
													{
														text = "CannotPrioritizeWorkTypeDisabled".Translate(new object[]
														{
															workType.gerundLabel
														});
													}
													else if ("CannotPrioritizeNotAssignedToWorkType".CanTranslate())
													{
														text = "CannotPrioritizeNotAssignedToWorkType".Translate(new object[]
														{
															workType.gerundLabel
														});
													}
													else
													{
														text = "CannotPrioritizeIsNotA".Translate(new object[]
														{
															pawn.LabelShort,
															workType.pawnLabel
														});
													}
												}
												else if (job.def == JobDefOf.Research && thing is Building_ResearchBench)
												{
													text = "CannotPrioritizeResearch".Translate();
												}
												else if (thing.IsForbidden(pawn))
												{
													if (!thing.Position.InAllowedArea(pawn))
													{
														text = "CannotPrioritizeForbiddenOutsideAllowedArea".Translate() + " (" + pawn.playerSettings.EffectiveAreaRestriction.Label + ")";
													}
													else
													{
														text = "CannotPrioritizeForbidden".Translate(new object[]
														{
															thing.Label
														});
													}
												}
												else if (!pawn.CanReach(thing, workGiver_Scanner.PathEndMode, Danger.Deadly, false, TraverseMode.ByPawn))
												{
													text = (thing.Label + ": " + "NoPath".Translate()).CapitalizeFirst();
												}
												else
												{
													text = "PrioritizeGeneric".Translate(new object[]
													{
														workGiver_Scanner.def.gerund,
														thing.Label
													});
													Job localJob = job;
													WorkGiver_Scanner localScanner = workGiver_Scanner;
													action = delegate()
													{
														bool flag2 = pawn.jobs.TryTakeOrderedJobPrioritizedWork(localJob, localScanner, clickCell);
														if (flag2 && workGiver.forceMote != null)
														{
															MoteMaker.MakeStaticMote(clickCell, pawn.Map, workGiver.forceMote, 1f);
														}
													};
												}
											}
										}
										if (DebugViewSettings.showFloatMenuWorkGivers)
										{
											text += string.Format(" (from {0})", workGiver.defName);
										}
										FloatMenuOption menuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, action, MenuOptionPriority.Default, null, null, 0f, null, null), pawn, thing, "ReservedBy");
										if (drafted && workGiver.autoTakeablePriorityDrafted != -1)
										{
											menuOption.autoTakeable = true;
											menuOption.autoTakeablePriority = (float)workGiver.autoTakeablePriorityDrafted;
										}
										if (!opts.Any((FloatMenuOption op) => op.Label == menuOption.Label))
										{
											if (workGiver.equivalenceGroup != null)
											{
												if (FloatMenuMakerMap.equivalenceGroupTempStorage[(int)workGiver.equivalenceGroup.index] == null || (FloatMenuMakerMap.equivalenceGroupTempStorage[(int)workGiver.equivalenceGroup.index].Disabled && !menuOption.Disabled))
												{
													FloatMenuMakerMap.equivalenceGroupTempStorage[(int)workGiver.equivalenceGroup.index] = menuOption;
													flag = true;
												}
											}
											else
											{
												opts.Add(menuOption);
											}
										}
									}
								}
							}
							IL_6F2:;
						}
					}
					if (flag)
					{
						for (int j = 0; j < FloatMenuMakerMap.equivalenceGroupTempStorage.Length; j++)
						{
							if (FloatMenuMakerMap.equivalenceGroupTempStorage[j] != null)
							{
								opts.Add(FloatMenuMakerMap.equivalenceGroupTempStorage[j]);
								FloatMenuMakerMap.equivalenceGroupTempStorage[j] = null;
							}
						}
					}
				}
				foreach (WorkTypeDef workTypeDef2 in DefDatabase<WorkTypeDef>.AllDefsListForReading)
				{
					for (int k = 0; k < workTypeDef2.workGiversByPriority.Count; k++)
					{
						WorkGiverDef workGiver = workTypeDef2.workGiversByPriority[k];
						if (!drafted || workGiver.canBeDoneWhileDrafted)
						{
							WorkGiver_Scanner workGiver_Scanner2 = workGiver.Worker as WorkGiver_Scanner;
							if (workGiver_Scanner2 != null && workGiver_Scanner2.def.directOrderable && !workGiver_Scanner2.ShouldSkip(pawn, true))
							{
								JobFailReason.Clear();
								if (workGiver_Scanner2.PotentialWorkCellsGlobal(pawn).Contains(clickCell))
								{
									Action action2 = null;
									string label = null;
									PawnCapacityDef pawnCapacityDef2 = workGiver_Scanner2.MissingRequiredCapacity(pawn);
									if (pawnCapacityDef2 != null)
									{
										label = "CannotMissingHealthActivities".Translate(new object[]
										{
											pawnCapacityDef2.label
										});
									}
									else
									{
										Job job2;
										if (!workGiver_Scanner2.HasJobOnCell(pawn, clickCell, true))
										{
											job2 = null;
										}
										else
										{
											job2 = workGiver_Scanner2.JobOnCell(pawn, clickCell, true);
										}
										if (job2 == null)
										{
											if (JobFailReason.HaveReason)
											{
												if (!JobFailReason.CustomJobString.NullOrEmpty())
												{
													label = "CannotGenericWorkCustom".Translate(new object[]
													{
														JobFailReason.CustomJobString
													});
												}
												else
												{
													label = "CannotGenericWork".Translate(new object[]
													{
														workGiver_Scanner2.def.verb,
														"AreaLower".Translate()
													});
												}
												label = label + " (" + JobFailReason.Reason + ")";
											}
											else
											{
												if (!clickCell.IsForbidden(pawn))
												{
													goto IL_D12;
												}
												if (!clickCell.InAllowedArea(pawn))
												{
													label = "CannotPrioritizeForbiddenOutsideAllowedArea".Translate() + " (" + pawn.playerSettings.EffectiveAreaRestriction.Label + ")";
												}
												else
												{
													label = "CannotPrioritizeCellForbidden".Translate();
												}
											}
										}
										else
										{
											WorkTypeDef workType2 = workGiver_Scanner2.def.workType;
											if (pawn.jobs.curJob != null && pawn.jobs.curJob.JobIsSameAs(job2))
											{
												label = "CannotGenericAlreadyAm".Translate(new object[]
												{
													workGiver_Scanner2.def.gerund,
													"AreaLower".Translate()
												});
											}
											else if (pawn.workSettings.GetPriority(workType2) == 0)
											{
												if (pawn.story.WorkTypeIsDisabled(workType2))
												{
													label = "CannotPrioritizeWorkTypeDisabled".Translate(new object[]
													{
														workType2.gerundLabel
													});
												}
												else if ("CannotPrioritizeNotAssignedToWorkType".CanTranslate())
												{
													label = "CannotPrioritizeNotAssignedToWorkType".Translate(new object[]
													{
														workType2.gerundLabel
													});
												}
												else
												{
													label = "CannotPrioritizeIsNotA".Translate(new object[]
													{
														pawn.LabelShort,
														workType2.pawnLabel
													});
												}
											}
											else if (clickCell.IsForbidden(pawn))
											{
												if (!clickCell.InAllowedArea(pawn))
												{
													label = "CannotPrioritizeForbiddenOutsideAllowedArea".Translate() + " (" + pawn.playerSettings.EffectiveAreaRestriction.Label + ")";
												}
												else
												{
													label = "CannotPrioritizeCellForbidden".Translate();
												}
											}
											else if (!pawn.CanReach(clickCell, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
											{
												label = "AreaLower".Translate().CapitalizeFirst() + ": " + "NoPath".Translate();
											}
											else
											{
												label = "PrioritizeGeneric".Translate(new object[]
												{
													workGiver_Scanner2.def.gerund,
													"AreaLower".Translate()
												});
												Job localJob = job2;
												WorkGiver_Scanner localScanner = workGiver_Scanner2;
												action2 = delegate()
												{
													bool flag2 = pawn.jobs.TryTakeOrderedJobPrioritizedWork(localJob, localScanner, clickCell);
													if (flag2 && workGiver.forceMote != null)
													{
														MoteMaker.MakeStaticMote(clickCell, pawn.Map, workGiver.forceMote, 1f);
													}
												};
											}
										}
									}
									if (!opts.Any((FloatMenuOption op) => op.Label == label.TrimEnd(new char[0])))
									{
										FloatMenuOption floatMenuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action2, MenuOptionPriority.Default, null, null, 0f, null, null), pawn, clickCell, "ReservedBy");
										if (drafted && workGiver.autoTakeablePriorityDrafted != -1)
										{
											floatMenuOption.autoTakeable = true;
											floatMenuOption.autoTakeablePriority = (float)workGiver.autoTakeablePriorityDrafted;
										}
										opts.Add(floatMenuOption);
									}
								}
							}
						}
						IL_D12:;
					}
				}
			}
		}

		// Token: 0x06002FC3 RID: 12227 RVA: 0x0019C80C File Offset: 0x0019AC0C
		private static FloatMenuOption GotoLocationOption(IntVec3 clickCell, Pawn pawn)
		{
			int num = GenRadial.NumCellsInRadius(2.9f);
			IntVec3 curLoc;
			for (int i = 0; i < num; i++)
			{
				curLoc = GenRadial.RadialPattern[i] + clickCell;
				if (curLoc.Standable(pawn.Map))
				{
					FloatMenuOption result;
					if (curLoc != pawn.Position)
					{
						if (!pawn.CanReach(curLoc, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
						{
							result = new FloatMenuOption("CannotGoNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
						}
						else
						{
							Action action = delegate()
							{
								IntVec3 intVec = RCellFinder.BestOrderedGotoDestNear(curLoc, pawn);
								Job job = new Job(JobDefOf.Goto, intVec);
								if (pawn.Map.exitMapGrid.IsExitCell(UI.MouseCell()))
								{
									job.exitMapOnArrival = true;
								}
								else if (!pawn.Map.IsPlayerHome && !pawn.Map.exitMapGrid.MapUsesExitGrid && CellRect.WholeMap(pawn.Map).IsOnEdge(UI.MouseCell(), 3) && pawn.Map.Parent.GetComponent<FormCaravanComp>() != null && MessagesRepeatAvoider.MessageShowAllowed("MessagePlayerTriedToLeaveMapViaExitGrid-" + pawn.Map.uniqueID, 60f))
								{
									Messages.Message("MessagePlayerTriedToLeaveMapViaExitGrid".Translate(), pawn.Map.Parent, MessageTypeDefOf.RejectInput, false);
								}
								if (pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc))
								{
									MoteMaker.MakeStaticMote(intVec, pawn.Map, ThingDefOf.Mote_FeedbackGoto, 1f);
								}
							};
							result = new FloatMenuOption("GoHere".Translate(), action, MenuOptionPriority.GoHere, null, null, 0f, null, null)
							{
								autoTakeable = true,
								autoTakeablePriority = 10f
							};
						}
					}
					else
					{
						result = null;
					}
					return result;
				}
			}
			return null;
		}

		// Token: 0x040019CC RID: 6604
		public static Pawn makingFor;

		// Token: 0x040019CD RID: 6605
		private static FloatMenuOption[] equivalenceGroupTempStorage;
	}
}
