using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B48 RID: 2888
	public class JobDef : Def
	{
		// Token: 0x06003F52 RID: 16210 RVA: 0x002161E0 File Offset: 0x002145E0
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return e;
			}
			if (this.joySkill != null && this.joyXpPerTick == 0f)
			{
				yield return "funSkill is not null but funXpPerTick is zero";
			}
			yield break;
		}

		// Token: 0x040029BD RID: 10685
		public Type driverClass;

		// Token: 0x040029BE RID: 10686
		[MustTranslate]
		public string reportString = "Doing something.";

		// Token: 0x040029BF RID: 10687
		public bool playerInterruptible = true;

		// Token: 0x040029C0 RID: 10688
		public CheckJobOverrideOnDamageMode checkOverrideOnDamage = CheckJobOverrideOnDamageMode.Always;

		// Token: 0x040029C1 RID: 10689
		public bool alwaysShowWeapon = false;

		// Token: 0x040029C2 RID: 10690
		public bool neverShowWeapon = false;

		// Token: 0x040029C3 RID: 10691
		public bool suspendable = true;

		// Token: 0x040029C4 RID: 10692
		public bool casualInterruptible = true;

		// Token: 0x040029C5 RID: 10693
		public bool allowOpportunisticPrefix = false;

		// Token: 0x040029C6 RID: 10694
		public bool collideWithPawns = false;

		// Token: 0x040029C7 RID: 10695
		public bool isIdle = false;

		// Token: 0x040029C8 RID: 10696
		public TaleDef taleOnCompletion = null;

		// Token: 0x040029C9 RID: 10697
		public bool makeTargetPrisoner = false;

		// Token: 0x040029CA RID: 10698
		public int joyDuration = 4000;

		// Token: 0x040029CB RID: 10699
		public int joyMaxParticipants = 1;

		// Token: 0x040029CC RID: 10700
		public float joyGainRate = 1f;

		// Token: 0x040029CD RID: 10701
		public SkillDef joySkill = null;

		// Token: 0x040029CE RID: 10702
		public float joyXpPerTick = 0f;

		// Token: 0x040029CF RID: 10703
		public JoyKindDef joyKind = null;

		// Token: 0x040029D0 RID: 10704
		public Rot4 faceDir = Rot4.Invalid;
	}
}
