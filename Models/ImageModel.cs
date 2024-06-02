namespace pots.Models
{
    public class ImageModel
    {
        // Saves the image to the images folder and returns the file name
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
                var uniqueIdentifier = Guid.NewGuid().ToString();
                fileName = Path.GetFileNameWithoutExtension(fileName) + "_" + uniqueIdentifier + Path.GetExtension(fileName);
                filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }

            return fileName;
        }

        // Deletes the image with the given file name from the images folder
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