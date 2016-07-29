using System;
using System.IO;
using Akka.Actor;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ToDoList.Actors
{
    /// <summary>
    /// JournalActor - Basisklasse für JournalWriter und JournalReader
    /// </summary>
    /// <description>
    /// stellt ein Verzeichnis bereit und Methoden zur Erzeugung von Dateinamen
    /// sowie zur Persistierung und dem Einlesen von Objekten als Dateien in
    /// dem bereit gestellten Verzeichnis.
    /// 
    /// Datei-Namen:
    /// yyyymmdd_hhmmss-Full.Type.Name-persistenceId.json
    /// 
    /// Use-Cases:
    ///  - Save(Typ)
    ///  - Load(filename)
    ///  - list all filenames (type/s)
    /// 
    /// </description>
    public class JournalActor : ReceiveActor
	{
        protected string StorageDir { get; set; }
        private readonly JsonSerializerSettings jsonSettings;

        public JournalActor()
        {
            // StorageDir = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();

            // geht nur aus der IDE heraus -- ist also eigentlich Schrott...
            StorageDir = AppDomain.CurrentDomain.BaseDirectory;


            if (!Directory.Exists(StorageDir))
                Directory.CreateDirectory(StorageDir);

            jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
            };
        }

        // constructs a new filename for serializing an object of type T with persistenceId 
        protected string FilePath(Type type, string persistenceId)
        {
            return Path.Combine(
                StorageDir,
                String.Format("{0}-{1}-{2}.json",
                    DateTime.Now.ToString("yyyyMMdd_HHmmss"), type.FullName, persistenceId
                )
            );
        }

        protected IEnumerable<string> Files(string glob)
        {
            return Directory.EnumerateFiles(StorageDir, glob)
                .OrderBy(f => f);
        }

        protected void Save(object message, Type type, string persistenceId)
        {
            Context.System.Log.Debug("Actor {0}: Save to file: {1}", Self.Path.Name, Path.GetFileName(FilePath(type, persistenceId)));

            var json = JsonConvert.SerializeObject(message, type, jsonSettings);

            File.WriteAllText(FilePath(type, persistenceId), json);
        }

        protected object Load(string filePath)
        {
            Context.System.Log.Debug("Actor {0}: Load from: {1}", Self.Path.Name, Path.GetFileName(filePath));

            var json = File.ReadAllText(filePath);
			var typeName = Path
				.GetFileNameWithoutExtension(filePath)
				.Split(new [] { '-' })
				.Skip(1)
				.First();
            var type = Type.GetType(typeName);
			var instance = JsonConvert.DeserializeObject(json, type, jsonSettings);
			return instance;
        }
	}
}
