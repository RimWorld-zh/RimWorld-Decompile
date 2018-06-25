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
	public class Recipe_InstallArtificialBodyPart : Recipe_Surgery
	{
		public Recipe_InstallArtificialBodyPart()
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
						IEnumerable<Hediff> diffs = from x in pawn.health.hediffSet.hediffs
						where x.Part == record
						select x;
						if (diffs.Count<Hediff>() != 1 || diffs.First<Hediff>().def != recipe.addsHediff)
						{
							if (record.parent == null || pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null).Contains(record.parent))
							{
								if (!pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(record) || pawn.health.hediffSet.HasDirectlyAddedPartFor(record))
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
				MedicalRecipesUtility.RestorePartAndSpawnAllPreviousParts(pawn, part, billDoer.Position, billDoer.Map);
			}
			else if (pawn.Map != null)
			{
				MedicalRecipesUtility.RestorePartAndSpawnAllPreviousParts(pawn, part, pawn.Position, pawn.Map);
			}
			else
			{
				pawn.health.RestorePart(part, null, true);
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

			internal IEnumerable<Hediff> <diffs>__4;

			internal BodyPartRecord $current;

			internal bool $disposing;

			internal int $PC;

			private Recipe_InstallArtificialBodyPart.<GetPartsToApplyOn>c__Iterator0.<GetPartsToApplyOn>c__AnonStorey1 $locvar0;

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
					goto IL_221;
				case 1u:
					break;
				default:
					return false;
				}
				IL_1EE:
				j++;
				IL_1FC:
				if (j >= bpList.Count)
				{
					i++;
				}
				else
				{
					BodyPartRecord record = bpList[j];
					if (record.def != part)
					{
						goto IL_1EE;
					}
					diffs = from x in pawn.health.hediffSet.hediffs
					where x.Part == record
					select x;
					if (diffs.Count<Hediff>() == 1 && diffs.First<Hediff>().def == recipe.addsHediff)
					{
						goto IL_1EE;
					}
					if (record.parent != null && !pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null).Contains(record.parent))
					{
						goto IL_1EE;
					}
					if (pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(record) && !pawn.health.hediffSet.HasDirectlyAddedPartFor(record))
					{
						goto IL_1EE;
					}
					this.$current = record;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				IL_221:
				if (i < recipe.appliedOnFixedBodyParts.Count)
				{
					part = recipe.appliedOnFixedBodyParts[i];
					bpList = pawn.RaceProps.body.AllParts;
					j = 0;
					goto IL_1FC;
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
				Recipe_InstallArtificialBodyPart.<GetPartsToApplyOn>c__Iterator0 <GetPartsToApplyOn>c__Iterator = new Recipe_InstallArtificialBodyPart.<GetPartsToApplyOn>c__Iterator0();
				<GetPartsToApplyOn>c__Iterator.recipe = recipe;
				<GetPartsToApplyOn>c__Iterator.pawn = pawn;
				return <GetPartsToApplyOn>c__Iterator;
			}

			private sealed class <GetPartsToApplyOn>c__AnonStorey1
			{
				internal BodyPartRecord record;

				internal Recipe_InstallArtificialBodyPart.<GetPartsToApplyOn>c__Iterator0 <>f__ref$0;

				public <GetPartsToApplyOn>c__AnonStorey1()
				{
				}

				internal bool <>m__0(Hediff x)
				{
					return x.Part == this.record;
				}
			}
		}
	}
}
