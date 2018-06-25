using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class Recipe_InstallImplant : Recipe_Surgery
	{
		public Recipe_InstallImplant()
		{
		}

		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			for (int i = 0; i < recipe.appliedOnFixedBodyParts.Count; i++)
			{
				BodyPartDef part = recipe.appliedOnFixedBodyParts[i];
				List<BodyPartRecord> bpList = pawn.RaceProps.body.AllParts;
				for (int j = 0; j < bpList.Count; j++)
				{
					BodyPartRecord record = bpList[j];
					if (record.def == part)
					{
						if (pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null).Contains(record))
						{
							if (!pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(record))
							{
								if (!pawn.health.hediffSet.hediffs.Any((Hediff x) => x.Part == record && x.def == recipe.addsHediff))
								{
									yield return record;
								}
							}
						}
					}
				}
			}
			yield break;
		}

		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (billDoer != null)
			{
				if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
				{
					return;
				}
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
				{
					billDoer,
					pawn
				});
			}
			pawn.health.AddHediff(this.recipe.addsHediff, part, null, null);
		}

		[CompilerGenerated]
		private sealed class <GetPartsToApplyOn>c__Iterator0 : IEnumerable, IEnumerable<BodyPartRecord>, IEnumerator, IDisposable, IEnumerator<BodyPartRecord>
		{
			internal int <i>__1;

			internal RecipeDef recipe;

			internal BodyPartDef <part>__2;

			internal Pawn pawn;

			internal List<BodyPartRecord> <bpList>__2;

			internal int <j>__3;

			internal BodyPartRecord $current;

			internal bool $disposing;

			internal int $PC;

			private Recipe_InstallImplant.<GetPartsToApplyOn>c__Iterator0.<GetPartsToApplyOn>c__AnonStorey2 $locvar0;

			private Recipe_InstallImplant.<GetPartsToApplyOn>c__Iterator0.<GetPartsToApplyOn>c__AnonStorey1 $locvar1;

			[DebuggerHidden]
			public <GetPartsToApplyOn>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					i = 0;
					goto IL_1EE;
				case 1u:
					break;
				default:
					return false;
				}
				IL_1BB:
				j++;
				IL_1C9:
				if (j >= bpList.Count)
				{
					i++;
				}
				else
				{
					BodyPartRecord record = bpList[j];
					if (record.def != part)
					{
						goto IL_1BB;
					}
					if (!pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null).Contains(record))
					{
						goto IL_1BB;
					}
					if (pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(record))
					{
						goto IL_1BB;
					}
					if (pawn.health.hediffSet.hediffs.Any((Hediff x) => x.Part == record && x.def == <GetPartsToApplyOn>c__AnonStorey.recipe.addsHediff))
					{
						goto IL_1BB;
					}
					this.$current = record;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				IL_1EE:
				if (i < <GetPartsToApplyOn>c__AnonStorey.recipe.appliedOnFixedBodyParts.Count)
				{
					part = <GetPartsToApplyOn>c__AnonStorey.recipe.appliedOnFixedBodyParts[i];
					bpList = pawn.RaceProps.body.AllParts;
					j = 0;
					goto IL_1C9;
				}
				this.$PC = -1;
				return false;
			}

			BodyPartRecord IEnumerator<BodyPartRecord>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.BodyPartRecord>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<BodyPartRecord> IEnumerable<BodyPartRecord>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Recipe_InstallImplant.<GetPartsToApplyOn>c__Iterator0 <GetPartsToApplyOn>c__Iterator = new Recipe_InstallImplant.<GetPartsToApplyOn>c__Iterator0();
				<GetPartsToApplyOn>c__Iterator.recipe = recipe;
				<GetPartsToApplyOn>c__Iterator.pawn = pawn;
				return <GetPartsToApplyOn>c__Iterator;
			}

			private sealed class <GetPartsToApplyOn>c__AnonStorey2
			{
				internal RecipeDef recipe;

				internal Recipe_InstallImplant.<GetPartsToApplyOn>c__Iterator0 <>f__ref$0;

				public <GetPartsToApplyOn>c__AnonStorey2()
				{
				}
			}

			private sealed class <GetPartsToApplyOn>c__AnonStorey1
			{
				internal BodyPartRecord record;

				internal Recipe_InstallImplant.<GetPartsToApplyOn>c__Iterator0 <>f__ref$0;

				internal Recipe_InstallImplant.<GetPartsToApplyOn>c__Iterator0.<GetPartsToApplyOn>c__AnonStorey2 <>f__ref$2;

				public <GetPartsToApplyOn>c__AnonStorey1()
				{
				}

				internal bool <>m__0(Hediff x)
				{
					return x.Part == this.record && x.def == this.<>f__ref$2.recipe.addsHediff;
				}
			}
		}
	}
}
