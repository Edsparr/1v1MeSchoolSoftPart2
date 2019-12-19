using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using SchoolClassTester.SchoolClasses;

namespace SchoolClassTester.Services
{
    public interface IFileService
    {
        void SaveObject(string relativePath, object @object);
        T LoadObject<T>(string relativePath) where T : new();

        SchoolClass LoadClass(string prefix, string suffix);
        void SaveClass(SchoolClass schoolClass);
        bool DoesClassExist(string prefix, string suffix);
    }
    public partial class FileService : IFileService
    {
        private const string PathPrefab = "SchoolManager/{0}";
        public void SaveObject(string relativePath, object @object)
        {
            var path = GetPath(relativePath);

            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(JsonConvert.SerializeObject(@object));
            }
        }
        public T LoadObject<T>(string relativePath) where T : new()
        {
            var path = GetPath(relativePath);
            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (StreamReader reader = new StreamReader(path))
            {
                return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
            }
        }

        private string GetPath(string relativePath) => 
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), string.Format(PathPrefab, relativePath));
    } //Base implemenmtation


    public partial class FileService : IFileService //More of helper methods, still implementation with the one above, just seperated for looks.
    {
        private const string SchoolClassPrefab = "Classes/{0}-{1}.json";
        public SchoolClass LoadClass(string prefix, string suffix)
        {
             return LoadObject<SchoolClass>(string.Format(SchoolClassPrefab, prefix, suffix));
        }
        public bool DoesClassExist(string prefix, string suffix)
        {
            return File.Exists(string.Format(SchoolClassPrefab, prefix, suffix));
        }

        public void SaveClass(SchoolClass schoolClass)
        {
            SaveObject(string.Format(SchoolClassPrefab, schoolClass.ClassPrefix, schoolClass.ClassSuffix), schoolClass);
        }
    }
}
