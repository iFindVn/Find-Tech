using KendoEditorImageBrowser.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestImageCrop;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Net;

namespace FindTech.Web.Areas.BO.Controllers
{
    public class ImageBOController : Controller
    {
        private const string contentFolderRoot = "~/Areas/BO/Upload/Admin";
        private const string prettyName = "Images/";
        private const string DefaultFilter = "*.png,*.gif,*.jpg,*.jpeg";

        private const int ThumbnailHeight = 80;
        private const int ThumbnailWidth = 80;

        private readonly DirectoryBrowser directoryBrowser;
        private readonly ThumbnailCreator thumbnailCreator;
        private readonly Cloudinary cloudinary;
        public ImageBOController()
        {
            directoryBrowser = new DirectoryBrowser();
            thumbnailCreator = new ThumbnailCreator();

            Account account = new Account(
              "ifind-vn",
              "527894251963919",
              "JxLoCiFSj4Rh5Wvfyek9JzUPetg");

            cloudinary = new Cloudinary(account);


        }

        public string ContentPath
        {
            get
            {
                return Path.Combine(contentFolderRoot, prettyName);
            }
        }

        private string ToAbsolute(string virtualPath)
        {
            return VirtualPathUtility.ToAbsolute(virtualPath);
        }

        private string CombinePaths(string basePath, string relativePath)
        {
            return VirtualPathUtility.Combine(VirtualPathUtility.AppendTrailingSlash(basePath), relativePath);
        }

        public virtual bool AuthorizeRead(string path)
        {
            return CanAccess(path);
        }

        protected virtual bool CanAccess(string path)
        {
            return path.StartsWith(ToAbsolute(ContentPath), StringComparison.OrdinalIgnoreCase);
        }

        private string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return ToAbsolute(ContentPath);
            }

