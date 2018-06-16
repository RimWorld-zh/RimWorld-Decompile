using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000669 RID: 1641
	public class Tale : IExposable, ILoadReferenceable
	{
		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06002248 RID: 8776 RVA: 0x00122E44 File Offset: 0x00121244
		public int AgeTicks
		{
			get
			{
				return Find.TickManager.TicksAbs - this.date;
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06002249 RID: 8777 RVA: 0x00122E6C File Offset: 0x0012126C
		public int Uses
		{
			get
			{
				return this.uses;
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x0600224A RID: 8778 RVA: 0x00122E88 File Offset: 0x00121288
		public bool Unused
		{
			get
			{
				return this.uses == 0;
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x0600224B RID: 8779 RVA: 0x00122EA8 File Offset: 0x001212A8
		public virtual Pawn DominantPawn
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x0600224C RID: 8780 RVA: 0x00122EC0 File Offset: 0x001212C0
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
		// (get) Token: 0x0600224D RID: 8781 RVA: 0x00122F74 File Offset: 0x00121374
		public bool Expired
		{
			get
			{
				return this.Unused && this.def.type == TaleType.Expirable && (float)this.AgeTicks > this.def.expireDays * 60000f;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x0600224E RID: 8782 RVA: 0x00122FD0 File Offset: 0x001213D0
		public virtual string ShortSummary
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x0600224F RID: 8783 RVA: 0x00122FF0 File Offset: 0x001213F0
		public virtual void GenerateTestData()
		{
			if (Find.CurrentMap == null)
			{
				Log.Error("Can't generate test data because there is no map.", false);
			}
			this.date = Rand.Range(-108000000, -7200000);
			this.surroundings = TaleData_Surroundings.GenerateRandom(Find.CurrentMap);
		}

		// Token: 0x06002250 RID: 8784 RVA: 0x00123030 File Offset: 0x00121430
		public virtual bool Concerns(Thing th)
		{
			return false;
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x00123048 File Offset: 0x00121448
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<TaleDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_Values.Look<int>(ref this.uses, "uses", 0, false);
			Scribe_Values.Look<int>(ref this.date, "date", 0, false);
			Scribe_Deep.Look<TaleData_Surroundings>(ref this.surroundings, "surroundings", new object[0]);
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x001230B2 File Offset: 0x001214B2
		public void Notify_NewlyUsed()
		{
			this.uses++;
		}

		// Token: 0x06002253 RID: 8787 RVA: 0x001230C3 File Offset: 0x001214C3
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

		// Token: 0x06002254 RID: 8788 RVA: 0x001230FC File Offset: 0x001214FC
		public IEnumerable<RulePack> GetTextGenerationIncludes()
		{
			if (this.def.rulePack != null)
			{
				yield return this.def.rulePack;
			}
			yield break;
		}

		// Token: 0x06002255 RID: 8789 RVA: 0x00123128 File Offset: 0x00121528
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

		// Token: 0x06002256 RID: 8790 RVA: 0x00123154 File Offset: 0x00121554
		protected virtual IEnumerable<Rule> SpecialTextGenerationRules()
		{
			yield break;
		}

		// Token: 0x06002257 RID: 8791 RVA: 0x00123178 File Offset: 0x00121578
		public string GetUniqueLoadID()
		{
			return "Tale_" + this.id;
		}

		// Token: 0x06002258 RID: 8792 RVA: 0x001231A4 File Offset: 0x001215A4
		public override int GetHashCode()
		{
			return this.id;
		}

		// Token: 0x06002259 RID: 8793 RVA: 0x001231C0 File Offset: 0x001215C0
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

		// Token: 0x0400137E RID: 4990
		public TaleDef def;

		// Token: 0x0400137F RID: 4991
		public int id;

		// Token: 0x04001380 RID: 4992
		private int uses = 0;

		// Token: 0x04001381 RID: 4993
		public int date = -1;

		// Token: 0x04001382 RID: 4994
		public TaleData_Surroundings surroundings;
	}
}
