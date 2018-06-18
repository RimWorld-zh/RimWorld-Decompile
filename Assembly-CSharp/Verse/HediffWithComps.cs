using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D27 RID: 3367
	public class HediffWithComps : Hediff
	{
		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x06004A13 RID: 18963 RVA: 0x000AADFC File Offset: 0x000A91FC
		public override string LabelInBrackets
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.LabelInBrackets);
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						string compLabelInBracketsExtra = this.comps[i].CompLabelInBracketsExtra;
						if (!compLabelInBracketsExtra.NullOrEmpty())
						{
							if (stringBuilder.Length != 0)
							{
								stringBuilder.Append(", ");
							}
							stringBuilder.Append(compLabelInBracketsExtra);
						}
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x06004A14 RID: 18964 RVA: 0x000AAE94 File Offset: 0x000A9294
		public override bool ShouldRemove
		{
			get
			{
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						if (this.comps[i].CompShouldRemove)
						{
							return true;
						}
					}
				}
				return base.ShouldRemove;
			}
		}

		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x06004A15 RID: 18965 RVA: 0x000AAEF8 File Offset: 0x000A92F8
		public override bool Visible
		{
			get
			{
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						if (this.comps[i].CompDisallowVisible())
						{
							return false;
						}
					}
				}
				return base.Visible;
			}
		}

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x06004A16 RID: 18966 RVA: 0x000AAF5C File Offset: 0x000A935C
		public override string TipStringExtra
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.TipStringExtra);
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						string compTipStringExtra = this.comps[i].CompTipStringExtra;
						if (!compTipStringExtra.NullOrEmpty())
						{
							stringBuilder.AppendLine(compTipStringExtra);
						}
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x06004A17 RID: 18967 RVA: 0x000AAFDC File Offset: 0x000A93DC
		public override TextureAndColor StateIcon
		{
			get
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					TextureAndColor compStateIcon = this.comps[i].CompStateIcon;
					if (compStateIcon.HasValue)
					{
						return compStateIcon;
					}
				}
				return TextureAndColor.None;
			}
		}

		// Token: 0x06004A18 RID: 18968 RVA: 0x000AB03C File Offset: 0x000A943C
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPostPostAdd(dinfo);
				}
			}
		}

		// Token: 0x06004A19 RID: 18969 RVA: 0x000AB088 File Offset: 0x000A9488
		public override void PostRemoved()
		{
			base.PostRemoved();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPostPostRemoved();
				}
			}
		}

		// Token: 0x06004A1A RID: 18970 RVA: 0x000AB0D8 File Offset: 0x000A94D8
		public override void PostTick()
		{
			base.PostTick();
			if (this.comps != null)
			{
				float num = 0f;
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPostTick(ref num);
				}
				if (num != 0f)
				{
					this.Severity += num;
				}
			}
		}

		// Token: 0x06004A1B RID: 18971 RVA: 0x000AB14C File Offset: 0x000A954C
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.InitializeComps();
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompExposeData();
				}
			}
		}

		// Token: 0x06004A1C RID: 18972 RVA: 0x000AB1B0 File Offset: 0x000A95B0
		public override void Tended(float quality, int batchPosition = 0)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompTended(quality, batchPosition);
			}
		}

		// Token: 0x06004A1D RID: 18973 RVA: 0x000AB1F0 File Offset: 0x000A95F0
		public override bool TryMergeWith(Hediff other)
		{
			bool result;
			if (base.TryMergeWith(other))
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPostMerged(other);
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06004A1E RID: 18974 RVA: 0x000AB24C File Offset: 0x000A964C
		public override void Notify_PawnDied()
		{
			base.Notify_PawnDied();
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_PawnDied();
			}
		}

		// Token: 0x06004A1F RID: 18975 RVA: 0x000AB290 File Offset: 0x000A9690
		public override void ModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompModifyChemicalEffect(chem, ref effect);
			}
		}

		// Token: 0x06004A20 RID: 18976 RVA: 0x000AB2D0 File Offset: 0x000A96D0
		public override void PostMake()
		{
			base.PostMake();
			this.InitializeComps();
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompPostMake();
			}
		}

		// Token: 0x06004A21 RID: 18977 RVA: 0x000AB31C File Offset: 0x000A971C
		private void InitializeComps()
		{
			if (this.def.comps != null)
			{
				this.comps = new List<HediffComp>();
				for (int i = 0; i < this.def.comps.Count; i++)
				{
					HediffComp hediffComp = (HediffComp)Activator.CreateInstance(this.def.comps[i].compClass);
					hediffComp.props = this.def.comps[i];
					hediffComp.parent = this;
					this.comps.Add(hediffComp);
				}
			}
		}

		// Token: 0x06004A22 RID: 18978 RVA: 0x000AB3B8 File Offset: 0x000A97B8
		public override string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.DebugString());
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					string str;
					if (this.comps[i].ToString().Contains('_'))
					{
						str = this.comps[i].ToString().Split(new char[]
						{
							'_'
						})[1];
					}
					else
					{
						str = this.comps[i].ToString();
					}
					stringBuilder.AppendLine("--" + str);
					string text = this.comps[i].CompDebugString();
					if (!text.NullOrEmpty())
					{
						stringBuilder.AppendLine(text.TrimEnd(new char[0]).Indented("    "));
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400322D RID: 12845
		public List<HediffComp> comps = new List<HediffComp>();
	}
}
