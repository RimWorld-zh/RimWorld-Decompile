using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;

namespace Verse
{
	public class RecipeWorker
	{
		public RecipeDef recipe;

		public RecipeWorker()
		{
		}

		public virtual IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			yield break;
		}

		public virtual void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
		}

		public virtual bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			return pawn.Faction != billDoerFaction && this.recipe.isViolation;
		}

		public virtual string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			return this.recipe.label;
		}

		public virtual void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
			ingredient.Destroy(DestroyMode.Vanish);
		}

		[CompilerGenerated]
		private sealed class <GetPartsToApplyOn>c__Iterator0 : IEnumerable, IEnumerable<BodyPartRecord>, IEnumerator, IDisposable, IEnumerator<BodyPartRecord>
		{
			internal BodyPartRecord $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetPartsToApplyOn>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				bool flag = this.$PC != 0;
				this.$PC = -1;
				if (!flag)
				{
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
				return new RecipeWorker.<GetPartsToApplyOn>c__Iterator0();
			}
		}
	}
}
