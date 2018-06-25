using System;

namespace Verse
{
	public class NameSingle : Name
	{
		private string nameInt;

		private bool numerical;

		public NameSingle()
		{
		}

		public NameSingle(string name, bool numerical = false)
		{
			this.nameInt = name;
			this.numerical = numerical;
		}

		public string Name
		{
			get
			{
				return this.nameInt;
			}
		}

		public override string ToStringFull
		{
			get
			{
				return this.nameInt;
			}
		}

		public override string ToStringShort
		{
			get
			{
				return this.nameInt;
			}
		}

		public override bool IsValid
		{
			get
			{
				return !this.nameInt.NullOrEmpty();
			}
		}

		public override bool Numerical
		{
			get
			{
				return this.numerical;
			}
		}

		private int FirstDigitPosition
		{
			get
			{
				int result;
				if (!this.numerical)
				{
					result = -1;
				}
				else if (this.nameInt.NullOrEmpty() || !char.IsDigit(this.nameInt[this.nameInt.Length - 1]))
				{
					result = -1;
				}
				else
				{
					for (int i = this.nameInt.Length - 2; i >= 0; i--)
					{
						if (!char.IsDigit(this.nameInt[i]))
						{
							return i + 1;
						}
					}
					result = 0;
				}
				return result;
			}
		}

		public string NameWithoutNumber
		{
			get
			{
				string result;
				if (!this.numerical)
				{
					result = this.nameInt;
				}
				else
				{
					int firstDigitPosition = this.FirstDigitPosition;
					if (firstDigitPosition < 0)
					{
						result = this.nameInt;
					}
					else
					{
						int num = firstDigitPosition;
						if (num - 1 >= 0 && this.nameInt[num - 1] == ' ')
						{
							num--;
						}
						if (num <= 0)
						{
							result = "";
						}
						else
						{
							result = this.nameInt.Substring(0, num);
						}
					}
				}
				return result;
			}
		}

		public int Number
		{
			get
			{
				int result;
				if (!this.numerical)
				{
					result = 0;
				}
				else
				{
					int firstDigitPosition = this.FirstDigitPosition;
					if (firstDigitPosition < 0)
					{
						result = 0;
					}
					else
					{
						result = int.Parse(this.nameInt.Substring(firstDigitPosition));
					}
				}
				return result;
			}
		}

		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.nameInt, "name", null, false);
			Scribe_Values.Look<bool>(ref this.numerical, "numerical", false, false);
		}

		public override bool ConfusinglySimilarTo(Name other)
		{
			NameSingle nameSingle = other as NameSingle;
			bool result;
			if (nameSingle != null && nameSingle.nameInt == this.nameInt)
			{
				result = true;
			}
			else
			{
				NameTriple nameTriple = other as NameTriple;
				result = (nameTriple != null && nameTriple.Nick == this.nameInt);
			}
			return result;
		}

		public override string ToString()
		{
			return this.nameInt;
		}

		public override bool Equals(object obj)
		{
			bool result;
			if (obj == null)
			{
				result = false;
			}
			else if (!(obj is NameSingle))
			{
				result = false;
			}
			else
			{
				NameSingle nameSingle = (NameSingle)obj;
				result = (this.nameInt == nameSingle.nameInt);
			}
			return result;
		}

		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.nameInt.GetHashCode(), 1384661390);
		}
	}
}
