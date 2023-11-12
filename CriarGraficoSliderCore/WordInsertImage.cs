using System.Drawing;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

namespace CriarGraficoSliderCore
{
    public class WordInsertImage
    {
        public static void InsertAPicture(WordprocessingDocument wordDoc, string imageFileName, string chave)
        {

            MainDocumentPart mainPart = wordDoc.MainDocumentPart;

            AddImageToBody(mainPart, chave, imageFileName);
            AddImageToTable(mainPart, chave, imageFileName);
            AddImageToTableHeader(mainPart, chave, imageFileName);
            AddImageToTableFooter(mainPart, chave, imageFileName);
            AddImageToHeader(mainPart, chave, imageFileName);
            AddImageToFooter(mainPart, chave, imageFileName);
            AddImageToTextBox(mainPart, chave, imageFileName);

        }

        public static (int Width, int Height) GetImageSize(string imagePath, float widthInCentimeters)
        {
            using (var image = Image.FromFile(imagePath))
            {
                int width = image.Width;
                int height = image.Height;

                var widthcm = ConvertPixelsToCm(width);
                var heightcm = ConvertPixelsToCm(height);

                if (widthcm > widthInCentimeters)
                {
                    var percent = widthInCentimeters / widthcm;

                    widthcm = widthcm * percent;
                    heightcm = heightcm * percent;
                }

                width = ConvertCmToEmu(widthcm);
                height = ConvertCmToEmu(heightcm);

                return (width, height);
            }
        }

        public static float ConvertPixelsToCm(int pixels)
        {
            const float pixelsPerInch = 96f; // Valor padrão de pixels por polegada
            const float inchesPerCm = 2.54f; // Valor de polegadas por centímetro

            float centimeters = (pixels * inchesPerCm) / pixelsPerInch; // Converte pixels para centímetros

            return centimeters;
        }

        public static int ConvertCmToEmu(float centimeters)
        {
            int emus = (int)(centimeters * 360000); // Converte centímetros para EMUs (1 cm = 360000 EMUs)

            return emus;
        }

