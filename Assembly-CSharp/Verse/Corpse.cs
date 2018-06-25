using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DC9 RID: 3529
	public class Corpse : ThingWithComps, IThingHolder, IThoughtGiver, IStrippable, IBillGiver
	{
		// Token: 0x04003475 RID: 13429
		private ThingOwner<Pawn> innerContainer;

		// Token: 0x04003476 RID: 13430
		public int timeOfDeath = -1;

		// Token: 0x04003477 RID: 13431
		private int vanishAfterTimestamp = -1;

		// Token: 0x04003478 RID: 13432
		private BillStack operationsBillStack = null;

		// Token: 0x04003479 RID: 13433
		public bool everBuriedInSarcophagus;

		// Token: 0x0400347A RID: 13434
		private const int VanishAfterTicksSinceDessicated = 6000000;

		// Token: 0x06004ED7 RID: 20183 RVA: 0x0029253E File Offset: 0x0029093E
		public Corpse()
		{
			this.operationsBillStack = new BillStack(this);
			this.innerContainer = new ThingOwner<Pawn>(this, true, LookMode.Reference);
		}

		// Token: 0x17000CBC RID: 3260
		// (get) Token: 0x06004ED8 RID: 20184 RVA: 0x00292578 File Offset: 0x00290978
		// (set) Token: 0x06004ED9 RID: 20185 RVA: 0x002925B4 File Offset: 0x002909B4
		public Pawn InnerPawn
		{
			get
			{
				Pawn result;
				if (this.innerContainer.Count > 0)
				{
					result = this.innerContainer[0];
				}
				else
				{
					result = null;
				}
				return result;
			}
			set
			{
				if (value == null)
				{
					this.innerContainer.Clear();
				}
				else
				{
					if (this.innerContainer.Count > 0)
					{
						Log.Error("Setting InnerPawn in corpse that already has one.", false);
						this.innerContainer.Clear();
					}
					this.innerContainer.TryAdd(value, true);
				}
			}
		}

		// Token: 0x17000CBD RID: 3261
		// (get) Token: 0x06004EDA RID: 20186 RVA: 0x00292614 File Offset: 0x00290A14
		// (set) Token: 0x06004EDB RID: 20187 RVA: 0x0029263A File Offset: 0x00290A3A
		public int Age
		{
			get
			{
				return Find.TickManager.TicksGame - this.timeOfDeath;
			}
			set
			{
				this.timeOfDeath = Find.TickManager.TicksGame - value;
			}
		}

		// Token: 0x17000CBE RID: 3262
		// (get) Token: 0x06004EDC RID: 20188 RVA: 0x00292650 File Offset: 0x00290A50
		public override string Label
		{
			get
			{
				string result;
				if (this.Bugged)
				{
					Log.ErrorOnce("Corpse.Label while Bugged", 57361644, false);
					result = "";
				}
				else
				{
					result = "DeadLabel".Translate(new object[]
					{
						this.InnerPawn.Label
					});
				}
				return result;
			}
		}

		// Token: 0x17000CBF RID: 3263
		// (get) Token: 0x06004EDD RID: 20189 RVA: 0x002926AC File Offset: 0x00290AAC
		public override bool IngestibleNow
		{
			get
			{
				bool result;
				if (this.Bugged)
				{
					Log.Error("IngestibleNow on Corpse while Bugged.", false);
					result = false;
				}
				else
				{
					result = (base.IngestibleNow && this.InnerPawn.RaceProps.IsFlesh && this.GetRotStage() == RotStage.Fresh);
				}
				return result;
			}
		}

		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x06004EDE RID: 20190 RVA: 0x00292720 File Offset: 0x00290B20
		public RotDrawMode CurRotDrawMode
		{
			get
			{
				CompRottable comp = base.GetComp<CompRottable>();
				if (comp != null)
				{
					if (comp.Stage == RotStage.Rotting)
					{
						return RotDrawMode.Rotting;
					}
					if (comp.Stage == RotStage.Dessicated)
					{
						return RotDrawMode.Dessicated;
					}
				}
				return RotDrawMode.Fresh;
			}
		}

		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x06004EDF RID: 20191 RVA: 0x0029276C File Offset: 0x00290B6C
		private bool ShouldVanish
		{
			get
			{
				return this.InnerPawn.RaceProps.Animal && this.vanishAfterTimestamp > 0 && this.Age >= this.vanishAfterTimestamp && base.Spawned && this.GetRoom(RegionType.Set_Passable) != null && this.GetRoom(RegionType.Set_Passable).TouchesMapEdge && !base.Map.roofGrid.Roofed(base.Position);
			}
		}

		// Token: 0x17000CC2 RID: 3266
		// (get) Token: 0x06004EE0 RID: 20192 RVA: 0x002927F8 File Offset: 0x00290BF8
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats
		{
			get
			{
				foreach (StatDrawEntry s in this.<get_SpecialDisplayStats>__BaseCallProxy0())
				{
					yield return s;
				}
				if (this.GetRotStage() == RotStage.Fresh)
				{
					StatDef meatAmount = StatDefOf.MeatAmount;
					yield return new StatDrawEntry(meatAmount.category, meatAmount, this.InnerPawn.GetStatValue(meatAmount, true), StatRequest.For(this.InnerPawn), ToStringNumberSense.Undefined);
					StatDef leatherAmount = StatDefOf.LeatherAmount;
					yield return new StatDrawEntry(leatherAmount.category, leatherAmount, this.InnerPawn.GetStatValue(leatherAmount, true), StatRequest.For(this.InnerPawn), ToStringNumberSense.Undefined);
				}
				yield break;
			}
		}

		// Token: 0x17000CC3 RID: 3267
		// (get) Token: 0x06004EE1 RID: 20193 RVA: 0x00292824 File Offset: 0x00290C24
		public BillStack BillStack
		{
			get
			{
				return this.operationsBillStack;
			}
		}

		// Token: 0x17000CC4 RID: 3268
		// (get) Token: 0x06004EE2 RID: 20194 RVA: 0x00292840 File Offset: 0x00290C40
		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				yield return this.InteractionCell;
				yield break;
			}
		}

		// Token: 0x17000CC5 RID: 3269
		// (get) Token: 0x06004EE3 RID: 20195 RVA: 0x0029286C File Offset: 0x00290C6C
		public bool Bugged
		{
			get
			{
				return this.innerContainer.Count == 0 || this.innerContainer[0] == null || this.innerContainer[0].def == null || this.innerContainer[0].kindDef == null;
			}
		}

		// Token: 0x06004EE4 RID: 20196 RVA: 0x002928D0 File Offset: 0x00290CD0
		public bool CurrentlyUsableForBills()
		{
			return this.InteractionCell.IsValid;
		}

		// Token: 0x06004EE5 RID: 20197 RVA: 0x002928F4 File Offset: 0x00290CF4
		public bool UsableForBillsAfterFueling()
		{
			return this.CurrentlyUsableForBills();
		}

		// Token: 0x06004EE6 RID: 20198 RVA: 0x00292910 File Offset: 0x00290D10
		public bool AnythingToStrip()
		{
			return this.InnerPawn.AnythingToStrip();
		}

		// Token: 0x06004EE7 RID: 20199 RVA: 0x00292930 File Offset: 0x00290D30
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06004EE8 RID: 20200 RVA: 0x0029294B File Offset: 0x00290D4B
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06004EE9 RID: 20201 RVA: 0x0029295A File Offset: 0x00290D5A
		public override void PostMake()
		{
			base.PostMake();
			this.timeOfDeath = Find.TickManager.TicksGame;
		}

		// Token: 0x06004EEA RID: 20202 RVA: 0x00292974 File Offset: 0x00290D74
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.Bugged)
			{
				Log.Error(this + " spawned in bugged state.", false);
			}
			else
			{
				base.SpawnSetup(map, respawningAfterLoad);
				this.InnerPawn.Rotation = Rot4.South;
				this.NotifyColonistBar();
			}
		}

		// Token: 0x06004EEB RID: 20203 RVA: 0x002929C2 File Offset: 0x00290DC2
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			if (!this.Bugged)
			{
				this.NotifyColonistBar();
			}
		}

		// Token: 0x06004EEC RID: 20204 RVA: 0x002929E0 File Offset: 0x00290DE0
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			Pawn pawn = null;
			if (!this.Bugged)
			{
				pawn = this.InnerPawn;
				this.NotifyColonistBar();
				this.innerContainer.Clear();
			}
			base.Destroy(mode);
			if (pawn != null)
			{
				Corpse.PostCorpseDestroy(pawn);
			}
		}

		// Token: 0x06004EED RID: 20205 RVA: 0x00292A28 File Offset: 0x00290E28
		public static void PostCorpseDestroy(Pawn pawn)
		{
			if (pawn.ownership != null)
			{
				pawn.ownership.UnclaimAll();
			}
			if (pawn.equipment != null)
			{
				pawn.equipment.DestroyAllEquipment(DestroyMode.Vanish);
			}
			pawn.inventory.DestroyAll(DestroyMode.Vanish);
			if (pawn.apparel != null)
			{
				pawn.apparel.DestroyAll(DestroyMode.Vanish);
			}
		}

		// Token: 0x06004EEE RID: 20206 RVA: 0x00292A88 File Offset: 0x00290E88
		public override void TickRare()
		{
			base.TickRare();
			if (!base.Destroyed)
			{
				if (this.Bugged)
				{
					Log.Error(this + " has null innerPawn. Destroying.", false);
					this.Destroy(DestroyMode.Vanish);
				}
				else
				{
					this.InnerPawn.TickRare();
					if (this.vanishAfterTimestamp < 0 || this.GetRotStage() != RotStage.Dessicated)
					{
						this.vanishAfterTimestamp = this.Age + 6000000;
					}
					if (this.ShouldVanish)
					{
						this.Destroy(DestroyMode.Vanish);
					}
				}
			}
		}

		// Token: 0x06004EEF RID: 20207 RVA: 0x00292B1C File Offset: 0x00290F1C
		protected override void IngestedCalculateAmounts(Pawn ingester, float nutritionWanted, out int numTaken, out float nutritionIngested)
		{
			BodyPartRecord bodyPartRecord = this.GetBestBodyPartToEat(ingester, nutritionWanted);
			if (bodyPartRecord == null)
			{
				Log.Error(string.Concat(new object[]
				{
					ingester,
					" ate ",
					this,
					" but no body part was found. Replacing with core part."
				}), false);
				bodyPartRecord = this.InnerPawn.RaceProps.body.corePart;
			}
			float bodyPartNutrition = FoodUtility.GetBodyPartNutrition(this, bodyPartRecord);
			if (bodyPartRecord == this.InnerPawn.RaceProps.body.corePart)
			{
				if (PawnUtility.ShouldSendNotificationAbout(this.InnerPawn) && this.InnerPawn.RaceProps.Humanlike)
				{
					Messages.Message("MessageEatenByPredator".Translate(new object[]
					{
						this.InnerPawn.LabelShort,
						ingester.LabelIndefinite()
					}).CapitalizeFirst(), ingester, MessageTypeDefOf.NegativeEvent, true);
				}
				numTaken = 1;
			}
			else
			{
				Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this.InnerPawn, bodyPartRecord);
				hediff_MissingPart.lastInjury = HediffDefOf.Bite;
				hediff_MissingPart.IsFresh = true;
				this.InnerPawn.health.AddHediff(hediff_MissingPart, null, null, null);
				numTaken = 0;
			}
			nutritionIngested = bodyPartNutrition;
		}

		// Token: 0x06004EF0 RID: 20208 RVA: 0x00292C58 File Offset: 0x00291058
		public override IEnumerable<Thing> ButcherProducts(Pawn butcher, float efficiency)
		{
			foreach (Thing t in this.InnerPawn.ButcherProducts(butcher, efficiency))
			{
				yield return t;
			}
			if (this.InnerPawn.RaceProps.BloodDef != null)
			{
				FilthMaker.MakeFilth(butcher.Position, butcher.Map, this.InnerPawn.RaceProps.BloodDef, this.InnerPawn.LabelIndefinite(), 1);
			}
			if (this.InnerPawn.RaceProps.Humanlike)
			{
				butcher.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.ButcheredHumanlikeCorpse, null);
				foreach (Pawn pawn in butcher.Map.mapPawns.SpawnedPawnsInFaction(butcher.Faction))
				{
					if (pawn != butcher && pawn.needs != null && pawn.needs.mood != null && pawn.needs.mood.thoughts != null)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowButcheredHumanlikeCorpse, null);
					}
				}
				TaleRecorder.RecordTale(TaleDefOf.ButcheredHumanlikeCorpse, new object[]
				{
					butcher
				});
			}
			yield break;
		}

		// Token: 0x06004EF1 RID: 20209 RVA: 0x00292C90 File Offset: 0x00291090
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.timeOfDeath, "timeOfDeath", 0, false);
			Scribe_Values.Look<int>(ref this.vanishAfterTimestamp, "vanishAfterTimestamp", 0, false);
			Scribe_Values.Look<bool>(ref this.everBuriedInSarcophagus, "everBuriedInSarcophagus", false, false);
			Scribe_Deep.Look<BillStack>(ref this.operationsBillStack, "operationsBillStack", new object[]
			{
				this
			});
			Scribe_Deep.Look<ThingOwner<Pawn>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
		}

		// Token: 0x06004EF2 RID: 20210 RVA: 0x00292D0E File Offset: 0x0029110E
		public void Strip()
		{
			this.InnerPawn.Strip();
		}

		// Token: 0x06004EF3 RID: 20211 RVA: 0x00292D1C File Offset: 0x0029111C
		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.InnerPawn.Drawer.renderer.RenderPawnAt(drawLoc);
		}

		// Token: 0x06004EF4 RID: 20212 RVA: 0x00292D38 File Offset: 0x00291138
		public Thought_Memory GiveObservedThought()
		{
			Thought_Memory result;
			if (!this.InnerPawn.RaceProps.Humanlike)
			{
				result = null;
			}
			else if (this.StoringThing() == null)
			{
				Thought_MemoryObservation thought_MemoryObservation;
				if (this.IsNotFresh())
				{
					thought_MemoryObservation = (Thought_MemoryObservation)ThoughtMaker.MakeThought(ThoughtDefOf.ObservedLayingRottingCorpse);
				}
				else
				{
					thought_MemoryObservation = (Thought_MemoryObservation)ThoughtMaker.MakeThought(ThoughtDefOf.ObservedLayingCorpse);
				}
				thought_MemoryObservation.Target = this;
				result = thought_MemoryObservation;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06004EF5 RID: 20213 RVA: 0x00292DB8 File Offset: 0x002911B8
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.InnerPawn.Faction != null)
			{
				stringBuilder.AppendLine("Faction".Translate() + ": " + this.InnerPawn.Faction.Name);
			}
			stringBuilder.AppendLine("DeadTime".Translate(new object[]
			{
				this.Age.ToStringTicksToPeriodVague(true, false)
			}));
			float num = 1f - this.InnerPawn.health.hediffSet.GetCoverageOfNotMissingNaturalParts(this.InnerPawn.RaceProps.body.corePart);
			if (num != 0f)
			{
				stringBuilder.AppendLine("CorpsePercentMissing".Translate() + ": " + num.ToStringPercent());
			}
			stringBuilder.AppendLine(base.GetInspectString());
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06004EF6 RID: 20214 RVA: 0x00292EAC File Offset: 0x002912AC
		public void RotStageChanged()
		{
			PortraitsCache.SetDirty(this.InnerPawn);
			this.NotifyColonistBar();
		}

		// Token: 0x06004EF7 RID: 20215 RVA: 0x00292EC0 File Offset: 0x002912C0
		private BodyPartRecord GetBestBodyPartToEat(Pawn ingester, float nutritionWanted)
		{
			IEnumerable<BodyPartRecord> source = from x in this.InnerPawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null)
			where x.depth == BodyPartDepth.Outside && FoodUtility.GetBodyPartNutrition(this, x) > 0.001f
			select x;
			BodyPartRecord result;
			if (!source.Any<BodyPartRecord>())
			{
				result = null;
			}
			else
			{
				result = source.MinBy((BodyPartRecord x) => Mathf.Abs(FoodUtility.GetBodyPartNutrition(this, x) - nutritionWanted));
			}
			return result;
		}

		// Token: 0x06004EF8 RID: 20216 RVA: 0x00292F37 File Offset: 0x00291337
		private void NotifyColonistBar()
		{
			if (this.InnerPawn.Faction == Faction.OfPlayer && Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}
	}
}
