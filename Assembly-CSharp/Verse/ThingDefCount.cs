using System;

namespace Verse
{
	// Token: 0x02000F01 RID: 3841
	public struct ThingDefCount : IEquatable<ThingDefCount>, IExposable
	{
		// Token: 0x06005C29 RID: 23593 RVA: 0x002EE0C0 File Offset: 0x002EC4C0
		public ThingDefCount(ThingDef thingDef, int count)
		{
			if (count < 0)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to set ThingDefCount count to ",
					count,
					". thingDef=",
					thingDef
				}), false);
				count = 0;
			}
			this.thingDef = thingDef;
			this.count = count;
		}

		// Token: 0x17000EC9 RID: 3785
		// (get) Token: 0x06005C2A RID: 23594 RVA: 0x002EE118 File Offset: 0x002EC518
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
		}

		// Token: 0x17000ECA RID: 3786
		// (get) Token: 0x06005C2B RID: 23595 RVA: 0x002EE134 File Offset: 0x002EC534
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x06005C2C RID: 23596 RVA: 0x002EE14F File Offset: 0x002EC54F
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x06005C2D RID: 23597 RVA: 0x002EE174 File Offset: 0x002EC574
		public ThingDefCount WithCount(int newCount)
		{
			return new ThingDefCount(this.thingDef, newCount);
		}

		// Token: 0x06005C2E RID: 23598 RVA: 0x002EE198 File Offset: 0x002EC598
		public override bool Equals(object obj)
		{
			return obj is ThingDefCount && this.Equals((ThingDefCount)obj);
		}

		// Token: 0x06005C2F RID: 23599 RVA: 0x002EE1CC File Offset: 0x002EC5CC
		public bool Equals(ThingDefCount other)
		{
			return this == other;
		}

		// Token: 0x06005C30 RID: 23600 RVA: 0x002EE1F0 File Offset: 0x002EC5F0
		public static bool operator ==(ThingDefCount a, ThingDefCount b)
		{
			return a.thingDef == b.thingDef && a.count == b.count;
		}

		// Token: 0x06005C31 RID: 23601 RVA: 0x002EE22C File Offset: 0x002EC62C
		public static bool operator !=(ThingDefCount a, ThingDefCount b)
		{
			return !(a == b);
		}

		// Token: 0x06005C32 RID: 23602 RVA: 0x002EE24C File Offset: 0x002EC64C
		public override int GetHashCode()
		{
			return Gen.HashCombine<ThingDef>(this.count, this.thingDef);
		}

		// Token: 0x06005C33 RID: 23603 RVA: 0x002EE274 File Offset: 0x002EC674
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.count,
				"x ",
				(this.thingDef == null) ? "null" : this.thingDef.defName,
				")"
			});
		}

		// Token: 0x06005C34 RID: 23604 RVA: 0x002EE2E0 File Offset: 0x002EC6E0
		public static implicit operator ThingDefCount(ThingDefCountClass t)
		{
			return new ThingDefCount(t.thingDef, t.count);
		}

		// Token: 0x04003CE4 RID: 15588
		private ThingDef thingDef;

		// Token: 0x04003CE5 RID: 15589
		private int count;
	}
}
