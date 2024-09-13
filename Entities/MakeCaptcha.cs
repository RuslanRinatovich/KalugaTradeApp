using System;
using System.Drawing;
using System.IO;
using AvalonBitmap = Avalonia.Media.Imaging.Bitmap;

namespace TradeApp
{
    public class MakeCaptcha
    {
        /// <summary>
        /// Генерирует картинку, содержащую текст капчи
        /// </summary>
        /// <param name="width">ширина изображения</param>
        /// <param name="height">высота изображения</param>
        /// <param name="symbolCount">длина капчи</param>
        /// <returns>возвращает картинку и текст капчи</returns>
        public static (AvalonBitmap image, string captcha) CreateImage(int width, int height, int symbolCount)
        {
            Random rnd = new Random();

            //Создадим изображение
            Bitmap result = new Bitmap(width, height);

            //Добавим различные цвета
            Brush[] colors = { Brushes.Black,
                     Brushes.Red,
                     Brushes.RoyalBlue,
                     Brushes.Green };

            //Укажем где рисовать
            Graphics g = Graphics.FromImage((System.Drawing.Image)result);

            //Пусть фон картинки будет серым
            g.Clear(Color.Gray);

            //Сгенерируем текст
            char symbol;
            // нбор допустимых символов
            string alphabet = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
           // текст капчи
            string captcha = "";
            // размер поля для одного символа
            int h = width / symbolCount;
            for (int i = 0; i < symbolCount; ++i)
            {
                // генерируем размер буквы
                int size = rnd.Next(20, h);
                // выбираем любой символ из допустимого набора
                symbol = alphabet[rnd.Next(alphabet.Length)];
                // текст капчи
                captcha += symbol;
                // генерируем позиции рисования символа
                int Ypos = rnd.Next(height - size - 20);
                int Xpos = rnd.Next(i * h, (i + 1) * h - size);

                //Нарисуем сгенерированный символ
                g.DrawString(symbol.ToString(),
                             new Font("Arial", size),
                             colors[rnd.Next(colors.Length)],
                             new PointF(Xpos, Ypos));
                //Добавим немного помех
                ///////Линии из углов
                //Устанавливаем цвет линии
                Pen skyBluePen = new Pen(colors[rnd.Next(colors.Length)]);
                // Устанавливаем толщину линии
                skyBluePen.Width = rnd.Next(2,5);

                g.DrawLine(skyBluePen,
                       new Point(Xpos + 5, Ypos + 5),
                       new Point(Xpos + size + 5, Ypos + size + 5));
            }
         
            ////Белые точки
            for (int i = 0; i < width; ++i)
                for (int j = 0; j < height; ++j)
                    if (rnd.Next() % 20 == 0)
                        result.SetPixel(i, j, Color.White);

            return (BitmapToImageSource(result), captcha);
        }
        /// <summary>
        /// Конвертирует Bitmap в BitmapImage для использования в качестве источника изображения для Image
        /// </summary>
        /// <param name="bitmap">Картинка созданная программным образом</param>
        /// <returns>BitmapImage, который можно использовать в качестве ImageSource для Image</returns>
        private static AvalonBitmap BitmapToImageSource(Bitmap bitmap)
        {
                using (var ms = new MemoryStream(ToByteArray(bitmap)))
                {
                  return new AvalonBitmap(ms);;
                }
        }

        private static byte[] ToByteArray(Image image)
            {
                    using(MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                    return ms.ToArray();
                }
            }

            
    }
   
}
