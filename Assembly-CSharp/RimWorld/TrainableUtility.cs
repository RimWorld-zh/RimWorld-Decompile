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
	public static class TrainableUtility
	{
		private static List<TrainableDef> defsInListOrder = new List<TrainableDef>();

		private static readonly SimpleCurve DecayIntervalDaysFromWildnessCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 15f),
				true
			},
			{
				new CurvePoint(1f, 5f),
				true
			}
		};

		[CompilerGenerated]
		private static Func<TrainableDef, float> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Pawn, Pawn> <>f__mg$cache0;

		[CompilerGenerated]
		private static Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<Pawn>>> <>f__mg$cache1;

		[CompilerGenerated]
		private static Func<Pawn, string> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<DirectPawnRelation, bool> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<DirectPawnRelation, Pawn> <>f__am$cache3;

		public static List<TrainableDef> TrainableDefsInListOrder
		{
			get
			{
				return TrainableUtility.defsInListOrder;
			}
		}

		public static void Reset()
		{
			TrainableUtility.defsInListOrder.Clear();
			TrainableUtility.defsInListOrder.AddRange(from td in DefDatabase<TrainableDef>.AllDefsListForReading
			orderby td.listPriority descending
			select td);
			bool flag;
			do
			{
				flag = false;
				for (int i = 0; i < TrainableUtility.defsInListOrder.Count; i++)
				{
					TrainableDef trainableDef = TrainableUtility.defsInListOrder[i];
					if (trainableDef.prerequisites != null)
					{
						for (int j = 0; j < trainableDef.prerequisites.Count; j++)
						{
							if (trainableDef.indent <= trainableDef.prerequisites[j].indent)
							{
								trainableDef.indent = trainableDef.prerequisites[j].indent + 1;
								flag = true;
								break;
							}
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			while (flag);
		}

		public static string MasterString(Pawn pawn)
		{
			return (pawn.playerSettings.Master == null) ? ("(" + "NoneLower".Translate() + ")") : RelationsUtility.LabelWithBondInfo(pawn.playerSettings.Master, pawn);
		}

		public static int MinimumHandlingSkill(Pawn p)
		{
			return Mathf.RoundToInt(p.GetStatValue(StatDefOf.MinimumHandlingSkill, true));
		}

		public static void MasterSelectButton(Rect rect, Pawn pawn, bool paintable)
		{
			Rect rect2 = rect;
			if (TrainableUtility.<>f__mg$cache0 == null)
			{
				TrainableUtility.<>f__mg$cache0 = new Func<Pawn, Pawn>(TrainableUtility.MasterSelectButton_GetMaster);
			}
			Func<Pawn, Pawn> getPayload = TrainableUtility.<>f__mg$cache0;
			if (TrainableUtility.<>f__mg$cache1 == null)
			{
				TrainableUtility.<>f__mg$cache1 = new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<Pawn>>>(TrainableUtility.MasterSelectButton_GenerateMenu);
			}
			Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<Pawn>>> menuGenerator = TrainableUtility.<>f__mg$cache1;
			string buttonLabel = TrainableUtility.MasterString(pawn).Truncate(rect.width, null);
			string dragLabel = TrainableUtility.MasterString(pawn);
			Widgets.Dropdown<Pawn, Pawn>(rect2, pawn, getPayload, menuGenerator, buttonLabel, null, dragLabel, null, null, paintable);
		}

		private static Pawn MasterSelectButton_GetMaster(Pawn pet)
		{
			return pet.playerSettings.Master;
		}

		private static IEnumerable<Widgets.DropdownMenuElement<Pawn>> MasterSelectButton_GenerateMenu(Pawn p)
		{
			yield return new Widgets.DropdownMenuElement<Pawn>
			{
				option = new FloatMenuOption("(" + "NoneLower".Translate() + ")", delegate()
				{
					p.playerSettings.Master = null;
				}, MenuOptionPriority.Default, null, null, 0f, null, null),
				payload = null
			};
			using (IEnumerator<Pawn> enumerator = PawnsFinder.AllMaps_FreeColonistsSpawned.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pawn col = enumerator.Current;
					string lab = RelationsUtility.LabelWithBondInfo(col, p);
					Action action = null;
					int level = col.skills.GetSkill(SkillDefOf.Animals).Level;
					int minLevel = TrainableUtility.MinimumHandlingSkill(p);
					if (level < minLevel)
					{
						action = null;
						lab = lab + " (" + "SkillTooLow".Translate(new object[]
						{
							SkillDefOf.Animals.LabelCap,
							level,
							minLevel
						}) + ")";
					}
					else if (TrainableUtility.CanBeMaster(col, p, true))
					{
						action = delegate()
						{
							p.playerSettings.Master = col;
						};
					}
					yield return new Widgets.DropdownMenuElement<Pawn>
					{
						option = new FloatMenuOption(lab, action, MenuOptionPriority.Default, null, null, 0f, null, null),
						payload = col
					};
				}
			}
			yield break;
		}

		public static bool CanBeMaster(Pawn master, Pawn animal, bool checkSpawned = true)
		{
			bool result;
			if ((checkSpawned && !master.Spawned) || master.IsPrisoner)
			{
				result = false;
			}
			else
			{
				int level = master.skills.GetSkill(SkillDefOf.Animals).Level;
				int num = TrainableUtility.MinimumHandlingSkill(animal);
				result = (level >= num);
			}
			return result;
		}

		public static string GetIconTooltipText(Pawn pawn)
		{
			string text = "";
			if (pawn.playerSettings.Master != null)
			{
				text += string.Format("{0}: {1}\n", "Master".Translate(), pawn.playerSettings.Master.LabelShort);
			}
			IEnumerable<Pawn> allColonistBondsFor = TrainableUtility.GetAllColonistBondsFor(pawn);
			if (allColonistBondsFor.Any<Pawn>())
			{
				text += string.Format("{0}: {1}\n", "BondedTo".Translate(), (from bond in allColonistBondsFor
				select bond.LabelShort).ToCommaList(true));
			}
			return text;
		}

		public static IEnumerable<Pawn> GetAllColonistBondsFor(Pawn pet)
		{
			return from bond in pet.relations.DirectRelations
			where bond.def == PawnRelationDefOf.Bond && bond.otherPawn != null && bond.otherPawn.IsColonistPlayerControlled
			select bond.otherPawn;
		}

		public static int DegradationPeriodTicks(ThingDef def)
		{
			return Mathf.RoundToInt(TrainableUtility.DecayIntervalDaysFromWildnessCurve.Evaluate(def.race.wildness) * 60000f);
		}

		public static bool TamenessCanDecay(ThingDef def)
		{
			return def.race.wildness > 0.101f;
		}

		public static string GetWildnessExplanation(ThingDef def)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("WildnessExplanation".Translate());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(string.Format("{0}: {1}", "TrainingDecayInterval".Translate(), TrainableUtility.DegradationPeriodTicks(def).ToStringTicksToDays("F1")));
			if (!TrainableUtility.TamenessCanDecay(def))
			{
				stringBuilder.AppendLine("TamenessWillNotDecay".Translate());
			}
			return stringBuilder.ToString();
		}

		// Note: this type is marked as 'beforefieldinit'.
		static TrainableUtility()
		{
		}

		[CompilerGenerated]
		private static float <Reset>m__0(TrainableDef td)
		{
			return td.listPriority;
		}

		[CompilerGenerated]
		private static string <GetIconTooltipText>m__1(Pawn bond)
		{
			return bond.LabelShort;
		}

		[CompilerGenerated]
		private static bool <GetAllColonistBondsFor>m__2(DirectPawnRelation bond)
		{
			return bond.def == PawnRelationDefOf.Bond && bond.otherPawn != null && bond.otherPawn.IsColonistPlayerControlled;
		}

		[CompilerGenerated]
		private static Pawn <GetAllColonistBondsFor>m__3(DirectPawnRelation bond)
		{
			return bond.otherPawn;
		}

		[CompilerGenerated]
		private sealed class <MasterSelectButton_GenerateMenu>c__Iterator0 : IEnumerable, IEnumerable<Widgets.DropdownMenuElement<Pawn>>, IEnumerator, IDisposable, IEnumerator<Widgets.DropdownMenuElement<Pawn>>
		{
			internal Pawn p;

			internal IEnumerator<Pawn> $locvar0;

			internal string <lab>__2;

			internal Action <action>__2;

			internal int <level>__2;

			internal int <minLevel>__2;

			internal Widgets.DropdownMenuElement<Pawn> $current;

			internal bool $disposing;

			internal int $PC;

			private TrainableUtility.<MasterSelectButton_GenerateMenu>c__Iterator0.<MasterSelectButton_GenerateMenu>c__AnonStorey1 $locvar1;

			private TrainableUtility.<MasterSelectButton_GenerateMenu>c__Iterator0.<MasterSelectButton_GenerateMenu>c__AnonStorey2 $locvar2;

			[DebuggerHidden]
			public <MasterSelectButton_GenerateMenu>c__Iterator0()
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
					this.$current = new Widgets.DropdownMenuElement<Pawn>
					{
						option = new FloatMenuOption("(" + "NoneLower".Translate() + ")", delegate()
						{
							p.playerSettings.Master = null;
						}, MenuOptionPriority.Default, null, null, 0f, null, null),
						payload = null
					};
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					enumerator = PawnsFinder.AllMaps_FreeColonistsSpawned.GetEnumerator();
					num = 4294967293u;
					break;
				case 2u:
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
						Pawn col = enumerator.Current;
						lab = RelationsUtility.LabelWithBondInfo(col, <MasterSelectButton_GenerateMenu>c__AnonStorey.p);
						action = null;
						level = col.skills.GetSkill(SkillDefOf.Animals).Level;
						minLevel = TrainableUtility.MinimumHandlingSkill(<MasterSelectButton_GenerateMenu>c__AnonStorey.p);
						if (level < minLevel)
						{
							action = null;
							lab = lab + " (" + "SkillTooLow".Translate(new object[]
							{
								SkillDefOf.Animals.LabelCap,
								level,
								minLevel
							}) + ")";
						}
						else if (TrainableUtility.CanBeMaster(col, <MasterSelectButton_GenerateMenu>c__AnonStorey.p, true))
						{
							action = delegate()
							{
								<MasterSelectButton_GenerateMenu>c__AnonStorey.p.playerSettings.Master = col;
							};
						}
						this.$current = new Widgets.DropdownMenuElement<Pawn>
						{
							option = new FloatMenuOption(lab, action, MenuOptionPriority.Default, null, null, 0f, null, null),
							payload = col
						};
						if (!this.$disposing)
						{
							this.$PC = 2;
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

			Widgets.DropdownMenuElement<Pawn> IEnumerator<Widgets.DropdownMenuElement<Pawn>>.Current
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
				case 2u:
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
				return this.System.Collections.Generic.IEnumerable<Verse.Widgets.DropdownMenuElement<Verse.Pawn>>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Widgets.DropdownMenuElement<Pawn>> IEnumerable<Widgets.DropdownMenuElement<Pawn>>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				TrainableUtility.<MasterSelectButton_GenerateMenu>c__Iterator0 <MasterSelectButton_GenerateMenu>c__Iterator = new TrainableUtility.<MasterSelectButton_GenerateMenu>c__Iterator0();
				<MasterSelectButton_GenerateMenu>c__Iterator.p = p;
				return <MasterSelectButton_GenerateMenu>c__Iterator;
			}

			private sealed class <MasterSelectButton_GenerateMenu>c__AnonStorey1
			{
				internal Pawn p;

				public <MasterSelectButton_GenerateMenu>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					this.p.playerSettings.Master = null;
				}
			}

			private sealed class <MasterSelectButton_GenerateMenu>c__AnonStorey2
			{
				internal Pawn col;

				internal TrainableUtility.<MasterSelectButton_GenerateMenu>c__Iterator0.<MasterSelectButton_GenerateMenu>c__AnonStorey1 <>f__ref$1;

				public <MasterSelectButton_GenerateMenu>c__AnonStorey2()
				{
				}

				internal void <>m__0()
				{
					this.<>f__ref$1.p.playerSettings.Master = this.col;
				}
			}
		}
	}
}
