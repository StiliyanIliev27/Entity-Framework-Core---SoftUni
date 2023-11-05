namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Text;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            string result = string.Empty;

            //02.All Albums Produced by Given Producer
            //result = ExportAlbumsInfo(context, 9);

            //03.Songs Above Given Duration
            //result = ExportSongsAboveDuration(context, 4);//The given duration time must be in seconds.

            Console.WriteLine(result);
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albumsInfo = context.Albums
                .Where(a => a.ProducerId == producerId)
                .AsEnumerable()//We use AsEnumerable() func to handle with System.InvalidOperationException: "LINQ expressiong couldn't be translated." 
                .OrderByDescending(a => a.Price)
                .Select(a => new
                {
                    AlbumName = a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = a.Producer.Name,
                    AlbumSongs = a.Songs.Select(ab => new
                    {
                        SongName = ab.Name,
                        SongPrice = ab.Price.ToString("f2"),
                        SongWriterName = ab.Writer.Name
                    }).OrderByDescending(s => s.SongName).ThenBy(s => s.SongWriterName).ToArray(),
                    AlbumPrice = a.Price.ToString("f2")
                }).ToArray();

            StringBuilder sb = new StringBuilder();

            foreach(var album in albumsInfo)
            {
                sb
                    .AppendLine($"-AlbumName: {album.AlbumName}")
                    .AppendLine($"-ReleaseDate: {album.ReleaseDate}")
                    .AppendLine($"-ProducerName: {album.ProducerName}")
                    .AppendLine("-Songs:");

                int songNumber = 1;
                foreach(var song in album.AlbumSongs)
                {
                    sb
                        .AppendLine($"---#{songNumber}")
                        .AppendLine($"---SongName: {song.SongName}")
                        .AppendLine($"---Price: {song.SongPrice}")
                        .AppendLine($"---Writer: {song.SongWriterName}");
                    songNumber++;
                }
                sb.AppendLine($"-AlbumPrice: {album.AlbumPrice}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songsInfo = context.Songs
                .AsEnumerable()//We use AsEnumerable() func to handle with System.InvalidOperationException: "LINQ expressiong couldn't be translated." 
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new
                {
                    SongName = s.Name,
                    Performers = s.SongPerformers
                        .Select(sp => $"{sp.Performer.FirstName} {sp.Performer.LastName}")
                        .OrderBy(sp => sp).ToArray(),
                    WriterName = s.Writer.Name,
                    AlbumProducer = s.Album.Producer.Name,
                    Duration = s.Duration.ToString("c")
                }).OrderBy(s => s.SongName).ThenBy(s => s.WriterName).ToList();

            StringBuilder sb = new StringBuilder();

            int songNumber = 1;
            foreach(var song in songsInfo)
            {
                sb
                    .AppendLine($"-Song #{songNumber}")
                    .AppendLine($"---SongName: {song.SongName}")
                    .AppendLine($"---Writer: {song.WriterName}");

                foreach(var performer in song.Performers)
                {
                    sb.AppendLine($"---Performer: {performer}");
                }

                sb
                    .AppendLine($"---AlbumProducer: {song.AlbumProducer}")
                    .AppendLine($"---Duration: {song.Duration}");

                songNumber++;
            }

            return sb.ToString().TrimEnd();
        }
    }
}
