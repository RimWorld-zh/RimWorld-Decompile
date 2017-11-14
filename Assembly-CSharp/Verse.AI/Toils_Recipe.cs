using RimWorld;
using System.Collections.Generic;
using System.Linq;

namespace Verse.AI
{
	public static class Toils_Recipe
	{
		private const int LongCraftingProjectThreshold = 20000;

		public static Toil MakeUnfinishedThingIfNeeded()
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				if (curJob.RecipeDef.UsesUnfinishedThing && !(curJob.GetTarget(TargetIndex.B).Thing is UnfinishedThing))
				{
					List<Thing> list = Toils_Recipe.CalculateIngredients(curJob, actor);
					Thing thing = Toils_Recipe.CalculateDominantIngredient(curJob, list);
					for (int i = 0; i < list.Count; i++)
					{
						Thing thing2 = list[i];
						actor.Map.designationManager.RemoveAllDesignationsOn(thing2, false);
						if (thing2.Spawned)
						{
							thing2.DeSpawn();
						}
					}
					ThingDef stuff = (!curJob.RecipeDef.unfinishedThingDef.MadeFromStuff) ? null : thing.def;
					UnfinishedThing unfinishedThing = (UnfinishedThing)ThingMaker.MakeThing(curJob.RecipeDef.unfinishedThingDef, stuff);
					unfinishedThing.Creator = actor;
					unfinishedThing.BoundBill = (Bill_ProductionWithUft)curJob.bill;
					unfinishedThing.ingredients = list;
					CompColorable compColorable = unfinishedThing.TryGetComp<CompColorable>();
					if (compColorable != null)
					{
						compColorable.Color = thing.DrawColor;
					}
					GenSpawn.Spawn(unfinishedThing, curJob.GetTarget(TargetIndex.A).Cell, actor.Map);
					curJob.SetTarget(TargetIndex.B, unfinishedThing);
					actor.Reserve(unfinishedThing, curJob, 1, -1, null);
				}
			};
			return toil;
		}

		public static Toil DoRecipeWork()
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor3 = toil.actor;
				Job curJob3 = actor3.jobs.curJob;
				JobDriver_DoBill jobDriver_DoBill2 = (JobDriver_DoBill)actor3.jobs.curDriver;
				UnfinishedThing unfinishedThing3 = curJob3.GetTarget(TargetIndex.B).Thing as UnfinishedThing;
				if (unfinishedThing3 != null && unfinishedThing3.Initialized)
				{
					jobDriver_DoBill2.workLeft = unfinishedThing3.workLeft;
				}
				else
				{
					jobDriver_DoBill2.workLeft = curJob3.bill.recipe.WorkAmountTotal((unfinishedThing3 == null) ? null : unfinishedThing3.Stuff);
					if (unfinishedThing3 != null)
					{
						unfinishedThing3.workLeft = jobDriver_DoBill2.workLeft;
					}
				}
				jobDriver_DoBill2.billStartTick = Find.TickManager.TicksGame;
				jobDriver_DoBill2.ticksSpentDoingRecipeWork = 0;
				curJob3.bill.Notify_DoBillStarted(actor3);
			};
			toil.tickAction = delegate
			{
				Pawn actor2 = toil.actor;
				Job curJob2 = actor2.jobs.curJob;
				JobDriver_DoBill jobDriver_DoBill = (JobDriver_DoBill)actor2.jobs.curDriver;
				UnfinishedThing unfinishedThing2 = curJob2.GetTarget(TargetIndex.B).Thing as UnfinishedThing;
				if (unfinishedThing2 != null && unfinishedThing2.Destroyed)
				{
					actor2.jobs.EndCurrentJob(JobCondition.Incompletable, true);
				}
				else
				{
					jobDriver_DoBill.ticksSpentDoingRecipeWork++;
					curJob2.bill.Notify_PawnDidWork(actor2);
					IBillGiverWithTickAction billGiverWithTickAction = toil.actor.CurJob.GetTarget(TargetIndex.A).Thing as IBillGiverWithTickAction;
					if (billGiverWithTickAction != null)
					{
						billGiverWithTickAction.UsedThisTick();
					}
					if (curJob2.RecipeDef.workSkill != null && curJob2.RecipeDef.UsesUnfinishedThing)
					{
						actor2.skills.GetSkill(curJob2.RecipeDef.workSkill).Learn((float)(0.10999999940395355 * curJob2.RecipeDef.workSkillLearnFactor), false);
					}
					float num = (float)((curJob2.RecipeDef.workSpeedStat != null) ? actor2.GetStatValue(curJob2.RecipeDef.workSpeedStat, true) : 1.0);
					Building_WorkTable building_WorkTable = jobDriver_DoBill.BillGiver as Building_WorkTable;
					if (building_WorkTable != null)
					{
						num *= building_WorkTable.GetStatValue(StatDefOf.WorkTableWorkSpeedFactor, true);
					}
					if (DebugSettings.fastCrafting)
					{
						num = (float)(num * 30.0);
					}
					jobDriver_DoBill.workLeft -= num;
					if (unfinishedThing2 != null)
					{
						unfinishedThing2.workLeft = jobDriver_DoBill.workLeft;
					}
					actor2.GainComfortFromCellIfPossible();
					if (jobDriver_DoBill.workLeft <= 0.0)
					{
						jobDriver_DoBill.ReadyForNextToil();
					}
					if (curJob2.bill.recipe.UsesUnfinishedThing)
					{
						int num2 = Find.TickManager.TicksGame - jobDriver_DoBill.billStartTick;
						if (num2 >= 3000 && num2 % 1000 == 0)
						{
							actor2.jobs.CheckForJobOverride();
						}
					}
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.WithEffect(() => toil.actor.CurJob.bill.recipe.effectWorking, TargetIndex.A);
			toil.PlaySustainerOrSound(() => toil.actor.CurJob.bill.recipe.soundWorking);
			toil.WithProgressBar(TargetIndex.A, delegate
			{
				Pawn actor = toil.actor;
				Job curJob = actor.CurJob;
				UnfinishedThing unfinishedThing = curJob.GetTarget(TargetIndex.B).Thing as UnfinishedThing;
				return (float)(1.0 - ((JobDriver_DoBill)actor.jobs.curDriver).workLeft / curJob.bill.recipe.WorkAmountTotal((unfinishedThing == null) ? null : unfinishedThing.Stuff));
			}, false, -0.5f);
			toil.FailOn(() => toil.actor.CurJob.bill.suspended);
			return toil;
		}

		public static Toil FinishRecipeAndStartStoringProduct()
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				JobDriver_DoBill jobDriver_DoBill = (JobDriver_DoBill)actor.jobs.curDriver;
				if (curJob.RecipeDef.workSkill != null && !curJob.RecipeDef.UsesUnfinishedThing)
				{
					float xp = (float)((float)jobDriver_DoBill.ticksSpentDoingRecipeWork * 0.10999999940395355 * curJob.RecipeDef.workSkillLearnFactor);
					actor.skills.GetSkill(curJob.RecipeDef.workSkill).Learn(xp, false);
				}
				List<Thing> ingredients = Toils_Recipe.CalculateIngredients(curJob, actor);
				Thing dominantIngredient = Toils_Recipe.CalculateDominantIngredient(curJob, ingredients);
				List<Thing> list = GenRecipe.MakeRecipeProducts(curJob.RecipeDef, actor, ingredients, dominantIngredient).ToList();
				Toils_Recipe.ConsumeIngredients(ingredients, curJob.RecipeDef, actor.Map);
				curJob.bill.Notify_IterationCompleted(actor, ingredients);
				RecordsUtility.Notify_BillDone(actor, list);
				UnfinishedThing unfinishedThing = curJob.GetTarget(TargetIndex.B).Thing as UnfinishedThing;
				if (curJob.bill.recipe.WorkAmountTotal((unfinishedThing == null) ? null : unfinishedThing.Stuff) >= 20000.0 && list.Count > 0)
				{
					TaleRecorder.RecordTale(TaleDefOf.CompletedLongCraftingProject, actor, list[0].def);
				}
				if (list.Count == 0)
				{
					actor.jobs.EndCurrentJob(JobCondition.Succeeded, true);
				}
				else if (curJob.bill.GetStoreMode() == BillStoreModeDefOf.DropOnFloor)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (!GenPlace.TryPlaceThing(list[i], actor.Position, actor.Map, ThingPlaceMode.Near, null))
						{
							Log.Error(actor + " could not drop recipe product " + list[i] + " near " + actor.Position);
						}
					}
					actor.jobs.EndCurrentJob(JobCondition.Succeeded, true);
				}
				else
				{
					if (list.Count > 1)
					{
						for (int j = 1; j < list.Count; j++)
						{
							if (!GenPlace.TryPlaceThing(list[j], actor.Position, actor.Map, ThingPlaceMode.Near, null))
							{
								Log.Error(actor + " could not drop recipe product " + list[j] + " near " + actor.Position);
							}
						}
					}
					list[0].SetPositionDirect(actor.Position);
					IntVec3 c = default(IntVec3);
					if (StoreUtility.TryFindBestBetterStoreCellFor(list[0], actor, actor.Map, StoragePriority.Unstored, actor.Faction, out c, true))
					{
						actor.carryTracker.TryStartCarry(list[0]);
						curJob.targetB = c;
						curJob.targetA = list[0];
						curJob.count = 99999;
					}
					else
					{
						if (!GenPlace.TryPlaceThing(list[0], actor.Position, actor.Map, ThingPlaceMode.Near, null))
						{
							Log.Error("Bill doer could not drop product " + list[0] + " near " + actor.Position);
						}
						actor.jobs.EndCurrentJob(JobCondition.Succeeded, true);
					}
				}
			};
			return toil;
		}

		private static List<Thing> CalculateIngredients(Job job, Pawn actor)
		{
			UnfinishedThing unfinishedThing = job.GetTarget(TargetIndex.B).Thing as UnfinishedThing;
			if (unfinishedThing != null)
			{
				List<Thing> ingredients = unfinishedThing.ingredients;
				job.RecipeDef.Worker.ConsumeIngredient(unfinishedThing, job.RecipeDef, actor.Map);
				job.placedThings = null;
				return ingredients;
			}
			List<Thing> list = new List<Thing>();
			if (job.placedThings != null)
			{
				for (int i = 0; i < job.placedThings.Count; i++)
				{
					if (job.placedThings[i].Count <= 0)
					{
						Log.Error("PlacedThing " + job.placedThings[i] + " with count " + job.placedThings[i].Count + " for job " + job);
					}
					else
					{
						Thing thing = (job.placedThings[i].Count >= job.placedThings[i].thing.stackCount) ? job.placedThings[i].thing : job.placedThings[i].thing.SplitOff(job.placedThings[i].Count);
						job.placedThings[i].Count = 0;
						if (list.Contains(thing))
						{
							Log.Error("Tried to add ingredient from job placed targets twice: " + thing);
						}
						else
						{
							list.Add(thing);
							IStrippable strippable = thing as IStrippable;
							if (strippable != null)
							{
								strippable.Strip();
							}
						}
					}
				}
			}
			job.placedThings = null;
			return list;
		}

		private static Thing CalculateDominantIngredient(Job job, List<Thing> ingredients)
		{
			UnfinishedThing uft = job.GetTarget(TargetIndex.B).Thing as UnfinishedThing;
			if (uft != null && uft.def.MadeFromStuff)
			{
				return uft.ingredients.First((Thing ing) => ing.def == uft.Stuff);
			}
			if (!ingredients.NullOrEmpty())
			{
				if (job.RecipeDef.productHasIngredientStuff)
				{
					return ingredients[0];
				}
				if (job.RecipeDef.products.Any((ThingCountClass x) => x.thingDef.MadeFromStuff))
				{
					return (from x in ingredients
					where x.def.IsStuff
					select x).RandomElementByWeight((Thing x) => (float)x.stackCount);
				}
				return ingredients.RandomElementByWeight((Thing x) => (float)x.stackCount);
			}
			return null;
		}

		private static void ConsumeIngredients(List<Thing> ingredients, RecipeDef recipe, Map map)
		{
			for (int i = 0; i < ingredients.Count; i++)
			{
				recipe.Worker.ConsumeIngredient(ingredients[i], recipe, map);
			}
		}
	}
}
