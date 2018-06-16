using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B4C RID: 2892
	public class JobDef : Def
	{
		// Token: 0x06003F50 RID: 16208 RVA: 0x00215AE0 File Offset: 0x00213EE0
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

		// Token: 0x040029BF RID: 10687
		public Type driverClass;

		// Token: 0x040029C0 RID: 10688
		[MustTranslate]
		public string reportString = "Doing something.";

		// Token: 0x040029C1 RID: 10689
		public bool playerInterruptible = true;

		// Token: 0x040029C2 RID: 10690
		public CheckJobOverrideOnDamageMode checkOverrideOnDamage = CheckJobOverrideOnDamageMode.Always;

		// Token: 0x040029C3 RID: 10691
		public bool alwaysShowWeapon = false;

		// Token: 0x040029C4 RID: 10692
		public bool neverShowWeapon = false;

		// Token: 0x040029C5 RID: 10693
		public bool suspendable = true;

		// Token: 0x040029C6 RID: 10694
		public bool casualInterruptible = true;

		// Token: 0x040029C7 RID: 10695
		public bool allowOpportunisticPrefix = false;

		// Token: 0x040029C8 RID: 10696
		public bool collideWithPawns = false;

		// Token: 0x040029C9 RID: 10697
		public bool isIdle = false;

		// Token: 0x040029CA RID: 10698
		public TaleDef taleOnCompletion = null;

		// Token: 0x040029CB RID: 10699
		public bool makeTargetPrisoner = false;

		// Token: 0x040029CC RID: 10700
		public int joyDuration = 4000;

		// Token: 0x040029CD RID: 10701
		public int joyMaxParticipants = 1;

		// Token: 0x040029CE RID: 10702
		public float joyGainRate = 1f;

		// Token: 0x040029CF RID: 10703
		public SkillDef joySkill = null;

		// Token: 0x040029D0 RID: 10704
		public float joyXpPerTick = 0f;

		// Token: 0x040029D1 RID: 10705
		public JoyKindDef joyKind = null;

		// Token: 0x040029D2 RID: 10706
		public Rot4 faceDir = Rot4.Invalid;
	}
}
