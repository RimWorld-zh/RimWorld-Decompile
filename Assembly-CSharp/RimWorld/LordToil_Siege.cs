using System;
using System.Collections.Generic;
using System.Linq;
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
				return (LordToilData_Siege)base.data;
			}
		}

		private IEnumerable<Frame> Frames
		{
			get
			{
				LordToilData_Siege data = this.Data;
				float radSquared = (float)((data.baseRadius + 10.0) * (data.baseRadius + 10.0));
				List<Thing> framesList = base.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingFrame);
				if (framesList.Count != 0)
				{
					int i = 0;
					Frame frame;
					while (true)
					{
						if (i < framesList.Count)
						{
							frame = (Frame)framesList[i];
							if (frame.Faction == base.lord.faction && (float)(frame.Position - data.siegeCenter).LengthHorizontalSquared < radSquared)
								break;
							i++;
							continue;
						}
						yield break;
					}
					yield return frame;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
		}

		public LordToil_Siege(IntVec3 siegeCenter, float blueprintPoints)
		{
			base.data = new LordToilData_Siege();
			this.Data.siegeCenter = siegeCenter;
			this.Data.blueprintPoints = blueprintPoints;
		}

		public override void Init()
		{
			base.Init();
			LordToilData_Siege data = this.Data;
			data.baseRadius = Mathf.InverseLerp(14f, 25f, (float)((float)base.lord.ownedPawns.Count / 50.0));
			data.baseRadius = Mathf.Clamp(data.baseRadius, 14f, 25f);
			List<Thing> list = new List<Thing>();
			foreach (Blueprint_Build item2 in SiegeBlueprintPlacer.PlaceBlueprints(data.siegeCenter, base.Map, base.lord.faction, data.blueprintPoints))
			{
				data.blueprints.Add(item2);
				foreach (ThingCountClass item3 in item2.MaterialsNeeded())
				{
					Thing thing = list.FirstOrDefault((Func<Thing, bool>)((Thing t) => t.def == item3.thingDef));
					if (thing != null)
					{
						thing.stackCount += item3.count;
					}
					else
					{
						Thing thing2 = ThingMaker.MakeThing(item3.thingDef, null);
						thing2.stackCount = item3.count;
						list.Add(thing2);
					}
				}
				ThingDef thingDef = item2.def.entityDefToBuild as ThingDef;
				if (thingDef != null)
				{
					ThingDef turret = thingDef;
					bool allowEMP = false;
					TechLevel techLevel = base.lord.faction.def.techLevel;
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
			int num2 = Mathf.RoundToInt(LordToil_Siege.MealCountRangePerRaider.RandomInRange * (float)base.lord.ownedPawns.Count);
			for (int num3 = 0; num3 < num2; num3++)
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
			if (base.lord.ticksInToil < 450)
			{
				for (int i = 0; i < base.lord.ownedPawns.Count; i++)
				{
					this.SetAsDefender(base.lord.ownedPawns[i]);
				}
			}
			else
			{
				this.rememberedDuties.Clear();
				int num = Mathf.RoundToInt((float)base.lord.ownedPawns.Count * data.desiredBuilderFraction);
				if (num <= 0)
				{
					num = 1;
				}
				int num2 = (from b in base.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial)
				where b.def.hasInteractionCell && b.Faction == base.lord.faction && b.Position.InHorDistOf(this.FlagLoc, data.baseRadius)
				select b).Count();
				if (num < num2)
				{
					num = num2;
				}
				int num3 = 0;
				for (int j = 0; j < base.lord.ownedPawns.Count; j++)
				{
					Pawn pawn = base.lord.ownedPawns[j];
					if (pawn.mindState.duty.def == DutyDefOf.Build)
					{
						this.rememberedDuties.Add(pawn, DutyDefOf.Build);
						this.SetAsBuilder(pawn);
						num3++;
					}
				}
				int num4 = num - num3;
				for (int num5 = 0; num5 < num4; num5++)
				{
					Pawn pawn2 = default(Pawn);
					if ((from pa in base.lord.ownedPawns
					where !this.rememberedDuties.ContainsKey(pa) && this.CanBeBuilder(pa)
					select pa).TryRandomElement<Pawn>(out pawn2))
					{
						this.rememberedDuties.Add(pawn2, DutyDefOf.Build);
						this.SetAsBuilder(pawn2);
						num3++;
					}
				}
				for (int k = 0; k < base.lord.ownedPawns.Count; k++)
				{
					Pawn pawn3 = base.lord.ownedPawns[k];
					if (!this.rememberedDuties.ContainsKey(pawn3))
					{
						this.SetAsDefender(pawn3);
						this.rememberedDuties.Add(pawn3, DutyDefOf.Defend);
					}
				}
				if (num3 == 0)
				{
					base.lord.ReceiveMemo("NoBuilders");
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
			if (frame.Faction == base.lord.faction && newBlueprint != null)
			{
				this.Data.blueprints.Add(newBlueprint);
			}
		}

		private bool CanBeBuilder(Pawn p)
		{
			return (byte)((!p.story.WorkTypeIsDisabled(WorkTypeDefOf.Construction) && !p.story.WorkTypeIsDisabled(WorkTypeDefOf.Firefighter)) ? 1 : 0) != 0;
		}

		private void SetAsBuilder(Pawn p)
		{
			LordToilData_Siege data = this.Data;
			p.mindState.duty = new PawnDuty(DutyDefOf.Build, data.siegeCenter, -1f);
			p.mindState.duty.radius = data.baseRadius;
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
			if (base.lord.ticksInToil == 450)
			{
				base.lord.CurLordToil.UpdateAllDuties();
			}
			if (base.lord.ticksInToil > 450 && base.lord.ticksInToil % 500 == 0)
			{
				this.UpdateAllDuties();
			}
			if (Find.TickManager.TicksGame % 500 == 0)
			{
				if (!(from frame in this.Frames
				where !frame.Destroyed
				select frame).Any() && !(from blue in data.blueprints
				where !blue.Destroyed
				select blue).Any() && !base.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial).Any((Predicate<Thing>)((Thing b) => b.Faction == base.lord.faction && b.def.building.buildingTags.Contains("Artillery"))))
				{
					base.lord.ReceiveMemo("NoArtillery");
				}
				else
				{
					int num = GenRadial.NumCellsInRadius(20f);
					int num2 = 0;
					int num3 = 0;
					for (int num4 = 0; num4 < num; num4++)
					{
						IntVec3 c = data.siegeCenter + GenRadial.RadialPattern[num4];
						if (c.InBounds(base.Map))
						{
							List<Thing> thingList = c.GetThingList(base.Map);
							for (int i = 0; i < thingList.Count; i++)
							{
								if (thingList[i].def.IsShell)
								{
									num2 += thingList[i].stackCount;
								}
								if (thingList[i].def == ThingDefOf.MealSurvivalPack)
								{
									num3 += thingList[i].stackCount;
								}
							}
						}
					}
					if (num2 < 4)
					{
						ThingDef turret_Mortar = ThingDefOf.Turret_Mortar;
						bool allowEMP = false;
						TechLevel techLevel = base.lord.faction.def.techLevel;
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
			data.blueprints.RemoveAll((Predicate<Blueprint>)((Blueprint blue) => blue.Destroyed));
			for (int i = 0; i < data.blueprints.Count; i++)
			{
				data.blueprints[i].Destroy(DestroyMode.Cancel);
			}
			foreach (Frame item in this.Frames.ToList())
			{
				item.Destroy(DestroyMode.Cancel);
			}
		}
	}
}
