using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class Need_Chemical : Need
	{
		private const float ThreshDesire = 0.01f;

		private const float ThreshSatisfied = 0.3f;

		public override int GUIChangeArrow
		{
			get
			{
				return -1;
			}
		}

		public DrugDesireCategory CurCategory
		{
			get
			{
				return (DrugDesireCategory)((!(this.CurLevel > 0.30000001192092896)) ? ((this.CurLevel > 0.0099999997764825821) ? 1 : 0) : 2);
			}
		}

		public override float CurLevel
		{
			get
			{
				return base.CurLevel;
			}
			set
			{
				DrugDesireCategory curCategory = this.CurCategory;
				base.CurLevel = value;
				if (this.CurCategory != curCategory)
				{
					this.CategoryChanged();
				}
			}
		}

		public Hediff_Addiction AddictionHediff
		{
			get
			{
				List<Hediff> hediffs = base.pawn.health.hediffSet.hediffs;
				int num = 0;
				Hediff_Addiction result;
				while (true)
				{
					if (num < hediffs.Count)
					{
						Hediff_Addiction hediff_Addiction = hediffs[num] as Hediff_Addiction;
						if (hediff_Addiction != null && hediff_Addiction.def.causesNeed == base.def)
						{
							result = hediff_Addiction;
							break;
						}
						num++;
						continue;
					}
					result = null;
					break;
				}
				return result;
			}
		}

		private float ChemicalFallPerTick
		{
			get
			{
				return (float)(base.def.fallPerDay / 60000.0);
			}
		}

		public Need_Chemical(Pawn pawn) : base(pawn)
		{
			base.threshPercents = new List<float>();
			base.threshPercents.Add(0.3f);
		}

		public override void SetInitialLevel()
		{
			base.CurLevelPercentage = Rand.Range(0.8f, 1f);
		}

		public override void NeedInterval()
		{
			if (!base.IsFrozen)
			{
				this.CurLevel -= (float)(this.ChemicalFallPerTick * 150.0);
			}
		}

		private void CategoryChanged()
		{
			Hediff_Addiction addictionHediff = this.AddictionHediff;
			if (addictionHediff != null)
			{
				addictionHediff.Notify_NeedCategoryChanged();
			}
		}
	}
}
