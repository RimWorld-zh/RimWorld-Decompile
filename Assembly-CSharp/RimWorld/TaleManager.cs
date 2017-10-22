using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public sealed class TaleManager : IExposable
	{
		private List<Tale> tales = new List<Tale>();

		private const int MaxUnusedVolatileTales = 350;

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
				Log.Warning("Tried to remove used tale " + tale);
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
			int num = 0;
			for (int i = 0; i < this.tales.Count; i++)
			{
				if (this.tales[i].def.type == TaleType.Volatile && this.tales[i].Unused)
				{
					num++;
				}
			}
			while (num > 350)
			{
				Tale tale = null;
				float num2 = 3.40282347E+38f;
				for (int j = 0; j < this.tales.Count; j++)
				{
					if (this.tales[j].def.type == TaleType.Volatile && this.tales[j].Unused && this.tales[j].InterestLevel < num2)
					{
						tale = this.tales[j];
						num2 = this.tales[j].InterestLevel;
					}
				}
				this.RemoveTale(tale);
				num--;
			}
		}

		private void CheckCullUnusedTalesWithMaxPerPawnLimit(Tale addedTale)
		{
			if (addedTale.def.maxPerPawn >= 0 && addedTale.DominantPawn != null)
			{
				int num = 0;
				for (int i = 0; i < this.tales.Count; i++)
				{
					if (this.tales[i].Unused && this.tales[i].def == addedTale.def && this.tales[i].DominantPawn == addedTale.DominantPawn)
					{
						num++;
					}
				}
				while (num > addedTale.def.maxPerPawn)
				{
					Tale tale = null;
					int num2 = -1;
					for (int j = 0; j < this.tales.Count; j++)
					{
						if (this.tales[j].Unused && this.tales[j].def == addedTale.def && this.tales[j].DominantPawn == addedTale.DominantPawn && this.tales[j].AgeTicks > num2)
						{
							tale = this.tales[j];
							num2 = this.tales[j].AgeTicks;
						}
					}
					this.RemoveTale(tale);
					num--;
				}
			}
		}

		private void RemoveExpiredTales()
		{
			for (int num = this.tales.Count - 1; num >= 0; num--)
			{
				if (this.tales[num].Expired)
				{
					this.RemoveTale(this.tales[num]);
				}
			}
		}

		public TaleReference GetRandomTaleReferenceForArt(ArtGenerationContext source)
		{
			TaleReference result;
			Tale tale = default(Tale);
			if (source == ArtGenerationContext.Outsider)
			{
				result = TaleReference.Taleless;
			}
			else if (this.tales.Count == 0)
			{
				result = TaleReference.Taleless;
			}
			else if (Rand.Value < 0.25)
			{
				result = TaleReference.Taleless;
			}
			else if (!(from x in this.tales
			where x.def.usableForArt
			select x).TryRandomElementByWeight<Tale>((Func<Tale, float>)((Tale ta) => ta.InterestLevel), out tale))
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
			Tale tale = default(Tale);
			if (this.tales.Count == 0)
			{
				result = TaleReference.Taleless;
			}
			else if (!(from x in this.tales
			where x.def.usableForArt && x.Concerns(th)
			select x).TryRandomElementByWeight<Tale>((Func<Tale, float>)((Tale x) => x.InterestLevel), out tale))
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
			for (int num = this.tales.Count - 1; num >= 0; num--)
			{
				if (this.tales[num].Unused && !this.tales[num].def.usableForArt && this.tales[num].def.type != TaleType.PermanentHistorical && this.tales[num].DominantPawn == pawn)
				{
					this.RemoveTale(this.tales[num]);
				}
			}
		}

		public void Notify_PawnDiscarded(Pawn p, bool silentlyRemoveReferences)
		{
			for (int num = this.tales.Count - 1; num >= 0; num--)
			{
				if (this.tales[num].Concerns(p))
				{
					if (!silentlyRemoveReferences)
					{
						Log.Warning("Discarding pawn " + p + ", but he is referenced by a tale " + this.tales[num] + ".");
					}
					else if (!this.tales[num].Unused)
					{
						Log.Warning("Discarding pawn " + p + ", but he is referenced by an active tale " + this.tales[num] + ".");
					}
					this.RemoveTale(this.tales[num]);
				}
			}
		}

		public bool AnyActiveTaleConcerns(Pawn p)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < this.tales.Count)
				{
					if (!this.tales[num].Unused && this.tales[num].Concerns(p))
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public bool AnyTaleConcerns(Pawn p)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < this.tales.Count)
				{
					if (this.tales[num].Concerns(p))
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}

		public float GetMaxHistoricalTaleDay()
		{
			float num = 0f;
			for (int i = 0; i < this.tales.Count; i++)
			{
				Tale tale = this.tales[i];
				if (tale.def.type == TaleType.PermanentHistorical)
				{
					float num2 = (float)((float)GenDate.TickAbsToGame(tale.date) / 60000.0);
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
			stringBuilder.AppendLine("Used count: " + enumerable.Count());
			stringBuilder.AppendLine("Unused volatile count: " + enumerable2.Count() + " (max: " + 350 + ")");
			stringBuilder.AppendLine("Unused permanent count: " + enumerable3.Count());
			stringBuilder.AppendLine("Unused expirable count: " + enumerable4.Count());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("-------Used-------");
			foreach (Tale item in enumerable)
			{
				stringBuilder.AppendLine(item.ToString());
			}
			stringBuilder.AppendLine("-------Unused volatile-------");
			foreach (Tale item2 in enumerable2)
			{
				stringBuilder.AppendLine(item2.ToString());
			}
			stringBuilder.AppendLine("-------Unused permanent-------");
			foreach (Tale item3 in enumerable3)
			{
				stringBuilder.AppendLine(item3.ToString());
			}
			stringBuilder.AppendLine("-------Unused expirable-------");
			foreach (Tale item4 in enumerable4)
			{
				stringBuilder.AppendLine(item4.ToString());
			}
			Log.Message(stringBuilder.ToString());
		}

		public void LogTaleInterestSummary()
		{
			StringBuilder stringBuilder = new StringBuilder();
			float num = (from t in this.tales
			where t.def.usableForArt
			select t).Sum((Func<Tale, float>)((Tale t) => t.InterestLevel));
			Func<TaleDef, float> defInterest = (Func<TaleDef, float>)((TaleDef def) => (from t in this.tales
			where t.def == def
			select t).Sum((Func<Tale, float>)((Tale t) => t.InterestLevel)));
			foreach (TaleDef item in from def in DefDatabase<TaleDef>.AllDefs
			where def.usableForArt
			orderby defInterest(def) descending
			select def)
			{
				stringBuilder.AppendLine(item.defName + ":   [" + this.tales.Where((Func<Tale, bool>)((Tale t) => t.def == item)).Count() + "]   " + (defInterest(item) / num).ToStringPercent("F2"));
			}
			Log.Message(stringBuilder.ToString());
		}
	}
}
