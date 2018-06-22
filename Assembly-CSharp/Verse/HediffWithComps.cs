using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D24 RID: 3364
	public class HediffWithComps : Hediff
	{
		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x06004A24 RID: 18980 RVA: 0x000AAE0C File Offset: 0x000A920C
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

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x06004A25 RID: 18981 RVA: 0x000AAEA4 File Offset: 0x000A92A4
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

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x06004A26 RID: 18982 RVA: 0x000AAF08 File Offset: 0x000A9308
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

		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x06004A27 RID: 18983 RVA: 0x000AAF6C File Offset: 0x000A936C
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

		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x06004A28 RID: 18984 RVA: 0x000AAFEC File Offset: 0x000A93EC
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

		// Token: 0x06004A29 RID: 18985 RVA: 0x000AB04C File Offset: 0x000A944C
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

		// Token: 0x06004A2A RID: 18986 RVA: 0x000AB098 File Offset: 0x000A9498
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

		// Token: 0x06004A2B RID: 18987 RVA: 0x000AB0E8 File Offset: 0x000A94E8
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

		// Token: 0x06004A2C RID: 18988 RVA: 0x000AB15C File Offset: 0x000A955C
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

		// Token: 0x06004A2D RID: 18989 RVA: 0x000AB1C0 File Offset: 0x000A95C0
		public override void Tended(float quality, int batchPosition = 0)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompTended(quality, batchPosition);
			}
		}

		// Token: 0x06004A2E RID: 18990 RVA: 0x000AB200 File Offset: 0x000A9600
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

		// Token: 0x06004A2F RID: 18991 RVA: 0x000AB25C File Offset: 0x000A965C
		public override void Notify_PawnDied()
		{
			base.Notify_PawnDied();
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_PawnDied();
			}
		}

		// Token: 0x06004A30 RID: 18992 RVA: 0x000AB2A0 File Offset: 0x000A96A0
		public override void ModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompModifyChemicalEffect(chem, ref effect);
			}
		}

		// Token: 0x06004A31 RID: 18993 RVA: 0x000AB2E0 File Offset: 0x000A96E0
		public override void PostMake()
		{
			base.PostMake();
			this.InitializeComps();
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompPostMake();
			}
		}

		// Token: 0x06004A32 RID: 18994 RVA: 0x000AB32C File Offset: 0x000A972C
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

		// Token: 0x06004A33 RID: 18995 RVA: 0x000AB3C8 File Offset: 0x000A97C8
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

		// Token: 0x04003238 RID: 12856
		public List<HediffComp> comps = new List<HediffComp>();
	}
}
