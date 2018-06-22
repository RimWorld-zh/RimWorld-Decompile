using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000665 RID: 1637
	public class Tale : IExposable, ILoadReferenceable
	{
		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06002242 RID: 8770 RVA: 0x00122FF4 File Offset: 0x001213F4
		public int AgeTicks
		{
			get
			{
				return Find.TickManager.TicksAbs - this.date;
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06002243 RID: 8771 RVA: 0x0012301C File Offset: 0x0012141C
		public int Uses
		{
			get
			{
				return this.uses;
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06002244 RID: 8772 RVA: 0x00123038 File Offset: 0x00121438
		public bool Unused
		{
			get
			{
				return this.uses == 0;
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06002245 RID: 8773 RVA: 0x00123058 File Offset: 0x00121458
		public virtual Pawn DominantPawn
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06002246 RID: 8774 RVA: 0x00123070 File Offset: 0x00121470
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
		// (get) Token: 0x06002247 RID: 8775 RVA: 0x00123124 File Offset: 0x00121524
		public bool Expired
		{
			get
			{
				return this.Unused && this.def.type == TaleType.Expirable && (float)this.AgeTicks > this.def.expireDays * 60000f;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06002248 RID: 8776 RVA: 0x00123180 File Offset: 0x00121580
		public virtual string ShortSummary
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x06002249 RID: 8777 RVA: 0x001231A0 File Offset: 0x001215A0
		public virtual void GenerateTestData()
		{
			if (Find.CurrentMap == null)
			{
				Log.Error("Can't generate test data because there is no map.", false);
			}
			this.date = Rand.Range(-108000000, -7200000);
			this.surroundings = TaleData_Surroundings.GenerateRandom(Find.CurrentMap);
		}

		// Token: 0x0600224A RID: 8778 RVA: 0x001231E0 File Offset: 0x001215E0
		public virtual bool Concerns(Thing th)
		{
			return false;
		}

		// Token: 0x0600224B RID: 8779 RVA: 0x001231F8 File Offset: 0x001215F8
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<TaleDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_Values.Look<int>(ref this.uses, "uses", 0, false);
			Scribe_Values.Look<int>(ref this.date, "date", 0, false);
			Scribe_Deep.Look<TaleData_Surroundings>(ref this.surroundings, "surroundings", new object[0]);
		}

		// Token: 0x0600224C RID: 8780 RVA: 0x00123262 File Offset: 0x00121662
		public void Notify_NewlyUsed()
		{
			this.uses++;
		}

		// Token: 0x0600224D RID: 8781 RVA: 0x00123273 File Offset: 0x00121673
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

		// Token: 0x0600224E RID: 8782 RVA: 0x001232AC File Offset: 0x001216AC
		public IEnumerable<RulePack> GetTextGenerationIncludes()
		{
			if (this.def.rulePack != null)
			{
				yield return this.def.rulePack;
			}
			yield break;
		}

		// Token: 0x0600224F RID: 8783 RVA: 0x001232D8 File Offset: 0x001216D8
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

		// Token: 0x06002250 RID: 8784 RVA: 0x00123304 File Offset: 0x00121704
		protected virtual IEnumerable<Rule> SpecialTextGenerationRules()
		{
			yield break;
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x00123328 File Offset: 0x00121728
		public string GetUniqueLoadID()
		{
			return "Tale_" + this.id;
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x00123354 File Offset: 0x00121754
		public override int GetHashCode()
		{
			return this.id;
		}

		// Token: 0x06002253 RID: 8787 RVA: 0x00123370 File Offset: 0x00121770
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

		// Token: 0x0400137C RID: 4988
		public TaleDef def;

		// Token: 0x0400137D RID: 4989
		public int id;

		// Token: 0x0400137E RID: 4990
		private int uses = 0;

		// Token: 0x0400137F RID: 4991
		public int date = -1;

		// Token: 0x04001380 RID: 4992
		public TaleData_Surroundings surroundings;
	}
}
