using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000661 RID: 1633
	public sealed class TaleManager : IExposable
	{
		// Token: 0x04001365 RID: 4965
		private List<Tale> tales = new List<Tale>();

		// Token: 0x04001366 RID: 4966
		private const int MaxUnusedVolatileTales = 350;

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x0600221C RID: 8732 RVA: 0x00121950 File Offset: 0x0011FD50
		public List<Tale> AllTalesListForReading
		{
			get
			{
				return this.tales;
			}
		}

		// Token: 0x0600221D RID: 8733 RVA: 0x0012196B File Offset: 0x0011FD6B
		public void ExposeData()
		{
			Scribe_Collections.Look<Tale>(ref this.tales, "tales", LookMode.Deep, new object[0]);
		}

		// Token: 0x0600221E RID: 8734 RVA: 0x00121985 File Offset: 0x0011FD85
		public void TaleManagerTick()
		{
			this.RemoveExpiredTales();
		}

		// Token: 0x0600221F RID: 8735 RVA: 0x0012198E File Offset: 0x0011FD8E
		public void Add(Tale tale)
		{
			this.tales.Add(tale);
			this.CheckCullTales(tale);
		}

		// Token: 0x06002220 RID: 8736 RVA: 0x001219A4 File Offset: 0x0011FDA4
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

		// Token: 0x06002221 RID: 8737 RVA: 0x001219D6 File Offset: 0x0011FDD6
		private void CheckCullTales(Tale addedTale)
		{
			this.CheckCullUnusedVolatileTales();
			this.CheckCullUnusedTalesWithMaxPerPawnLimit(addedTale);
		}

		// Token: 0x06002222 RID: 8738 RVA: 0x001219E8 File Offset: 0x0011FDE8
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

		// Token: 0x06002223 RID: 8739 RVA: 0x00121B00 File Offset: 0x0011FF00
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

		// Token: 0x06002224 RID: 8740 RVA: 0x00121C7C File Offset: 0x0012007C
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

		// Token: 0x06002225 RID: 8741 RVA: 0x00121CD4 File Offset: 0x001200D4
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

		// Token: 0x06002226 RID: 8742 RVA: 0x00121D98 File Offset: 0x00120198
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

		// Token: 0x06002227 RID: 8743 RVA: 0x00121E2C File Offset: 0x0012022C
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

		// Token: 0x06002228 RID: 8744 RVA: 0x00121ED0 File Offset: 0x001202D0
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

		// Token: 0x06002229 RID: 8745 RVA: 0x00121F84 File Offset: 0x00120384
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

		// Token: 0x0600222A RID: 8746 RVA: 0x00122078 File Offset: 0x00120478
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

		// Token: 0x0600222B RID: 8747 RVA: 0x001220E0 File Offset: 0x001204E0
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

		// Token: 0x0600222C RID: 8748 RVA: 0x00122134 File Offset: 0x00120534
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

		// Token: 0x0600222D RID: 8749 RVA: 0x001221B0 File Offset: 0x001205B0
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

		// Token: 0x0600222E RID: 8750 RVA: 0x001224B8 File Offset: 0x001208B8
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
	}
}
