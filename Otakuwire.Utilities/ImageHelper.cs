using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

namespace Otakuwire.Utilities
{
    public class ImageHelper
    {
        // Copied from http://stackoverflow.com/questions/27921/what-is-the-best-way-to-create-a-thumbnail-using-asp-net

        public static void ResizeImage(string fileNameInput, string fileNameOutput, double resizeHeight, double resizeWidth, ImageFormat outputFormat)
        {
            using (System.Drawing.Image photo = new Bitmap(fileNameInput))
            {
                double aspectRatio = (double)photo.Width / photo.Height;
                double boxRatio = resizeWidth / resizeHeight;
                double scaleFactor = 0;

                if (photo.Width < resizeWidth && photo.Height < resizeHeight)
                {
                    // Keep the image the same size since it is already smaller than our max width/height
                    scaleFactor = 1.0;
                }
                else
                {
                    // Compare the ratios to determine if the picture should be resized by height or width.
                    if (boxRatio > aspectRatio)
                        scaleFactor = resizeHeight / photo.Height;
                    else
                        scaleFactor = resizeWidth / photo.Width;
                }

                int newWidth = (int)(photo.Width * scaleFactor);
                int newHeight = (int)(photo.Height * scaleFactor);

                using (Bitmap bmp = new Bitmap(newWidth, newHeight))
                {
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                        g.DrawImage(photo, 0, 0, newWidth, newHeight);

                        if (ImageFormat.Png.Equals(outputFormat))
                        {
                            bmp.Save(fileNameOutput, outputFormat);
                        }
                        else if (ImageFormat.Jpeg.Equals(outputFormat))
                        {
                            ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();
                            EncoderParameters encoderParameters;
                            using (encoderParameters = new System.Drawing.Imaging.EncoderParameters(1))
                            {
                                // use jpeg info[1] and set quality to 90
                                encoderParameters.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
                                bmp.Save(fileNameOutput, info[1], encoderParameters);
                            }
                        }
                    }
                }
            }
        }

    }
}
