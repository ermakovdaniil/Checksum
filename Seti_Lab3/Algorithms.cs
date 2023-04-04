using System;
using System.Text;

namespace Seti_Lab3
{
    class Algorithms
    {
        public static byte[] HorParityControl(byte[] bytes)
        {
            byte[] checksum = new byte[bytes.Length];
            for(int i = 0; i < bytes.Length; i++)
            {
                checksum[i] = (byte)(bytes[i] & (1 << 7));
                for (int j = 1; j < 8; j++)
                {
                    //bytes[i] ^= (byte)(bytes[i] >> 4);
                    //bytes[i] ^= (byte)(bytes[i] >> 2);
                    //bytes[i] ^= (byte)(bytes[i] >> 1);
                    //checksum[i] = (byte)((~bytes[i]) & 1);
                    checksum[i] ^= (byte)((bytes[i] & (1 << 8 - j - 1)) >> j);
                }
                checksum[i] = (byte)(checksum[i] >> 7);
            }
            return checksum;
        }

        public static byte[] VertParityControl(byte[] bytes)
        {
            byte[] checksum = new byte[8];
            byte tempByte;
            for (int i = 0; i < 8; i++)
            {
                checksum[i] = (byte)(bytes[0] & (1 << 8 - i - 1));
                for (int j = 1; j < bytes.Length; j++)
                {
                    tempByte = (byte)(bytes[j] & (1 << 8 - i - 1));
                    checksum[i] ^= (byte)(tempByte & (1 << 8 - i - 1));
                }
                checksum[i] = (byte)(checksum[i] >> 8 - i - 1);
            }
            return checksum;
        }

        public static string CRC(byte[] bytes, byte[] polynomial, int polynomialDegree)
        {
            byte tempBit;
            byte tempBit2;
            byte tempBit3;
            byte tempBit4;
            byte tempByte = 255;
            byte[] register = new byte[polynomial.Length];
            int registerSize = register.Length * 8 - polynomialDegree;
            Array.Resize(ref bytes, bytes.Length + polynomial.Length);
            string crclog;
            string registers;
            StringBuilder sb = new StringBuilder();
            sb.Append("Ход циклического избыточного контроля:\n");

            for (int i = 0; i < register.Length; i++)
            {
                register[i] = 0;
            }

            for (int i = bytes.Length - polynomial.Length; i < bytes.Length; i++)
            {
                bytes[i] = 0;
            }

            for (int i = 0; i < bytes.Length * 8 - (polynomial.Length * 8 - polynomialDegree); i++)  // 10011110101100101
            {
                registers = "";
                // Берём 0-ой бит первого байта в регистре для будующей проверки. Сдвиг первого байта
                tempBit = (byte)(register[0] & (1 << 7 - registerSize));
                tempBit = (byte)(tempBit >> 7 - registerSize);
                register[0] = (byte)(register[0] << 1);
                register[0] = (byte)(register[0] & (tempByte >> registerSize));
                for (int j = 1; j < register.Length; j++) // Сдвиг на бит ргистра
                {
                    // 0-ой бит след. байта. Сдвиг его до вида 0000000_ (0/1). Сдвиг байта на 1 влево (будет место справа для бита)
                    tempBit2 = (byte)(register[j] & (1 << 7));
                    tempBit2 = (byte)(tempBit2 >> 7);
                    register[j] = (byte)(register[j] << 1);

                    // Текущий байт регистра | на результат предыдущего шага (0000000_ ). Получаем перенос.
                    register[j - 1] = (byte)(register[j - 1] | tempBit2);
                }

                // Берём 0-ой бит текущего байта. Сдвиг его до вида 0000000_ (0/1) для перемещения в регистр.
                tempBit3 = (byte)(bytes[0] & (1 << 7));
                tempBit3 = (byte)(tempBit3 >> 7);
                bytes[0] = (byte)(bytes[0] << 1);

                for (int j = 1; j < bytes.Length; j++) // Сдвиг на бит байтов
                {
                    // 0-ой бит след. байта. Сдвиг его до вида 0000000_ (0/1). Сдвиг байта на 1 влево (будет место справа для бита)
                    tempBit4 = (byte)(bytes[j] & (1 << 7));
                    tempBit4 = (byte)(tempBit4 >> 7);
                    bytes[j] = (byte)(bytes[j] << 1);

                    // Текущий байт байтов | на результат предыдущего шага ( 0000000_ ). Получаем перенос.
                    bytes[j - 1] = (byte)(bytes[j - 1] | tempBit4);
                }

                // Перенос в регистр бита
                // Последний байт регистра | на 0-ой бит последовательности байтов в виде ( 0000000_).
                register[register.Length - 1] = (byte)(register[register.Length - 1] | tempBit3);

                // Проверка сдвинутого бита
                if (tempBit == 1)
                {
                    for (int k = 0; k < register.Length; k++)
                    {
                        register[k] ^= polynomial[k];
                    }
                }
                for (int z = 0; z < register.Length; z++)
                {
                    registers += Convert.ToString(register[z], 2).PadLeft(8, '0');
                }
                registers = registers.Remove(0, registerSize);
                sb.Append(String.Format("{0, 3}) ", i + 1) + registers + "\n");
                if (i == bytes.Length * 8 - (8 * polynomial.Length - polynomialDegree) - 1)
                {
                    while (true)
                    {
                        if (registers[0].ToString() == "0")
                        {
                            registers = registers.Remove(0, 1);
                        }
                        else
                            break;
                    }
                    sb.Append(String.Format("Контрольная сумма равна: " + registers));
                }
            }
            crclog = sb.ToString();
            return crclog;
        }
    }
}