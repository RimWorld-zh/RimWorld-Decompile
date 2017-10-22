using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class FloatMenuMakerMap
	{
		public static Pawn makingFor;

		private static bool CanTakeOrder(Pawn pawn)
		{
			return pawn.IsColonistPlayerControlled;
		}

		public static void TryMakeFloatMenu(Pawn pawn)
		{
			if (FloatMenuMakerMap.CanTakeOrder(pawn))
			{
				if (pawn.Downed)
				{
					Messages.Message("IsIncapped".Translate(pawn.LabelCap), (Thing)pawn, MessageTypeDefOf.RejectInput);
				}
				else if (pawn.Map == Find.VisibleMap)
				{
					List<FloatMenuOption> list = FloatMenuMakerMap.ChoicesAtFor(UI.MouseMapPosition(), pawn);
					if (list.Count != 0)
					{
						if (list.Count == 1 && list[0].autoTakeable)
						{
							list[0].Chosen(true);
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

		public static List<FloatMenuOption> ChoicesAtFor(Vector3 clickPos, Pawn pawn)
		{
			IntVec3 intVec = IntVec3.FromVector3(clickPos);
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			List<FloatMenuOption> result;
			if (!intVec.InBounds(pawn.Map) || !FloatMenuMakerMap.CanTakeOrder(pawn))
			{
				result = list;
			}
			else if (pawn.Map != Find.VisibleMap)
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
						FloatMenuOption floatMenuOption = FloatMenuMakerMap.GotoLocationOption(intVec, pawn);
						if (floatMenuOption != null && !floatMenuOption.Disabled)
						{
							list.Add(floatMenuOption);
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

		private static void AddDraftedOrders(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
		{
			IntVec3 clickCell = IntVec3.FromVector3(clickPos);
			foreach (LocalTargetInfo item in GenUI.TargetsAt(clickPos, TargetingParameters.ForAttackHostile(), true))
			{
				LocalTargetInfo attackTarg = item;
				if (pawn.equipment.Primary != null && !pawn.equipment.PrimaryEq.PrimaryVerb.verbProps.MeleeRange)
				{
					string str = default(string);
					Action rangedAct = FloatMenuUtility.GetRangedAttackAction(pawn, attackTarg, out str);
					string text = "FireAt".Translate(attackTarg.Thing.Label);
					FloatMenuOption floatMenuOption = new FloatMenuOption(MenuOptionPriority.High);
					if ((object)rangedAct == null)
					{
						text = text + " (" + str + ")";
					}
					else
					{
						floatMenuOption.autoTakeable = true;
						floatMenuOption.action = (Action)delegate
						{
							MoteMaker.MakeStaticMote(attackTarg.Thing.DrawPos, attackTarg.Thing.Map, ThingDefOf.Mote_FeedbackAttack, 1f);
							rangedAct();
						};
					}
					floatMenuOption.Label = text;
					opts.Add(floatMenuOption);
				}
				string str2 = default(string);
				Action meleeAct = FloatMenuUtility.GetMeleeAttackAction(pawn, attackTarg, out str2);
				Pawn pawn2 = attackTarg.Thing as Pawn;
				string text2 = (pawn2 == null || !pawn2.Downed) ? "MeleeAttack".Translate(attackTarg.Thing.Label) : "MeleeAttackToDeath".Translate(attackTarg.Thing.Label);
				MenuOptionPriority menuOptionPriority = (MenuOptionPriority)((!attackTarg.HasThing || !pawn.HostileTo(attackTarg.Thing)) ? 1 : 6);
				string label = "";
				Action action = null;
				MenuOptionPriority priority = menuOptionPriority;
				Thing thing = attackTarg.Thing;
				FloatMenuOption floatMenuOption2 = new FloatMenuOption(label, action, priority, null, thing, 0f, null, null);
				if ((object)meleeAct == null)
				{
					text2 = text2 + " (" + str2 + ")";
				}
				else
				{
					floatMenuOption2.action = (Action)delegate
					{
						MoteMaker.MakeStaticMote(attackTarg.Thing.DrawPos, attackTarg.Thing.Map, ThingDefOf.Mote_FeedbackAttack, 1f);
						meleeAct();
					};
				}
				floatMenuOption2.Label = text2;
				opts.Add(floatMenuOption2);
			}
			if (pawn.RaceProps.Humanlike && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				foreach (LocalTargetInfo item2 in GenUI.TargetsAt(clickPos, TargetingParameters.ForArrest(pawn), true))
				{
					LocalTargetInfo _ = item2;
					if (!pawn.CanReach(item2, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						opts.Add(new FloatMenuOption("CannotArrest".Translate() + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					else
					{
						Pawn pTarg = (Pawn)item2.Thing;
						Action action2 = (Action)delegate()
						{
							Building_Bed building_Bed = RestUtility.FindBedFor(pTarg, pawn, true, false, false);
							if (building_Bed == null)
							{
								building_Bed = RestUtility.FindBedFor(pTarg, pawn, true, false, true);
							}
							if (building_Bed == null)
							{
								Messages.Message("CannotArrest".Translate() + ": " + "NoPrisonerBed".Translate(), (Thing)pTarg, MessageTypeDefOf.RejectInput);
							}
							else
							{
								Job job = new Job(JobDefOf.Arrest, (Thing)pTarg, (Thing)building_Bed);
								job.count = 1;
								pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
								TutorUtility.DoModalDialogIfNotKnown(ConceptDefOf.ArrestingCreatesEnemies);
							}
						};
						string label = "TryToArrest".Translate(item2.Thing.LabelCap);
						Action action = action2;
						MenuOptionPriority priority = MenuOptionPriority.High;
						Thing thing = item2.Thing;
						opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action, priority, null, thing, 0f, null, null), pawn, (Thing)pTarg, "ReservedBy"));
					}
				}
			}
			FloatMenuOption floatMenuOption3 = FloatMenuMakerMap.GotoLocationOption(clickCell, pawn);
			if (floatMenuOption3 != null)
			{
				opts.Add(floatMenuOption3);
			}
		}

		private static void AddHumanlikeOrders(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
		{
			IntVec3 c = IntVec3.FromVector3(clickPos);
			foreach (Thing thing2 in c.GetThingList(pawn.Map))
			{
				Thing t = thing2;
				if (t.def.ingestible != null && pawn.RaceProps.CanEverEat(t) && t.IngestibleNow)
				{
					string text = (!t.def.ingestible.ingestCommandString.NullOrEmpty()) ? string.Format(t.def.ingestible.ingestCommandString, t.LabelShort) : "ConsumeThing".Translate(t.LabelShort);
					if (!t.IsSociallyProper(pawn))
					{
						text = text + " (" + "ReservedForPrisoners".Translate() + ")";
					}
					FloatMenuOption item3;
					if (t.def.IsNonMedicalDrug && pawn.IsTeetotaler())
					{
						item3 = new FloatMenuOption(text + " (" + TraitDefOf.DrugDesire.DataAtDegree(-1).label + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else if (!pawn.CanReach(t, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						item3 = new FloatMenuOption(text + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else
					{
						MenuOptionPriority priority = (MenuOptionPriority)((!(t is Corpse)) ? 4 : 2);
						item3 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, (Action)delegate()
						{
							t.SetForbidden(false, true);
							Job job13 = new Job(JobDefOf.Ingest, t);
							job13.count = FoodUtility.WillIngestStackCountOf(pawn, t.def);
							pawn.jobs.TryTakeOrderedJob(job13, JobTag.Misc);
						}, priority, null, null, 0f, null, null), pawn, t, "ReservedBy");
					}
					opts.Add(item3);
				}
			}
			if (pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				foreach (LocalTargetInfo item7 in GenUI.TargetsAt(clickPos, TargetingParameters.ForRescue(pawn), true))
				{
					Pawn victim = (Pawn)item7.Thing;
					if (!victim.InBed() && pawn.CanReserveAndReach((Thing)victim, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, true))
					{
						if (!victim.IsPrisonerOfColony && !victim.InMentalState && (victim.Faction == Faction.OfPlayer || victim.Faction == null || !victim.Faction.HostileTo(Faction.OfPlayer)))
						{
							string label = "Rescue".Translate(victim.LabelCap);
							Action action = (Action)delegate()
							{
								Building_Bed building_Bed2 = RestUtility.FindBedFor(victim, pawn, false, false, false);
								if (building_Bed2 == null)
								{
									building_Bed2 = RestUtility.FindBedFor(victim, pawn, false, false, true);
								}
								if (building_Bed2 == null)
								{
									string str2 = (!victim.RaceProps.Animal) ? "NoNonPrisonerBed".Translate() : "NoAnimalBed".Translate();
									Messages.Message("CannotRescue".Translate() + ": " + str2, (Thing)victim, MessageTypeDefOf.RejectInput);
								}
								else
								{
									Job job12 = new Job(JobDefOf.Rescue, (Thing)victim, (Thing)building_Bed2);
									job12.count = 1;
									pawn.jobs.TryTakeOrderedJob(job12, JobTag.Misc);
									PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Rescuing, KnowledgeAmount.Total);
								}
							};
							MenuOptionPriority priority2 = MenuOptionPriority.RescueOrCapture;
							Pawn revalidateClickTarget = victim;
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action, priority2, null, revalidateClickTarget, 0f, null, null), pawn, (Thing)victim, "ReservedBy"));
						}
						if (!victim.NonHumanlikeOrWildMan() && (victim.InMentalState || victim.Faction != Faction.OfPlayer || (victim.Downed && (victim.guilt.IsGuilty || victim.IsPrisonerOfColony))))
						{
							string label = "Capture".Translate(victim.LabelCap);
							Action action = (Action)delegate()
							{
								Building_Bed building_Bed = RestUtility.FindBedFor(victim, pawn, true, false, false);
								if (building_Bed == null)
								{
									building_Bed = RestUtility.FindBedFor(victim, pawn, true, false, true);
								}
								if (building_Bed == null)
								{
									Messages.Message("CannotCapture".Translate() + ": " + "NoPrisonerBed".Translate(), (Thing)victim, MessageTypeDefOf.RejectInput);
								}
								else
								{
									Job job11 = new Job(JobDefOf.Capture, (Thing)victim, (Thing)building_Bed);
									job11.count = 1;
									pawn.jobs.TryTakeOrderedJob(job11, JobTag.Misc);
									PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Capturing, KnowledgeAmount.Total);
								}
							};
							MenuOptionPriority priority2 = MenuOptionPriority.RescueOrCapture;
							Pawn revalidateClickTarget = victim;
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action, priority2, null, revalidateClickTarget, 0f, null, null), pawn, (Thing)victim, "ReservedBy"));
						}
					}
				}
				foreach (LocalTargetInfo item8 in GenUI.TargetsAt(clickPos, TargetingParameters.ForRescue(pawn), true))
				{
					LocalTargetInfo localTargetInfo = item8;
					Pawn victim2 = (Pawn)localTargetInfo.Thing;
					if (victim2.Downed && pawn.CanReserveAndReach((Thing)victim2, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, true) && Building_CryptosleepCasket.FindCryptosleepCasketFor(victim2, pawn, true) != null)
					{
						string text2 = "CarryToCryptosleepCasket".Translate(localTargetInfo.Thing.LabelCap);
						JobDef jDef = JobDefOf.CarryToCryptosleepCasket;
						Action action2 = (Action)delegate()
						{
							Building_CryptosleepCasket building_CryptosleepCasket = Building_CryptosleepCasket.FindCryptosleepCasketFor(victim2, pawn, false);
							if (building_CryptosleepCasket == null)
							{
								building_CryptosleepCasket = Building_CryptosleepCasket.FindCryptosleepCasketFor(victim2, pawn, true);
							}
							if (building_CryptosleepCasket == null)
							{
								Messages.Message("CannotCarryToCryptosleepCasket".Translate() + ": " + "NoCryptosleepCasket".Translate(), (Thing)victim2, MessageTypeDefOf.RejectInput);
							}
							else
							{
								Job job10 = new Job(jDef, (Thing)victim2, (Thing)building_CryptosleepCasket);
								job10.count = 1;
								pawn.jobs.TryTakeOrderedJob(job10, JobTag.Misc);
							}
						};
						string label = text2;
						Action action = action2;
						Pawn revalidateClickTarget = victim2;
						opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action, MenuOptionPriority.Default, null, revalidateClickTarget, 0f, null, null), pawn, (Thing)victim2, "ReservedBy"));
					}
				}
			}
			foreach (LocalTargetInfo item9 in GenUI.TargetsAt(clickPos, TargetingParameters.ForStrip(pawn), true))
			{
				LocalTargetInfo stripTarg = item9;
				FloatMenuOption item4 = pawn.CanReach(stripTarg, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn) ? FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Strip".Translate(stripTarg.Thing.LabelCap), (Action)delegate()
				{
					stripTarg.Thing.SetForbidden(false, false);
					pawn.jobs.TryTakeOrderedJob(new Job(JobDefOf.Strip, stripTarg), JobTag.Misc);
				}, MenuOptionPriority.Default, null, null, 0f, null, null), pawn, stripTarg, "ReservedBy") : new FloatMenuOption("CannotStrip".Translate(stripTarg.Thing.LabelCap) + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
				opts.Add(item4);
			}
			if (pawn.equipment != null)
			{
				ThingWithComps equipment = null;
				List<Thing> thingList = c.GetThingList(pawn.Map);
				int num = 0;
				while (num < thingList.Count)
				{
					if (thingList[num].TryGetComp<CompEquippable>() == null)
					{
						num++;
						continue;
					}
					equipment = (ThingWithComps)thingList[num];
					break;
				}
				if (equipment != null)
				{
					string labelShort = equipment.LabelShort;
					FloatMenuOption item5;
					if (equipment.def.IsWeapon && pawn.story.WorkTagIsDisabled(WorkTags.Violent))
					{
						item5 = new FloatMenuOption("CannotEquip".Translate(labelShort) + " (" + "IsIncapableOfViolenceLower".Translate(pawn.LabelShort) + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else if (!pawn.CanReach((Thing)equipment, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						item5 = new FloatMenuOption("CannotEquip".Translate(labelShort) + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
					{
						item5 = new FloatMenuOption("CannotEquip".Translate(labelShort) + " (" + "Incapable".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else
					{
						string text3 = "Equip".Translate(labelShort);
						if (equipment.def.IsRangedWeapon && pawn.story != null && pawn.story.traits.HasTrait(TraitDefOf.Brawler))
						{
							text3 = text3 + " " + "EquipWarningBrawler".Translate();
						}
						item5 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text3, (Action)delegate()
						{
							equipment.SetForbidden(false, true);
							pawn.jobs.TryTakeOrderedJob(new Job(JobDefOf.Equip, (Thing)equipment), JobTag.Misc);
							MoteMaker.MakeStaticMote(equipment.DrawPos, equipment.Map, ThingDefOf.Mote_FeedbackEquip, 1f);
							PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.EquippingWeapons, KnowledgeAmount.Total);
						}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, (Thing)equipment, "ReservedBy");
					}
					opts.Add(item5);
				}
			}
			if (pawn.apparel != null)
			{
				Apparel apparel = pawn.Map.thingGrid.ThingAt<Apparel>(c);
				if (apparel != null)
				{
					FloatMenuOption item6 = pawn.CanReach((Thing)apparel, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn) ? (ApparelUtility.HasPartsToWear(pawn, apparel.def) ? FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("ForceWear".Translate(apparel.LabelShort), (Action)delegate()
					{
						apparel.SetForbidden(false, true);
						Job job9 = new Job(JobDefOf.Wear, (Thing)apparel);
						pawn.jobs.TryTakeOrderedJob(job9, JobTag.Misc);
					}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, (Thing)apparel, "ReservedBy") : new FloatMenuOption("CannotWear".Translate(apparel.Label) + " (" + "CannotWearBecauseOfMissingBodyParts".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null)) : new FloatMenuOption("CannotWear".Translate(apparel.Label) + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
					opts.Add(item6);
				}
			}
			if (!pawn.Map.IsPlayerHome)
			{
				Thing item = c.GetFirstItem(pawn.Map);
				if (item != null && item.def.EverHaulable)
				{
					if (!pawn.CanReach(item, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						opts.Add(new FloatMenuOption("CannotPickUp".Translate(item.Label) + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					else if (MassUtility.WillBeOverEncumberedAfterPickingUp(pawn, item, 1))
					{
						opts.Add(new FloatMenuOption("CannotPickUp".Translate(item.Label) + " (" + "TooHeavy".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					else if (item.stackCount == 1)
					{
						opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PickUp".Translate(item.Label), (Action)delegate()
						{
							item.SetForbidden(false, false);
							Job job8 = new Job(JobDefOf.TakeInventory, item);
							job8.count = 1;
							pawn.jobs.TryTakeOrderedJob(job8, JobTag.Misc);
						}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, item, "ReservedBy"));
					}
					else
					{
						if (MassUtility.WillBeOverEncumberedAfterPickingUp(pawn, item, item.stackCount))
						{
							opts.Add(new FloatMenuOption("CannotPickUpAll".Translate(item.Label) + " (" + "TooHeavy".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
						else
						{
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PickUpAll".Translate(item.Label), (Action)delegate()
							{
								item.SetForbidden(false, false);
								Job job7 = new Job(JobDefOf.TakeInventory, item);
								job7.count = item.stackCount;
								pawn.jobs.TryTakeOrderedJob(job7, JobTag.Misc);
							}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, item, "ReservedBy"));
						}
						opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PickUpSome".Translate(item.Label), (Action)delegate()
						{
							int to2 = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(pawn, item), item.stackCount);
							Dialog_Slider window2 = new Dialog_Slider("PickUpCount".Translate(item.LabelShort), 1, to2, (Action<int>)delegate(int count)
							{
								item.SetForbidden(false, false);
								Job job6 = new Job(JobDefOf.TakeInventory, item);
								job6.count = count;
								pawn.jobs.TryTakeOrderedJob(job6, JobTag.Misc);
							}, -2147483648);
							Find.WindowStack.Add(window2);
						}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, item, "ReservedBy"));
					}
				}
			}
			if (!pawn.Map.IsPlayerHome)
			{
				Thing item2 = c.GetFirstItem(pawn.Map);
				if (item2 != null && item2.def.EverHaulable)
				{
					Pawn bestPackAnimal = GiveToPackAnimalUtility.PackAnimalWithTheMostFreeSpace(pawn.Map, pawn.Faction);
					if (bestPackAnimal != null)
					{
						if (!pawn.CanReach(item2, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
						{
							opts.Add(new FloatMenuOption("CannotGiveToPackAnimal".Translate(item2.Label) + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
						else if (MassUtility.WillBeOverEncumberedAfterPickingUp(bestPackAnimal, item2, 1))
						{
							opts.Add(new FloatMenuOption("CannotGiveToPackAnimal".Translate(item2.Label) + " (" + "TooHeavy".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
						else if (item2.stackCount == 1)
						{
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("GiveToPackAnimal".Translate(item2.Label), (Action)delegate()
							{
								item2.SetForbidden(false, false);
								Job job5 = new Job(JobDefOf.GiveToPackAnimal, item2);
								job5.count = 1;
								pawn.jobs.TryTakeOrderedJob(job5, JobTag.Misc);
							}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, item2, "ReservedBy"));
						}
						else
						{
							if (MassUtility.WillBeOverEncumberedAfterPickingUp(bestPackAnimal, item2, item2.stackCount))
							{
								opts.Add(new FloatMenuOption("CannotGiveToPackAnimalAll".Translate(item2.Label) + " (" + "TooHeavy".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
							}
							else
							{
								opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("GiveToPackAnimalAll".Translate(item2.Label), (Action)delegate()
								{
									item2.SetForbidden(false, false);
									Job job4 = new Job(JobDefOf.GiveToPackAnimal, item2);
									job4.count = item2.stackCount;
									pawn.jobs.TryTakeOrderedJob(job4, JobTag.Misc);
								}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, item2, "ReservedBy"));
							}
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("GiveToPackAnimalSome".Translate(item2.Label), (Action)delegate()
							{
								int to = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(bestPackAnimal, item2), item2.stackCount);
								Dialog_Slider window = new Dialog_Slider("GiveToPackAnimalCount".Translate(item2.LabelShort), 1, to, (Action<int>)delegate(int count)
								{
									item2.SetForbidden(false, false);
									Job job3 = new Job(JobDefOf.GiveToPackAnimal, item2);
									job3.count = count;
									pawn.jobs.TryTakeOrderedJob(job3, JobTag.Misc);
								}, -2147483648);
								Find.WindowStack.Add(window);
							}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, item2, "ReservedBy"));
						}
					}
				}
			}
			if (!pawn.Map.IsPlayerHome && pawn.Map.exitMapGrid.MapUsesExitGrid)
			{
				foreach (LocalTargetInfo item10 in GenUI.TargetsAt(clickPos, TargetingParameters.ForRescue(pawn), true))
				{
					Pawn p = (Pawn)item10.Thing;
					if (p.Faction == Faction.OfPlayer || p.HostFaction == Faction.OfPlayer)
					{
						IntVec3 exitSpot;
						if (!pawn.CanReach((Thing)p, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
						{
							opts.Add(new FloatMenuOption("CannotCarryToExit".Translate(p.Label) + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
						else if (!RCellFinder.TryFindBestExitSpot(pawn, out exitSpot, TraverseMode.ByPawn))
						{
							opts.Add(new FloatMenuOption("CannotCarryToExit".Translate(p.Label) + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
						else
						{
							opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("CarryToExit".Translate(p.Label), (Action)delegate()
							{
								Job job2 = new Job(JobDefOf.CarryDownedPawnToExit, (Thing)p, exitSpot);
								job2.count = 1;
								pawn.jobs.TryTakeOrderedJob(job2, JobTag.Misc);
							}, MenuOptionPriority.High, null, null, 0f, null, null), pawn, item10, "ReservedBy"));
						}
					}
				}
			}
			if (pawn.equipment != null && pawn.equipment.Primary != null && GenUI.TargetsAt(clickPos, TargetingParameters.ForSelf(pawn), true).Any())
			{
				Action action3 = (Action)delegate()
				{
					pawn.jobs.TryTakeOrderedJob(new Job(JobDefOf.DropEquipment, (Thing)pawn.equipment.Primary), JobTag.Misc);
				};
				opts.Add(new FloatMenuOption("Drop".Translate(pawn.equipment.Primary.Label), action3, MenuOptionPriority.Default, null, null, 0f, null, null));
			}
			foreach (LocalTargetInfo item11 in GenUI.TargetsAt(clickPos, TargetingParameters.ForTrade(), true))
			{
				LocalTargetInfo _ = item11;
				if (!pawn.CanReach(item11, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					opts.Add(new FloatMenuOption("CannotTrade".Translate() + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				else if (pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
				{
					opts.Add(new FloatMenuOption("CannotPrioritizeWorkTypeDisabled".Translate(SkillDefOf.Social.LabelCap), null, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				else
				{
					Pawn pTarg = (Pawn)item11.Thing;
					Action action4 = (Action)delegate()
					{
						Job job = new Job(JobDefOf.TradeWithPawn, (Thing)pTarg);
						job.playerForced = true;
						pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.InteractingWithTraders, KnowledgeAmount.Total);
					};
					string str = "";
					if (pTarg.Faction != null)
					{
						str = " (" + pTarg.Faction.Name + ")";
					}
					string label = "TradeWith".Translate(pTarg.LabelShort + ", " + pTarg.TraderKind.label) + str;
					Action action = action4;
					MenuOptionPriority priority2 = MenuOptionPriority.InitiateSocial;
					Thing thing = item11.Thing;
					opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action, priority2, null, thing, 0f, null, null), pawn, (Thing)pTarg, "ReservedBy"));
				}
			}
			foreach (Thing item12 in pawn.Map.thingGrid.ThingsAt(c))
			{
				foreach (FloatMenuOption floatMenuOption in item12.GetFloatMenuOptions(pawn))
				{
					opts.Add(floatMenuOption);
				}
			}
		}

		private static void AddUndraftedOrders(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
		{
			IntVec3 clickCell = IntVec3.FromVector3(clickPos);
			bool flag = false;
			bool flag2 = false;
			foreach (Thing item in pawn.Map.thingGrid.ThingsAt(clickCell))
			{
				flag2 = true;
				if (pawn.CanReach(item, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
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
				JobGiver_Work jobGiver_Work = pawn.thinker.TryGetMainTreeThinkNode<JobGiver_Work>();
				if (jobGiver_Work != null)
				{
					foreach (Thing item2 in pawn.Map.thingGrid.ThingsAt(clickCell))
					{
						foreach (WorkTypeDef item3 in DefDatabase<WorkTypeDef>.AllDefsListForReading)
						{
							for (int i = 0; i < item3.workGiversByPriority.Count; i++)
							{
								WorkGiver_Scanner workGiver_Scanner = item3.workGiversByPriority[i].Worker as WorkGiver_Scanner;
								string label;
								Action action;
								if (workGiver_Scanner != null && workGiver_Scanner.def.directOrderable && !workGiver_Scanner.ShouldSkip(pawn))
								{
									JobFailReason.Clear();
									if (!workGiver_Scanner.PotentialWorkThingRequest.Accepts(item2) && (workGiver_Scanner.PotentialWorkThingsGlobal(pawn) == null || !workGiver_Scanner.PotentialWorkThingsGlobal(pawn).Contains(item2)))
									{
										continue;
									}
									label = (string)null;
									action = null;
									PawnCapacityDef pawnCapacityDef = workGiver_Scanner.MissingRequiredCapacity(pawn);
									if (pawnCapacityDef != null)
									{
										label = "CannotMissingHealthActivities".Translate(pawnCapacityDef.label);
									}
									else
									{
										Job job = workGiver_Scanner.HasJobOnThing(pawn, item2, true) ? workGiver_Scanner.JobOnThing(pawn, item2, true) : null;
										if (job == null)
										{
											if (JobFailReason.HaveReason)
											{
												label = "CannotGenericWork".Translate(workGiver_Scanner.def.verb, item2.LabelShort) + " (" + JobFailReason.Reason + ")";
												goto IL_05af;
											}
											continue;
										}
										WorkTypeDef workType = workGiver_Scanner.def.workType;
										if (pawn.story != null && pawn.story.WorkTagIsDisabled(workGiver_Scanner.def.workTags))
										{
											label = "CannotPrioritizeWorkGiverDisabled".Translate(workGiver_Scanner.def.label);
										}
										else if (pawn.jobs.curJob != null && pawn.jobs.curJob.JobIsSameAs(job))
										{
											label = "CannotGenericAlreadyAm".Translate(workType.gerundLabel, item2.LabelShort);
										}
										else if (pawn.workSettings.GetPriority(workType) == 0)
										{
											if (pawn.story.WorkTypeIsDisabled(workType))
											{
												label = "CannotPrioritizeWorkTypeDisabled".Translate(workType.gerundLabel);
											}
											else if ("CannotPrioritizeNotAssignedToWorkType".CanTranslate())
											{
												label = "CannotPrioritizeNotAssignedToWorkType".Translate(workType.gerundLabel);
											}
											else
											{
												label = "CannotPrioritizeIsNotA".Translate(pawn.NameStringShort, workType.pawnLabel);
											}
										}
										else if (job.def == JobDefOf.Research && item2 is Building_ResearchBench)
										{
											label = "CannotPrioritizeResearch".Translate();
										}
										else if (item2.IsForbidden(pawn))
										{
											if (!item2.Position.InAllowedArea(pawn))
											{
												label = "CannotPrioritizeForbiddenOutsideAllowedArea".Translate(item2.Label);
											}
											else
											{
												label = "CannotPrioritizeForbidden".Translate(item2.Label);
											}
										}
										else if (!pawn.CanReach(item2, workGiver_Scanner.PathEndMode, Danger.Deadly, false, TraverseMode.ByPawn))
										{
											label = (item2.Label + ": " + "NoPath".Translate()).CapitalizeFirst();
										}
										else
										{
											label = "PrioritizeGeneric".Translate(workGiver_Scanner.def.gerund, item2.Label);
											Job localJob = job;
											WorkGiver_Scanner localScanner = workGiver_Scanner;
											action = (Action)delegate()
											{
												pawn.jobs.TryTakeOrderedJobPrioritizedWork(localJob, localScanner, clickCell);
											};
										}
									}
									goto IL_05af;
								}
								continue;
								IL_05af:
								if (!opts.Any((Predicate<FloatMenuOption>)((FloatMenuOption op) => op.Label == label.TrimEnd())))
								{
									opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action, MenuOptionPriority.Default, null, null, 0f, null, null), pawn, item2, "ReservedBy"));
								}
							}
						}
					}
					foreach (WorkTypeDef item4 in DefDatabase<WorkTypeDef>.AllDefsListForReading)
					{
						for (int j = 0; j < item4.workGiversByPriority.Count; j++)
						{
							WorkGiver_Scanner workGiver_Scanner2 = item4.workGiversByPriority[j].Worker as WorkGiver_Scanner;
							Action action2;
							string label2;
							if (workGiver_Scanner2 != null && workGiver_Scanner2.def.directOrderable && !workGiver_Scanner2.ShouldSkip(pawn))
							{
								JobFailReason.Clear();
								if (workGiver_Scanner2.PotentialWorkCellsGlobal(pawn).Contains(clickCell))
								{
									action2 = null;
									label2 = (string)null;
									PawnCapacityDef pawnCapacityDef2 = workGiver_Scanner2.MissingRequiredCapacity(pawn);
									if (pawnCapacityDef2 != null)
									{
										label2 = "CannotMissingHealthActivities".Translate(pawnCapacityDef2.label);
									}
									else
									{
										Job job2 = workGiver_Scanner2.HasJobOnCell(pawn, clickCell) ? workGiver_Scanner2.JobOnCell(pawn, clickCell) : null;
										if (job2 == null)
										{
											if (JobFailReason.HaveReason)
											{
												label2 = "CannotGenericWork".Translate(workGiver_Scanner2.def.verb, "AreaLower".Translate()) + " (" + JobFailReason.Reason + ")";
												goto IL_09cb;
											}
											continue;
										}
										WorkTypeDef workType2 = workGiver_Scanner2.def.workType;
										if (pawn.jobs.curJob != null && pawn.jobs.curJob.JobIsSameAs(job2))
										{
											label2 = "CannotGenericAlreadyAm".Translate(workType2.gerundLabel, "AreaLower".Translate());
										}
										else if (pawn.workSettings.GetPriority(workType2) == 0)
										{
											if (pawn.story.WorkTypeIsDisabled(workType2))
											{
												label2 = "CannotPrioritizeWorkTypeDisabled".Translate(workType2.gerundLabel);
											}
											else if ("CannotPrioritizeNotAssignedToWorkType".CanTranslate())
											{
												label2 = "CannotPrioritizeNotAssignedToWorkType".Translate(workType2.gerundLabel);
											}
											else
											{
												label2 = "CannotPrioritizeIsNotA".Translate(pawn.NameStringShort, workType2.pawnLabel);
											}
										}
										else if (!pawn.CanReach(clickCell, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
										{
											label2 = "AreaLower".Translate().CapitalizeFirst() + ": " + "NoPath".Translate();
										}
										else
										{
											label2 = "PrioritizeGeneric".Translate(workGiver_Scanner2.def.gerund, "AreaLower".Translate());
											Job localJob2 = job2;
											WorkGiver_Scanner localScanner2 = workGiver_Scanner2;
											action2 = (Action)delegate()
											{
												pawn.jobs.TryTakeOrderedJobPrioritizedWork(localJob2, localScanner2, clickCell);
											};
										}
									}
									goto IL_09cb;
								}
							}
							continue;
							IL_09cb:
							if (!opts.Any((Predicate<FloatMenuOption>)((FloatMenuOption op) => op.Label == label2.TrimEnd())))
							{
								opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label2, action2, MenuOptionPriority.Default, null, null, 0f, null, null), pawn, clickCell, "ReservedBy"));
							}
						}
					}
				}
			}
		}

		private static FloatMenuOption GotoLocationOption(IntVec3 clickCell, Pawn pawn)
		{
			int num = GenRadial.NumCellsInRadius(2.9f);
			int num2 = 0;
			IntVec3 curLoc;
			FloatMenuOption result;
			while (true)
			{
				if (num2 < num)
				{
					curLoc = GenRadial.RadialPattern[num2] + clickCell;
					if (curLoc.Standable(pawn.Map))
					{
						if (curLoc != pawn.Position)
						{
							if (!pawn.CanReach(curLoc, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
							{
								result = new FloatMenuOption("CannotGoNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
							}
							else
							{
								Action action = (Action)delegate()
								{
									IntVec3 intVec = RCellFinder.BestOrderedGotoDestNear(curLoc, pawn);
									Job job = new Job(JobDefOf.Goto, intVec);
									if (pawn.Map.exitMapGrid.IsExitCell(UI.MouseCell()))
									{
										job.exitMapOnArrival = true;
									}
									if (pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc))
									{
										MoteMaker.MakeStaticMote(intVec, pawn.Map, ThingDefOf.Mote_FeedbackGoto, 1f);
									}
								};
								FloatMenuOption floatMenuOption = new FloatMenuOption("GoHere".Translate(), action, MenuOptionPriority.GoHere, null, null, 0f, null, null);
								floatMenuOption.autoTakeable = true;
								result = floatMenuOption;
							}
						}
						else
						{
							result = null;
						}
						break;
					}
					num2++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}
	}
}
