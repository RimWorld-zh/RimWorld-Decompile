using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200052D RID: 1325
	public class Pawn_RelationsTracker : IExposable
	{
		// Token: 0x06001843 RID: 6211 RVA: 0x000D3904 File Offset: 0x000D1D04
		public Pawn_RelationsTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06001844 RID: 6212 RVA: 0x000D393C File Offset: 0x000D1D3C
		public List<DirectPawnRelation> DirectRelations
		{
			get
			{
				return this.directRelations;
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06001845 RID: 6213 RVA: 0x000D3958 File Offset: 0x000D1D58
		public IEnumerable<Pawn> Children
		{
			get
			{
				foreach (Pawn p in this.pawnsWithDirectRelationsWithMe)
				{
					List<DirectPawnRelation> hisDirectRels = p.relations.directRelations;
					for (int i = 0; i < hisDirectRels.Count; i++)
					{
						if (hisDirectRels[i].otherPawn == this.pawn && hisDirectRels[i].def == PawnRelationDefOf.Parent)
						{
							yield return p;
							break;
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06001846 RID: 6214 RVA: 0x000D3984 File Offset: 0x000D1D84
		public int ChildrenCount
		{
			get
			{
				return this.Children.Count<Pawn>();
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06001847 RID: 6215 RVA: 0x000D39A4 File Offset: 0x000D1DA4
		public bool RelatedToAnyoneOrAnyoneRelatedToMe
		{
			get
			{
				return this.directRelations.Any<DirectPawnRelation>() || this.pawnsWithDirectRelationsWithMe.Any<Pawn>();
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06001848 RID: 6216 RVA: 0x000D39D8 File Offset: 0x000D1DD8
		public IEnumerable<Pawn> FamilyByBlood
		{
			get
			{
				IEnumerable<Pawn> familyByBlood_Internal;
				if (this.canCacheFamilyByBlood)
				{
					if (!this.familyByBloodIsCached)
					{
						this.cachedFamilyByBlood.Clear();
						this.cachedFamilyByBlood.AddRange(this.FamilyByBlood_Internal);
						this.familyByBloodIsCached = true;
					}
					familyByBlood_Internal = this.cachedFamilyByBlood;
				}
				else
				{
					familyByBlood_Internal = this.FamilyByBlood_Internal;
				}
				return familyByBlood_Internal;
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06001849 RID: 6217 RVA: 0x000D3A3C File Offset: 0x000D1E3C
		private IEnumerable<Pawn> FamilyByBlood_Internal
		{
			get
			{
				if (!this.RelatedToAnyoneOrAnyoneRelatedToMe)
				{
					yield break;
				}
				List<Pawn> familyStack = null;
				List<Pawn> familyChildrenStack = null;
				HashSet<Pawn> familyVisited = null;
				try
				{
					familyStack = SimplePool<List<Pawn>>.Get();
					familyChildrenStack = SimplePool<List<Pawn>>.Get();
					familyVisited = SimplePool<HashSet<Pawn>>.Get();
					familyStack.Add(this.pawn);
					familyVisited.Add(this.pawn);
					while (familyStack.Any<Pawn>())
					{
						Pawn p = familyStack[familyStack.Count - 1];
						familyStack.RemoveLast<Pawn>();
						if (p != this.pawn)
						{
							yield return p;
						}
						Pawn father = p.GetFather();
						if (father != null && !familyVisited.Contains(father))
						{
							familyStack.Add(father);
							familyVisited.Add(father);
						}
						Pawn mother = p.GetMother();
						if (mother != null && !familyVisited.Contains(mother))
						{
							familyStack.Add(mother);
							familyVisited.Add(mother);
						}
						familyChildrenStack.Clear();
						familyChildrenStack.Add(p);
						while (familyChildrenStack.Any<Pawn>())
						{
							Pawn child = familyChildrenStack[familyChildrenStack.Count - 1];
							familyChildrenStack.RemoveLast<Pawn>();
							if (child != p && child != this.pawn)
							{
								yield return child;
							}
							IEnumerable<Pawn> children = child.relations.Children;
							foreach (Pawn item in children)
							{
								if (!familyVisited.Contains(item))
								{
									familyChildrenStack.Add(item);
									familyVisited.Add(item);
								}
							}
						}
					}
				}
				finally
				{
					familyStack.Clear();
					SimplePool<List<Pawn>>.Return(familyStack);
					familyChildrenStack.Clear();
					SimplePool<List<Pawn>>.Return(familyChildrenStack);
					familyVisited.Clear();
					SimplePool<HashSet<Pawn>>.Return(familyVisited);
				}
				yield break;
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x0600184A RID: 6218 RVA: 0x000D3A68 File Offset: 0x000D1E68
		public IEnumerable<Pawn> PotentiallyRelatedPawns
		{
			get
			{
				if (!this.RelatedToAnyoneOrAnyoneRelatedToMe)
				{
					yield break;
				}
				List<Pawn> stack = null;
				HashSet<Pawn> visited = null;
				try
				{
					stack = SimplePool<List<Pawn>>.Get();
					visited = SimplePool<HashSet<Pawn>>.Get();
					stack.Add(this.pawn);
					visited.Add(this.pawn);
					while (stack.Any<Pawn>())
					{
						Pawn p = stack[stack.Count - 1];
						stack.RemoveLast<Pawn>();
						if (p != this.pawn)
						{
							yield return p;
						}
						for (int i = 0; i < p.relations.directRelations.Count; i++)
						{
							Pawn otherPawn = p.relations.directRelations[i].otherPawn;
							if (!visited.Contains(otherPawn))
							{
								stack.Add(otherPawn);
								visited.Add(otherPawn);
							}
						}
						foreach (Pawn item in p.relations.pawnsWithDirectRelationsWithMe)
						{
							if (!visited.Contains(item))
							{
								stack.Add(item);
								visited.Add(item);
							}
						}
					}
				}
				finally
				{
					stack.Clear();
					SimplePool<List<Pawn>>.Return(stack);
					visited.Clear();
					SimplePool<HashSet<Pawn>>.Return(visited);
				}
				yield break;
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x0600184B RID: 6219 RVA: 0x000D3A94 File Offset: 0x000D1E94
		public IEnumerable<Pawn> RelatedPawns
		{
			get
			{
				this.canCacheFamilyByBlood = true;
				this.familyByBloodIsCached = false;
				this.cachedFamilyByBlood.Clear();
				try
				{
					foreach (Pawn p in this.PotentiallyRelatedPawns)
					{
						if ((this.familyByBloodIsCached && this.cachedFamilyByBlood.Contains(p)) || this.pawn.GetRelations(p).Any<PawnRelationDef>())
						{
							yield return p;
						}
					}
				}
				finally
				{
					this.canCacheFamilyByBlood = false;
					this.familyByBloodIsCached = false;
					this.cachedFamilyByBlood.Clear();
				}
				yield break;
			}
		}

		// Token: 0x0600184C RID: 6220 RVA: 0x000D3AC0 File Offset: 0x000D1EC0
		public void ExposeData()
		{
			Scribe_Collections.Look<DirectPawnRelation>(ref this.directRelations, "directRelations", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = 0; i < this.directRelations.Count; i++)
				{
					if (this.directRelations[i].otherPawn == null)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Pawn ",
							this.pawn,
							" has relation \"",
							this.directRelations[i].def.defName,
							"\" with null pawn after loading. This means that we forgot to serialize pawns somewhere (e.g. pawns from passing trade ships)."
						}), false);
					}
				}
				this.directRelations.RemoveAll((DirectPawnRelation x) => x.otherPawn == null);
				for (int j = 0; j < this.directRelations.Count; j++)
				{
					this.directRelations[j].otherPawn.relations.pawnsWithDirectRelationsWithMe.Add(this.pawn);
				}
			}
			Scribe_Values.Look<bool>(ref this.everSeenByPlayer, "everSeenByPlayer", true, false);
			Scribe_Values.Look<bool>(ref this.canGetRescuedThought, "canGetRescuedThought", true, false);
			Scribe_References.Look<Pawn>(ref this.relativeInvolvedInRescueQuest, "relativeInvolvedInRescueQuest", false);
		}

		// Token: 0x0600184D RID: 6221 RVA: 0x000D3C14 File Offset: 0x000D2014
		public void RelationsTrackerTick()
		{
			if (!this.pawn.Dead)
			{
				this.Tick_CheckStartMarriageCeremony();
				this.Tick_CheckDevelopBondRelation();
			}
		}

		// Token: 0x0600184E RID: 6222 RVA: 0x000D3C38 File Offset: 0x000D2038
		public DirectPawnRelation GetDirectRelation(PawnRelationDef def, Pawn otherPawn)
		{
			DirectPawnRelation result;
			if (def.implied)
			{
				Log.Warning(def + " is not a direct relation.", false);
				result = null;
			}
			else
			{
				result = this.directRelations.Find((DirectPawnRelation x) => x.def == def && x.otherPawn == otherPawn);
			}
			return result;
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x000D3CA8 File Offset: 0x000D20A8
		public Pawn GetFirstDirectRelationPawn(PawnRelationDef def, Predicate<Pawn> predicate = null)
		{
			Pawn result;
			if (def.implied)
			{
				Log.Warning(def + " is not a direct relation.", false);
				result = null;
			}
			else
			{
				for (int i = 0; i < this.directRelations.Count; i++)
				{
					DirectPawnRelation directPawnRelation = this.directRelations[i];
					if (directPawnRelation.def == def && (predicate == null || predicate(directPawnRelation.otherPawn)))
					{
						return directPawnRelation.otherPawn;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x000D3D3C File Offset: 0x000D213C
		public bool DirectRelationExists(PawnRelationDef def, Pawn otherPawn)
		{
			bool result;
			if (def.implied)
			{
				Log.Warning(def + " is not a direct relation.", false);
				result = false;
			}
			else
			{
				for (int i = 0; i < this.directRelations.Count; i++)
				{
					DirectPawnRelation directPawnRelation = this.directRelations[i];
					if (directPawnRelation.def == def && directPawnRelation.otherPawn == otherPawn)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x000D3DC0 File Offset: 0x000D21C0
		public void AddDirectRelation(PawnRelationDef def, Pawn otherPawn)
		{
			if (def.implied)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to directly add implied pawn relation ",
					def,
					", pawn=",
					this.pawn,
					", otherPawn=",
					otherPawn
				}), false);
			}
			else if (otherPawn == this.pawn)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to add pawn relation ",
					def,
					" with self, pawn=",
					this.pawn
				}), false);
			}
			else if (this.DirectRelationExists(def, otherPawn))
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to add the same relation twice: ",
					def,
					", pawn=",
					this.pawn,
					", otherPawn=",
					otherPawn
				}), false);
			}
			else
			{
				int startTicks = (Current.ProgramState != ProgramState.Playing) ? 0 : Find.TickManager.TicksGame;
				this.directRelations.Add(new DirectPawnRelation(def, otherPawn, startTicks));
				otherPawn.relations.pawnsWithDirectRelationsWithMe.Add(this.pawn);
				if (def.reflexive)
				{
					otherPawn.relations.directRelations.Add(new DirectPawnRelation(def, this.pawn, startTicks));
					this.pawnsWithDirectRelationsWithMe.Add(otherPawn);
				}
				this.GainedOrLostDirectRelation();
				otherPawn.relations.GainedOrLostDirectRelation();
			}
		}

		// Token: 0x06001852 RID: 6226 RVA: 0x000D3F34 File Offset: 0x000D2334
		public void RemoveDirectRelation(DirectPawnRelation relation)
		{
			this.RemoveDirectRelation(relation.def, relation.otherPawn);
		}

		// Token: 0x06001853 RID: 6227 RVA: 0x000D3F4C File Offset: 0x000D234C
		public void RemoveDirectRelation(PawnRelationDef def, Pawn otherPawn)
		{
			if (!this.TryRemoveDirectRelation(def, otherPawn))
			{
				Log.Warning(string.Concat(new object[]
				{
					"Could not remove relation ",
					def,
					" because it's not here. pawn=",
					this.pawn,
					", otherPawn=",
					otherPawn
				}), false);
			}
		}

		// Token: 0x06001854 RID: 6228 RVA: 0x000D3FA4 File Offset: 0x000D23A4
		public bool TryRemoveDirectRelation(PawnRelationDef def, Pawn otherPawn)
		{
			bool result;
			if (def.implied)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to remove implied pawn relation ",
					def,
					", pawn=",
					this.pawn,
					", otherPawn=",
					otherPawn
				}), false);
				result = false;
			}
			else
			{
				for (int i = 0; i < this.directRelations.Count; i++)
				{
					if (this.directRelations[i].def == def && this.directRelations[i].otherPawn == otherPawn)
					{
						if (def.reflexive)
						{
							List<DirectPawnRelation> list = otherPawn.relations.directRelations;
							DirectPawnRelation item = list.Find((DirectPawnRelation x) => x.def == def && x.otherPawn == this.pawn);
							list.Remove(item);
							if (list.Find((DirectPawnRelation x) => x.otherPawn == this.pawn) == null)
							{
								this.pawnsWithDirectRelationsWithMe.Remove(otherPawn);
							}
						}
						this.directRelations.RemoveAt(i);
						if (this.directRelations.Find((DirectPawnRelation x) => x.otherPawn == otherPawn) == null)
						{
							otherPawn.relations.pawnsWithDirectRelationsWithMe.Remove(this.pawn);
						}
						this.GainedOrLostDirectRelation();
						otherPawn.relations.GainedOrLostDirectRelation();
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06001855 RID: 6229 RVA: 0x000D4154 File Offset: 0x000D2554
		public int OpinionOf(Pawn other)
		{
			int result;
			if (!other.RaceProps.Humanlike || this.pawn == other)
			{
				result = 0;
			}
			else if (this.pawn.Dead)
			{
				result = 0;
			}
			else
			{
				int num = 0;
				foreach (PawnRelationDef pawnRelationDef in this.pawn.GetRelations(other))
				{
					num += pawnRelationDef.opinionOffset;
				}
				if (this.pawn.RaceProps.Humanlike)
				{
					num += this.pawn.needs.mood.thoughts.TotalOpinionOffset(other);
				}
				if (num != 0)
				{
					float num2 = 1f;
					List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
					for (int i = 0; i < hediffs.Count; i++)
					{
						if (hediffs[i].CurStage != null)
						{
							num2 *= hediffs[i].CurStage.opinionOfOthersFactor;
						}
					}
					num = Mathf.RoundToInt((float)num * num2);
				}
				if (num > 0 && this.pawn.HostileTo(other))
				{
					num = 0;
				}
				result = Mathf.Clamp(num, -100, 100);
			}
			return result;
		}

		// Token: 0x06001856 RID: 6230 RVA: 0x000D42D0 File Offset: 0x000D26D0
		public string OpinionExplanation(Pawn other)
		{
			string result;
			if (!other.RaceProps.Humanlike || this.pawn == other)
			{
				result = "";
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("OpinionOf".Translate(new object[]
				{
					other.LabelShort
				}) + ": " + this.OpinionOf(other).ToStringWithSign());
				string pawnSituationLabel = SocialCardUtility.GetPawnSituationLabel(other, this.pawn);
				if (!pawnSituationLabel.NullOrEmpty())
				{
					stringBuilder.AppendLine(pawnSituationLabel);
				}
				stringBuilder.AppendLine("--------------");
				bool flag = false;
				if (this.pawn.Dead)
				{
					stringBuilder.AppendLine("IAmDead".Translate());
					flag = true;
				}
				else
				{
					IEnumerable<PawnRelationDef> relations = this.pawn.GetRelations(other);
					foreach (PawnRelationDef pawnRelationDef in relations)
					{
						stringBuilder.AppendLine(pawnRelationDef.GetGenderSpecificLabelCap(other) + ": " + pawnRelationDef.opinionOffset.ToStringWithSign());
						flag = true;
					}
					if (this.pawn.RaceProps.Humanlike)
					{
						ThoughtHandler thoughts = this.pawn.needs.mood.thoughts;
						thoughts.GetDistinctSocialThoughtGroups(other, Pawn_RelationsTracker.tmpSocialThoughts);
						for (int i = 0; i < Pawn_RelationsTracker.tmpSocialThoughts.Count; i++)
						{
							ISocialThought socialThought = Pawn_RelationsTracker.tmpSocialThoughts[i];
							int num = 1;
							Thought thought = (Thought)socialThought;
							if (thought.def.IsMemory)
							{
								num = thoughts.memories.NumMemoriesInGroup((Thought_MemorySocial)socialThought);
							}
							stringBuilder.Append(thought.LabelCapSocial);
							if (num != 1)
							{
								stringBuilder.Append(" x" + num);
							}
							stringBuilder.AppendLine(": " + thoughts.OpinionOffsetOfGroup(socialThought, other).ToStringWithSign());
							flag = true;
						}
					}
					List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
					for (int j = 0; j < hediffs.Count; j++)
					{
						HediffStage curStage = hediffs[j].CurStage;
						if (curStage != null && curStage.opinionOfOthersFactor != 1f)
						{
							stringBuilder.Append(hediffs[j].LabelBase.CapitalizeFirst());
							if (curStage.opinionOfOthersFactor != 0f)
							{
								stringBuilder.AppendLine(": x" + curStage.opinionOfOthersFactor.ToStringPercent());
							}
							else
							{
								stringBuilder.AppendLine();
							}
							flag = true;
						}
					}
					if (this.pawn.HostileTo(other))
					{
						stringBuilder.AppendLine("Hostile".Translate());
						flag = true;
					}
				}
				if (!flag)
				{
					stringBuilder.AppendLine("NoneBrackets".Translate());
				}
				result = stringBuilder.ToString().TrimEndNewlines();
			}
			return result;
		}

		// Token: 0x06001857 RID: 6231 RVA: 0x000D460C File Offset: 0x000D2A0C
		public float SecondaryLovinChanceFactor(Pawn otherPawn)
		{
			float result;
			if (this.pawn.def != otherPawn.def || this.pawn == otherPawn)
			{
				result = 0f;
			}
			else
			{
				if (Rand.ValueSeeded(this.pawn.thingIDNumber ^ 3273711) >= 0.015f)
				{
					if (this.pawn.RaceProps.Humanlike && this.pawn.story.traits.HasTrait(TraitDefOf.Gay))
					{
						if (otherPawn.gender != this.pawn.gender)
						{
							return 0f;
						}
					}
					else if (otherPawn.gender == this.pawn.gender)
					{
						return 0f;
					}
				}
				float ageBiologicalYearsFloat = this.pawn.ageTracker.AgeBiologicalYearsFloat;
				float ageBiologicalYearsFloat2 = otherPawn.ageTracker.AgeBiologicalYearsFloat;
				float num = 1f;
				if (this.pawn.gender == Gender.Male)
				{
					if (ageBiologicalYearsFloat2 < 16f)
					{
						return 0f;
					}
					float min = Mathf.Max(16f, ageBiologicalYearsFloat - 30f);
					float lower = Mathf.Max(20f, ageBiologicalYearsFloat - 10f);
					num = GenMath.FlatHill(0.15f, min, lower, ageBiologicalYearsFloat, ageBiologicalYearsFloat + 10f, 0.15f, ageBiologicalYearsFloat2);
				}
				else if (this.pawn.gender == Gender.Female)
				{
					if (ageBiologicalYearsFloat2 < 16f)
					{
						return 0f;
					}
					if (ageBiologicalYearsFloat2 < ageBiologicalYearsFloat - 10f)
					{
						return 0.15f;
					}
					if (ageBiologicalYearsFloat2 < ageBiologicalYearsFloat - 3f)
					{
						num = Mathf.InverseLerp(ageBiologicalYearsFloat - 10f, ageBiologicalYearsFloat - 3f, ageBiologicalYearsFloat2) * 0.3f;
					}
					else
					{
						num = GenMath.FlatHill(0.3f, ageBiologicalYearsFloat - 3f, ageBiologicalYearsFloat, ageBiologicalYearsFloat + 10f, ageBiologicalYearsFloat + 30f, 0.15f, ageBiologicalYearsFloat2);
					}
				}
				float num2 = 1f;
				num2 *= Mathf.Lerp(0.2f, 1f, otherPawn.health.capacities.GetLevel(PawnCapacityDefOf.Talking));
				num2 *= Mathf.Lerp(0.2f, 1f, otherPawn.health.capacities.GetLevel(PawnCapacityDefOf.Manipulation));
				num2 *= Mathf.Lerp(0.2f, 1f, otherPawn.health.capacities.GetLevel(PawnCapacityDefOf.Moving));
				int num3 = 0;
				if (otherPawn.RaceProps.Humanlike)
				{
					num3 = otherPawn.story.traits.DegreeOfTrait(TraitDefOf.Beauty);
				}
				float num4 = 1f;
				if (num3 < 0)
				{
					num4 = 0.3f;
				}
				else if (num3 > 0)
				{
					num4 = 2.3f;
				}
				float num5 = Mathf.InverseLerp(15f, 18f, ageBiologicalYearsFloat);
				float num6 = Mathf.InverseLerp(15f, 18f, ageBiologicalYearsFloat2);
				float num7 = num * num2 * num5 * num6 * num4;
				result = num7;
			}
			return result;
		}

		// Token: 0x06001858 RID: 6232 RVA: 0x000D4930 File Offset: 0x000D2D30
		public float SecondaryRomanceChanceFactor(Pawn otherPawn)
		{
			float num = 1f;
			foreach (PawnRelationDef pawnRelationDef in this.pawn.GetRelations(otherPawn))
			{
				num *= pawnRelationDef.attractionFactor;
			}
			return this.SecondaryLovinChanceFactor(otherPawn) * num;
		}

		// Token: 0x06001859 RID: 6233 RVA: 0x000D49AC File Offset: 0x000D2DAC
		public float CompatibilityWith(Pawn otherPawn)
		{
			float result;
			if (this.pawn.def != otherPawn.def || this.pawn == otherPawn)
			{
				result = 0f;
			}
			else
			{
				float x = Mathf.Abs(this.pawn.ageTracker.AgeBiologicalYearsFloat - otherPawn.ageTracker.AgeBiologicalYearsFloat);
				float num = GenMath.LerpDouble(0f, 20f, 0.45f, -0.45f, x);
				num = Mathf.Clamp(num, -0.45f, 0.45f);
				float num2 = this.ConstantPerPawnsPairCompatibilityOffset(otherPawn.thingIDNumber);
				result = num + num2;
			}
			return result;
		}

		// Token: 0x0600185A RID: 6234 RVA: 0x000D4A4C File Offset: 0x000D2E4C
		public float ConstantPerPawnsPairCompatibilityOffset(int otherPawnID)
		{
			Rand.PushState();
			Rand.Seed = (this.pawn.thingIDNumber ^ otherPawnID) * 37;
			float result = Rand.GaussianAsymmetric(0.3f, 1f, 1.4f);
			Rand.PopState();
			return result;
		}

		// Token: 0x0600185B RID: 6235 RVA: 0x000D4A98 File Offset: 0x000D2E98
		public void ClearAllRelations()
		{
			List<DirectPawnRelation> list = this.directRelations.ToList<DirectPawnRelation>();
			for (int i = 0; i < list.Count; i++)
			{
				this.RemoveDirectRelation(list[i]);
			}
			List<Pawn> list2 = this.pawnsWithDirectRelationsWithMe.ToList<Pawn>();
			for (int j = 0; j < list2.Count; j++)
			{
				List<DirectPawnRelation> list3 = list2[j].relations.directRelations.ToList<DirectPawnRelation>();
				for (int k = 0; k < list3.Count; k++)
				{
					if (list3[k].otherPawn == this.pawn)
					{
						list2[j].relations.RemoveDirectRelation(list3[k]);
					}
				}
			}
		}

		// Token: 0x0600185C RID: 6236 RVA: 0x000D4B68 File Offset: 0x000D2F68
		internal void Notify_PawnKilled(DamageInfo? dinfo, Map mapBeforeDeath)
		{
			foreach (Pawn pawn in this.PotentiallyRelatedPawns)
			{
				if (!pawn.Dead && pawn.needs.mood != null)
				{
					pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
				}
			}
			this.RemoveMySpouseMarriageRelatedThoughts();
			if (this.everSeenByPlayer && !PawnGenerator.IsBeingGenerated(this.pawn))
			{
				if (!this.pawn.RaceProps.Animal)
				{
					this.AffectBondedAnimalsOnMyDeath();
				}
			}
			this.Notify_FailedRescueQuest();
		}

		// Token: 0x0600185D RID: 6237 RVA: 0x000D4C3C File Offset: 0x000D303C
		public void Notify_PassedToWorld()
		{
			if (!this.pawn.Dead)
			{
				this.relativeInvolvedInRescueQuest = null;
			}
		}

		// Token: 0x0600185E RID: 6238 RVA: 0x000D4C56 File Offset: 0x000D3056
		public void Notify_ExitedMap()
		{
			this.Rescued();
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x000D4C5F File Offset: 0x000D305F
		public void Notify_ChangedFaction()
		{
			if (this.pawn.Faction == Faction.OfPlayer)
			{
				this.Rescued();
			}
		}

		// Token: 0x06001860 RID: 6240 RVA: 0x000D4C80 File Offset: 0x000D3080
		public void Notify_PawnSold(Pawn playerNegotiator)
		{
			foreach (Pawn pawn in this.PotentiallyRelatedPawns)
			{
				if (!pawn.Dead && pawn.needs.mood != null)
				{
					PawnRelationDef mostImportantRelation = pawn.GetMostImportantRelation(this.pawn);
					if (mostImportantRelation != null && mostImportantRelation.soldThought != null)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(mostImportantRelation.soldThought, playerNegotiator);
					}
				}
			}
			this.RemoveMySpouseMarriageRelatedThoughts();
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x000D4D3C File Offset: 0x000D313C
		public void Notify_PawnKidnapped()
		{
			this.RemoveMySpouseMarriageRelatedThoughts();
		}

		// Token: 0x06001862 RID: 6242 RVA: 0x000D4D48 File Offset: 0x000D3148
		public void Notify_RescuedBy(Pawn rescuer)
		{
			if (rescuer.RaceProps.Humanlike && this.canGetRescuedThought)
			{
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RescuedMe, rescuer);
				this.canGetRescuedThought = false;
			}
		}

		// Token: 0x06001863 RID: 6243 RVA: 0x000D4DA0 File Offset: 0x000D31A0
		public void Notify_FailedRescueQuest()
		{
			if (this.relativeInvolvedInRescueQuest != null && !this.relativeInvolvedInRescueQuest.Dead && this.relativeInvolvedInRescueQuest.needs.mood != null)
			{
				Messages.Message("MessageFailedToRescueRelative".Translate(new object[]
				{
					this.pawn.LabelShort,
					this.relativeInvolvedInRescueQuest.LabelShort
				}), this.relativeInvolvedInRescueQuest, MessageTypeDefOf.PawnDeath, true);
				this.relativeInvolvedInRescueQuest.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FailedToRescueRelative, this.pawn);
			}
			this.relativeInvolvedInRescueQuest = null;
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x000D4E54 File Offset: 0x000D3254
		private void Rescued()
		{
			if (this.relativeInvolvedInRescueQuest != null && !this.relativeInvolvedInRescueQuest.Dead && this.relativeInvolvedInRescueQuest.needs.mood != null)
			{
				Messages.Message("MessageRescuedRelative".Translate(new object[]
				{
					this.pawn.LabelShort,
					this.relativeInvolvedInRescueQuest.LabelShort
				}), this.relativeInvolvedInRescueQuest, MessageTypeDefOf.PositiveEvent, true);
				this.relativeInvolvedInRescueQuest.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RescuedRelative, this.pawn);
			}
			this.relativeInvolvedInRescueQuest = null;
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x000D4F08 File Offset: 0x000D3308
		public float GetFriendDiedThoughtPowerFactor(int opinion)
		{
			return Mathf.Lerp(0.15f, 1f, Mathf.InverseLerp(20f, 100f, (float)opinion));
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x000D4F40 File Offset: 0x000D3340
		public float GetRivalDiedThoughtPowerFactor(int opinion)
		{
			return Mathf.Lerp(0.15f, 1f, Mathf.InverseLerp(-20f, -100f, (float)opinion));
		}

		// Token: 0x06001867 RID: 6247 RVA: 0x000D4F78 File Offset: 0x000D3378
		private void RemoveMySpouseMarriageRelatedThoughts()
		{
			Pawn spouse = this.pawn.GetSpouse();
			if (spouse != null && !spouse.Dead && spouse.needs.mood != null)
			{
				MemoryThoughtHandler memories = spouse.needs.mood.thoughts.memories;
				memories.RemoveMemoriesOfDef(ThoughtDefOf.GotMarried);
				memories.RemoveMemoriesOfDef(ThoughtDefOf.HoneymoonPhase);
			}
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x000D4FE4 File Offset: 0x000D33E4
		public void CheckAppendBondedAnimalDiedInfo(ref string letter, ref string label)
		{
			if (this.pawn.RaceProps.Animal && this.everSeenByPlayer && !PawnGenerator.IsBeingGenerated(this.pawn))
			{
				Predicate<Pawn> isAffected = (Pawn x) => !x.Dead && (!x.RaceProps.Humanlike || !x.story.traits.HasTrait(TraitDefOf.Psychopath));
				int num = 0;
				for (int i = 0; i < this.directRelations.Count; i++)
				{
					if (this.directRelations[i].def == PawnRelationDefOf.Bond && isAffected(this.directRelations[i].otherPawn))
					{
						num++;
					}
				}
				if (num != 0)
				{
					string str;
					if (num == 1)
					{
						Pawn firstDirectRelationPawn = this.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, (Pawn x) => isAffected(x));
						str = "LetterPartBondedAnimalDied".Translate(new object[]
						{
							this.pawn.LabelDefinite(),
							firstDirectRelationPawn.LabelShort
						}).CapitalizeFirst();
					}
					else
					{
						StringBuilder stringBuilder = new StringBuilder();
						for (int j = 0; j < this.directRelations.Count; j++)
						{
							if (this.directRelations[j].def == PawnRelationDefOf.Bond && isAffected(this.directRelations[j].otherPawn))
							{
								stringBuilder.AppendLine("  - " + this.directRelations[j].otherPawn.LabelShort);
							}
						}
						str = "LetterPartBondedAnimalDiedMulti".Translate(new object[]
						{
							stringBuilder.ToString().TrimEndNewlines()
						});
					}
					label = label + " (" + "LetterLabelSuffixBondedAnimalDied".Translate() + ")";
					if (!letter.NullOrEmpty())
					{
						letter += "\n\n";
					}
					letter += str;
				}
			}
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x000D5204 File Offset: 0x000D3604
		private void AffectBondedAnimalsOnMyDeath()
		{
			int num = 0;
			Pawn pawn = null;
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				if (this.directRelations[i].def == PawnRelationDefOf.Bond && this.directRelations[i].otherPawn.Spawned)
				{
					pawn = this.directRelations[i].otherPawn;
					num++;
					float value = Rand.Value;
					MentalStateDef stateDef;
					if (value < 0.25f)
					{
						stateDef = MentalStateDefOf.Wander_Sad;
					}
					if (value < 0.5f)
					{
						stateDef = MentalStateDefOf.Wander_Psychotic;
					}
					else if (value < 0.75f)
					{
						stateDef = MentalStateDefOf.Berserk;
					}
					else
					{
						stateDef = MentalStateDefOf.Manhunter;
					}
					this.directRelations[i].otherPawn.mindState.mentalStateHandler.TryStartMentalState(stateDef, null, true, false, null, false);
				}
			}
			if (num == 1)
			{
				string str;
				if (pawn.Name != null && !pawn.Name.Numerical)
				{
					str = "MessageNamedBondedAnimalMentalBreak".Translate(new object[]
					{
						pawn.KindLabelIndefinite(),
						pawn.Name.ToStringShort,
						this.pawn.LabelShort
					});
				}
				else
				{
					str = "MessageBondedAnimalMentalBreak".Translate(new object[]
					{
						pawn.LabelIndefinite(),
						this.pawn.LabelShort
					});
				}
				Messages.Message(str.CapitalizeFirst(), pawn, MessageTypeDefOf.ThreatSmall, true);
			}
			else if (num > 1)
			{
				Messages.Message("MessageBondedAnimalsMentalBreak".Translate(new object[]
				{
					num,
					this.pawn.LabelShort
				}).CapitalizeFirst(), pawn, MessageTypeDefOf.ThreatSmall, true);
			}
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x000D53E4 File Offset: 0x000D37E4
		private void Tick_CheckStartMarriageCeremony()
		{
			if (this.pawn.Spawned && !this.pawn.RaceProps.Animal)
			{
				if (this.pawn.IsHashIntervalTick(1017))
				{
					int ticksGame = Find.TickManager.TicksGame;
					for (int i = 0; i < this.directRelations.Count; i++)
					{
						float num = (float)(ticksGame - this.directRelations[i].startTicks) / 60000f;
						if (this.directRelations[i].def == PawnRelationDefOf.Fiance && this.pawn.thingIDNumber < this.directRelations[i].otherPawn.thingIDNumber && num > 10f && Rand.MTBEventOccurs(2f, 60000f, 1017f) && this.pawn.Map == this.directRelations[i].otherPawn.Map && this.pawn.Map.IsPlayerHome && MarriageCeremonyUtility.AcceptableGameConditionsToStartCeremony(this.pawn.Map) && MarriageCeremonyUtility.FianceReadyToStartCeremony(this.pawn) && MarriageCeremonyUtility.FianceReadyToStartCeremony(this.directRelations[i].otherPawn))
						{
							this.pawn.Map.lordsStarter.TryStartMarriageCeremony(this.pawn, this.directRelations[i].otherPawn);
						}
					}
				}
			}
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x000D5588 File Offset: 0x000D3988
		private void Tick_CheckDevelopBondRelation()
		{
			if (this.pawn.Spawned && this.pawn.RaceProps.Animal && this.pawn.Faction == Faction.OfPlayer && this.pawn.playerSettings.RespectedMaster != null)
			{
				Pawn respectedMaster = this.pawn.playerSettings.RespectedMaster;
				if (this.pawn.IsHashIntervalTick(2500) && this.pawn.Position.InHorDistOf(respectedMaster.Position, 12f) && GenSight.LineOfSight(this.pawn.Position, respectedMaster.Position, this.pawn.Map, false, null, 0, 0))
				{
					RelationsUtility.TryDevelopBondRelation(respectedMaster, this.pawn, 0.001f);
				}
			}
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x000D5674 File Offset: 0x000D3A74
		private void GainedOrLostDirectRelation()
		{
			if (Current.ProgramState == ProgramState.Playing && !this.pawn.Dead && this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
		}

		// Token: 0x04000E70 RID: 3696
		private Pawn pawn;

		// Token: 0x04000E71 RID: 3697
		private List<DirectPawnRelation> directRelations = new List<DirectPawnRelation>();

		// Token: 0x04000E72 RID: 3698
		public bool everSeenByPlayer;

		// Token: 0x04000E73 RID: 3699
		public bool canGetRescuedThought = true;

		// Token: 0x04000E74 RID: 3700
		public Pawn relativeInvolvedInRescueQuest;

		// Token: 0x04000E75 RID: 3701
		private HashSet<Pawn> pawnsWithDirectRelationsWithMe = new HashSet<Pawn>();

		// Token: 0x04000E76 RID: 3702
		private List<Pawn> cachedFamilyByBlood = new List<Pawn>();

		// Token: 0x04000E77 RID: 3703
		private bool familyByBloodIsCached;

		// Token: 0x04000E78 RID: 3704
		private bool canCacheFamilyByBlood;

		// Token: 0x04000E79 RID: 3705
		private const int CheckDevelopBondRelationIntervalTicks = 2500;

		// Token: 0x04000E7A RID: 3706
		private const float MaxBondRelationCheckDist = 12f;

		// Token: 0x04000E7B RID: 3707
		private const float BondRelationPerIntervalChance = 0.001f;

		// Token: 0x04000E7C RID: 3708
		public const int FriendOpinionThreshold = 20;

		// Token: 0x04000E7D RID: 3709
		public const int RivalOpinionThreshold = -20;

		// Token: 0x04000E7E RID: 3710
		private static List<ISocialThought> tmpSocialThoughts = new List<ISocialThought>();
	}
}
