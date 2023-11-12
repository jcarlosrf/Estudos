using System.IO;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;


namespace CriarGraficoSliderCore
{
    public class WordOpenXML
    {
        public static void ReplaceTextWithText(string filePath, string searchText, string imagePath, string outputFilePath)
        {

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                Regex regexText = new Regex(searchText);
                docText = regexText.Replace(docText, "Hi Everyone!");

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
            }
        }

        public static void ReplaceTextWithImage(string filePath, string searchText, string imagePath, string outputFilePath)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, true))
            {
                WordInsertImage.InsertAPicture(wordDoc, imagePath, searchText);
            }

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }                

                Regex regexText = new Regex(searchText);
                docText = regexText.Replace(docText, "");

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
            }
        }
    }
}
