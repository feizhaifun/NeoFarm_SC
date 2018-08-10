using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
namespace NeoFarmSC
{
    public class SC01_Produce
    {

        #region 可回溯的随机算法
        //1.不要进行类型转换
        //2.数组不能进行复杂操作，如+= 等

        //    private int[] SeedArray = new int[56];
        //    private int inext;
        //    private int inextp;


        //inext  ->56
        //inextp ->57,
        static public void InitRandom(int Seed, int[] SeedArray)
        {
            SeedArray[56] = 0;
            SeedArray[57] = 0;

            int absSeed = Seed < 0 ? Seed * -1 : Seed;
            int num1 = 161803398 - (Seed == int.MinValue ? int.MaxValue : absSeed);
            SeedArray[55] = num1;
            int num2 = 1;
            for (int index1 = 1; index1 < 55; ++index1)
            {
                int index2 = 21 * index1 % 55;
                SeedArray[index2] = num2;
                num2 = num1 - num2;
                if (num2 < 0)
                    num2 += int.MaxValue;
                num1 = SeedArray[index2];
            }

            for (int index1 = 1; index1 < 5; ++index1)
            {
                for (int index2 = 1; index2 < 56; ++index2)
                {
                    SeedArray[index2] -= SeedArray[1 + (index2 + 30) % 55];
                    if (SeedArray[index2] < 0)
                        SeedArray[index2] += int.MaxValue;
                }
            }

            SeedArray[56] = 0;
            SeedArray[57] = 21;
            Seed = 1;
        }


        static public BigInteger Sample(int[] SeedArray)
        {
            var internalInt = InternalSample(SeedArray);

            return BigInteger.Multiply(internalInt, 161803398);

        }

        static public int InternalSample(int[] SeedArray)
        {
            int _inext = SeedArray[56];
            int _inextp = SeedArray[57];
            int index1;
            if ((index1 = _inext + 1) >= 56)
                index1 = 1;
            int index2;
            if ((index2 = _inextp + 1) >= 56)
                index2 = 1;
            int num = SeedArray[index1] - SeedArray[index2];
            if (num == int.MaxValue)
                --num;
            if (num < 0)
                num += int.MaxValue;
            SeedArray[index1] = num;
            SeedArray[56] = index1;
            SeedArray[57] = index2;
            return num;
        }


        static public int RandomNext(int minValue, int maxValue, int[] SeedArray)
        {
            long num = maxValue - minValue;
            if (num <= int.MaxValue)
            {
                var si = Sample(SeedArray);
                return (int)(si % maxValue) + minValue; 
            }
            return -9999;
        }
        #endregion

        public static Int32[] Main(Int32 time, Int32 seedId, Int32 minOne, Int32 maxOne)
        {
            //magic code for hash
            string magicstr = "2018.8.9";

            if (minOne <= 0 || maxOne >= 100)
            {
                throw new Exception("Invalid value: minOne or maxOne!!!! ");
            }
            //产出 这包种子
            int[] results = new int[12];
            //height + seedId

            //加入一个魔数,永远无法预测结果
            int[] arrRand = new int[58];
            InitRandom(100, arrRand);
            var magicSeed = RandomNext(1, 100000, arrRand);
            //
            arrRand = new int[58];
            InitRandom(time + seedId + magicSeed, arrRand); 
            int all = 100;
            for (int i = 0; i < 11; i++)
            {
                var r = RandomNext(minOne, (int)(maxOne * all / 100), arrRand);

                all = all - r;
                results[i] = r;
            }
            results[11] = all;

            arrRand = new int[58];
            InitRandom(seedId, arrRand);
            for (int i = 0; i < 12; i++)
            {
                var r = RandomNext(0, 12, arrRand);
                var r2 = RandomNext(0, 12, arrRand);
                var t = results[r];
                results[r] = results[r2];
                results[r2] = t;
            }

            return results;
        }
    }
}
