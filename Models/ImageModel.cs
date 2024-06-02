namespace pots.Models
{
    public class ImageModel
    {
        public string SaveImage(IFormFile image)
        {
            var fileName = Path.GetFileName(image.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (System.IO.File.Exists(filePath))
            {
                var uniqueIdentifier = DateTime.Now.Ticks;
                fileName = Path.GetFileNameWithoutExtension(fileName) + "_" + uniqueIdentifier + Path.GetExtension(fileName);
                filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }

            return fileName;
        }

        public void DeleteImage(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}