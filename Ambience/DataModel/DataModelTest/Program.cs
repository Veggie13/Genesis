using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Genesis.Ambience.DataModel;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace DataModelTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Project proj = new Project();

            FolderLibraryElement folder1 = new FolderLibraryElement() { Path = @"E:\Media" };
            FolderLibraryElement folder2 = new FolderLibraryElement() { Path = @"E:\Veggie" };

            PeriodicEventElement period = new PeriodicEventElement() { Name = "Periodic1", BaseColor = Color.Red };
            DelayEventElement delay = new DelayEventElement() { Name = "Delay1", BaseColor = Color.Aqua, Subordinate = period };
            RandomEventElement rand = new RandomEventElement() { Name = "Random1", BaseColor = Color.Blue, Selection = new SubordinateElement[] { period, delay }.ToList() };
            SoundEventElement snd = new SoundEventElement() { Name = "Sound1", Resource = "Media::sttng.wav" };

            proj.Libraries = new ALibraryElement[] { folder1, folder2 }.ToList();
            proj.Events = new AEventElement[] { delay, period, rand, snd }.ToList();

            MemoryStream stream = new MemoryStream();
            using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.ASCII) { Formatting = Formatting.Indented })
            {
                XmlSerializer serializer = Project.GetSerializer();
                serializer.Serialize(writer, proj);

                stream = writer.BaseStream as MemoryStream;
            }

            string content = (stream == null) ? "<nope />" : Encoding.ASCII.GetString(stream.ToArray());

            Console.WriteLine(content);
            Console.ReadLine();

            MemoryStream instream = new MemoryStream(Encoding.ASCII.GetBytes(content));
            StreamReader reader = new StreamReader(instream);

            XmlSerializer deser = Project.GetSerializer();
            Project newproj = (Project)deser.Deserialize(reader);
            reader.Close();

            Console.ReadLine();
        }
    }
}