        private static Drawing ImageToBody(string relationshipId, string imageFileName, float widthInCentimeters)
        {

            var tamanho = GetImageSize(imageFileName, widthInCentimeters);

            // Define the reference of the image.
            var element =
                 new Drawing(
                     new DW.Inline(
                         new DW.Extent() { Cx = tamanho.Width, Cy = tamanho.Height },
                         new DW.EffectExtent()
                         {
                             LeftEdge = 0L,
                             TopEdge = 0L,
                             RightEdge = 0L,
                             BottomEdge = 0L
                         },
                         new DW.DocProperties()
                         {
                             Id = (UInt32Value)1U,
                             Name = "Picture 1"
                         },
                         new DW.NonVisualGraphicFrameDrawingProperties(
                             new A.GraphicFrameLocks() { NoChangeAspect = true }),
                         new A.Graphic(
                             new A.GraphicData(
                                 new PIC.Picture(
                                     new PIC.NonVisualPictureProperties(
                                         new PIC.NonVisualDrawingProperties()
                                         {
                                             Id = (UInt32Value)0U,
                                             Name = imageFileName
                                         },
                                         new PIC.NonVisualPictureDrawingProperties()),
                                     new PIC.BlipFill(
                                         new A.Blip(
                                             new A.BlipExtensionList(
                                                 new A.BlipExtension()
                                                 {
                                                     Uri =
                                                        "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                 })
                                         )
                                         {
                                             Embed = relationshipId,
                                             CompressionState = A.BlipCompressionValues.Print
                                         },
                                         new A.Stretch(
                                             new A.FillRectangle())),
                                     new PIC.ShapeProperties(
                                         new A.Transform2D(
                                             new A.Offset() { X = 0L, Y = 0L },
                                             new A.Extents() { Cx = tamanho.Width, Cy = tamanho.Height }),
                                         new A.PresetGeometry(
                                             new A.AdjustValueList()
                                         )
                                         { Preset = A.ShapeTypeValues.Rectangle }))
                             )
                             { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                     )
                     {
                         DistanceFromTop = (UInt32Value)0U,
                         DistanceFromBottom = (UInt32Value)0U,
                         DistanceFromLeft = (UInt32Value)0U,
                         DistanceFromRight = (UInt32Value)0U,
                         EditId = "50D07946"
                     });


            return element;
        }

        private static void AddImageToBody(MainDocumentPart mainPart, string chave, string imageFileName)
        {

            foreach (Paragraph para in mainPart.Document.Body.Elements<Paragraph>())
            {
                if (!(para.InnerText.Contains(chave) || para.OuterXml.Contains(chave) || para.InnerXml.Contains(chave)))
                    continue;

                foreach (Run run in para.Elements<Run>())
                {
                    int runTextLength = run.Elements<Text>().Sum(t => t.Text.Length);

                    foreach (Text txt in run.Elements<Text>())
                    {
                        string secondPart = txt.Text;

                        txt.Text = ""; // Changing the existing text to truncate it at the insertion point


                        ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Png);

                        using (FileStream stream = new FileStream(imageFileName, FileMode.Open))
                        {
                            imagePart.FeedData(stream);
                        }

                        var imagem = ImageToBody(mainPart.GetIdOfPart(imagePart), imageFileName, 14.5f);

                        Run newRun = new Run(
                           imagem
                        );

                        if (!string.IsNullOrEmpty(secondPart))
                        {
                            run.InsertAfterSelf(new Run(new Text(secondPart))); // Insert the remaining text after the image
                        }

                        run.InsertAfterSelf(newRun); // Insert the new run with the image

                        return;
                    }
                }
            }
        }

        private static void AddImageToTable(MainDocumentPart mainPart, string chave, string imageFileName)
        {
            foreach (Table table in mainPart.Document.Body.Descendants<Table>())
            {
                foreach (TableRow row in table.Elements<TableRow>())
                {
                    foreach (TableCell cell in row.Elements<TableCell>())
                    {
                        foreach (Paragraph para in cell.Elements<Paragraph>())
                        {
                            if (para.InnerText.Contains(chave) || para.OuterXml.Contains(chave) || para.InnerXml.Contains(chave))
                            {
                                bool achou = false;

                                TableCellProperties cellProperties = cell.Elements<TableCellProperties>().FirstOrDefault();
                                int widthInTwips = 0;
                                float widthInCentimeters = 0;

                                // Verifica se existem propriedades de célula
                                if (cellProperties != null)
                                {
                                    // Obtém a largura da célula
                                    TableCellWidth cellWidth = cellProperties.Elements<TableCellWidth>().FirstOrDefault();

                                    if (cellWidth != null)
                                    {
                                        widthInTwips = int.Parse(cellWidth.Width);
                                        widthInCentimeters = (widthInTwips * 0.001764f) - 0.5f; // 1 twip = 0,0001764 centímetros
                                    }
                                }


                                foreach (Run run in para.Elements<Run>())
                                {
                                    foreach (Text txt in run.Elements<Text>())
                                    {
                                        if (achou)
                                        {
                                            txt.Text = "";
                                            continue;
                                        }

                                        string secondPart = txt.Text;

                                        txt.Text = ""; // Changing the existing text to truncate it at the insertion point

                                        ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Png);

                                        using (FileStream stream = new FileStream(imageFileName, FileMode.Open))
                                        {
                                            imagePart.FeedData(stream);
                                        }

                                        var imagem = ImageToBody(mainPart.GetIdOfPart(imagePart), imageFileName, widthInCentimeters);

                                        Run newRun = new Run(imagem);

                                        //if (!string.IsNullOrEmpty(secondPart))
                                        //{
                                        //    run.InsertAfterSelf(new Run(new Text(secondPart))); // Insert the remaining text after the image
                                        //}

                                        run.InsertAfterSelf(newRun); // Insert the new run with the image

                                        achou = true;
                                    }
                                }

                                if (achou)
                                    return;
                            }
                        }
                    }
                }
            }
        }

        private static void AddImageToTableHeader(MainDocumentPart mainPart, string chave, string imageFileName)
        {
            foreach (SectionProperties sectionProperties in mainPart.Document.Body.Descendants<SectionProperties>())
            {
                // Percorre os elementos de cabeçalho na seção
                foreach (HeaderReference headerReference in sectionProperties.Elements<HeaderReference>())
                {
                    // Obtém o tipo de cabeçalho (First, Default, Even, etc.)
                    string headerType = headerReference.Type;

                    // Obtém o relacionamento do cabeçalho
                    string headerRelationshipId = headerReference.Id;

                    // Obtém o cabeçalho pelo relacionamento
                    HeaderPart headerPart = (HeaderPart)mainPart.GetPartById(headerRelationshipId);

                    // Agora você pode percorrer as tabelas, linhas, células e parágrafos dentro do cabeçalho
                    foreach (Table table in headerPart.RootElement.Descendants<Table>())
                    {
                        foreach (TableRow row in table.Elements<TableRow>())
                        {
                            foreach (TableCell cell in row.Elements<TableCell>())
                            {
                                foreach (Paragraph para in cell.Elements<Paragraph>())
                                {
                                    if (para.InnerText.Contains(chave) || para.OuterXml.Contains(chave) || para.InnerXml.Contains(chave))
                                    {
                                        bool achou = false;

                                        TableCellProperties cellProperties = cell.Elements<TableCellProperties>().FirstOrDefault();
                                        int widthInTwips = 0;
                                        float widthInCentimeters = 0;

                                        // Verifica se existem propriedades de célula
                                        if (cellProperties != null)
                                        {
                                            // Obtém a largura da célula
                                            TableCellWidth cellWidth = cellProperties.Elements<TableCellWidth>().FirstOrDefault();

                                            if (cellWidth != null)
                                            {
                                                widthInTwips = int.Parse(cellWidth.Width);
                                                widthInCentimeters = (widthInTwips * 0.001764f) - 0.5f; // 1 twip = 0,0001764 centímetros
                                            }
                                        }


                                        foreach (Run run in para.Elements<Run>())
                                        {
                                            foreach (Text txt in run.Elements<Text>())
                                            {
                                                if (achou)
                                                {
                                                    txt.Text = "";
                                                    continue;
                                                }

                                                string secondPart = txt.Text;

                                                txt.Text = ""; // Changing the existing text to truncate it at the insertion point


                                                // Adiciona a imagem ao cabeçalho
                                                ImagePart headerImagePart = headerPart.AddImagePart(ImagePartType.Png);

                                                using (FileStream stream = new FileStream(imageFileName, FileMode.Open))
                                                {
                                                    headerImagePart.FeedData(stream);
                                                }

                                                var imagem = ImageToBody(headerPart.GetIdOfPart(headerImagePart), imageFileName, widthInCentimeters);

                                                Run newRun = new Run(imagem);

                                                //if (!string.IsNullOrEmpty(secondPart))
                                                //{
                                                //    run.InsertAfterSelf(new Run(new Text(secondPart))); // Insert the remaining text after the image
                                                //}

                                                run.InsertAfterSelf(newRun); // Insert the new run with the image

                                                achou = true;
                                            }
                                        }

                                        if (achou)
                                            return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void AddImageToHeader(MainDocumentPart mainPart, string chave, string imageFileName)
        {

            foreach (SectionProperties sectionProperties in mainPart.Document.Body.Descendants<SectionProperties>())
            {
                // Percorre os elementos de cabeçalho na seção
                foreach (HeaderReference headerReference in sectionProperties.Elements<HeaderReference>())
                {
                    // Obtém o tipo de cabeçalho (First, Default, Even, etc.)
                    string headerType = headerReference.Type;

                    // Obtém o relacionamento do cabeçalho
                    string headerRelationshipId = headerReference.Id;

                    // Obtém o cabeçalho pelo relacionamento
                    HeaderPart headerPart = (HeaderPart)mainPart.GetPartById(headerRelationshipId);
                    foreach (Paragraph para in headerPart.RootElement.Descendants<Paragraph>())
                    {
                        if (!(para.OuterXml.Contains(chave) || para.InnerXml.Contains(chave) || para.InnerText.Contains(chave)))
                            continue;

                        // Percorre as execuções dentro do parágrafo do cabeçalho
                        foreach (Run run in para.Elements<Run>())
                        {
                            int runTextLength = run.Elements<Text>().Sum(t => t.Text.Length);

                            foreach (Text txt in run.Elements<Text>())
                            {

                                var remainingPosition = txt.Text.IndexOf(chave);
                                if (remainingPosition < 0)
                                    continue;

                                string firstPart = txt.Text.Substring(0, remainingPosition);
                                string secondPart = txt.Text.Substring(remainingPosition);

                                txt.Text = firstPart; // Changing the existing text to truncate it at the insertion point


                                ImagePart headerImagePart = headerPart.AddImagePart(ImagePartType.Png);

                                using (FileStream stream = new FileStream(imageFileName, FileMode.Open))
                                {
                                    headerImagePart.FeedData(stream);
                                }

                                var imagem = ImageToBody(headerPart.GetIdOfPart(headerImagePart), imageFileName, 14.5f);

                                Run newRun = new Run(
                                   imagem
                                );

                                if (!string.IsNullOrEmpty(secondPart))
                                {
                                    run.InsertAfterSelf(new Run(new Text(secondPart))); // Insert the remaining text after the image
                                }

                                run.InsertAfterSelf(newRun); // Insert the new run with the image

                                return;
                            }
                        }
                    }
                }
            }
        }

        private static void AddImageToTableFooter(MainDocumentPart mainPart, string chave, string imageFileName)
        {
            foreach (SectionProperties sectionProperties in mainPart.Document.Body.Descendants<SectionProperties>())
            {
                // Percorre os elementos de cabeçalho na seção
                foreach (FooterReference footerReference in sectionProperties.Elements<FooterReference>())
                {
                    // Obtém o tipo de cabeçalho (First, Default, Even, etc.)
                    string footerType = footerReference.Type;

                    // Obtém o relacionamento do cabeçalho
                    string footerRelationshipId = footerReference.Id;

                    // Obtém o cabeçalho pelo relacionamento
                    FooterPart footerPart = (FooterPart)mainPart.GetPartById(footerRelationshipId);

                    // Agora você pode percorrer as tabelas, linhas, células e parágrafos dentro do cabeçalho
                    foreach (Table table in footerPart.RootElement.Descendants<Table>())
                    {
                        foreach (TableRow row in table.Elements<TableRow>())
                        {
                            foreach (TableCell cell in row.Elements<TableCell>())
                            {
                                foreach (Paragraph para in cell.Elements<Paragraph>())
                                {
                                    if (para.InnerText.Contains(chave) || para.OuterXml.Contains(chave) || para.InnerXml.Contains(chave))
                                    {
                                        bool achou = false;

                                        TableCellProperties cellProperties = cell.Elements<TableCellProperties>().FirstOrDefault();
                                        int widthInTwips = 0;
                                        float widthInCentimeters = 0;

                                        // Verifica se existem propriedades de célula
                                        if (cellProperties != null)
                                        {
                                            // Obtém a largura da célula
                                            TableCellWidth cellWidth = cellProperties.Elements<TableCellWidth>().FirstOrDefault();

                                            if (cellWidth != null)
                                            {
                                                widthInTwips = int.Parse(cellWidth.Width);
                                                widthInCentimeters = (widthInTwips * 0.001764f) - 0.5f; // 1 twip = 0,0001764 centímetros
                                            }
                                        }


                                        foreach (Run run in para.Elements<Run>())
                                        {
                                            foreach (Text txt in run.Elements<Text>())
                                            {
                                                if (achou)
                                                {
                                                    txt.Text = "";
                                                    continue;
                                                }

                                                string secondPart = txt.Text;

                                                txt.Text = ""; // Changing the existing text to truncate it at the insertion point


                                                // Adiciona a imagem ao cabeçalho
                                                ImagePart footerImagePart = footerPart.AddImagePart(ImagePartType.Png);

                                                using (FileStream stream = new FileStream(imageFileName, FileMode.Open))
                                                {
                                                    footerImagePart.FeedData(stream);
                                                }

                                                var imagem = ImageToBody(footerPart.GetIdOfPart(footerImagePart), imageFileName, widthInCentimeters);

                                                Run newRun = new Run(imagem);

                                                //if (!string.IsNullOrEmpty(secondPart))
                                                //{
                                                //    run.InsertAfterSelf(new Run(new Text(secondPart))); // Insert the remaining text after the image
                                                //}

                                                run.InsertAfterSelf(newRun); // Insert the new run with the image

                                                achou = true;
                                            }
                                        }

                                        if (achou)
                                            return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void AddImageToFooter(MainDocumentPart mainPart, string chave, string imageFileName)
        {

            foreach (SectionProperties sectionProperties in mainPart.Document.Body.Descendants<SectionProperties>())
            {
                // Percorre os elementos de cabeçalho na seção
                foreach (FooterReference footerReference in sectionProperties.Elements<FooterReference>())
                {
                    // Obtém o tipo de cabeçalho (First, Default, Even, etc.)
                    string footerType = footerReference.Type;

                    // Obtém o relacionamento do cabeçalho
                    string footerRelationshipId = footerReference.Id;

                    // Obtém o cabeçalho pelo relacionamento
                    FooterPart footerPart = (FooterPart)mainPart.GetPartById(footerRelationshipId);

                    foreach (Paragraph para in footerPart.RootElement.Descendants<Paragraph>())
                    {
                        if (!(para.OuterXml.Contains(chave) || para.InnerXml.Contains(chave) || para.InnerText.Contains(chave)))
                            continue;

                        // Percorre as execuções dentro do parágrafo do cabeçalho
                        foreach (Run run in para.Elements<Run>())
                        {
                            int runTextLength = run.Elements<Text>().Sum(t => t.Text.Length);

                            foreach (Text txt in run.Elements<Text>())
                            {

                                txt.Text = "";

                                ImagePart footerImagePart = footerPart.AddImagePart(ImagePartType.Png);

                                using (FileStream stream = new FileStream(imageFileName, FileMode.Open))
                                {
                                    footerImagePart.FeedData(stream);
                                }

                                var imagem = ImageToBody(footerPart.GetIdOfPart(footerImagePart), imageFileName, 14.5f);

                                Run newRun = new Run(
                                   imagem
                                );

                                run.InsertAfterSelf(newRun); // Insert the new run with the image

                                return;
                            }
                        }
                    }
                }
            }
        }

        private static void AddImageToTextBox(MainDocumentPart mainPart, string chave, string imageFileName)
        {

            foreach (Drawing drawing in mainPart.Document.Body.Descendants<Drawing>())
            {
                if (!(drawing.OuterXml.Contains(chave) || drawing.InnerXml.Contains(chave) || drawing.InnerText.Contains(chave)))
                    continue;

                var extentElement = drawing.Descendants<DW.Extent>().FirstOrDefault();

                if (extentElement != null)
                {
                    long cx = extentElement.Cx ?? 0; // Largura em unidades EMU (English Metric Units)
                    long cy = extentElement.Cy ?? 0; // Altura em unidades EMU

                    // Converter unidades EMU para centímetros, se necessário
                    float widthInCentimeters = cx / 360000.0f; // 1 cm = 360000 EMU
                    float heightInCentimeters = cy / 360000.0f;


                    foreach (Run run in drawing.Descendants<Run>())
                    {
                        int runTextLength = run.Elements<Text>().Sum(t => t.Text.Length);

                        foreach (Text txt in run.Elements<Text>())
                        {
                            string secondPart = txt.Text;

                            txt.Text = ""; // Changing the existing text to truncate it at the insertion point

                            ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Png);

                            using (FileStream stream = new FileStream(imageFileName, FileMode.Open))
                            {
                                imagePart.FeedData(stream);
                            }

                            var imagem = ImageToBody(mainPart.GetIdOfPart(imagePart), imageFileName, widthInCentimeters - 0.5f);

                            Run newRun = new Run(
                               imagem
                            );

                            if (!string.IsNullOrEmpty(secondPart))
                            {
                                run.InsertAfterSelf(new Run(new Text(secondPart))); // Insert the remaining text after the image
                            }

                            run.InsertAfterSelf(newRun); // Insert the new run with the image

                            return;
                        }
                    }
                }
            }
        }
    }
}
