using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Verse
{
	public class HediffWithComps : Hediff
	{
		public List<HediffComp> comps = new List<HediffComp>();

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

		public override bool ShouldRemove
		{
			get
			{
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						if (this.comps[i].CompShouldRemove)
							goto IL_002b;
					}
				}
				bool result = base.ShouldRemove;
				goto IL_0055;
				IL_002b:
				result = true;
				goto IL_0055;
				IL_0055:
				return result;
			}
		}

		public override bool Visible
		{
			get
			{
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						if (this.comps[i].CompDisallowVisible())
							goto IL_002b;
					}
				}
				bool result = base.Visible;
				goto IL_0055;
				IL_002b:
				result = false;
				goto IL_0055;
				IL_0055:
				return result;
			}
		}

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

		public override TextureAndColor StateIcon
		{
			get
			{
				int num = 0;
				TextureAndColor result;
				while (true)
				{
					if (num < this.comps.Count)
					{
						TextureAndColor compStateIcon = this.comps[num].CompStateIcon;
						if (compStateIcon.HasValue)
						{
							result = compStateIcon;
							break;
						}
						num++;
						continue;
					}
					result = TextureAndColor.None;
					break;
				}
				return result;
			}
		}

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
				if (num != 0.0)
				{
					this.Severity += num;
				}
			}
		}

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

		public override void Tended(float quality, int batchPosition = 0)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompTended(quality, batchPosition);
			}
		}

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

		public override void Notify_PawnDied()
		{
			base.Notify_PawnDied();
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_PawnDied();
			}
		}

		public override void ModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompModifyChemicalEffect(chem, ref effect);
			}
		}

		public override void PostMake()
		{
			base.PostMake();
			this.InitializeComps();
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompPostMake();
			}
		}

		private void InitializeComps()
		{
			if (base.def.comps != null)
			{
				this.comps = new List<HediffComp>();
				for (int i = 0; i < base.def.comps.Count; i++)
				{
					HediffComp hediffComp = (HediffComp)Activator.CreateInstance(base.def.comps[i].compClass);
					hediffComp.props = base.def.comps[i];
					hediffComp.parent = this;
					this.comps.Add(hediffComp);
				}
			}
		}

		public override string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.DebugString());
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					string str = (!this.comps[i].ToString().Contains('_')) ? this.comps[i].ToString() : this.comps[i].ToString().Split('_')[1];
					stringBuilder.AppendLine("--" + str);
					string text = this.comps[i].CompDebugString();
					if (!text.NullOrEmpty())
					{
						stringBuilder.AppendLine(text.TrimEnd().Indented());
					}
				}
			}
			return stringBuilder.ToString();
		}
	}
}
