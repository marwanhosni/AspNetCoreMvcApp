namespace MvcWithRootAcademy.Helper;

public class DoucmentSettings
{
    public static string UploadFile(IFormFile file, String folderName)
    {
        // 1. get Location Folder Path بنمسك مكان الي هيحتفظ فيه الصوره  الي هو اسم الفولدر 

        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName);

        // 2. Get File Name Make It Uniqu بندي للصورة اسم منفرد ينيك

        var fileName = $"{Guid.NewGuid()}{file.FileName}";


        // 3. Get File Path => FolderPath + FileName علشان ننادي علي الصورة يبقي اسم الفولدر / اسم الصورة 

        var filePath = Path.Combine(folderPath, fileName);

        // 4. Save File As Stream : Data Per Time

        using var fileStream = new FileStream(filePath, FileMode.Create);

        file.CopyTo(fileStream);

        return fileName;

    }

    public static void Delete(string fileName, string folderName)
    {
        var filepath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName, fileName);
        if (File.Exists(filepath))
            File.Delete(filepath);
    }
}