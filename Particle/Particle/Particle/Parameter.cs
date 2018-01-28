///作成日：2016.12.21
///作成者：柏
///作成内容： 常数管理用クラス
///最後修正内容：花火用常数整理追加
///最後修正者：柏
///最後修正日：2017.1.16

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Particle
{
    static class Parameter
    {
        public const int ScreenWidth = 1280;      //windowsのSize
        public const int ScreenHeight = 720;      //windowsのSize
        public const int FireworksCount = 16;       //2016.12.22  by柏　一回の爆発生成してくる花火の個数
        public const int FireworksReBurnCount = 2;  //2016.12.22  by柏  再爆発回数
        public const float FireworksSpeedDownRate = 0.95f;  //2016.12.22  by柏　by柏　花火の減速率
        public const int FireworksStartSpeedMin = 20;  //2016.12.22  by柏　花火生成時の最小初速度
        public const int FireworksStartSpeedMax = 35;  //2016.12.22  by柏　花火生成時の最大初速度
        public const int BurnStartSpeed = 5;    //2017.1.16 by柏 子花火の爆発初速度
        public const float BurnTime = 0.8f;    //2017.1.16 by柏 子花火の爆発時間
        public const float BurnNextTime = 0.25f;    //2017.1.16 by柏 花火の再爆発間隔
        public const float NewFireworksTime = 0.8f;    //2017.1.16 by柏 花火の生成間隔

    }
}
