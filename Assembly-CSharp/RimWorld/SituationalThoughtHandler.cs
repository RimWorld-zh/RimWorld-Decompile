using System;
using System.Collections.Generic;
using UnityEngine.Profiling;
using Verse;

namespace RimWorld
{
	// Token: 0x0200052F RID: 1327
	public sealed class SituationalThoughtHandler
	{
		// Token: 0x04000E85 RID: 3717
		public Pawn pawn;

		// Token: 0x04000E86 RID: 3718
		private List<Thought_Situational> cachedThoughts = new List<Thought_Situational>();

		// Token: 0x04000E87 RID: 3719
		private int lastMoodThoughtsRecalculationTick = -99999;

		// Token: 0x04000E88 RID: 3720
		private Dictionary<Pawn, SituationalThoughtHandler.CachedSocialThoughts> cachedSocialThoughts = new Dictionary<Pawn, SituationalThoughtHandler.CachedSocialThoughts>();

		// Token: 0x04000E89 RID: 3721
		private const int RecalculateStateEveryTicks = 100;

		// Token: 0x04000E8A RID: 3722
		private HashSet<ThoughtDef> tmpCachedThoughts = new HashSet<ThoughtDef>();

		// Token: 0x04000E8B RID: 3723
		private HashSet<ThoughtDef> tmpCachedSocialThoughts = new HashSet<ThoughtDef>();

		// Token: 0x06001885 RID: 6277 RVA: 0x000D77D4 File Offset: 0x000D5BD4
		public SituationalThoughtHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06001886 RID: 6278 RVA: 0x000D7826 File Offset: 0x000D5C26
		public void SituationalThoughtInterval()
		{
			Profiler.BeginSample("SituationalThoughtInterval()");
			this.RemoveExpiredThoughtsFromCache();
			Profiler.EndSample();
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x000D7840 File Offset: 0x000D5C40
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
		}

		// Token: 0x06001888 RID: 6280 RVA: 0x000D7894 File Offset: 0x000D5C94
		public void AppendSocialThoughts(Pawn otherPawn, List<ISocialThought> outThoughts)
		{
			this.CheckRecalculateSocialThoughts(otherPawn);
			SituationalThoughtHandler.CachedSocialThoughts cachedSocialThoughts = this.cachedSocialThoughts[otherPawn];
			cachedSocialThoughts.lastQueryTick = Find.TickManager.TicksGame;
			List<Thought_SituationalSocial> activeThoughts = cachedSocialThoughts.activeThoughts;
			for (int i = 0; i < activeThoughts.Count; i++)
			{
				outThoughts.Add(activeThoughts[i]);
			}
		}

