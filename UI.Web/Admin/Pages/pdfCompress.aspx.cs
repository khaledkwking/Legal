using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using System.Data;
using System.Collections;
using UI.Web.Admin.Controller;
using Permission.DAL.Entities;
using Infrastructure.DAL;
using Microsoft.VisualBasic;
using Permission.DAL.Repository;
using System.IO;
using iTextSharp.text.pdf;
using Infrastructure.DAL.Model;
using Infrastructure.DAL.Enum;
using iTextSharp.text.pdf.parser;
using System.Drawing;
using System.Drawing.Imaging;

using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing.Drawing2D;

namespace UI.Web.Admin.Pages
{
    public partial class pdfCompress : BaseFormAdmin
    {

        public string ScannerRepository = System.Configuration.ConfigurationManager.AppSettings["ScannerRepository"].ToString();
        List<string> errorList = new List<string>();

        protected void Page_PreInit(object sender, System.EventArgs e)
        {
            PageUrl = "AdminManager.aspx";
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {



        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblSucess.Text = "";
            lblerror.Text = "";
            // Get Files From Souce

            //Compress File
            //Save Files To taget
            try
            {
                string soucepath = txtSouceLocation.Text;
                string targetpath = txtTargetLocation.Text;

                string[] filePaths = Directory.GetFiles(soucepath, "*.pdf", SearchOption.AllDirectories);
                if (filePaths.Length > 0)
                {
                    lblSucess.Text ="</br>(" + filePaths.Length + ") files found in taget location";

                    for (int i = 0; i < filePaths.Length; i++)
                    {
                        //string fileFullpath = System.IO.Path.GetDirectoryName(filePaths[i]);
                        //Replace Soucepath by Traget path
                        string fileFullpath= filePaths[i].Replace(soucepath, targetpath);

                        string fileExtention = filePaths[i].ToString().Substring(filePaths[i].LastIndexOf(".")+1);
                        if (fileExtention.ToLower()=="pdf")
                        {
                            //if (!Directory.Exists(fileFullpath))
                            //    Directory.CreateDirectory(fileFullpath);


                            //PdfReader reader = new PdfReader(filePaths[i], new System.Text.ASCIIEncoding().GetBytes(""));
                            try
                            {
                                ExtractImagesFromPDF(filePaths[i], fileFullpath);
                                lblSucess.Text += "</br>File :" + System.IO.Path.GetDirectoryName(filePaths[i]) + " Compressed";
                            }
                            catch (Exception ex)
                            {
                                lblerror.Text += "</br>Exception :" + filePaths[i].ToString() + ex.Message.ToLower();

                            }
                        }

                    }

                }
                else { lblerror.Text = "No files found in taget location"; }


            }
            catch (Exception ex)
            {

                lblerror.Text += ex.Message;
            }


        }


        #region "Compress PDF"


        #region ExtractImagesFromPDF
        //public static void ExtractImagesFromPDF(string sourcePdf, string outputPath)
        //{
        //    // NOTE:  This will only get the first image it finds per page.
        //    PdfReader pdf = new PdfReader(sourcePdf);
        //    RandomAccessFileOrArray raf = new iTextSharp.text.pdf.RandomAccessFileOrArray(sourcePdf);

        //    try
        //    {
        //        for (int pageNumber = 1; pageNumber <= pdf.NumberOfPages; pageNumber++)
        //        {
        //            PdfDictionary pg = pdf.GetPageN(pageNumber);
        //            PdfDictionary res = (PdfDictionary)PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));
        //            PdfDictionary xobj = (PdfDictionary)PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));
        //            if (xobj != null)
        //            {
        //                foreach (PdfName name in xobj.Keys)
        //                {
        //                    PdfObject obj = xobj.Get(name);
        //                    if (obj.IsIndirect())
        //                    {
        //                        PdfDictionary tg = (PdfDictionary)PdfReader.GetPdfObject(obj);
        //                        PdfName type =  (PdfName)PdfReader.GetPdfObject(tg.Get(PdfName.SUBTYPE));
        //                        if (PdfName.IMAGE.Equals(type))
        //                        {

        //                            int XrefIndex = Convert.ToInt32(((PRIndirectReference)obj).Number.ToString(System.Globalization.CultureInfo.InvariantCulture));
        //                            PdfObject pdfObj = pdf.GetPdfObject(XrefIndex);
        //                            PdfStream pdfStrem = (PdfStream)pdfObj;
        //                            byte[] bytes = PdfReader.GetStreamBytesRaw((PRStream)pdfStrem);
        //                            if ((bytes != null))
        //                            {
        //                                using (System.IO.MemoryStream memStream = new System.IO.MemoryStream(bytes))
        //                                {
        //                                    memStream.Position = 0;
        //                                    System.Drawing.Image img = System.Drawing.Image.FromStream(memStream);
        //                                    // must save the file while stream is open.
        //                                    if (!Directory.Exists(outputPath))
        //                                        Directory.CreateDirectory(outputPath);

        //                                    string path = System.IO.Path.Combine(outputPath, String.Format(@"{0}.jpg", pageNumber));
        //                                    System.Drawing.Imaging.EncoderParameters parms = new System.Drawing.Imaging.EncoderParameters(1);
        //                                    parms.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Compression, 0);
        //                                    // GetImageEncoder is found below this method
        //                                    System.Drawing.Imaging.ImageCodecInfo jpegEncoder = GetImageEncoder("JPEG");
        //                                    img.Save(path, jpegEncoder, parms);
        //                                    break;

        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        pdf.Close();
        //    }


        //}
        #endregion

        #region GetImageEncoder

        #endregion

        public void ExtractImagesFromPDF(string sourcePdf, string outputPath)
        {
            if (!Directory.Exists(System.IO.Path.GetDirectoryName(outputPath)))
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            Document document = new Document();

            // NOTE:  This will only get the first image it finds per page.
            PdfReader pdf = new PdfReader(sourcePdf);
            RandomAccessFileOrArray raf = new iTextSharp.text.pdf.RandomAccessFileOrArray(sourcePdf);
            PdfWriter.GetInstance(document, new FileStream(outputPath , FileMode.Create));
            document.Open();


            try
            {
                for (int pageNumber = 1; pageNumber <= pdf.NumberOfPages; pageNumber++)
                {
                    PdfDictionary pg = pdf.GetPageN(pageNumber);

                    // recursively search pages, forms and groups for images.
                    PdfObject obj = FindImageInPDFDictionary(pg);
                    if (obj != null)
                    {

                        int XrefIndex = Convert.ToInt32(((PRIndirectReference)obj).Number.ToString(System.Globalization.CultureInfo.InvariantCulture));
                        PdfObject pdfObj = pdf.GetPdfObject(XrefIndex);
                        PdfStream pdfStrem = (PdfStream)pdfObj;
                      //  byte[] bytes = PdfReader.GetStreamBytesRaw((PRStream)pdfStrem);

                        var image = new PdfImageObject((PRStream)pdfStrem);
                        System.Drawing.Image img = image.GetDrawingImage();
                        if (img == null) continue;
                        //Convert BW

                        using (var bw = ToBlackAndWhite(img))
                        {
                            //  Replace to Addd PDF Pages

                            //if (!Directory.Exists(outputPath))
                            //    Directory.CreateDirectory(outputPath);

                            string path = System.IO.Path.Combine(outputPath, String.Format(@"{0}.png", pageNumber));
                            System.Drawing.Imaging.EncoderParameters parms = new System.Drawing.Imaging.EncoderParameters(1);
                            parms.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Compression, 0);
                            System.Drawing.Imaging.ImageCodecInfo jpegEncoder = GetImageEncoder("png");
                            //  bw.Save(path, jpegEncoder, parms);

                            iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(bw, System.Drawing.Imaging.ImageFormat.Png);
                            pic.ScaleToFit(document.PageSize);
                            pic.SetAbsolutePosition(0, 0);
                            document.Add(pic);
                            document.NewPage();


                        }

                    }
                }
            }
            catch( Exception ex)
            {
                throw ex;
            }
            finally
            {
                pdf.Close();
                raf.Close();
                document.Close();

            }


        }

