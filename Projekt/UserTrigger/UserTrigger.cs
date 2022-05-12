using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Projekt.Models;

namespace UserTrigger
{
    public static class UserTrigger
    {
        [FunctionName("UserTrigger")]
        public static void Run([CosmosDBTrigger(
            databaseName: "Users",
            collectionName: "Users",
            ConnectionStringSetting = "AzureWebJobsStorage",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input,
            ILogger log, [Blob("pictures/{sys.randguid}", FileAccess.Write, Connection = "StorageConnectionString")] Stream image)
        {
            var user = Newtonsoft.Json.JsonConvert.DeserializeObject<Person>(input[0].ToString());

            Bitmap img = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(img);

            // The font for our text
            Font f = new Font("Arial", 14);

            // work out how big the text will be when drawn as an image
            SizeF size = g.MeasureString(user.PictureLink, f);

            // create a new Bitmap of the required size
            img = new Bitmap((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height));
            g = Graphics.FromImage(img);

            // give it a white background
            g.Clear(Color.White);

            // draw the text in black
            g.DrawString(user.PictureLink, f, Brushes.Black, 0, 0);

            img.Save(image, ImageFormat.Jpeg);
        }
    }
}
