using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Pawn_RelationsTracker : IExposable
	{
		private Pawn pawn;

		private List<DirectPawnRelation> directRelations = new List<DirectPawnRelation>();

		public bool everSeenByPlayer;

		public bool canGetRescuedThought = true;

		public Pawn relativeInvolvedInRescueQuest;

		private HashSet<Pawn> pawnsWithDirectRelationsWithMe = new HashSet<Pawn>();

		private List<Pawn> cachedFamilyByBlood = new List<Pawn>();

		private bool familyByBloodIsCached;

		private bool canCacheFamilyByBlood;

		private const int CheckDevelopBondRelationIntervalTicks = 2500;

		private const float MaxBondRelationCheckDist = 12f;

		private const float BondRelationPerIntervalChance = 0.001f;

		public const int FriendOpinionThreshold = 20;

		public const int RivalOpinionThreshold = -20;

		private static List<ISocialThought> tmpSocialThoughts = new List<ISocialThought>();

		public List<DirectPawnRelation> DirectRelations
		{
			get
			{
				return this.directRelations;
			}
		}

		public IEnumerable<Pawn> Children
		{
			get
			{
				foreach (Pawn item in this.pawnsWithDirectRelationsWithMe)
				{
					List<DirectPawnRelation> hisDirectRels = item.relations.directRelations;
					for (int i = 0; i < hisDirectRels.Count; i++)
					{
						if (hisDirectRels[i].otherPawn == this.pawn && hisDirectRels[i].def == PawnRelationDefOf.Parent)
						{
							yield return item;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				yield break;
				IL_0145:
				/*Error near IL_0146: Unexpected return in MoveNext()*/;
			}
		}

		public int ChildrenCount
		{
			get
			{
				return this.Children.Count();
			}
		}

		public bool RelatedToAnyoneOrAnyoneRelatedToMe
		{
			get
			{
				return this.directRelations.Any() || this.pawnsWithDirectRelationsWithMe.Any();
			}
		}

		public IEnumerable<Pawn> FamilyByBlood
		{
			get
			{
				if (this.canCacheFamilyByBlood)
				{
					if (!this.familyByBloodIsCached)
					{
						this.cachedFamilyByBlood.Clear();
						this.cachedFamilyByBlood.AddRange(this.FamilyByBlood_Internal);
						this.familyByBloodIsCached = true;
					}
					return this.cachedFamilyByBlood;
				}
				return this.FamilyByBlood_Internal;
			}
		}

		private IEnumerable<Pawn> FamilyByBlood_Internal
		{
			get
			{
				if (this.RelatedToAnyoneOrAnyoneRelatedToMe)
				{
					try
					{
						List<Pawn> familyStack = SimplePool<List<Pawn>>.Get();
						List<Pawn> familyChildrenStack = SimplePool<List<Pawn>>.Get();
						HashSet<Pawn> familyVisited = SimplePool<HashSet<Pawn>>.Get();
						familyStack.Add(this.pawn);
						familyVisited.Add(this.pawn);
						Pawn p;
						while (true)
						{
							if (familyStack.Any())
							{
								p = familyStack[familyStack.Count - 1];
								familyStack.RemoveLast();
								if (p == this.pawn)
								{
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
									while (familyChildrenStack.Any())
									{
										Pawn child = familyChildrenStack[familyChildrenStack.Count - 1];
										familyChildrenStack.RemoveLast();
										if (child != p && child != this.pawn)
										{
											yield return child;
											/*Error: Unable to find new state assignment for yield return*/;
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
									continue;
								}
								break;
							}
							yield break;
						}
						yield return p;
						/*Error: Unable to find new state assignment for yield return*/;
					}
					finally
					{
						((_003C_003Ec__Iterator1)/*Error near IL_030c: stateMachine*/)._003C_003E__Finally0();
					}
				}
				yield break;
				IL_031c:
				/*Error near IL_031d: Unexpected return in MoveNext()*/;
			}
		}

		public IEnumerable<Pawn> PotentiallyRelatedPawns
		{
			get
			{
				if (this.RelatedToAnyoneOrAnyoneRelatedToMe)
				{
					try
					{
						List<Pawn> stack = SimplePool<List<Pawn>>.Get();
						HashSet<Pawn> visited = SimplePool<HashSet<Pawn>>.Get();
						stack.Add(this.pawn);
						visited.Add(this.pawn);
						Pawn p;
						while (true)
						{
							if (stack.Any())
							{
								p = stack[stack.Count - 1];
								stack.RemoveLast();
								if (p == this.pawn)
								{
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
									continue;
								}
								break;
							}
							yield break;
						}
						yield return p;
						/*Error: Unable to find new state assignment for yield return*/;
					}
					finally
					{
						((_003C_003Ec__Iterator2)/*Error near IL_0204: stateMachine*/)._003C_003E__Finally0();
					}
				}
				yield break;
				IL_0214:
				/*Error near IL_0215: Unexpected return in MoveNext()*/;
			}
		}

		public IEnumerable<Pawn> RelatedPawns
		{
			get
			{
				this.canCacheFamilyByBlood = true;
				this.familyByBloodIsCached = false;
				this.cachedFamilyByBlood.Clear();
				try
				{
					foreach (Pawn potentiallyRelatedPawn in this.PotentiallyRelatedPawns)
					{
						if ((!this.familyByBloodIsCached || !this.cachedFamilyByBlood.Contains(potentiallyRelatedPawn)) && !this.pawn.GetRelations(potentiallyRelatedPawn).Any())
						{
							continue;
						}
						yield return potentiallyRelatedPawn;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				finally
				{
					((_003C_003Ec__Iterator3)/*Error near IL_014a: stateMachine*/)._003C_003E__Finally0();
				}
				yield break;
				IL_015a:
				/*Error near IL_015b: Unexpected return in MoveNext()*/;
			}
		}

		public Pawn_RelationsTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<DirectPawnRelation>(ref this.directRelations, "directRelations", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = 0; i < this.directRelations.Count; i++)
				{
					if (this.directRelations[i].otherPawn == null)
					{
						Log.Warning("Pawn " + this.pawn + " has relation \"" + this.directRelations[i].def.defName + "\" with null pawn after loading. This means that we forgot to serialize pawns somewhere (e.g. pawns from passing trade ships).");
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

		public void SocialTrackerTick()
		{
			if (!this.pawn.Dead)
			{
				this.Tick_CheckStartMarriageCeremony();
				this.Tick_CheckDevelopBondRelation();
			}
		}

		public DirectPawnRelation GetDirectRelation(PawnRelationDef def, Pawn otherPawn)
		{
			if (def.implied)
			{
				Log.Warning(def + " is not a direct relation.");
				return null;
			}
			return this.directRelations.Find((DirectPawnRelation x) => x.def == def && x.otherPawn == otherPawn);
		}

		public Pawn GetFirstDirectRelationPawn(PawnRelationDef def, Predicate<Pawn> predicate = null)
		{
			if (def.implied)
			{
				Log.Warning(def + " is not a direct relation.");
				return null;
			}
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				DirectPawnRelation directPawnRelation = this.directRelations[i];
				if (directPawnRelation.def == def && (predicate == null || predicate(directPawnRelation.otherPawn)))
				{
					return directPawnRelation.otherPawn;
				}
			}
			return null;
		}

		public bool DirectRelationExists(PawnRelationDef def, Pawn otherPawn)
		{
			if (def.implied)
			{
				Log.Warning(def + " is not a direct relation.");
				return false;
			}
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				DirectPawnRelation directPawnRelation = this.directRelations[i];
				if (directPawnRelation.def == def && directPawnRelation.otherPawn == otherPawn)
				{
					return true;
				}
			}
			return false;
		}

		public void AddDirectRelation(PawnRelationDef def, Pawn otherPawn)
		{
			if (def.implied)
			{
				Log.Warning("Tried to directly add implied pawn relation " + def + ", pawn=" + this.pawn + ", otherPawn=" + otherPawn);
			}
			else if (otherPawn == this.pawn)
			{
				Log.Warning("Tried to add pawn relation " + def + " with self, pawn=" + this.pawn);
			}
			else if (this.DirectRelationExists(def, otherPawn))
			{
				Log.Warning("Tried to add the same relation twice: " + def + ", pawn=" + this.pawn + ", otherPawn=" + otherPawn);
			}
			else
			{
				int startTicks = (Current.ProgramState == ProgramState.Playing) ? Find.TickManager.TicksGame : 0;
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

		public void RemoveDirectRelation(DirectPawnRelation relation)
		{
			this.RemoveDirectRelation(relation.def, relation.otherPawn);
		}

		public void RemoveDirectRelation(PawnRelationDef def, Pawn otherPawn)
		{
			if (!this.TryRemoveDirectRelation(def, otherPawn))
			{
				Log.Warning("Could not remove relation " + def + " because it's not here. pawn=" + this.pawn + ", otherPawn=" + otherPawn);
			}
		}

		public bool TryRemoveDirectRelation(PawnRelationDef def, Pawn otherPawn)
		{
			if (def.implied)
			{
				Log.Warning("Tried to remove implied pawn relation " + def + ", pawn=" + this.pawn + ", otherPawn=" + otherPawn);
				return false;
			}
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
			return false;
		}

		public int OpinionOf(Pawn other)
		{
			if (other.RaceProps.Humanlike && this.pawn != other)
			{
				if (this.pawn.Dead)
				{
					return 0;
				}
				int num = 0;
				foreach (PawnRelationDef relation in this.pawn.GetRelations(other))
				{
					num += relation.opinionOffset;
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
				return Mathf.Clamp(num, -100, 100);
			}
			return 0;
		}

		public string OpinionExplanation(Pawn other)
		{
			if (other.RaceProps.Humanlike && this.pawn != other)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("OpinionOf".Translate(other.LabelShort) + ": " + this.OpinionOf(other).ToStringWithSign());
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
					foreach (PawnRelationDef item in relations)
					{
						stringBuilder.AppendLine(item.GetGenderSpecificLabelCap(other) + ": " + item.opinionOffset.ToStringWithSign());
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
						if (curStage != null && curStage.opinionOfOthersFactor != 1.0)
						{
							stringBuilder.Append(hediffs[j].LabelBase.CapitalizeFirst());
							if (curStage.opinionOfOthersFactor != 0.0)
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
				return stringBuilder.ToString().TrimEndNewlines();
			}
			return string.Empty;
		}

		public float SecondaryLovinChanceFactor(Pawn otherPawn)
		{
			if (this.pawn.def == otherPawn.def && this.pawn != otherPawn)
			{
				if (!(Rand.ValueSeeded(this.pawn.thingIDNumber ^ 3273711) < 0.014999999664723873))
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
					if (ageBiologicalYearsFloat2 < 16.0)
					{
						return 0f;
					}
					float min = Mathf.Max(16f, (float)(ageBiologicalYearsFloat - 30.0));
					float lower = Mathf.Max(20f, (float)(ageBiologicalYearsFloat - 10.0));
					num = GenMath.FlatHill(0.15f, min, lower, ageBiologicalYearsFloat, (float)(ageBiologicalYearsFloat + 10.0), 0.15f, ageBiologicalYearsFloat2);
				}
				else if (this.pawn.gender == Gender.Female)
				{
					if (ageBiologicalYearsFloat2 < 16.0)
					{
						return 0f;
					}
					if (ageBiologicalYearsFloat2 < ageBiologicalYearsFloat - 10.0)
					{
						return 0.15f;
					}
					num = (float)((!(ageBiologicalYearsFloat2 < ageBiologicalYearsFloat - 3.0)) ? GenMath.FlatHill(0.3f, (float)(ageBiologicalYearsFloat - 3.0), ageBiologicalYearsFloat, (float)(ageBiologicalYearsFloat + 10.0), (float)(ageBiologicalYearsFloat + 30.0), 0.15f, ageBiologicalYearsFloat2) : (Mathf.InverseLerp((float)(ageBiologicalYearsFloat - 10.0), (float)(ageBiologicalYearsFloat - 3.0), ageBiologicalYearsFloat2) * 0.30000001192092896));
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
				return num * num2 * num5 * num6 * num4;
			}
			return 0f;
		}

		public float SecondaryRomanceChanceFactor(Pawn otherPawn)
		{
			float num = 1f;
			foreach (PawnRelationDef relation in this.pawn.GetRelations(otherPawn))
			{
				num *= relation.attractionFactor;
			}
			return this.SecondaryLovinChanceFactor(otherPawn) * num;
		}

		public float CompatibilityWith(Pawn otherPawn)
		{
			if (this.pawn.def == otherPawn.def && this.pawn != otherPawn)
			{
				float x = Mathf.Abs(this.pawn.ageTracker.AgeBiologicalYearsFloat - otherPawn.ageTracker.AgeBiologicalYearsFloat);
				float value = GenMath.LerpDouble(0f, 20f, 0.45f, -0.45f, x);
				value = Mathf.Clamp(value, -0.45f, 0.45f);
				float num = this.ConstantPerPawnsPairCompatibilityOffset(otherPawn.thingIDNumber);
				return value + num;
			}
			return 0f;
		}

		public float ConstantPerPawnsPairCompatibilityOffset(int otherPawnID)
		{
			Rand.PushState();
			Rand.Seed = (this.pawn.thingIDNumber ^ otherPawnID) * 37;
			float result = Rand.GaussianAsymmetric(0.3f, 1f, 1.4f);
			Rand.PopState();
			return result;
		}

		public void ClearAllRelations()
		{
			List<DirectPawnRelation> list = this.directRelations.ToList();
			for (int i = 0; i < list.Count; i++)
			{
				this.RemoveDirectRelation(list[i]);
			}
			List<Pawn> list2 = this.pawnsWithDirectRelationsWithMe.ToList();
			for (int j = 0; j < list2.Count; j++)
			{
				List<DirectPawnRelation> list3 = list2[j].relations.directRelations.ToList();
				for (int k = 0; k < list3.Count; k++)
				{
					if (list3[k].otherPawn == this.pawn)
					{
						list2[j].relations.RemoveDirectRelation(list3[k]);
					}
				}
			}
		}

		internal void Notify_PawnKilled(DamageInfo? dinfo, Map mapBeforeDeath)
		{
			foreach (Pawn potentiallyRelatedPawn in this.PotentiallyRelatedPawns)
			{
				if (!potentiallyRelatedPawn.Dead && potentiallyRelatedPawn.needs.mood != null)
				{
					potentiallyRelatedPawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
				}
			}
			this.RemoveMySpouseMarriageRelatedThoughts();
			if (this.everSeenByPlayer && !PawnGenerator.IsBeingGenerated(this.pawn))
			{
				if (this.pawn.RaceProps.Animal)
				{
					this.SendBondedAnimalDiedLetter(mapBeforeDeath);
				}
				else
				{
					this.AffectBondedAnimalsOnMyDeath();
				}
			}
			if (this.relativeInvolvedInRescueQuest != null && !this.relativeInvolvedInRescueQuest.Dead && this.relativeInvolvedInRescueQuest.needs.mood != null)
			{
				Messages.Message("MessageFailedToRescueRelative".Translate(this.pawn.LabelShort, this.relativeInvolvedInRescueQuest.LabelShort), this.relativeInvolvedInRescueQuest, MessageTypeDefOf.PawnDeath);
				this.relativeInvolvedInRescueQuest.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FailedToRescueRelative, this.pawn);
			}
			this.relativeInvolvedInRescueQuest = null;
		}

		public void Notify_PassedToWorld()
		{
			if (!this.pawn.Dead)
			{
				this.relativeInvolvedInRescueQuest = null;
			}
		}

		public void Notify_ExitedMap()
		{
			this.Rescued();
		}

		public void Notify_ChangedFaction()
		{
			if (this.pawn.Faction == Faction.OfPlayer)
			{
				this.Rescued();
			}
		}

		public void Notify_PawnSold(Pawn playerNegotiator)
		{
			foreach (Pawn potentiallyRelatedPawn in this.PotentiallyRelatedPawns)
			{
				if (!potentiallyRelatedPawn.Dead && potentiallyRelatedPawn.needs.mood != null)
				{
					PawnRelationDef mostImportantRelation = potentiallyRelatedPawn.GetMostImportantRelation(this.pawn);
					if (mostImportantRelation != null && mostImportantRelation.soldThought != null)
					{
						potentiallyRelatedPawn.needs.mood.thoughts.memories.TryGainMemory(mostImportantRelation.soldThought, playerNegotiator);
					}
				}
			}
			this.RemoveMySpouseMarriageRelatedThoughts();
		}

		public void Notify_PawnKidnapped()
		{
			this.RemoveMySpouseMarriageRelatedThoughts();
		}

		public void Notify_RescuedBy(Pawn rescuer)
		{
			if (rescuer.RaceProps.Humanlike && this.canGetRescuedThought)
			{
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RescuedMe, rescuer);
				this.canGetRescuedThought = false;
			}
		}

		private void Rescued()
		{
			if (this.relativeInvolvedInRescueQuest != null && !this.relativeInvolvedInRescueQuest.Dead && this.relativeInvolvedInRescueQuest.needs.mood != null)
			{
				Messages.Message("MessageRescuedRelative".Translate(this.pawn.LabelShort, this.relativeInvolvedInRescueQuest.LabelShort), this.relativeInvolvedInRescueQuest, MessageTypeDefOf.PositiveEvent);
				this.relativeInvolvedInRescueQuest.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RescuedRelative, this.pawn);
			}
			this.relativeInvolvedInRescueQuest = null;
		}

		public float GetFriendDiedThoughtPowerFactor(int opinion)
		{
			return Mathf.Lerp(0.15f, 1f, Mathf.InverseLerp(20f, 100f, (float)opinion));
		}

		public float GetRivalDiedThoughtPowerFactor(int opinion)
		{
			return Mathf.Lerp(0.15f, 1f, Mathf.InverseLerp(-20f, -100f, (float)opinion));
		}

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

		private void SendBondedAnimalDiedLetter(Map mapBeforeDeath)
		{
			Predicate<Pawn> isAffected = delegate(Pawn x)
			{
				if (x.Dead)
				{
					return false;
				}
				if (x.RaceProps.Humanlike && x.story.traits.HasTrait(TraitDefOf.Psychopath))
				{
					return false;
				}
				return true;
			};
			int num = 0;
			for (int i = 0; i < this.directRelations.Count; i++)
			{
				if (this.directRelations[i].def == PawnRelationDefOf.Bond && isAffected(this.directRelations[i].otherPawn))
				{
					num++;
				}
			}
			string str;
			switch (num)
			{
			case 0:
				return;
			case 1:
			{
				Pawn firstDirectRelationPawn = this.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, (Pawn x) => isAffected(x));
				str = ((this.pawn.Name == null) ? "LetterBondedAnimalDied".Translate(this.pawn.KindLabel, firstDirectRelationPawn.LabelShort) : "LetterNamedBondedAnimalDied".Translate(this.pawn.KindLabel, this.pawn.Name.ToStringShort, firstDirectRelationPawn.LabelShort));
				break;
			}
			default:
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int j = 0; j < this.directRelations.Count; j++)
				{
					if (this.directRelations[j].def == PawnRelationDefOf.Bond && isAffected(this.directRelations[j].otherPawn))
					{
						stringBuilder.AppendLine("  - " + this.directRelations[j].otherPawn.LabelShort);
					}
				}
				str = ((this.pawn.Name == null) ? "LetterBondedAnimalDiedMulti".Translate(this.pawn.KindLabel, stringBuilder.ToString().TrimEndNewlines()) : "LetterNamedBondedAnimalDiedMulti".Translate(this.pawn.KindLabel, this.pawn.Name.ToStringShort, stringBuilder.ToString().TrimEndNewlines()));
				break;
			}
			}
			TargetInfo target = (mapBeforeDeath == null) ? TargetInfo.Invalid : new TargetInfo(this.pawn.Position, mapBeforeDeath, false);
			Find.LetterStack.ReceiveLetter("LetterLabelBondedAnimalDied".Translate(), str.CapitalizeFirst(), LetterDefOf.NegativeEvent, target, null);
		}

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
					MentalStateDef wanderSad;
					if (value < 0.25)
					{
						wanderSad = MentalStateDefOf.WanderSad;
					}
					wanderSad = ((!(value < 0.5)) ? ((!(value < 0.75)) ? MentalStateDefOf.Manhunter : MentalStateDefOf.Berserk) : MentalStateDefOf.WanderPsychotic);
					this.directRelations[i].otherPawn.mindState.mentalStateHandler.TryStartMentalState(wanderSad, null, true, false, null);
				}
			}
			if (num == 1)
			{
				string str = (pawn.Name == null || pawn.Name.Numerical) ? "MessageBondedAnimalMentalBreak".Translate(pawn.KindLabel, this.pawn.LabelShort) : "MessageNamedBondedAnimalMentalBreak".Translate(pawn.KindLabel, pawn.Name.ToStringShort, this.pawn.LabelShort);
				Messages.Message(str.CapitalizeFirst(), pawn, MessageTypeDefOf.ThreatSmall);
			}
			else if (num > 1)
			{
				Messages.Message("MessageBondedAnimalsMentalBreak".Translate(num, this.pawn.LabelShort).CapitalizeFirst(), pawn, MessageTypeDefOf.ThreatSmall);
			}
		}

		private void Tick_CheckStartMarriageCeremony()
		{
			if (this.pawn.Spawned && !this.pawn.RaceProps.Animal && this.pawn.IsHashIntervalTick(1017))
			{
				int ticksGame = Find.TickManager.TicksGame;
				for (int i = 0; i < this.directRelations.Count; i++)
				{
					float num = (float)((float)(ticksGame - this.directRelations[i].startTicks) / 60000.0);
					if (this.directRelations[i].def == PawnRelationDefOf.Fiance && this.pawn.thingIDNumber < this.directRelations[i].otherPawn.thingIDNumber && num > 10.0 && Rand.MTBEventOccurs(2f, 60000f, 1017f) && this.pawn.Map == this.directRelations[i].otherPawn.Map && this.pawn.Map.IsPlayerHome && MarriageCeremonyUtility.AcceptableGameConditionsToStartCeremony(this.pawn.Map) && MarriageCeremonyUtility.FianceReadyToStartCeremony(this.pawn) && MarriageCeremonyUtility.FianceReadyToStartCeremony(this.directRelations[i].otherPawn))
					{
						this.pawn.Map.lordsStarter.TryStartMarriageCeremony(this.pawn, this.directRelations[i].otherPawn);
					}
				}
			}
		}

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

		private void GainedOrLostDirectRelation()
		{
			if (Current.ProgramState == ProgramState.Playing && !this.pawn.Dead && this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
		}
	}
}
