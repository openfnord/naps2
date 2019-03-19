﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAPS2.Images;
using NAPS2.ImportExport;

namespace NAPS2.WinForms
{
    public class ImageClipboard
    {
        private readonly BitmapRenderer bitmapRenderer;

        public ImageClipboard()
        {
            bitmapRenderer = new BitmapRenderer();
        }

        public ImageClipboard(BitmapRenderer bitmapRenderer)
        {
            this.bitmapRenderer = bitmapRenderer;
        }


        public async Task Write(IEnumerable<ScannedImage> images, bool includeBitmap)
        {
            var imageList = images.ToList();
            if (imageList.Count == 0)
            {
                return;
            }

            // Fast path for copying within NAPS2
            var ido = GetDataObject(imageList);
            Clipboard.SetDataObject(ido);

            // Slow path for more full-featured copying
            if (includeBitmap)
            {
                using (var firstBitmap = await bitmapRenderer.Render(imageList[0]))
                {
                    ido.SetData(DataFormats.Bitmap, true, new Bitmap(firstBitmap));
                    ido.SetData(DataFormats.Rtf, true, await RtfEncodeImages(firstBitmap, imageList));
                }
                Clipboard.SetDataObject(ido);
            }
        }

        public IDataObject GetDataObject(IEnumerable<ScannedImage> imageList)
        {
            IDataObject ido = new DataObject();
            ido.SetData(typeof(DirectImageTransfer), new DirectImageTransfer(imageList));
            return ido;
        }

        public DirectImageTransfer Read()
        {
            var ido = Clipboard.GetDataObject();
            if (ido != null && ido.GetDataPresent(typeof(DirectImageTransfer).FullName))
            {
                return (DirectImageTransfer)ido.GetData(typeof(DirectImageTransfer).FullName);
            }
            return null;
        }

        public bool CanRead
        {
            get
            {
                var ido = Clipboard.GetDataObject();
                return ido != null && ido.GetDataPresent(typeof(DirectImageTransfer).FullName);
            }
        }

        private async Task<string> RtfEncodeImages(Bitmap firstBitmap, List<ScannedImage> images)
        {
            var sb = new StringBuilder();
            sb.Append("{");
            // TODO: Is this the right format?
            if (!AppendRtfEncodedImage(firstBitmap, firstBitmap.RawFormat, sb, false))
            {
                return null;
            }
            foreach (var img in images.Skip(1))
            {
                using (var bitmap = await bitmapRenderer.Render(img))
                {
                    // TODO: Is this the right format?
                    if (!AppendRtfEncodedImage(bitmap, bitmap.RawFormat, sb, true))
                    {
                        break;
                    }
                }
            }
            sb.Append("}");
            return sb.ToString();
        }

        private static bool AppendRtfEncodedImage(Image image, ImageFormat format, StringBuilder sb, bool par)
        {
            const int maxRtfSize = 20 * 1000 * 1000;
            using (var stream = new MemoryStream())
            {
                image.Save(stream, format);
                if (sb.Length + stream.Length * 2 > maxRtfSize)
                {
                    return false;
                }

                if (par)
                {
                    sb.Append(@"\par");
                }
                sb.Append(@"{\pict\pngblip\picw");
                sb.Append(image.Width);
                sb.Append(@"\pich");
                sb.Append(image.Height);
                sb.Append(@"\picwgoa");
                sb.Append(image.Width);
                sb.Append(@"\pichgoa");
                sb.Append(image.Height);
                sb.Append(@"\hex ");
                // Do a "low-level" conversion to save on memory by avoiding intermediate representations
                stream.Seek(0, SeekOrigin.Begin);
                int value;
                while ((value = stream.ReadByte()) != -1)
                {
                    int hi = value / 16, lo = value % 16;
                    sb.Append(GetHexChar(hi));
                    sb.Append(GetHexChar(lo));
                }
                sb.Append("}");
            }
            return true;
        }

        private static char GetHexChar(int n) => (char)(n < 10 ? '0' + n : 'A' + (n - 10));
    }
}
