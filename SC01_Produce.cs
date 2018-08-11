﻿using System;
using System.Numerics;

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
            return (int) (si % maxValue) + minValue;
        }

        return -9999;
    }

    #endregion

    public static Int32[] Main(Int32 time, Int32 maxProduceId)
    {
        //magic code for hash
        string magicstr = "2018.8.9";

        int[] results = new int[20];
        int[] seedArray = new int[58];
        InitRandom(time, seedArray);
        
        int min = 0;
        //step.1 生成最大的果实下标
        int max = RandomNext(2, 90, seedArray);

        //先瓜分总额100
        int all = 100;
        for (int i = 0; i < 9; i++)
        {
            var r = RandomNext(min, (int) (max * all / 100), seedArray);
            all = all - r;
            results[i] = r;
        }

        results[9] = all;


        //打乱顺序
        for (int i = 0; i < 10; i++)
        {
            var r = RandomNext(0, 10, seedArray);
            var r2 = RandomNext(0, 10, seedArray);
            var t = results[r];
            results[r] = results[r2];
            results[r2] = t;
        }


        //产出果实

        for (int i = 10; i < 19; i++)
        {
            var r = RandomNext(1, maxProduceId, seedArray);
            results[i] = r;
        }

        return results;
    }
}