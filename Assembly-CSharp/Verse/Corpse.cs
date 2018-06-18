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
		// Token: 0x06004EBE RID: 20158 RVA: 0x00290B82 File Offset: 0x0028EF82
		public Corpse()
		{
			this.operationsBillStack = new BillStack(this);
			this.innerContainer = new ThingOwner<Pawn>(this, true, LookMode.Reference);
		}

		// Token: 0x17000CBB RID: 3259
		// (get) Token: 0x06004EBF RID: 20159 RVA: 0x00290BBC File Offset: 0x0028EFBC
		// (set) Token: 0x06004EC0 RID: 20160 RVA: 0x00290BF8 File Offset: 0x0028EFF8
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

		// Token: 0x17000CBC RID: 3260
		// (get) Token: 0x06004EC1 RID: 20161 RVA: 0x00290C58 File Offset: 0x0028F058
		// (set) Token: 0x06004EC2 RID: 20162 RVA: 0x00290C7E File Offset: 0x0028F07E
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

		// Token: 0x17000CBD RID: 3261
		// (get) Token: 0x06004EC3 RID: 20163 RVA: 0x00290C94 File Offset: 0x0028F094
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

		// Token: 0x17000CBE RID: 3262
		// (get) Token: 0x06004EC4 RID: 20164 RVA: 0x00290CF0 File Offset: 0x0028F0F0
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

		// Token: 0x17000CBF RID: 3263
		// (get) Token: 0x06004EC5 RID: 20165 RVA: 0x00290D64 File Offset: 0x0028F164
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

		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x06004EC6 RID: 20166 RVA: 0x00290DB0 File Offset: 0x0028F1B0
		private bool ShouldVanish
		{
			get
			{
				return this.InnerPawn.RaceProps.Animal && this.vanishAfterTimestamp > 0 && this.Age >= this.vanishAfterTimestamp && base.Spawned && this.GetRoom(RegionType.Set_Passable) != null && this.GetRoom(RegionType.Set_Passable).TouchesMapEdge && !base.Map.roofGrid.Roofed(base.Position);
			}
		}

		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x06004EC7 RID: 20167 RVA: 0x00290E3C File Offset: 0x0028F23C
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

		// Token: 0x17000CC2 RID: 3266
		// (get) Token: 0x06004EC8 RID: 20168 RVA: 0x00290E68 File Offset: 0x0028F268
		public BillStack BillStack
		{
			get
			{
				return this.operationsBillStack;
			}
		}

		// Token: 0x17000CC3 RID: 3267
		// (get) Token: 0x06004EC9 RID: 20169 RVA: 0x00290E84 File Offset: 0x0028F284
		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				yield return this.InteractionCell;
				yield break;
			}
		}

		// Token: 0x17000CC4 RID: 3268
		// (get) Token: 0x06004ECA RID: 20170 RVA: 0x00290EB0 File Offset: 0x0028F2B0
		public bool Bugged
		{
			get
			{
				return this.innerContainer.Count == 0 || this.innerContainer[0] == null;
			}
		}

		// Token: 0x06004ECB RID: 20171 RVA: 0x00290EE8 File Offset: 0x0028F2E8
		public bool CurrentlyUsableForBills()
		{
			return this.InteractionCell.IsValid;
		}

		// Token: 0x06004ECC RID: 20172 RVA: 0x00290F0C File Offset: 0x0028F30C
		public bool UsableForBillsAfterFueling()
		{
			return this.CurrentlyUsableForBills();
		}

		// Token: 0x06004ECD RID: 20173 RVA: 0x00290F28 File Offset: 0x0028F328
		public bool AnythingToStrip()
		{
			return this.InnerPawn.AnythingToStrip();
		}

		// Token: 0x06004ECE RID: 20174 RVA: 0x00290F48 File Offset: 0x0028F348
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06004ECF RID: 20175 RVA: 0x00290F63 File Offset: 0x0028F363
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06004ED0 RID: 20176 RVA: 0x00290F72 File Offset: 0x0028F372
		public override void PostMake()
		{
			base.PostMake();
			this.timeOfDeath = Find.TickManager.TicksGame;
		}

		// Token: 0x06004ED1 RID: 20177 RVA: 0x00290F8C File Offset: 0x0028F38C
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

		// Token: 0x06004ED2 RID: 20178 RVA: 0x00290FDA File Offset: 0x0028F3DA
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			if (!this.Bugged)
			{
				this.NotifyColonistBar();
			}
		}

		// Token: 0x06004ED3 RID: 20179 RVA: 0x00290FF8 File Offset: 0x0028F3F8
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

		// Token: 0x06004ED4 RID: 20180 RVA: 0x00291040 File Offset: 0x0028F440
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

		// Token: 0x06004ED5 RID: 20181 RVA: 0x002910A0 File Offset: 0x0028F4A0
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

		// Token: 0x06004ED6 RID: 20182 RVA: 0x00291134 File Offset: 0x0028F534
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

		// Token: 0x06004ED7 RID: 20183 RVA: 0x00291270 File Offset: 0x0028F670
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

		// Token: 0x06004ED8 RID: 20184 RVA: 0x002912A8 File Offset: 0x0028F6A8
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

		// Token: 0x06004ED9 RID: 20185 RVA: 0x00291326 File Offset: 0x0028F726
		public void Strip()
		{
			this.InnerPawn.Strip();
		}

		// Token: 0x06004EDA RID: 20186 RVA: 0x00291334 File Offset: 0x0028F734
		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.InnerPawn.Drawer.renderer.RenderPawnAt(drawLoc);
		}

		// Token: 0x06004EDB RID: 20187 RVA: 0x00291350 File Offset: 0x0028F750
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

		// Token: 0x06004EDC RID: 20188 RVA: 0x002913D0 File Offset: 0x0028F7D0
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

		// Token: 0x06004EDD RID: 20189 RVA: 0x002914C4 File Offset: 0x0028F8C4
		public void RotStageChanged()
		{
			PortraitsCache.SetDirty(this.InnerPawn);
			this.NotifyColonistBar();
		}

		// Token: 0x06004EDE RID: 20190 RVA: 0x002914D8 File Offset: 0x0028F8D8
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

		// Token: 0x06004EDF RID: 20191 RVA: 0x0029154F File Offset: 0x0028F94F
		private void NotifyColonistBar()
		{
			if (this.InnerPawn.Faction == Faction.OfPlayer && Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		// Token: 0x04003463 RID: 13411
		private ThingOwner<Pawn> innerContainer;

		// Token: 0x04003464 RID: 13412
		public int timeOfDeath = -1;

		// Token: 0x04003465 RID: 13413
		private int vanishAfterTimestamp = -1;

		// Token: 0x04003466 RID: 13414
		private BillStack operationsBillStack = null;

		// Token: 0x04003467 RID: 13415
		public bool everBuriedInSarcophagus;

		// Token: 0x04003468 RID: 13416
		private const int VanishAfterTicksSinceDessicated = 6000000;
	}
}