            return CombinePaths(ToAbsolute(ContentPath), path);
        }

        public virtual JsonResult Read(string path)
        {
            var listResourceParams = new ListResourcesParams()
            {

                NextCursor = null,
                Tags = true,
                Context = true,
                Moderations = true,
                Type = "upload",
                MaxResults = 100
            };
            var listResource = cloudinary.ListResources(listResourceParams).Resources;
            //var listResource = cloudinary.ListResourcesByTag("user_upload").Resources;
            

            var result = listResource.Select(f => new
            {
                name = f.PublicId,
                type = "f",
                size = f.Length
            });

            return Json(result, JsonRequestBehavior.AllowGet);



            //path = NormalizePath(path);

            //if (AuthorizeRead(path))
            //{
            //    try
            //    {
            //        directoryBrowser.Server = Server;

            //        var result = directoryBrowser
            //            .GetContent(path, DefaultFilter)
            //            .Select(f => new
            //            {
            //                name = f.Name,
            //                type = f.Type == EntryType.File ? "f" : "d",
            //                size = f.Size
            //            });

            //        return Json(result, JsonRequestBehavior.AllowGet);
            //    }
            //    catch (DirectoryNotFoundException)
            //    {
            //        throw new HttpException(404, "File Not Found");
            //    }
            //}

            //throw new HttpException(403, "Forbidden");
        }


        public virtual bool AuthorizeThumbnail(string path)
        {
            return CanAccess(path);
        }

        [OutputCache(Duration = 3600, VaryByParam = "path")]
        public virtual ActionResult Thumbnail(string path)
        {
            //var resource = cloudinary.GetResource(path);

            string url = cloudinary.Api.UrlImgUp.Transform(new Transformation().Width(80).Height(80).Crop("pad")).BuildUrl(path);
            if (url != null)
            {
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                // Sends the HttpWebRequest and waits for the response.			
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                // Gets the stream associated with the response.
                Stream receiveStream = myHttpWebResponse.GetResponseStream();
                return new FileStreamResult(receiveStream, "image/jpg");
            }
            return Json(false);
            //return new File("http://res.cloudinary.com/ifind-vn/image/upload/c_pad,h_80,w_80/v1425529303/el05mkztm6ztgrwl0mne.jpg");
            //path = NormalizePath(path);

            //if (AuthorizeThumbnail(path))
            //{
            //    var physicalPath = Server.MapPath(path);

            //    if (System.IO.File.Exists(physicalPath))
            //    {
            //        Response.AddFileDependency(physicalPath);

            //        return CreateThumbnail(physicalPath);
            //    }
            //    else
            //    {
            //        return Json(false);
            //    }
            //}
            //else
            //{
            //    return Json(false);
            //}
        }

        private FileContentResult CreateThumbnail(string physicalPath)
        {
            using (var fileStream = System.IO.File.OpenRead(physicalPath))
            {
                var desiredSize = new ImageSize
                {
                    Width = ThumbnailWidth,
                    Height = ThumbnailHeight
                };

                const string contentType = "image/png";

                return File(thumbnailCreator.Create(fileStream, desiredSize, contentType), contentType);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Destroy(string path, string name, string type)
        {
            path = NormalizePath(path);

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(type))
            {
                path = CombinePaths(path, name);
                if (type.ToLowerInvariant() == "f")
                {
                    DeleteFile(path);
                }
                else
                {
                    DeleteDirectory(path);
                }

                return Json(null);
            }
            throw new HttpException(404, "File Not Found");
        }

        public virtual bool AuthorizeDeleteFile(string path)
        {
            return CanAccess(path);
        }

        public virtual bool AuthorizeDeleteDirectory(string path)
        {
            return CanAccess(path);
        }

        protected virtual void DeleteFile(string path)
        {
            if (!AuthorizeDeleteFile(path))
            {
                throw new HttpException(403, "Forbidden");
            }

            var physicalPath = Server.MapPath(path);

            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
            }
        }

        protected virtual void DeleteDirectory(string path)
        {
            if (!AuthorizeDeleteDirectory(path))
            {
                throw new HttpException(403, "Forbidden");
            }

            var physicalPath = Server.MapPath(path);

            if (Directory.Exists(physicalPath))
            {
                Directory.Delete(physicalPath, true);
            }
        }

        public virtual bool AuthorizeCreateDirectory(string path, string name)
        {
            return CanAccess(path);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Create(string path, FileBrowserEntry entry)
        {
            var imageUploadParams = new ImageUploadParams()
            {
                Folder = "testFolders"
            };
            var result = cloudinary.Upload(imageUploadParams);

            return Json(null);
            //path = NormalizePath(path);
            //var name = entry.Name;

            //if (!string.IsNullOrEmpty(name) && AuthorizeCreateDirectory(path, name))
            //{
            //    var physicalPath = Path.Combine(Server.MapPath(path), name);

            //    if (!Directory.Exists(physicalPath))
            //    {
            //        Directory.CreateDirectory(physicalPath);
            //    }

            //    return Json(null);
            //}
            //return Json(false);
        }


        public virtual bool AuthorizeUpload(string path, HttpPostedFileBase file)
        {
            return CanAccess(path) && IsValidFile(file.FileName);
        }

        private bool IsValidFile(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var allowedExtensions = DefaultFilter.Split(',');

            return allowedExtensions.Any(e => e.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Upload(string path, HttpPostedFileBase file)
        {
            path = NormalizePath(path);
            var fileName = Path.GetFileName(file.FileName);

            if (AuthorizeUpload(path, file))
            {
                var imageUploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, file.InputStream),
                    PublicId = Path.GetFileNameWithoutExtension(file.FileName),
                    Tags = "user_upload"
                };
                var result = cloudinary.Upload(imageUploadParams);

                //cloudinary.Upload(new ImageUploadParams { File = new FileDescription (file.FileName, file.InputStream) });
                //file.SaveAs(Path.Combine(Server.MapPath(path), fileName));

                return Json(new
                {
                    size = file.ContentLength,
                    name = fileName,
                    type = "f"
                }, "text/plain");
            }

            return Json(false);
        }

        [OutputCache(Duration = 360, VaryByParam = "path")]
        public ActionResult Image(string path)
        {
            var url = cloudinary.Api.Url.BuildUrl(path);
            //var resource = cloudinary.GetResource(path);
            //string url = resource.Url;

            //string url = cloudinary.Api.UrlImgUp.Transform(new Transformation().Width(80).Height(80).Crop("pad")).BuildUrl(path);
            if (url != null)
            {
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                // Sends the HttpWebRequest and waits for the response.			
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                // Gets the stream associated with the response.
                Stream receiveStream = myHttpWebResponse.GetResponseStream();
                return new FileStreamResult(receiveStream, "image/jpg");
            }
            return Json(false);


            //path = NormalizePath(path);

            //if (AuthorizeImage(path))
            //{
            //    var physicalPath = Server.MapPath(path);

            //    if (System.IO.File.Exists(physicalPath))
            //    {
            //        const string contentType = "image/png";
            //        return File(System.IO.File.OpenRead(physicalPath), contentType);
            //    }
            //}
            //return Json(false);
        }

        public virtual bool AuthorizeImage(string path)
        {
            return CanAccess(path) && IsValidFile(Path.GetExtension(path));
        }

        public ActionResult CropImage1(string imagePath)
        {
            string urls = cloudinary.Api.UrlImgUp.Transform(new Transformation().Width(270).Height(270).Crop("fill"))
                .BuildUrl(imagePath);
            string urlr = cloudinary.Api.UrlImgUp.Transform(new Transformation().Width(150).Height(100).Crop("fill"))
                .BuildUrl(imagePath);
            string avartar = cloudinary.Api.UrlImgUp.Transform(new Transformation().Width(270).Crop("fill"))
                .BuildUrl(imagePath);
            string banner = cloudinary.Api.UrlImgUp.Transform(new Transformation().Width(900).Height(450).Crop("fill"))
                .BuildUrl(imagePath);
            Object obj = new
            {
                displayAvatar = urls,
                avatar = avartar,
                squareAvatar = urls.Replace("c_fill,w_270", "c_fill,w_{width}"),
                rectangleAvatar = urlr.Replace("c_fill,w_150", "c_fill,w_{width}"),
                bannerAvatar = banner.Replace("c_fill,w_270", "c_fill,w_{width}")
            };
            return Json(obj);
        }

        public ActionResult CropImage(string imagePath, float scales, int ws, int hs, int xs, int ys, float scaler, int wr, int hr, int xr, int yr, float scalea, float wa, float ha, int xa, int ya, float scaleb, float wb, float hb, int xb, int yb)
        {
            ws = (int)Math.Round((ws / scales), 0);
            hs = (int)Math.Round((hs / scales), 0);
            xs = (int)Math.Round((xs / scales), 0);
            ys = (int)Math.Round((ys / scales), 0);

            wr = (int)Math.Round((wr / scaler), 0);
            hr = (int)Math.Round((hr / scaler), 0);
            xr = (int)Math.Round((xr / scaler), 0);
            yr = (int)Math.Round((yr / scaler), 0);


            wa = (int)Math.Round((wa / scalea));
            ha = (int)Math.Round((ha / scalea));
            xa = (int)Math.Round((xa / scalea));
            ya = (int)Math.Round((ya / scalea));

            wb = (int)Math.Round((wb / scaleb));
            hb = (int)Math.Round((hb / scaleb));
            xb = (int)Math.Round((xb / scaleb));
            yb = (int)Math.Round((yb / scaleb));

            string urls = cloudinary.Api.UrlImgUp.Transform(new Transformation().Width(ws).Height(hs).Crop("crop").X(xs).Y(ys)
                .Chain().Width(270).Crop("fill"))
                .BuildUrl(imagePath);
            string urlr = cloudinary.Api.UrlImgUp.Transform(new Transformation().Width(wr).Height(hr).Crop("crop").X(xr).Y(yr)
                .Chain().Width(150).Crop("fill"))
                .BuildUrl(imagePath);
            string avartar = cloudinary.Api.UrlImgUp.Transform(new Transformation().Width((int)wa).Height((int)ha).Crop("crop").X(xa).Y(ya)
                .Chain().Width(270).Crop("fill"))
                .BuildUrl(imagePath);
            string banner = cloudinary.Api.UrlImgUp.Transform(new Transformation().Width((int)wa).Height((int)ha).Crop("crop").X(xa).Y(ya)
                .Chain().Width(900).Crop("fill"))
                .BuildUrl(imagePath);
            Object obj = new
            {
                displayAvatar = urls,
                avatar = avartar,
                squareAvatar = urls.Replace("c_fill,w_270", "c_fill,w_{width}"),
                rectangleAvatar = urlr.Replace("c_fill,w_150", "c_fill,w_{width}"),
                bannerAvatar = banner.Replace("c_fill,w_270", "c_fill,w_{width}")
            };
            return Json(obj);

            //crop(imagePath, scales, ws, hs, xs, ys);
            //crop(imagePath, scaler, wr, hr, xr, yr);
            //string resizedPath = "";
            //if(Path.GetDirectoryName(imagePath).ToString() != "")
            //{
            //    resizedPath = Path.GetDirectoryName(imagePath) + @"\272x272\" + Path.GetFileName(imagePath);
            //}
            //else
            //{
            //    resizedPath = Path.GetDirectoryName(imagePath) + @"272x272\" + Path.GetFileName(imagePath);
            //}

            //resizedPath = NormalizePath(resizedPath);
            ////resizedPath = Path.Combine(resizedPath, Path.GetFileName(imagePath));
            //return Json(resizedPath);
        }

        private void crop(string imagePath, float scale, int w, int h, int x, int y)
        {
            string imgPath = NormalizePath(imagePath);
            var physicalPath = Server.MapPath(imgPath);
            Rectangle cropRect = new Rectangle((int)(x / scale), (int)(y / scale), (int)(w / scale), (int)(h / scale));
            Bitmap source = System.Drawing.Image.FromFile(physicalPath) as Bitmap;
            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);
            string cropPath = Path.GetDirectoryName(physicalPath) + @"\croped";

            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(source, new Rectangle(0, 0, cropRect.Width, cropRect.Height), cropRect, GraphicsUnit.Pixel);
                MemoryStream ms = new MemoryStream();
                //if (!Directory.Exists(cropPath))
                //{
                //    Directory.CreateDirectory(cropPath);
                //}
                //FileStream fs = new FileStream(cropPath + @"\" + Path.GetFileName(physicalPath), FileMode.Create, FileAccess.ReadWrite);
                target.Save(ms, ImageFormat.Jpeg);
                if (w == h)
                {
                    resizeImage(ms, 272, 272, physicalPath);
                    resizeImage(ms, 72, 72, physicalPath);
                }
                else
                {
                    resizeImage(ms, 150, 100, physicalPath);
                }
                //byte[] matriz = ms.ToArray();
                //fs.Write(matriz, 0, matriz.Length);
                //ms.Close();
                //fs.Close();
            }
        }

        private void resizeImage(MemoryStream image, int width, int heigth, string path)
        {
            string savePath = Path.GetDirectoryName(path) + @"\" + width.ToString() + "x" + heigth.ToString();
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            using (Image fromStream = System.Drawing.Image.FromStream(image))
            {
                // calculate height based on the width parameter

                using (Bitmap resizedImg = new Bitmap(fromStream, width, heigth))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        resizedImg.Save(ms, fromStream.RawFormat);
                        FileStream fs = new FileStream(savePath + @"\" + Path.GetFileName(path), FileMode.Create, FileAccess.ReadWrite);
                        byte[] matriz = ms.ToArray();
                        fs.Write(matriz, 0, matriz.Length);
                        ms.Close();
                        fs.Close();

                    }
                }
            }
        }
    }
}

