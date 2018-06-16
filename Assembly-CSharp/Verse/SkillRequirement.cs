using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B62 RID: 2914
	public class SkillRequirement
	{
		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x06003F90 RID: 16272 RVA: 0x00217D18 File Offset: 0x00216118
		public string Summary
		{
			get
			{
				string result;
				if (this.skill == null)
				{
					result = "";
				}
				else
				{
					result = string.Format("{0} ({1})", this.skill.LabelCap, this.minLevel);
				}
				return result;
			}
		}

		// Token: 0x06003F91 RID: 16273 RVA: 0x00217D64 File Offset: 0x00216164
		public bool PawnSatisfies(Pawn pawn)
		{
			return pawn.skills != null && pawn.skills.GetSkill(this.skill).Level >= this.minLevel;
		}

		// Token: 0x06003F92 RID: 16274 RVA: 0x00217DAC File Offset: 0x002161AC
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "skill", xmlRoot.Name);
			this.minLevel = (int)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(int));
		}

		// Token: 0x06003F93 RID: 16275 RVA: 0x00217DE8 File Offset: 0x002161E8
		public override string ToString()
		{
			string result;
			if (this.skill == null)
			{
				result = "null-skill-requirement";
			}
			else
			{
				result = this.skill.defName + "-" + this.minLevel;
			}
			return result;
		}

		// Token: 0x04002A77 RID: 10871
		public SkillDef skill;

		// Token: 0x04002A78 RID: 10872
		public int minLevel;
	}
}
