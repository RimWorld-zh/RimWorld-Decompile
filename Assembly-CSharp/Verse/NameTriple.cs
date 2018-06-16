using System;
using System.Linq;

namespace Verse
{
	// Token: 0x02000D4C RID: 3404
	public class NameTriple : Name
	{
		// Token: 0x06004AF1 RID: 19185 RVA: 0x002709A2 File Offset: 0x0026EDA2
		public NameTriple()
		{
		}

		// Token: 0x06004AF2 RID: 19186 RVA: 0x002709AB File Offset: 0x0026EDAB
		public NameTriple(string first, string nick, string last)
		{
			this.firstInt = first;
			this.nickInt = nick;
			this.lastInt = last;
		}

		// Token: 0x17000C00 RID: 3072
		// (get) Token: 0x06004AF3 RID: 19187 RVA: 0x002709CC File Offset: 0x0026EDCC
		public string First
		{
			get
			{
				return this.firstInt;
			}
		}

		// Token: 0x17000C01 RID: 3073
		// (get) Token: 0x06004AF4 RID: 19188 RVA: 0x002709E8 File Offset: 0x0026EDE8
		public string Nick
		{
			get
			{
				return this.nickInt;
			}
		}

		// Token: 0x17000C02 RID: 3074
		// (get) Token: 0x06004AF5 RID: 19189 RVA: 0x00270A04 File Offset: 0x0026EE04
		public string Last
		{
			get
			{
				return this.lastInt;
			}
		}

		// Token: 0x17000C03 RID: 3075
		// (get) Token: 0x06004AF6 RID: 19190 RVA: 0x00270A20 File Offset: 0x0026EE20
		public override string ToStringFull
		{
			get
			{
				string result;
				if (this.First == this.Nick || this.Last == this.Nick)
				{
					result = this.First + " " + this.Last;
				}
				else
				{
					result = string.Concat(new string[]
					{
						this.First,
						" '",
						this.Nick,
						"' ",
						this.Last
					});
				}
				return result;
			}
		}

		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x06004AF7 RID: 19191 RVA: 0x00270AB4 File Offset: 0x0026EEB4
		public override string ToStringShort
		{
			get
			{
				return this.nickInt;
			}
		}

		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x06004AF8 RID: 19192 RVA: 0x00270AD0 File Offset: 0x0026EED0
		public override bool IsValid
		{
			get
			{
				return !this.First.NullOrEmpty() && !this.Last.NullOrEmpty();
			}
		}

		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x06004AF9 RID: 19193 RVA: 0x00270B08 File Offset: 0x0026EF08
		public override bool Numerical
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x06004AFA RID: 19194 RVA: 0x00270B20 File Offset: 0x0026EF20
		public static NameTriple Invalid
		{
			get
			{
				return NameTriple.invalidInt;
			}
		}

