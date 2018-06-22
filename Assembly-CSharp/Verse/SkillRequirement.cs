using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B5E RID: 2910
	public class SkillRequirement
	{
		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x06003F93 RID: 16275 RVA: 0x00218454 File Offset: 0x00216854
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

		// Token: 0x06003F94 RID: 16276 RVA: 0x002184A0 File Offset: 0x002168A0
		public bool PawnSatisfies(Pawn pawn)
		{
			return pawn.skills != null && pawn.skills.GetSkill(this.skill).Level >= this.minLevel;
		}

		// Token: 0x06003F95 RID: 16277 RVA: 0x002184E8 File Offset: 0x002168E8
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "skill", xmlRoot.Name);
			this.minLevel = (int)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(int));
		}

		// Token: 0x06003F96 RID: 16278 RVA: 0x00218524 File Offset: 0x00216924
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

		// Token: 0x04002A78 RID: 10872
		public SkillDef skill;

		// Token: 0x04002A79 RID: 10873
		public int minLevel;
	}
}
