using Microsoft.Extensions.Configuration;
using Pursue.Extension.Cache;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;

namespace Pursue.Extension.Cryptologys
{
    public sealed class UserVerifyCode : IUserVerifyCode
    {
        private static readonly Color[] Colors = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Brown, Color.DarkBlue };
        private static readonly char[] Chars = { '1', '2', '3', '4', '5', '6', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'H', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        private const string REDIS_VERIFY_CODE_KTY = "pageCode";
        private const int REDIS_TTL_KEY = 60;

        private const int IMG_WIDTH = 90;
        private const int IMG_HEIGHT = 35;
        private const int IMG_FONTSIZE = 25;

        private readonly string _fontPath;

        private readonly FontFamily _fontFamily;

        private readonly RedisClient _redisClient;

        public UserVerifyCode(IConfiguration configuration)
        {
            _redisClient = CacheFactory.GetRedisClient();

            // 切记需要配置字体文件
            _fontPath = Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), configuration.GetSection("Startup:FontPath").Get<string>());
            _fontFamily = new FontCollection().Add(_fontPath);
        }

        /// <summary>
        /// 生成页面验证码
        /// </summary>
        /// <returns></returns>
        public string CreateVerifyCode()
        {
            var (code, btyes) = CreateVerifyCode(4, IMG_WIDTH, IMG_HEIGHT, IMG_FONTSIZE);

            _redisClient.Set($"{REDIS_VERIFY_CODE_KTY}:{code}", code, REDIS_TTL_KEY);

            return $"data:images/gif;base64,{Convert.ToBase64String(btyes)}";
        }

        /// <summary>
        /// 验证页面验证码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool VerifyCode(string code)
        {
            var redisKey = $"{REDIS_VERIFY_CODE_KTY}:{code.ToUpper()}";
            var result = _redisClient.Exists(redisKey);
            if (result)
            {
                _redisClient.Del(redisKey);
            }
            return result;
        }

        /// <summary>
        /// 生成验证码图片流文件
        /// </summary>
        /// <param name="CodeLength"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="FontSize"></param>
        /// <returns></returns>
        private (string code, byte[] bytes) CreateVerifyCode(int CodeLength, int Width, int Height, int FontSize)
        {
            var r = new Random();

            var code = string.Empty;
            for (int i = 0; i < CodeLength; i++)
            {
                code += Chars[r.Next(Chars.Length)].ToString();
            }

            using var image = new Image<Rgba32>(Width, Height);

            // 字体
            var font = _fontFamily.CreateFont(FontSize);

            image.Mutate(ctx =>
            {
                // 白底背景
                ctx.Fill(Color.White);

                // 画验证码
                for (int i = 0; i < code.Length; i++)
                {
                    ctx.DrawText(code[i].ToString(), font, Colors[r.Next(Colors.Length)], new PointF(20 * i + 10, r.Next(2, 12)));
                }

                // 画干扰线
                for (int i = 0; i < 6; i++)
                {
                    var pen = Pens.DashDot(Colors[r.Next(Colors.Length)], 1);
                    var p1 = new PointF(r.Next(Width), r.Next(Height));
                    var p2 = new PointF(r.Next(Width), r.Next(Height));

                    ctx.DrawLine(pen, p1, p2);
                }

                // 画噪点
                for (int i = 0; i < 60; i++)
                {
                    var pen = Pens.DashDot(Colors[r.Next(Colors.Length)], 1);
                    var p1 = new PointF(r.Next(Width), r.Next(Height));
                    var p2 = new PointF(p1.X + 1f, p1.Y + 1f);

                    ctx.DrawLine(pen, p1, p2);
                }
            });

            using var ms = new MemoryStream();

            // 格式自定义
            image.SaveAsGif(ms);

            return (code, ms.ToArray());
        }
    }
}
