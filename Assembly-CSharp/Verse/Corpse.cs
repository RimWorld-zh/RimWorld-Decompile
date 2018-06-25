using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using RimWorld;
using UnityEngine;

namespace Verse
{
	public class Corpse : ThingWithComps, IThingHolder, IThoughtGiver, IStrippable, IBillGiver
	{
		private ThingOwner<Pawn> innerContainer;

		public int timeOfDeath = -1;

		private int vanishAfterTimestamp = -1;

		private BillStack operationsBillStack = null;

		public bool everBuriedInSarcophagus;

		private const int VanishAfterTicksSinceDessicated = 6000000;

		public Corpse()
		{
			this.operationsBillStack = new BillStack(this);
			this.innerContainer = new ThingOwner<Pawn>(this, true, LookMode.Reference);
		}

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

		private bool ShouldVanish
		{
			get
			{
				return this.InnerPawn.RaceProps.Animal && this.vanishAfterTimestamp > 0 && this.Age >= this.vanishAfterTimestamp && base.Spawned && this.GetRoom(RegionType.Set_Passable) != null && this.GetRoom(RegionType.Set_Passable).TouchesMapEdge && !base.Map.roofGrid.Roofed(base.Position);
			}
		}

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

		public BillStack BillStack
		{
			get
			{
				return this.operationsBillStack;
			}
		}

		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				yield return this.InteractionCell;
				yield break;
			}
		}

		public bool Bugged
		{
			get
			{
				return this.innerContainer.Count == 0 || this.innerContainer[0] == null || this.innerContainer[0].def == null || this.innerContainer[0].kindDef == null;
			}
		}

		public bool CurrentlyUsableForBills()
		{
			return this.InteractionCell.IsValid;
		}

		public bool UsableForBillsAfterFueling()
		{
			return this.CurrentlyUsableForBills();
		}

		public bool AnythingToStrip()
		{
			return this.InnerPawn.AnythingToStrip();
		}

		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		public override void PostMake()
		{
			base.PostMake();
			this.timeOfDeath = Find.TickManager.TicksGame;
		}

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

		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			if (!this.Bugged)
			{
				this.NotifyColonistBar();
			}
		}

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

		public void Strip()
		{
			this.InnerPawn.Strip();
		}

		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.InnerPawn.Drawer.renderer.RenderPawnAt(drawLoc);
		}

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

		public void RotStageChanged()
		{
			PortraitsCache.SetDirty(this.InnerPawn);
			this.NotifyColonistBar();
		}

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

		private void NotifyColonistBar()
		{
			if (this.InnerPawn.Faction == Faction.OfPlayer && Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<StatDrawEntry> <get_SpecialDisplayStats>__BaseCallProxy0()
		{
			return base.SpecialDisplayStats;
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<StatDrawEntry>, IEnumerator, IDisposable, IEnumerator<StatDrawEntry>
		{
			internal IEnumerator<StatDrawEntry> $locvar0;

			internal StatDrawEntry <s>__1;

			internal StatDef <meatAmount>__2;

			internal StatDef <leatherAmount>__2;

			internal Corpse $this;

			internal StatDrawEntry $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = base.<get_SpecialDisplayStats>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					leatherAmount = StatDefOf.LeatherAmount;
					this.$current = new StatDrawEntry(leatherAmount.category, leatherAmount, base.InnerPawn.GetStatValue(leatherAmount, true), StatRequest.For(base.InnerPawn), ToStringNumberSense.Undefined);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					goto IL_194;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						s = enumerator.Current;
						this.$current = s;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				if (this.GetRotStage() == RotStage.Fresh)
				{
					meatAmount = StatDefOf.MeatAmount;
					this.$current = new StatDrawEntry(meatAmount.category, meatAmount, base.InnerPawn.GetStatValue(meatAmount, true), StatRequest.For(base.InnerPawn), ToStringNumberSense.Undefined);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_194:
				this.$PC = -1;
				return false;
			}

			StatDrawEntry IEnumerator<StatDrawEntry>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<RimWorld.StatDrawEntry>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Corpse.<>c__Iterator0 <>c__Iterator = new Corpse.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator1 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal Corpse $this;

			internal IntVec3 $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.$current = this.InteractionCell;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			IntVec3 IEnumerator<IntVec3>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.IntVec3>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Corpse.<>c__Iterator1 <>c__Iterator = new Corpse.<>c__Iterator1();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <ButcherProducts>c__Iterator2 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal Pawn butcher;

			internal float efficiency;

			internal IEnumerator<Thing> $locvar0;

			internal Thing <t>__1;

			internal Corpse $this;

			internal Thing $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ButcherProducts>c__Iterator2()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = base.InnerPawn.ButcherProducts(butcher, efficiency).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						t = enumerator.Current;
						this.$current = t;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				if (base.InnerPawn.RaceProps.BloodDef != null)
				{
					FilthMaker.MakeFilth(butcher.Position, butcher.Map, base.InnerPawn.RaceProps.BloodDef, base.InnerPawn.LabelIndefinite(), 1);
				}
				if (base.InnerPawn.RaceProps.Humanlike)
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
				this.$PC = -1;
				return false;
			}

			Thing IEnumerator<Thing>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Thing>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Corpse.<ButcherProducts>c__Iterator2 <ButcherProducts>c__Iterator = new Corpse.<ButcherProducts>c__Iterator2();
				<ButcherProducts>c__Iterator.$this = this;
				<ButcherProducts>c__Iterator.butcher = butcher;
				<ButcherProducts>c__Iterator.efficiency = efficiency;
				return <ButcherProducts>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GetBestBodyPartToEat>c__AnonStorey3
		{
			internal float nutritionWanted;

			internal Corpse $this;

			public <GetBestBodyPartToEat>c__AnonStorey3()
			{
			}

			internal bool <>m__0(BodyPartRecord x)
			{
				return x.depth == BodyPartDepth.Outside && FoodUtility.GetBodyPartNutrition(this.$this, x) > 0.001f;
			}

			internal float <>m__1(BodyPartRecord x)
			{
				return Mathf.Abs(FoodUtility.GetBodyPartNutrition(this.$this, x) - this.nutritionWanted);
			}
		}
	}
}
