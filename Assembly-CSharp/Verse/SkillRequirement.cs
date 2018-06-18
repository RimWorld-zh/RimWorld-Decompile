using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B62 RID: 2914
	public class SkillRequirement
	{
		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x06003F92 RID: 16274 RVA: 0x00217DEC File Offset: 0x002161EC
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

		// Token: 0x06003F93 RID: 16275 RVA: 0x00217E38 File Offset: 0x00216238
		public bool PawnSatisfies(Pawn pawn)
		{
			return pawn.skills != null && pawn.skills.GetSkill(this.skill).Level >= this.minLevel;
		}

		// Token: 0x06003F94 RID: 16276 RVA: 0x00217E80 File Offset: 0x00216280
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "skill", xmlRoot.Name);
			this.minLevel = (int)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(int));
		}

		// Token: 0x06003F95 RID: 16277 RVA: 0x00217EBC File Offset: 0x002162BC
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
