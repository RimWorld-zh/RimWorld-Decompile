using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B60 RID: 2912
	public class SkillRequirement
	{
		// Token: 0x04002A78 RID: 10872
		public SkillDef skill;

		// Token: 0x04002A79 RID: 10873
		public int minLevel;

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x06003F96 RID: 16278 RVA: 0x00218530 File Offset: 0x00216930
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

		// Token: 0x06003F97 RID: 16279 RVA: 0x0021857C File Offset: 0x0021697C
		public bool PawnSatisfies(Pawn pawn)
		{
			return pawn.skills != null && pawn.skills.GetSkill(this.skill).Level >= this.minLevel;
		}

		// Token: 0x06003F98 RID: 16280 RVA: 0x002185C4 File Offset: 0x002169C4
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "skill", xmlRoot.Name);
			this.minLevel = (int)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(int));
		}

		// Token: 0x06003F99 RID: 16281 RVA: 0x00218600 File Offset: 0x00216A00
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
	}
}