        private static PdfObject FindImageInPDFDictionary(PdfDictionary pg)
        {
            PdfDictionary res =
                (PdfDictionary)PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));


            PdfDictionary xobj =
              (PdfDictionary)PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));
            if (xobj != null)
            {
                foreach (PdfName name in xobj.Keys)
                {

                    PdfObject obj = xobj.Get(name);
                    if (obj.IsIndirect())
                    {
                        PdfDictionary tg = (PdfDictionary)PdfReader.GetPdfObject(obj);

                        PdfName type =
                          (PdfName)PdfReader.GetPdfObject(tg.Get(PdfName.SUBTYPE));

                        //image at the root of the pdf
                        if (PdfName.IMAGE.Equals(type))
                        {
                            return obj;
                        }// image inside a form
                        else if (PdfName.FORM.Equals(type))
                        {
                            return FindImageInPDFDictionary(tg);
                        } //image inside a group
                        else if (PdfName.GROUP.Equals(type))
                        {
                            return FindImageInPDFDictionary(tg);
                        }

                    }
                }
            }

            return null;

        }
        public static System.Drawing.Imaging.ImageCodecInfo GetImageEncoder(string imageType)
        {
            imageType = imageType.ToUpperInvariant();



            foreach (ImageCodecInfo info in ImageCodecInfo.GetImageEncoders())
            {
                if (info.FormatDescription == imageType)
                {
                    return info;
                }
            }

            return null;
        }
        #endregion

        private bool TryCompressPdfImages(PdfReader reader)
        {
            try
            {
                int n = reader.XrefSize;
                for (int i = 0; i < n; i++)
                {
                    PdfObject obj = reader.GetPdfObject(i);
                    if (obj == null || !obj.IsStream())
                    {
                        continue;
                    }

                    var dict = (PdfDictionary)PdfReader.GetPdfObject(obj);
                    var subType = (PdfName)PdfReader.GetPdfObject(dict.Get(PdfName.SUBTYPE));
                    if (!PdfName.IMAGE.Equals(subType))
                    {
                        continue;
                    }

                    var stream = (PRStream)obj;
                    try
                    {
                        var image = new PdfImageObject(stream);

                       System.Drawing.Image img = image.GetDrawingImage();
                        if (img == null) continue;

                        using (img)
                        {
                            int width = img.Width;
                            int height = img.Height;

                            using (var msImg = new MemoryStream())
                            using (var bw = ToBlackAndWhite(img))
                            {
                                bw.Save(msImg, ImageFormat.Png);
                                msImg.Position = 0;
                                stream.SetData(msImg.ToArray(), false, PdfStream.NO_COMPRESSION);
                                stream.Put(PdfName.TYPE, PdfName.XOBJECT);
                                stream.Put(PdfName.SUBTYPE, PdfName.IMAGE);
                                stream.Put(PdfName.FILTER, PdfName.FLATEDECODE);
                                stream.Put(PdfName.WIDTH, new PdfNumber(width));
                                stream.Put(PdfName.HEIGHT, new PdfNumber(height));
                                stream.Put(PdfName.BITSPERCOMPONENT, new PdfNumber(8));
                                stream.Put(PdfName.COLORSPACE, PdfName.DEVICERGB);
                                stream.Put(PdfName.LENGTH, new PdfNumber(msImg.Length));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                       // Trace.TraceError(ex.ToString());
                    }
                    finally
                    {
                        // may or may not help
                        reader.RemoveUnusedObjects();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                //Trace.TraceError(ex.ToString());
                return false;
            }
        }

        public System.Drawing.Image ToBlackAndWhite( System.Drawing.Image image)
        {
           var  _image = new Bitmap(image);
            _image.SetResolution(72.0f, 72.0f); // Change to any dpi
            using (Graphics gr = Graphics.FromImage(_image))
            {

                var grayMatrix = new[]
                {
                new[] {0.299f, 0.299f, 0.299f, 0, 0},
                new[] {0.587f, 0.587f, 0.587f, 0, 0},
                new[] {0.114f, 0.114f, 0.114f, 0, 0},
                new [] {0f, 0, 0, 1, 0},
                new [] {0f, 0, 0, 0, 1}
            };

                gr.CompositingQuality = CompositingQuality.HighQuality;
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;


                var ia = new ImageAttributes();
                ia.SetColorMatrix(new ColorMatrix(grayMatrix));
                ia.SetThreshold((float)0.8); // Change this threshold as needed
                var rc = new System.Drawing.Rectangle(0, 0, image.Width, image.Height);
                gr.DrawImage(_image, rc, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, ia);
            }
            return _image;
        }



        private bool MoveFile(string filename, string soucepath, string destpath)
        {
            try
            {


                if (File.Exists(soucepath + filename))
                {
                    if (!Directory.Exists(destpath))
                        Directory.CreateDirectory(destpath);

                    File.Move(soucepath + filename, destpath + filename);
                }
                else
                {
                    errorList.Add("File Not Found  : " + filename);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                errorList.Add("Move Failed :" + ex.Message.ToString());
                return false;
            }

        }

        protected void btnAgreementMigation_Click(object sender, EventArgs e)
        {
            int successCount = 0;
            int FailCount = 0;
            string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "AgreementsAttachments/";
            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.Agreement_Attachments
                     orderby obj.Code

                     select obj);

                var AgreementList = result.ToList<Agreement_Attachments>();
                foreach (var item in AgreementList)
                {
                    //Loop and Move Files
                    if (item.Filepath!=null && item.Filepath!="")
                    {
                        if (MoveFile(item.Filepath, ScannerRepository + _TargetUploadPath , ScannerRepository + _TargetUploadPath + item.AgreementCode.ToString() + "/" + item.ProcedureID.ToString() + "/"  ))
                        {
                            successCount++;
                        }
                        else
                        { FailCount++; }

                    }

                }
                lblerror.Text = "["+successCount.ToString() + "] Moved  [" + FailCount.ToString() + "] Failed";
                if (errorList.Count>0)
                {
                    lblerror.Text += "<hr/>";

                    foreach (var item in errorList)
                    {
                        lblerror.Text += "<br/>" + item.ToString();
                    }

                }

            }
            errorList.Clear();

            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.AgreementMain_Attachments
                     orderby obj.Code

                     select obj);

                var AgreementList = result.ToList<AgreementMain_Attachments>();
                foreach (var item in AgreementList)
                {
                    //Loop and Move Files
                    if (item.Filepath != null && item.Filepath != "")
                    {
                        if (MoveFile(item.Filepath, ScannerRepository + _TargetUploadPath, ScannerRepository + _TargetUploadPath + item.AgreementCode.ToString() + "/"))
                        {
                            successCount++;
                        }
                        else
                        { FailCount++; }

                    }

                }
                lblerror.Text = "[" + successCount.ToString() + "] Moved  [" + FailCount.ToString() + "] Failed";
                if (errorList.Count > 0)
                {
                    lblerror.Text += "<hr/>";

                    foreach (var item in errorList)
                    {
                        lblerror.Text += "<br/>" + item.ToString();
                    }

                }

            }


            errorList.Clear();
            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.Agreement_procedureHistory
                     orderby obj.Code

                     select obj);

                var AgreementList = result.ToList<Agreement_procedureHistory>();
                foreach (var item in AgreementList)
                {
                    //Loop and Move Files
                    if (item.CMGSDessionFile != null && item.CMGSDessionFile != "")
                    {
                        if (MoveFile(item.CMGSDessionFile, ScannerRepository + _TargetUploadPath, ScannerRepository + _TargetUploadPath + item.AgreementCode.ToString() + "/" + item.Code.ToString() + "/"))
                        {
                            successCount++;
                        }
                        else
                        { FailCount++; }

                    }

                    if (item.publishDessionFile != null && item.publishDessionFile != "")
                    {
                        if (MoveFile(item.publishDessionFile, ScannerRepository + _TargetUploadPath, ScannerRepository + _TargetUploadPath + item.AgreementCode.ToString() + "/" + item.Code.ToString() + "/"))
                        {
                            successCount++;
                        }
                        else
                        { FailCount++; }

                    }

                }
                lblerror.Text = "[" + successCount.ToString() + "] Moved  [" + FailCount.ToString() + "] Failed";
                if (errorList.Count > 0)
                {
                    lblerror.Text += "<hr/>";

                    foreach (var item in errorList)
                    {
                        lblerror.Text += "<br/>" + item.ToString();
                    }

                }

            }
            errorList.Clear();

        }


        protected void btnMedal_Click(object sender, EventArgs e)
        {

            int successCount = 0;
            int FailCount = 0;
            string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "MedalAttachments/";

            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.Medal_Attachments
                     orderby obj.Code

                     select obj);

                var AgreementList = result.ToList<Medal_Attachments>();
                foreach (var item in AgreementList)
                {
                    //Loop and Move Files
                    if (item.Filepath != null && item.Filepath != "")
                    {
                        string folderpath = item.ProcedureID != null && item.ProcedureID != 0 ? item.MedalMasterCode.ToString() + "/" + item.ProcedureID.ToString() + "/" : item.MedalMasterCode.ToString() + "/";
                        if (MoveFile(item.Filepath, ScannerRepository + _TargetUploadPath, ScannerRepository + _TargetUploadPath + folderpath))
                        {
                            successCount++;
                        }
                        else
                        { FailCount++; }

                    }

                }
                lblerror.Text = "[" + successCount.ToString() + "] Moved  [" + FailCount.ToString() + "] Failed";
                if (errorList.Count > 0)
                {
                    lblerror.Text += "<hr/>";

                    foreach (var item in errorList)
                    {
                        lblerror.Text += "<br/>" + item.ToString();
                    }

                }

            }

            errorList.Clear();

            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.Medalmain_Attachments
                     orderby obj.Code

                     select obj);

                var objList = result.ToList<Medalmain_Attachments>();
                foreach (var item in objList)
                {
                    //Loop and Move Files
                    if (item.Filepath != null && item.Filepath != "")
                    {
                        string folderpath = item.MedalMasterCode.ToString() + "/";
                        if (MoveFile(item.Filepath, ScannerRepository + _TargetUploadPath, ScannerRepository + _TargetUploadPath + folderpath))
                        {
                            successCount++;
                        }
                        else
                        { FailCount++; }

                    }

                }
                lblerror.Text += "[" + successCount.ToString() + "] Moved  [" + FailCount.ToString() + "] Failed";
                if (errorList.Count > 0)
                {
                    lblerror.Text += "<hr/>";

                    foreach (var item in errorList)
                    {
                        lblerror.Text += "<br/>" + item.ToString();
                    }

                }





            }
            errorList.Clear();
        }

        protected void btnCases_Click(object sender, EventArgs e)
        {
            int successCount = 0;
            int FailCount = 0;
            string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "CasesAttachments/";


            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.arc_Attachments

                     orderby obj.Code

                     select obj);

                var objList = result.ToList<arc_Attachments>();
                foreach (var item in objList)
                {
                    //Loop and Move Files
                    if (item.Filepath != null && item.Filepath != "")
                    {
                        string folderpath = item.CaseID != null ? "Cases/" +item.CaseID.ToString() + "/" : "Cases/";
                        if (item.DocCode != null && item.DocCode!=0)
                        {
                            folderpath += item.DocCode.ToString() +"/";
                        }


                        if (MoveFile(item.Filepath, ScannerRepository + _TargetUploadPath, ScannerRepository + _TargetUploadPath + folderpath))
                        {
                            successCount++;
                        }
                        else
                        { FailCount++; }

                    }

                }
                lblerror.Text = "[" + successCount.ToString() + "] Moved  [" + FailCount.ToString() + "] Failed";
                if (errorList.Count > 0)
                {
                    lblerror.Text += "<hr/>";

                    foreach (var item in errorList)
                    {
                        lblerror.Text += "<br/>" + item.ToString();
                    }

                }

            }

            errorList.Clear();



            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.arc_Data
                     where obj.TargetModule == (int)ArcTargetModules.CasesModules
                     orderby obj.Code

                     select obj);

                var objList = result.ToList<arc_Data>();
                foreach (var item in objList)
                {
                    //Loop and Move Files
                    if (item.Filepath != null && item.Filepath != "")
                    {
                        string folderpath = item.RefDocID.ToString() + "/" + (item.Doc_Type == 1 ? "incoming/" : "outgoing/") + item.Code + "/";


                        if (MoveFile(item.Filepath, ScannerRepository + _TargetUploadPath, ScannerRepository + _TargetUploadPath + folderpath))
                        {
                            successCount++;
                        }
                        else
                        { FailCount++; }

                    }

                }
                lblerror.Text = "[" + successCount.ToString() + "] Moved  [" + FailCount.ToString() + "] Failed";
                if (errorList.Count > 0)
                {
                    lblerror.Text += "<hr/>";

                    foreach (var item in errorList)
                    {
                        lblerror.Text += "<br/>" + item.ToString();
                    }

                }

            }
            errorList.Clear();


  using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.Cases_H_Hearing
                     orderby obj.Code

                     select obj);

                var objList = result.ToList<Cases_H_Hearing>();
                foreach (var item in objList)
                {
                    //Loop and Move Files
                    if (item.HearingAttachment != null && item.HearingAttachment != "")
                    {
                        string folderpath = item.CaseID.ToString() + "/hearing/" + item.Code + "/";


                        if (MoveFile(item.HearingAttachment, ScannerRepository + _TargetUploadPath, ScannerRepository + _TargetUploadPath + folderpath))
                        {
                            successCount++;
                        }
                        else
                        { FailCount++; }

                    }

                }
                lblerror.Text = "[" + successCount.ToString() + "] Moved  [" + FailCount.ToString() + "] Failed";
                if (errorList.Count > 0)
                {
                    lblerror.Text += "<hr/>";

                    foreach (var item in errorList)
                    {
                        lblerror.Text += "<br/>" + item.ToString();
                    }

                }

            }
            errorList.Clear();
        }

        protected void btnQuestions_Click(object sender, EventArgs e)
        {

            int successCount = 0;
            int FailCount = 0;
            string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "QuestionsAttachments/";

            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.Parliament_Questions

                     orderby obj.code

                     select obj);

                var objList = result.ToList<Parliament_Questions>();
                foreach (var item in objList)
                {
                    string folderpath = "";
                    //Loop and Move Files
                    if (item.Q_Attachment != null && item.Q_Attachment != "")
                    {

                        if (item.QType == 1)
                        {
                              folderpath=_TargetUploadPath + item.code.ToString() + "/";
                        }
                        else {
                            folderpath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "Interrogation/" + item.code.ToString() + "/";
                        }

                        if (MoveFile(item.Q_Attachment, ScannerRepository + _TargetUploadPath, ScannerRepository + folderpath))
                        {
                            successCount++;
                        }
                        else
                        { FailCount++; }

                    }

                }
                lblerror.Text = "[" + successCount.ToString() + "] Moved  [" + FailCount.ToString() + "] Failed";
                if (errorList.Count > 0)
                {
                    lblerror.Text += "<hr/>";

                    foreach (var item in errorList)
                    {
                        lblerror.Text += "<br/>" + item.ToString();
                    }

                }

            }

            errorList.Clear();


            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.arc_Data
                     where obj.TargetModule == (int)ArcTargetModules.QuestionsModules
                      orderby obj.Code

                     select obj);

                var objList = result.ToList<arc_Data>();
                foreach (var item in objList)
                {
                    //Loop and Move Files
                    if (item.Filepath != null && item.Filepath != "")
                    {
                        string folderpath = item.RefDocID.ToString() + "/" + (item.Doc_Type==1?"incoming/":"outgoing/")+ item.Code +"/"  ;


                        if (MoveFile(item.Filepath, ScannerRepository + _TargetUploadPath, ScannerRepository + _TargetUploadPath + folderpath))
                        {
                            successCount++;
                        }
                        else
                        { FailCount++; }

                    }

                }
                lblerror.Text = "[" + successCount.ToString() + "] Moved  [" + FailCount.ToString() + "] Failed";
                if (errorList.Count > 0)
                {
                    lblerror.Text += "<hr/>";

                    foreach (var item in errorList)
                    {
                        lblerror.Text += "<br/>" + item.ToString();
                    }

                }

            }
            errorList.Clear();


             using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.Parliament_QuestionsAnswers
                      orderby obj.Code
                     select obj);

                var objList = result.ToList<Parliament_QuestionsAnswers>();
                foreach (var item in objList)
                {
                    //Loop and Move Files
                    if (item.Answerattachments != null && item.Answerattachments != "")
                    {
                        string folderpath = item.QuestionID.ToString() + "/answers/" + item.Code +"/"  ;

                        if (MoveFile(item.Answerattachments, ScannerRepository + _TargetUploadPath, ScannerRepository + _TargetUploadPath + folderpath))
                        {
                            successCount++;
                        }
                        else
                        { FailCount++; }

                    }

                }
                lblerror.Text = "[" + successCount.ToString() + "] Moved  [" + FailCount.ToString() + "] Failed";
                if (errorList.Count > 0)
                {
                    lblerror.Text += "<hr/>";

                    foreach (var item in errorList)
                    {
                        lblerror.Text += "<br/>" + item.ToString();
                    }

                }

            }
            errorList.Clear();
        }

        protected void btnMadbata_Click(object sender, EventArgs e)
        {
            int successCount = 0;
            int FailCount = 0;
            string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "Parliament_madbata/";
            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.Parliament_madbata
                     orderby obj.code
                     select obj);

                var objList = result.ToList<Parliament_madbata>();
                foreach (var item in objList)
                {
                    //Loop and Move Files
                    if (item.M_Attachment != null && item.M_Attachment != "")
                    {

                        if (MoveFile(item.M_Attachment, ScannerRepository + _TargetUploadPath, ScannerRepository + _TargetUploadPath + gets(item.code)+"/"))
                        {
                            successCount++;
                        }
                        else
                        { FailCount++; }

                    }

                }
                lblerror.Text = "[" + successCount.ToString() + "] Moved  [" + FailCount.ToString() + "] Failed";
                if (errorList.Count > 0)
                {
                    lblerror.Text += "<hr/>";

                    foreach (var item in errorList)
                    {
                        lblerror.Text += "<br/>" + item.ToString();
                    }

                }

            }
            errorList.Clear();

        }

        protected void btnSuggestion_Click(object sender, EventArgs e)
        {
            int successCount = 0;
            int FailCount = 0;
            string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "suggestions/";
            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.Complaints_Data
                     orderby obj.Code
                     select obj);

                var objList = result.ToList<Complaints_Data>();
                foreach (var item in objList)
                {
                    //Loop and Move Files
                    if (item.Complaints_File != null && item.Complaints_File != "")
                    {

                        if (MoveFile(item.Complaints_File, ScannerRepository + _TargetUploadPath, ScannerRepository + _TargetUploadPath + gets(item.Code) + "/"))
                        {
                            successCount++;
                        }
                        else
                        { FailCount++; }

                    }

                }
                lblerror.Text = "[" + successCount.ToString() + "] Moved  [" + FailCount.ToString() + "] Failed";
                if (errorList.Count > 0)
                {
                    lblerror.Text += "<hr/>";

                    foreach (var item in errorList)
                    {
                        lblerror.Text += "<br/>" + item.ToString();
                    }

                }

            }
            errorList.Clear();




            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.arc_Data
                     where obj.TargetModule == (int)ArcTargetModules.ComplaintsModules
                     orderby obj.Code

                     select obj);

                var objList = result.ToList<arc_Data>();
                foreach (var item in objList)
                {
                    //Loop and Move Files
                    if (item.Filepath != null && item.Filepath != "")
                    {
                        string folderpath = item.RefDocID.ToString() + "/" + (item.Doc_Type == 1 ? "incoming/" : "outgoing/") + item.Code + "/";


                        if (MoveFile(item.Filepath, ScannerRepository + _TargetUploadPath, ScannerRepository + _TargetUploadPath + folderpath))
                        {
                            successCount++;
                        }
                        else
                        { FailCount++; }

                    }

                }
                lblerror.Text = "[" + successCount.ToString() + "] Moved  [" + FailCount.ToString() + "] Failed";
                if (errorList.Count > 0)
                {
                    lblerror.Text += "<hr/>";

                    foreach (var item in errorList)
                    {
                        lblerror.Text += "<br/>" + item.ToString();
                    }

                }

            }
            errorList.Clear();




        }

        protected void btnLib_Click(object sender, EventArgs e)
        {

            int successCount = 0;
            int FailCount = 0;
            string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "LibraryDocs/";
            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.Library_Documents
                     orderby obj.Code
                     select obj);

                var objList = result.ToList<Library_Documents>();
                foreach (var item in objList)
                {
                    //Loop and Move Files
                    if (item.Filepath != null && item.Filepath != "")
                    {

                        if (MoveFile(item.Filepath, ScannerRepository + _TargetUploadPath, ScannerRepository + _TargetUploadPath + gets(item.Code) + "/"))
                        {
                            successCount++;
                        }
                        else
                        { FailCount++; }

                    }

                }
                lblerror.Text = "[" + successCount.ToString() + "] Moved  [" + FailCount.ToString() + "] Failed";
                if (errorList.Count > 0)
                {
                    lblerror.Text += "<hr/>";

                    foreach (var item in errorList)
                    {
                        lblerror.Text += "<br/>" + item.ToString();
                    }

                }

            }
            errorList.Clear();
        }

        protected void btnPm_Click(object sender, EventArgs e)
        {
            int successCount = 0;
            int FailCount = 0;
            string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "Pm_Letters/";
            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.Pm_Letters
                     orderby obj.code
                     select obj);

                var objList = result.ToList<Pm_Letters>();
                foreach (var item in objList)
                {
                    //Loop and Move Files
                    if (item.Arc_Attachment != null && item.Arc_Attachment != "")
                    {

                        if (MoveFile(item.Arc_Attachment, ScannerRepository + _TargetUploadPath, ScannerRepository + _TargetUploadPath + gets(item.code) + "/"))
                        {
                            successCount++;
                        }
                        else
                        { FailCount++; }

                    }

                }
                lblerror.Text = "[" + successCount.ToString() + "] Moved  [" + FailCount.ToString() + "] Failed";
                if (errorList.Count > 0)
                {
                    lblerror.Text += "<hr/>";

                    foreach (var item in errorList)
                    {
                        lblerror.Text += "<br/>" + item.ToString();
                    }

                }

            }
            errorList.Clear();
        }

        protected void btbLawsDocs_Click(object sender, EventArgs e)
        {
            int successCount = 0;
            int FailCount = 0;
            string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "LawsAttachments/";
            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.Law_DocData
                     orderby obj.Code
                     select obj);

                var objList = result.ToList<Law_DocData>();
                foreach (var item in objList)
                {
                    //Loop and Move Files
                    if (item.DocFilepath != null && item.DocFilepath != "")
                    {

                        if (MoveFile(item.DocFilepath, ScannerRepository + _TargetUploadPath, ScannerRepository + _TargetUploadPath + gets(item.Code) + "/"))
                        {
                            successCount++;
                        }
                        else
                        { FailCount++; }



                    }

                    if (item.DocFilepath_published != null && item.DocFilepath_published != "")
                    {

                        if (MoveFile(item.DocFilepath_published, ScannerRepository + _TargetUploadPath, ScannerRepository + _TargetUploadPath + gets(item.Code) + "/"))
                        {
                            successCount++;
                        }
                        else
                        { FailCount++; }



                    }


                }
                lblerror.Text = "[" + successCount.ToString() + "] Moved  [" + FailCount.ToString() + "] Failed";
                if (errorList.Count > 0)
                {
                    lblerror.Text += "<hr/>";

                    foreach (var item in errorList)
                    {
                        lblerror.Text += "<br/>" + item.ToString();
                    }

                }

            }
            errorList.Clear();

            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.Law_DocProcedures
                     orderby obj.Code
                     select obj);

                var objList = result.ToList<Law_DocProcedures>();
                foreach (var item in objList)
                {
                    //Loop and Move Files
                    if (item.Procedureattachments != null && item.Procedureattachments != "")
                    {

                        if (MoveFile(item.Procedureattachments, ScannerRepository + _TargetUploadPath, ScannerRepository + _TargetUploadPath +gets(item.DocRefID)+ "/procedure/" + gets(item.Code) + "/"))
                        {
                            successCount++;
                        }
                        else
                        { FailCount++; }



                    }

                }
                lblerror.Text = "[" + successCount.ToString() + "] Moved  [" + FailCount.ToString() + "] Failed";
                if (errorList.Count > 0)
                {
                    lblerror.Text += "<hr/>";

                    foreach (var item in errorList)
                    {
                        lblerror.Text += "<br/>" + item.ToString();
                    }

                }

            }
            errorList.Clear();


            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.arc_Data
                     where obj.TargetModule == (int)ArcTargetModules.DecisionModules
                     orderby obj.Code

                     select obj);

                var objList = result.ToList<arc_Data>();
                foreach (var item in objList)
                {
                    //Loop and Move Files
                    if (item.Filepath != null && item.Filepath != "")
                    {
                        string folderpath = item.RefDocID.ToString() + "/" + (item.Doc_Type == 1 ? "incoming/" : "outgoing/") + item.Code + "/";


                        if (MoveFile(item.Filepath, ScannerRepository + _TargetUploadPath, ScannerRepository + _TargetUploadPath + folderpath))
                        {
                            successCount++;
                        }
                        else
                        { FailCount++; }

                    }

                }
                lblerror.Text = "[" + successCount.ToString() + "] Moved  [" + FailCount.ToString() + "] Failed";
                if (errorList.Count > 0)
                {
                    lblerror.Text += "<hr/>";

                    foreach (var item in errorList)
                    {
                        lblerror.Text += "<br/>" + item.ToString();
                    }

                }

            }
            errorList.Clear();



        }

        protected void btnAgreementMigation2_Click(object sender, EventArgs e)
        {
            int successCount = 0;
            int FailCount = 0;
            string _sourcePath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "AgreementsAttachments/0/";
            string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "AgreementsAttachments/";
            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.Agreement_Attachments
                     orderby obj.Code

                     select obj);

                var AgreementList = result.ToList<Agreement_Attachments>();
                foreach (var item in AgreementList)
                {
                    //Loop and Move Files
                    if (item.Filepath != null && item.Filepath != "")
                    {
                        if (File.Exists(ScannerRepository + _sourcePath+item.Filepath))
                        {
                            if (MoveFile(item.Filepath, ScannerRepository + _sourcePath, ScannerRepository + _TargetUploadPath + item.AgreementCode.ToString() + "/" + item.ProcedureID.ToString() + "/"))
                            {
                                successCount++;
                                lblerror.Text += item.Filepath;
                                lblerror.Text += "<br/>";
                            }
                            else
                            { FailCount++; }
                        }
                       

                    }

                }
                lblerror.Text += "[" + successCount.ToString() + "] Moved  [" + FailCount.ToString() + "] Failed";
                if (errorList.Count > 0)
                {
                    lblerror.Text += "<hr/>";

                    foreach (var item in errorList)
                    {
                        lblerror.Text += "<br/>" + item.ToString();
                    }

                }

            }
            errorList.Clear();

            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.AgreementMain_Attachments
                     orderby obj.Code

                     select obj);

                var AgreementList = result.ToList<AgreementMain_Attachments>();
                foreach (var item in AgreementList)
                {
                    //Loop and Move Files
                    if (item.Filepath != null && item.Filepath != "")
                    {
                        if (File.Exists(ScannerRepository + _sourcePath + item.Filepath))
                        {
                            if (MoveFile(item.Filepath, ScannerRepository + _sourcePath, ScannerRepository + _TargetUploadPath + item.AgreementCode.ToString() + "/"))
                            {
                                successCount++;
                                lblerror.Text += item.Filepath;
                                lblerror.Text += "<br/>";
                            }
                            else
                            { FailCount++; }
                        }

                    }

                }
                lblerror.Text += "[" + successCount.ToString() + "] Moved  [" + FailCount.ToString() + "] Failed";
                if (errorList.Count > 0)
                {
                    lblerror.Text += "<hr/>";

                    foreach (var item in errorList)
                    {
                        lblerror.Text += "<br/>" + item.ToString();
                    }

                }

            }


            errorList.Clear();
            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.Agreement_procedureHistory
                     orderby obj.Code

                     select obj);

                var AgreementList = result.ToList<Agreement_procedureHistory>();
                foreach (var item in AgreementList)
                {
                    //Loop and Move Files
                    if (item.CMGSDessionFile != null && item.CMGSDessionFile != "")
                    {
                        if (File.Exists(ScannerRepository + _sourcePath + item.CMGSDessionFile))
                        {
                            if (MoveFile(item.CMGSDessionFile, ScannerRepository + _sourcePath, ScannerRepository + _TargetUploadPath + item.AgreementCode.ToString() + "/" + item.Code.ToString() + "/"))
                            {
                                successCount++;
                                lblerror.Text += item.CMGSDessionFile;
                                lblerror.Text += "<br/>";
                            }
                            else
                            { FailCount++; }
                        }
                    }

                    if (item.publishDessionFile != null && item.publishDessionFile != "")
                    {
                        if (File.Exists(ScannerRepository + _sourcePath +  item.publishDessionFile))
                        {
                            if (MoveFile(item.publishDessionFile, ScannerRepository + _sourcePath, ScannerRepository + _TargetUploadPath + item.AgreementCode.ToString() + "/" + item.Code.ToString() + "/"))
                            {
                                successCount++;
                                lblerror.Text += item.publishDessionFile;
                                lblerror.Text += "<br/>";
                            }
                            else
                            { FailCount++; }
                        }
                    }

                }
                lblerror.Text += "[" + successCount.ToString() + "] Moved  [" + FailCount.ToString() + "] Failed";
                if (errorList.Count > 0)
                {
                    lblerror.Text += "<hr/>";

                    foreach (var item in errorList)
                    {
                        lblerror.Text += "<br/>" + item.ToString();
                    }

                }

            }
            errorList.Clear();
        }

        protected void btnMedal2_Click(object sender, EventArgs e)
        {


            int successCount = 0;
            int FailCount = 0;
            string _sourcePath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "MedalAttachments/0/";
            string _TargetUploadPath = System.Configuration.ConfigurationManager.AppSettings["legalRepository"].ToString() + "MedalAttachments/";

            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.Medal_Attachments
                     orderby obj.Code

                     select obj);

                var AgreementList = result.ToList<Medal_Attachments>();
                foreach (var item in AgreementList)
                {
                    //Loop and Move Files
                    if (item.Filepath != null && item.Filepath != "")
                    {
                        if (File.Exists(ScannerRepository + _sourcePath +  item.Filepath))
                        {
                            string folderpath = item.ProcedureID != null && item.ProcedureID != 0 ? item.MedalMasterCode.ToString() + "/" + item.ProcedureID.ToString() + "/" : item.MedalMasterCode.ToString() + "/";
                            if (MoveFile(item.Filepath, ScannerRepository + _sourcePath, ScannerRepository + _TargetUploadPath + folderpath))
                            {
                                successCount++;
                            }
                            else
                            { FailCount++; }
                        }

                    }

                }
                lblerror.Text = "[" + successCount.ToString() + "] Moved  [" + FailCount.ToString() + "] Failed";
                if (errorList.Count > 0)
                {
                    lblerror.Text += "<hr/>";

                    foreach (var item in errorList)
                    {
                        lblerror.Text += "<br/>" + item.ToString();
                    }

                }

            }

            errorList.Clear();

            using (var DC = new CMGS_DBEntities())
            {
                var result =
                    (from obj in DC.Medalmain_Attachments
                     orderby obj.Code

                     select obj);

                var objList = result.ToList<Medalmain_Attachments>();
                foreach (var item in objList)
                {
                    //Loop and Move Files
                    if (item.Filepath != null && item.Filepath != "")
                    {
                        if (File.Exists(ScannerRepository + _sourcePath + item.Filepath))
                        {
                            string folderpath = item.MedalMasterCode.ToString() + "/";
                            if (MoveFile(item.Filepath, ScannerRepository + _sourcePath, ScannerRepository + _TargetUploadPath + folderpath))
                            {
                                successCount++;
                            }
                            else
                            { FailCount++; }
                        }

                    }

                }
                lblerror.Text += "[" + successCount.ToString() + "] Moved  [" + FailCount.ToString() + "] Failed";
                if (errorList.Count > 0)
                {
                    lblerror.Text += "<hr/>";

                    foreach (var item in errorList)
                    {
                        lblerror.Text += "<br/>" + item.ToString();
                    }

                }





            }
            errorList.Clear();
        }
    }
}