		// Token: 0x06004AFB RID: 19195 RVA: 0x00270B3A File Offset: 0x0026EF3A
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.firstInt, "first", null, false);
			Scribe_Values.Look<string>(ref this.nickInt, "nick", null, false);
			Scribe_Values.Look<string>(ref this.lastInt, "last", null, false);
		}

		// Token: 0x06004AFC RID: 19196 RVA: 0x00270B74 File Offset: 0x0026EF74
		public void PostLoad()
		{
			if (this.firstInt != null)
			{
				this.firstInt = this.firstInt.Trim();
			}
			if (this.nickInt != null)
			{
				this.nickInt = this.nickInt.Trim();
			}
			if (this.lastInt != null)
			{
				this.lastInt = this.lastInt.Trim();
			}
		}

		// Token: 0x06004AFD RID: 19197 RVA: 0x00270BD8 File Offset: 0x0026EFD8
		public void ResolveMissingPieces(string overrideLastName = null)
		{
			if (this.First.NullOrEmpty() && this.Nick.NullOrEmpty() && this.Last.NullOrEmpty())
			{
				Log.Error("Cannot resolve misssing pieces in PawnName: No name data.", false);
				this.firstInt = (this.nickInt = (this.lastInt = "Empty"));
			}
			else
			{
				if (this.First == null)
				{
					this.firstInt = "";
				}
				if (this.Last == null)
				{
					this.lastInt = "";
				}
				if (overrideLastName != null)
				{
					this.lastInt = overrideLastName;
				}
				if (this.Nick.NullOrEmpty())
				{
					if (this.Last == "")
					{
						this.nickInt = this.First;
					}
					else
					{
						if (Rand.ValueSeeded(Gen.HashCombine<string>(this.First.GetHashCode(), this.Last)) < 0.5f)
						{
							this.nickInt = this.First;
						}
						else
						{
							this.nickInt = this.Last;
						}
						this.CapitalizeNick();
					}
				}
			}
		}

		// Token: 0x06004AFE RID: 19198 RVA: 0x00270D00 File Offset: 0x0026F100
		public override bool ConfusinglySimilarTo(Name other)
		{
			NameTriple nameTriple = other as NameTriple;
			if (nameTriple != null)
			{
				if (this.Nick != null && this.Nick == nameTriple.Nick)
				{
					return true;
				}
				if (this.First == nameTriple.First && this.Last == nameTriple.Last)
				{
					return true;
				}
			}
			NameSingle nameSingle = other as NameSingle;
			return nameSingle != null && nameSingle.Name == this.Nick;
		}

		// Token: 0x06004AFF RID: 19199 RVA: 0x00270DAC File Offset: 0x0026F1AC
		public static NameTriple FromString(string rawName)
		{
			NameTriple result;
			if (rawName.Trim().Length == 0)
			{
				Log.Error("Tried to parse PawnName from empty or whitespace string.", false);
				result = NameTriple.Invalid;
			}
			else
			{
				NameTriple nameTriple = new NameTriple();
				int num = -1;
				int num2 = -1;
				for (int i = 0; i < rawName.Length - 1; i++)
				{
					if (rawName[i] == ' ' && rawName[i + 1] == '\'')
					{
						if (num == -1)
						{
							num = i;
						}
					}
					if (rawName[i] == '\'' && rawName[i + 1] == ' ')
					{
						num2 = i;
					}
				}
				if (num == -1 || num2 == -1)
				{
					if (!rawName.Contains(' '))
					{
						nameTriple.nickInt = rawName.Trim();
					}
					else
					{
						string[] array = rawName.Split(new char[]
						{
							' '
						});
						if (array.Length == 1)
						{
							nameTriple.nickInt = array[0].Trim();
						}
						else if (array.Length == 2)
						{
							nameTriple.firstInt = array[0].Trim();
							nameTriple.lastInt = array[1].Trim();
						}
						else
						{
							nameTriple.firstInt = array[0].Trim();
							nameTriple.lastInt = "";
							for (int j = 1; j < array.Length; j++)
							{
								NameTriple nameTriple2 = nameTriple;
								nameTriple2.lastInt += array[j];
								if (j < array.Length - 1)
								{
									NameTriple nameTriple3 = nameTriple;
									nameTriple3.lastInt += " ";
								}
							}
						}
					}
				}
				else
				{
					nameTriple.firstInt = rawName.Substring(0, num).Trim();
					nameTriple.nickInt = rawName.Substring(num + 2, num2 - num - 2).Trim();
					nameTriple.lastInt = ((num2 >= rawName.Length - 2) ? "" : rawName.Substring(num2 + 2).Trim());
				}
				result = nameTriple;
			}
			return result;
		}

		// Token: 0x06004B00 RID: 19200 RVA: 0x00270FC4 File Offset: 0x0026F3C4
		public void CapitalizeNick()
		{
			if (!this.nickInt.NullOrEmpty())
			{
				this.nickInt = char.ToUpper(this.Nick[0]) + this.Nick.Substring(1);
			}
		}

		// Token: 0x06004B01 RID: 19201 RVA: 0x00271004 File Offset: 0x0026F404
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.First,
				" '",
				this.Nick,
				"' ",
				this.Last
			});
		}

		// Token: 0x06004B02 RID: 19202 RVA: 0x00271050 File Offset: 0x0026F450
		public override bool Equals(object obj)
		{
			bool result;
			if (obj == null)
			{
				result = false;
			}
			else if (!(obj is NameTriple))
			{
				result = false;
			}
			else
			{
				NameTriple nameTriple = (NameTriple)obj;
				result = (this.First == nameTriple.First && this.Last == nameTriple.Last && this.Nick == nameTriple.Nick);
			}
			return result;
		}

		// Token: 0x06004B03 RID: 19203 RVA: 0x002710CC File Offset: 0x0026F4CC
		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombine<string>(seed, this.First);
			seed = Gen.HashCombine<string>(seed, this.Last);
			return Gen.HashCombine<string>(seed, this.Nick);
		}

		// Token: 0x04003277 RID: 12919
		[LoadAlias("first")]
		private string firstInt;

		// Token: 0x04003278 RID: 12920
		[LoadAlias("nick")]
		private string nickInt;

		// Token: 0x04003279 RID: 12921
		[LoadAlias("last")]
		private string lastInt;

		// Token: 0x0400327A RID: 12922
		private static NameTriple invalidInt = new NameTriple("Invalid", "Invalid", "Invalid");
	}
}
