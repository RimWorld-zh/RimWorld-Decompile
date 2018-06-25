using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public class Recipe_RemoveHediff : Recipe_Surgery
	{
		public Recipe_RemoveHediff()
		{
		}

		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			List<Hediff> allHediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < allHediffs.Count; i++)
			{
				if (allHediffs[i].Part != null)
				{
					if (allHediffs[i].def == recipe.removesHediff)
					{
						if (allHediffs[i].Visible)
						{
							yield return allHediffs[i].Part;
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
				if (PawnUtility.ShouldSendNotificationAbout(pawn) || PawnUtility.ShouldSendNotificationAbout(billDoer))
				{
					string text;
					if (!this.recipe.successfullyRemovedHediffMessage.NullOrEmpty())
					{
						text = string.Format(this.recipe.successfullyRemovedHediffMessage, billDoer.LabelShort, pawn.LabelShort);
					}
					else
					{
						text = "MessageSuccessfullyRemovedHediff".Translate(new object[]
						{
							billDoer.LabelShort,
							pawn.LabelShort,
							this.recipe.removesHediff.label
						});
					}
					Messages.Message(text, pawn, MessageTypeDefOf.PositiveEvent, true);
				}
			}
			Hediff hediff = pawn.health.hediffSet.hediffs.Find((Hediff x) => x.def == this.recipe.removesHediff && x.Part == part && x.Visible);
			if (hediff != null)
			{
				pawn.health.RemoveHediff(hediff);
			}
		}

		[CompilerGenerated]
		private sealed class <GetPartsToApplyOn>c__Iterator0 : IEnumerable, IEnumerable<BodyPartRecord>, IEnumerator, IDisposable, IEnumerator<BodyPartRecord>
		{
			internal Pawn pawn;

			internal List<Hediff> <allHediffs>__0;

			internal int <i>__1;

			internal RecipeDef recipe;

			internal BodyPartRecord $current;

			internal bool $disposing;

			internal int $PC;

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
					allHediffs = pawn.health.hediffSet.hediffs;
					i = 0;
					goto IL_F4;
				case 1u:
					break;
				default:
					return false;
				}
				IL_E6:
				i++;
				IL_F4:
				if (i >= allHediffs.Count)
				{
					this.$PC = -1;
				}
				else
				{
					if (allHediffs[i].Part == null)
					{
						goto IL_E6;
					}
					if (allHediffs[i].def != recipe.removesHediff)
					{
						goto IL_E6;
					}
					if (!allHediffs[i].Visible)
					{
						goto IL_E6;
					}
					this.$current = allHediffs[i].Part;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
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
				Recipe_RemoveHediff.<GetPartsToApplyOn>c__Iterator0 <GetPartsToApplyOn>c__Iterator = new Recipe_RemoveHediff.<GetPartsToApplyOn>c__Iterator0();
				<GetPartsToApplyOn>c__Iterator.pawn = pawn;
				<GetPartsToApplyOn>c__Iterator.recipe = recipe;
				return <GetPartsToApplyOn>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <ApplyOnPawn>c__AnonStorey1
		{
			internal BodyPartRecord part;

			internal Recipe_RemoveHediff $this;

			public <ApplyOnPawn>c__AnonStorey1()
			{
			}

			internal bool <>m__0(Hediff x)
			{
				return x.def == this.$this.recipe.removesHediff && x.Part == this.part && x.Visible;
			}
		}
	}
}
