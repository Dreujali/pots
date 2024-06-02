namespace pots.Models
{
    public class ImageModel
    {
        public string SaveImage(IFormFile image)
        {
            var fileName = Path.GetFileName(image.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            // Check if the directory exists
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                // If the directory doesn't exist - create it
                Directory.CreateDirectory(directory);
            }

            // Check if the file exists
            if (System.IO.File.Exists(filePath))
            {
                // If file already exists, append a unique identifier to the filename
                var uniqueIdentifier = DateTime.Now.Ticks;
                fileName = Path.GetFileNameWithoutExtension(fileName) + "_" + uniqueIdentifier + Path.GetExtension(fileName);
                filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }

            return fileName; // Return the saved file name
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