using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;  //コンテンツ利用
using Microsoft.Xna.Framework.Audio;    //WAVデータ
using System.Diagnostics;               //Assert

namespace Particle
{
    public class Sound
    {
        private ContentManager contentManager;
        private Dictionary<string, SoundEffect> soundEffects;           //WAV管理用

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="content">Game1のコンテンツ管理者</param>
        public Sound(ContentManager content)
        {
            //Game1のコンテンツ管理者と繋ぐ
            contentManager = content;

            soundEffects = new Dictionary<string, SoundEffect>();

        }

        /// <summary>
        /// Assert用メッセージ
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <returns></returns>
        private string ErrorMessage(string name)
        {
            return "再生する音データのアセット名（" + name + "）がありません\n"
                +
                  "アセット名の確認、Dictionaryに登録されているか確認してください\n";
        }
        
        #region WAV関連
        /// <summary>
        /// SE(wav)の読み込み
        /// </summary>
        /// <param name="name">wavのアセット名</param>
        /// <param name="filepath">ファイルのパス</param>
        public void LoadSE(string name, string filepath = "./")
        {
            //すでに登録されていれば何もしない
            if (soundEffects.ContainsKey(name)) { return; }

            //読み込みと追加
            soundEffects.Add(name, contentManager.Load<SoundEffect>(filepath + name));
        }

        /// <summary>
        /// 単純SE再生（連続で呼ばれた場合、おとは重なる。途中停止不可）
        /// </summary>
        /// <param name="name"></param>
        public void PlaySE(string name)
        {
            //WAV用ディクションナリをチェック
            Debug.Assert(soundEffects.ContainsKey(name), ErrorMessage(name));

            soundEffects[name].Play();
        }

        #endregion

        /// <summary>
        /// 解放
        /// </summary>
        public void Unload() { soundEffects.Clear(); }

    }
}
