using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B61 RID: 2913
	public class SkillRequirement
	{
		// Token: 0x04002A7F RID: 10879
		public SkillDef skill;

		// Token: 0x04002A80 RID: 10880
		public int minLevel;

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x06003F96 RID: 16278 RVA: 0x00218810 File Offset: 0x00216C10
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

		// Token: 0x06003F97 RID: 16279 RVA: 0x0021885C File Offset: 0x00216C5C
		public bool PawnSatisfies(Pawn pawn)
		{
			return pawn.skills != null && pawn.skills.GetSkill(this.skill).Level >= this.minLevel;
		}

		// Token: 0x06003F98 RID: 16280 RVA: 0x002188A4 File Offset: 0x00216CA4
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "skill", xmlRoot.Name);
			this.minLevel = (int)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(int));
		}

		// Token: 0x06003F99 RID: 16281 RVA: 0x002188E0 File Offset: 0x00216CE0
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
