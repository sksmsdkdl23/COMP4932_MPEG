using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_JPEG
{
    public class DCT
    {
        private int[,] qTable = {
            { 16, 11, 10, 16, 24, 40, 51, 61 },
            { 12, 12, 14, 19, 26, 58, 60, 55 },
            { 14, 13, 16, 24, 40, 57, 69, 56 },
            { 14, 17, 22, 29, 51, 87, 80, 62 },
            { 18, 22, 37, 56, 68, 109, 103, 77 },
            { 24, 35, 55, 64, 81, 104, 113, 92 },
            { 49, 64, 78, 87, 103, 121, 120, 101 },
            { 72, 92, 95, 98, 112, 100, 103, 99 }
        };

        public DCT()
        {

        }

        private double C(int x)
        {
            if (x == 0)
            {
                return (1 / (Math.Sqrt(2)));
            }
            else
            {
                return 1;
            }
        }

        public double[,] forwardDCT(byte[,] img, int width, int height)
        {
            double[,] data = new double[256, 256];
            double sum = 0;
            for (int row = 0; row < width / 8; row++)
            {
                for (int column = 0; column < height / 8; column++)
                {
                    for (int u = 0; u < 8; u++)
                    {
                        for (int v = 0; v < 8; v++)
                        {
                            sum = 0;
                            for (int i = 0; i < 8; i++)
                            {
                                for (int j = 0; j < 8; j++)
                                {
                                    sum += Math.Cos(((2 * i + 1) * u * Math.PI) / 16)
                                        * Math.Cos(((2 * j + 1) * v * Math.PI) / 16) * (img[i + row * 8, j + column * 8] - 128);
                                }
                            }
                            data[u + row * 8, v + column * 8] = sum * ((C(u)) * C(v) / 4);
                        }
                    }
                }
            }
            return data;
        }

        public byte[,] inverseDCT(double[,] img, int width, int height)
        {
            byte[,] data = new byte[256, 256];
            double sum = 0;
            for (int row = 0; row < width / 8; row++)
            {
                for (int column = 0; column < height / 8; column++)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            sum = 0;
                            for (int u = 0; u < 8; u++)
                            {
                                for (int v = 0; v < 8; v++)
                                {
                                    sum += ((C(u) * C(v)))
                                        * Math.Cos(((2 * i + 1) * u * Math.PI) / 16)
                                        * Math.Cos(((2 * j + 1) * v * Math.PI) / 16)
                                        * img[u + row * 8, v + column * 8];
                                }
                            }
                            sum = sum / 4 + 128;
                            if (sum > 255)
                                sum = 255;
                            if (sum < 0)
                                sum = 0;
                            byte byteSum = (byte)sum;
                            data[i + row * 8, j + column * 8] = (byte)sum;
                        }
                    }
                }
            }
            return data;
        }

        public byte[,] quantize(double[,] img, int width, int height)
        {
            byte[,] quantized = new byte[256, 256];
            double temp = 0;

            for (int row = 0; row < width / 8; row++)
            {
                for (int column = 0; column < height / 8; column++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        for (int x = 0; x < 8; x++)
                        {
                            temp = (img[x + row * 8, y + column * 8] / qTable[x, y]);
                            temp = Math.Round(temp);
                            if (temp < -128)
                                temp = -128;
                            if (temp > 127)
                                temp = 127;
                            byte unsignedByte = (byte) (temp + 128);

                            quantized[x + row * 8, y + column * 8] = unsignedByte;
                        }
                    }
                }
            }
            return quantized;
        }

        public double[,] inverseQuantize(byte[,] imgQ, int width, int height)
        {
            double[,] invQData = new double[256, 256];

            for (int row = 0; row < width / 8; row++)
            {
                for (int column = 0; column < height / 8; column++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        for (int x = 0; x < 8; x++)
                        {
                            invQData[x + row * 8, y + column * 8] = Math.Round((double) (imgQ[x + row * 8, y + column * 8] - 128) * qTable[x, y]);
                        }
                    }
                }
            }
            return invQData;
        }
    }
}
