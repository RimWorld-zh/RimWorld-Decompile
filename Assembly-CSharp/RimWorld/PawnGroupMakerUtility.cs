using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[HasDebugOutput]
	public class PawnGroupMakerUtility
	{
		private static readonly SimpleCurve PawnWeightFactorByMostExpensivePawnCostFractionCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.2f, 0.01f),
				true
			},
			{
				new CurvePoint(0.3f, 0.3f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			}
		};

		[CompilerGenerated]
		private static Func<PawnGroupMaker, float> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Faction, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<Faction, string> <>f__am$cache2;

		[CompilerGenerated]
		private static Action<Faction> <>f__am$cache3;

		public PawnGroupMakerUtility()
		{
		}

		public static IEnumerable<Pawn> GeneratePawns(PawnGroupMakerParms parms, bool warnOnZeroResults = true)
		{
			if (parms.groupKind == null)
			{
				Log.Error("Tried to generate pawns with null pawn group kind def. parms=" + parms, false);
				yield break;
			}
			if (parms.faction == null)
			{
				Log.Error("Tried to generate pawn kinds with null faction. parms=" + parms, false);
				yield break;
			}
			if (parms.faction.def.pawnGroupMakers.NullOrEmpty<PawnGroupMaker>())
			{
				Log.Error(string.Concat(new object[]
				{
					"Faction ",
					parms.faction,
					" of def ",
					parms.faction.def,
					" has no any PawnGroupMakers."
				}), false);
				yield break;
			}
			PawnGroupMaker chosenGroupMaker;
			if (!PawnGroupMakerUtility.TryGetRandomPawnGroupMaker(parms, out chosenGroupMaker))
			{
				Log.Error(string.Concat(new object[]
				{
					"Faction ",
					parms.faction,
					" of def ",
					parms.faction.def,
					" has no usable PawnGroupMakers for parms ",
					parms
				}), false);
				yield break;
			}
			foreach (Pawn p in chosenGroupMaker.GeneratePawns(parms, warnOnZeroResults))
			{
				yield return p;
			}
			yield break;
		}

		public static IEnumerable<PawnKindDef> GeneratePawnKindsExample(PawnGroupMakerParms parms)
		{
			if (parms.groupKind == null)
			{
				Log.Error("Tried to generate pawn kinds with null pawn group kind def. parms=" + parms, false);
				yield break;
			}
			if (parms.faction == null)
			{
				Log.Error("Tried to generate pawn kinds with null faction. parms=" + parms, false);
				yield break;
			}
			if (parms.faction.def.pawnGroupMakers.NullOrEmpty<PawnGroupMaker>())
			{
				Log.Error(string.Concat(new object[]
				{
					"Faction ",
					parms.faction,
					" of def ",
					parms.faction.def,
					" has no any PawnGroupMakers."
				}), false);
				yield break;
			}
			PawnGroupMaker chosenGroupMaker;
			if (!PawnGroupMakerUtility.TryGetRandomPawnGroupMaker(parms, out chosenGroupMaker))
			{
				Log.Error(string.Concat(new object[]
				{
					"Faction ",
					parms.faction,
					" of def ",
					parms.faction.def,
					" has no usable PawnGroupMakers for parms ",
					parms
				}), false);
				yield break;
			}
			foreach (PawnKindDef p in chosenGroupMaker.GeneratePawnKindsExample(parms))
			{
				yield return p;
			}
			yield break;
		}

		private static bool TryGetRandomPawnGroupMaker(PawnGroupMakerParms parms, out PawnGroupMaker pawnGroupMaker)
		{
			if (parms.seed != null)
			{
				Rand.PushState(parms.seed.Value);
			}
			IEnumerable<PawnGroupMaker> source = from gm in parms.faction.def.pawnGroupMakers
			where gm.kindDef == parms.groupKind && gm.CanGenerateFrom(parms)
			select gm;
			bool result = source.TryRandomElementByWeight((PawnGroupMaker gm) => gm.commonality, out pawnGroupMaker);
			if (parms.seed != null)
			{
				Rand.PopState();
			}
			return result;
		}

		public static IEnumerable<PawnGenOption> ChoosePawnGenOptionsByPoints(float pointsTotal, List<PawnGenOption> options, PawnGroupMakerParms groupParms)
		{
			if (groupParms.seed != null)
			{
				Rand.PushState(groupParms.seed.Value);
			}
			float num = PawnGroupMakerUtility.MaxPawnCost(groupParms.faction, pointsTotal, groupParms.raidStrategy, groupParms.groupKind);
			List<PawnGenOption> list = new List<PawnGenOption>();
			List<PawnGenOption> list2 = new List<PawnGenOption>();
			float num2 = pointsTotal;
			bool flag = false;
			float highestCost = -1f;
			for (;;)
			{
				list.Clear();
				for (int i = 0; i < options.Count; i++)
				{
					PawnGenOption pawnGenOption = options[i];
					if (pawnGenOption.Cost <= num2)
					{
						if (pawnGenOption.Cost <= num)
						{
							if (!groupParms.generateFightersOnly || pawnGenOption.kind.isFighter)
							{
								if (groupParms.raidStrategy == null || groupParms.raidStrategy.Worker.CanUsePawnGenOption(pawnGenOption, list2))
								{
									if (!groupParms.dontUseSingleUseRocketLaunchers || pawnGenOption.kind.weaponTags == null || !pawnGenOption.kind.weaponTags.Contains("GunHeavy"))
									{
										if (!flag || !pawnGenOption.kind.factionLeader)
										{
											if (pawnGenOption.Cost > highestCost)
											{
												highestCost = pawnGenOption.Cost;
											}
											list.Add(pawnGenOption);
										}
									}
								}
							}
						}
					}
				}
				if (list.Count == 0)
				{
					break;
				}
				Func<PawnGenOption, float> weightSelector = delegate(PawnGenOption gr)
				{
					float selectionWeight = gr.selectionWeight;
					return selectionWeight * PawnGroupMakerUtility.PawnWeightFactorByMostExpensivePawnCostFractionCurve.Evaluate(gr.Cost / highestCost);
				};
				PawnGenOption pawnGenOption2 = list.RandomElementByWeight(weightSelector);
				list2.Add(pawnGenOption2);
				num2 -= pawnGenOption2.Cost;
				if (pawnGenOption2.kind.factionLeader)
				{
					flag = true;
				}
			}
			if (list2.Count == 1 && num2 > pointsTotal / 2f)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Used only ",
					pointsTotal - num2,
					" / ",
					pointsTotal,
					" points generating for ",
					groupParms.faction
				}), false);
			}
			if (groupParms.seed != null)
			{
				Rand.PopState();
			}
			return list2;
		}

		public static float MaxPawnCost(Faction faction, float totalPoints, RaidStrategyDef raidStrategy, PawnGroupKindDef groupKind)
		{
			float num = faction.def.maxPawnCostPerTotalPointsCurve.Evaluate(totalPoints);
			if (raidStrategy != null)
			{
				num = Mathf.Min(num, totalPoints / raidStrategy.minPawns);
			}
			num = Mathf.Max(num, faction.def.MinPointsToGeneratePawnGroup(groupKind) * 1.2f);
			if (raidStrategy != null)
			{
				num = Mathf.Max(num, raidStrategy.Worker.MinMaxAllowedPawnGenOptionCost(faction, groupKind) * 1.2f);
			}
			return num;
		}

		public static bool CanGenerateAnyNormalGroup(Faction faction, float points)
		{
			bool result;
			if (faction.def.pawnGroupMakers == null)
			{
				result = false;
			}
			else
			{
				PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
				pawnGroupMakerParms.faction = faction;
				pawnGroupMakerParms.points = points;
				for (int i = 0; i < faction.def.pawnGroupMakers.Count; i++)
				{
					PawnGroupMaker pawnGroupMaker = faction.def.pawnGroupMakers[i];
					if (pawnGroupMaker.kindDef == PawnGroupKindDefOf.Combat)
					{
						if (pawnGroupMaker.CanGenerateFrom(pawnGroupMakerParms))
						{
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		[DebugOutput]
		public static void PawnGroupsMade()
		{
			Dialog_DebugOptionListLister.ShowSimpleDebugMenu<Faction>(from fac in Find.FactionManager.AllFactions
			where !fac.def.pawnGroupMakers.NullOrEmpty<PawnGroupMaker>()
			select fac, (Faction fac) => fac.Name + " (" + fac.def.defName + ")", delegate(Faction fac)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine(string.Concat(new object[]
				{
					"FACTION: ",
					fac.Name,
					" (",
					fac.def.defName,
					") min=",
					fac.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat)
				}));
				Action<float> action = delegate(float points)
				{
					if (points >= fac.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat))
					{
						PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
						pawnGroupMakerParms.groupKind = PawnGroupKindDefOf.Combat;
						pawnGroupMakerParms.tile = Find.CurrentMap.Tile;
						pawnGroupMakerParms.points = points;
						pawnGroupMakerParms.faction = fac;
						sb.AppendLine(string.Concat(new object[]
						{
							"Group with ",
							pawnGroupMakerParms.points,
							" points (max option cost: ",
							PawnGroupMakerUtility.MaxPawnCost(fac, points, RaidStrategyDefOf.ImmediateAttack, PawnGroupKindDefOf.Combat),
							")"
						}));
						float num2 = 0f;
						foreach (Pawn pawn in PawnGroupMakerUtility.GeneratePawns(pawnGroupMakerParms, false).OrderBy((Pawn pa) => pa.kindDef.combatPower))
						{
							string text;
							if (pawn.equipment.Primary != null)
							{
								text = pawn.equipment.Primary.Label;
							}
							else
							{
								text = "no-equipment";
							}
							Apparel apparel = pawn.apparel.FirstApparelOnBodyPartGroup(BodyPartGroupDefOf.Torso);
							string text2;
							if (apparel != null)
							{
								text2 = apparel.LabelCap;
							}
							else
							{
								text2 = "shirtless";
							}
							sb.AppendLine(string.Concat(new string[]
							{
								"  ",
								pawn.kindDef.combatPower.ToString("F0").PadRight(6),
								pawn.kindDef.defName,
								", ",
								text,
								", ",
								text2
							}));
							num2 += pawn.kindDef.combatPower;
						}
						sb.AppendLine("         totalCost " + num2);
						sb.AppendLine();
					}
				};
				foreach (float num in Dialog_DebugActionsMenu.PointsOptions(false))
				{
					float obj = num;
					action(obj);
				}
				Log.Message(sb.ToString(), false);
			});
		}

		public static bool TryGetRandomFactionForCombatPawnGroup(float points, out Faction faction, Predicate<Faction> validator = null, bool allowNonHostileToPlayer = false, bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true)
		{
			List<Faction> source = Find.FactionManager.AllFactions.Where(delegate(Faction f)
			{
				if ((allowHidden || !f.def.hidden) && (allowDefeated || !f.defeated) && (allowNonHumanlike || f.def.humanlikeFaction) && (allowNonHostileToPlayer || f.HostileTo(Faction.OfPlayer)) && f.def.pawnGroupMakers != null)
				{
					if (f.def.pawnGroupMakers.Any((PawnGroupMaker x) => x.kindDef == PawnGroupKindDefOf.Combat) && (validator == null || validator(f)))
					{
						return points >= f.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat);
					}
				}
				return false;
			}).ToList<Faction>();
			return source.TryRandomElementByWeight((Faction f) => f.def.RaidCommonalityFromPoints(points), out faction);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static PawnGroupMakerUtility()
		{
		}

		[CompilerGenerated]
		private static float <TryGetRandomPawnGroupMaker>m__0(PawnGroupMaker gm)
		{
			return gm.commonality;
		}

		[CompilerGenerated]
		private static bool <PawnGroupsMade>m__1(Faction fac)
		{
			return !fac.def.pawnGroupMakers.NullOrEmpty<PawnGroupMaker>();
		}

		[CompilerGenerated]
		private static string <PawnGroupsMade>m__2(Faction fac)
		{
			return fac.Name + " (" + fac.def.defName + ")";
		}

		[CompilerGenerated]
		private static void <PawnGroupsMade>m__3(Faction fac)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(string.Concat(new object[]
			{
				"FACTION: ",
				fac.Name,
				" (",
				fac.def.defName,
				") min=",
				fac.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat)
			}));
			Action<float> action = delegate(float points)
			{
				if (points >= fac.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat))
				{
					PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
					pawnGroupMakerParms.groupKind = PawnGroupKindDefOf.Combat;
					pawnGroupMakerParms.tile = Find.CurrentMap.Tile;
					pawnGroupMakerParms.points = points;
					pawnGroupMakerParms.faction = fac;
					sb.AppendLine(string.Concat(new object[]
					{
						"Group with ",
						pawnGroupMakerParms.points,
						" points (max option cost: ",
						PawnGroupMakerUtility.MaxPawnCost(fac, points, RaidStrategyDefOf.ImmediateAttack, PawnGroupKindDefOf.Combat),
						")"
					}));
					float num2 = 0f;
					foreach (Pawn pawn in from pa in PawnGroupMakerUtility.GeneratePawns(pawnGroupMakerParms, false)
					orderby pa.kindDef.combatPower
					select pa)
					{
						string text;
						if (pawn.equipment.Primary != null)
						{
							text = pawn.equipment.Primary.Label;
						}
						else
						{
							text = "no-equipment";
						}
						Apparel apparel = pawn.apparel.FirstApparelOnBodyPartGroup(BodyPartGroupDefOf.Torso);
						string text2;
						if (apparel != null)
						{
							text2 = apparel.LabelCap;
						}
						else
						{
							text2 = "shirtless";
						}
						sb.AppendLine(string.Concat(new string[]
						{
							"  ",
							pawn.kindDef.combatPower.ToString("F0").PadRight(6),
							pawn.kindDef.defName,
							", ",
							text,
							", ",
							text2
						}));
						num2 += pawn.kindDef.combatPower;
					}
					sb.AppendLine("         totalCost " + num2);
					sb.AppendLine();
				}
			};
			foreach (float num in Dialog_DebugActionsMenu.PointsOptions(false))
			{
				float obj = num;
				action(obj);
			}
			Log.Message(sb.ToString(), false);
		}

		[CompilerGenerated]
		private sealed class <GeneratePawns>c__Iterator0 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal PawnGroupMakerParms parms;

			internal PawnGroupMaker <chosenGroupMaker>__0;

			internal bool warnOnZeroResults;

			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__1;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GeneratePawns>c__Iterator0()
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
					if (parms.groupKind == null)
					{
						Log.Error("Tried to generate pawns with null pawn group kind def. parms=" + parms, false);
						return false;
					}
					if (parms.faction == null)
					{
						Log.Error("Tried to generate pawn kinds with null faction. parms=" + parms, false);
						return false;
					}
					if (parms.faction.def.pawnGroupMakers.NullOrEmpty<PawnGroupMaker>())
					{
						Log.Error(string.Concat(new object[]
						{
							"Faction ",
							parms.faction,
							" of def ",
							parms.faction.def,
							" has no any PawnGroupMakers."
						}), false);
						return false;
					}
					if (!PawnGroupMakerUtility.TryGetRandomPawnGroupMaker(parms, out chosenGroupMaker))
					{
						Log.Error(string.Concat(new object[]
						{
							"Faction ",
							parms.faction,
							" of def ",
							parms.faction.def,
							" has no usable PawnGroupMakers for parms ",
							parms
						}), false);
						return false;
					}
					enumerator = chosenGroupMaker.GeneratePawns(parms, warnOnZeroResults).GetEnumerator();
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
						p = enumerator.Current;
						this.$current = p;
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
				this.$PC = -1;
				return false;
			}

			Pawn IEnumerator<Pawn>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.Pawn>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				PawnGroupMakerUtility.<GeneratePawns>c__Iterator0 <GeneratePawns>c__Iterator = new PawnGroupMakerUtility.<GeneratePawns>c__Iterator0();
				<GeneratePawns>c__Iterator.parms = parms;
				<GeneratePawns>c__Iterator.warnOnZeroResults = warnOnZeroResults;
				return <GeneratePawns>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <GeneratePawnKindsExample>c__Iterator1 : IEnumerable, IEnumerable<PawnKindDef>, IEnumerator, IDisposable, IEnumerator<PawnKindDef>
		{
			internal PawnGroupMakerParms parms;

			internal PawnGroupMaker <chosenGroupMaker>__0;

			internal IEnumerator<PawnKindDef> $locvar0;

			internal PawnKindDef <p>__1;

			internal PawnKindDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GeneratePawnKindsExample>c__Iterator1()
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
					if (parms.groupKind == null)
					{
						Log.Error("Tried to generate pawn kinds with null pawn group kind def. parms=" + parms, false);
						return false;
					}
					if (parms.faction == null)
					{
						Log.Error("Tried to generate pawn kinds with null faction. parms=" + parms, false);
						return false;
					}
					if (parms.faction.def.pawnGroupMakers.NullOrEmpty<PawnGroupMaker>())
					{
						Log.Error(string.Concat(new object[]
						{
							"Faction ",
							parms.faction,
							" of def ",
							parms.faction.def,
							" has no any PawnGroupMakers."
						}), false);
						return false;
					}
					if (!PawnGroupMakerUtility.TryGetRandomPawnGroupMaker(parms, out chosenGroupMaker))
					{
						Log.Error(string.Concat(new object[]
						{
							"Faction ",
							parms.faction,
							" of def ",
							parms.faction.def,
							" has no usable PawnGroupMakers for parms ",
							parms
						}), false);
						return false;
					}
					enumerator = chosenGroupMaker.GeneratePawnKindsExample(parms).GetEnumerator();
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
						p = enumerator.Current;
						this.$current = p;
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
				this.$PC = -1;
				return false;
			}

			PawnKindDef IEnumerator<PawnKindDef>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.PawnKindDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<PawnKindDef> IEnumerable<PawnKindDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				PawnGroupMakerUtility.<GeneratePawnKindsExample>c__Iterator1 <GeneratePawnKindsExample>c__Iterator = new PawnGroupMakerUtility.<GeneratePawnKindsExample>c__Iterator1();
				<GeneratePawnKindsExample>c__Iterator.parms = parms;
				return <GeneratePawnKindsExample>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <TryGetRandomPawnGroupMaker>c__AnonStorey2
		{
			internal PawnGroupMakerParms parms;

			public <TryGetRandomPawnGroupMaker>c__AnonStorey2()
			{
			}

			internal bool <>m__0(PawnGroupMaker gm)
			{
				return gm.kindDef == this.parms.groupKind && gm.CanGenerateFrom(this.parms);
			}
		}

		[CompilerGenerated]
		private sealed class <ChoosePawnGenOptionsByPoints>c__AnonStorey3
		{
			internal float highestCost;

			public <ChoosePawnGenOptionsByPoints>c__AnonStorey3()
			{
			}

			internal float <>m__0(PawnGenOption gr)
			{
				float selectionWeight = gr.selectionWeight;
				return selectionWeight * PawnGroupMakerUtility.PawnWeightFactorByMostExpensivePawnCostFractionCurve.Evaluate(gr.Cost / this.highestCost);
			}
		}

		[CompilerGenerated]
		private sealed class <TryGetRandomFactionForCombatPawnGroup>c__AnonStorey5
		{
			internal bool allowHidden;

			internal bool allowDefeated;

			internal bool allowNonHumanlike;

			internal bool allowNonHostileToPlayer;

			internal Predicate<Faction> validator;

			internal float points;

			private static Predicate<PawnGroupMaker> <>f__am$cache0;

			public <TryGetRandomFactionForCombatPawnGroup>c__AnonStorey5()
			{
			}

			internal bool <>m__0(Faction f)
			{
				if ((this.allowHidden || !f.def.hidden) && (this.allowDefeated || !f.defeated) && (this.allowNonHumanlike || f.def.humanlikeFaction) && (this.allowNonHostileToPlayer || f.HostileTo(Faction.OfPlayer)) && f.def.pawnGroupMakers != null)
				{
					if (f.def.pawnGroupMakers.Any((PawnGroupMaker x) => x.kindDef == PawnGroupKindDefOf.Combat) && (this.validator == null || this.validator(f)))
					{
						return this.points >= f.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat);
					}
				}
				return false;
			}

			internal float <>m__1(Faction f)
			{
				return f.def.RaidCommonalityFromPoints(this.points);
			}

			private static bool <>m__2(PawnGroupMaker x)
			{
				return x.kindDef == PawnGroupKindDefOf.Combat;
			}
		}

		[CompilerGenerated]
		private sealed class <PawnGroupsMade>c__AnonStorey4
		{
			internal Faction fac;

			internal StringBuilder sb;

			private static Func<Pawn, float> <>f__am$cache0;

			public <PawnGroupsMade>c__AnonStorey4()
			{
			}

			internal void <>m__0(float points)
			{
				if (points >= this.fac.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat))
				{
					PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
					pawnGroupMakerParms.groupKind = PawnGroupKindDefOf.Combat;
					pawnGroupMakerParms.tile = Find.CurrentMap.Tile;
					pawnGroupMakerParms.points = points;
					pawnGroupMakerParms.faction = this.fac;
					this.sb.AppendLine(string.Concat(new object[]
					{
						"Group with ",
						pawnGroupMakerParms.points,
						" points (max option cost: ",
						PawnGroupMakerUtility.MaxPawnCost(this.fac, points, RaidStrategyDefOf.ImmediateAttack, PawnGroupKindDefOf.Combat),
						")"
					}));
					float num = 0f;
					foreach (Pawn pawn in from pa in PawnGroupMakerUtility.GeneratePawns(pawnGroupMakerParms, false)
					orderby pa.kindDef.combatPower
					select pa)
					{
						string text;
						if (pawn.equipment.Primary != null)
						{
							text = pawn.equipment.Primary.Label;
						}
						else
						{
							text = "no-equipment";
						}
						Apparel apparel = pawn.apparel.FirstApparelOnBodyPartGroup(BodyPartGroupDefOf.Torso);
						string text2;
						if (apparel != null)
						{
							text2 = apparel.LabelCap;
						}
						else
						{
							text2 = "shirtless";
						}
						this.sb.AppendLine(string.Concat(new string[]
						{
							"  ",
							pawn.kindDef.combatPower.ToString("F0").PadRight(6),
							pawn.kindDef.defName,
							", ",
							text,
							", ",
							text2
						}));
						num += pawn.kindDef.combatPower;
					}
					this.sb.AppendLine("         totalCost " + num);
					this.sb.AppendLine();
				}
			}

			private static float <>m__1(Pawn pa)
			{
				return pa.kindDef.combatPower;
			}
		}
	}
}
