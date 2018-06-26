using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	internal class Recipe_RemoveBodyPart : Recipe_Surgery
	{
		public Recipe_RemoveBodyPart()
		{
		}

		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			IEnumerable<BodyPartRecord> parts = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null);
			using (IEnumerator<BodyPartRecord> enumerator = parts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BodyPartRecord part = enumerator.Current;
					if (pawn.health.hediffSet.HasDirectlyAddedPartFor(part))
					{
						yield return part;
					}
					else if (MedicalRecipesUtility.IsCleanAndDroppable(pawn, part))
					{
						yield return part;
					}
					else if (part != pawn.RaceProps.body.corePart && part.def.canSuggestAmputation && pawn.health.hediffSet.hediffs.Any((Hediff d) => !(d is Hediff_Injury) && d.def.isBad && d.Visible && d.Part == part))
					{
						yield return part;
					}
				}
			}
			yield break;
		}

		public override bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			return pawn.Faction != billDoerFaction && pawn.Faction != null && HealthUtility.PartRemovalIntent(pawn, part) == BodyPartRemovalIntent.Harvest;
		}

		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			bool flag = MedicalRecipesUtility.IsClean(pawn, part);
			bool flag2 = this.IsViolationOnPawn(pawn, part, Faction.OfPlayer);
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
				MedicalRecipesUtility.SpawnNaturalPartIfClean(pawn, part, billDoer.Position, billDoer.Map);
				MedicalRecipesUtility.SpawnThingsFromHediffs(pawn, part, billDoer.Position, billDoer.Map);
			}
			DamageDef surgicalCut = DamageDefOf.SurgicalCut;
			float amount = 99999f;
			float armorPenetration = 999f;
			pawn.TakeDamage(new DamageInfo(surgicalCut, amount, armorPenetration, -1f, null, part, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			if (flag)
			{
				if (pawn.Dead)
				{
					ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, PawnExecutionKind.OrganHarvesting);
				}
				ThoughtUtility.GiveThoughtsForPawnOrganHarvested(pawn);
			}
			if (flag2 && pawn.Faction != null && billDoer.Faction != null)
			{
				Faction faction = pawn.Faction;
				Faction faction2 = billDoer.Faction;
				int goodwillChange = -15;
				string reason = "GoodwillChangedReason_RemovedBodyPart".Translate(new object[]
				{
					part.LabelShort
				});
				GlobalTargetInfo? lookTarget = new GlobalTargetInfo?(pawn);
				faction.TryAffectGoodwillWith(faction2, goodwillChange, true, true, reason, lookTarget);
			}
		}

		public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			string result;
			if (pawn.RaceProps.IsMechanoid || pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(part))
			{
				result = RecipeDefOf.RemoveBodyPart.label;
			}
			else
			{
				BodyPartRemovalIntent bodyPartRemovalIntent = HealthUtility.PartRemovalIntent(pawn, part);
				if (bodyPartRemovalIntent != BodyPartRemovalIntent.Amputate)
				{
					if (bodyPartRemovalIntent != BodyPartRemovalIntent.Harvest)
					{
						throw new InvalidOperationException();
					}
					result = "HarvestOrgan".Translate();
				}
				else if (part.depth == BodyPartDepth.Inside || part.def.socketed)
				{
					result = "RemoveOrgan".Translate();
				}
				else
				{
					result = "Amputate".Translate();
				}
			}
			return result;
		}

		[CompilerGenerated]
		private sealed class <GetPartsToApplyOn>c__Iterator0 : IEnumerable, IEnumerable<BodyPartRecord>, IEnumerator, IDisposable, IEnumerator<BodyPartRecord>
		{
			internal Pawn pawn;

			internal IEnumerable<BodyPartRecord> <parts>__0;

			internal IEnumerator<BodyPartRecord> $locvar0;

			internal BodyPartRecord $current;

			internal bool $disposing;

			internal int $PC;

			private Recipe_RemoveBodyPart.<GetPartsToApplyOn>c__Iterator0.<GetPartsToApplyOn>c__AnonStorey1 $locvar1;

			[DebuggerHidden]
			public <GetPartsToApplyOn>c__Iterator0()
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
					parts = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null);
					enumerator = parts.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
				case 2u:
				case 3u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					IL_1DA:
					if (enumerator.MoveNext())
					{
						BodyPartRecord part = enumerator.Current;
						if (pawn.health.hediffSet.HasDirectlyAddedPartFor(part))
						{
							this.$current = part;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
						if (MedicalRecipesUtility.IsCleanAndDroppable(pawn, part))
						{
							this.$current = part;
							if (!this.$disposing)
							{
								this.$PC = 2;
							}
							flag = true;
							return true;
						}
						if (part != pawn.RaceProps.body.corePart && part.def.canSuggestAmputation && pawn.health.hediffSet.hediffs.Any((Hediff d) => !(d is Hediff_Injury) && d.def.isBad && d.Visible && d.Part == part))
						{
							this.$current = part;
							if (!this.$disposing)
							{
								this.$PC = 3;
							}
							flag = true;
							return true;
						}
						goto IL_1DA;
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
				case 2u:
				case 3u:
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
				return this.System.Collections.Generic.IEnumerable<Verse.BodyPartRecord>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<BodyPartRecord> IEnumerable<BodyPartRecord>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Recipe_RemoveBodyPart.<GetPartsToApplyOn>c__Iterator0 <GetPartsToApplyOn>c__Iterator = new Recipe_RemoveBodyPart.<GetPartsToApplyOn>c__Iterator0();
				<GetPartsToApplyOn>c__Iterator.pawn = pawn;
				return <GetPartsToApplyOn>c__Iterator;
			}

			private sealed class <GetPartsToApplyOn>c__AnonStorey1
			{
				internal BodyPartRecord part;

				internal Recipe_RemoveBodyPart.<GetPartsToApplyOn>c__Iterator0 <>f__ref$0;

				public <GetPartsToApplyOn>c__AnonStorey1()
				{
				}

				internal bool <>m__0(Hediff d)
				{
					return !(d is Hediff_Injury) && d.def.isBad && d.Visible && d.Part == this.part;
				}
			}
		}
	}
}
