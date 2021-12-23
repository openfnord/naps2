﻿using System.Drawing;
using NAPS2.Images.Gdi;
using NAPS2.Scan;
using ZXing.Rendering;

namespace NAPS2.Sdk.Samples;

public class FileStorageSample
{
    public static async Task Run()
    {
        ScanningContext scanningContext = new ScanningContext(new GdiImageContext());

        // To save memory, we can store scanned images on disk after initial processing.
        // This will put files in the system temp folder by default, which can be
        // overriden by changing FileStorageManager.Current.
        // TODO: Change
        //imageContext.ConfigureBackingStorage<FileStorage>();

        var controller = new ScanController(scanningContext);
        var device = (await controller.GetDeviceList()).First();
        var options = new ScanOptions
        {
            Device = device,
            Dpi = 300,
            NoUI = true
        };
            
        // We can wait for the entire scan to complete and not worry about using an
        // excessive amount of memory, since it is all stored on disk until rendered.
        // This is just for illustration purposes; in real code you usually want to
        // process images as they come rather than waiting for the full scan.
        List<RenderableImage> renderableImages = await controller.Scan(options).ToList();

        try
        {
            foreach (var renderableImage in renderableImages)
            {
                // This seamlessly loads the image data from disk.
                using Bitmap bitmap = renderableImage.RenderToBitmap();
                // TODO: Do something with the bitmap
            }
        }
        finally
        {
            foreach (var scannedImage in renderableImages)
            {
                // This cleanly deletes any data from the filesystem.
                scannedImage.Dispose();
            }
        }
    }
}