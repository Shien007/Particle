///作成日：2016.12.21
///作成者：柏
///作成内容： パーティクルクラス
///最後修正内容：コードの整理　＋　コメント追加
///最後修正者：柏
///最後修正日：2016.12.22

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Particle
{
    class Particle
    {
        private static Random rnd = new Random();
        private string name;        //花火リソースの名前
        private float size;         //花火の大きさ
        private float gravity;      //重力
        private int fireCount;      //花火の爆発回数管理
        private Timer fireContinueTimer;     //複数爆発のタイミング管理
        private Timer burntimer;        //子花火の爆発時間管理
        private Vector2 position;       //打ち上げと爆発の位置
        private Vector2 velocity;       //花火の速度
        private Color color;    //花火の色

        private List<Particle> fireworks;       //子花火管理リストに追加
        private List<Vector2> fireVelocitys;    //花火の飛び方向の保存用
        private bool isDead;        //終わったかどうか
        private bool burn;          //爆発したかどうか
        private bool isVisible;       //見えるかどうか
        private Sound sound;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="velocity">移動方向と速度</param>
        /// <param name="burn">初期値は爆発した</param>
        /// <param name="size">初期サイズは0.3倍の画像の大きさ</param>
        public Particle(Vector2 velocity, Sound sound, bool burn = true, float size = 0.3f)
        {
            name = "firework";
            this.size = size;
            gravity = 1.0f;
            fireCount = 0;
            fireContinueTimer = new Timer(Parameter.BurnNextTime);    //花火の再爆発時間設定
            burntimer = new Timer(Parameter.BurnTime);        //子花火の爆発時間を設定
            this.sound = sound;
            
            //花火は窓口の一番下の任意位置で生成するのに設定
            position = new Vector2(rnd.Next(Parameter.ScreenWidth), Parameter.ScreenHeight); 
            this.velocity = velocity;

            fireworks = new List<Particle>();
            fireVelocitys = new List<Vector2>();
            isDead = false;
            this.burn = burn;
            isVisible = true;

            InitializeFireVelocity();   //花火の飛び方向を登録
        }


        /// <summary>
        /// 花火の飛び方向の登録
        /// </summary>
        private void InitializeFireVelocity() {
            float x = 0, y = 0;
            int fireMax = Parameter.FireworksCount;     //一回の爆発生成してくる花火の個数のリネーム
            for (int i = 0; i < fireMax; i++) {
                x = (float)Math.Cos(Math.PI * 2 / fireMax * i);     //横方向速度(円を花火の個数で分割して計算)
                y = (float)Math.Sin(Math.PI * 2 / fireMax * i);     //縦方向速度(円を花火の個数で分割して計算)
                fireVelocitys.Add(new Vector2(x, y));       //登録
            }
        }

        /// <summary>
        /// 花火の追加
        /// </summary>
        private void AddFireworks() {
            List<Particle> fireList = new List<Particle>();
            for (int i = 0; i < fireVelocitys.Count; i++)
            {
                Particle newP = new Particle(Parameter.BurnStartSpeed * fireVelocitys[i], sound);
                newP.color = color;
                newP.position = position;   //爆発座標を設定
                newP.SetFireCount();       //爆発回数の上限で設定（子花火の再爆発を防ぐため）
                fireList.Add(newP);
            }
            fireworks.AddRange(fireList);   //子花火管理リストに追加
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            Move();     //花火の移動処理
            ReBurn();   //花火の再爆発処理
            if (CheckBurnAble()) { Burn(); }
            BurnedFireUpdate();         //爆発した花火の更新
        }

        /// <summary>
        /// 移動処理
        /// </summary>
        private void Move() {
            position += velocity;
            if (burn) { return; }
            //爆発しなかった場合打ち上げの減速処理
            float speedDownRate = Parameter.FireworksSpeedDownRate;     //減速率のリネーム
            velocity *= speedDownRate;   
        }

        /// <summary>
        /// 爆発した花火の更新処理
        /// </summary>
        private void BurnedFireUpdate() {
            if (!burn) { return; }      //爆発しなかったら更新しない
            burntimer.Update();     //爆発時間の更新はじめ
            if (burntimer.IsTime()) { isVisible = false; }    //爆発時間終わると見えないようになる
            fireworks.RemoveAll(f => !f.isVisible);       //見えない子花火を削除

            //子花火の更新
            foreach (var f in fireworks) {
                f.Update();
            }
            position.Y += gravity;      //Ｙ座標に重力加算

            //サイズと重力自分自身の加算
            size *= 1.06f;      //サイズ大きくなる
            gravity *= 1.03f;    //重力大きくなる

            isDead = (fireworks.Count()) == 0 ? true : false;       //子花火全部爆発終わると親花火は死ぬ
        }

        //再爆発処理
        private void ReBurn() {
            if (!burn) { return; }      //爆発状態じゃないと処理しない
            if (fireCount >= Parameter.FireworksReBurnCount) { return; }  //再爆発回数は上限だったら処理しない

            fireContinueTimer.Update();
            if (fireContinueTimer.IsTime()) {
                fireContinueTimer.Initialize();
                fireCount++;    //爆発回数　＋１
                burn = false;   //爆発できるようになる
            }
        }

        /// <summary>
        /// 爆発できるかどうかチェック
        /// </summary>
        /// <returns></returns>
        private bool CheckBurnAble() {
            if (burn) { return false; }     //親花火もう爆発した状態だったら、爆発できない
            if (velocity.Y < -1.5f) { return false; }
            velocity.Y = 0;     //親花火速度０で初期化
            burn = true;        //親花火爆発したに設定
            isVisible = false;    //親花火見えないに設定
            return true;
        }

        /// <summary>
        /// 爆発処理
        /// </summary>
        private void Burn() {
            AddFireworks();     //子花火の追加
            if (fireCount > 0) { return; }
            sound.PlaySE("fireburn");
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer">描画用クラス</param>
        public void Draw(Renderer renderer) {
            DrawFires(renderer);    //子花火の描画処理
            if (!isVisible) { return; }

            //爆発した花火どんどん透明になる
            float alpha = burn ? burntimer.Rate() : 1.0f;
            renderer.DrawTexture(name, position, color, size, alpha);
        }

        /// <summary>
        /// 子花火の描画処理
        /// </summary>
        /// <param name="renderer">描画用クラス</param>
        private void DrawFires(Renderer renderer) {
            if (fireworks.Count() == 0) { return; }
            fireworks.ForEach(f => f.Draw(renderer));
        }

        /// <summary>
        /// 再爆破回数は上限に設定
        /// </summary>
        private void SetFireCount() {
            fireCount = Parameter.FireworksReBurnCount; 
        }

        /// <summary>
        /// 死亡状態のゲット
        /// </summary>
        public bool IsDead {
            get { return isDead; }
        }

        /// <summary>
        /// 色の変更
        /// </summary>
        public Color SetColor {
            set { color = value; }
        }
    }
}
