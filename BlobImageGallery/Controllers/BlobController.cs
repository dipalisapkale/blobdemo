using Microsoft.AspNetCore.Mvc;
using BlobImageGallery.Services;

namespace BlobImageGallery.Controllers
{
    public class BlobController : Controller  
    {
        private readonly BlobService _blobService;

        public BlobController(BlobService blobService)
        {
            _blobService = blobService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var images = await _blobService.GetBlobUrlsAsync();
            return View(images);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                await _blobService.UploadFileAsync(file);
            }
            return RedirectToAction("Index");
        }
    }
}
