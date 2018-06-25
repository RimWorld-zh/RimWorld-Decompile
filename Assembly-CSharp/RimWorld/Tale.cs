using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000667 RID: 1639
	public class Tale : IExposable, ILoadReferenceable
	{
		// Token: 0x04001380 RID: 4992
		public TaleDef def;

		// Token: 0x04001381 RID: 4993
		public int id;

		// Token: 0x04001382 RID: 4994
		private int uses = 0;

		// Token: 0x04001383 RID: 4995
		public int date = -1;

		// Token: 0x04001384 RID: 4996
		public TaleData_Surroundings surroundings;

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06002245 RID: 8773 RVA: 0x001233AC File Offset: 0x001217AC
		public int AgeTicks
		{
			get
			{
				return Find.TickManager.TicksAbs - this.date;
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06002246 RID: 8774 RVA: 0x001233D4 File Offset: 0x001217D4
		public int Uses
		{
			get
			{
				return this.uses;
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06002247 RID: 8775 RVA: 0x001233F0 File Offset: 0x001217F0
		public bool Unused
		{
			get
			{
				return this.uses == 0;
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06002248 RID: 8776 RVA: 0x00123410 File Offset: 0x00121810
		public virtual Pawn DominantPawn
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06002249 RID: 8777 RVA: 0x00123428 File Offset: 0x00121828
		public float InterestLevel
		{
			get
			{
				float num = this.def.baseInterest;
				num /= (float)(1 + this.uses * 3);
				float a = 0f;
				TaleType type = this.def.type;
				if (type != TaleType.Volatile)
				{
					if (type != TaleType.PermanentHistorical)
					{
						if (type == TaleType.Expirable)
						{
							a = this.def.expireDays;
						}
					}
					else
					{
						a = 50f;
					}
				}
				else
				{
					a = 50f;
				}
				float value = (float)(this.AgeTicks / 60000);
				num *= Mathf.InverseLerp(a, 0f, value);
				if (num < 0.01f)
				{
					num = 0.01f;
				}
				return num;
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x0600224A RID: 8778 RVA: 0x001234DC File Offset: 0x001218DC
		public bool Expired
		{
			get
			{
				return this.Unused && this.def.type == TaleType.Expirable && (float)this.AgeTicks > this.def.expireDays * 60000f;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x0600224B RID: 8779 RVA: 0x00123538 File Offset: 0x00121938
		public virtual string ShortSummary
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x0600224C RID: 8780 RVA: 0x00123558 File Offset: 0x00121958
		public virtual void GenerateTestData()
		{
			if (Find.CurrentMap == null)
			{
				Log.Error("Can't generate test data because there is no map.", false);
			}
			this.date = Rand.Range(-108000000, -7200000);
			this.surroundings = TaleData_Surroundings.GenerateRandom(Find.CurrentMap);
		}

		// Token: 0x0600224D RID: 8781 RVA: 0x00123598 File Offset: 0x00121998
		public virtual bool Concerns(Thing th)
		{
			return false;
		}

		// Token: 0x0600224E RID: 8782 RVA: 0x001235B0 File Offset: 0x001219B0
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<TaleDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_Values.Look<int>(ref this.uses, "uses", 0, false);
			Scribe_Values.Look<int>(ref this.date, "date", 0, false);
			Scribe_Deep.Look<TaleData_Surroundings>(ref this.surroundings, "surroundings", new object[0]);
		}

		// Token: 0x0600224F RID: 8783 RVA: 0x0012361A File Offset: 0x00121A1A
		public void Notify_NewlyUsed()
		{
			this.uses++;
		}

		// Token: 0x06002250 RID: 8784 RVA: 0x0012362B File Offset: 0x00121A2B
		public void Notify_ReferenceDestroyed()
		{
			if (this.uses == 0)
			{
				Log.Warning("Called reference destroyed method on tale " + this + " but uses count is 0.", false);
			}
			else
			{
				this.uses--;
			}
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x00123664 File Offset: 0x00121A64
		public IEnumerable<RulePack> GetTextGenerationIncludes()
		{
			if (this.def.rulePack != null)
			{
				yield return this.def.rulePack;
			}
			yield break;
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x00123690 File Offset: 0x00121A90
		public IEnumerable<Rule> GetTextGenerationRules()
		{
			Vector2 location = Vector2.zero;
			if (this.surroundings != null && this.surroundings.tile >= 0)
			{
				location = Find.WorldGrid.LongLatOf(this.surroundings.tile);
			}
			yield return new Rule_String("DATE", GenDate.DateFullStringAt((long)this.date, location));
			if (this.surroundings != null)
			{
				foreach (Rule r in this.surroundings.GetRules())
				{
					yield return r;
				}
			}
			foreach (Rule r2 in this.SpecialTextGenerationRules())
			{
				yield return r2;
			}
			yield break;
		}

		// Token: 0x06002253 RID: 8787 RVA: 0x001236BC File Offset: 0x00121ABC
		protected virtual IEnumerable<Rule> SpecialTextGenerationRules()
		{
			yield break;
		}

		// Token: 0x06002254 RID: 8788 RVA: 0x001236E0 File Offset: 0x00121AE0
		public string GetUniqueLoadID()
		{
			return "Tale_" + this.id;
		}

		// Token: 0x06002255 RID: 8789 RVA: 0x0012370C File Offset: 0x00121B0C
		public override int GetHashCode()
		{
			return this.id;
		}

		// Token: 0x06002256 RID: 8790 RVA: 0x00123728 File Offset: 0x00121B28
		public override string ToString()
		{
			string str = string.Concat(new object[]
			{
				"(#",
				this.id,
				": ",
				this.ShortSummary,
				"(age=",
				((float)this.AgeTicks / 60000f).ToString("F2"),
				" interest=",
				this.InterestLevel
			});
			if (this.Unused && this.def.type == TaleType.Expirable)
			{
				str = str + ", expireDays=" + this.def.expireDays.ToString("F2");
			}
			return str + ")";
		}
	}
}
