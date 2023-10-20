using Newtonsoft.Json;

//Вариант 7: Транспортные средства

namespace AnimalGuesser
{
    public class Question
    {
        public Question PositiveResponse { get; set; }
        public Question NegativeResponse { get; set; }
        public string QuestionText { get; set; }
        public string Result { get; set; }

        [JsonConstructor]
        public Question(Question positiv, Question negative, string questionText, string result)
        {
            QuestionText = questionText;
            Result = result ?? "";
            PositiveResponse = positiv;
            NegativeResponse = negative;
        }

        public Question(Question clone)
        {
            PositiveResponse = clone.PositiveResponse;
            NegativeResponse = clone.NegativeResponse;
            QuestionText = clone.QuestionText;
            Result = clone.Result;
        }

        public void ExecuteQuestion()
        {
            Console.WriteLine(QuestionText);
            Console.WriteLine("1 - да\n0 - нет");
            if (Console.ReadLine() == "1")
            {
                if (PositiveResponse == null)
                    Console.WriteLine("Вы загадали " + Result);
                else
                    PositiveResponse.ExecuteQuestion();
            }
            else if(NegativeResponse == null)
                ExtendTree();
            else
                NegativeResponse.ExecuteQuestion();
        }

        public void ExtendTree()
        {
            Console.WriteLine("Сдаюсь. Что было загадано?");
            string guesedObject = Console.ReadLine();

            Console.WriteLine("Что отличает загаданый оъект?");
            string objectCharacteristic = Console.ReadLine() ?? "";

            NegativeResponse = new Question(this);
            PositiveResponse = new Question(null, null, "Это " + guesedObject + "?", guesedObject);
            QuestionText = objectCharacteristic;
            Result = "";
        }
    }

    static class Program
    {
        public static string fileName = "QuestionBase.json";
        public static string deflt = "{\"PositiveResponse\":null,\"NegativeResponse\":null,\"QuestionText\":\"Это самолёт\",\"Result\":\"Самолёт\"}";
        public static void Save(Question toSave)
        {
            string jsonString = JsonConvert.SerializeObject(toSave);
            File.WriteAllText(fileName, jsonString);
        }

        public static void Drop()
        {
            File.WriteAllText(fileName, deflt);
        }

        public static Question Read()
        {
            Question quest = null;
            try
            {
                quest = JsonConvert.DeserializeObject<Question>(File.ReadAllText(fileName));
            }
            catch (Exception ex)
            {
                quest = new Question(null, null, "Это самолёт", "Самолёт");
            }
            return quest;
        }

        public static void PrintTree(Question root, int depth)
        {
            if (root == null)
                return;

            depth += 5;

            PrintTree(root.NegativeResponse, depth);

            var outLine = new string(' ', depth) + root.QuestionText;
            Console.WriteLine(root.Result.Length > 0 ? outLine + " -> " +root.Result : outLine);

            PrintTree(root.PositiveResponse, depth);
        }

        static void Main()
        {
            string key = "";
            while(key != "3")
            {
                var currentNode = Read();
                Console.WriteLine("1 - Выполнить\n2 - Вывести дерево\n3 - Выход");
                key = Console.ReadLine();
                switch(key) {
                    case "1":
                        currentNode.ExecuteQuestion();
                        Save(currentNode);
                        continue;
                    case "2":
                        PrintTree(currentNode,0);
                        Save(currentNode);
                        continue;
                }
            }
        }
    }
}