﻿using System.Threading;
using NAPS2.ImportExport.Images;
using NAPS2.ImportExport.Pdf;

namespace NAPS2.ImportExport;

public class ScannedImageImporter : IScannedImageImporter
{
    private readonly IScannedImageImporter _pdfImporter;
    private readonly IScannedImageImporter _imageImporter;

    public ScannedImageImporter(IPdfImporter pdfImporter, IImageImporter imageImporter)
    {
        _pdfImporter = pdfImporter;
        _imageImporter = imageImporter;
    }

    public AsyncSource<ProcessedImage> Import(string filePath, ImportParams importParams, ProgressHandler progressCallback, CancellationToken cancelToken)
    {
        if (filePath == null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }
        switch (Path.GetExtension(filePath).ToLowerInvariant())
        {
            case ".pdf":
                return _pdfImporter.Import(filePath, importParams, progressCallback, cancelToken);
            default:
                return _imageImporter.Import(filePath, importParams, progressCallback, cancelToken);
        }
    }
}