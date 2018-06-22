using System;

namespace Verse
{
	// Token: 0x02000C2D RID: 3117
	public struct ThingRequest
	{
		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x06004490 RID: 17552 RVA: 0x00240D84 File Offset: 0x0023F184
		public bool IsUndefined
		{
			get
			{
				return this.singleDef == null && this.group == ThingRequestGroup.Undefined;
			}
		}

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x06004491 RID: 17553 RVA: 0x00240DB0 File Offset: 0x0023F1B0
		public bool CanBeFoundInRegion
		{
			get
			{
				return !this.IsUndefined && (this.singleDef != null || this.group == ThingRequestGroup.Nothing || this.group.StoreInRegion());
			}
		}

		// Token: 0x06004492 RID: 17554 RVA: 0x00240DFC File Offset: 0x0023F1FC
		public static ThingRequest ForUndefined()
		{
			return new ThingRequest
			{
				singleDef = null,
				group = ThingRequestGroup.Undefined
			};
		}

		// Token: 0x06004493 RID: 17555 RVA: 0x00240E2C File Offset: 0x0023F22C
		public static ThingRequest ForDef(ThingDef singleDef)
		{
			return new ThingRequest
			{
				singleDef = singleDef,
				group = ThingRequestGroup.Undefined
			};
		}

		// Token: 0x06004494 RID: 17556 RVA: 0x00240E5C File Offset: 0x0023F25C
		public static ThingRequest ForGroup(ThingRequestGroup group)
		{
			return new ThingRequest
			{
				singleDef = null,
				group = group
			};
		}

		// Token: 0x06004495 RID: 17557 RVA: 0x00240E8C File Offset: 0x0023F28C
		public bool Accepts(Thing t)
		{
			bool result;
			if (this.singleDef != null)
			{
				result = (t.def == this.singleDef);
			}
			else
			{
				result = (this.group == ThingRequestGroup.Everything || this.group.Includes(t.def));
			}
			return result;
		}

		// Token: 0x06004496 RID: 17558 RVA: 0x00240EE4 File Offset: 0x0023F2E4
		public override string ToString()
		{
			string str;
			if (this.singleDef != null)
			{
				str = "singleDef " + this.singleDef.defName;
			}
			else
			{
				str = "group " + this.group.ToString();
			}
			return "ThingRequest(" + str + ")";
		}

		// Token: 0x04002E86 RID: 11910
		public ThingDef singleDef;

		// Token: 0x04002E87 RID: 11911
		public ThingRequestGroup group;
	}
}
