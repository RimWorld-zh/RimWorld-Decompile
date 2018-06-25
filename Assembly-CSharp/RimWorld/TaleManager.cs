using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	public sealed class TaleManager : IExposable
	{
		private List<Tale> tales = new List<Tale>();

		private const int MaxUnusedVolatileTales = 350;

		[CompilerGenerated]
		private static Func<Tale, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Tale, float> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<Tale, float> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<Tale, bool> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<Tale, bool> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<Tale, bool> <>f__am$cache5;

		[CompilerGenerated]
		private static Func<Tale, bool> <>f__am$cache6;

		[CompilerGenerated]
		private static Func<Tale, bool> <>f__am$cache7;

		[CompilerGenerated]
		private static Func<Tale, float> <>f__am$cache8;

		[CompilerGenerated]
		private static Func<TaleDef, bool> <>f__am$cache9;

		public TaleManager()
		{
		}

		public List<Tale> AllTalesListForReading
		{
			get
			{
				return this.tales;
			}
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<Tale>(ref this.tales, "tales", LookMode.Deep, new object[0]);
		}

		public void TaleManagerTick()
		{
			this.RemoveExpiredTales();
		}

		public void Add(Tale tale)
		{
			this.tales.Add(tale);
			this.CheckCullTales(tale);
		}

		private void RemoveTale(Tale tale)
		{
			if (!tale.Unused)
			{
				Log.Warning("Tried to remove used tale " + tale, false);
			}
			else
			{
				this.tales.Remove(tale);
			}
		}

		private void CheckCullTales(Tale addedTale)
		{
			this.CheckCullUnusedVolatileTales();
			this.CheckCullUnusedTalesWithMaxPerPawnLimit(addedTale);
		}

		private void CheckCullUnusedVolatileTales()
		{
			int i = 0;
			for (int j = 0; j < this.tales.Count; j++)
			{
				if (this.tales[j].def.type == TaleType.Volatile && this.tales[j].Unused)
				{
					i++;
				}
			}
			while (i > 350)
			{
				Tale tale = null;
				float num = float.MaxValue;
				for (int k = 0; k < this.tales.Count; k++)
				{
					if (this.tales[k].def.type == TaleType.Volatile && this.tales[k].Unused && this.tales[k].InterestLevel < num)
					{
						tale = this.tales[k];
						num = this.tales[k].InterestLevel;
					}
				}
				this.RemoveTale(tale);
				i--;
			}
		}

		private void CheckCullUnusedTalesWithMaxPerPawnLimit(Tale addedTale)
		{
			if (addedTale.def.maxPerPawn >= 0)
			{
				if (addedTale.DominantPawn != null)
				{
					int i = 0;
					for (int j = 0; j < this.tales.Count; j++)
					{
						if (this.tales[j].Unused && this.tales[j].def == addedTale.def && this.tales[j].DominantPawn == addedTale.DominantPawn)
						{
							i++;
						}
					}
					while (i > addedTale.def.maxPerPawn)
					{
						Tale tale = null;
						int num = -1;
						for (int k = 0; k < this.tales.Count; k++)
						{
							if (this.tales[k].Unused && this.tales[k].def == addedTale.def && this.tales[k].DominantPawn == addedTale.DominantPawn && this.tales[k].AgeTicks > num)
							{
								tale = this.tales[k];
								num = this.tales[k].AgeTicks;
							}
						}
						this.RemoveTale(tale);
						i--;
					}
				}
			}
		}

		private void RemoveExpiredTales()
		{
			for (int i = this.tales.Count - 1; i >= 0; i--)
			{
				if (this.tales[i].Expired)
				{
					this.RemoveTale(this.tales[i]);
				}
			}
		}

		public TaleReference GetRandomTaleReferenceForArt(ArtGenerationContext source)
		{
			TaleReference result;
			Tale tale;
			if (source == ArtGenerationContext.Outsider)
			{
				result = TaleReference.Taleless;
			}
			else if (this.tales.Count == 0)
			{
				result = TaleReference.Taleless;
			}
			else if (Rand.Value < 0.25f)
			{
				result = TaleReference.Taleless;
			}
			else if (!(from x in this.tales
			where x.def.usableForArt
			select x).TryRandomElementByWeight((Tale ta) => ta.InterestLevel, out tale))
			{
				result = TaleReference.Taleless;
			}
			else
			{
				tale.Notify_NewlyUsed();
				result = new TaleReference(tale);
			}
			return result;
		}

		public TaleReference GetRandomTaleReferenceForArtConcerning(Thing th)
		{
			TaleReference result;
			Tale tale;
			if (this.tales.Count == 0)
			{
				result = TaleReference.Taleless;
			}
			else if (!(from x in this.tales
			where x.def.usableForArt && x.Concerns(th)
			select x).TryRandomElementByWeight((Tale x) => x.InterestLevel, out tale))
			{
				result = TaleReference.Taleless;
			}
			else
			{
				tale.Notify_NewlyUsed();
				result = new TaleReference(tale);
			}
			return result;
		}

		public Tale GetLatestTale(TaleDef def, Pawn pawn)
		{
			Tale tale = null;
			int num = 0;
			for (int i = 0; i < this.tales.Count; i++)
			{
				if (this.tales[i].def == def && this.tales[i].DominantPawn == pawn && (tale == null || this.tales[i].AgeTicks < num))
				{
					tale = this.tales[i];
					num = this.tales[i].AgeTicks;
				}
			}
			return tale;
		}

		public void Notify_PawnDestroyed(Pawn pawn)
		{
			for (int i = this.tales.Count - 1; i >= 0; i--)
			{
				if (this.tales[i].Unused)
				{
					if (!this.tales[i].def.usableForArt)
					{
						if (this.tales[i].def.type != TaleType.PermanentHistorical)
						{
							if (this.tales[i].DominantPawn == pawn)
							{
								this.RemoveTale(this.tales[i]);
							}
						}
					}
				}
			}
		}

		public void Notify_PawnDiscarded(Pawn p, bool silentlyRemoveReferences)
		{
			for (int i = this.tales.Count - 1; i >= 0; i--)
			{
				if (this.tales[i].Concerns(p))
				{
					if (!silentlyRemoveReferences)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Discarding pawn ",
							p,
							", but he is referenced by a tale ",
							this.tales[i],
							"."
						}), false);
					}
					else if (!this.tales[i].Unused)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Discarding pawn ",
							p,
							", but he is referenced by an active tale ",
							this.tales[i],
							"."
						}), false);
					}
					this.RemoveTale(this.tales[i]);
				}
			}
		}

		public bool AnyActiveTaleConcerns(Pawn p)
		{
			for (int i = 0; i < this.tales.Count; i++)
			{
				if (!this.tales[i].Unused && this.tales[i].Concerns(p))
				{
					return true;
				}
			}
			return false;
		}

		public bool AnyTaleConcerns(Pawn p)
		{
			for (int i = 0; i < this.tales.Count; i++)
			{
				if (this.tales[i].Concerns(p))
				{
					return true;
				}
			}
			return false;
		}

		public float GetMaxHistoricalTaleDay()
		{
			float num = 0f;
			for (int i = 0; i < this.tales.Count; i++)
			{
				Tale tale = this.tales[i];
				if (tale.def.type == TaleType.PermanentHistorical)
				{
					float num2 = (float)GenDate.TickAbsToGame(tale.date) / 60000f;
					if (num2 > num)
					{
						num = num2;
					}
				}
			}
			return num;
		}

		public void LogTales()
		{
			StringBuilder stringBuilder = new StringBuilder();
			IEnumerable<Tale> enumerable = from x in this.tales
			where !x.Unused
			select x;
			IEnumerable<Tale> enumerable2 = from x in this.tales
			where x.def.type == TaleType.Volatile && x.Unused
			select x;
			IEnumerable<Tale> enumerable3 = from x in this.tales
			where x.def.type == TaleType.PermanentHistorical && x.Unused
			select x;
			IEnumerable<Tale> enumerable4 = from x in this.tales
			where x.def.type == TaleType.Expirable && x.Unused
			select x;
			stringBuilder.AppendLine("All tales count: " + this.tales.Count);
			stringBuilder.AppendLine("Used count: " + enumerable.Count<Tale>());
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Unused volatile count: ",
				enumerable2.Count<Tale>(),
				" (max: ",
				350,
				")"
			}));
			stringBuilder.AppendLine("Unused permanent count: " + enumerable3.Count<Tale>());
			stringBuilder.AppendLine("Unused expirable count: " + enumerable4.Count<Tale>());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("-------Used-------");
			foreach (Tale tale in enumerable)
			{
				stringBuilder.AppendLine(tale.ToString());
			}
			stringBuilder.AppendLine("-------Unused volatile-------");
			foreach (Tale tale2 in enumerable2)
			{
				stringBuilder.AppendLine(tale2.ToString());
			}
			stringBuilder.AppendLine("-------Unused permanent-------");
			foreach (Tale tale3 in enumerable3)
			{
				stringBuilder.AppendLine(tale3.ToString());
			}
			stringBuilder.AppendLine("-------Unused expirable-------");
			foreach (Tale tale4 in enumerable4)
			{
				stringBuilder.AppendLine(tale4.ToString());
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		public void LogTaleInterestSummary()
		{
			StringBuilder stringBuilder = new StringBuilder();
			float num = (from t in this.tales
			where t.def.usableForArt
			select t).Sum((Tale t) => t.InterestLevel);
			Func<TaleDef, float> defInterest = (TaleDef def) => (from t in this.tales
			where t.def == def
			select t).Sum((Tale t) => t.InterestLevel);
			using (IEnumerator<TaleDef> enumerator = (from def in DefDatabase<TaleDef>.AllDefs
			where def.usableForArt
			orderby defInterest(def) descending
			select def).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TaleDef def = enumerator.Current;
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						def.defName,
						":   [",
						(from t in this.tales
						where t.def == def
						select t).Count<Tale>(),
						"]   ",
						(defInterest(def) / num).ToStringPercent("F2")
					}));
				}
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		[CompilerGenerated]
		private static bool <GetRandomTaleReferenceForArt>m__0(Tale x)
		{
			return x.def.usableForArt;
		}

		[CompilerGenerated]
		private static float <GetRandomTaleReferenceForArt>m__1(Tale ta)
		{
			return ta.InterestLevel;
		}

		[CompilerGenerated]
		private static float <GetRandomTaleReferenceForArtConcerning>m__2(Tale x)
		{
			return x.InterestLevel;
		}

		[CompilerGenerated]
		private static bool <LogTales>m__3(Tale x)
		{
			return !x.Unused;
		}

		[CompilerGenerated]
		private static bool <LogTales>m__4(Tale x)
		{
			return x.def.type == TaleType.Volatile && x.Unused;
		}

		[CompilerGenerated]
		private static bool <LogTales>m__5(Tale x)
		{
			return x.def.type == TaleType.PermanentHistorical && x.Unused;
		}

		[CompilerGenerated]
		private static bool <LogTales>m__6(Tale x)
		{
			return x.def.type == TaleType.Expirable && x.Unused;
		}

		[CompilerGenerated]
		private static bool <LogTaleInterestSummary>m__7(Tale t)
		{
			return t.def.usableForArt;
		}

		[CompilerGenerated]
		private static float <LogTaleInterestSummary>m__8(Tale t)
		{
			return t.InterestLevel;
		}

		[CompilerGenerated]
		private static bool <LogTaleInterestSummary>m__9(TaleDef def)
		{
			return def.usableForArt;
		}

		[CompilerGenerated]
		private sealed class <GetRandomTaleReferenceForArtConcerning>c__AnonStorey0
		{
			internal Thing th;

			public <GetRandomTaleReferenceForArtConcerning>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Tale x)
			{
				return x.def.usableForArt && x.Concerns(this.th);
			}
		}

		[CompilerGenerated]
		private sealed class <LogTaleInterestSummary>c__AnonStorey2
		{
			internal Func<TaleDef, float> defInterest;

			internal TaleManager $this;

			private static Func<Tale, float> <>f__am$cache0;

			public <LogTaleInterestSummary>c__AnonStorey2()
			{
			}

			internal float <>m__0(TaleDef def)
			{
				return (from t in this.$this.tales
				where t.def == def
				select t).Sum((Tale t) => t.InterestLevel);
			}

			internal float <>m__1(TaleDef def)
			{
				return this.defInterest(def);
			}

			private static float <>m__2(Tale t)
			{
				return t.InterestLevel;
			}

			private sealed class <LogTaleInterestSummary>c__AnonStorey1
			{
				internal TaleDef def;

				internal TaleManager.<LogTaleInterestSummary>c__AnonStorey2 <>f__ref$2;

				public <LogTaleInterestSummary>c__AnonStorey1()
				{
				}

				internal bool <>m__0(Tale t)
				{
					return t.def == this.def;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <LogTaleInterestSummary>c__AnonStorey3
		{
			internal TaleDef def;

			public <LogTaleInterestSummary>c__AnonStorey3()
			{
			}

			internal bool <>m__0(Tale t)
			{
				return t.def == this.def;
			}
		}
	}
}
