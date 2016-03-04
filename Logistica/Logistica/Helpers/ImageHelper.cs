using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logistica.Helpers
{
    public static class ImageHelper
    {
        public static Image GetThumbnail(this HttpPostedFileWrapper avatar, int maxPixels)
        {

            Bitmap original = null;

            #region Get Bitmap
            original = Bitmap.FromStream(avatar.InputStream) as Bitmap;
            #endregion

            #region Verify Orientation depending by device
            if (original.PropertyIdList.Contains(0x112)) //0x112 = Orientation
            {
                var prop = original.GetPropertyItem(0x112);
                if (prop.Type == 3 && prop.Len == 2)
                {
                    UInt16 orientationExif = BitConverter.ToUInt16(original.GetPropertyItem(0x112).Value, 0);
                    if (orientationExif == 8)
                    {
                        original.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    }
                    else if (orientationExif == 3)
                    {
                        original.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    }
                    else if (orientationExif == 6)
                    {
                        original.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    }
                }
            }
            #endregion

            string fName = string.Concat(Guid.NewGuid().ToString().Replace("-", string.Empty), ".png");

            Size thumbnailSize = GetThumbnailSize(original, maxPixels);

            Image thumb = original.GetThumbnailImage(thumbnailSize.Width, thumbnailSize.Height, () => false, IntPtr.Zero);


            return thumb;
        }

        public static  byte[] ImageToByteArray(this Image imageIn)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
        }

        private static Size GetThumbnailSize(Image original, int maxPixels)
        {
            // Maximum size of any dimension.

            // Width and height.
            int originalWidth = original.Width;
            int originalHeight = original.Height;

            // Compute best factor to scale entire image based on larger dimension.
            double factor;
            if (originalWidth > originalHeight)
            {
                factor = (double)maxPixels / originalWidth;
            }
            else
            {
                factor = (double)maxPixels / originalHeight;
            }

            // Return thumbnail size.
            return new Size((int)(originalWidth * factor), (int)(originalHeight * factor));
        }
    }
}