		// Token: 0x06001889 RID: 6281 RVA: 0x000D78F4 File Offset: 0x000D5CF4
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
					while (j < count)
					{
						if (!this.tmpCachedThoughts.Contains(situationalNonSocialThoughtDefs[j]))
						{
							Thought_Situational thought_Situational = this.TryCreateThought(situationalNonSocialThoughtDefs[j]);
							if (thought_Situational != null)
							{
								this.cachedThoughts.Add(thought_Situational);
							}
						}
						j++;
					}
				}
				finally
				{
				}
			}
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x000D79FC File Offset: 0x000D5DFC
		private void CheckRecalculateSocialThoughts(Pawn otherPawn)
		{
			try
			{
				SituationalThoughtHandler.CachedSocialThoughts cachedSocialThoughts;
				if (!this.cachedSocialThoughts.TryGetValue(otherPawn, out cachedSocialThoughts))
				{
					cachedSocialThoughts = new SituationalThoughtHandler.CachedSocialThoughts();
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
					while (j < count)
					{
						if (!this.tmpCachedSocialThoughts.Contains(situationalSocialThoughtDefs[j]))
						{
							Thought_SituationalSocial thought_SituationalSocial2 = this.TryCreateSocialThought(situationalSocialThoughtDefs[j], otherPawn);
							if (thought_SituationalSocial2 != null)
							{
								cachedSocialThoughts.thoughts.Add(thought_SituationalSocial2);
							}
						}
						j++;
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

		// Token: 0x0600188B RID: 6283 RVA: 0x000D7B84 File Offset: 0x000D5F84
		private Thought_Situational TryCreateThought(ThoughtDef def)
		{
			Thought_Situational thought_Situational = null;
			try
			{
				if (!ThoughtUtility.CanGetThought(this.pawn, def))
				{
					return null;
				}
				if (!def.Worker.CurrentState(this.pawn).ActiveFor(def))
				{
					return null;
				}
				thought_Situational = (Thought_Situational)ThoughtMaker.MakeThought(def);
				thought_Situational.pawn = this.pawn;
				thought_Situational.RecalculateState();
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception while recalculating ",
					def,
					" thought state for pawn ",
					this.pawn,
					": ",
					ex
				}), false);
			}
			return thought_Situational;
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x000D7C54 File Offset: 0x000D6054
		private Thought_SituationalSocial TryCreateSocialThought(ThoughtDef def, Pawn otherPawn)
		{
			Thought_SituationalSocial thought_SituationalSocial = null;
			try
			{
				if (!ThoughtUtility.CanGetThought(this.pawn, def))
				{
					return null;
				}
				if (!def.Worker.CurrentSocialState(this.pawn, otherPawn).ActiveFor(def))
				{
					return null;
				}
				thought_SituationalSocial = (Thought_SituationalSocial)ThoughtMaker.MakeThought(def);
				thought_SituationalSocial.pawn = this.pawn;
				thought_SituationalSocial.otherPawn = otherPawn;
				thought_SituationalSocial.RecalculateState();
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception while recalculating ",
					def,
					" thought state for pawn ",
					this.pawn,
					": ",
					ex
				}), false);
			}
			return thought_SituationalSocial;
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x000D7D2C File Offset: 0x000D612C
		public void Notify_SituationalThoughtsDirty()
		{
			this.cachedThoughts.Clear();
			this.cachedSocialThoughts.Clear();
		}

		// Token: 0x0600188E RID: 6286 RVA: 0x000D7D45 File Offset: 0x000D6145
		private void RemoveExpiredThoughtsFromCache()
		{
			this.cachedSocialThoughts.RemoveAll((KeyValuePair<Pawn, SituationalThoughtHandler.CachedSocialThoughts> x) => x.Value.Expired || x.Key.Discarded);
		}

		// Token: 0x02000530 RID: 1328
		private class CachedSocialThoughts
		{
			// Token: 0x04000E8D RID: 3725
			public List<Thought_SituationalSocial> thoughts = new List<Thought_SituationalSocial>();

			// Token: 0x04000E8E RID: 3726
			public List<Thought_SituationalSocial> activeThoughts = new List<Thought_SituationalSocial>();

			// Token: 0x04000E8F RID: 3727
			public int lastRecalculationTick = -99999;

			// Token: 0x04000E90 RID: 3728
			public int lastQueryTick = -99999;

			// Token: 0x04000E91 RID: 3729
			private const int ExpireAfterTicks = 300;

			// Token: 0x17000368 RID: 872
			// (get) Token: 0x06001891 RID: 6289 RVA: 0x000D7DDC File Offset: 0x000D61DC
			public bool Expired
			{
				get
				{
					return Find.TickManager.TicksGame - this.lastQueryTick >= 300;
				}
			}

			// Token: 0x17000369 RID: 873
			// (get) Token: 0x06001892 RID: 6290 RVA: 0x000D7E0C File Offset: 0x000D620C
			public bool ShouldRecalculateState
			{
				get
				{
					return Find.TickManager.TicksGame - this.lastRecalculationTick >= 100;
				}
			}
		}
	}
}
