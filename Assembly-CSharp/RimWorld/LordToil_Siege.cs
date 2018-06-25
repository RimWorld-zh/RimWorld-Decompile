using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class LordToil_Siege : LordToil
	{
		public Dictionary<Pawn, DutyDef> rememberedDuties = new Dictionary<Pawn, DutyDef>();

		private const float BaseRadiusMin = 14f;

		private const float BaseRadiusMax = 25f;

		private static readonly FloatRange MealCountRangePerRaider = new FloatRange(1f, 3f);

		private const int StartBuildingDelay = 450;

		private static readonly FloatRange BuilderCountFraction = new FloatRange(0.25f, 0.4f);

		private const float FractionLossesToAssault = 0.4f;

		private const int InitalShellsPerCannon = 5;

		private const int ReplenishAtShells = 4;

		private const int ShellReplenishCount = 10;

		private const int ReplenishAtMeals = 5;

		private const int MealReplenishCount = 12;

		[CompilerGenerated]
		private static Func<Frame, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Blueprint, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Predicate<Blueprint> <>f__am$cache2;

		public LordToil_Siege(IntVec3 siegeCenter, float blueprintPoints)
		{
			this.data = new LordToilData_Siege();
			this.Data.siegeCenter = siegeCenter;
			this.Data.blueprintPoints = blueprintPoints;
		}

		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.siegeCenter;
			}
		}

		private LordToilData_Siege Data
		{
			get
			{
				return (LordToilData_Siege)this.data;
			}
		}

		private IEnumerable<Frame> Frames
		{
			get
			{
				LordToilData_Siege data = this.Data;
				float radSquared = (data.baseRadius + 10f) * (data.baseRadius + 10f);
				List<Thing> framesList = base.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingFrame);
				if (framesList.Count == 0)
				{
					yield break;
				}
				for (int i = 0; i < framesList.Count; i++)
				{
					Frame frame = (Frame)framesList[i];
					if (frame.Faction == this.lord.faction && (float)(frame.Position - data.siegeCenter).LengthHorizontalSquared < radSquared)
					{
						yield return frame;
					}
				}
				yield break;
			}
		}

		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		public override void Init()
		{
			base.Init();
			LordToilData_Siege data = this.Data;
			data.baseRadius = Mathf.InverseLerp(14f, 25f, (float)this.lord.ownedPawns.Count / 50f);
			data.baseRadius = Mathf.Clamp(data.baseRadius, 14f, 25f);
			List<Thing> list = new List<Thing>();
			foreach (Blueprint_Build blueprint_Build in SiegeBlueprintPlacer.PlaceBlueprints(data.siegeCenter, base.Map, this.lord.faction, data.blueprintPoints))
			{
				data.blueprints.Add(blueprint_Build);
				using (List<ThingDefCountClass>.Enumerator enumerator2 = blueprint_Build.MaterialsNeeded().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ThingDefCountClass cost = enumerator2.Current;
						Thing thing = list.FirstOrDefault((Thing t) => t.def == cost.thingDef);
						if (thing != null)
						{
							thing.stackCount += cost.count;
						}
						else
						{
							Thing thing2 = ThingMaker.MakeThing(cost.thingDef, null);
							thing2.stackCount = cost.count;
							list.Add(thing2);
						}
					}
				}
				ThingDef thingDef = blueprint_Build.def.entityDefToBuild as ThingDef;
				if (thingDef != null)
				{
					ThingDef turret = thingDef;
					bool allowEMP = false;
					TechLevel techLevel = this.lord.faction.def.techLevel;
					ThingDef thingDef2 = TurretGunUtility.TryFindRandomShellDef(turret, allowEMP, true, techLevel, false, 250f);
					if (thingDef2 != null)
					{
						Thing thing3 = ThingMaker.MakeThing(thingDef2, null);
						thing3.stackCount = 5;
						list.Add(thing3);
					}
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				list[i].stackCount = Mathf.CeilToInt((float)list[i].stackCount * Rand.Range(1f, 1.2f));
			}
			List<List<Thing>> list2 = new List<List<Thing>>();
			for (int j = 0; j < list.Count; j++)
			{
				while (list[j].stackCount > list[j].def.stackLimit)
				{
					int num = Mathf.CeilToInt((float)list[j].def.stackLimit * Rand.Range(0.9f, 0.999f));
					Thing thing4 = ThingMaker.MakeThing(list[j].def, null);
					thing4.stackCount = num;
					list[j].stackCount -= num;
					list.Add(thing4);
				}
			}
			List<Thing> list3 = new List<Thing>();
			for (int k = 0; k < list.Count; k++)
			{
				list3.Add(list[k]);
				if (k % 2 == 1 || k == list.Count - 1)
				{
					list2.Add(list3);
					list3 = new List<Thing>();
				}
			}
			List<Thing> list4 = new List<Thing>();
			int num2 = Mathf.RoundToInt(LordToil_Siege.MealCountRangePerRaider.RandomInRange * (float)this.lord.ownedPawns.Count);
			for (int l = 0; l < num2; l++)
			{
				Thing item = ThingMaker.MakeThing(ThingDefOf.MealSurvivalPack, null);
				list4.Add(item);
			}
			list2.Add(list4);
			DropPodUtility.DropThingGroupsNear(data.siegeCenter, base.Map, list2, 110, false, false, true, false);
			data.desiredBuilderFraction = LordToil_Siege.BuilderCountFraction.RandomInRange;
		}

		public override void UpdateAllDuties()
		{
			LordToilData_Siege data = this.Data;
			if (this.lord.ticksInToil < 450)
			{
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					this.SetAsDefender(this.lord.ownedPawns[i]);
				}
			}
			else
			{
				this.rememberedDuties.Clear();
				int num = Mathf.RoundToInt((float)this.lord.ownedPawns.Count * data.desiredBuilderFraction);
				if (num <= 0)
				{
					num = 1;
				}
				int num2 = (from b in base.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial)
				where b.def.hasInteractionCell && b.Faction == this.lord.faction && b.Position.InHorDistOf(this.FlagLoc, data.baseRadius)
				select b).Count<Thing>();
				if (num < num2)
				{
					num = num2;
				}
				int num3 = 0;
				for (int j = 0; j < this.lord.ownedPawns.Count; j++)
				{
					Pawn pawn = this.lord.ownedPawns[j];
					if (pawn.mindState.duty.def == DutyDefOf.Build)
					{
						this.rememberedDuties.Add(pawn, DutyDefOf.Build);
						this.SetAsBuilder(pawn);
						num3++;
					}
				}
				int num4 = num - num3;
				for (int k = 0; k < num4; k++)
				{
					Pawn pawn2;
					if ((from pa in this.lord.ownedPawns
					where !this.rememberedDuties.ContainsKey(pa) && this.CanBeBuilder(pa)
					select pa).TryRandomElement(out pawn2))
					{
						this.rememberedDuties.Add(pawn2, DutyDefOf.Build);
						this.SetAsBuilder(pawn2);
						num3++;
					}
				}
				for (int l = 0; l < this.lord.ownedPawns.Count; l++)
				{
					Pawn pawn3 = this.lord.ownedPawns[l];
					if (!this.rememberedDuties.ContainsKey(pawn3))
					{
						this.SetAsDefender(pawn3);
						this.rememberedDuties.Add(pawn3, DutyDefOf.Defend);
					}
				}
				if (num3 == 0)
				{
					this.lord.ReceiveMemo("NoBuilders");
				}
			}
		}

		public override void Notify_PawnLost(Pawn victim, PawnLostCondition cond)
		{
			this.UpdateAllDuties();
			base.Notify_PawnLost(victim, cond);
		}

		public override void Notify_ConstructionFailed(Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
		{
			base.Notify_ConstructionFailed(pawn, frame, newBlueprint);
			if (frame.Faction == this.lord.faction && newBlueprint != null)
			{
				this.Data.blueprints.Add(newBlueprint);
			}
		}

		private bool CanBeBuilder(Pawn p)
		{
			return !p.story.WorkTypeIsDisabled(WorkTypeDefOf.Construction) && !p.story.WorkTypeIsDisabled(WorkTypeDefOf.Firefighter);
		}

		private void SetAsBuilder(Pawn p)
		{
			LordToilData_Siege data = this.Data;
			p.mindState.duty = new PawnDuty(DutyDefOf.Build, data.siegeCenter, -1f);
			p.mindState.duty.radius = data.baseRadius;
			int minLevel = Mathf.Max(ThingDefOf.Sandbags.constructionSkillPrerequisite, ThingDefOf.Turret_Mortar.constructionSkillPrerequisite);
			p.skills.GetSkill(SkillDefOf.Construction).EnsureMinLevelWithMargin(minLevel);
			p.workSettings.EnableAndInitialize();
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				if (workTypeDef == WorkTypeDefOf.Construction)
				{
					p.workSettings.SetPriority(workTypeDef, 1);
				}
				else
				{
					p.workSettings.Disable(workTypeDef);
				}
			}
		}

		private void SetAsDefender(Pawn p)
		{
			LordToilData_Siege data = this.Data;
			p.mindState.duty = new PawnDuty(DutyDefOf.Defend, data.siegeCenter, -1f);
			p.mindState.duty.radius = data.baseRadius;
		}

		public override void LordToilTick()
		{
			base.LordToilTick();
			LordToilData_Siege data = this.Data;
			if (this.lord.ticksInToil == 450)
			{
				this.lord.CurLordToil.UpdateAllDuties();
			}
			if (this.lord.ticksInToil > 450)
			{
				if (this.lord.ticksInToil % 500 == 0)
				{
					this.UpdateAllDuties();
				}
			}
			if (Find.TickManager.TicksGame % 500 == 0)
			{
				if (!(from frame in this.Frames
				where !frame.Destroyed
				select frame).Any<Frame>())
				{
					if (!(from blue in data.blueprints
					where !blue.Destroyed
					select blue).Any<Blueprint>())
					{
						if (!base.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial).Any((Thing b) => b.Faction == this.lord.faction && b.def.building.buildingTags.Contains("Artillery")))
						{
							this.lord.ReceiveMemo("NoArtillery");
							return;
						}
					}
				}
				int num = GenRadial.NumCellsInRadius(20f);
				int num2 = 0;
				int num3 = 0;
				for (int i = 0; i < num; i++)
				{
					IntVec3 c = data.siegeCenter + GenRadial.RadialPattern[i];
					if (c.InBounds(base.Map))
					{
						List<Thing> thingList = c.GetThingList(base.Map);
						for (int j = 0; j < thingList.Count; j++)
						{
							if (thingList[j].def.IsShell)
							{
								num2 += thingList[j].stackCount;
							}
							if (thingList[j].def == ThingDefOf.MealSurvivalPack)
							{
								num3 += thingList[j].stackCount;
							}
						}
					}
				}
				if (num2 < 4)
				{
					ThingDef turret_Mortar = ThingDefOf.Turret_Mortar;
					bool allowEMP = false;
					TechLevel techLevel = this.lord.faction.def.techLevel;
					ThingDef thingDef = TurretGunUtility.TryFindRandomShellDef(turret_Mortar, allowEMP, true, techLevel, false, 250f);
					if (thingDef != null)
					{
						this.DropSupplies(thingDef, 10);
					}
				}
				if (num3 < 5)
				{
					this.DropSupplies(ThingDefOf.MealSurvivalPack, 12);
				}
			}
		}

		private void DropSupplies(ThingDef thingDef, int count)
		{
			List<Thing> list = new List<Thing>();
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			thing.stackCount = count;
			list.Add(thing);
			DropPodUtility.DropThingsNear(this.Data.siegeCenter, base.Map, list, 110, false, false, true, false);
		}

		public override void Cleanup()
		{
			LordToilData_Siege data = this.Data;
			data.blueprints.RemoveAll((Blueprint blue) => blue.Destroyed);
			for (int i = 0; i < data.blueprints.Count; i++)
			{
				data.blueprints[i].Destroy(DestroyMode.Cancel);
			}
			foreach (Frame frame in this.Frames.ToList<Frame>())
			{
				frame.Destroy(DestroyMode.Cancel);
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static LordToil_Siege()
		{
		}

		[CompilerGenerated]
		private static bool <LordToilTick>m__0(Frame frame)
		{
			return !frame.Destroyed;
		}

		[CompilerGenerated]
		private static bool <LordToilTick>m__1(Blueprint blue)
		{
			return !blue.Destroyed;
		}

		[CompilerGenerated]
		private bool <LordToilTick>m__2(Thing b)
		{
			return b.Faction == this.lord.faction && b.def.building.buildingTags.Contains("Artillery");
		}

		[CompilerGenerated]
		private static bool <Cleanup>m__3(Blueprint blue)
		{
			return blue.Destroyed;
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<Frame>, IEnumerator, IDisposable, IEnumerator<Frame>
		{
			internal LordToilData_Siege <data>__0;

			internal float <radSquared>__0;

			internal List<Thing> <framesList>__0;

			internal int <i>__1;

			internal Frame <frame>__2;

			internal LordToil_Siege $this;

			internal Frame $current;

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
				switch (num)
				{
				case 0u:
					data = base.Data;
					radSquared = (data.baseRadius + 10f) * (data.baseRadius + 10f);
					framesList = base.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingFrame);
					if (framesList.Count == 0)
					{
						return false;
					}
					i = 0;
					break;
				case 1u:
					IL_126:
					i++;
					break;
				default:
					return false;
				}
				if (i >= framesList.Count)
				{
					this.$PC = -1;
				}
				else
				{
					frame = (Frame)framesList[i];
					if (frame.Faction == this.lord.faction && (float)(frame.Position - data.siegeCenter).LengthHorizontalSquared < radSquared)
					{
						this.$current = frame;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_126;
				}
				return false;
			}

			Frame IEnumerator<Frame>.Current
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.Frame>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Frame> IEnumerable<Frame>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				LordToil_Siege.<>c__Iterator0 <>c__Iterator = new LordToil_Siege.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <Init>c__AnonStorey1
		{
			internal ThingDefCountClass cost;

			public <Init>c__AnonStorey1()
			{
			}

			internal bool <>m__0(Thing t)
			{
				return t.def == this.cost.thingDef;
			}
		}

		[CompilerGenerated]
		private sealed class <UpdateAllDuties>c__AnonStorey2
		{
			internal LordToilData_Siege data;

			internal LordToil_Siege $this;

			public <UpdateAllDuties>c__AnonStorey2()
			{
			}

			internal bool <>m__0(Thing b)
			{
				return b.def.hasInteractionCell && b.Faction == this.$this.lord.faction && b.Position.InHorDistOf(this.$this.FlagLoc, this.data.baseRadius);
			}

			internal bool <>m__1(Pawn pa)
			{
				return !this.$this.rememberedDuties.ContainsKey(pa) && this.$this.CanBeBuilder(pa);
			}
		}
	}
}
