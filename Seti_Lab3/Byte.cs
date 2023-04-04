using System;

namespace Seti_Lab3
{
    public class Byte
    {
        public string Number { get; set; }
        public string Bit0 { get; set; }
        public string Bit1 { get; set; }
        public string Bit2 { get; set; }
        public string Bit3 { get; set; }
        public string Bit4 { get; set; }
        public string Bit5 { get; set; }
        public string Bit6 { get; set; }
        public string Bit7 { get; set; }
        public string Checksum { get; set; }

        public Byte(string number, string bit0, string bit1, string bit2, string bit3, string bit4, string bit5, string bit6, string bit7, string checksum)
        {
            Number = number;
            Bit0 = bit0;
            Bit1 = bit1;
            Bit2 = bit2;
            Bit3 = bit3;
            Bit4 = bit4;
            Bit5 = bit5;
            Bit6 = bit6;
            Bit7 = bit7;
            Checksum = checksum;
        }

        public Byte(string number)
        {
            Number = number;
            Bit0 = "";
            Bit1 = "";
            Bit2 = "";
            Bit3 = "";
            Bit4 = "";
            Bit5 = "";
            Bit6 = "";
            Bit7 = "";
            Checksum = "";
        }

        public Byte(string number, string[] bits, string checksum)
        {
            Number = number;
            Bit0 = bits[0];
            Bit1 = bits[1];
            Bit2 = bits[2];
            Bit3 = bits[3];
            Bit4 = bits[4];
            Bit5 = bits[5];
            Bit6 = bits[6];
            Bit7 = bits[7];
            Checksum = checksum;
        }

        public Byte(string number, byte[] bits, string checksum)
        {
            Number = number;
            Bit0 = bits[0].ToString();
            Bit1 = bits[1].ToString();
            Bit2 = bits[2].ToString();
            Bit3 = bits[3].ToString();
            Bit4 = bits[4].ToString();
            Bit5 = bits[5].ToString();
            Bit6 = bits[6].ToString();
            Bit7 = bits[7].ToString();
            Checksum = checksum;
        }

        public Byte(string number, byte bits, string checksum)
        {
            var temp = Convert.ToString(bits, 2).PadLeft(8, '0');
            Number = number;
            Bit0 = temp[0].ToString();
            Bit1 = temp[1].ToString();
            Bit2 = temp[2].ToString();
            Bit3 = temp[3].ToString();
            Bit4 = temp[4].ToString();
            Bit5 = temp[5].ToString();
            Bit6 = temp[6].ToString();
            Bit7 = temp[7].ToString();
            Checksum = checksum;
        }
    }
}
