///作成日：2016.12.21
///作成者：柏
///作成内容： 描画用クラス
///最後修正内容：。。
///最後修正者：。。
///最後修正日：。。

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Particle
{
    class Renderer
    {
        private ContentManager contentManager;  //コンテンツ管理者
        private GraphicsDevice graphicsDevice;  //グラフィック機器
        private SpriteBatch spriteBatch;        //スプライト一括

        //Dictionaryで複数の画像を管理
        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="content">Game1のコンテンツ管理者</param>
        /// <param name="graphics">Game1のグラフィック機器</param>
        public Renderer(ContentManager content, GraphicsDevice graphics)
        {
            contentManager = content;
            graphicsDevice = graphics;
            spriteBatch = new SpriteBatch(graphicsDevice);
        }

        /// <summary>
        /// 画像の読み込み
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="filepath">ファイルまでのパス</param>
        public void LoadTexture(string name, string filepath = "./")
        {
            //ガード節
            //Dictionaryへの２重登録を回避
            if (textures.ContainsKey(name))
            {
#if DEBUG   //DEBUGモードの時のみ有効
                Console.WriteLine("この" + name + "はKeyで、すでに登録しています");
#endif
                //処理終了
                return;
            }

            //画像の読み込みとDictionaryにアセット名と画像を追加


            textures.Add(name, contentManager.Load<Texture2D>(filepath + name));
        }

        /// <summary>
        /// アンロード
        /// </summary>
        public void Unload()
        {
            //Dictionary登録情報をクリア
            textures.Clear();
        }

        /// <summary>
        /// 描画処理(拡大縮小)
        /// </summary>
        /// <param name="name">アセット</param>
        /// <param name="position">位置</param>
        /// <param name="scale">拡大率</param>
        /// <param name="alpha">透明度</param>
        public void DrawTexture(string name, Vector2 position, float scale, float alpha = 1.0f)
        {
            //登録されているキーがなければエラー表示
            Debug.Assert(
                textures.ContainsKey(name),
                "アセット名が間違えていませんか？\n" +
                "大文字小文字間違ってませんか？\n" +
                "LoadTextureで読み込んでますか？\n" +
                "プログラムを確認してください");

            if (IsInScreen(position, textures[name]))
                spriteBatch.Draw(
                    textures[name],
                    position,
                    new Rectangle(0, 0, textures[name].Width, textures[name].Height),
                    Color.White * alpha,
                    0.0f,
                    Vector2.Zero,
                    scale,
                    SpriteEffects.None,
                    0);
        }

        
        /// <summary>
        /// 描画処理(拡大縮小,色付け)
        /// </summary>
        /// <param name="name">アセット</param>
        /// <param name="position">位置</param>
        /// <param name="color"></param>
        /// <param name="scale">拡大率</param>
        /// <param name="alpha">透明度</param>
        public void DrawTexture(string name, Vector2 position, Color color, float scale, float alpha = 1.0f)
        {
            //登録されているキーがなければエラー表示
            Debug.Assert(
                textures.ContainsKey(name),
                "アセット名が間違えていませんか？\n" +
                "大文字小文字間違ってませんか？\n" +
                "LoadTextureで読み込んでますか？\n" +
                "プログラムを確認してください");

            if (IsInScreen(position, textures[name]))
                spriteBatch.Draw(
                    textures[name],
                    position,
                    new Rectangle(0, 0, textures[name].Width, textures[name].Height),
                    color * alpha,
                    0.0f,
                    Vector2.Zero,
                    scale,
                    SpriteEffects.None,
                    0);
        }


        private bool IsInScreen(Vector2 pos, Texture2D textrue)
        {
            int width = textrue.Width;
            int height = textrue.Height;
            return (pos.X >= -width && pos.X <= Parameter.ScreenWidth) && (pos.Y >= -height && pos.Y <= Parameter.ScreenHeight);
        }

        public void Begin() {
            spriteBatch.Begin();
        }
        public void End() {
            spriteBatch.End();
        }
    }
}
