using Newtonsoft.Json;


namespace blockchain_prototype
{
    public class DataFile
    {
        public void Save(Block newBlock)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(newBlock);
            string filePath = CreateNewFile(newBlock.Index);

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(json);
            }
        }

        public List<Block> GetChain()
        {
            string rootDirectory = @"C:\Users\olavi\uniblock\blockchain_prototype\Blockchain_prototype\blockchain_prototype\blockchain_prototype\bin\Debug\net6.0\Transactions";

            List<Block> allObjects = new List<Block>();

            string[] jsonFiles = Directory.GetFiles(rootDirectory);

            foreach (string jsonFile in jsonFiles)
            {
                var objectFromFile = DeserializeObjectsFromFile<Block>(jsonFile);
                allObjects.Add(objectFromFile);
            }

            return allObjects;
        }

        private string CreateNewFile(int id)
        {
            string pathName = @$"C:\Users\olavi\uniblock\blockchain_prototype\Blockchain_prototype\blockchain_prototype\blockchain_prototype\bin\Debug\net6.0\Transactions\{id}.json";

            using (FileStream fs = File.Create(pathName))
            {

            }

            if (File.Exists(pathName))
            {
                Console.WriteLine("File is created.");
            }

            else
            {
                Console.WriteLine("File is not created.");
            }

            return pathName;
        }

        static Block DeserializeObjectsFromFile<T>(string filePath)
        {
            Block? block = null;

            try
            {
                string jsonData = File.ReadAllText(filePath);
                block = System.Text.Json.JsonSerializer.Deserialize<Block>(jsonData);
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении файла {filePath}: {ex.Message}");
            }

            return block;
        }

        public string SerializeToJsonObject(Block block)
        {
            string json = JsonConvert.SerializeObject(block);
            return json;
        }
    }
}