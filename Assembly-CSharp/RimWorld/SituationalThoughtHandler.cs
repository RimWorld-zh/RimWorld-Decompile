using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public sealed class SituationalThoughtHandler
	{
		private class CachedSocialThoughts
		{
			public List<Thought_SituationalSocial> thoughts = new List<Thought_SituationalSocial>();

			public List<Thought_SituationalSocial> activeThoughts = new List<Thought_SituationalSocial>();

			public int lastRecalculationTick = -99999;

			public int lastQueryTick = -99999;

			private const int ExpireAfterTicks = 300;

			public bool Expired
			{
				get
				{
					return Find.TickManager.TicksGame - this.lastQueryTick >= 300;
				}
			}

			public bool ShouldRecalculateState
			{
				get
				{
					return Find.TickManager.TicksGame - this.lastRecalculationTick >= 100;
				}
			}
		}

		public Pawn pawn;

		private List<Thought_Situational> cachedThoughts = new List<Thought_Situational>();

		private int lastMoodThoughtsRecalculationTick = -99999;

		private Dictionary<Pawn, CachedSocialThoughts> cachedSocialThoughts = new Dictionary<Pawn, CachedSocialThoughts>();

		private Dictionary<Pawn, CachedSocialThoughts> cachedSocialThoughtsAffectingMood = new Dictionary<Pawn, CachedSocialThoughts>();

		private const int RecalculateStateEveryTicks = 100;

		private HashSet<ThoughtDef> tmpCachedThoughts = new HashSet<ThoughtDef>();

		private HashSet<Pair<ThoughtDef, Pawn>> tmpToAdd = new HashSet<Pair<ThoughtDef, Pawn>>();

		private HashSet<ThoughtDef> tmpCachedSocialThoughts = new HashSet<ThoughtDef>();

		public SituationalThoughtHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public void SituationalThoughtInterval()
		{
			this.RemoveExpiredThoughtsFromCache();
		}

		public void AppendMoodThoughts(List<Thought> outThoughts)
		{
			this.CheckRecalculateMoodThoughts();
			for (int i = 0; i < this.cachedThoughts.Count; i++)
			{
				Thought_Situational thought_Situational = this.cachedThoughts[i];
				if (thought_Situational.Active)
				{
					outThoughts.Add(thought_Situational);
				}
			}
			int ticksGame = Find.TickManager.TicksGame;
			foreach (KeyValuePair<Pawn, CachedSocialThoughts> item in this.cachedSocialThoughtsAffectingMood)
			{
				item.Value.lastQueryTick = ticksGame;
				List<Thought_SituationalSocial> activeThoughts = item.Value.activeThoughts;
				for (int j = 0; j < activeThoughts.Count; j++)
				{
					outThoughts.Add(activeThoughts[j]);
				}
			}
		}

		public void AppendSocialThoughts(Pawn otherPawn, List<ISocialThought> outThoughts)
		{
			this.CheckRecalculateSocialThoughts(otherPawn);
			CachedSocialThoughts cachedSocialThoughts = this.cachedSocialThoughts[otherPawn];
			cachedSocialThoughts.lastQueryTick = Find.TickManager.TicksGame;
			List<Thought_SituationalSocial> activeThoughts = cachedSocialThoughts.activeThoughts;
			for (int i = 0; i < activeThoughts.Count; i++)
			{
				outThoughts.Add(activeThoughts[i]);
			}
		}

		private void CheckRecalculateMoodThoughts()
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (ticksGame - this.lastMoodThoughtsRecalculationTick >= 100)
			{
				this.lastMoodThoughtsRecalculationTick = ticksGame;
				try
				{
					this.tmpCachedThoughts.Clear();
					for (int i = 0; i < this.cachedThoughts.Count; i++)
					{
						this.cachedThoughts[i].RecalculateState();
						this.tmpCachedThoughts.Add(this.cachedThoughts[i].def);
					}
					List<ThoughtDef> situationalNonSocialThoughtDefs = ThoughtUtility.situationalNonSocialThoughtDefs;
					int j = 0;
					int count = situationalNonSocialThoughtDefs.Count;
					for (; j < count; j++)
					{
						if (!this.tmpCachedThoughts.Contains(situationalNonSocialThoughtDefs[j]))
						{
							Thought_Situational thought_Situational = this.TryCreateThought(situationalNonSocialThoughtDefs[j]);
							if (thought_Situational != null)
							{
								this.cachedThoughts.Add(thought_Situational);
							}
						}
					}
					this.RecalculateSocialThoughtsAffectingMood();
				}
				finally
				{
				}
			}
		}

		private void RecalculateSocialThoughtsAffectingMood()
		{
			try
			{
				this.tmpToAdd.Clear();
				List<ThoughtDef> situationalSocialThoughtDefs = ThoughtUtility.situationalSocialThoughtDefs;
				int i = 0;
				int count = situationalSocialThoughtDefs.Count;
				for (; i < count; i++)
				{
					if (situationalSocialThoughtDefs[i].socialThoughtAffectingMood)
					{
						foreach (Pawn item in situationalSocialThoughtDefs[i].Worker.PotentialPawnCandidates(this.pawn))
						{
							if (item != this.pawn)
							{
								this.tmpToAdd.Add(new Pair<ThoughtDef, Pawn>(situationalSocialThoughtDefs[i], item));
							}
						}
					}
				}
				foreach (KeyValuePair<Pawn, CachedSocialThoughts> item2 in this.cachedSocialThoughtsAffectingMood)
				{
					List<Thought_SituationalSocial> thoughts = item2.Value.thoughts;
					for (int num = thoughts.Count - 1; num >= 0; num--)
					{
						if (!this.tmpToAdd.Contains(new Pair<ThoughtDef, Pawn>(thoughts[num].def, item2.Key)))
						{
							thoughts.RemoveAt(num);
						}
					}
				}
				foreach (Pair<ThoughtDef, Pawn> item3 in this.tmpToAdd)
				{
					ThoughtDef first = item3.First;
					Pawn second = item3.Second;
					CachedSocialThoughts cachedSocialThoughts = default(CachedSocialThoughts);
					bool flag = this.cachedSocialThoughtsAffectingMood.TryGetValue(second, out cachedSocialThoughts);
					if (flag)
					{
						bool flag2 = false;
						int num2 = 0;
						while (num2 < cachedSocialThoughts.thoughts.Count)
						{
							if (cachedSocialThoughts.thoughts[num2].def != first)
							{
								num2++;
								continue;
							}
							flag2 = true;
							break;
						}
						if (!flag2)
							goto IL_01e5;
						continue;
					}
					goto IL_01e5;
					IL_01e5:
					Thought_SituationalSocial thought_SituationalSocial = this.TryCreateSocialThought(first, second);
					if (thought_SituationalSocial != null)
					{
						if (!flag)
						{
							cachedSocialThoughts = new CachedSocialThoughts();
							this.cachedSocialThoughtsAffectingMood.Add(second, cachedSocialThoughts);
						}
						cachedSocialThoughts.thoughts.Add(thought_SituationalSocial);
					}
				}
				this.cachedSocialThoughtsAffectingMood.RemoveAll((KeyValuePair<Pawn, CachedSocialThoughts> x) => x.Value.thoughts.Count == 0);
				int ticksGame = Find.TickManager.TicksGame;
				foreach (KeyValuePair<Pawn, CachedSocialThoughts> item4 in this.cachedSocialThoughtsAffectingMood)
				{
					CachedSocialThoughts value = item4.Value;
					List<Thought_SituationalSocial> thoughts2 = value.thoughts;
					value.activeThoughts.Clear();
					for (int j = 0; j < thoughts2.Count; j++)
					{
						thoughts2[j].RecalculateState();
						value.lastRecalculationTick = ticksGame;
						if (thoughts2[j].Active)
						{
							value.activeThoughts.Add(thoughts2[j]);
						}
					}
				}
			}
			finally
			{
			}
		}

		private void CheckRecalculateSocialThoughts(Pawn otherPawn)
		{
			try
			{
				CachedSocialThoughts cachedSocialThoughts = default(CachedSocialThoughts);
				if (!this.cachedSocialThoughts.TryGetValue(otherPawn, out cachedSocialThoughts))
				{
					cachedSocialThoughts = new CachedSocialThoughts();
					this.cachedSocialThoughts.Add(otherPawn, cachedSocialThoughts);
				}
				if (cachedSocialThoughts.ShouldRecalculateState)
				{
					cachedSocialThoughts.lastRecalculationTick = Find.TickManager.TicksGame;
					this.tmpCachedSocialThoughts.Clear();
					for (int i = 0; i < cachedSocialThoughts.thoughts.Count; i++)
					{
						Thought_SituationalSocial thought_SituationalSocial = cachedSocialThoughts.thoughts[i];
						thought_SituationalSocial.RecalculateState();
						this.tmpCachedSocialThoughts.Add(thought_SituationalSocial.def);
					}
					List<ThoughtDef> situationalSocialThoughtDefs = ThoughtUtility.situationalSocialThoughtDefs;
					int j = 0;
					int count = situationalSocialThoughtDefs.Count;
					for (; j < count; j++)
					{
						if (!this.tmpCachedSocialThoughts.Contains(situationalSocialThoughtDefs[j]))
						{
							Thought_SituationalSocial thought_SituationalSocial2 = this.TryCreateSocialThought(situationalSocialThoughtDefs[j], otherPawn);
							if (thought_SituationalSocial2 != null)
							{
								cachedSocialThoughts.thoughts.Add(thought_SituationalSocial2);
							}
						}
					}
					cachedSocialThoughts.activeThoughts.Clear();
					for (int k = 0; k < cachedSocialThoughts.thoughts.Count; k++)
					{
						Thought_SituationalSocial thought_SituationalSocial3 = cachedSocialThoughts.thoughts[k];
						if (thought_SituationalSocial3.Active)
						{
							cachedSocialThoughts.activeThoughts.Add(thought_SituationalSocial3);
						}
					}
				}
			}
			finally
			{
			}
		}

		private Thought_Situational TryCreateThought(ThoughtDef def)
		{
			Thought_Situational thought_Situational = null;
			try
			{
				if (!ThoughtUtility.CanGetThought(this.pawn, def))
				{
					return null;
				}
				if (!def.Worker.CurrentState(this.pawn).Active)
				{
					return null;
				}
				thought_Situational = (Thought_Situational)ThoughtMaker.MakeThought(def);
				thought_Situational.pawn = this.pawn;
				thought_Situational.RecalculateState();
				return thought_Situational;
			}
			catch (Exception ex)
			{
				Log.Error("Exception while recalculating " + def + " thought state for pawn " + this.pawn + ": " + ex);
				return thought_Situational;
			}
		}

		private Thought_SituationalSocial TryCreateSocialThought(ThoughtDef def, Pawn otherPawn)
		{
			Thought_SituationalSocial thought_SituationalSocial = null;
			try
			{
				if (!ThoughtUtility.CanGetThought(this.pawn, def))
				{
					return null;
				}
				if (!def.Worker.CurrentSocialState(this.pawn, otherPawn).Active)
				{
					return null;
				}
				thought_SituationalSocial = (Thought_SituationalSocial)ThoughtMaker.MakeThought(def);
				thought_SituationalSocial.pawn = this.pawn;
				thought_SituationalSocial.otherPawn = otherPawn;
				thought_SituationalSocial.RecalculateState();
				return thought_SituationalSocial;
			}
			catch (Exception ex)
			{
				Log.Error("Exception while recalculating " + def + " thought state for pawn " + this.pawn + ": " + ex);
				return thought_SituationalSocial;
			}
		}

		public void Notify_SituationalThoughtsDirty()
		{
			this.cachedThoughts.Clear();
			this.cachedSocialThoughts.Clear();
			this.cachedSocialThoughtsAffectingMood.Clear();
		}

		private void RemoveExpiredThoughtsFromCache()
		{
			this.cachedSocialThoughts.RemoveAll((KeyValuePair<Pawn, CachedSocialThoughts> x) => x.Value.Expired || x.Key.Discarded);
			this.cachedSocialThoughtsAffectingMood.RemoveAll((KeyValuePair<Pawn, CachedSocialThoughts> x) => x.Value.Expired || x.Key.Discarded);
		}
	}
}
