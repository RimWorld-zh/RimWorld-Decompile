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
		// Token: 0x0400323F RID: 12863
		public List<HediffComp> comps = new List<HediffComp>();

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x06004A28 RID: 18984 RVA: 0x000AB15C File Offset: 0x000A955C
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

		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x06004A29 RID: 18985 RVA: 0x000AB1F4 File Offset: 0x000A95F4
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

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x06004A2A RID: 18986 RVA: 0x000AB258 File Offset: 0x000A9658
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

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x06004A2B RID: 18987 RVA: 0x000AB2BC File Offset: 0x000A96BC
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

		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x06004A2C RID: 18988 RVA: 0x000AB33C File Offset: 0x000A973C
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

		// Token: 0x06004A2D RID: 18989 RVA: 0x000AB39C File Offset: 0x000A979C
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

		// Token: 0x06004A2E RID: 18990 RVA: 0x000AB3E8 File Offset: 0x000A97E8
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

		// Token: 0x06004A2F RID: 18991 RVA: 0x000AB438 File Offset: 0x000A9838
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

		// Token: 0x06004A30 RID: 18992 RVA: 0x000AB4AC File Offset: 0x000A98AC
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

		// Token: 0x06004A31 RID: 18993 RVA: 0x000AB510 File Offset: 0x000A9910
		public override void Tended(float quality, int batchPosition = 0)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompTended(quality, batchPosition);
			}
		}

		// Token: 0x06004A32 RID: 18994 RVA: 0x000AB550 File Offset: 0x000A9950
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

		// Token: 0x06004A33 RID: 18995 RVA: 0x000AB5AC File Offset: 0x000A99AC
		public override void Notify_PawnDied()
		{
			base.Notify_PawnDied();
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_PawnDied();
			}
		}

		// Token: 0x06004A34 RID: 18996 RVA: 0x000AB5F0 File Offset: 0x000A99F0
		public override void ModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompModifyChemicalEffect(chem, ref effect);
			}
		}

		// Token: 0x06004A35 RID: 18997 RVA: 0x000AB630 File Offset: 0x000A9A30
		public override void PostMake()
		{
			base.PostMake();
			this.InitializeComps();
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompPostMake();
			}
		}

		// Token: 0x06004A36 RID: 18998 RVA: 0x000AB67C File Offset: 0x000A9A7C
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

		// Token: 0x06004A37 RID: 18999 RVA: 0x000AB718 File Offset: 0x000A9B18
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
	}
}
